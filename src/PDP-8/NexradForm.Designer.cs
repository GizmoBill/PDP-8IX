namespace PDP_8
{
  partial class NexradForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      label1 = new Label();
      radarCombo = new ComboBox();
      fetchButton = new Button();
      liveCheck = new CheckBox();
      label2 = new Label();
      intervalNumeric = new NumericUpDown();
      label3 = new Label();
      fetchTimeLabel = new Label();
      keepCheck = new CheckBox();
      enableCheck = new CheckBox();
      showButton = new Button();
      loadButton = new Button();
      sweepSelectionCombo = new ComboBox();
      ((System.ComponentModel.ISupportInitialize)intervalNumeric).BeginInit();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 9);
      label1.Name = "label1";
      label1.Size = new Size(63, 28);
      label1.TabIndex = 0;
      label1.Text = "Radar";
      // 
      // radarCombo
      // 
      radarCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      radarCombo.FormattingEnabled = true;
      radarCombo.Items.AddRange(new object[] { "AK-Bethel-KABC", "AK-King Salmon-KAKC", "AK-Middleton Island-KAIH", "AK-Nikiski-KAHG", "AK-Nome-KAEC", "AK-Pedro Dome-KAPD", "AK-Sitka-KACG", "AL-(Maxwell AFB RDA)-KMXX", "AL-Alabaster-KBMX", "AL-Fort Rucker-KEOX", "AL-Mobile-KMOB", "AL-Northeastern Alabama-KHTX", "AR-North Little Rock-KLZK", "AZ-Coconino-KFSX", "AZ-Mesa-KIWA", "AZ-Pima County-KEMX", "AZ-Yuma-KYUX", "CA-(Beale AFB)-KBBX", "CA-(Vandenberg AFB)-KVBX", "CA-Hanford-KHNX", "CA-Humboldt County-KBHX", "CA-Orange County-KSOX", "CA-Sacramento-KDAX", "CA-San Diego-KNKX", "CA-Santa Clara County-KMUX", "CA-Ventura County-KVTX", "CO-Denver-KFTG", "CO-Mesa-KGJX", "CO-Pueblo County-KPUX", "FL-(Moody AFB)-KVAX", "FL-Boca Chica Key-KBYX", "FL-Eglin AFB RDA-KEVX", "FL-Jacksonville-KJAX", "FL-Melbourne-KMLB", "FL-Miami-KAMX", "FL-Ruskin-KTBW", "FL-Tallahassee-KTLH", "GA-Peachtree City-KFFC", "GA-State Hwy 96 (Robins AFB)-KJGX", "GU-Barrigada Comm. Station-KGUA", "HI-Hawaii-KHWA", "HI-Kauai-KHKI", "HI-Kohala-KHKM", "HI-Molokai-KHMO", "IA-Davenport-KDVN", "IA-Johnston-KDMX", "ID-Ada County-KCBX", "ID-Springfield-KSFX", "IL-Lincoln-KILX", "IL-Romeoville-KLOT", "IN-EVANSVILLE-KVWX", "IN-Indianapolis-KIND", "IN-North Webster-KIWX", "JA-KSON/BRANDON, MS-KDGX", "KS-Alma-KTWX", "KS-Dodge City-KDDC", "KS-Goodland-KGLD", "KS-Wichita-KICT", "KY-(Fort Campbell)-KHPX", "KY-Fort Knox-KLVX", "KY-Noctor-KJKL", "KY-Paducah-KPAH", "LA-Ft Polk Firing Pt 707-KPOE", "LA-GLEY HILL (NW WASHINGTON)-KLGX", "LA-Lake Charles-KLCH", "LA-Shreveport-KSHV", "LA-Slidell-KLIX", "MA-Taunton-KBOX", "MD- WASHINGTON DC/Sterling-KLWX", "ME-Gray-KGYX", "ME-Houlton-KCBW", "MI-Gaylord-KAPX", "MI-Grand Rapids-KGRR", "MI-Marquette-KMQT", "MI-White Lake-KDTX", "MN-Chanhassen-KMPX", "MN-Duluth-KDLH", "MO-Pleasant Hill-KEAX", "MO-Springfield-KSGF", "MO-St Charles-KLSX", "MT-Glasgow-KGGW", "MT-Great Falls-KTFX", "MT-Missoula County-KMSX", "MT-Yellowstone County-KBLX", "NC-Clayton-KRAX", "NC-Newport-KMHX", "NC-Shallotte-KLTX", "ND-(Minot AFB)-KMBX", "ND-Bismarck-KBIS", "ND-Mayville-KMVX", "NE-Thedford-KLNX", "NE-Valley-KOAX", "NE-Webster County-KUEX", "NM-(Cannon AFB)-KFDX", "NM-Albuquerque-KABX", "NV-Edwards AFB-KEYX", "NV-Lander County-KLRX", "NV-Nelson-KESX", "NV-Washoe County-KRGX", "NY-Binghamton-KBGM", "NY-Cheektowaga-KBUF", "NY-East Berne-KENX", "NY-Upton-KOKX", "OH-Cleveland-KCLE", "OH-Wilmington-KILN", "OK-(Vance AFB)-KVNX", "OK-Altus AFB-KFDR", "OK-Inola-KINX", "OK-Norman-KTLX", "OK-Western Arkansas-KSRX", "OR-Jackson County-KMAX", "OR-Pendleton-KPDT", "OR-Scappoose-KRTX", "PA-Coraopolis-KPBZ", "PA-Manchester-KDIX", "PA-Rush-KCCX", "SC-Columbia-KCAE", "SC-Grays-KCLX", "SC-Greer-KGSP", "SD-Aberdeen-KABR", "SD-New Underwood-KUDX", "SD-Sioux Falls-KFSD", "TN-(Columbus AFB)-KGWX", "TN-Millington-KNQA", "TN-Morristown-KMRX", "TN-Old Hickory-KOHX", "TX-(Dyess AFB)-KDYX", "TX-(Laughlin AFB)-KDFX", "TX-Amarillo-KAMA", "TX-Brownsville-KBRO", "TX-Corpus Christi-KCRP", "TX-Dickinson-KHGX", "TX-Fort Worth-KFWS", "TX-Ft Hood RDA-KGRK", "TX-Lubbock-KLBB", "TX-Midland-KMAF", "TX-New Braunfels-KEWX", "TX-San Angelo-KSJT", "TX-Santa Teresa-KEPZ", "TX-White Sands Missile Range-KHDX", "UT-Cedar City-KICX", "UT-Elder County-KMTX", "VA-(Dover AFB)-KDOX", "VA-Floyd County-KFCX", "VA-Wakefield-KAKQ", "VT-Colchester-KCXX", "VT-TBD-KTYX", "WA-Camano Island-KATX", "WA-Spokane-KOTX", "WI-Ashwaubenon-KGRB", "WI-Dousman-KMKX", "WI-La Crosse-KARX", "WV-Ruthdale-KRLX", "WY-Cheyenne-KCYS", "WY-Riverton-KRIW" });
      radarCombo.Location = new Point(81, 12);
      radarCombo.Name = "radarCombo";
      radarCombo.Size = new Size(378, 36);
      radarCombo.TabIndex = 1;
      radarCombo.SelectedIndexChanged += radarCombo_SelectedIndexChanged;
      // 
      // fetchButton
      // 
      fetchButton.Location = new Point(12, 173);
      fetchButton.Name = "fetchButton";
      fetchButton.Size = new Size(116, 54);
      fetchButton.TabIndex = 2;
      fetchButton.Text = "Fetch Now";
      fetchButton.UseVisualStyleBackColor = true;
      fetchButton.Click += fetchButton_Click;
      // 
      // liveCheck
      // 
      liveCheck.AutoSize = true;
      liveCheck.Location = new Point(142, 62);
      liveCheck.Name = "liveCheck";
      liveCheck.Size = new Size(72, 32);
      liveCheck.TabIndex = 3;
      liveCheck.Text = "Live";
      liveCheck.UseVisualStyleBackColor = true;
      liveCheck.CheckedChanged += liveCheck_CheckedChanged;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(220, 62);
      label2.Name = "label2";
      label2.Size = new Size(163, 28);
      label2.TabIndex = 4;
      label2.Text = "Interval (minutes)";
      // 
      // intervalNumeric
      // 
      intervalNumeric.Increment = new decimal(new int[] { 5, 0, 0, 0 });
      intervalNumeric.Location = new Point(389, 60);
      intervalNumeric.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
      intervalNumeric.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
      intervalNumeric.Name = "intervalNumeric";
      intervalNumeric.Size = new Size(70, 34);
      intervalNumeric.TabIndex = 5;
      intervalNumeric.TextAlign = HorizontalAlignment.Right;
      intervalNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });
      intervalNumeric.ValueChanged += intervalNumeric_ValueChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(12, 118);
      label3.Name = "label3";
      label3.Size = new Size(98, 28);
      label3.TabIndex = 6;
      label3.Text = "Last Fetch";
      // 
      // fetchTimeLabel
      // 
      fetchTimeLabel.BorderStyle = BorderStyle.Fixed3D;
      fetchTimeLabel.Location = new Point(116, 118);
      fetchTimeLabel.Name = "fetchTimeLabel";
      fetchTimeLabel.Size = new Size(340, 33);
      fetchTimeLabel.TabIndex = 7;
      fetchTimeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // keepCheck
      // 
      keepCheck.AutoSize = true;
      keepCheck.Location = new Point(346, 185);
      keepCheck.Name = "keepCheck";
      keepCheck.Size = new Size(82, 32);
      keepCheck.TabIndex = 8;
      keepCheck.Text = "Keep";
      keepCheck.UseVisualStyleBackColor = true;
      // 
      // enableCheck
      // 
      enableCheck.AutoSize = true;
      enableCheck.Location = new Point(32, 62);
      enableCheck.Name = "enableCheck";
      enableCheck.Size = new Size(96, 32);
      enableCheck.TabIndex = 9;
      enableCheck.Text = "Enable";
      enableCheck.UseVisualStyleBackColor = true;
      // 
      // showButton
      // 
      showButton.Enabled = false;
      showButton.Location = new Point(152, 173);
      showButton.Name = "showButton";
      showButton.Size = new Size(71, 54);
      showButton.TabIndex = 10;
      showButton.Text = "Show";
      showButton.UseVisualStyleBackColor = true;
      showButton.Click += showButton_Click;
      // 
      // loadButton
      // 
      loadButton.Location = new Point(243, 176);
      loadButton.Name = "loadButton";
      loadButton.Size = new Size(70, 49);
      loadButton.TabIndex = 11;
      loadButton.Text = "Load";
      loadButton.UseVisualStyleBackColor = true;
      loadButton.Click += loadButton_Click;
      // 
      // sweepSelectionCombo
      // 
      sweepSelectionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      sweepSelectionCombo.FormattingEnabled = true;
      sweepSelectionCombo.Items.AddRange(new object[] { "Both", "First", "Second" });
      sweepSelectionCombo.Location = new Point(160, 251);
      sweepSelectionCombo.Name = "sweepSelectionCombo";
      sweepSelectionCombo.Size = new Size(124, 36);
      sweepSelectionCombo.TabIndex = 12;
      // 
      // NexradForm
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      ClientSize = new Size(468, 231);
      Controls.Add(sweepSelectionCombo);
      Controls.Add(loadButton);
      Controls.Add(showButton);
      Controls.Add(enableCheck);
      Controls.Add(keepCheck);
      Controls.Add(fetchTimeLabel);
      Controls.Add(label3);
      Controls.Add(intervalNumeric);
      Controls.Add(label2);
      Controls.Add(liveCheck);
      Controls.Add(fetchButton);
      Controls.Add(radarCombo);
      Controls.Add(label1);
      Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      MaximizeBox = false;
      Name = "NexradForm";
      StartPosition = FormStartPosition.Manual;
      Text = "Nexrad";
      FormClosing += NexradForm_FormClosing;
      Shown += NexradForm_Shown;
      ((System.ComponentModel.ISupportInitialize)intervalNumeric).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private ComboBox radarCombo;
    private Button fetchButton;
    private CheckBox liveCheck;
    private Label label2;
    private NumericUpDown intervalNumeric;
    private Label label3;
    private Label fetchTimeLabel;
    private CheckBox keepCheck;
    private CheckBox enableCheck;
    private Button showButton;
    private Button loadButton;
    private ComboBox sweepSelectionCombo;
  }
}