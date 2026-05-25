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

    private HRPForm hrpForm;

    private Tek611Form tek611Form = new Tek611Form();

    private FrontPanel frontPanel = new FrontPanel();

    private Listing listingForm = new Listing();

    private DECtapeForm dectapeForm = new DECtapeForm();

    private PPIForm ppiForm;

    private NexradForm nexradForm;

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

      ppiForm = new PPIForm();
      nexradForm = new NexradForm(ppiForm);
      hrpForm = new HRPForm(ppiForm, nexradForm);

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

    private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
      loadListingToolStripMenuItem.Enabled = !Cpu.Run;
    }

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
      dectapeForm.Reset();

      switchText.Text = "000000";
      documentFilename = null;

      cycleCount = 0;
      realTime = 0;
      SyncCycleTimer();
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (offerToSave())
        return;
      MasterReset();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "PDP-8 Files|*.pdp8";
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        if (offerToSave())
          return;
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

    bool offerToSave()
    {
      if (documentFilename != null)
      {
        string msg = string.Format("Save changes to {0}?", Text);
        switch (MessageBox.Show(msg, "Save Changes", MessageBoxButtons.YesNoCancel))
        {
          case DialogResult.Yes:
            saveToolStripMenuItem.PerformClick();
            break;

          case DialogResult.No:
            break;

          case DialogResult.Cancel:
            return true;
        }
      }

      return false;
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

    private void dECtapeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      dectapeForm.Show();
      dectapeForm.BringToFront();
    }

    private void pPIToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ppiForm.Show();
      ppiForm.BringToFront();
    }

    private void nexradToolStripMenuItem_Click(object sender, EventArgs e)
    {
      nexradForm.Show();
      nexradForm.BringToFront();
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

                if (generateListing && s.Length > 13 && !addressMap.ContainsKey(address))
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
    //
    // The runTimer is the heartbeat of the simulator. The actions of the CPU
    // and all I/O devices are triggered by runTimer_Tick by calling Cpu.Cycle,
    // SetClockQueueTime, and ProcessLights. Every CPU cycle consumes
    // cycleTimeTarget us of simulated real time, 1.5 us or 0.15 us in 10x mode.
    // The realTime variable holds the number of simulated microseconds since
    // Reset, which may have heppened in a previous run of the simulator and
    // restored in the current run. Executing a cycle is usually much faster
    // than cycleTimeTarget, even in 10x mode. A Stopwatch is used to measure
    // actual real time, and runTimer_Tick insures that simulated realtime
    // matches actual realtime over human-detectable intervals by executing
    // cycles until simulated time catches up to actual time. The result is that
    // cycles are executed in bursts, typically around 10,000 cycles per timer
    // tick (or 100,000 in 10x mode). This is computationally very efficient and
    // allows the UI thread to be idle most of the time.

    // Times in microsecs
    private double cycleTimeTarget = 1.5;

    double burstTime = 0;         // Time to run the cycle burst, measured, filtered
    double timerPeriod = 0;       // measured, filtered timer interval
    double maxTimerPeriod = 0;    // running max measured time

    double runTimeFilter = (1.0 - 1 / Math.E) / 64; // one sec time constant at 64 Hz

    // Every 8 timer ticks, the front panels are updated
    int runUpdateCounter = 0;

    // These are for the clock queues
    long cycleCount = 0;
    double realTime;

    // Simulating the lights is expensive, so sample the CPU by a set of
    // periods to avoid getting in sync with a loop
    int[] lightSampleCounts = { 3, 5, 7, 11 };

    double cycleT0;     // offset of realTime wrt cycleTimer
    Stopwatch cycleTimer = new Stopwatch();

    double lastTime = 0;

    public void SyncCycleTimer()
    {
      lastTime = cycleT0 = realTime;
      cycleTimer.Reset();
      cycleTimer.Start();
    }

    private void runTimer_Tick(object sender, EventArgs e)
    {
      double t0 = cycleTimer.Elapsed.TotalMicroseconds + cycleT0;

      int lightSampleIndex = 0;
      int lightSampleCounter = 0;
      bool doLights = frontPanel.Visible;

      int cycles = 0;

      double cycleTimerTime = t0;

#if !DEBUG
      try
#endif
      {
        // Run until simulated realTime catches up with StopWatch time, but no longer
        // than 12.5 ms.
        while (realTime < cycleTimerTime && cycleTimerTime - t0 < 12500.0)
        {
          Cpu.Cycle();

          if (frontPanel.SingleStep ||
              frontPanel.SingleInstr && Cpu.CurrentCycle == CPU.CycleState.fetch)
          {
            Cpu.Run = false;
            Cpu.IntSuppress = true;
          }

          if (doLights && ++lightSampleCounter == lightSampleCounts[lightSampleIndex])
          {
            frontPanel.ProcessLights();
            lightSampleCounter = 0;
            lightSampleIndex = (lightSampleIndex + 1) % lightSampleCounts.Length;
          }

          // Stopwatch is slow
          if ((cycles % 64) == 0)
            cycleTimerTime = cycleTimer.Elapsed.TotalMicroseconds + cycleT0;

          ++cycles;

          if (!Cpu.IntSuppress)
          {
            ++cycleCount;
            realTime += cycleTimeTarget;
            SetClockQueueTime(cycleCount, realTime);
          }
          // When interrupts are suppressed (by the user via the console or front panel,
          // or by a breakpoint) realTime is not advancing. Do some cycles so that
          // breakPorts can run, panel lights work, measurements and displays make
          // sense. But no point in running until the 12.5 limit is reached, that just
          // heats up the host CPU. Note that when IntSuppress is removed, SyncCycleTimer
          // is called. For short duration suppression realTime could catch up quickly,
          // but IntSuppress can last indefinitely.
          else if (cycles == 1024)
            break;
        }
      }
#if !DEBUG
      catch (Exception ex)
      {
        MasterReset();
        MessageBox.Show(string.Format("{0}, resetting", ex.Message));
        return;
      }
#endif

      // Update filtered measurements
      burstTime += (cycleTimerTime - t0 - burstTime) * runTimeFilter;
      timerPeriod += (t0 - lastTime - timerPeriod) * runTimeFilter;
      maxTimerPeriod = Math.Max(maxTimerPeriod, t0 - lastTime);
      lastTime = t0;

      if (++runUpdateCounter == 8)
      {
        //cpuCycleTimeLabel.Text = cycleTimeTarget.ToString("f3");
        cpuCycleTimeLabel.Text = (timerPeriod / 1000.0).ToString("f1");
        //cpuCycleTimeLabel.Text = (maxTimerPeriod / 1000.0).ToString("f1");

        burstCycleTimeLabel.Text = (burstTime / cycles).ToString("f3");
        busyTimeLabel.Text = (burstTime / timerPeriod * 100.0).ToString("f1");

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
        if (CheckTag(value, "time"))
          return;

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
              SyncCycleTimer();
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
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(dectapeForm, "dectape"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(ppiForm, "ppiForm"));
        root.Children.Add(CSharpCommon.PreserveState.GetFormStateX(nexradForm, "nexradForm"));

        root.Children.Add(CpuTime);
        root.Children.Add(tty1.State);
        root.Children.Add(tty2.State);
        root.Children.Add(dectapeForm.State);
        rk05Form.SaveDrives();

        if (documentFilename != null)
          root.Children.Add(new XmlLiteNode("docfile", documentFilename));

        return root;
      }

      set
      {
        if (CheckTag(value, "console"))
          return;

        XmlLiteNode root = value;
        CSharpCommon.PreserveState.SetFormState(this, root, "consoleForm");
        CSharpCommon.PreserveState.SetFormState(tty1, root, "tty1Form");
        CSharpCommon.PreserveState.SetFormState(tty2, root, "tty2Form");
        CSharpCommon.PreserveState.SetFormState(rk05Form, root, "rk05Form");
        CSharpCommon.PreserveState.SetFormState(frontPanel, root, "frontPanel");
        CSharpCommon.PreserveState.SetFormState(hrpForm, root, "hrpForm");
        CSharpCommon.PreserveState.SetFormState(listingForm, root, "listing");
        CSharpCommon.PreserveState.SetFormState(tek611Form, root, "tek611");
        CSharpCommon.PreserveState.SetFormState(dectapeForm, root, "dectape");
        CSharpCommon.PreserveState.SetFormState(ppiForm, root, "ppiForm");
        CSharpCommon.PreserveState.SetFormState(nexradForm, root, "nexradForm");

        CpuTime = value["time"];
        tty1.State = value[tty1.XmlTag];
        tty2.State = value[tty2.XmlTag];
        dectapeForm.State = value[DECtapeForm.XmlTag];
        rk05Form.LoadDrives();

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
      if (e.CloseReason == CloseReason.UserClosing && offerToSave())
      {
        e.Cancel = true;
        return;
      }

      saveState();
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
        rk05Form.LoadDrives();
      }

      SyncCycleTimer();
      runTimer.Enabled = true;
    }
  }
}
