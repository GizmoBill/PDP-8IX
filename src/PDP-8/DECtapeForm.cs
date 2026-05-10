using DECtapeControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static PDP8;

namespace PDP_8
{
  public partial class DECtapeForm : Form
  {
    const int InstalledDrives = 2;

    // unitNum [0 .. 7] is selected by the numericUpDown, bits 0-2 of
    // status register A. unitIndex [0 .. 1] is which physical drive.
    // driveMap maps unitNum to unitIndex, or -1 if no drive selects
    // that unitNum.
    int[] driveMap = new int[8];

    // The two installed physical drives
    DecTapeDriveControl[] drives;

    // So we can access buttons by unitIndex for setting enable
    Button[] saveButtons;
    Button[] unloadButtons;

    string[] filenames = new string[InstalledDrives];

    TC01 tc01;

    public DECtapeForm()
    {
      InitializeComponent();

      for (int i = 0; i < driveMap.Length; ++i)
        driveMap[i] = -1;
      driveMap[(int)unitNumeric0.Value] = 0;
      driveMap[(int)unitNumeric1.Value] = 1;

      drives = new DecTapeDriveControl[] { dectape0, dectape1 };
      saveButtons = new Button[] { saveButton0, saveButton1 };
      unloadButtons = new Button[] { unloadButton0, unloadButton1 };

      tc01 = new TC01(this);
    }

    int getUnitIndex(object obj)
    {
      string name = ((Control)obj).Name;
      return name[name.Length - 1] - '0';
    }

    public int UnitIndex(int unitNum)
    {
      return driveMap[unitNum];
    }

    public bool TapeLoaded(int unitIndex)
    {
      return unitIndex >= 0 && filenames[unitIndex] != null;
    }

    public void SetSpeed(int unitIndex, float speed)
    {
      if (unitIndex >= 0)
        drives[unitIndex].SpeedFactor = speed;
    }

    public void SetTapeFill(int unitIndex, float fill)
    {
      if (unitIndex >= 0)
        drives[unitIndex].TapeFillFactor = fill;
    }

    // ***********
    // *         *
    // *  State  *
    // *         *
    // ***********

    public const string XmlTag = "tu55";

    public void SetChanged(int unitIndex, bool changed)
    {
      saveButtons[unitIndex].Enabled = changed;
    }

    public XmlLiteNode State
    {
      get
      {
        XmlLiteNode node = new XmlLiteNode(XmlTag);
        for (int i = 0; i < InstalledDrives; ++i)
          if (filenames[i] != null)
          {
            XmlLiteNode tape = new XmlLiteNode("tape", filenames[i]);
            tape.SetAttribute("unit", i.ToString());
            node.Children.Add(tape);
          }

        for (int i = 0; i < InstalledDrives; ++i)
          saveTape(i);

        return node;
      }

      set
      {
        if (CheckTag(value, XmlTag))
          return;

        for (int i = 0; i < InstalledDrives; ++i)
          filenames[i] = null;

        foreach (XmlLiteNode node in value.Children)
          if (node.Tag == "tape")
            filenames[int.Parse(node.GetAttribute("unit"))] = node.Value;

        for (int i = 0; i < InstalledDrives; ++i)
          restoreTape(i);
      }
    }

    private void setDriveState(int unitIndex, string filename)
    {
      filenames[unitIndex] = filename;
      bool loaded = filename != null;
      drives[unitIndex].TapeLoaded = loaded;
      drives[unitIndex].LeftReelLabel = loaded ? Path.GetFileNameWithoutExtension(filename) : string.Empty;
      saveButtons[unitIndex].Enabled = false;
      unloadButtons[unitIndex].Enabled = loaded;
    }

    public void Reset()
    {
      for (int i = 0; i < InstalledDrives; ++i)
      {
        saveTape(i);
        setDriveState(i, null);
      }
    }

    // ************
    // *          *
    // *  Events  *
    // *          *
    // ************

    // Prevent unitNum conflicts
    private void unit0Numeric_ValueChanged(object sender, EventArgs e)
    {
      NumericUpDown nud = (NumericUpDown)sender;
      int newUnitNum = (int)nud.Value;
      int unitIndex = getUnitIndex(sender);

      int oldUnitNum;
      for (oldUnitNum = 0; driveMap[oldUnitNum] != unitIndex; ++oldUnitNum) ;

      driveMap[oldUnitNum] = -1;

      // If not visible, restore state is causing this event. Don't correct it.
      if (Visible)
      {
        int del = Math.Sign(newUnitNum - oldUnitNum);
        while (driveMap[newUnitNum] >= 0)
          newUnitNum = (newUnitNum + del) % 8;
      }

      driveMap[newUnitNum] = unitIndex;
      nud.Value = newUnitNum;
    }

    private void DECtapeForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    bool diagVisible = false;
    private void DECtapeForm_DoubleClick(object sender, EventArgs e)
    {
      int margin = Height - ClientSize.Height + 16;

      if (diagVisible)
      {
        Height = dectape1.Top + dectape1.Height + margin;
        diagVisible = false;
      }
      else
      {
        Height = paint0TimeLabel.Top + paint0TimeLabel.Height + margin;
        diagVisible = true;
      }
    }

    private int timerPhase = 0;

    bool[] rewinding = new bool[InstalledDrives];

    private void timer1_Tick(object sender, EventArgs e)
    {
      drives[timerPhase & 1].ReelTick();

      for (int i = 0; i < InstalledDrives; ++i)
        if (rewinding[i])
        {
          float blocksLeft = (1.0f - drives[i].TapeFillFactor) * 1471;
          if (blocksLeft > 0)
            drives[i].TapeFillFactor = 1.0f - (blocksLeft - 2) / 1471;
          else
          {
            drives[i].TapeLoaded = false;
            drives[i].SpeedFactor = 0;
            rewinding[i] = false;
          }
        }

      if (timerPhase == 0 & diagVisible)
      {
        paint0TimeLabel.Text = dectape0.BackTime.ToString("f3");
        paint1TimeLabel.Text = dectape0.ReelTime.ToString("f3");
      }

      timerPhase = (timerPhase + 1) % 4;
    }

    private void DECtapeForm_Shown(object sender, EventArgs e)
    {
      timer1.Enabled = true;
    }

    // *********************
    // *                   *
    // *  New, Load, Save  *
    // *                   *
    // *********************

    private void saveTape(int unitIndex)
    {
      if (saveButtons[unitIndex].Enabled)
        tc01.SaveTape(unitIndex, filenames[unitIndex]);
    }

    private void restoreTape(int unitIndex)
    {
      string filename = filenames[unitIndex];

      if (filename != null && File.Exists(filename))
      {
        tc01.LoadTape(unitIndex, filename);
        setDriveState(unitIndex, filename);
      }
      else
      {
        setDriveState(unitIndex, null);
        tc01.NewTape(unitIndex);
      }
    }

    private void newButton0_Click(object sender, EventArgs e)
    {
      int index = getUnitIndex(sender);
      saveTape(index);

      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "TC01 DECtape Files|*.tc01";
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        setDriveState(index, sfd.FileName);
        tc01.NewTape(index);
      }
    }

    private void loadButton0_Click(object sender, EventArgs e)
    {
      int index = getUnitIndex(sender);
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "TC01 DECtape Files|*.tc01";
      if (filenames[index] != null && File.Exists(filenames[index]))
        ofd.InitialDirectory = Path.GetDirectoryName(filenames[index]);
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        saveTape(index);
        tc01.LoadTape(index, ofd.FileName);
        setDriveState(index, ofd.FileName);
      }
    }

    private void saveButton0_Click(object sender, EventArgs e)
    {
      int index = getUnitIndex(sender);
      saveTape(index);
    }

    private void unloadButton0_Click(object sender, EventArgs e)
    {
      int index = getUnitIndex(sender);
      saveTape(index);
      setDriveState(index, null);
      drives[index].TapeLoaded = true;
      drives[index].SpeedFactor = -1;
      rewinding[index] = true;
    }

    // *****************
    // *               *
    // *  For Testing  *
    // *               *
    // *****************

    public void SetBlock(int block)
    {
      if (timer1.Enabled)
        blockLabel.Text = block.ToString();
    }

    private void reverseButton_Click(object sender, EventArgs e)
    {
      SetSpeed(0, -1);
    }

    private void stopButton_Click(object sender, EventArgs e)
    {
      SetSpeed(0, 0);
    }

    private void forwardButton_Click(object sender, EventArgs e)
    {
      SetSpeed(0, 1);
    }
  }
}
