using CSharpCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    const int MaxRange = 250;   // km
    const int ImageDimension = 2 * MaxRange + 1;
    const double kmPerNmi = 1.852;

    Bitmap bmp;

    // For drawing rays
    int azimuth;      // az of ray [0 .. 360)
    int binCounter;   // counts bins [0 .. 100]
    int binRes;       // half-nmi per bin
    int mapIndex;     // index into binMap[azimuth]

    // 1D array for faster access with compiled binMap data. Each byte is 1 kn x 1 km.
    byte[] dbzMap = new byte[ImageDimension * ImageDimension];

    // For each azimuth, a list of dbzMap indices for each half-nmi bin. The values for
    // a bin are a count followed by count indices. Count may be 0. Count -1 terminates
    // the list. All of the polar->cartesian work is done at construction, drawing
    // and erasing is very fast.
    List<int>[] binMap = new List<int>[360];

    // Range ring indices. 5 50 km rings at each azimuth
    List<int>[] rangeRings = new List<int>[360];

    // Screen fade in non-color mode. dbzMap values persist for maxLife update
    // cycles, then fade to black.
    byte[] lifeTime = new byte[ImageDimension * ImageDimension];
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

    bool screenChanged = false;

    public PPIForm()
    {
      InitializeComponent();

      bmp = new Bitmap(ImageDimension, ImageDimension, PixelFormat.Format8bppIndexed);
      setPalette();

      erase();

      BackgroundImage = bmp;

      initMap();

      RegisterAction("PPI", wakeup);
      wakeup();
    }

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
      for (int i = 0; i < dbzMap.Length; ++i)
      {
        double x = i % ImageDimension - MaxRange;
        double y = i / ImageDimension - MaxRange;
        int angle = MathUtil.mod((int)Math.Round(MathUtil.Degrees(Math.Atan2(y, x))), 360);
        double range = MathUtil.EuclidDist(x, y);
        int bin = (int)Math.Round(range / kmPerNmi * 2.0);
        map[angle].Add((bin, i));

        if ((int)Math.Round(range) % 50 == 0 && range <= MaxRange + 25)
          rangeRings[angle].Add(i);
      }

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
        for (int z = 1; z < 256; z += 2)
          pal.Entries[z / 2 + 128] = Color.FromArgb(z, z, z);
      }
      else
        for (int i = 0; i < 256; i++)
        {
          int z = Math.Min((int)Math.Round(i / 63.0 * 255.0), 255);
          pal.Entries[i] = Color.FromArgb(z, z, z);
        }

      bmp.Palette = pal;
    }

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
          azimuth = MathUtil.mod((int)Math.Round(w * 360.0 / 4096.0 - 90.0), 360);
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
              for (int i = 0; i < count; ++i)
              {
                int index = binMap[azimuth][mapIndex++];
                dbzMap[index] = (byte)w;
                lifeTime[index] = maxLife;
              }
            else
            {
              binCounter = 99;
              break;
            }
          }

          if (++binCounter == 100)
          {
            byte z = (byte)(colorMode ? 255 : 64);
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

    void erase()
    {
      Array.Clear(dbzMap);
      screenChanged = true;
    }

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
          for (int y = 0; y < bmp.Height; ++y)
          {
            byte* row = (byte*)scan0 + (y * stride);

            if (colorMode)
              for (int x = 0; x < ImageDimension; ++x)
                row[x] = dbzMap[p++];
            else
              for (int x = 0; x < ImageDimension; ++x, ++p)
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

    void wakeup()
    {
      if (screenChanged)
        update();

      CallMeReal("PPI", 1.0e6 / 8.0);
    }

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
  }
}
