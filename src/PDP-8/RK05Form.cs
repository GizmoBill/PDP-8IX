using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDP_8
{
  public partial class RK05Form : Form
  {
    CheckBox[] saveOnExitChecks;

    TextBox[] filenameTextBoxes;

    Button[] saveButtons;

    public RK05Form()
    {
      InitializeComponent();

      RK05 = new RK05(this);

      saveOnExitChecks = new CheckBox[]
      {
        saveOnExit0Check, saveOnExit1Check, saveOnExit2Check, saveOnExit3Check
      };

      filenameTextBoxes = new TextBox[]
      {
        filename0Text, filename1Text, filename2Text, filename3Text
      };

      saveButtons = new Button[]
      {
        save0Button, save1Button, save2Button, save3Button
      };
    }

    public RK05 RK05 { get; private set; }

    private void RK05Form_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    // Can't load in constructor because filenames haven't yet been restored
    public void LoadDrives()
    {
      for (int drive = 0; drive < 4; ++drive)
      {
        if (File.Exists(getFilename(drive)))
        {
          RK05.LoadDisk(drive, getFilename(drive));
          filenameTextBoxes[drive].BackColor = Color.White;
        }
        else
          filenameTextBoxes[drive].BackColor = Color.LightPink;
        filenameTextBoxes[drive].Select(filenameTextBoxes[drive].Text.Length, 0);
      }
    }

    public void SaveDrives()
    {
      for (int drive = 0; drive < 4; ++drive)
        if (saveOnExitChecks[drive].Checked && saveButtons[drive].Enabled)
          save0Button_Click(saveButtons[drive], null);
    }

    public void SetChanged(int driveNum, bool changed)
    {
      saveButtons[driveNum].Enabled = changed;
    }

    int getDriveNum(object sender)
    {
      Button b = (Button)sender;
      return int.Parse(b.Parent.Text.Substring(5, 1));
    }

    string getFilename(int driveNum)
    {
      return filenameTextBoxes[driveNum].Text;
    }

    void setFilename(int driveNum, string filename)
    {
      filenameTextBoxes[driveNum].Text = filename;
      filenameTextBoxes[driveNum].Select(filename.Length, 0);
    }

    private void load0Button_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "RK05 File|*.rk05";
      if (File.Exists(getFilename(getDriveNum(sender))))
        ofd.InitialDirectory = Path.GetDirectoryName(getFilename(getDriveNum(sender)));
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        int drive = getDriveNum(sender);
        RK05.LoadDisk(drive, ofd.FileName);
        setFilename(getDriveNum(sender), ofd.FileName);
        filenameTextBoxes[drive].BackColor = Color.White;
      }
    }

    private void save0Button_Click(object sender, EventArgs e)
    {
      if (getFilename(getDriveNum(sender)).Length == 0)
        saveAs0Button_Click(sender, e);
      else
        RK05.SaveDisk(getDriveNum(sender), getFilename(getDriveNum(sender)));
    }

    private void saveAs0Button_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "RK05 File|*.rk05";
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        int drive = getDriveNum(sender);
        RK05.SaveDisk(drive, sfd.FileName);
        setFilename(getDriveNum(sender), sfd.FileName);
        filenameTextBoxes[drive].BackColor = Color.White;
      }
    }
  }
}
