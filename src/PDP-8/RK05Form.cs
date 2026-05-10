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

    // ***********
    // *         *
    // *  State  *
    // *         *
    // ***********

    public const string XmlTag = "rk05";

    public void LoadDrives()
    {
      for (int drive = 0; drive < 4; ++drive)
      {
        restoreDisk(drive);
        filenameTextBoxes[drive].Select(filenameTextBoxes[drive].Text.Length, 0);
      }
    }

    public void SaveDrives()
    {
      for (int drive = 0; drive < 4; ++drive)
        saveDisk(drive, false);
    }

    // **********************************
    // *                                *
    // *  Get/Set Drives and Filenames  *
    // *                                *
    // **********************************

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

    // ********************************
    // *                              *
    // *  Load, Save, SaveAs Buttons  *
    // *                              *
    // ********************************

    private bool saveDisk(int unitIndex, bool offer)
    {
      if (!RK05.GetChanged(unitIndex) || !saveOnExitChecks[unitIndex].Checked)
        return false;

      string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      string defaultFile = string.Format("disc{0}_.rk05", unitIndex);
      defaultFile = Path.Combine(docs, defaultFile);

      string filename = getFilename(unitIndex);
      if (filename.Length == 0)
      {
        if (offer)
          switch (MessageBox.Show("Save disc?", "RK05", MessageBoxButtons.YesNoCancel))
          {
            case DialogResult.Yes:
              SaveFileDialog sfd = new SaveFileDialog();
              sfd.Filter = "RK05 File|*.rk05";
              if (sfd.ShowDialog() == DialogResult.OK)
                filename = sfd.FileName;
              else
                return true;
              break;

            case DialogResult.No:
              return false;

            case DialogResult.Cancel:
              return true;
          }
        else
          filename = defaultFile;
      }
      else if (File.Exists(defaultFile))
        File.Delete(defaultFile);

      RK05.SaveDisk(unitIndex, filename);
      return false;
    }

    private void restoreDisk(int unitIndex)
    {
      string filename = getFilename(unitIndex);
      if (filename.Length == 0)
      {
        string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        filename = string.Format("disc{0}_.rk05", unitIndex);
        filename = Path.Combine(docs, filename);
      }

      if (File.Exists(filename))
        RK05.LoadDisk(unitIndex, filename);
      else
        setFilename(unitIndex, string.Empty);
    }

    private void load0Button_Click(object sender, EventArgs e)
    {
      int drive = getDriveNum(sender);
      string filename = getFilename(drive);

      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "RK05 File|*.rk05";
      if (filename.Length > 0 && File.Exists(filename))
        ofd.InitialDirectory = Path.GetDirectoryName(filename);
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        if (saveDisk(drive, true))
          return;
        RK05.LoadDisk(drive, ofd.FileName);
        setFilename(getDriveNum(sender), ofd.FileName);
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
      }
    }
  }
}
