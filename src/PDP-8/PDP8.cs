// *********************
// *                   *
// *  PDP-8/IX System  *
// *                   *
// *********************

using PDP_8;

public static class PDP8
{
  // **************
  // *            *
  // *  Hardware  *
  // *            *
  // **************

  public static CPU Cpu { get; private set; }

  public static Core Core { get; private set; }

  public static ConsoleForm Console {  get; private set; }

  public static IODevice[] Devices { get; private set; }

  public static BreakPort[] BreakPorts { get; private set; }

  public static PriorityInterrupt PriorityInterrupt { get; private set; }

  // ******************
  // *                *
  // *  Clock Queues  *
  // *                *
  // ******************

  private static ClockQueue cycleTimeQueue = new ClockQueue("cycleQ");
  private static ClockQueue realTimeQueue = new ClockQueue("timeQ");

  public static void SetClockQueueTime(long cycles, double microsecs)
  {
    cycleTimeQueue.Time = cycles;
    realTimeQueue.Time = microsecs;
  }

  public static void CallMeReal(string id, double microSecsFromNow)
  {
    realTimeQueue.CallMe(id, microSecsFromNow);
  }

  public static void CallMeCycles(string id, int cyclesFromNow)
  {
    cycleTimeQueue.CallMe(id, cyclesFromNow);
  }

  // ************************
  // *                      *
  // *  Static Constructor  *
  // *                      *
  // ************************

  static PDP8()
  {
    Devices = new IODevice[64];
    BreakPorts = new BreakPort[8];
    Core = new Core();

    // Put internal IOT addresses in device table for error checking
    IODevice nulldev = new NullDevice();
    Devices[0] = nulldev;           // ION/IOF
    for (int i = FromOctal("20"); i < FromOctal("27"); ++i)   // memory extension
      Devices[i] = nulldev;
    Devices[FromOctal("60")] = nulldev; // index register
    Devices[FromOctal("07")] = nulldev;

    Devices[FromOctal("56")] = PriorityInterrupt = new PriorityInterrupt();

    Cpu = new CPU();
  }

  // ***************
  // *             *
  // *  Utilities  *
  // *             *
  // ***************

  public static int FromOctal(string s)
  {
    return Convert.ToInt32(s, 8);
  }

  public static string ToOctal(int n, int digits = 4)
  {
    string s = Convert.ToString(n, 8);
    if (s.Length < digits)
      s = new string('0', digits - s.Length) + s;
    return s;
  }

  public static void CheckTag(XmlLiteNode node, string tag)
  {
    if (tag == null || node.Tag != tag)
      throw new Exception(string.Format("Mismatched XML tag {0}, expected {1}",
                                        node != null ? node.Tag : "null", tag));
  }

  // ******************
  // *                *
  // *  Registration  *
  // *                *
  // ******************

  public static void SetConsole(ConsoleForm console)
  {
    Console = console;
  }

  public static void RegisterDevice(int addr, IODevice device)
  {
    if (Devices[addr] != null)
      throw new ArgumentException("I/O device conflict");
    Devices[addr] = device;
  }

  public static void UnregisterDevice(int addr)
  {
    Devices[addr] = null;
  }

  public static void RegisterBreakPort(BreakPort bp)
  {
    if (BreakPorts[bp.PortNumber] != null)
      throw new Exception(string.Format("Break port {0} conflict", bp.PortNumber));
    BreakPorts[bp.PortNumber] = bp;
  }


  private static Dictionary<string, Action> actionRegistry = new Dictionary<string, Action>();

  public static void RegisterAction(string id, Action action)
  {
    if (actionRegistry.ContainsKey(id))
      throw new Exception("Duplicate action registry id" + id);
    actionRegistry[id] = action;
  }

  public static Action ResolveAction(string id)
  {
    if (!actionRegistry.ContainsKey(id))
      throw new Exception("Unknown action registry id" + id);
    return actionRegistry[id];
  }

  // ***************************
  // *                         *
  // *  Get/Set Machine State  *
  // *                         *
  // ***************************

  public static XmlLiteNode State
  {
    get
    {
      XmlLiteNode root = new XmlLiteNode("PDP-8");

      root.Children.Add(Console.State);
      root.Children.Add(Cpu.State);

      foreach (IODevice dev in Devices)
        if (dev != null)
        {
          XmlLiteNode state = dev.State;
          if (state != null)
            root.Children.Add(state);
        }

      root.Children.Add(cycleTimeQueue.State);
      root.Children.Add(realTimeQueue.State);

      root.Children.Add(Core.State);

      return root;
    }

    set
    {
      CheckTag(value, "PDP-8");

      Console.State = value["console"];
      Cpu.State = value["cpu"];

      foreach (IODevice dev in Devices)
        if (dev != null && dev.XmlTag != "")
          dev.State = value[dev.XmlTag];

      cycleTimeQueue.State = value["cycleQ"];
      realTimeQueue.State = value["timeQ"];

      Core.State = value["core"];
    }
  }

  // ****************************
  // *                          *
  // *  Hardware Configuration  *
  // *                          *
  // ****************************

  // Mask bits are little-endian numbered here. 0 is known to be clock. 3 and 6
  // are deduced to be unused and reader, respectively, based on the apparent
  // bit/channel pattern. Others are guesses, but the bit/chan pattern supports
  // them. SYSMSK=0111 initially masks off unused, reader, and clock. When clock
  // starts by field 1 code, bit 0 is masked on. Chan is priority. Note that
  // interrupt is disabled on a given channel if the corresponding mask bit is 1.
  // The interrupt will occcur when the mask bit is 0 and CPU interrupts are
  // on (ION).
  private static DeviceConfiguration[] hardwareConfiguration
    = { 
        // Weather Radar Configuration. Follows IHTAB.
        //                                 IOT  Chan Mask
        //                                ADDR  Prty Bit
        //                                        0   3     // unused
        new DeviceConfiguration(FromOctal("157"), 1,  2),   // DECTape (TSVC)
        new DeviceConfiguration(FromOctal("17"),  2,  1),   // interval timer
        new DeviceConfiguration(FromOctal("37"),  3,  0),   // clock
        new DeviceConfiguration(FromOctal("33"),  8, 11),   // integrator
        new DeviceConfiguration(FromOctal("14"),  9, 10),   // TTY2 printer
        new DeviceConfiguration(FromOctal("04"), 10,  9),   // TTY1 printer
        new DeviceConfiguration(FromOctal("02"), 11,  8),   // punch
        new DeviceConfiguration(FromOctal("03"), 12,  7),   // TTY1 keyboard
        new DeviceConfiguration(FromOctal("01"), 13,  6),   // paper tape reader, iotAddr is guess
        new DeviceConfiguration(FromOctal("13"), 14,  5),   // TTY2 keyboard
        new DeviceConfiguration(FromOctal("57"), 15,  4),   // supervisor call

        // Non-maskable interrupts
        new DeviceConfiguration(FromOctal("74"),  4, 12),   // RK05 has its own mask
      };

  public static int GetChannel(int iotAddr)
  {
    foreach (DeviceConfiguration config in hardwareConfiguration)
      if (config.iotAddress == iotAddr)
        return config.channel;
    throw new Exception(string.Format("IOT address {0} not in hardware config", ToOctal(iotAddr)));
  }

  public static int GetMaskBit(int channel)
  {
    foreach (DeviceConfiguration config in hardwareConfiguration)
      if (config.channel == channel)
        return 1 << config.maskBit;
    return 0;
  }
}