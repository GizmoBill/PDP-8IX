
// *******************
// *                 *
// *  Shape Control  *
// *                 *
// *******************

// Written by MS Copilot

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ShapeControls
{
  [DesignerCategory("Code")]
  public class ShapeControl : Control
  {
    public enum ShapeKind
    {
      Rectangle,
      Ellipse,
      RoundedRectangle
    }

    // Internal properties (not exposed in designer unless you add attributes)

    private ShapeKind kind = ShapeKind.Rectangle;
    private Color color = Color.LightBlue;
    private int cornerRad = 12;


    public ShapeKind Shape
    {
      get { return kind; }
      set
      {
        kind = value;
        Invalidate();
      }
    }

    public Color FillColor
    {
      get { return color; }
      set
      {
        color = value;
        Invalidate();
      }
    }

    public int CornerRadius
    {
      get { return cornerRad; }
      set
      {
        cornerRad = value;
        Invalidate();
      }
    }

    public ShapeControl()
    {
      SetStyle(ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.UserPaint |
               ControlStyles.SupportsTransparentBackColor, true);

      Size = new Size(60, 60);
      BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      var g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;

      var rect = new Rectangle(0, 0, Width - 1, Height - 1);

      using var brush = new SolidBrush(FillColor);

      switch (Shape)
      {
        case ShapeKind.Rectangle:
          g.FillRectangle(brush, rect);
          break;

        case ShapeKind.Ellipse:
          g.FillEllipse(brush, rect);
          break;

        case ShapeKind.RoundedRectangle:
          using (var path = CreateRoundedRect(rect, CornerRadius))
            g.FillPath(brush, path);
          break;
      }

      // Design-time outline so the control is easy to see/select
      if (DesignMode)
      {
        using var dash = new Pen(Color.Gray) { DashStyle = DashStyle.Dot };
        g.DrawRectangle(dash, rect);
      }
    }

    private GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
    {
      int d = radius * 2;
      var path = new GraphicsPath();

      path.AddArc(rect.X, rect.Y, d, d, 180, 90);                         // TL
      path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);                 // TR
      path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);          // BR
      path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);                 // BL

      path.CloseFigure();
      return path;
    }
  }
}
