using ShapeControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static PDP8;

namespace PDP_8
{
  public partial class FrontPanel : Form
  {
    float aspect;

    public FrontPanel()
    {
      InitializeComponent();

      aspect = (float)ClientSize.Width / ClientSize.Height;

      initLights();
      initSwitches();
    }

    // ************
    // *          *
    // *  Lights  *
    // *          *
    // ************

    struct Light
    {
      public ShapeControl shape;
      public float left, top, width, height;

      public Light(FrontPanel fp, ShapeControl shape)
      {
        this.shape = shape;
        shape.FillColor = Color.Black;

        left = (float)shape.Left / fp.ClientSize.Width;
        top = (float)shape.Top / fp.ClientSize.Height;
        width = (float)shape.Width / fp.ClientSize.Width;
        height = (float)shape.Height / fp.ClientSize.Height;

        fp.lights.Add(this);
      }

      public Light(FrontPanel fp, bool box, int x, int y)
      {
        shape = new ShapeControl();
        shape.FillColor = Color.Black;
        shape.Shape = ShapeControl.ShapeKind.RoundedRectangle;
        shape.CornerRadius = fp.lightShape0.CornerRadius;

        if (!box)
        {
          left = fp.lightShape0.Left + x * (fp.lightShape1.Left - fp.lightShape0.Left) / 17.0f;
          top = fp.lightShape0.Top + y * (fp.lightShape1.Top - fp.lightShape0.Top) / 4.0f;
        }
        else
        {
          left = fp.lightShape2.Left + x * (fp.lightShape3.Left - fp.lightShape2.Left) / 2.0f;
          top = fp.lightShape2.Top + y * (fp.lightShape3.Top - fp.lightShape2.Top) / 2.0f;
        }

        shape.SetBounds((int)left, (int)top, fp.lightShape0.Width, fp.lightShape0.Height);

        left /= fp.ClientSize.Width;
        top /= fp.ClientSize.Height;

        width = (float)fp.lightShape0.Width / fp.ClientSize.Width;
        height = (float)fp.lightShape0.Height / fp.ClientSize.Height;


        shape.Visible = true;
        fp.Controls.Add(shape);
        fp.lights.Add(this);
      }
    }

    List<Light> lights = new List<Light>();
    int[] onCounts;
    int onCycles;

    void initLights()
    {
      // CDF, CIF
      new Light(this, lightShape0);
      for (int x = 1; x < 6; ++x)
        new Light(this, false, x, 0);

      // PC, MAR, MBR
      for (int y = 0; y < 3; ++y)
        for (int x = 6; x < 18; ++x)
          new Light(this, false, x, y);

      // Link, AC
      for (int x = 5; x < 18; ++x)
        new Light(this, false, x, 3);

      // SC
      for (int x = 0; x < 5; ++x)
        new Light(this, false, x, 4);

      // MQ
      for (int x = 6; x < 17; ++x)
        new Light(this, false, x, 4);
      new Light(this, lightShape1);

      // The box
      new Light(this, lightShape2);
      for (int y = 1; y < 8; ++y)
        new Light(this, true, 0, y);
      for (int y = 0; y < 6; ++y)
        new Light(this, true, 1, y);
      for (int y = 0; y < 2; ++y)
        new Light(this, true, 2, y);
      new Light(this, lightShape3);

      onCounts = new int[lights.Count];
    }

    void processRegister(ulong r, int bits, int index)
    {
      for (ulong i = 1ul << (bits - 1); i != 0; i >>= 1, ++index)
        if ((r & i) != 0)
          ++onCounts[index];
    }

    public void ProcessLights()
    {
      ulong r = ((ulong)Cpu.CDF << 52) | ((ulong)Cpu.CIF << 49) | ((ulong)Cpu.PC << 37);
      r |= ((ulong)Cpu.MAR << 25) | ((ulong)Cpu.MBR << 13) | ((ulong)Cpu.Link << 12);
      r |= (ulong)Cpu.AC;
      processRegister(r, 55, 0);

      r = ((ulong)Cpu.SC << 12) | (ulong)Cpu.MQ;
      processRegister(r, 17, 55);

      ++onCounts[Cpu.Instr + 72];

      ++onCounts[(int)Cpu.CurrentCycle + 80];

      if (Cpu.ION)
        ++onCounts[86];

      if (Cpu.Run)
        ++onCounts[88];

      ++onCycles;
    }

    public void SetLights()
    {
      const int z0 = 96;
      for (int i = 0; i < onCounts.Length; ++i)
      {
        int z = (255 - z0) * onCounts[i] / onCycles + z0;
        lights[i].shape.FillColor = Color.FromArgb(z, z, z);
        onCounts[i] = 0;
      }

      onCycles = 0;
    }

    // **************
    // *            *
    // *  Switches  *
    // *            *
    // **************

    enum SwitchType
    {
      Toggle,
      Start,
      LoadAddr,
      Examine,
      Deposit,
      Continue,
      Stop
    }

    class Switch
    {
      public PictureBox sw0, sw1;
      public float left, top, width, height;

      public SwitchType swType;

      private int on = 0;

      public int On
      {
        get { return sw1.Visible ? 1 : 0; }
        set
        {
          on = value;
          sw0.Visible = value == 0;
          sw1.Visible = value != 0;
        }
      }

      public Switch(FrontPanel fp, PictureBox model0, PictureBox model1, bool flip)
      {
        PictureBox pb = new PictureBox();
        pb.Image = model1.Image;
        pb.SizeMode = PictureBoxSizeMode.Zoom;
        pb.SetBounds(model0.Left, model0.Top, model0.Width, model0.Height);

        left = (float)model0.Left / fp.ClientSize.Width;
        top = (float)model0.Top / fp.ClientSize.Height;
        width = (float)model0.Width / fp.ClientSize.Width;
        height = (float)model0.Height / fp.ClientSize.Height;

        if (flip)
        {
          sw1 = model0;
          sw0 = pb;
        }
        else
        {
          sw0 = model0;
          sw1 = pb;
        }

        sw0.Cursor = Cursors.Hand;
        sw1.Cursor = Cursors.Hand;

        On = 0;
        swType = SwitchType.Toggle;

        fp.Controls.Add(pb);
        fp.switches.Add(this);
      }

      public Switch(FrontPanel fp, int x, PictureBox model0, PictureBox model1,
                    SwitchType st = SwitchType.Toggle)
      {
        sw0 = new PictureBox();
        sw1 = new PictureBox();

        sw0.Image = model0.Image;
        sw1.Image = model1.Image;

        sw0.SizeMode = PictureBoxSizeMode.Zoom;
        sw1.SizeMode = PictureBoxSizeMode.Zoom;

        sw0.Cursor = Cursors.Hand;
        sw1.Cursor = Cursors.Hand;

        left = fp.tan0Picture.Left + x * (fp.tan1Picture.Left - fp.tan0Picture.Left) / 25.0f;
        sw0.SetBounds((int)left, fp.tan0Picture.Top, model0.Width, model0.Height);
        sw1.SetBounds((int)left, fp.tan0Picture.Top, model0.Width, model0.Height);

        left /= fp.ClientSize.Width;
        top = (float)fp.tan0Picture.Top / fp.ClientSize.Height;

        width = (float)model0.Width / fp.ClientSize.Width;
        height = (float)model0.Height / fp.ClientSize.Height;

        swType = st;
        On = st == SwitchType.Deposit ? 1 : 0;

        fp.Controls.Add(sw0);
        fp.Controls.Add(sw1);
        fp.switches.Add(this);
      }
    }

    List<Switch> switches = new List<Switch>();

    void initSwitches()
    {
      new Switch(this, tan0Picture, tan1Picture, false);
      for (int i = 1; i < 18; ++i)
        if ((i / 3 & 1) == 0)
          new Switch(this, i, tan0Picture, tan1Picture);
        else
          new Switch(this, i, white0Picture, white1Picture);

      new Switch(this, 18, tan0Picture, tan1Picture, SwitchType.Start);
      new Switch(this, 19, white0Picture, white1Picture, SwitchType.LoadAddr);
      new Switch(this, 20, white0Picture, white1Picture, SwitchType.Deposit);
      new Switch(this, 21, white0Picture, white1Picture, SwitchType.Examine);
      new Switch(this, 22, white0Picture, white1Picture, SwitchType.Continue);
      new Switch(this, 23, white0Picture, white1Picture, SwitchType.Stop);

      new Switch(this, 24, tan0Picture, tan1Picture);
      new Switch(this, tan1Picture, tan0Picture, true);

      foreach (Switch sw in switches)
      {
        sw.sw0.MouseDown += switch_MouseDown;
        sw.sw1.MouseDown += switch_MouseDown;
        sw.sw0.MouseUp += switch_MouseUp;
        sw.sw1.MouseUp += switch_MouseUp;
        sw.sw0.MouseMove += switch_MouseMove;
        sw.sw1.MouseMove += switch_MouseMove;

        sw.sw0.Tag = sw;
        sw.sw1.Tag = sw;
      }
    }

    private void Sw0_MouseMove(object? sender, MouseEventArgs e)
    {
      throw new NotImplementedException();
    }

    public int SwitchRegister
    {
      get
      {
        int sw = 0;
        for (int i = 0; i < 18; ++i)
          sw = (sw << 1) | switches[i].On;
        return sw;
      }

      set
      {
        for (int i = 17; i >= 0; --i, value >>= 1)
          switches[i].On = value & 1;
      }
    }

    float touchZone = 0.4f;   // fraction of switch that is active to touch

    bool swiping = false;
    private void switch_MouseMove(object sender, MouseEventArgs e)
    {
      if (!swiping)
        return;

      PictureBox pb = (PictureBox)sender;
      int screenX = pb.Left + e.Location.X;
      int screenY = pb.Top + e.Location.Y;

      int y = screenY - pb.Top;
      if (y <= 0 || y >= pb.Height)
        return;

      foreach (Switch sw in switches)
        if (sw.swType == SwitchType.Toggle)
        {
          int x =  screenX - sw.sw0.Left;
          if (0.2f * sw.sw0.Width <= x && x <= 0.8f * sw.sw0.Width)
          {
            int on = sw.On;
            if (y < touchZone * pb.Height)
              sw.On = 0;
            else if (y > (1 - touchZone) * pb.Height)
              sw.On = 1;

            if (sw.On != on)
              PDP8.Console.SwitchRegister = SwitchRegister;

            break;
          }
        }
    }

    private void switch_MouseDown(object sender, MouseEventArgs e)
    {
      PictureBox pb = (PictureBox)sender;
      Switch sw = (Switch)pb.Tag;

      if (sw.swType != SwitchType.Toggle)
        sw.On = 1 - sw.On;

      switch (sw.swType)
      {
        case SwitchType.Toggle:
          int on = sw.On;
          if (e.Location.Y < touchZone * pb.Height)
            sw.On = 0;
          else if (e.Location.Y > (1 - touchZone) * pb.Height)
            sw.On = 1;

          if (sw.On != on)
            PDP8.Console.SwitchRegister = SwitchRegister;
          break;

        case SwitchType.Start:
          PDP8.Console.startButton.PerformClick();
          break;

        case SwitchType.LoadAddr:
          PDP8.Console.loadAddrButton.PerformClick();
          break;

        case SwitchType.Examine:
          PDP8.Console.examineButton.PerformClick();
          break;

        case SwitchType.Deposit:
          PDP8.Console.depositButton.PerformClick();
          break;

        case SwitchType.Continue:
          if (switches[24].On == 1)
            PDP8.Console.stepButton.PerformClick();
          else if (switches[25].On == 1)
            PDP8.Console.instrButton.PerformClick();
          else
            PDP8.Console.continueButton.PerformClick();
          break;

        case SwitchType.Stop:
          PDP8.Console.stopButton.PerformClick();
          break;
      }

      swiping = true;
    }

    private void switch_MouseUp(object sender, MouseEventArgs e)
    {
      PictureBox pb = (PictureBox)sender;
      Switch sw = (Switch)pb.Tag;
      if (sw.swType != SwitchType.Toggle)
        sw.On = 1 - sw.On;

      swiping = false;
    }


    // ************
    // *          *
    // *  Events  *
    // *          *
    // ************

    private void FrontPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    bool adjusting = false;

    private void FrontPanel_Resize(object sender, EventArgs e)
    {
      // Keep aspect ratio
      // Prevent recursive resize loops
      if (adjusting) return;
      adjusting = true;

      int w = ClientSize.Width;
      int h = ClientSize.Height;

      // Decide which dimension the user is dragging more
      if (w / (float)h > aspect)
      {
        // Too wide → adjust width from height
        w = (int)(h * aspect);
      }
      else
      {
        // Too tall → adjust height from width
        h = (int)(w / aspect);
      }

      ClientSize = new Size(w, h);
      adjusting = false;

      // Move and resize lights
      foreach (Light light in lights)
      {
        light.shape.SetBounds((int)(w * light.left), (int)(h * light.top),
                              (int)(w * light.width), (int)(h * light.height));
        light.shape.CornerRadius = light.shape.Width / 4;
      }

      // And switches
      foreach (Switch sw in switches)
      {
        sw.sw0.SetBounds((int)(w * sw.left), (int)(h * sw.top),
                         (int)(w * sw.width), (int)(h * sw.height));
        sw.sw1.SetBounds((int)(w * sw.left), (int)(h * sw.top),
                         (int)(w * sw.width), (int)(h * sw.height));
      }
    }
  }
}
