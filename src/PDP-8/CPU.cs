
// ***************
// *             *
// *  PDP-8 CPU  *
// *             *
// ***************

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.DirectoryServices.ActiveDirectory;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using CSharpCommon;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using static PDP8;

namespace PDP_8
{

  // **************************
  // *                        *
  // *  Magnetic Core Memory  *
  // *                        *
  // **************************

  // Hardware breakpoints can be added here
  public class Core
  {
    private short[,] core = new short[8, 4096];

    public int this[int field, int addr]
    {
      get
      {
        if ((uint)core[field, addr] > 0xFFF)
          throw new Exception(string.Format("Core[{0}, {1}] holds {2}",
                                            field, addr, ToOctal(core[field, addr])));

        int sw = PDP8.Console.FullSwitches;
        int breakPointOp = sw >> 15;
        if ((breakPointOp & 1) != 0 && (cycleType(addr) & 1) != 0)
        {
          if (breakPointOp == 1 && ((field << 12) | addr) == (sw & 0x7FFF))
            breakpoint();
          else if (breakPointOp == 5 && core[field, addr] == (sw & 0xFFF))
            breakpoint();
        }

        return core[field, addr];
      }

      set
      {
        if ((uint)value > 0xFFF)
          throw new Exception(string.Format("Trying to write {2} to Core[{0}, {1}]",
                                            field, addr, ToOctal(value)));

        int sw = PDP8.Console.FullSwitches;
        int breakPointOp = sw >> 15;
        if ((breakPointOp & 2) != 0 && (cycleType(addr) & 2) != 0)
        {
          if (breakPointOp == 2 && ((field << 12) | addr) == (sw & 0x7FFF))
            breakpoint();
          else if (breakPointOp == 6 && value == (sw & 0xFFF))
            breakpoint();
        }

        core[field, addr] = (short)value;
      }
    }

    public void Clear()
    {
      Array.Clear(core);
    }

    // 1 read, 2 write, 3 both
    private int cycleType(int addr)
    {
      int rwBits = 0;

      switch (Cpu.CurrentCycle)
      {
        case CPU.CycleState.fetch:
          rwBits = 1;
          break;

        case CPU.CycleState.defer:
          rwBits = 1;
          if ((addr & 0xFF8) == 8)
            rwBits = 3;               // auto-increment locations
          break;

        case CPU.CycleState.execute:
          if (Cpu.Instr <= 1)
            rwBits = 1;               // AND, TAD
          else if (Cpu.Instr == 2)
            rwBits = 3;               // ISZ
          else
            rwBits = 2;               // DCA, JMS
          break;

        case CPU.CycleState.wordCount:
          rwBits = 3;
          break;

        case CPU.CycleState.currentAddress:
          rwBits = 3;
          break;

        case CPU.CycleState.dataBreak:
          if (!Cpu.BreakPort.ThreeCycle & Cpu.BreakPort.IncMBR)
            rwBits = 3;
          else
            rwBits = Cpu.BreakPort.DataWrite ? 2 : 1;
          break;
      }

      return rwBits;
    }

    private void breakpoint()
    {
      if (Cpu.EventRecorder.Enable)
        Cpu.EventRecorder.Record();
      else
      {
        Cpu.Run = false;
        Cpu.IntSuppress = true;
      }
    }

    public XmlLiteNode State
    {
      get
      {
        string s = string.Empty;
        for (int field = 0; field < 8; ++field)
          for (int addr = 0; addr < 0x1000; addr += 8)
          {
            for (int i = 0; i < 8; ++i)
              if (core[field, addr + i] != 0)
                goto nonZero;
            continue;

          nonZero:
            s += string.Format("{0}{1}:", field, ToOctal(addr));
            for (int i = 0; i < 8; ++i)
              s += string.Format(" {0}", ToOctal(core[field, addr + i]));

            s += "\n";
          }

        return new XmlLiteNode("core", s);
      }

      set
      {
        CheckTag(value, "core");

        string s = value.Value;
        for (int i = 0; i < s.Length; i = s.IndexOf('\n', i) + 1)
        {
          // skip newlines and spaces
          while (char.IsWhiteSpace(s[i]))
            if (++i == s.Length)
              return;

          int field = int.Parse(s.Substring(i, 1));
          int addr = FromOctal(s.Substring(i + 1, 4));
          for (int j = 0; j < 8; ++j)
            core[field, addr + j] = (short)FromOctal(s.Substring(i + 5 * j + 7, 4));
        }
      }
    }
  }

  // ********************
  // *                  *
  // *  Event Recorder  *
  // *                  *
  // ********************

  public class EventRecorder
  {
    int saveLast = 1000;

    XmlLiteNode[] events;

    int currentEvent;
    int writeIndex;

    public EventRecorder()
    {
      events = new XmlLiteNode[saveLast];
      for (int i = 0; i < events.Length; ++i)
        events[i] = new XmlLiteNode();

      Clear();
    }

    public bool Enable { get; set; }

    public int Count { get; private set; }

    public int Index
    {
      get
      {
        return Count > 0 ? MathUtil.mod((writeIndex - 1) - currentEvent, Count) : 0;
      }
    }

    public void Clear()
    {
      foreach (XmlLiteNode n in events)
        n.Children.Clear();
      currentEvent = 0;
      Count = 0;
      writeIndex = 0;
    }

    public void Record()
    {
      if (Enable)
      {
        XmlLiteNode ev = events[writeIndex];
        ev.Children.Clear();
        ev.Children.Add(PDP8.Console.CpuTime);
        ev.Children.Add(Cpu.State);
        ev.Children.Add(IOFlag.FullState);

        writeIndex = (writeIndex + 1) % events.Length;
        Count = Math.Min(Count + 1, saveLast);
      }
    }

    public void next(int inc = 1)
    {
      if (Count == 0)
        return;

      if (inc == 0)
        inc = (writeIndex - 1) - currentEvent;

      currentEvent = MathUtil.mod(currentEvent + inc,  Count);

      Cpu.State = events[currentEvent]["cpu"];
      PDP8.Console.CpuTime = events[currentEvent]["time"];
      IOFlag.FullState = events[currentEvent]["allFlags"];

      Cpu.Run = false;
      Cpu.IntSuppress = true;
    }
  }

  // *****************
  // *               *
  // *  PDP-8/I CPU  *
  // *               *
  // *****************

  public class CPU
  {
    // ******************
    // *                *
    // *  Enumerations  *
    // *                *
    // ******************

    public enum Opcodes
    {
      and,
      tad,
      isz,
      dca,
      jms,
      jmp,
      iot,
      opr
    }

    public enum EaeCodes
    {
      idle,
      scl,
      muy,
      dvi,
      nmi,
      shl,
      asr,
      lsr
    }

    public enum CycleState
    {
      fetch,
      execute,
      defer,
      wordCount,
      currentAddress,
      dataBreak,
    }

    // ******************************
    // *                            *
    // *  Programmer-Visible State  *
    // *                            *
    // ******************************

    int pc;
    int ac;
    int link;
    int mq;
    int sc;
    int cif;
    int cdf;
    int xr;
    int xMode;

    // ********************
    // *                  *
    // *  Internal State  *
    // *                  *
    // ********************

    int mar;    // memory address register
    int mbr;    // memory buffer register
    int ir;     // instruction register (3-bit opcode)

    // Core reads and writebacks go to the field specified by mfr. mfr is set
    // to cif at the start of an instruction. In a defer cycle that is not
    // jmp or jms, it is set to cdf. The CDF instruction sets cdf immediatly.
    // The CIF instruction sets the ibr.
    // jmp direct: cif = ibr (fetch cycle)
    // jmp indirect: cif = ibr (execute cycle)
    // jms direct: mfr = ibr (fetch cycle)
    // jms indirect: cif = ibr (execute cycle)
    int ibr;    // instruction (field) buffer register
    int sfr;    // save field register, set on interrupt cdf bits 6-8, cif in bits 9-11
    int mfr;    // memory field register (cif or cdf for next cycle)

    // ion bit 0 is the ION flag, set/cleared by ION/IOF. Other bits are interrupt suppression
    // flags, so that interrupts are enabled when ion == 1. Suppression bits
    //    1   single instruction suppression for ION
    //    2   CIF suppression between CIF and JMP/JMS
    //    3   breakpoint/single instruction suppression
    int ion;
    int irq;    // interrupt request counter--number of devices currently requesting interrupt

    int brq;    // data break request bits

    BreakPort breakPort;

    bool run;

    CycleState cycle;

    EaeCodes eaeState;

    // *******************
    // *                 *
    // *  Get/Set State  *
    // *                 *
    // *******************

    public XmlLiteNode State
    {
      get
      {
        string s = string.Format("pc = {0};\n", ToOctal(pc));
        s += string.Format("ac = {0};\n", ToOctal(ac));
        s += string.Format("link = {0};\n", link);
        s += string.Format("mq = {0};\n", ToOctal(mq));
        s += string.Format("sc = {0};\n", sc);
        s += string.Format("cif = {0};\n", cif);
        s += string.Format("cdf = {0};\n", cdf);
        s += string.Format("xr = {0};\n", ToOctal(xr));
        s += string.Format("xMode = {0};\n", xMode);

        s += string.Format("mar = {0};\n", ToOctal(mar));
        s += string.Format("mbr = {0};\n", ToOctal(mbr));
        s += string.Format("ir = {0};\n", ir);
        s += string.Format("ibr = {0};\n", ibr);
        s += string.Format("mfr = {0};\n", mfr);
        s += string.Format("sfr = {0};\n", ToOctal(sfr, 2));

        s += string.Format("ion = {0};\n", ToOctal(ion, 2));
        s += string.Format("irq = {0};\n", irq);
        s += string.Format("brq = {0};\n", ToOctal(brq));

        s += string.Format("run = {0};\n", run);
        s += string.Format("cycle = {0};\n", (int)cycle);
        s += string.Format("eae = {0};\n", (int)eaeState);

        s += string.Format("pc0 = {0};\n", CurrentPC);

        return new XmlLiteNode("cpu", s);
      }

      set
      {
        CheckTag(value, "cpu");

        ParamHolder ph = new ParamHolder(value.Value);
        for (int i = 0; i < ph.Count; ++i)
          switch (ph.Name(i))
          {
            case "pc":
              pc = FromOctal(ph[i]);
              break;

            case "ac":
              ac = FromOctal(ph[i]);
              break;

            case "link":
              link = int.Parse(ph[i]);
              break;

            case "mq":
              mq = FromOctal(ph[i]);
              break;

            case "sc":
              sc = int.Parse(ph[i]);
              break;

            case "cif":
              cif = int.Parse(ph[i]);
              break;

            case "cdf":
              cdf = int.Parse(ph[i]);
              break;

            case "xr":
              xr = FromOctal(ph[i]);
              break;

            case "xMode":
              xMode = int.Parse(ph[i]);
              break;

            case "mar":
              mar = FromOctal(ph[i]);
              break;

            case "mbr":
              mbr = FromOctal(ph[i]);
              break;

            case "ir":
              ir = int.Parse(ph[i]);
              break;

            case "ibr":
              ibr = int.Parse(ph[i]);
              break;

            case "mfr":
              mfr = int.Parse(ph[i]);
              break;

            case "sfr":
              sfr = FromOctal(ph[i]);
              break;

            case "ion":
              ion = FromOctal(ph[i]);
              break;

            case "irq":
              // Don't restore irq, it is derived from the interrupt mask and the state
              // of all IOFlags.
              break;

            case "brq":
              // Don't restore brq, it is derived from the Breakports.
              break;

            case "run":
              run = bool.Parse(ph[i]);
              break;

            case "cycle":
              cycle = (CycleState)int.Parse(ph[i]);
              break;

            case "eae":
              eaeState = (EaeCodes)int.Parse(ph[i]);
              break;

            case "pc0":
              CurrentPC = int.Parse(ph[i]);
              break;

            default:
              throw new Exception("Unknown cpu parameter " + ph.Name(i));
          }
      }
    }

    // *******************
    // *                 *
    // *  Public Fields  *
    // *                 *
    // *******************

    public int PC { get { return pc; } }
    public int AC { get { return ac; } }
    public int Link { get { return link; } }
    public int MQ { get { return mq; } }
    public int SC { get { return sc; } }
    public int XReg { get { return xr; } }
    public int XMode { get { return xMode; } }
    public int MAR { get { return mar; } }
    public int MBR { get { return mbr; } }
    public int CDF { get { return cdf; } }
    public int CIF { get { return cif; } }
    public int Instr {  get { return ir; } }
    public bool ION { get { return (ion & 1) != 0; } }
    public int IONS { get { return ion; } }
    public bool Run { get { return run; } set { run = value; } }
    public int IRQ { get { return irq; } }

    public bool IntSuppress
    {
      get { return (ion & bkptSuppress) != 0; }
      set
      {
        if (value)
          ion |= bkptSuppress;
        else
          ion &= ~bkptSuppress;
      }
    }

    public BreakPort BreakPort { get { return breakPort; } }  // for hardware breakpoint

    public bool GetBreak(int port)
    {
      return (brq & (1 << port)) != 0;
    }

    public void SetBreak(int port, bool set)
    {
      if (set)
        brq |= (1 << port);
      else
        brq &= ~(1 << port);
    }

    public CycleState CurrentCycle { get { return cycle; } }

    // Not part of CPU state, used only my diagnostice: ListingForm and the
    // event recorded
    public int CurrentPC { get; private set; }

    public EventRecorder EventRecorder { get; private set; } = new EventRecorder();

    // *********************
    // *                   *
    // *  Console Actions  *
    // *                   *
    // *********************
    public void LoadAddress(int address)
    {
      pc = address & 0xFFF;
      cif = (address >> 12) & 7;
      cdf = address >> 15;
    }

    public void Examine()
    {
      mar = pc;
      Add12(ref pc);
      mbr = PDP8.Core[cif, mar];
    }

    public void Deposit(int data)
    {
      mar = pc;
      Add12(ref pc);
      mbr = data;
      PDP8.Core[cif, mar] = mbr;
    }

    // ***************
    // *             *
    // *  Constants  *
    // *             *
    // ***************

    const int pageOffsetMask  = 0x07F;
    const int pageZeroBit     = 0x080;
    const int pageMask        = 0xF80;
    const int indirectBit     = 0x100;
    const int group1Bit       = 0x100;
    const int eaeClaBit       = 0x080;
    const int eaeMqlBit       = 0x010;
    const int eaeMqaBit       = 0x040;
    const int eaeScaBit       = 0x020;
    const int eaeScMask       = 0x01F;
    const int claBit          = 0x080;
    const int cllBit          = 0x040;
    const int cmaBit          = 0x020;
    const int cmlBit          = 0x010;
    const int rotMask         = 0x00E;
    const int smaBit          = 0x040;
    const int szaBit          = 0x020;
    const int snlBit          = 0x010;
    const int invertBit       = 0x008;
    const int autoIncMask     = 0xFF8;
    const int autoIncAddr     = 0x008;
    const int xActiveMask     = 0x0C0;  // second half of page 0
    const int xActiveCode     = 0x040;
    const int ionSuppress     = 0x002;
    const int cifSuppress     = 0x004;
    const int bkptSuppress    = 0x008;

    // *****************
    // *               *
    // *  Constructor  *
    // *               *
    // *****************

    public CPU()
    {
      Clear();
    }

    // *********************
    // *                   *
    // *  Reset and Clear  *
    // *                   *
    // *********************

    public void Reset()
    {
      mbr = 0;
      ac = 0;
      link = 0;
      mq = 0;
      sc = 0;
      xr = 0;
      xMode = 0;
      eaeState = EaeCodes.idle;
      ibr = cif;
      ion = 0;
      brq = 0;
      run = false;
      cycle = CycleState.fetch;

      foreach (IODevice dev in Devices)
        if (dev != null)
          dev.Reset();

      foreach (BreakPort bp in BreakPorts)
        if (bp != null)
          bp.Reset();

      if (irq != 0)
        throw new Exception(string.Format("IRQ {0} after reset", irq));
    }

    public void Clear()
    {
      pc = FromOctal("0200");
      mar = 0;
      ir = 0;
      cif = 0;
      cdf = 0;
      sfr = 0;
      mfr = 0;

      Reset();
      PDP8.Core.Clear();
    }

    // ***************
    // *             *
    // *  Utilities  *
    // *             *
    // ***************

    public void IllegalInstr()
    {
      run = false;
      IntSuppress = true;

      string msg = string.Format("Illegal instruction {0} at {1}{2}",
                                  ToOctal(mbr), cif, ToOctal((pc - 1) & 0xFFF));

      MessageBox.Show(msg);
    }

    public static bool Add12(ref int n, int inc = 1)
    {
      n += inc;
      bool overflow = (uint)n > 0xFFF;
      n &= 0xFFF;
      return overflow;
    }

    void setIf()
    {
      cif = ibr;
      ion &= ~cifSuppress;
    }

    // *************************************
    // *                                   *
    // *  Memory Referencing Instructions  *
    // *                                   *
    // *************************************

    private void memref(ref CycleState nextState)
    {
      int effectiveAddr = mbr & pageOffsetMask;
      if ((mbr & pageZeroBit) != 0)
        effectiveAddr |= mar & pageMask;

      if (xMode != 0 && (mbr & xActiveMask) == xActiveCode)
      {
        effectiveAddr += xr - xActiveCode;
        if (xMode == 4)
          mfr = 0;
        else if (xMode == 7)
          mfr = cdf;
      }
      else if (xMode == 4 && (mbr & pageZeroBit) == 0)
        mfr = 0;

      bool indirect = (mbr & indirectBit) != 0;

      if (ir == (int)Opcodes.jmp)
      {
        if (indirect)
        {
          mar = effectiveAddr;
          nextState = CycleState.defer;
          //EventRecorder.Record();
        }
        else
        {
          pc = effectiveAddr;
          setIf();
        }
      }
      else
      {
        mar = effectiveAddr;
        nextState = indirect ? CycleState.defer : CycleState.execute;

        if (ir == (int)Opcodes.jms)
        {
          if (!indirect)
            mfr = ibr;

          //EventRecorder.Record();
        }
      }
    }

    private void memrefEx()
    {
      CycleState nextState = CycleState.fetch;  // for calling eae, not needed

      switch ((Opcodes)ir)
      {
        case Opcodes.and:
          ac &= mbr;
          break;

        case Opcodes.tad:
          if (Add12(ref ac, mbr))
            link ^= 1;
          break;

        case Opcodes.isz:
          if (Add12(ref mbr))
            Add12(ref pc);
          break;

        case Opcodes.dca:
          mbr = ac;
          ac = 0;
          break;

        case Opcodes.jms:
          mbr = pc;
          pc = mar;
          Add12(ref pc);
          setIf();
          EventRecorder.Record();
          break;

        case Opcodes.jmp:
          break;

        case Opcodes.opr:
          eae(ref nextState);
          break;

        default:
          IllegalInstr();
          break;
      }
    }

    // **************************
    // *                        *
    // *  Operate Instructions  *
    // *                        *
    // **************************

    private void opr(ref CycleState nextState)
    {
      if ((mbr & group1Bit) == 0)
      {
        // Group 1
        if ((mbr & claBit) != 0)
          ac = 0;
        if ((mbr & cllBit) != 0)
          link = 0;
        if ((mbr & cmaBit) != 0)
          ac ^= 0xFFF;
        if ((mbr & cmlBit) != 0)
          link ^= 1;
        if ((mbr & 1) != 0)
          if (Add12(ref ac))
            link ^= 1;

        int ac0 = ac;

        switch (mbr & rotMask)
        {
          case 0:     // NOP
            break;

          case 2:     // BSW
            ac = (ac >> 6) | ((ac & 0x3F) << 6);
            break;

          case 4:     // RAL
            ac = ((ac << 1) | link) & 0xFFF;
            link = ac0 >> 11;
            break;

          case 6:     // RTL
            ac = ((ac << 2) | (link << 1) | (ac >> 11)) & 0xFFF;
            link = (ac0 >> 10) & 1;
            break;

          case 8:     // RAR
            ac = (ac >> 1) | (link << 11);
            link = ac0 & 1;
            break;

          case 0xA:   // RTR
            ac = (ac >> 2) | (link << 10) | ((ac & 1) << 11);
            link = (ac0 >> 1) & 1;
            break;

          default:
            IllegalInstr();
            break;
        }
      }
      else if ((mbr & 1) == 0)
      {
        // Group 2
        bool skip = (mbr & smaBit) != 0 & (ac & 0x800) != 0;
        skip |= (mbr & szaBit) != 0 & ac == 0;
        skip |= (mbr & snlBit) != 0 & link != 0;
        skip ^= (mbr & invertBit) != 0;
        if (skip)
          Add12(ref pc);

        if ((mbr & claBit) != 0)
          ac = 0;

        if ((mbr & 4) != 0)
          ac |= PDP8.Console.SwitchRegister;

        if ((mbr & 2) != 0)
          run = false;
      }
      else
        eae(ref nextState);
    }

    // **********************
    // *                    *
    // *  I/O Instructions  *
    // *                    *
    // **********************

    private void iot()
    {
      const int ionBits = 1 | ionSuppress;

      int ioAddr = (mbr >> 3) & 0x03F;
      int controlBits = mbr & 7;

      if (ioAddr == 0)
      {
        // Interrupt control
        switch (controlBits)
        {
          case 1:
          case 5:         // HRP PION
            if ((ion & ionBits) == 0)
              ion |= ionBits;
            break;

          case 2:
            ion &= ~ionBits;
            break;

          default:
            IllegalInstr();
            break;
        }
      }
      else if ((ioAddr >> 3) == 2)
        mec();
      else if ((ioAddr >> 3) == 6 | ioAddr == 7)
        xio();
      else
      {
        IODevice dev = Devices[ioAddr];
        if (dev == null)
          IllegalInstr();
        else
        {
          bool skip;
          dev.Iot(controlBits, ref ac, out skip);
          if (skip)
            Add12(ref pc);
        }
      }
    }

    // *********************************
    // *                               *
    // *  Extended Arithmetic Element  *
    // *                               *
    // *********************************

    private void eae(ref CycleState nextState)
    {
      switch (cycle)
      {
        case CycleState.fetch:
          int mqTemp = mq;
          if ((mbr & eaeClaBit) != 0)
            ac = 0;
          if ((mbr & eaeMqlBit) != 0)
          {
            mq = ac;
            ac = 0;
          }
          if ((mbr & eaeMqaBit) != 0)
            ac |= mqTemp;
          if ((mbr & eaeScaBit) != 0)
            ac |= sc;

          eaeState = (EaeCodes)((mbr >> 1) & 7);
          if (eaeState != EaeCodes.idle)
          {
            if (eaeState == EaeCodes.nmi)
            {
              sc = 0;
              int n = (ac << 12) | mq;
              if (n != 0)
              {
                while (n != 0xC00000 && ((n ^ (n << 1)) & 0x800000) == 0)
                {
                  link = n >> 23;
                  n = (n << 1) & 0xFFFFFF;
                  ++sc;
                }
                mq = n & 0xFFF;
                ac = n >> 12;
              }
            }
            else
            {
              nextState = CycleState.execute;
              mar = pc;
              Add12(ref pc);
            }
          }
          break;

        case CycleState.execute:
          switch (eaeState)
          {
            case EaeCodes.scl:
              sc = ~mbr & eaeScMask;
              break;

            case EaeCodes.muy:
              int n = mq * mbr;
              mq = n & 0xFFF;
              ac = n >> 12;
              link = 0;
              break;

            case EaeCodes.dvi:
              if (mbr > ac)
              {
                n = (ac << 12) | mq;
                mq = n / mbr;
                ac = n % mbr;
                link = 0;
              }
              else
                link = 1;
              break;

            case EaeCodes.shl:
              n = (link << 24) | (ac << 12) | mq;
              n <<= (mbr & eaeScMask) + 1;
              n &= 0x1FFFFFF;
              mq = n & 0xFFF;
              ac = (n >> 12) & 0xFFF;
              link = n >> 24;
              break;

            case EaeCodes.asr:
              n = (ac << 12) | mq;
              int sign = n & 0x800000;
              for (int i = 0; i <= (mbr & eaeScMask); ++i)
                n = (n >> 1) | sign;
              mq = n & 0xFFF;
              ac = (n >> 12) & 0xFFF;
              link = n >> 23;
              break;

            case EaeCodes.lsr:
              n = (ac << 12) | mq;
              n >>= (mbr & eaeScMask) + 1;
              mq = n & 0xFFF;
              ac = (n >> 12) & 0xFFF;
              link = 0;
              break;
          }
          break;
      }
    }

    // ******************************
    // *                            *
    // *  Memory Extension Control  *
    // *                            *
    // ******************************

    private void mec()
    {
      int field = (mbr >> 3) & 7;
      if ((mbr & 1) != 0)
        cdf = field;
      if ((mbr & 2) != 0)
      {
        ibr = field;
        ion |= cifSuppress;   // VB6 had &= but that's wrong
      }
      if ((mbr & 4) != 0)
        switch (field)
        {
          case 1:
            ac |= cdf << 3;
            break;
          case 2:
            ac |= cif << 3;
            break;
          case 3:
            ac |= sfr;
            break;
          case 4:
            ibr = sfr & 7;
            cdf = sfr >> 3;
            break;
          default:
            IllegalInstr();
            break;
        }
    }

    // *************************************
    // *                                   *
    // *  Index Register I/O Instructions  *
    // *                                   *
    // *************************************

    private void xio()
    {
      int ioAddr = (mbr >> 3) & 0x03F;

      if (ioAddr == 7)
      {
        if ((mbr & 1) != 0)
        {
          if (Add12(ref xr))
            Add12(ref pc);
        }

        if ((mbr & 2) != 0)
          ac = xr;

        if ((mbr & 4) != 0)
            ac = xMode << 3;
      }
      else
      {
        if ((mbr & 1) != 0)
          xr = ac;

        if ((mbr & 2) != 0)
          xMode = ioAddr & 7;

        if ((mbr & 4) != 0)
          ac = 0;
      }

    }

    // *********************************
    // *                               *
    // *  Interrupt Request and Clear  *
    // *                               *
    // *********************************

    public void RequestInterrupt()
    {
      ++irq;
    }

    public void ClearInterrupt()
    {
      if (--irq < 0)
        throw new Exception("Interrupt request underflow");
    }

    // *******************
    // *                 *
    // *  One CPU Cycle  *
    // *                 *
    // *******************

    public void Cycle()
    {
      breakPort = null;

      if (cycle == CycleState.fetch)
      {
        if (brq != 0)
        {
          breakPort = BreakPorts[int.TrailingZeroCount(brq)];
          if (breakPort.ThreeCycle)
          {
            cycle = CycleState.wordCount;
            mfr = 0;
          }
          else
          {
            cycle = CycleState.dataBreak;
            mfr = breakPort.MemField;
          }
          mar = breakPort.MemAddr;
        }
        else if (ion == 1 && irq > 0)
        {
          // Interrupt
          ion = 0;
          mar = 0;
          mfr = 0;
          sfr = (ibr << 3) | cdf;
          ibr = 0;
          cdf = 0;
          ir = (int)Opcodes.jms;
          cycle = CycleState.execute;
          run = true;
          //EventRecorder.Record();
        }
        else if (run)
        {
          ion &= ~ionSuppress;
          mar = pc;
          CurrentPC = (cif << 12) | pc;
          Add12(ref pc);
          mfr = cif;
        }
        else
          return;
      }

      if (!run && brq == 0)
        return;

      // Memory read
      int mar0 = mar;   // Save for writeback
      int mfr0 = mfr;
      mbr = PDP8.Core[mfr, mar];

      // Update CPU state
      CycleState nextState = cycle;
      switch (cycle)
      {
        case CycleState.fetch:
          ir = mbr >> 9;
          if (ir <= 5)
            memref(ref nextState);
          else if (ir == 6)
            iot();
          else
            opr(ref nextState);
          break;

        case CycleState.defer:
          if ((mar & autoIncMask) == autoIncAddr)
            Add12(ref mbr);
          mar = mbr;

          if (ir == (int)Opcodes.jmp)
          {
            pc = mbr;
            setIf();
            nextState = CycleState.fetch;
            EventRecorder.Record();
          }
          else
          {
            nextState = CycleState.execute;
            mfr = ir == (int)Opcodes.jms ? ibr : cdf;
          }
          break;

        case CycleState.execute:
          memrefEx();
          nextState = CycleState.fetch;
          break;

        case CycleState.wordCount:
          if (Add12(ref mbr))
            breakPort.WCOverflow = true;
          Add12(ref mar);
          break;

        case CycleState.currentAddress:
          if (!breakPort.IncCAInhibit)
            Add12(ref mbr);
          mar = mbr;
          nextState = CycleState.dataBreak;
          mfr = breakPort.MemField;
          break;

        case CycleState.dataBreak:
          if (!breakPort.ThreeCycle & breakPort.IncMBR)
            Add12(ref mbr);
          else if (breakPort.DataWrite)
            mbr = breakPort.MemData;
          else
            breakPort.MemData = mbr;
          breakPort.Brq = false;
          nextState = CycleState.fetch;
          break;
      }

      PDP8.Core[mfr0, mar0] = mbr;
      cycle = nextState;

      if ((uint)ac > 0xFFF)
      {
        run = false;
        MessageBox.Show("Halting with bad AC");
      }
    }

    // ******************
    // *                *
    // *  Disassembler  *
    // *                *
    // ******************

    public string Disassemble(int pc, int cif, int cdf)
    {
      int instr = PDP8.Core[cif, pc];
      string s = string.Format("{0} {1} ",
                               ToOctal((cif << 12) | pc, 5),
                               ToOctal(instr));
      if (instr == 0)
        return s;

      int opcode = instr >> 9;

      if (opcode <= 5)
      {
        s += ((Opcodes)opcode).ToString().ToUpper();

        bool indirect = (instr & indirectBit) != 0;

        if (indirect)
          s += " I";

        bool xActive = xMode != 0 && (instr & xActiveMask) == xActiveCode;
        if (xActive)
          s += string.Format(" X+{0}", ToOctal(instr & 0x3F, 1));

        int ea = instr & 0x7F;
        if ((instr & pageZeroBit) != 0)
          ea |= pc & 0xF80;

        int field = cif;
        if (xActive)
        {
          ea += xr - xActiveCode;
          if (xMode == 4)
            field = 0;
          else if (xMode == 7)
            field = cdf;
          else if (xMode != 6)
            s += "??";
        }
        else if (xMode == 4 && (instr & pageZeroBit) == 0)
          field = 0;

        s += string.Format(" {0}", ToOctal(ea));

        int eaData = PDP8.Core[field, ea];

        if (opcode <= 3)
        {
          s += string.Format(" -> {0}", ToOctal(eaData));

          if (indirect)
            s += string.Format(" -> {0}", ToOctal(PDP8.Core[cdf, eaData]));
        }
        else if (indirect)
          s += string.Format(" -> {0}", ToOctal(eaData));
      }

      else if (opcode == 6)
      {
        s += string.Format("IOT {0}.{1}", ToOctal((instr >> 3) & 0x3F, 2), instr & 7);
      }

      else if ((instr & group1Bit) == 0)
      {
        // Group 1
        if ((instr & claBit) != 0)
          s += "CLA ";
        if ((instr & cllBit) != 0)
          s += "CLL ";
        if ((instr & cmaBit) != 0)
          s += "CMA ";
        if ((instr & cmlBit) != 0)
          s += "CML ";
        if ((instr & 1) != 0)
          s += "IAC ";

        string[] rottab = new string[] { "", "BSW", "RAL", "RTL", "RAR", "RTR", "ILG", "ILG" };
        s += rottab[(instr >> 1) & 7];
      }

      else if ((instr & 1) == 0)
      {
        // Group 2
        bool inv = (instr & invertBit) != 0;
        if ((instr & smaBit) != 0)
          s += inv ? "SPA " : "SMA ";
        if ((instr & szaBit) != 0)
          s += inv ? "SNA " : "SZA ";
        if ((instr & snlBit) != 0)
          s += inv ? "SZL " : "SNL ";

        if ((instr & claBit) != 0)
          s += "CLA ";

        if ((instr & 4) != 0)
          s += "OSR";
        if ((instr & 2) != 0)
          s += "HLT";
      }

      else
      {
        // EAE
        if ((instr & claBit) != 0)
          s += "CLA ";

        if ((instr & eaeMqaBit) != 0)
          s += "MQA ";
        if ((instr & eaeScaBit) != 0)
          s += "SCA ";
        if ((instr & eaeMqlBit) != 0)
          s += "MQL ";

        string[] eaetab = new string[] { "", "SCL", "MUY", "DVI", "NMI", "SHL", "ASR", "LSR"};
        int eaeCode = (instr >> 1) & 7;
        s += eaetab[eaeCode];

        if (eaeCode != 0 & eaeCode != 4)
          s += string.Format(" {0}", PDP8.Core[cif, pc + 1]);
      }

      return s;
    }

    public string Disassemble()
    {
      return Disassemble(pc, cif, cdf);
    }
  }


}

