using CSharpCommon;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Policy;
using System.Windows.Forms;

using static PDP8;

// *********************
// *                   *
// *  PDP-8/I Console  *
// *                   *
// *********************

namespace PDP_8
{
  public partial class ConsoleForm : Form
  {
    string docFile_ = null;
    string documentFilename
    {
      get => docFile_;
      set
      {
        docFile_ = value;
        if (docFile_ == null)
          this.Text = "Untitled";
        else
          this.Text = Path.GetFileNameWithoutExtension(docFile_);
      }
    }

    // Forms
    private ASR38 tty1;

    private ASR38 tty2;

    private RK05Form rk05Form;

    private HRPForm hrpForm = new HRPForm();

    private Tek611Form tek611Form = new Tek611Form();

    private FrontPanel frontPanel = new FrontPanel();

    private Listing listingForm = new Listing();

    // *****************
    // *               *
    // *  Constructor  *
    // *               *
    // *****************

    public ConsoleForm()
    {
      InitializeComponent();

      SetConsole(this);

      tty1 = new ASR38();
      tty1.SetIO(3, 4);

      tty2 = new ASR38();
      tty2.SetIO(FromOctal("13"), FromOctal("14"));

      rk05Form = new RK05Form();

      documentFilename = null;

      updateCheapPanel();
    }

    // *********************
    // *                   *
    // *  Switch Register  *
    // *                   *
    // *********************

    // Read and convert on text changed so access is fast

    private void switchText_TextChanged(object sender, EventArgs e)
    {
      if (switchText.Text.Length > 0)
        FullSwitches = FromOctal(switchText.Text);
      else
        FullSwitches = 0;

      frontPanel.SwitchRegister = FullSwitches;
    }

    public int SwitchRegister
    {
      get { return FullSwitches & 0xFFF; }
      set
      {
        switchText.Text = ToOctal(value, 6);
      }
    }

    public int FullSwitches { get; private set; } = 0;

    // ***************
    // *             *
    // *  File Menu  *
    // *             *
    // ***************

    // Force upper case and CR/LF line endings
    private void convertToPAL8ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "PAL-8 Files|*.pal8";
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        string src = File.ReadAllText(ofd.FileName);
        string dst = string.Empty;

        bool cr = false;

        foreach (char c in src)
        {
          char s = char.ToUpper(c);

          if (s == '\r')
            cr = true;
          else if (s == '\n')
          {
            dst += "\r\n";
            cr = false;
          }
          else
          {
            if (cr)
            {
              dst += "\r\n";
              cr = false;
            }
            dst += s;
          }
        }

        //dst += "$\r\n";

        string outfile = ofd.FileName.Substring(0, ofd.FileName.Length - 2);
        File.WriteAllText(outfile, dst);
      }
    }

    private void oS8ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Cpu.LoadAddress(0x200);
      Cpu.Deposit(FromOctal("6743"));
      Cpu.Deposit(FromOctal("6741"));
      Cpu.Deposit(FromOctal("5201"));
      Cpu.Deposit(FromOctal("5000"));
      Cpu.LoadAddress(0x200);
      startButton.PerformClick();
    }

    private void coreDumpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "Core Dump|*.txt";
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        File.WriteAllText(sfd.FileName, PDP8.Core.State.ToString());
      }
    }

    // ***********************************
    // *                                 *
    // *  Listing Commands in File Menu  *
    // *                                 *
    // ***********************************

    private void loadListingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      splitListing(false, false, true, true);
    }

    private void splitListingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      splitListing(true, true, false, false);
    }

    private void writeSourceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      splitListing(false, true, false, false);
    }

    private void writeBinaryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      splitListing(true, false, false, false);
    }

    private void readListingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      splitListing(false, false, false, true);
    }

    // ****************************
    // *                          *
    // *  Documents in File Menu  *
    // *                          *
    // ****************************

    public void Reset()
    {
      tty1.Reset();
      tty2.Reset();

      switchText.Text = "000000";
      documentFilename = null;

      cycleCount = 0;
      realTime = 0;
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MasterReset();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "PDP-8 Files|*.pdp8";
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        XmlLiteNode root = XmlLiteNode.ReadFromFile(ofd.FileName);
        PDP8.State = root["PDP-8"];
        documentFilename = ofd.FileName;
      }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (documentFilename != null)
      {
        XmlLiteNode root = PDP8.State;
        XmlWriter.WriteFile(root, documentFilename, true, 2);
      }
      else
        saveAsToolStripMenuItem_Click(sender, e);
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "PDP-8 Files|*.pdp8";
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        XmlLiteNode root = PDP8.State;
        XmlWriter.WriteFile(root, sfd.FileName, true, 2);
        documentFilename = sfd.FileName;
      }
    }

    // ******************
    // *                *
    // *  Options Menu  *
    // *                *
    // ******************

    private void x10ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      cycleTimeTarget = x10ToolStripMenuItem.Checked ? 0.15 : 1.5;
    }

    private void showFrontPanelToolStripMenuItem_Click(object sender, EventArgs e)
    {
      frontPanel.Show();
      frontPanel.BringToFront();
    }

    // ******************
    // *                *
    // *  Devices Menu  *
    // *                *
    // ******************

    private void aSR38ConsoleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tty1.Show();
      tty1.BringToFront();
    }

    private void tTY2ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tty2.Show();
      tty2.BringToFront();
    }

    private void rK05ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      rk05Form.Show();
      rk05Form.BringToFront();
    }

    private void hRPToolStripMenuItem_Click(object sender, EventArgs e)
    {
      hrpForm.Show();
      hrpForm.BringToFront();
    }

    private void tektronix611ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tek611Form.Show();
      tek611Form.BringToFront();
    }

    // ******************
    // *                *
    // *  Analyze Menu  *
    // *                *
    // ******************

    private void showListingToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      listingForm.Show();
      listingForm.BringToFront();
    }

    private void recordToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Cpu.EventRecorder.Enable = recordToolStripMenuItem.Checked;
    }

    private void recordOnlyBreaksToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Cpu.EventRecorder.OnlyBreaks = recordOnlyBreaksToolStripMenuItem.Checked;
    }

    private void firstEventToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Cpu.EventRecorder.next(0);
    }

    private void nextEventToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Cpu.EventRecorder.next(1);
    }

    private void previousEventToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Cpu.EventRecorder.next(-1);
    }

    private void clearEventsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Cpu.EventRecorder.Clear();
    }


    // ************************
    // *                      *
    // *  Update Cheap Panel  *
    // *                      *
    // ************************

    void updateCheapPanel()
    {
      pcLabel.Text = ToOctal((Cpu.CDF << 15) | (Cpu.CIF << 12) | Cpu.PC, 6);
      marLabel.Text = ToOctal(Cpu.MAR);
      mbrLabel.Text = ToOctal(Cpu.MBR);
      acLabel.Text = ToOctal(Cpu.AC + (Cpu.Link << 12), 5);
      mqLabel.Text = ToOctal(Cpu.MQ);
      cycleLabel.Text = Cpu.CurrentCycle.ToString();
      instrLabel.Text = ((CPU.Opcodes)Cpu.Instr).ToString().ToUpper();
      ionShape.FillColor = Cpu.ION ? Color.White : Color.Black;
      ionLabel.Text = ToOctal(Cpu.IONS, 2);
      xRegLabel.Text = ToOctal(Cpu.XReg);
      xModeLabel.Text = Cpu.XMode.ToString();
      irqLabel.Text = Cpu.IRQ.ToString();
      maskLabel.Text = ToOctal(PDP8.PriorityInterrupt.Mask);

      disasmLabel.Text = Cpu.Disassemble();
      cycleCountLabel.Text = MathUtil.AddCommas(cycleCount);
      realTimeLabel.Text = (realTime / 1.0e6).ToString("f3");
      eventCountLabel.Text = Cpu.EventRecorder.Index.ToString();

      if (!Cpu.Run)
        listingForm.ShowAddress(Cpu.CurrentPC);

      startButton.Enabled = !Cpu.Run;
      continueButton.Enabled = !Cpu.Run;
      stopButton.Enabled = Cpu.Run;
      stepButton.Enabled = !Cpu.Run;
      instrButton.Enabled = !Cpu.Run;
      loadAddrButton.Enabled = !Cpu.Run;
      examineButton.Enabled = !Cpu.Run;
      depositButton.Enabled = !Cpu.Run;
    }

    private void switchText_KeyDown(object sender, KeyEventArgs e)
    {
      if ((e.KeyValue < '0' | e.KeyValue > '7') &&
          e.KeyCode != Keys.Delete && e.KeyCode != Keys.Back && e.KeyCode != Keys.Left &&
          e.KeyCode != Keys.Right)
        e.SuppressKeyPress = true;
    }

    // **********************
    // *                    *
    // *  Console Switches  *
    // *                    *
    // **********************

    private void startButton_Click(object sender, EventArgs e)
    {
      Cpu.Reset();
      Cpu.Run = true;
    }

    private void continueButton_Click(object sender, EventArgs e)
    {
      Cpu.Run = true;
      Cpu.IntSuppress = false;
    }

    private void stopButton_Click(object sender, EventArgs e)
    {
      Cpu.Run = false;
      Cpu.IntSuppress = true;
      updateCheapPanel();
    }

    private void stepButton_Click(object sender, EventArgs e)
    {
      Cpu.Run = true;
      Cpu.Cycle();
      Cpu.Run = false;
      updateCheapPanel();
    }

    private void instrButton_Click(object sender, EventArgs e)
    {
      Cpu.Run = true;
      do
        Cpu.Cycle();
      while (Cpu.CurrentCycle != CPU.CycleState.fetch);
      Cpu.Run = false;
      updateCheapPanel();
    }

    private void loadAddrButton_Click(object sender, EventArgs e)
    {
      Cpu.LoadAddress(FullSwitches);
      updateCheapPanel();
    }

    private void examineButton_Click(object sender, EventArgs e)
    {
      Cpu.Examine();
      updateCheapPanel();
    }

    private void depositButton_Click(object sender, EventArgs e)
    {
      Cpu.Deposit(SwitchRegister);
      updateCheapPanel();
    }

    // *******************************************
    // *                                         *
    // *  Split Listing into Source and Binaary  *
    // *                                         *
    // *******************************************

    void splitListing(bool writeBinary, bool writeSource, bool loadCore, bool loadListing)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "PAL-8 Listing|*.ls*";
      if (loadCore && !writeSource)
        ofd.Filter += "|Windows Binary|*.bin|OS-8 Binary|*.BN";
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        string text = File.ReadAllText(ofd.FileName);

        List<string> binary = writeBinary ? new List<string>() : null;
        List<string> source = writeSource ? new List<string>() : null;
        List<string> extra = new List<string>();

        bool generateListing = loadListing &&
          Path.GetExtension(ofd.FileName).Substring(1, 2).Equals("ls", StringComparison.OrdinalIgnoreCase);

        Dictionary<int, int> addressMap = null;
        if (generateListing)
          addressMap = new Dictionary<int, int>();

        int blanklines = 0;
        int loadCount = 0;

        bool noPunch = false;

        int endLineIndex = 0;
        for (int i = 0; i < text.Length; i = endLineIndex + 1)
        {
          endLineIndex = text.IndexOf('\n', i);
          if (endLineIndex < 0)
            endLineIndex = text.Length;
          string s = text.Substring(i, endLineIndex - i).TrimEnd();

          if (s.Length == 0)
          {
            ++blanklines;
            continue;
          }

          switch (s[0])
          {
            case ' ':
              if (writeSource && s.Length > 13)
              {
                if (blanklines > 0)
                {
                  source.Add(string.Empty);
                  blanklines = 0;
                }
                source.Add(s.Substring(13));
              }

              if (s.Contains("NOPUNCH"))
                noPunch = true;
              if (s.Contains("ENPUNCH"))
                noPunch = false;
              break;

            case '/':
              if (writeSource && source.Count > 0 && s[s.Length - 2] != '-')
                source.Add("\f\r\n");
              blanklines = -1;    // eat the next one
              break;

            case >= '0' and <= '7':
              if (writeBinary && !noPunch && s.Length >= 11)
                binary.Add(s.Substring(0, 11));

              if (writeSource && s.Length > 13)
              {
                if (blanklines > 0)
                {
                  source.Add(string.Empty);
                  blanklines = 0;
                }
                source.Add(s.Substring(13));
              }

              if ((loadCore | generateListing) && !noPunch)
              {
                int address, data;
                try
                {
                  address = Convert.ToInt32(s.Substring(0, 5), 8);
                  data = Convert.ToInt32(s.Substring(7, 4), 8);
                }
                catch (Exception ex)
                {
                  extra.Add(s);
                  continue;
                }

                if (loadCore)
                {
                  PDP8.Core[address >> 12, address & 0xFFF] = data;
                  ++loadCount;
                }

                if (generateListing)
                  addressMap.Add(address, i);
              }
              break;

            default:
              extra.Add(s);
              break;
          }
        }

        string binext = ".bin";
        string srcext = ".pal8";
        string extext = ".txt";

        if (Path.GetExtension(ofd.FileName) == ".LS")
        {
          binext = ".BN";
          srcext = ".PA";
          extext = ".TX";
        }

        if (writeBinary)
        {
          string filename = Path.Combine(Path.GetDirectoryName(ofd.FileName),
                                         Path.GetFileNameWithoutExtension(ofd.FileName) + binext);
          File.WriteAllLines(filename, binary);
        }

        if (writeSource)
        {
          string filename = Path.Combine(Path.GetDirectoryName(ofd.FileName),
                                         Path.GetFileNameWithoutExtension(ofd.FileName) + srcext);
          File.WriteAllLines(filename, source);
        }

        if (extra.Count > 0 && (writeBinary | writeSource))
        {
          string filename = Path.Combine(Path.GetDirectoryName(ofd.FileName),
                                         Path.GetFileNameWithoutExtension(ofd.FileName) + extext);
          File.WriteAllLines(filename, extra);
        }

        if (generateListing)
          listingForm.Load(text, addressMap);

        if (loadCore)
          MessageBox.Show(string.Format("{0} locations loaded", loadCount));
        else
          MessageBox.Show("Done");
      }
    }

    // ***************
    // *             *
    // *  Run Timer  *
    // *             *
    // ***************

    // Times in microsecs
    const double runTimerInterval = 1.0e6 / 64.0;

    private double cycleTimeTarget = 1.5;

    double burstTime = runTimerInterval;   // microseconds, measured, filtered
    double runTimeFilter = (1.0 - 1 / Math.E) * runTimerInterval / 1.0e6; // one sec time constant

    private int cycleBurst { get { return (int)Math.Round(runTimerInterval / cycleTimeTarget); } }

    int runUpdateCounter = 0;

    long cycleCount = 0;
    double realTime = 0;

    int[] lightSampleCounts = { 3, 5, 7, 11 };

    private void runTimer_Tick(object sender, EventArgs e)
    {
      Stopwatch cycleTimer = new Stopwatch();
      cycleTimer.Start();

      int cycles;

      // Simulating the lights is expensive, so sample the CPU by a set of
      // periods to avoid getting in sync with a loop
      int lightSampleIndex = 0;
      int lightSampleCounter = 0;
      bool doLights = frontPanel.Visible;

      for (cycles = 0; cycles < cycleBurst; ++cycles)
      {
        Cpu.Cycle();

        if (doLights && ++lightSampleCounter == lightSampleCounts[lightSampleIndex])
        {
          frontPanel.ProcessLights();
          lightSampleCounter = 0;
          lightSampleIndex = (lightSampleIndex + 1) % lightSampleCounts.Length;
        }

        if (!Cpu.IntSuppress)
        {
          ++cycleCount;
          realTime += cycleTimeTarget;
          SetClockQueueTime(cycleCount, realTime);
        }

        // Stopwatch is slow
        if ((cycles % 64) == 0 && cycleTimer.Elapsed.TotalMicroseconds >= 0.98 * runTimerInterval)
          break;
      }
      cycleTimer.Stop();

      double t = cycleTimer.Elapsed.TotalMicroseconds;
      burstTime += (t - burstTime) * runTimeFilter;

      if (++runUpdateCounter == 8)
      {
        cpuCycleTimeLabel.Text = cycleTimeTarget.ToString("f3");
        burstCycleTimeLabel.Text = (burstTime / cycles).ToString("f3");
        busyTimeLabel.Text = (burstTime / runTimerInterval * 100.0).ToString("f1");

        updateCheapPanel();
        if (doLights)
          frontPanel.SetLights();
        runUpdateCounter = 0;
      }
    }

    // ***************************
    // *                         *
    // *  Get/Set Console State  *
    // *                         *
    // ***************************

    public XmlLiteNode CpuTime
    {
      get
      {
        string s = string.Format("switches = {0};\n", switchText.Text);
        s += string.Format("cycleCount = {0};\n", cycleCount);
        s += string.Format("realTime = {0};\n", realTime);
        return new XmlLiteNode("time", s); ;
      }

      set
      {
        CheckTag(value, "time");

        ParamHolder ph = new ParamHolder(value.Value);
        for (int i = 0; i < ph.Count; ++i)
          switch (ph.Name(i))
          {
            case "switches":
              switchText.Text = ph[i];
              break;

            case "cycleCount":
              cycleCount = long.Parse(ph[i]);
              break;

            case "realTime":
              realTime = double.Parse(ph[i]);
              break;

            default:
              throw new Exception("Unknown time parameter " + ph.Name(i));
          }
      }
    }

    public XmlLiteNode State
    {
      get
      {
        XmlLiteNode root = new XmlLiteNode("console");

        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(this, "consoleForm"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(tty1, "tty1Form"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(tty2, "tty2Form"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(rk05Form, "rk05Form"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(frontPanel, "frontPanel"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(hrpForm, "hrpForm"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(listingForm, "listing"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(tek611Form, "tek611"));

        root.Children.Add(CpuTime);
        root.Children.Add(tty1.State);
        root.Children.Add(tty2.State);

        if (documentFilename != null)
          root.Children.Add(new XmlLiteNode("docfile", documentFilename));

        return root;
      }

      set
      {
        CheckTag(value, "console");

        XmlLiteNode root = value;
        CSharpCommon.PreserveState.SetFormState(this, root, "consoleForm");
        CSharpCommon.PreserveState.SetFormState(tty1, root, "tty1Form");
        CSharpCommon.PreserveState.SetFormState(tty2, root, "tty2Form");
        CSharpCommon.PreserveState.SetFormState(rk05Form, root, "rk05Form");
        CSharpCommon.PreserveState.SetFormState(frontPanel, root, "frontPanel");
        CSharpCommon.PreserveState.SetFormState(hrpForm, root, "hrpForm");
        CSharpCommon.PreserveState.SetFormState(listingForm, root, "listing");
        CSharpCommon.PreserveState.SetFormState(tek611Form, root, "tek611");

        CpuTime = value["time"];
        tty1.State = value[tty1.XmlTag];
        tty2.State = value[tty2.XmlTag];

        XmlLiteNode docfile = root["docFile"];
        documentFilename = docfile != null ? docfile.Value : null;

      }
    }

    // ********************
    // *                  *
    // *  Preserve State  *
    // *                  *
    // ********************

    private string stateFile1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                             "PDP-8.txt");

    private string stateFile2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                             "PDP-8v2.txt");

    private void saveState()
    {
      CSharpCommon.XmlWriter.WriteFile(PDP8.State, stateFile2, false, 2);
    }

    private void restoreState(string file)
    {
      XmlLiteNode root = XmlLiteNode.ReadFromFile(file)["PDP-8"];
      PDP8.State = root;
    }

    private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing && documentFilename != null)
      {
        string msg = string.Format("Save changes to {0}?", Text);
        switch (MessageBox.Show(msg, "Save Changes", MessageBoxButtons.YesNoCancel))
        {
          case DialogResult.Yes:
            saveAsToolStripMenuItem.PerformClick();
            break;

          case DialogResult.No:
            break;

          case DialogResult.Cancel:
            e.Cancel = true;
            return;
        }
      }

      saveState();
      rk05Form.SaveDrives();
    }

    private void ConsoleForm_Shown(object sender, EventArgs e)
    {
      string[] args = Environment.GetCommandLineArgs();
      if (args.Length >= 2 && File.Exists(args[1]))
      {
        string file = args[1];
        documentFilename = args[1];
        restoreState(file);
      }
      else if (File.Exists(stateFile2))
        restoreState(stateFile2);
      else if (File.Exists(stateFile1))
      {
        XmlLiteNode root = XmlLiteNode.ReadFromFile(stateFile1)["PDP-8I"];
        CSharpCommon.PreserveState.SetFormState(this, root, "consoleForm");
        CSharpCommon.PreserveState.SetFormState(tty1, root, "tty1Form");
        CSharpCommon.PreserveState.SetFormState(tty2, root, "tty2Form");
        CSharpCommon.PreserveState.SetFormState(rk05Form, root, "rk05Form");
        CSharpCommon.PreserveState.SetFormState(frontPanel, root, "frontPanel");
        CSharpCommon.PreserveState.SetFormState(hrpForm, root, "hrpForm");
        CSharpCommon.PreserveState.SetFormState(listingForm, root, "listing");
      }

      runTimer.Enabled = true;
      rk05Form.LoadDrives();
    }

    private void ConsoleForm_Resize(object sender, EventArgs e)
    {
      if (Visible)
      {

      }
    }
  }
}
