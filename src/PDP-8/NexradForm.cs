// ******************************
// *                            *
// *  Nexrad Radar Acquisition  *
// *                            *
// ******************************

// NexradParser.exe is a Windows console app that uses an open-source library,
// wrapped by code written mostly by Claude, to fetch, parse, and write to an
// XML file Nexrad Level 2 near-realtime weather radar data.The source is
// NexradParser.py (in a parallel project). The XML file is here read, compiled
// for fast execution, and made available to the Integrator I/O device. The
// Integrator asks for a ray of dbZ values at a specific azimuth and elevation,
// and code here finds the closet azimuth in the closest elevation sweep from
// the Nexrad rays.
//
// fetch(), running on the UI thread, inkoves NexradParser.exe as a separate
// Windows process and returns immediatly, so that the UI thread doesn't hang.
// Output from the exe (stdout and stderr) is monitored to learn the XML
// filename and report errors; messages are received on a threadpool thread and
// must not interact with Winforms, so they set status variables. When the exe
// exits, execution resumes in WaitForExitAsync() on a threadpool thread. The
// wait/resume pattern uses the SynchronizationContext mechanism. If the
// threadpool thread determines that the fetch was successful, it reads the
// XML and compiles it. This can take a couple of seconds, but the UI thread is
// undisturbed and the multi-core CPU doesn't break a sweat. Once complete
// a message is posted to the Winforms messge pump to at some point invoke
// exitHandler() on the UI thread.
//
// Compiling the XML means organizing the rays for fast access, expanding the
// run-length encoded values, and converting everything from text to binary
// values the Integrator expects. Organizing rays is further described below.

using System.Diagnostics;

using static PDP8;

namespace PDP_8
{
  public partial class NexradForm : Form
  {
    PPIForm ppi;

    const string ExeName = "NexradParser.exe";
    string nexradPath;
    string exePath;

    // *****************
    // *               *
    // *  Constructor  *
    // *               *
    // *****************

    public NexradForm(PPIForm ppi)
    {
      InitializeComponent();

      this.ppi = ppi;

      radarCombo.SelectedIndex = 69;  // Portland ME

      findNexradPaths();

      onExit += exitHandler;

      RegisterAction("nexrad", wakeup);
    }

    private void findNexradPaths()
    {
      nexradPath = null;
      exePath = null;

      // In the standalone version, the exe is in the same directory as PDP-8.exe
      if (File.Exists(ExeName))
      {
        nexradPath = ".";
        exePath = ExeName;
      }

      // Search parent directories for src
      string dir = Directory.GetCurrentDirectory();
      int p = dir.IndexOf("\\src", StringComparison.OrdinalIgnoreCase);
      if (p >= 0)
      {
        nexradPath = Path.Combine(dir.Substring(0, p + 4), "NexradParser");
        string s = Path.Combine(nexradPath, "dist", ExeName);
        if (File.Exists(s))
          exePath = s;
      }
    }

    // ***********************
    // *                     *
    // *  Fetch Nexrad Data  *
    // *                     *
    // ***********************

    Process process;                  // NexradParser.exe runs here
    string nexradDataFile;            // set by outputHandler from NexradParser stdout
    string errorMessage;              // set by errorHandler if NexradParser reports error
    bool warning;                     // errorMessage is a warning
    Action<int> onExit;               // set to exitHandler by constructor
    SynchronizationContext uiContext; // capture UI context
    string[] dataFiles;               // files that exist before geting new one, for keepCheck
    string captureRadarId;            // capture radarId in case it changes after starting
    DateTime timestamp;               // time of fetch

    // Compiles XML, holds the result, and finds rays from (az, el).
    public NexradRays nexradRays = new NexradRays();

    private string radarID
    {
      get { return radarCombo.Text.Substring(radarCombo.Text.Length - 4); }
    }

    private void fetch()
    {
      if (exePath == null)
      {
        fetchTimeLabel.Text = "Can't find exe";
        return;
      }

      if (process != null)
        return;

      setButtonStates();

      // Initialize so we can see what happened
      nexradDataFile = null;
      errorMessage = null;
      warning = false;
      dataFiles = Directory.GetFiles(Path.Combine(nexradPath, "radar_data"));
      captureRadarId = radarID;
      fetchTimeLabel.Text = "Fetching " + captureRadarId;

      // Start NexradParser.exe
      ProcessStartInfo psi = new ProcessStartInfo();
      psi.FileName = exePath;
      psi.Arguments = "--radar " + captureRadarId;
      psi.RedirectStandardOutput = true;
      psi.RedirectStandardError = true;
      psi.UseShellExecute = false;
      psi.CreateNoWindow = true;
      psi.WorkingDirectory = nexradPath;

      process = new Process { StartInfo = psi, EnableRaisingEvents = true };
      process.Start();
      process.OutputDataReceived += new DataReceivedEventHandler(outputHandler);
      process.ErrorDataReceived += new DataReceivedEventHandler(errorHandler);
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();

      // Set up to resume on exit and return immediatly
      uiContext = SynchronizationContext.Current;   // capture UI thread context
      _ = WaitForExitAsync();   // does not block UI thread
    }

    private async Task WaitForExitAsync()
    {
      // Running on threadpool thread
      await process.WaitForExitAsync().ConfigureAwait(false);

      // Still running on threadpool thread
      int code = process.ExitCode;
      process = null;

      // Process results on threadpool thread
      if (code == 0 && nexradDataFile != null && File.Exists(nexradDataFile))
      {
        XmlLiteNode root = XmlLiteNode.ReadFromFile(nexradDataFile)["nexrad_data"];
        timestamp = DateTime.Parse(root["header"]["timestamp"].Value);
        nexradRays.Compile(root);
      }
      else
      {
        if (errorMessage == null || warning)
          errorMessage = "Can't find XML file";
        code = -1;
      }

      // Marshal back to UI to signal done
      uiContext.Post(_ => { onExit.Invoke(code); }, null);
    }

    private void outputHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
      // Threadpool thread
      string s = outLine.Data;
      if (!string.IsNullOrEmpty(s))
      {
        if (s.StartsWith("[OK]"))
          nexradDataFile = Path.Combine(nexradPath, s.Substring(5));
      }
    }

    private void errorHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
      // Threadpool thread

      // Report only first error
      if (errorMessage != null && !warning)
        return;

      string s = outLine.Data;
      if (!string.IsNullOrEmpty(s))
      {
        int p = s.IndexOf("WARNING");
        if (p >= 0 && errorMessage == null)
        {
          errorMessage = s.Substring(p + 10);
          warning = true;
        }

        p = s.IndexOf("ERROR");
        if (p >= 0)
        {
          errorMessage = s.Substring(p + 8);
          warning = false;
        }
      }
    }

    private void exitHandler(int exitCode)
    {
      // UI thread
      if (exitCode == 0)
      {
        fetchTimeLabel.Text = timestamp.ToString("dd MMM yyyy HH:mm");

        if (!keepCheck.Checked)
        {
          string currentFile = Path.GetFileName(nexradDataFile);
          foreach (string path in dataFiles)
          {
            string file = Path.GetFileName(path);
            if (file != currentFile && file.StartsWith(captureRadarId))
              File.Delete(path);
          }
        }
      }
      else if (errorMessage != null)
        fetchTimeLabel.Text = errorMessage;
      else
        fetchTimeLabel.Text = "error, no message";

      setButtonStates();
    }

    public bool DataAvailable
    {
      get { return enableCheck.Checked && nexradRays.DataAvailable; }
    }

    void setButtonStates()
    {
      fetchButton.Enabled = process == null;
      showButton.Enabled = nexradRays.DataAvailable;
      loadButton.Enabled = process == null && !liveCheck.Checked;
    }

    // ********************************
    // *                              *
    // *  Rays for use by Integrator  *
    // *                              *
    // ********************************

    // Based on inspecting Nexrad data from a few sites, as reported by Py-ART:
    // Range bins (gates) are 0.25 kn. At low elevations the antenna sweeps
    // 0.5 deg per ray (presumably integration period), 720 rays/sweep, and
    // two full sweeps are taken, for a total of 1440 rays. Here those rays
    // are combined into a single sweep, sorted by azimuth. The elevation
    // reported in the sweep is actually just the elevation of the first ray,
    // and is not reliable. The mean elevation of a sweep is very reliable.
    // At higher elevations the antenna sweeps 1 deg/ray, one sweep/elevation,
    // total 360 rays.
    //
    // What this code assumes is that, after combining the two low elevation
    // sweeps, elevations are monotonically increasing. Bin size, azimuth
    // order, and rays/sweep are not assumed. 
    public class NexradRays
    {
      public bool DataAvailable
      {
        get { return rays != null; }
      }

      public void Compile(XmlLiteNode root)
      {
        // Runs on threadpool thread for fetch, UI thread for load. Not thread safe.
        XmlLiteNode headerNode = root["header"];
        XmlLiteNode sweepsNode = root["sweeps"];
        XmlLiteNode raysNode = root["rays"];

        const double kmPerNmi = 1.852;
        double kmPerBin = double.Parse(headerNode["bin_resolution_m"].Value) / 1000;
        double nmiPerBin = kmPerBin / kmPerNmi;
        int srcBinsPerRay = int.Parse(headerNode["number_of_gates"].Value);
        int dstBinsPerRay = (int)(srcBinsPerRay * nmiPerBin * 2); // dst bins are 0.5 nmi
        int numRays = int.Parse(headerNode["number_of_rays"].Value);

        double[] rawBins = new double[srcBinsPerRay];

        // Create local elevations, azimuths, and rays so that the integrator on
        // the UI thread can continue to have access to the current ones while
        // the new ones are being made. The new ones are switched in quickly under
        // a Mutex.

        // First pass, combine sweeps at same elevation
        List<(double, int, int)> trueSweeps = new List<(double, int, int)>();
        foreach (XmlLiteNode sweep in sweepsNode.Children)
        {
          int start = int.Parse(sweep["start_ray_index"].Value);
          int stop = int.Parse(sweep["end_ray_index"].Value);
          int n = stop - start + 1;
          double meanEl = 0;

          for (int i = start; i <= stop; ++i)
            meanEl += double.Parse(raysNode.Children[i]["elevation"].Value);
          meanEl /= n;

          int prev = trueSweeps.Count - 1;
          if (prev >= 0 && Math.Abs(trueSweeps[prev].Item1 - meanEl) < 0.1)
          {
            double newEl = 0.5 * (meanEl + trueSweeps[prev].Item1);
            int oldStart = trueSweeps[prev].Item2;
            trueSweeps.RemoveAt(prev);
            trueSweeps.Add((newEl, oldStart, stop));
          }
          else
            trueSweeps.Add((meanEl, start, stop));
        }

        // Create rays
        byte[][] rays = new byte[numRays][];
        for (int i = 0; i < numRays; ++i)
          rays[i] = new byte[dstBinsPerRay];

        // Make azimuths and rays
        List<(double, int)> elevations = new List<(double, int)>();
        List<(double, int)>[] azimuths = new List<(double, int)>[trueSweeps.Count];

        int azi = 0;    // index into azimuths

        foreach (var ts in trueSweeps)
        {
          elevations.Add((ts.Item1, azi));
          azimuths[azi] = new List<(double, int)>();

          // All rays in current trueSweep
          for (int r = ts.Item2; r <= ts.Item3; ++r)
          {
            XmlLiteNode ray = raysNode.Children[r];
            azimuths[azi].Add((double.Parse(ray["azimuth"].Value), r));

            // Unpack run-length data
            string[] rleBins = ray["dbz_values"].Value.Split(',');
            int j = 0;
            foreach (string b in rleBins)
              if (b[0] == 'z')
              {
                int runLen = int.Parse(b.Substring(1));
                while (--runLen >= 0)
                  rawBins[j++] = 0;
              }
              else
                rawBins[j++] = Math.Max(double.Parse(b), 0);

            // Write destination ray
            for (int b = 0; b < dstBinsPerRay; ++b)
            {
              int p = (int)Math.Round(0.5 * (b - 0.5) / nmiPerBin);
              int q = (int)Math.Round(0.5 * (b + 0.5) / nmiPerBin) + 1;
              p = Math.Max(p, 0);
              q = Math.Min(q, srcBinsPerRay);
              double z = 0;
              for (int k = p; k < q; ++k)
                z += rawBins[k];
              z /= q - p;

              rays[r][b] = (byte)Math.Round(z);
            }
          }

          azimuths[azi].Sort();
          ++azi;
        }


        // Switch in new ones
        mutex.WaitOne();
        this.elevations = elevations;
        this.azimuths = azimuths;
        this.rays = rays;
        mutex.ReleaseMutex();
      }

      public byte[] GetRay(double az, double el)
      {
        // Runs on UI thread
        mutex.WaitOne();
        int azIndex = find(el, elevations);
        int rayIndex = find(az, azimuths[azIndex]);
        byte[] ray = rays[rayIndex];
        mutex.ReleaseMutex();

        return ray;
      }

      private int find(double x, List<(double, int)> list)
      {
        int index = ~list.BinarySearch((x, -1));   // guarantee not found

        if (index == list.Count)
          --index;
        else if (index > 0 && x <= 0.5 * (list[index - 1].Item1 + list[index].Item1))
          --index;

        return list[index].Item2;
      }

      private Mutex mutex = new Mutex();

      private List<(double, int)> elevations;
      private List<(double, int)>[] azimuths;
      private byte[][] rays;
    }

    // ********************
    // *                  *
    // *  Display on PPI  *
    // *                  *
    // ********************

    private void ppiDisplay()
    {
      const double kmPerNmi = 1.852;
      int binSize = 2;    // 1, 2, or 4
      double nmiPerBin = binSize / 2;
      int skipBins = (int)Math.Round(250.0 / kmPerNmi / nmiPerBin) - 100;
      skipBins = Math.Max(skipBins, 0);
      int icw1 = (skipBins << 3) | binSize;

      for (int az = 0; az < 360; ++az)
      {
        int azimuth = az * 4096 / 360;
        byte[] ray = nexradRays.GetRay(az, 0.5);

        ppi.Ray(0xAAA);   // sync
        ppi.Ray(0);       // command
        ppi.Ray(azimuth);
        ppi.Ray(0);       // unknown
        ppi.Ray(icw1);
        for (int i = 0; i < 100; ++i)
          ppi.Ray(ray[(i + skipBins) * binSize]);
      }
    }

    // ************
    // *          *
    // *  Events  *
    // *          *
    // ************

    private void fetchButton_Click(object sender, EventArgs e)
    {
      fetch();
    }

    private void showButton_Click(object sender, EventArgs e)
    {
      if (nexradRays.DataAvailable)
        ppiDisplay();
    }

    private void loadButton_Click(object sender, EventArgs e)
    {
      // Load should be disabled if fetch is running or could run in live mode,
      // but be safe because nexradRays.Convert is not thread safe.
      if (liveCheck.Checked || process != null)
        return;

      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "Nexrad|*.xml";
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        fetchButton.Enabled = false;
        XmlLiteNode root = XmlLiteNode.ReadFromFile(ofd.FileName)["nexrad_data"];
        fetchTimeLabel.Text =
          DateTime.Parse(root["header"]["timestamp"].Value).ToString("dd MMM yyyy HH:mm");
        nexradRays.Compile(root);
        ppiDisplay();
        setButtonStates();
      }
    }

    private void liveCheck_CheckedChanged(object sender, EventArgs e)
    {
      setButtonStates();
      wakeup();
    }

    private void intervalNumeric_ValueChanged(object sender, EventArgs e)
    {
      if (liveCheck.Checked)
        CallMeReal("nexrad", (double)intervalNumeric.Value * 60.0e6);
    }

    private void radarCombo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void wakeup()
    {
      if (liveCheck.Checked)
      {
        CallMeReal("nexrad", (double)intervalNumeric.Value * 60.0e6);
        fetch();
      }
    }

    private void NexradForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }
  }
}
