// *****************
// *               *
// *  PPI Display  *
// *               *
// *****************

using CSharpCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static PDP8;

namespace PDP_8
{
  public partial class PPIForm : Form
  {
    const int MaxRange = 250;   // km = pixel
    const int ImageRadius = MaxRange + 20;
    const int ImageDimension = 2 * ImageRadius + 1;
    const double kmPerNmi = 1.852;

    // Form background image
    Bitmap bmp;

    // For drawing rays
    int azimuth;      // az of ray [0 .. 360)
    int binCounter;   // counts bins [0 .. 100]
    int binRes;       // half-nmi per bin
    int mapIndex;     // index into binMap[azimuth]

    // List of (offset, count) row descriptors in circular PPI region
    // of image
    List<(int, int)> ppiPixels = new List<(int, int)>();
    int ppiRow0;    // y coord of first row

    // Pixels in PPI region of image. Each pixel is 1 kn x 1 km.
    byte[] dbzMap;

    // For each azimuth, a list of dbzMap indices for each half-nmi bin. The values for
    // a bin are a count followed by count indices. Count may be 0. Count -1 terminates
    // the list. All of the polar->cartesian work is done at construction, drawing
    // and erasing is very fast.
    List<int>[] binMap = new List<int>[360];

    // Range ring indices. 5 50 km rings at each azimuth
    List<int>[] rangeRings = new List<int>[360];

    // Screen fade in non-color mode. dbzMap values persist for maxLife update
    // cycles, then fade to black.
    byte[] lifeTime;
    const byte maxLife = 80;

    // Ray format used by RADAR
    enum RayPhase
    {
      Idle,
      Command,
      Azimuth,
      Unknown,
      ICW1,
      Bin,
    }

    RayPhase rayPhase = RayPhase.Idle;
    bool colorMode { get { return colorCheck.Checked; } }

    bool screenChanged_;
    bool screenChanged
    {
      get => screenChanged_;
      set
      {
        screenChanged_ = value;
        if (value)
          CallMeReal("ppi", 1.0e6 / 8, true);
      }
    }

    public void SetElevation(double el)
    {
      elLabel.Text = el.ToString("f1");
    }

    // *****************
    // *               *
    // *  Constructor  *
    // *               *
    // *****************

    public PPIForm()
    {
      InitializeComponent();

      bmp = new Bitmap(ImageDimension, ImageDimension, PixelFormat.Format8bppIndexed);
      setPalette();

      az0Label.BackColor = Color.Beige;
      el0Label.BackColor = Color.Beige;

      RegisterAction("ppi", wakeup);

      setBackground(255);
      initMap();
      erase();

      BackgroundImage = bmp;
    }

    // *****************************
    // *                           *
    // *  Image and Palette Setup  *
    // *                           *
    // *****************************

    void initMap()
    {
      // map is a list, for each azimuth, of (bin number, dbzMap index) pairs.
      List<(int, int)>[] map = new List<(int, int)>[360];
      for (int i = 0; i < 360; ++i)
      {
        map[i] = new List<(int, int)>();
        rangeRings[i] = new List<int>();
      }

      // Every 1 km x 1 km pixel in dbzMap is owned by one 1 degree x 1/2 nmi
      // bin. Find them all.

      int pMap = 0;    // index intp dmzMap
      ppiRow0 = -1;    // first non-empty row

      for (int j = 0; j < ImageDimension; ++j)
      {
        int rowOffset = -1;
        int rowCount = 0;

        for (int i = 0; i < ImageDimension; ++i)
        {
          double x = i - ImageRadius;
          double y = j - ImageRadius;
          double range = MathUtil.EuclidDist(x, y);

          if (range <= MaxRange)
          {
            int angle = MathUtil.mod((int)Math.Round(MathUtil.Degrees(Math.Atan2(y, x))), 360);
            int bin = (int)Math.Round(range / kmPerNmi * 2.0);
            map[angle].Add((bin, pMap));

            if ((int)Math.Round(range) % 50 == 0 && range < MaxRange)
              rangeRings[angle].Add(pMap);

            if (rowOffset < 0)
              rowOffset = i;
            ++rowCount;

            if (ppiRow0 < 0)
              ppiRow0 = j;

            ++pMap;
          }
        }

        if (rowCount > 0)
          ppiPixels.Add((rowOffset, rowCount));
      }

      dbzMap = new byte[pMap];
      lifeTime = new byte[pMap];

      // Sort each list by increasing bin number. Ties are sorted by dbzMap index, which
      // doesn't really matter but may slightly improve data cache performance.
      for (int i = 0; i < 360; ++i)
        map[i].Sort();

      // Generate binMap.
      for (int i = 0; i < 360; ++i)
      {
        binMap[i] = new List<int>();

        int bin = 0;
        int p = 0;

        while (p < map[i].Count)
        {
          int q;
          for (q = p; q < map[i].Count && map[i][q].Item1 == bin; ++q) ;

          binMap[i].Add(q - p);
          for (; p < q; ++p)
            binMap[i].Add(map[i][p].Item2);

          ++bin;
        }

        binMap[i].Add(-1);
      }
    }

    void setPalette()
    {
      Color[] colors = new Color[]
      {
        Color.Black,
        Color.Violet,
        Color.Cyan,
        Color.Green,
        Color.Yellow,
        Color.Orange,
        Color.Red
      };

      ColorPalette pal = bmp.Palette;
      if (colorMode)
      {
        for (int i = 0; i < 128; ++i)
          pal.Entries[i] = colors[Math.Min(i / 10, colors.Length - 1)];
        for (int z = 1; z < 255; z += 2)
          pal.Entries[z / 2 + 128] = Color.FromArgb(z, z, z);
      }
      else
        for (int i = 0; i < 255; i++)
        {
          int z = Math.Min((int)Math.Round(i / 63.0 * 255.0), 255);
          pal.Entries[i] = Color.FromArgb(z, z, z);
        }

      pal.Entries[255] = Color.Beige;

      bmp.Palette = pal;
    }

    // ****************************
    // *                          *
    // *  Process Ray from PDP-8  *
    // *                          *
    // ****************************

    public void Ray(int w)
    {
      // 0xAAA is beginning of ray sync word. Only azimuth, a binary angle, can take
      // on that value. If seen in any other phase, resync. Loss of sync might be
      // caused by ray transmission being aborted. 
      if (rayPhase != RayPhase.Azimuth && w == 0xAAA)
        rayPhase = RayPhase.Idle;

      switch (rayPhase)
      {
        case RayPhase.Idle:
          if (w == 0xAAA)
            rayPhase = RayPhase.Command;
          break;

        case RayPhase.Command:    // don't know what this does. value is 4000 for PPI
          rayPhase = RayPhase.Azimuth;
          break;

        case RayPhase.Azimuth:
          double azDeg = w * 360.0 / 4096.0;
          azLabel.Text = azDeg.ToString("f1");
          azimuth = MathUtil.mod((int)Math.Round(azDeg - 90.0), 360);
          rayPhase = RayPhase.Unknown;
          break;

        case RayPhase.Unknown:
          // radar calls it width, value 8; display calls it direction, values +-9
          rayPhase = RayPhase.ICW1;
          break;

        case RayPhase.ICW1:
          binRes = w & 7;
          binCounter = 0;

          mapIndex = 0;
          for (int skip = w >> 3; skip > 0; --skip)
            for (int r = 0; r < binRes; ++r)
            {
              int count = binMap[azimuth][mapIndex++];
              mapIndex += count;
              if (count < 0)
                break;
            }

          rayPhase = RayPhase.Bin;
          break;

        case RayPhase.Bin:
          for (int r = 0; r < binRes; ++r)
          {
            int count = binMap[azimuth][mapIndex++];
            if (count >= 0)
            {
              if (w >= 0x800)     // dbZ can be negative in close
                w = 0;

              for (int i = 0; i < count; ++i)
              {
                int index = binMap[azimuth][mapIndex++];
                dbzMap[index] = (byte)w; 
                lifeTime[index] = maxLife;
              }
            }
            else
            {
              binCounter = 99;
              break;
            }
          }

          if (++binCounter == 100)
          {
            byte z = (byte)(colorMode ? 254 : 64);
            foreach (int index in rangeRings[azimuth])
            {
              dbzMap[index] = z;
              lifeTime[index] = maxLife;
            }

            screenChanged = true;

            rayPhase = RayPhase.Idle;
          }

          break;
      }
    }

    // **************************
    // *                        *
    // *  Background and Erase  *
    // *                        *
    // **************************

    void erase()
    {
      for (int i = 0; i < dbzMap.Length; ++i)
        dbzMap[i] = 0;
      screenChanged = true;
    }

    void setBackground(byte z)
    {
      BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                     ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
      try
      {
        int stride = data.Stride;
        IntPtr scan0 = data.Scan0;

        unsafe
        {
          for (int y = 0; y < ImageDimension; ++y)
          {
            byte* row = (byte*)scan0 + (y * stride);
            for (int x = 0; x < ImageDimension; ++x)
              row[x] = z;
          }
        }
      }
      finally
      {
        bmp.UnlockBits(data);
      }
    }

    // ******************
    // *                *
    // *  Update Image  *
    // *                *
    // ******************

    private void update()
    {
      // Lock the bitmap for direct memory access
      BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                     ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
      try
      {
        int stride = data.Stride;
        IntPtr scan0 = data.Scan0;
        screenChanged = false;

        unsafe
        {
          int p = 0;
          int y = ppiRow0;
          foreach (var rowBounds in ppiPixels)
          {
            byte* row = (byte*)scan0 + (y++ * stride) + rowBounds.Item1;

            if (colorMode)
              for (int x = 0; x < rowBounds.Item2; ++x)
                row[x] = dbzMap[p++];
            else
              for (int x = 0; x < rowBounds.Item2; ++x, ++p)
                if ((row[x] = dbzMap[p]) > 0)
                {
                  if (lifeTime[p] > 0)
                    --lifeTime[p];
                  else
                    --dbzMap[p];

                  screenChanged = true;
                }
          }
        }
      }
      finally
      {
        bmp.UnlockBits(data);
      }

      Invalidate();
    }

    private void wakeup()
    {
      if (screenChanged)
        update();
    }

    // ************
    // *          *
    // *  Events  *
    // *          *
    // ************

    private void PPIForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    private void colorCheck_CheckedChanged(object sender, EventArgs e)
    {
      erase();
      setPalette();
    }

    private void eraseButton_Click(object sender, EventArgs e)
    {
      erase();
    }

    private void PPIForm_Paint(object sender, PaintEventArgs e)
    {
      float dim = Math.Min(ClientRectangle.Width, ClientRectangle.Height);
      dim *= (float)MaxRange / ImageRadius;

      float x0 = (ClientRectangle.Width - dim) / 2;
      float y0 = (ClientRectangle.Height - dim) / 2;
      RectangleF ppi = new RectangleF(x0, y0, dim, dim);

      using (var pen = new Pen(Color.SlateGray, 16))
        e.Graphics.DrawEllipse(pen, ppi);

      float r0 = dim / 2 + 8;
      x0 = ClientRectangle.Width / 2.0f;
      y0 = ClientRectangle.Height / 2.0f;

      using (var pen = new Pen(Color.Green, 4))
        for (int i = 0; i < 360; i += 15)
        {
          float r1 = r0 * (i % 45 == 0 ? 1.05f : 1.025f);
          double t = MathUtil.Radians(i);
          float cs = (float)Math.Cos(t);
          float sn = (float)Math.Sin(t);
          e.Graphics.DrawLine(pen, x0 + r0 * cs, y0 + r0 * sn, x0 + r1 * cs, y0 + r1 * sn);
        }
    }

    private void PPIForm_Resize(object sender, EventArgs e)
    {
      int dim = Math.Min(ClientRectangle.Width, ClientRectangle.Height);
      int x0 = (ClientRectangle.Width - dim) / 2;
      int y0 = (ClientRectangle.Height - dim) / 2;
      int x1 = x0 + dim;
      int y1 = y0 + dim;

      colorCheck.Left = x1 - colorCheck.Width;
      colorCheck.Top = y1 - colorCheck.Height;

      eraseButton.Left = x1 - eraseButton.Width;
      eraseButton.Top = colorCheck.Top - eraseButton.Height;

      int offset = dim / 28;
      az0Label.Left = azLabel.Left = x0 + offset;
      az0Label.Top = y0 + offset;
      azLabel.Top = az0Label.Bottom;

      el0Label.Left = x1 - el0Label.Width - offset;
      el0Label.Top = y0 + offset;

      elLabel.Left = x1 - elLabel.Width - offset;
      elLabel.Top = el0Label.Bottom;
    }

    private void PPIForm_Shown(object sender, EventArgs e)
    {
      PPIForm_Resize(sender, e);
      screenChanged = true;
    }

  }
}
