using CSharpCommon;
using ShapeControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace PDP_8
{
  public partial class ASR38 : Form
  {
    // From CoPilot to fix RickTextBox flicker and ALT handling
    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern bool GetKeyboardState(byte[] lpKeyState);

    [DllImport("user32.dll")]
    static extern uint MapVirtualKey(uint uCode, uint uMapType);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
                                [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
                                int cchBuff, uint wFlags);

    private CharIn keyboard;
    private CharOut printer;

    private string actionId;

    public string XmlTag { get { return actionId; } }

    public ASR38()
    {
      InitializeComponent();

      clearScreen();
    }

    public void Reset()
    {
      paperText.Text = string.Empty;
      clearScreen();
      scrollWindowTop = 0;
      scrollWindowBottom = screenHt - 1;
      escapeState = EscapeState.Idle;
    }

    public void SetIO(int keyAddr, int printAddr)
    {
      keyboard = new CharIn(keyAddr, kbFlagChanged);
      printer = new CharOut(printAddr, printOne, prFlagChanged);
      keyboard.Reset();
      printer.Reset();
      actionId = "tty" + PDP8.ToOctal(keyAddr, 2);
      PDP8.RegisterAction(actionId, escape);
    }

    private void ASR38_Shown(object sender, EventArgs e)
    {
      printSpeedNumeric_ValueChanged(this, e);
    }

    private void ASR38_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    const char esc = (char)0x1B;

    // *******************
    // *                 *
    // *  Get/Set State  *
    // *                 *
    // *******************

    public XmlLiteNode State
    {
      get
      {
        XmlLiteNode state = new XmlLiteNode(XmlTag);

        XmlLiteNode node = new XmlLiteNode("paper", paperText.Text);
        node.SetAttribute("noIndent", "true");
        state.Children.Add(node);

        node = new XmlLiteNode("screen", getVT100Text());
        node.SetAttribute("noIndent", "true");
        state.Children.Add(node);

        string s = string.Empty;
        s += string.Format("xCursor = {0};\n", xCursor);
        s += string.Format("yCursor = {0};\n", yCursor);
        s += string.Format("top = {0};\n", scrollWindowTop);
        s += string.Format("bot = {0};\n", scrollWindowBottom);
        s += string.Format("first = {0};\n", firstArg);
        s += string.Format("second = {0};\n", secondArg);
        s += string.Format("escState = {0};\n", (int)escapeState);
        s += string.Format("alt = {0};\n", alt);
        s += string.Format("escChars = {0};\n", escapeChars);
        state.Children.Add(new XmlLiteNode("state", s));

        return state;
      }

      set
      {
        PDP8.CheckTag(value, XmlTag);

        paperText.Text = value["paper"].Value;
        setVT100Text(value["screen"].Value);

        ParamHolder ph = new ParamHolder(value["state"].Value);
        for (int i = 0; i < ph.Count; ++i)
          switch (ph.Name(i))
          {
            case "xCursor":
              xCursor = int.Parse(ph[i]);
              break;

            case "yCursor":
              yCursor = int.Parse(ph[i]);
              break;

            case "top":
              scrollWindowTop = int.Parse(ph[i]);
              break;

            case "bot":
              scrollWindowBottom = int.Parse(ph[i]);
              break;

            case "first":
              firstArg = int.Parse(ph[i]);
              break;

            case "second":
              secondArg = int.Parse(ph[i]);
              break;

            case "escState":
              escapeState = (EscapeState)int.Parse(ph[i]);
              break;

            case "alt":
              alt = bool.Parse(ph[i]);
              break;

            case "escChars":
              escapeChars = ph[i];
              break;

            default:
              throw new Exception("Unknown TTY parameter " + ph.Name(i));
          }
      }
    }

    // *************
    // *           *
    // *  Printer  *
    // *           *
    // *************

    void setCursor(int loc = -1)
    {
      if (loc < 0)
        loc = paperText.TextLength;
      paperText.Select(loc, 1);
    }

    // Flicker-free text replace, from CoPilot
    void replaceText(string text, int cursorLoc = -1)
    {
      const int WM_SETREDRAW = 0x000B;

      SendMessage(paperText.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);

      paperText.Text = text;
      setCursor(cursorLoc);

      SendMessage(paperText.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
      paperText.Invalidate();
    }

    void printOne(char c)
    {
      c = (char)(c & 0x7F);
      switch (c)
      {
        case '\a':
          Console.Beep();
          break;

        case '\b':
          string s = paperText.Text;
          if (s[s.Length - 1] != '\n')
            replaceText(s.Substring(0, s.Length - 1));
          break;

        case '\r':
          break;

        case '\n':
          goto default;

        case < ' ':
          break;      // ignore other control characters

        default:
          paperText.AppendText(new string(c, 1));
          setCursor(-1);
          break;
      }

      prLabel.Text = PDP8.ToOctal(c, 3);
    }

    void prFlagChanged(bool flag)
    {
      prShape.FillColor = flag ? Color.LightBlue : Color.Black;
    }

    private void printSpeedNumeric_ValueChanged(object sender, EventArgs e)
    {
      printer.PrintTime = Math.Pow(10.0, 6.0 - (double)printSpeedNumeric.Value);
      paperText.Focus();
    }

    //private void paperText_MouseDoubleClick(object sender, MouseEventArgs e)
    //{
    //  paperText.SelectionColor = Color.Red;
    //}

    // ***********
    // *         *
    // *  VT100  *
    // *         *
    // ***********

    const int screenWd = 80, screenHt = 42;

    char[,] screen = new char[screenWd, screenHt];

    int xCursor = 0, yCursor = 0;

    int scrollWindowTop = 0, scrollWindowBottom = screenHt - 1;

    string savePaper = null;

    Size saveASR33Size;

    private void vt100Check_CheckedChanged(object sender, EventArgs e)
    {
      if (vt100Check.Checked)
      {
        printer.PrintOne = vt100Print;
        savePaper = paperText.Text;
        saveASR33Size = this.Size;
        this.Size = new Size(1078, 1354);
        Text = "VT100";
        vt100Redraw();
      }
      else
      {
        printer.PrintOne = printOne;
        Text = "ASR-38";
        this.Size = saveASR33Size;
        replaceText(savePaper);
      }

      paperText.Focus();
    }

    private void paperText_Click(object sender, EventArgs e)
    {
      if (vt100Check.Checked)
        setVt100Cursor();
      else
        setCursor();
    }

    int vt100CursorLoc(string s)
    {
      int loc = 0;
      for (int y = 0; y < yCursor; ++y)
        loc = s.IndexOf('\n', loc) + 1;
      loc += xCursor;
      return loc;
    }

    void setVt100Cursor()
    {
      paperText.Select(vt100CursorLoc(paperText.Text), 1);
    }

    void eraseScreen()
    {
      for (int y = 0; y < screenHt; ++y)
        for (int x = 0; x < screenWd; ++x)
          screen[x, y] = ' ';
    }

    void clearScreen()
    {
      eraseScreen();

      xCursor = 0;
      yCursor = 0;

      if (vt100Check.Checked)
        vt100Redraw();
    }

    string getVT100Text()
    {
      string s = string.Empty;
      int blankLines = 0;

      for (int y = 0; y < screenHt; ++y)
      {
        int spaces = 0;
        for (int x = 0; x < screenWd; ++x)
        {
          if (screen[x, y] != ' ' || y == yCursor && x <= xCursor)
          {
            for (; blankLines > 0; --blankLines)
              s += '\n';
            for (; spaces > 0; --spaces)
              s += ' ';
            s += screen[x, y];
          }
          else
            ++spaces;
        }

        if (spaces < screenWd)
          s += '\n';
        else
          ++blankLines;
      }

      return s;
    }

    void setVT100Text(string s)
    {
      eraseScreen();
      int x = 0, y = 0;
      foreach (char c in s)
        if (c == '\n')
        {
          ++y;
          x = 0;
        }
        else
          screen[x++, y] = c;
    }

    void vt100Redraw()
    {
      string s = getVT100Text();
      replaceText(s, vt100CursorLoc(s));
    }

    void cursorDown()
    {
      if (yCursor == scrollWindowBottom)
      {
        int y;
        for (y = scrollWindowTop; y < scrollWindowBottom; ++y)
          for (int x = 0; x < screenWd; ++x)
            screen[x, y] = screen[x, y + 1];

        for (int x = 0; x < screenWd; ++x)
          screen[x, y] = ' ';

        vt100Redraw();
      }
      else
      {
        ++yCursor;
        vt100Redraw();
      }
    }

    void cursorUp()
    {
      if (yCursor == scrollWindowTop)
      {
        int y;
        for (y = scrollWindowBottom; y > scrollWindowTop; --y)
          for (int x = 0; x < screenWd; ++x)
            screen[x, y] = screen[x, y - 1];

        for (int x = 0; x < screenWd; ++x)
          screen[x, y] = ' ';

        vt100Redraw();
      }
      else
      {
        --yCursor;
        vt100Redraw();
      }
    }

    void vt100Print(char c)
    {
      c = (char)(c & 0x7F);

      if (escapeState != EscapeState.Idle)
      {
        escapeSeq(c);
        return;
      }

      switch (c)
      {
        case '\a':
          Console.Beep();
          break;

        case '\b':
          if (xCursor > 0)
          {
            --xCursor;
            screen[xCursor, yCursor] = ' ';
            vt100Redraw();
          }
          break;

        case '\r':
          xCursor = 0;
          setVt100Cursor();
          break;

        case '\n':
          cursorDown();
          break;

        case esc:
          escapeSeq(c);
          break;

        case < ' ':
          break;      // ignore other control characters

        default:
          if (xCursor < screenWd)
          {
            screen[xCursor++, yCursor] = c;
            vt100Redraw();
          }
          break;
      }
    }

    // Escape sequence processing
    enum EscapeState
    {
      Idle,
      Escape,
      FirstArg,
      SecondArg
    }

    int firstArg = 0, secondArg = 0;
    EscapeState escapeState = EscapeState.Idle;

    void escapeSeq(char c)
    {
      switch (escapeState)
      {
        case EscapeState.Idle:
          firstArg = 0;
          secondArg = 0;
          escapeState = EscapeState.Escape;
          break;

        case EscapeState.Escape:
          escapeState = EscapeState.Idle;
          switch (c)
          {
            case '[':
              escapeState = EscapeState.FirstArg;
              break;

            case 'D':
              cursorDown();
              break;

            case 'M':
              cursorUp();
              break;

            case 'c':
              clearScreen();
              scrollWindowTop = 0;
              scrollWindowBottom = screenHt - 1;
              break;
          }
          break;

        case EscapeState.FirstArg:
          if (c == ';')
            escapeState = EscapeState.SecondArg;
          else if (char.IsAsciiDigit(c))
            firstArg = firstArg * 10 + (c - '0');
          else
            goto escEx;
          break;

        case EscapeState.SecondArg:
          if (char.IsAsciiDigit(c))
            secondArg = secondArg * 10 + (c - '0');
          else
            goto escEx;
          break;
      }

      return;

    escEx:
      switch (c)
      {
        case 'J':
          if (firstArg == 2)
            clearScreen();
          break;

        case 'K':
          if (firstArg == 0)
          {
            for (int x = xCursor; x < screenWd; ++x)
              screen[x, yCursor] = ' ';
            vt100Redraw();
          }
          break;

        case 'H':
          xCursor = Math.Min(Math.Max(secondArg, 1), screenWd) - 1;
          yCursor = Math.Min(Math.Max(firstArg , 1), screenHt) - 1;
          vt100Redraw();
          break;

        case 'r':
          scrollWindowTop    = Math.Min(Math.Max(firstArg , 1), screenHt) - 1;
          scrollWindowBottom = Math.Min(Math.Max(secondArg, 1), screenHt) - 1;
          break;
      }

      escapeState = EscapeState.Idle;
    }

    // **************
    // *            *
    // *  Keyboard  *
    // *            *
    // **************

    void kbFlagChanged(bool flag)
    {
      kbShape.FillColor = flag ? Color.LightBlue : Color.Black;
    }

    bool alt;

    string escapeChars = string.Empty;

    private void paperText_KeyDown(object sender, KeyEventArgs e)
    {
      bool alt = (e.Modifiers & Keys.Alt) != 0;
      bool ctrl = (e.Modifiers & Keys.Control) != 0;

      byte[] ks = new byte[256];
      GetKeyboardState(ks);

      // Remove the ALT bit so ToUnicode gives the normal character
      ks[(int)Keys.Menu] = 0;

      uint vk = (uint)e.KeyCode;
      uint sc = MapVirtualKey(vk, 0);

      StringBuilder sb = new StringBuilder(4);

      int result = ToUnicode(vk, sc, ks, sb, sb.Capacity, 0);

      if (result > 0 && sb.Length > 0)
      {
        char c = sb[0];

        if (c == '\b')
          c = (char)0x7F;

        if (c == ' ' & ctrl)
          c = (char)0;

        if (alt)
        {
          escapeChars += c;
          c = esc;
          PDP8.CallMeReal(actionId, 1.0e5);
        }

        if (vt100Check.Checked)
          keyboard.Ascii = c;
        else
          keyboard.Ascii = c = (char)(c | 0x80);

        kbLabel.Text = PDP8.ToOctal(c, 3);
      }

      e.Handled = true;
      e.SuppressKeyPress = true;
    }

    private void paperText_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = true;
    }

    void escape()
    {
      char c = escapeChars[0];
      kbLabel.Text = PDP8.ToOctal(c, 3);
      keyboard.Ascii = c;
      escapeChars = escapeChars.Substring(1);
      if (escapeChars.Length > 0)
        PDP8.CallMeReal(actionId, 1.0e5);
    }
  }

  public class TeletypeRichTextBox : RichTextBox
  {
    public TeletypeRichTextBox()
    {
      ReadOnly = true;
      TabStop = false;
      AcceptsTab = true;        // allow TAB to reach KeyDown
      DetectUrls = false;
    }

    // Intercept CTRL+E/L/J/R before RICHEDIT swallows them
    protected override bool ProcessCmdKey(ref Message m, Keys keyData)
    {
      if (keyData == (Keys.Control | Keys.E) ||
          keyData == (Keys.Control | Keys.L) ||
          keyData == (Keys.Control | Keys.J) ||
          keyData == (Keys.Control | Keys.R))
      {
        // Manually raise KeyDown so your existing handler sees it
        OnKeyDown(new KeyEventArgs(keyData));
        return true;    // swallow before RICHEDIT eats it
      }

      return base.ProcessCmdKey(ref m, keyData);
    }
  }

}
