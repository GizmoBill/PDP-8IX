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
  public partial class Tek611Form : Form
  {
    Tek611 tek611;

    Bitmap bmp;

    byte[,] plotArea = new byte[520 / 8, 513];

    private bool nonStoreMode { get { return nonStoreCheck.Checked; } }

    private void nonStoreCheck_CheckedChanged(object sender, EventArgs e)
    {
      setScreenChanged();
    }

    int screenChanged = 0;

    public Tek611Form()
    {
      InitializeComponent();

      tek611 = new Tek611(this);

      bmp = new Bitmap(520, 513, PixelFormat.Format1bppIndexed);
      ColorPalette pal = bmp.Palette;
      pal.Entries[0] = Color.Black;
      pal.Entries[1] = Color.White;
      bmp.Palette = pal;

      RegisterAction("611", wakeup);

      Erase();

      BackgroundImage = bmp;
    }

    void setScreenChanged()
    {
      screenChanged = nonStoreMode ? 2 : 1;
      CallMeReal("611", 1.0e6 / 6, true);
    }

    public void Erase()
    {
      Array.Clear(plotArea);
      setScreenChanged();
    }

    public void Plot(int x, int y)
    {
      const int pixSize = 2;

      x >>= 1;
      y = bmp.Height - pixSize - (y >> 1);

      if ((uint)x >= bmp.Width || (uint)y >= bmp.Height)
        throw new ArgumentOutOfRangeException("Pixel coordinates out of bounds.");

      for (int dy = 0; dy < pixSize; ++dy)
        for (int dx = 0; dx < pixSize; ++dx)
        {
          int bit = 1 << ((x + dx & 7) ^ 7);
          plotArea[(x + dx) >> 3, y + dy] |= (byte)bit;
        }

      setScreenChanged();
    }

    private void update()
    {
      // Lock the bitmap for direct memory access
      BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                     ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
      try
      {
        int stride = data.Stride;
        IntPtr scan0 = data.Scan0;

        unsafe
        {
          for (int y = 0; y < bmp.Height; ++y)
          {
            byte* row = (byte*)scan0 + (y * stride);

            if (nonStoreMode)
              for (int x = 0; x < bmp.Width / 8; ++x)
              {
                row[x] = plotArea[x, y];
                plotArea[x, y] = 0;
              }
            else
              for (int x = 0; x < bmp.Width / 8; ++x)
                row[x] = plotArea[x, y];
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
      if (screenChanged > 0)
      {
        --screenChanged;
        update();
      }
    }

    private void Tek611Form_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }
  }
}
