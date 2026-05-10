using CSharpCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static PDP8;

namespace PDP_8
{
  public partial class HRPForm : Form
  {
    HRP hrp;
    AF01A a2d;
    Integrator integ;

    const string actionId = "hrpAction";

    public HRPForm(PPIForm ppi)
    {
      InitializeComponent();

      hrp = new HRP(this, ppi);
      a2d = new AF01A(this);
      integ = new Integrator(this);

      spyCombo.SelectedIndex = 0;
      esrCombo.SelectedIndex = 0;

      RegisterAction(actionId, wakeup);
    }

    private void HRPForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }


    // *********
    // *       *
    // *  TLS  *
    // *       *
    // *********

    bool tlsClicked = false;

    private void tlsButton_Click(object sender, EventArgs e)
    {
      tlsClicked = true;
    }

    public bool TLSClicked
    {
      get
      {
        bool clicked = tlsClicked;
        tlsClicked = false;
        return clicked;
      }
    }

    public int TLS
    {
      get { return spyCombo.SelectedIndex; }
      set { spyCombo.SelectedIndex = value; }
    }

    // ***********
    // *         *
    // *  AZ/EL  *
    // *         *
    // ***********

    bool azUpdate_ = false;

    private void wr66AzNumeric_ValueChanged(object sender, EventArgs e)
    {
      if (azUpdate_)
        return;

      azUpdate_ = true;
      NumericUpDown nud = (NumericUpDown)sender;
      if (nud.Value < 0)
        nud.Value += 360;
      if (nud.Value >= 360)
        nud.Value -= 360;
      azUpdate_ = false;
    }

    // Like the original hardware, WR66 angles are 14-bit binary and WR73 are BCD tenths degree
    public int Az66
    {
      get { return (int)Math.Round((double)wr66AzNumeric.Value / 360.0 * (1 << 14)); }
    }
    public int El66
    {
      get { return (int)Math.Round((double)wr66ElNumeric.Value / 360.0 * (1 << 14)); }
    }

    int toBCD(decimal angle)
    {
      int n = (int)(angle * 10);
      int bcd = 0;
      for (int i = 0; i < 4; ++i, n /= 10)
        bcd |= (n % 10) << (4 * i);
      return bcd;
    }

    public int Az73 { get { return toBCD(wr73AzNumeric.Value); } }

    public int El73 { get { return toBCD(wr73ElNumeric.Value); } }

    float azv66, elv66;
    float azv73;

    const double actionPeriod = 40000;

    public void SetVel66(float azv, float elv)
    {
      if (!scan66Check.Checked)
      {
        azv66 = azv;
        wr66AzVelLabel.Text = azv.ToString("f2");
      }

      elv -= 128;
      if (elv < 0)
        elv += 16;
      else if (elv > 0)
        elv -= 24;
      elv /= 64;

      wr66ElVelLabel.Text = elv.ToString("f2");
      elv66 = elv;

      CallMeReal(actionId, actionPeriod, true);
    }

    public void SetAzVel73(float azv)
    {
      if (!scan73Check.Checked)
      {
        azv73 = azv;
        wr73AzVelLabel.Text = azv.ToString("f2");
        CallMeReal(actionId, actionPeriod, true);
      }
    }

    public void SetEl73(float el)
    {
      wr73ElSetLabel.Text = el.ToString("f1");
      if (autoEl73Check.Checked)
        try
        {
          wr73ElNumeric.Value = (decimal)el;
        }
        catch (ArgumentOutOfRangeException)
        { }
    }

    private void scan66Check_CheckedChanged(object sender, EventArgs e)
    {
      azv66 = scan66Check.Checked ? 1 : 0;
      wr66AzVelLabel.Text = azv66.ToString("f2");
    }

    private void scan73Check_CheckedChanged(object sender, EventArgs e)
    {
      azv73 = scan73Check.Checked ? 1 : 0;
      wr73AzVelLabel.Text = azv73.ToString("f2");
    }

    void wakeup()
    {
      // vel in deg/integration period (RADAR uses -1, 0, or +1)
      if (azv66 != 0)
      {
        // deg/period * usec / usec/period = deg
        // make it a little less so scanning doesn't miss a ray
        double dAz = 0.75 * (azv66 * actionPeriod / integ.IntegrationPeriod);
        double newAz = MathUtil.mod((double)wr66AzNumeric.Value + dAz, 360.0);
        wr66AzNumeric.Value = (decimal)newAz;
      }

      if (azv73 != 0)
      {
        double dAz = 0.75 * (azv73 * actionPeriod / integ.IntegrationPeriod);
        double newAz = MathUtil.mod((double)wr73AzNumeric.Value + dAz, 360.0);
        wr73AzNumeric.Value = (decimal)newAz;
      }

      if (elv66 != 0 && autoEl66Check.Checked)
      {
        double dEl = 4.0 * elv66 * actionPeriod * 1.0e-6;
        try
        {
          wr66ElNumeric.Value += (decimal)dEl;
        }
        catch (ArgumentOutOfRangeException)
        {
        }
      }

      CallMeReal(actionId, actionPeriod);
    }


    // ******************************
    // *                            *
    // *  External Switch Register  *
    // *                            *
    // ******************************

    bool zeroClicked = false;
    bool radarSel = false;
    bool nextClicked = false;

    private void zeroButton_Click(object sender, EventArgs e)
    {
      zeroClicked = true;
    }

    private void radarSelButton_Click(object sender, EventArgs e)
    {
      radarSel = !radarSel;
    }

    private void nextButton_Click(object sender, EventArgs e)
    {
      nextClicked = true;
    }

    public int ESR1
    {
      get
      {
        int esr1 = 0;
        if (zeroClicked)
          esr1 |= 0x100;
        if (nextClicked)
          esr1 |= 0x040;
        if (radarSel)
          esr1 |= 0x010;
        if (runCheck.Checked)
          esr1 |= 0x004;
        if (ppiCheck.Checked)
          esr1 |= 0x008;

        zeroClicked = false;
        nextClicked = false;

        return esr1;
      }
    }

    public int ESR2
    {
      get { return esrCombo.SelectedIndex << 8; }
    }

    // ******************
    // *                *
    // *  Dial for A/D  *
    // *                *
    // ******************

    public int Dial(int channel)
    {
      switch (channel)
      {
        case 1:
          return aToD1Track.Value ^ 0x800;

        case 2:
          return aToD2Track.Value ^ 0x800;

        default:
          return 0;
      }
    }
  }
}
