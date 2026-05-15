// ***********************
// *                     *
// *  DECtape Animation  *
// *                     *
// ***********************

// Originally written by Microsoft Copilot, with major human
// reorganization, improvements, and modifications.

using CSharpCommon;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Policy;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DECtapeControl
{
  public class DecTapeDriveControl : UserControl
  {
    // *********************************
    // *                               *
    // *  Designer-Exposed Properties  *
    // *                               *
    // *********************************

    public float BaseLinearSpeed { get; set; } = 180f; // pixels/sec at SpeedFactor = 1
    public int ReelRadiusLogical { get; set; } = 50;

    public string LeftReelLabel { get; set; } = string.Empty;

    public string RightReelLabel { get; set; } = String.Empty;

    bool tapeLoaded_ = false;
    public bool TapeLoaded
    {
      get => tapeLoaded_;
      set
      {
        tapeLoaded_ = value;
        Invalidate();
      }
    }

    public bool UseInternalTimer {  get; set; }

    private float reelRadiusPixels { get { return LogicalToDeviceUnits(ReelRadiusLogical); } }

    // *****************
    // *               *
    // *  Constructor  *
    // *               *
    // *****************

    public DecTapeDriveControl()
    {
      DoubleBuffered = true;
      ResizeRedraw = true;

      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      this.SetStyle(ControlStyles.ResizeRedraw, true);

      if (UseInternalTimer)
      {
        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = 50; // ~20 FPS
        _timer.Tick += Timer_Tick;
        _timer.Start();
      }

      reelTimer.Start();

      this.MinimumSize = new Size(220, 120);
    }

    // *****************
    // *               *
    // *  Diagnostics  *
    // *               *
    // *****************

    Stopwatch drawTimer = new Stopwatch();

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public double ReelTime { get; private set; }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public double BackTime { get; private set; }

    // *****************************
    // *                           *
    // *  Reel spinning animation  *
    // *                           *
    // *****************************

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public float SpeedFactor
    {
      get => targetSpeed;
      set
      {
        currentAccel = (value - targetSpeed) / 0.375f;
        targetSpeed = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public float TapeFillFactor { get; set; } = 0.90f;

    private const float emptyTapeRadius = 0.65f;
    private const float fullTapeRadius  = 0.95f;

    private float _leftAngleDeg;
    private float _rightAngleDeg;

    private readonly System.Windows.Forms.Timer _timer;

    private Stopwatch reelTimer = new Stopwatch();

    private float currentAccel = 0f;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    public void ReelTick()
    {
      float deltaTimeSec = (float)reelTimer.Elapsed.TotalSeconds;
      reelTimer.Restart();

      if (currentAccel != 0)
      {
        currentSpeed += currentAccel * deltaTimeSec;

        if (currentAccel > 0)
          currentSpeed = Math.Min(currentSpeed, targetSpeed);
        else
          currentSpeed = Math.Max(currentSpeed, targetSpeed);

        if (currentSpeed == targetSpeed)
          currentAccel = 0;
      }

      UpdateAngles(deltaTimeSec);
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      ReelTick();
    }

    // Radius of tape on left reel at TapeFillFactor
    private float leftTapeRadius
    {
      get
      {
        float r = emptyTapeRadius + (fullTapeRadius - emptyTapeRadius) * TapeFillFactor;
        return r * reelRadiusPixels;
      }
    }

    // Radius of tape on right reel at TapeFillFactor
    private float rightTapeRadius
    {
      get
      {
        float r = emptyTapeRadius + (fullTapeRadius - emptyTapeRadius) * (1f - TapeFillFactor);
        return r * reelRadiusPixels;
      }
    }

    // Make reels spin
    private void UpdateAngles(float dt)
    {
      if (currentSpeed == 0f)
        return;

      float linearSpeed = BaseLinearSpeed * currentSpeed; // pixels/sec

      float leftOmegaDeg  = (linearSpeed / leftTapeRadius ) * (180f / (float)Math.PI); // deg/sec
      float rightOmegaDeg = (linearSpeed / rightTapeRadius) * (180f / (float)Math.PI); // deg/sec

      float leftDeltaDeg = leftOmegaDeg * dt;
      float rightDeltaDeg = rightOmegaDeg * dt;

      _leftAngleDeg += leftDeltaDeg;
      _rightAngleDeg += rightDeltaDeg;

      Invalidate();
    }

    // **************
    // *            *
    // *  Geometry  *
    // *            *
    // **************

    private struct HeadBlock
    {
      const float aFctr = 0.15f;
      const float bFctr = 0.20f;
      const float rFctr = 1.5f;

      float x;    // left edge
      float y;    // top edge
      float w;    // width
      float h;    // height

      float a;    // height of head
      float b;    // width of curved top of head
      float e;    // height of curved top of head
      float r;    // radius of curved top of head
      float d;    // angle of curve, degrees

      float u;    // width of each straight part of head
      float v;    // height of each straight part of head

      public void Compute(RectangleF rect)
      {
        x = rect.X;
        y = rect.Y;
        w = rect.Width;
        h = rect.Height;

        a = aFctr * h;
        b = bFctr * w;
        r = rFctr * a;

        double t = Math.Acos(1 - b * b / (2 * r * r)); // theta radians
        e = (float)(r * (1 - Math.Cos(t / 2)));
        d = (float)MathUtil.Degrees(t);

        u = (w - b) / 2;
        v = a - e;
      }

      //             F  A
      //         E         B
      //
      //
      //         D         C
      public PointF A { get { return new PointF(x + u + b, y + e); } }
      public PointF B { get { return new PointF(x + w, y + e + v); } }
      public PointF C { get { return new PointF(x + w, y + h); } }
      public PointF D { get { return new PointF(x, y + h); } }
      public PointF E { get { return new PointF(x, y + e + v); } }
      public PointF F { get { return new PointF(x + u, y + e); } }

      public void Arc(GraphicsPath path)
      {
        path.AddArc(x + w / 2 - r, y, 2 * r, 2 * r, 270 - d / 2, d);
      }
    }

    // Derived geometry (computed on resize)
    private Point     _leftReelCenter;
    private Point     _rightReelCenter;
    private Rectangle _frameRect;
    private Rectangle _headBlockRect;
    private int       _frameMargin;
    private float     _reelSpacing;

    private HeadBlock headBlock;

    public void ComputeGeometry(Rectangle rect)
    {
      float R = reelRadiusPixels;

      // Reel spacing ~2.6R
      _reelSpacing = R * 2.6f;

      // TU55 head proportions: compact, slightly taller than wide
      float headWidth  = R * 0.50f;   // was 55
      float headHeight = R * 0.80f;   // was 42

      float headTopAboveReelCenter = R * 1.50f + headHeight / 2;
      float fullHeight = R + headTopAboveReelCenter;

      float centerX = rect.Left + 0.5f * rect.Width;
      float centerY = rect.Top + 0.5f * rect.Height + 0.5f * fullHeight - R;

      _leftReelCenter = new Point((int)(centerX - _reelSpacing / 2), (int)centerY);

      _rightReelCenter = new Point((int)(centerX + _reelSpacing / 2), (int)centerY);

      // --- Correct TU55 head geometry aligned to tape path ---
      int tapeY = _leftReelCenter.Y - (int)headTopAboveReelCenter;

      // Door frame margin ~0.18R
      _frameMargin = (int)(R * 0.18f);
      _frameRect = Rectangle.Inflate(rect, -_frameMargin, -_frameMargin);

      // Center the head vertically on the tape path
      _headBlockRect = new Rectangle((int)(centerX - headWidth / 2),  tapeY,
                                     (int)headWidth, (int)headHeight);

      headBlock.Compute(_headBlockRect);
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      ComputeGeometry(ClientRectangle);   // recompute reel centers, spacing, tape path, head block
      Invalidate();        // request redraw
    }

    // *************************
    // *                       *
    // *  Draw Reels and Tape  *
    // *                       *
    // *************************

    protected override void OnPaint(PaintEventArgs e)
    {
      drawTimer.Restart();
      base.OnPaint(e);

      var g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;

      DrawReels(g);

      drawTimer.Stop();
      ReelTime = drawTimer.Elapsed.TotalMilliseconds;
    }

    private void DrawReels(Graphics g)
    {
      DrawReel(g, _leftReelCenter, _leftAngleDeg, LeftReelLabel);
      DrawReel(g, _rightReelCenter, _rightAngleDeg, RightReelLabel);
    }

    private PointF tapeEmergePoint(Point origin, PointF top, double r, double q, int right)
    {
      double cx = top.X - origin.X;
      double cy = top.Y - origin.Y;
      double c2 = cx * cx + cy * cy;

      double ax = (q * q * cx - right * q * cy * Math.Sqrt(c2 - q * q)) / c2;
      double ay = -Math.Sqrt(q * q - ax * ax);

      double bx = ax - right * Math.Sqrt(ax * ax + MathUtil.square(ay * r / q) - q * q);
      double by = -Math.Sqrt(r * r - bx * bx);

      return new PointF((float)(bx + origin.X), (float)(by + origin.Y));
    }

    private void DrawTape(Graphics g, Point leftCenter, Point rightCenter)
    {
      // Replaced Copilot poor attenpt
      float R = reelRadiusPixels;
      float tapeThickness = 6f;   // thin side-view tape

      float midX = 0.5f * (leftCenter.X + rightCenter.X);
      float midY = _headBlockRect.Top;
      PointF top = new PointF(midX, midY);

      PointF leftTangentTop  = tapeEmergePoint(leftCenter , top, R, leftTapeRadius, -1);
      PointF rightTangentTop = tapeEmergePoint(rightCenter, top, R, rightTapeRadius, 1);

      GraphicsPath path = new GraphicsPath();
      path.AddLine(leftTangentTop, headBlock.F);
      headBlock.Arc(path);
      path.AddLine(headBlock.A, rightTangentTop);

      using var tapePen = new Pen(Color.FromArgb(210, 90, 90), tapeThickness)
      {
        LineJoin = LineJoin.Round,
        StartCap = LineCap.Round,
        EndCap = LineCap.Round
      };

      g.DrawPath(tapePen, path);
    }

    private void DrawReel(Graphics g, Point center, float angleDeg, string label)
    {
      float radius = reelRadiusPixels;
      var reelRect = new RectangleF(center.X - radius, center.Y - radius,
                                    radius * 2, radius * 2);

      // Save state
      var state = g.Save();

      // Move origin to center and rotate
      g.TranslateTransform(center.X, center.Y);

      // Draw outer rim
      using (var rimBrush = new SolidBrush(Color.FromArgb(240, 240, 240)))
      using (var rimPen = new Pen(Color.Gray, 2f))
      {
        g.FillEllipse(rimBrush, -radius, -radius, radius * 2, radius * 2);
        g.DrawEllipse(rimPen, -radius, -radius, radius * 2, radius * 2);
      }

      // Tape area (inner darker ring)
      float tapeInnerRadius = radius * 0.65f;
      using (var tapeBrush = new SolidBrush(Color.FromArgb(120, 120, 120)))
      {
        g.FillEllipse(tapeBrush,
            -tapeInnerRadius, -tapeInnerRadius,
             tapeInnerRadius * 2, tapeInnerRadius * 2);
      }

      // Hub
      float hubRadius = radius * 0.18f;
      using (var hubBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
      using (var hubPen = new Pen(Color.DimGray, 1.5f))
      {
        g.FillEllipse(hubBrush, -hubRadius, -hubRadius, hubRadius * 2, hubRadius * 2);
        g.DrawEllipse(hubPen, -hubRadius, -hubRadius, hubRadius * 2, hubRadius * 2);
      }

      // Spokes
      g.RotateTransform(angleDeg);
      DrawSpokes(g, hubRadius, radius * 0.65f);

      // Label
      drawLabel(g, label);

      // Restore
      g.Restore(state);
    }

    private void DrawSpokes(Graphics g, float innerRadius, float outerRadius)
    {
      using var spokePen = new Pen(Color.LightGray, 4f)
      {
        StartCap = LineCap.Round,
        EndCap = LineCap.Round
      };

      int spokeCount = 6;
      for (int i = 0; i < spokeCount; i++)
      {
        float angle = (float)(i * (360.0 / spokeCount));
        float rad = angle * (float)Math.PI / 180f;

        float x1 = innerRadius * (float)Math.Cos(rad);
        float y1 = innerRadius * (float)Math.Sin(rad);
        float x2 = outerRadius * (float)Math.Cos(rad);
        float y2 = outerRadius * (float)Math.Sin(rad);

        g.DrawLine(spokePen, x1, y1, x2, y2);
      }
    }

    private void drawLabel(Graphics g, string label)
    {
      for (int i = 0; i < label.Length; ++i)
      {
        string c = label.Substring(i, 1);
        float charWd = g.MeasureString(c, Font).Width;
        g.DrawString(c, Font, Brushes.Red, -0.5f * charWd, -reelRadiusPixels);
        g.RotateTransform((float)MathUtil.Degrees(charWd / reelRadiusPixels));
      }
    }

    // *********************
    // *                   *
    // *  Draw Background  *
    // *                   *
    // *********************

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      drawTimer.Restart();
      DrawTu55Background(e.Graphics, this.ClientRectangle);
      drawTimer.Stop();
      BackTime = drawTimer.Elapsed.TotalMilliseconds;
    }

    private void DrawTu55Background(Graphics g, Rectangle rect)
    {
      g.SmoothingMode = SmoothingMode.AntiAlias;

      DrawDoorFrame(g);
      if (TapeLoaded)
        DrawTape(g, _leftReelCenter, _rightReelCenter);
      DrawHeadBlock2(g);
      DrawCornerScrews(g, rect);
    }

    private void DrawDoorFrame(Graphics g)
    {
      using (var frameBrush = new LinearGradientBrush(
          _frameRect,
          Color.FromArgb(80, 80, 80),
          Color.FromArgb(40, 40, 40),
          LinearGradientMode.Vertical))
      {
        g.FillRectangle(frameBrush, _frameRect);
      }

      using var framePen = new Pen(Color.FromArgb(100, 100, 100), 3f);
      g.DrawRectangle(framePen, _frameRect);
    }

    private void DrawHeadBlock2(Graphics g)
    {
      // Replaced Copilot's lousy attempt

      GraphicsPath path = new GraphicsPath();

      path.AddLine(headBlock.A, headBlock.B);
      path.AddLine(headBlock.B, headBlock.C);
      path.AddLine(headBlock.C, headBlock.D);
      path.AddLine(headBlock.D, headBlock.E);
      path.AddLine(headBlock.E, headBlock.F);
      headBlock.Arc(path);

      path.CloseFigure();

      using (var brush = new LinearGradientBrush
        (_headBlockRect, Color.FromArgb(192, 192, 192), Color.FromArgb(232, 232, 232),
         LinearGradientMode.Vertical))
      {
        g.FillPath(brush, path);
      }
    }

    private void DrawCornerScrews(Graphics g, Rectangle rect)
    {
      int margin = 2 * _frameMargin;
      DrawScrew(g, new Point(rect.Left + margin, rect.Top + margin));
      DrawScrew(g, new Point(rect.Right - margin, rect.Top + margin));
      DrawScrew(g, new Point(rect.Left + margin, rect.Bottom - margin));
      DrawScrew(g, new Point(rect.Right - margin, rect.Bottom - margin));
    }

    private void DrawScrew(Graphics g, Point center)
    {
      int r = (int)(reelRadiusPixels * 0.07f);

      using (var ringBrush = new LinearGradientBrush(
          new Rectangle(center.X - r, center.Y - r, r * 2, r * 2),
          Color.FromArgb(200, 200, 200),
          Color.FromArgb(120, 120, 120),
          LinearGradientMode.Vertical))
      {
        g.FillEllipse(ringBrush, center.X - r, center.Y - r, r * 2, r * 2);
      }

      using var ringPen = new Pen(Color.FromArgb(60, 60, 60), 1.5f);
      g.DrawEllipse(ringPen, center.X - r, center.Y - r, r * 2, r * 2);

      using var slotPen = new Pen(Color.FromArgb(40, 40, 40), 2f);
      g.DrawLine(slotPen, center.X - r / 2, center.Y, center.X + r / 2, center.Y);
    }
  }

}
