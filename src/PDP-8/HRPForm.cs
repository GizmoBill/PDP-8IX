using CSharpCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static PDP8;

namespace PDP_8
{
  public partial class HRPForm : Form
  {
    HRP hrp;

    public HRPForm()
    {
      InitializeComponent();

      hrp = new HRP(this);
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
    // *  OSW  *
    // *       *
    // *********

    public int OSW
    {
      get { return FromOctal(oswLabel.Text); }
      set { oswLabel.Text = ToOctal(value); }
    }

    public int SevenSeg
    {
      get { return int.Parse(sevenSegLabel.Text); }
      set { sevenSegLabel.Text = value.ToString("d2"); }
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
      get { return (int)tls2Numeric.Value; }
      set { tls2Numeric.Value = value; }
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
    public int Az66 { get { return (int)((float)wr66AzNumeric.Value / 360.0f * (1 << 14)); } }
    public int El66 { get { return (int)((float)wr66ElNumeric.Value / 360.0f * (1 << 14)); } }

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

    // ****************
    // *              *
    // *  WR66 Servo  *
    // *              *
    // ****************

    public int WR66ServoLow
    {
      get { return FromOctal(wr66ServoLowLabel.Text); }
      set { wr66ServoLowLabel.Text = ToOctal(value); }
    }

    public int WR66ServoHigh
    {
      get { return FromOctal(wr66ServoHighLabel.Text); }
      set { wr66ServoHighLabel.Text = ToOctal(value); }
    }

    // ****************
    // *              *
    // *  Integrator  *
    // *              *
    // ****************

    public int IntegratorControlWord1
    {
      get { return FromOctal(icw1Label.Text); }
      set { icw1Label.Text = ToOctal(value); }
    }

    public int IntegratorControlWord2
    {
      get { return FromOctal(icw2Label.Text); }
      set { icw2Label.Text = ToOctal(value); }
    }

    // ***********
    // *         *
    // *  State  *
    // *         *
    // ***********

    public XmlLiteNode State
    {
      get
      {
        string s = string.Format("osw1 = {0};\nosw2 = {1};\n", oswLabel.Text, sevenSegLabel.Text);
        s += string.Format("tls2 = {0};\n", TLS);
        s += string.Format("wr66az = {0};\n", wr66AzNumeric.Value);
        s += string.Format("wr66el = {0};\n", wr66ElNumeric.Value);
        s += string.Format("wr73az = {0};\n", wr73AzNumeric.Value);
        s += string.Format("wr73el = {0};\n", wr73ElNumeric.Value);
        return new XmlLiteNode("hrp", s);
      }

      set
      {
        CheckTag(value, "hrp");
        ParamHolder ph = new ParamHolder(value.Value);
        for (int i = 0; i < ph.Count; ++i)
          switch (ph.Name(i))
          {
            case "osw1":
              oswLabel.Text = ph[i];
              break;

            case "osw2":
              sevenSegLabel.Text = ph[i];
              break;

            case "tls2":
              TLS = int.Parse(ph[i]);
              break;

            case "wr66az":
              wr66AzNumeric.Value = decimal.Parse(ph[i]);
              break;

            case "wr66el":
              wr66ElNumeric.Value = decimal.Parse(ph[i]);
              break;

            case "wr73az":
              wr73AzNumeric.Value = decimal.Parse(ph[i]);
              break;

            case "wr73el":
              wr73ElNumeric.Value = decimal.Parse(ph[i]);
              break;

            default:
              throw new Exception("Unknown osw parameter " + ph.Name(i));
          }
      }
    }
  }
}
