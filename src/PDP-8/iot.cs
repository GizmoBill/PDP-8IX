// ****************
// *              *
// *  I/O System  *
// *              *
// ****************


// ***********************
// *                     *
// *  IOT Abstract Base  *
// *                     *
// ***********************

using Accessibility;
using CSharpCommon;
using Microsoft.VisualBasic.Devices;
using PDP_8;
using System.Diagnostics;
using System.Security.Policy;
using System.Text;
using static PDP8;
using static XmlLiteNode;

public abstract class IODevice
{
  public abstract void Reset();

  public abstract XmlLiteNode State { get; set; }

  public abstract string XmlTag { get; }

  public abstract void Iot(int controlBits, ref int bus, out bool skip);
}

// *****************
// *               *
// *  Null Device  *
// *               *
// *****************

// Null devices are put in the device table for internal I/O instructions so
// devices can't override them.
public class NullDevice : IODevice
{
  public override XmlLiteNode State
  {
    get { return null; }
    set { }
  }

  public override string XmlTag => string.Empty;

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;
  }

  public override void Reset() { }
}

// ****************************
// *                          *
// *  Hardware Configuration  *
// *                          *
// ****************************

public struct DeviceConfiguration
{
  public int iotAddress;
  public int channel;
  public int maskBit;

  public DeviceConfiguration(int iotAddress, int channel, int maskBit)
  {
    this.channel = channel;
    this.iotAddress = iotAddress;
    this.maskBit = maskBit;
  }
}

// *****************
// *               *
// *  Clock Queue  *
// *               *
// *****************

public class ClockQueue
{
  private double time;

  private List<(double, string)> queue = new List<(double, string)>();

  string xmlTag;

  public ClockQueue(string xmlTag)
  {
    this.xmlTag = xmlTag;
  }

  public double Time
  {
    get { return time; }
    set
    {
      time = value;
      while (queue.Count > 0 && queue[queue.Count - 1].Item1 <= time)
      {
        Action action = ResolveAction(queue[queue.Count - 1].Item2);
        queue.RemoveAt(queue.Count - 1);
        action.Invoke();
      }
    }
  }

  public void CallMe(string id, double fromNow)
  {
    int i;
    for (i = 0; i < queue.Count; ++i)
      if (queue[i].Item2 == id)
      {
        queue.RemoveAt(i);
        break;
      }

    double wakeTime = Time + fromNow;
    for (i = 0; i < queue.Count && queue[i].Item1 > wakeTime; ++i) ;
    queue.Insert(i, (wakeTime, id));
  }

  public XmlLiteNode State
  {
    get
    {
      string s = string.Format("time = {0};\n", time);
      foreach ((double, string) item in queue)
        s += string.Format("{0} = {1};\n", item.Item2, item.Item1);
      return new XmlLiteNode(xmlTag, s);
    }

    set
    {
      CheckTag(value, xmlTag);

      ParamHolder ph = new ParamHolder(value.Value);
      time = double.Parse(ph[0]);
      for (int i = 1; i < ph.Count; ++i)
        queue.Add((double.Parse(ph[i]), ph.Name(i)));
    }
  }

  public void Reset()
  {
    queue.Clear();
  }
}

// **************
// *            *
// *  I/O Flag  *
// *            *
// **************

public class IOFlag
{
  private bool flag;
  private int channel;

  private static IOFlag[] allFlags = new IOFlag[PriorityInterrupt.NumChannels];

  public IOFlag(int channel)
  {
    if (allFlags[channel] != null)
      throw new Exception(string.Format("Interrupt channel {0} conflict", channel));
    this.channel = channel;
    allFlags[channel] = this;
  }

  public bool Flag
  {
    get => flag;
    set
    {
      if (flag != value)
      {
        if (value)
          PDP8.PriorityInterrupt.RequestInterrupt(channel);
        else
          PDP8.PriorityInterrupt.ClearInterrupt(channel);
        flag = value;
      }
    }
  }

  public static XmlLiteNode FullState
  {
    get
    {
      string s = string.Format("mask = {0};\n", PDP8.PriorityInterrupt.Mask);

      for (int i = 0; i < PriorityInterrupt.NumChannels; ++i)
        if (allFlags[i] != null)
          s += string.Format("flag{0} = {1};\n", i, allFlags[i].Flag);

      return new XmlLiteNode("allFlags", s);
    }

    set
    {
      CheckTag(value, "allFlags");

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
      {
        string tag = ph.Name(i);
        switch (tag)
        {
          case "mask":
            PDP8.PriorityInterrupt.Mask = int.Parse(ph[i]);
            break;

          default:
            if (tag.Length < 5 || tag.Substring(0, 4) != "flag")
              throw new Exception(string.Format("Unknown FullState parameter {0}", tag));

            int index = int.Parse(tag.Substring(4));
            allFlags[index].Flag = bool.Parse(ph[i]);
            break;
        }
      }
    }
  }
}

// ****************
// *              *
// *  Break Port  *
// *              *
// ****************

public class BreakPort
{
  public int PortNumber { get; private set; }

  public bool ThreeCycle { get; private set; }

  public bool IncMBR {  get; private set; }

  public bool WCOverflow { get; set; }

  public int MemAddr { get; set; }

  public int MemData { get; set; }

  public int MemField {  get; set; }

  public bool DataWrite {  get; set; }

  public bool IncCAInhibit { get; set; }

  public bool Brq
  {
    get { return Cpu.GetBreak(PortNumber); }
    set { Cpu.SetBreak(PortNumber, value); }
  }

  public void Reset()
  {
    WCOverflow = false;
    Brq = false;
  }

  public BreakPort(int portNum, bool threeCycle, bool incMBR)
  {
    PortNumber = portNum;
    ThreeCycle = threeCycle;
    IncMBR = incMBR;
    RegisterBreakPort(this);
  }

  public XmlLiteNode State
  {
    get
    {
      string s = string.Empty;
      s += string.Format("wcOverflow = {0};\n", WCOverflow);
      s += string.Format("memAddr = {0};\n", MemAddr);
      s += string.Format("memData = {0};\n", MemData);
      s += string.Format("memField = {0};\n", MemField);
      s += string.Format("dataWrite = {0};\n", DataWrite);
      s += string.Format("incCAInhibit = {0};\n", IncCAInhibit);
      s += string.Format("brq = {0};\n", Brq);

      return new XmlLiteNode("breakPort", s);
    }

    set
    {
      CheckTag(value, "breakPort");

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i <  ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "wcOverflow":
            WCOverflow = bool.Parse(ph[i]);
            break;

          case "memAddr":
            MemAddr = int.Parse(ph[i]);
            break;

          case "memData":
            MemData = int.Parse(ph[i]);
            break;

          case "memField":
            MemField = int.Parse(ph[i]);
            break;

          case "dataWrite":
            DataWrite = bool.Parse(ph[i]);
            break;

          case "incCAInhibit":
            IncCAInhibit = bool.Parse(ph[i]);
            break;

          case "brq":
            Brq = bool.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown breakport parameter " + ph.Name(i));
        }
    }
  }
}

// *******************************
// *                             *
// *  Priority Interrupt Device  *
// *                             *
// *******************************

public class PriorityInterrupt : IODevice
{
  public const int NumChannels = 16;

  private int flags = 0;

  // 1 means interrupt channel masked off. Public set for IOFlag.FullState
  int mask = 0;
  public int Mask { get { return mask; } set { setMask(value); } }

  public PriorityInterrupt()
  {
    RegisterDevice(FromOctal("56"), this);
  }

  public override XmlLiteNode State
  {
    get
    {
      return new XmlLiteNode(XmlTag, string.Format("mask = {0};", Mask));
    }

    set
    {
      CheckTag(value, XmlTag);

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "mask":
            setMask(int.Parse(ph[i]));
            break;

          default:
            throw new Exception("Unknown priInt parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => "priInt";

  private void setMask(int m)
  {
    int changes = m ^ Mask;
    for (int channel = 0; channel < NumChannels; ++channel)
    {
      int bit = GetMaskBit(channel);
      if ((changes & bit) == 0 | (flags & bit) == 0)
        continue;

      if ((m & bit) == 0)
        Cpu.RequestInterrupt();
      else
        Cpu.ClearInterrupt();
    }
    mask = m;
  }

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    switch (controlBits)
    {
      case 1:
        setMask(bus);
        break;

      case 2:
        bus = Mask;
        break;

      case 4:
        bus = 0;
        for (int channel = NumChannels - 1; channel >= 0; --channel)
        {
          int bit = GetMaskBit(channel);
          if ((~Mask & flags & bit) != 0)
          {
            bus = channel;
            break;
          }
        }
        break;

      default:
        Cpu.IllegalInstr();
        break;
    }

    skip = false;
  }

  public override void Reset()
  {
    // other devices handle flags;
    setMask(0);
  }

  public void RequestInterrupt(int channel)
  {
    int bit = GetMaskBit(channel);
    if ((flags & bit) != 0)
      throw new InvalidOperationException(string.Format("Redundant interrupt request on channel {0}", channel));
    flags |= bit;
    if ((~Mask & bit) != 0)
      Cpu.RequestInterrupt();
  }

  public void ClearInterrupt(int channel)
  {
    int bit = GetMaskBit(channel);
    if ((flags & bit) == 0)
      throw new InvalidOperationException(string.Format("Redundant interrupt clear on channel {0}", channel));
    if ((~Mask & bit) != 0)
      Cpu.ClearInterrupt();
    flags &= ~bit;
  }
}

// *****************************
// *                           *
// *  Character Output Device  *
// *                           *
// *****************************

public class CharOut : IODevice
{
  private IOFlag ready;

  public Action<char> PrintOne { get; set; }

  private Action<bool> flagChanged;

  private string actionId;

  public CharOut(int ioAddr, Action<char> printOne, Action<bool> flagChanged)
  {
    RegisterDevice(ioAddr, this);

    actionId = ToOctal(ioAddr, 2);
    RegisterAction(actionId, printDone);

    ready = new IOFlag(GetChannel(ioAddr));
    this.PrintOne = printOne;
    this.flagChanged = flagChanged;
  }

  public override XmlLiteNode State
  {
    get { return new XmlLiteNode(XmlTag, string.Format("flag = {0};", ready.Flag)); }
    set
    {
      CheckTag(value, XmlTag);

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "flag":
            ready.Flag = bool.Parse(ph[i]);
            flagChanged.Invoke(ready.Flag);
            break;

          default:
            throw new Exception("Unknown charOut parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => "charOut" + actionId;

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    if ((controlBits & 1) != 0)
      skip = ready.Flag;
    else
      skip = false;

    if ((controlBits & 2) != 0)
    {
      ready.Flag = false;
      flagChanged.Invoke(ready.Flag);
    }

    if ((controlBits & 4) != 0)
    {
      PrintOne.Invoke((char)bus);
      CallMeReal(actionId, PrintTime);
    }
  }

  private void printDone()
  {
    ready.Flag = true;
    flagChanged.Invoke(ready.Flag);
  }

  public override void Reset()
  {
    ready.Flag = false;
    flagChanged.Invoke(ready.Flag);
  }

  // microsecs
  public double PrintTime { get; set; } = 1.0e5;
}

// ****************************
// *                          *
// *  Character Input Device  *
// *                          *
// ****************************

public class CharIn : IODevice
{
  private IOFlag ready;

  private char ascii;

  private Action<bool> flagChanged;

  string xmlTag;

  public CharIn(int ioAddr, Action<bool> flagCHanged)
  {
    RegisterDevice(ioAddr, this);
    ready = new IOFlag(GetChannel(ioAddr));
    this.flagChanged = flagCHanged;

    xmlTag = "charIn" + ToOctal(ioAddr, 2);
  }

  public char Ascii
  {
    get { return ascii; }
    set
    {
      ascii = value;
      ready.Flag = true;
      flagChanged.Invoke(ready.Flag);
    }
  }

  public override XmlLiteNode State
  {
    get
    {
      string s = string.Format("flag = {0};\nascii = {1};\n", ready.Flag, (int)ascii);
      return new XmlLiteNode(XmlTag, s);
    }

    set
    {
      CheckTag(value, XmlTag);

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "flag":
            ready.Flag = bool.Parse(ph[i]);
            flagChanged.Invoke(ready.Flag);
            break;

          case "ascii":
            ascii = (char)int.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown charIn parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => xmlTag;

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    if ((controlBits & 1) != 0)
      skip = ready.Flag;
    else
      skip = false;

    if ((controlBits & 2) != 0)
    {
      ready.Flag = false;
      bus = 0;
      flagChanged.Invoke(ready.Flag);
    }

    if ((controlBits & 4) != 0)
      bus |= Ascii;
  }

  public override void Reset()
  {
    ready.Flag = false;
    flagChanged.Invoke(ready.Flag);
  }
}

// *****************
// *               *
// *  RK05 Device  *
// *               *
// *****************

public class RK05 : IODevice
{
  private const int diskWords = 1662976;

  RK05Form rk05Form = null;

  private IOFlag ready;

  private BreakPort breakPort;

  private short[,] diskImages = new short[4, diskWords];

  private bool[] driveChanged = new bool[4];

  public bool GetChanged(int driveNum)
  {
    return driveChanged[driveNum];
  }

  private int status, command, diskAddress, memAddress, diskIndex, wordCount, driveNum;

  private const string actionId = "74";

  public RK05(RK05Form rk05Form)
  {
    RegisterDevice(FromOctal("74"), this);
    RegisterAction(actionId, wakeup);

    ready = new IOFlag(GetChannel(FromOctal("74")));
    breakPort = new BreakPort(1, false, false);
    this.rk05Form = rk05Form;
  }

  public override XmlLiteNode State
  {
    get
    {
      XmlLiteNode state = new XmlLiteNode(XmlTag);

      string s = string.Format("flag = {0};\n", ready.Flag);
      s += "changed = ";
      foreach (bool b in driveChanged)
        s += b.ToString() + ',';
      s += ";\n";
      state.Children.Add(new XmlLiteNode("state", s));

      state.Children.Add(breakPort.State);
      return state;
    }

    set
    {
      CheckTag(value, XmlTag);

      ParamHolder ph = new ParamHolder(value["state"].Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "flag":
            ready.Flag = bool.Parse(ph[i]);
            break;

          case "changed":
            string[] s = ph[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
            for (int drive = 0; drive < 4; ++drive)
              driveChanged[i] = bool.Parse(s[drive]);
            break;

          default:
            throw new Exception("Unknown rk05 parameter " + ph.Name(i));
        }

      breakPort.State = value["breakPort"];
    }
  }

  public override string XmlTag => "rk05";

  private void setIrq()
  {
    ready.Flag = (status & 0x800) != 0 & (command & 0x100) != 0;
  }

  private void clearStatus()
  {
    status = 0;
    setIrq();
    breakPort.Reset();
  }

  private void setChanged(int driveNum, bool changed)
  {
    driveChanged[driveNum] = changed;
    rk05Form.SetChanged(driveNum, changed);
  }

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;
    switch (controlBits)
    {
      case 0:
        break;

      case 1:
        skip = (status & 0x800) != 0;
        break;

      case 2:
        int bits = bus & 3;
        bus = 0;
        if (bits == 0 | bits == 3)
          clearStatus();
        break;

      case 3:
        diskAddress = bus;
        bus = 0;

        if ((command & 0x400) == 0)
        {
          wordCount = (command & 0x040) == 0 ? 256 : 128;
          diskIndex = (diskAddress + ((command & 1) << 12)) << 8;
          driveNum = (command >> 1) & 3;

          breakPort.MemField = (command >> 3) & 7;
          breakPort.DataWrite = (command & 0x800) == 0;
          if (breakPort.DataWrite)
            CPU.Add12(ref memAddress, -1);
          else
          {
            breakPort.MemAddr = memAddress;
            breakPort.Brq = true;
          }

        }
        else if ((command & 0xE00) == 0x600)
        {
          // seek only
          status = 0x800;
          setIrq();
        }

        CallMeCycles(actionId, (int)Math.Round(5000 / 1.5));  // 5ms seek delay
        break;

      case 4:
        memAddress = bus;
        bus = 0;
        break;

      case 5:
        bus = status;
        break;

      case 6:
        command = bus;
        bus = 0;
        clearStatus();
        break;

      case 7:
        // Maintenance functions
        break;
    }
  }

  void wakeup()
  {
    int transferCycles = (int)Math.Round(20.0 / 1.5);

    if (breakPort.Brq)
      // break cycle not done, try again
      CallMeCycles(actionId, transferCycles);
    else
    {
      CPU.Add12(ref memAddress);
      breakPort.MemAddr = memAddress;

      if (breakPort.DataWrite)
        breakPort.MemData = diskImages[driveNum, diskIndex];
      else
      {
        diskImages[driveNum, diskIndex] = (short)breakPort.MemData;
        setChanged(driveNum, true);
      }

      ++diskIndex;
      --wordCount;

      if (wordCount > 0 | breakPort.DataWrite)
        breakPort.Brq = true;

      if (wordCount == 0)
      {
        status = 0x800;
        setIrq();
        if (breakPort.DataWrite)
          CPU.Add12(ref memAddress);
        else
        {
          if ((command & 0x40) != 0)
            for (int i = 0; i < 128; ++i, ++diskIndex)
              diskImages[driveNum, diskIndex] = 0;
          // RK05.setModified driveNum, True
        }

      }
      else
        CallMeCycles(actionId, transferCycles);
    }
  }

  public override void Reset()
  {
    clearStatus();
    command = 0;
    diskAddress = 0;
    memAddress = 0;
  }

  public void LoadDisk(int drive, string filename)
  {
    if (!File.Exists(filename))
      throw new Exception(string.Format("Disk file {0} does not exist", filename));

    byte[] data = File.ReadAllBytes(filename);

    int i;
    for (i = 0; i < Math.Min(data.Length / 2, diskWords); ++i)
      diskImages[drive, i] = (short)(data[2 * i] | (data[2 * i + 1] << 8) & 0xFFF);

    for (; i < diskWords; ++i)
      diskImages[drive, i] = 0;

    setChanged(drive, false);
  }

  public void SaveDisk(int drive, string filename)
  {
    int wordsUsed = 0;
    for (wordsUsed = diskWords; wordsUsed > 0 && diskImages[drive, wordsUsed - 1] == 0; --wordsUsed);

    wordsUsed = (wordsUsed + 0xFF) & ~0xFF;

    byte[] data = new byte[2 * wordsUsed];
    for (int i = 0; i < wordsUsed; ++i)
    {
      data[2 * i    ] = (byte)(diskImages[drive, i] & 0xFF);
      data[2 * i + 1] = (byte)(diskImages[drive, i] >> 8);
    }

    File.WriteAllBytes(filename, data);

    setChanged(drive, false);
  }
}

// ***********
// *         *
// *  Clock  *
// *         *
// ***********

// Tick every 125 ms
public class RealTimeClock : IODevice
{
  private const double interval = 125000.0;

  private IOFlag ready;

  private const string actionId = "37";

  public RealTimeClock()
  {
    RegisterDevice(FromOctal("37"), this);
    RegisterAction(actionId, wakeup);

    ready = new IOFlag(GetChannel(FromOctal("37")));
  }

  public override XmlLiteNode State
  {
    get { return new XmlLiteNode(XmlTag, string.Format("flag = {0};", ready.Flag)); }
    set
    {
      CheckTag(value, XmlTag);

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "flag":
            ready.Flag = bool.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown clock parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => "clock";

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;

    if (controlBits == 2)
      ready.Flag = false;
    else
      Cpu.IllegalInstr();
  }

  private void wakeup()
  {
    ready.Flag = true;
    CallMeReal(actionId, interval);
  }

  public override void Reset()
  {
    ready.Flag = false;
    CallMeReal(actionId, interval);
  }
}

// ********************
// *                  *
// *  Interval Timer  *
// *                  *
// ********************

public class IntervalTimer : IODevice
{
  private IOFlag ready;

  bool interruptEnable = false;
  bool timeout = false;

  private const string actionId = "17";

  public IntervalTimer()
  {
    RegisterDevice(FromOctal("17"), this);
    RegisterAction(actionId, wakeup);

    ready = new IOFlag(GetChannel(FromOctal("17")));
  }

  public override XmlLiteNode State
  {
    get
    {
      string s = string.Empty;
      s += string.Format("flag = {0};\n", ready.Flag);
      s += string.Format("intEnable = {0};\n", interruptEnable);
      s += string.Format("timeout = {0};\n", timeout);
      return new XmlLiteNode("timer", s);
    }
    set
    {
      CheckTag(value, XmlTag); ;

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "flag":
            ready.Flag = bool.Parse(ph[i]);
            break;

          case "intEnable":
            interruptEnable = bool.Parse(ph[i]);
            break;

          case "timeout":
            timeout = bool.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown timer parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => "timer";
  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;
    
    if ((controlBits & 1) != 0)
    {
      timeout = false;
      ready.Flag = false;
      interruptEnable = false;

      int interval = bus & 0x7FF;
      if (interval > 0)
      {
        interruptEnable = (bus >> 11) != 0;
        CallMeReal(actionId, interval * 1000.0);
      }
    }

    if ((controlBits & 2) != 0)
      bus = 0;

    if ((controlBits & 4) != 0)
      skip = timeout;
  }

  private void wakeup()
  {
    timeout = true;
    if (interruptEnable)
      ready.Flag = true;
  }

  public override void Reset()
  {
    ready.Flag = false;
    timeout = false;
  }
}

// *********************
// *                   *
// *  Supervisor Call  *
// *                   *
// *********************

public class SupervisorCall : IODevice
{
  private IOFlag svcReady, tsvcReady;

  public SupervisorCall()
  {
    RegisterDevice(FromOctal("57"), this);
    svcReady = new IOFlag(GetChannel(FromOctal("57")));
    tsvcReady = new IOFlag(GetChannel(FromOctal("157")));
    Reset();
  }

  public override XmlLiteNode State
  {
    get
    {
      string s = string.Format("svc = {0};\ntsvc = {1};\n", svcReady.Flag, tsvcReady.Flag);
      return new XmlLiteNode(XmlTag, s);
    }

    set
    {
      CheckTag(value, XmlTag);

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "svc":
            svcReady.Flag = bool.Parse(ph[i]);
            break;

          case "tsvc":
            tsvcReady.Flag = bool.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown svc parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => "svc";

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;
    switch (controlBits)
    {
      case 1:
        svcReady.Flag = true;
        Cpu.EventRecorder.Record();
        if (!Cpu.ION | Cpu.IRQ == 0)
          illegalSVC();
        break;

      case 2:
        // unknown
        break;

      case 3:
        svcReady.Flag = false;
        break;

      case 4:
        tsvcReady.Flag = !tsvcReady.Flag;
        if (tsvcReady.Flag & (!Cpu.ION | Cpu.IRQ == 0))
          illegalSVC();
        break;

      default:
        Cpu.IllegalInstr();
        break;
    }
  }

  private void illegalSVC()
  {
    Cpu.Run = false;
    Cpu.IntSuppress = true;
    MessageBox.Show(string.Format("SVC with interrupts off at {0}",
                                  ToOctal(Cpu.CurrentPC)));
  }

  public override void Reset()
  {
    svcReady.Flag = false;
    tsvcReady.Flag = false;
  }
}

// *******************
// *                 *
// *  DEC AF01A A/D  *
// *                 *
// *******************

public class AF01A : IODevice
{
  HRPForm hrp;

  int channel = 0;

  public AF01A(HRPForm hrp)
  {
    RegisterDevice(FromOctal("53"), this);
    RegisterDevice(FromOctal("54"), this);
    this.hrp = hrp;
  }

  public override XmlLiteNode State
  {
    get
    {
      string s = string.Format("channel = {0};\n", channel);
      return new XmlLiteNode(XmlTag, s);
    }

    set
    {
      CheckTag(value, XmlTag);

      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "channel":
            channel = int.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown A/D parameter " + ph.Name(i));
        }

    }
  }

  public override string XmlTag => "aToD";

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;

    switch ((Cpu.MBR >> 3) & 0x3F)
    {
      case 43:
        // Skip on flag, always ready
        if ((controlBits & 1) != 0)
          skip = true;

        // Start conversion
        if ((controlBits & 2) != 0)
          ;

        // Read
        if ((controlBits & 4) != 0)
          bus |= hrp.Dial(channel);

        break;

      case 44:
        // Clear channel
        if ((controlBits & 1) != 0)
          channel = 0;

        // Set channel
        if ((controlBits & 2) != 0)
          channel = bus & 0x3F;

        // Increment channel
        if ((controlBits & 4) != 0)
          channel = (channel + 1) & 0x3F;

        break;

      default:
        throw new Exception(string.Format("Internal A/D error {0}", ToOctal(Cpu.MBR)));
    }
  }

  public override void Reset()
  {
  }
}

// ********************************
// *                              *
// *  Hurricane Research Project  *
// *                              *
// ********************************
//
// Hardware from Air Force, modified and augmented as needed by MIT.

public class HRP : IODevice
{
  private HRPForm hrp;

  int wr66Low_;
  int wr66High_;
  int wr73AzV_;
  int wr73El_;
  int osw_;
  int sevenSeg_;

  int wr66Low
  {
    get => wr66Low_;
    set
    {
      wr66Low_ = value;

      hrp.wr66AzVelLabel.Text = ((wr66Low & 0x0FF) - 128).ToString();
    }
  }

  int wr66High
  {
    get => wr66High_;
    set
    {
      wr66High_ = value;

      hrp.wr66ElVelLabel.Text = (((wr66High >> 4) & 0x0F0) | (wr66Low >> 8)).ToString();
    }
  }

  int wr73AzV
  {
    get => wr73AzV_;
    set
    {
      wr73AzV_ = value;

      hrp.wr73AzVelLabel.Text = (512 - wr73AzV).ToString();
    }
  }

  int wr73El
  {
    get => wr73El_;
    set
    {
      wr73El_ = value;

      hrp.wr73ElSetLabel.Text = (wr73El * 360.0 / 4096.0 - 115.0).ToString();
    }
  }

  int osw
  {
    get => osw_;
    set
    {
      osw_ = value;

      hrp.oswRadarSelLabel.Text = (osw & 0x10) != 0 ? "WR73" : "WR66";
    }
  }

  int sevenSeg
  {
    get => sevenSeg_;
    set
    {
      sevenSeg_ = value;

      hrp.sevenSegLabel.Text = sevenSeg.ToString();
    }
  }

  int eswSelect = 0;

  public HRP(HRPForm hrpForm)
  {
    hrp = hrpForm;

    RegisterDevice(FromOctal("11"), this);    // INA, read WR66 az, el
    RegisterDevice(FromOctal("12"), this);    // DNS, read WR73 az, el
    RegisterDevice(FromOctal("16"), this);    // TI-980 interface, MIT
    RegisterDevice(FromOctal("31"), this);    // FRE, WR66 S/D (ignored)
    RegisterDevice(FromOctal("35"), this);    // INB, WR73 az, el (ignored)
    RegisterDevice(FromOctal("36"), this);    // ESR, external switch register
    RegisterDevice(FromOctal("41"), this);    // TL2, one-digit dial and pushbutton
    RegisterDevice(FromOctal("42"), this);    // WR73 set elevation, MIT
    RegisterDevice(FromOctal("44"), this);    // WR73 azimuth velocity, MIT
    RegisterDevice(FromOctal("45"), this);    // OSW, output switch word
    RegisterDevice(FromOctal("51"), this);    // WR66 control, MIT

    Reset();
  }

  public override XmlLiteNode State
  {
    get
    {
      string s = string.Format("wr66Low = {0};\nwr66High = {1};\n", wr66Low, wr66High);
      s += string.Format("wr73AzV = {0};\nwr73El = {1};\n", wr73AzV, wr73El);
      s += string.Format("osw = {0};\nseg7 = {1};\n", osw, sevenSeg);
      s += string.Format("eswSel = {0};\n", eswSelect);
      return new XmlLiteNode(XmlTag, s);
    }

    set
    {
      CheckTag(value, XmlTag);
      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "wr66Low":
            wr66Low = int.Parse(ph[i]);
            break;

          case "wr66High":
            wr66High = int.Parse(ph[i]);
            break;

          case "wr73AzV":
            wr73AzV = int.Parse(ph[i]);
            break;

          case "wr73El":
            wr73El = int.Parse(ph[i]);
            break;

          case "osw":
            osw = int.Parse(ph[i]);
            break;

          case "seg7":
            sevenSeg = int.Parse(ph[i]);
            break;

          case "eswSel":
            eswSelect = int.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown hprDevice parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => "hrpDevices";

  // WR66 AZ and EL come from 14-bit synchro-to-digital converters producing binary angles.
  // the HRPForm properties Az66 and El66 return 14-bit binary angles.
  //
  // WR73 AZ and EL come from binary-coded-decimal angles in tenths degree. The HRPForm
  // properties Az73 and El73 return BCD angles in tenths degree using 4 4-bit fields.
  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;

    switch (ToOctal((Cpu.MBR >> 3) & 0x3F, 2))
    {
      case "11":
        // INA reads WR66 az and el
        switch (controlBits)
        {
          case 1:
            // Latch S/D
            break;

          case 2:
            // Read low 12 bits of WR66 AZ
            bus = hrp.Az66 & 0xFFF;
            break;

          case 3:
            // Read low 12 bits of WR66 EL
            bus = hrp.El66 & 0xFFF;
            break;

          case 7:
            // Read high 2 bits of WR66 EL (bits 8-9) and AZ (bits 10-11)
            bus = (hrp.El66 & 0x3000) >> 10 | (hrp.Az66 & 0x3000) >> 12;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "12":
        // WR73 azimuth and elevation
        // Here 0 is tenths digit, 1 is unit, 2 is tens, 3 is hundreds
        // AZ     DNS 2        DNS 7         EL    DNS 7       DNS 3
        //    332222111100  00----------       ----------33 222211110000
        switch (controlBits)
        {
          case 2:
            bus = hrp.Az73 >> 2;
            break;

          case 3:
            bus = hrp.El73 & 0xFFF;
            break;

          case 7:
            bus = ((hrp.Az73 << 10) & 0xFFF) | (hrp.El73 >> 12);
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "16":
        switch (controlBits)
        {
          case 1:
            // TIR, red from TI-980, unused
            break;

          case 2:
            // TIW, write to TI, skip if successful. Always skip to avoid hanging
            // in a wait loop at 23673. Wait loop at 15662 times out
            skip = true;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "31":
        // FRE
        switch (controlBits)
        {
          case 4:
            // release WR66 S/D
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "35":
        // INB unneeded WR73 az el instructions
        switch (controlBits)
        {
          case 1:
            // latch (bus = 1) release (bus = 1) WR73 S/D
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "36":
        // External switch register
        switch (controlBits)
        {
          case 2:
            switch (eswSelect)
            {
              case 0x800:
                bus = hrp.ESR1;
                break;

              case 0x008:
                bus = hrp.ESR2;
                break;

              default:
                Cpu.IllegalInstr();
                break;
            }
            break;

          case 5:
            eswSelect = bus;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "41":
        // TL2 (one-digit dial and switch)
        // Skip on switch pressed
        if ((controlBits & 1) != 0)
          skip = hrp.TLSClicked;

        if ((controlBits & 2) != 0)
          bus = 0;

        // Read switch
        if ((controlBits & 4) != 0)
          bus |= hrp.TLS;

        break;

      case "42":
        switch (controlBits)
        {
          case 2:
            // PEL73, position WR73 elevation
            wr73El = bus;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "44":
        // INI
        switch (controlBits)
        {
          case 1:
            // set WR73 AZ velocity
            wr73AzV = bus;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "45":
        // OSW (radar select, 2-digit 7-segment display)
        switch (controlBits)
        {
          case 1:
            osw = bus;
            break;

          case 2:
            int highDigit = (bus & 0x0F0) >> 4;
            int lowDigit = bus & 0x00F;
            sevenSeg = highDigit * 10 + lowDigit;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "51":
        // WR66 interface
        switch (controlBits)
        {
          // The reads are apparently unused
          case 4:
            // read high
            bus = 0;
            break;

          case 5:
            // read low
            bus = 0;
            break;

          // source comment: /SEND WR66 SERVO INFO FROM EVH4, EL4AV
          case 6:
            // write low
            wr66Low = bus;
            bus = 0;
            break;

          case 7:
            // write high
            wr66High = bus;
            bus = 0;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      default:
        throw new Exception(string.Format("Internal HRP error {0}", ToOctal(Cpu.MBR)));
    }
  }

  public override void Reset()
  {
    wr66Low = 0;
    wr66High = 0;
    wr73AzV = 0;
    wr73El = 0;
    osw = 0;
    sevenSeg = 0;
  }
}

// ****************
// *              *
// *  Integrator  *
// *              *
// ****************

public class Integrator : IODevice
{
  private HRPForm hrp;

  int icw1_;
  int icw2_;

  int icw1
  {
    get => icw1_;
    set
    {
      icw1_ = value;

      hrp.intSkipLabel.Text = binskip.ToString();
      hrp.intResLabel.Text = binres.ToString("f1");
    }
  }

  int icw2
  {
    get => icw2_;
    set
    {
      icw2_ = value;

      hrp.intRadarSelLabel.Text = wr73Selected ? "WR73" : "WR66";
      hrp.intTNPLabel.Text = ((icw2 >> 7) & 7).ToString();
      hrp.intPulsesLabel.Text = (icw2 & 0x7F).ToString();
    }
  }

  private IOFlag integratorFlag = new IOFlag(GetChannel(FromOctal("33")));

  GaussRandom grand = new GaussRandom();

  int binskip { get { return icw1 >> 3; } }

  double binres { get { return (icw1 & 7) / 2.0; } }

  bool wr73Selected { get { return (icw2 & 0x400) == 0; } }

  // Synthetic radar data. A elliptical Gaussian centered at (x0, y0), at angle ang0,
  // with stDev (xSigma, ySigma). The IOT instructions set range and angle, with
  // range updated as bins are read. 
  double range;
  double angle;

  // Elliptical Gaussian parameters
  double x0 = -40.0;
  double y0 = 10;
  double ang0 = MathUtil.Radians(15.0);
  double xSigma = 6.0;
  double ySigma = 60.0;

  // Retrieve the synthetic bin at the current range and angle.
  int bin
  {
    get
    {
      // Elliptical Gauss, 0 <= z <= 1
      double x = range * Math.Cos(angle + ang0);
      double y = range * Math.Sin(angle + ang0);
      double z = MathUtil.square((x - x0) / xSigma) + MathUtil.square((y - y0) / ySigma);
      z = Math.Exp(-0.5 * z);

      // Add some noise, correct for power loss at range (emperical, not realistic),
      // convert to integer bin value
      z += grand.NextDouble();
      z -= 0.2 * Math.Log10(Math.Max(range, 1));
      z = Math.Max(z, 0);
      return (int)(z * 768.0) + 192;
    }
  }

  public Integrator(HRPForm hrpForm)
  {
    hrp = hrpForm;

    RegisterDevice(FromOctal("32"), this);    // SRA, load integrator control words
    RegisterDevice(FromOctal("33"), this);    // RVI, read integrator bins

    RegisterAction("integrator", integratorDone);

    grand.Mean = 0;
    grand.Sigma = 0.02;

    Reset();

    //test();
  }

  public override XmlLiteNode State
  {
    get
    {
      string s = string.Format("integratorFlag = {0};\n", integratorFlag.Flag);
      s += string.Format("icw1 = {0};\nicw2 = {1};\n", icw1, icw2);
      return new XmlLiteNode(XmlTag, s);
    }

    set
    {
      CheckTag(value, XmlTag);
      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "integratorFlag":
            integratorFlag.Flag = bool.Parse(ph[i]);
            break;

          case "icw1":
            icw1 = int.Parse(ph[i]);
            break;

          case "icw2":
            icw2 = int.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown integrator parameter " + ph.Name(i));
        }
    }
  }

  public override string XmlTag => "integrator";

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;

    switch (ToOctal((Cpu.MBR >> 3) & 0x3F, 2))
    {
      case "32":
        // SRA instructions. 2 and 6 each used once in ITGST to load
        // integrator params, including a yet unknown start bit
        switch (controlBits)
        {
          case 2:
            // ICW1
            icw1 = bus;
            break;

          case 6:
            // ICW2
            icw2 = bus;
            range = binskip * binres;
            decimal az = wr73Selected ? hrp.wr73AzNumeric.Value : hrp.wr66AzNumeric.Value;
            angle = MathUtil.Radians(MathUtil.mod((double)(90 - az), 360));

            CallMeReal("integrator", (icw2 & 0x07F) * 4000.0);  // pulses at 250 PRF
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      case "33":
        // RVI instructions. 2 and 4 each used once to read the integrator.
        // There's probably an unused skip on flag
        switch (controlBits)
        {
          case 2:
            // Clear flag
            integratorFlag.Flag = false;
            break;

          case 4:
            // Read next bin
            bus = bin;
            range += binres;
            break;

          default:
            Cpu.IllegalInstr();
            break;
        }
        break;

      default:
        throw new Exception(string.Format("Internal integrator error {0}", ToOctal(Cpu.MBR)));
    }
  }

  void integratorDone()
  {
    integratorFlag.Flag = true;
  }

  public override void Reset()
  {
    integratorFlag.Flag = false;

    icw1 = FromOctal("0121");
    icw2 = FromOctal("6240");
  }

  void test()
  {
    Bitmap img = new Bitmap(250, 250);

    for (int y = 0; y < img.Height; ++y)
      for (int x = 0; x < img.Width; ++x)
      {
        range = MathUtil.EuclidDist(x - 0.5 * img.Width, y - 0.5 * img.Height);
        angle = Math.Atan2(y - 0.5 * img.Height, x - 0.5 * img.Width);
        int z = bin >> 2;
        img.SetPixel(x, y, Color.FromArgb(z, z, z));
      }

    img.Save("testRadarImg.bmp");
  }
}

// *********************************
// *                               *
// *  Tektronix 611 Storage Scope  *
// *                               *
// *********************************

// x and y scope coordinates are 10-bit values
public class Tek611 : IODevice
{
  Tek611Form tek611;

  int xCoord, yCoord;

  public Tek611(Tek611Form tek611)
  {
    RegisterDevice(5, this);
    RegisterDevice(6, this);
    this.tek611 = tek611;
  }

  public override XmlLiteNode State
  {
    get
    {
      string s = string.Format("x = {0};\ny = {1};\n", xCoord, yCoord);
      return new XmlLiteNode(XmlTag, s);
    }

    set
    {
      CheckTag(value, XmlTag);
      ParamHolder ph = new ParamHolder(value.Value);
      for (int i = 0; i < ph.Count; ++i)
        switch (ph.Name(i))
        {
          case "x":
            xCoord = int.Parse(ph[i]);
            break;

          case "y":
            yCoord = int.Parse(ph[i]);
            break;

          default:
            throw new Exception("Unknown 611 scope parameter " + ph.Name(i));
            break;
        }
    }
  }

  public override string XmlTag => "tek611";

  public override void Iot(int controlBits, ref int bus, out bool skip)
  {
    skip = false;

    switch ((Cpu.MBR >> 3) & 0x3F)
    {
      case 5:
        if ((controlBits & 1) != 0)
          eraseScope();

        if ((controlBits & 2) != 0)
          xCoord = bus & 0x3FF;

        if ((controlBits & 4) != 0)
          plotPoint();

        break;

      case 6:
        if ((controlBits & 1) != 0)
          // skip if scope still erasing, which it never is
          ;

        if ((controlBits & 2) != 0)
          yCoord = bus & 0x3FF;

        if ((controlBits & 4) != 0)
          plotPoint();

        break;

      default:
        throw new Exception(string.Format("Internal Tek611 error {0}", ToOctal(Cpu.MBR)));
    }
  }

  public override void Reset()
  {
    xCoord = yCoord = 0;
    eraseScope();
  }

  void eraseScope()
  {
    tek611.Erase();
  }

  void plotPoint()
  {
    tek611.Plot(xCoord, yCoord);
  }
}