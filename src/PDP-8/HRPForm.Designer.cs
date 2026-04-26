namespace PDP_8
{
  partial class HRPForm
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
      sevenSegLabel = new Label();
      pictureBox1 = new PictureBox();
      groupBox1 = new GroupBox();
      wr66ElNumeric = new NumericUpDown();
      wr66AzNumeric = new NumericUpDown();
      label3 = new Label();
      label2 = new Label();
      wr66AzVelLabel = new Label();
      wr66ElVelLabel = new Label();
      groupBox2 = new GroupBox();
      wr73ElNumeric = new NumericUpDown();
      wr73AzNumeric = new NumericUpDown();
      label4 = new Label();
      label5 = new Label();
      wr73ElSetLabel = new Label();
      wr73AzVelLabel = new Label();
      esrCombo = new ComboBox();
      nextButton = new Button();
      label8 = new Label();
      spyCombo = new ComboBox();
      label11 = new Label();
      label14 = new Label();
      label15 = new Label();
      intRadarSelLabel = new Label();
      label17 = new Label();
      zeroButton = new Button();
      runCheck = new CheckBox();
      spyButton = new Button();
      radarSelButton = new Button();
      oswRadarSelLabel = new Label();
      intSkipLabel = new Label();
      intResLabel = new Label();
      intPulsesLabel = new Label();
      intTNPLabel = new Label();
      groupBox3 = new GroupBox();
      groupBox4 = new GroupBox();
      groupBox5 = new GroupBox();
      panel1 = new Panel();
      aToD1Track = new TrackBar();
      aToD2Track = new TrackBar();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)wr66ElNumeric).BeginInit();
      ((System.ComponentModel.ISupportInitialize)wr66AzNumeric).BeginInit();
      groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)wr73ElNumeric).BeginInit();
      ((System.ComponentModel.ISupportInitialize)wr73AzNumeric).BeginInit();
      groupBox3.SuspendLayout();
      groupBox4.SuspendLayout();
      groupBox5.SuspendLayout();
      panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)aToD1Track).BeginInit();
      ((System.ComponentModel.ISupportInitialize)aToD2Track).BeginInit();
      SuspendLayout();
      // 
      // sevenSegLabel
      // 
      sevenSegLabel.BackColor = Color.White;
      sevenSegLabel.BorderStyle = BorderStyle.Fixed3D;
      sevenSegLabel.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      sevenSegLabel.Location = new Point(105, 75);
      sevenSegLabel.Name = "sevenSegLabel";
      sevenSegLabel.Size = new Size(56, 53);
      sevenSegLabel.TabIndex = 10;
      sevenSegLabel.Text = "00";
      sevenSegLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // pictureBox1
      // 
      pictureBox1.Image = Properties.Resources._160406_F_LA452_003;
      pictureBox1.Location = new Point(286, 459);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(142, 165);
      pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      pictureBox1.TabIndex = 15;
      pictureBox1.TabStop = false;
      // 
      // groupBox1
      // 
      groupBox1.BackColor = Color.FromArgb(160, 192, 230);
      groupBox1.Controls.Add(wr66ElNumeric);
      groupBox1.Controls.Add(wr66AzNumeric);
      groupBox1.Controls.Add(label3);
      groupBox1.Controls.Add(label2);
      groupBox1.Controls.Add(wr66AzVelLabel);
      groupBox1.Controls.Add(wr66ElVelLabel);
      groupBox1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      groupBox1.Location = new Point(241, 12);
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new Size(240, 127);
      groupBox1.TabIndex = 16;
      groupBox1.TabStop = false;
      groupBox1.Text = "WR66";
      // 
      // wr66ElNumeric
      // 
      wr66ElNumeric.DecimalPlaces = 1;
      wr66ElNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
      wr66ElNumeric.Location = new Point(58, 81);
      wr66ElNumeric.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
      wr66ElNumeric.Name = "wr66ElNumeric";
      wr66ElNumeric.Size = new Size(100, 34);
      wr66ElNumeric.TabIndex = 1;
      wr66ElNumeric.Tag = "wr66El";
      wr66ElNumeric.TextAlign = HorizontalAlignment.Right;
      wr66ElNumeric.Value = new decimal(new int[] { 5, 0, 0, 65536 });
      // 
      // wr66AzNumeric
      // 
      wr66AzNumeric.DecimalPlaces = 1;
      wr66AzNumeric.Location = new Point(58, 36);
      wr66AzNumeric.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
      wr66AzNumeric.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
      wr66AzNumeric.Name = "wr66AzNumeric";
      wr66AzNumeric.Size = new Size(100, 34);
      wr66AzNumeric.TabIndex = 1;
      wr66AzNumeric.Tag = "wr66Az";
      wr66AzNumeric.TextAlign = HorizontalAlignment.Right;
      wr66AzNumeric.ValueChanged += wr66AzNumeric_ValueChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(15, 83);
      label3.Name = "label3";
      label3.Size = new Size(33, 28);
      label3.TabIndex = 0;
      label3.Text = "EL";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(15, 38);
      label2.Name = "label2";
      label2.Size = new Size(38, 28);
      label2.TabIndex = 0;
      label2.Text = "AZ";
      // 
      // wr66AzVelLabel
      // 
      wr66AzVelLabel.BackColor = Color.White;
      wr66AzVelLabel.BorderStyle = BorderStyle.Fixed3D;
      wr66AzVelLabel.Location = new Point(164, 38);
      wr66AzVelLabel.Name = "wr66AzVelLabel";
      wr66AzVelLabel.Size = new Size(68, 34);
      wr66AzVelLabel.TabIndex = 12;
      wr66AzVelLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // wr66ElVelLabel
      // 
      wr66ElVelLabel.BackColor = Color.White;
      wr66ElVelLabel.BorderStyle = BorderStyle.Fixed3D;
      wr66ElVelLabel.Location = new Point(164, 81);
      wr66ElVelLabel.Name = "wr66ElVelLabel";
      wr66ElVelLabel.Size = new Size(68, 34);
      wr66ElVelLabel.TabIndex = 12;
      wr66ElVelLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // groupBox2
      // 
      groupBox2.BackColor = Color.FromArgb(160, 192, 230);
      groupBox2.Controls.Add(wr73ElNumeric);
      groupBox2.Controls.Add(wr73AzNumeric);
      groupBox2.Controls.Add(label4);
      groupBox2.Controls.Add(label5);
      groupBox2.Controls.Add(wr73ElSetLabel);
      groupBox2.Controls.Add(wr73AzVelLabel);
      groupBox2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      groupBox2.Location = new Point(241, 152);
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new Size(240, 128);
      groupBox2.TabIndex = 16;
      groupBox2.TabStop = false;
      groupBox2.Text = "WR73";
      // 
      // wr73ElNumeric
      // 
      wr73ElNumeric.DecimalPlaces = 1;
      wr73ElNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
      wr73ElNumeric.Location = new Point(56, 81);
      wr73ElNumeric.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
      wr73ElNumeric.Name = "wr73ElNumeric";
      wr73ElNumeric.Size = new Size(100, 34);
      wr73ElNumeric.TabIndex = 1;
      wr73ElNumeric.Tag = "wr73El";
      wr73ElNumeric.TextAlign = HorizontalAlignment.Right;
      wr73ElNumeric.Value = new decimal(new int[] { 5, 0, 0, 65536 });
      // 
      // wr73AzNumeric
      // 
      wr73AzNumeric.DecimalPlaces = 1;
      wr73AzNumeric.Location = new Point(56, 36);
      wr73AzNumeric.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
      wr73AzNumeric.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
      wr73AzNumeric.Name = "wr73AzNumeric";
      wr73AzNumeric.Size = new Size(100, 34);
      wr73AzNumeric.TabIndex = 1;
      wr73AzNumeric.Tag = "wr73Az";
      wr73AzNumeric.TextAlign = HorizontalAlignment.Right;
      wr73AzNumeric.ValueChanged += wr66AzNumeric_ValueChanged;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(13, 83);
      label4.Name = "label4";
      label4.Size = new Size(33, 28);
      label4.TabIndex = 0;
      label4.Text = "EL";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(13, 38);
      label5.Name = "label5";
      label5.Size = new Size(38, 28);
      label5.TabIndex = 0;
      label5.Text = "AZ";
      // 
      // wr73ElSetLabel
      // 
      wr73ElSetLabel.BackColor = Color.White;
      wr73ElSetLabel.BorderStyle = BorderStyle.Fixed3D;
      wr73ElSetLabel.Location = new Point(162, 80);
      wr73ElSetLabel.Name = "wr73ElSetLabel";
      wr73ElSetLabel.Size = new Size(68, 34);
      wr73ElSetLabel.TabIndex = 12;
      wr73ElSetLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // wr73AzVelLabel
      // 
      wr73AzVelLabel.BackColor = Color.White;
      wr73AzVelLabel.BorderStyle = BorderStyle.Fixed3D;
      wr73AzVelLabel.Location = new Point(162, 38);
      wr73AzVelLabel.Name = "wr73AzVelLabel";
      wr73AzVelLabel.Size = new Size(68, 34);
      wr73AzVelLabel.TabIndex = 12;
      wr73AzVelLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // esrCombo
      // 
      esrCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      esrCombo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      esrCombo.FormattingEnabled = true;
      esrCombo.Items.AddRange(new object[] { "0", "1 (dBZ KM)", "2 (AZ/EL)", "3 (Time)", "4 (Max)", "5 (dBZ KM)", "6", "7", "8", "9" });
      esrCombo.Location = new Point(110, 39);
      esrCombo.Name = "esrCombo";
      esrCombo.Size = new Size(111, 36);
      esrCombo.TabIndex = 17;
      // 
      // nextButton
      // 
      nextButton.BackColor = SystemColors.Control;
      nextButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      nextButton.Location = new Point(25, 140);
      nextButton.Name = "nextButton";
      nextButton.Size = new Size(136, 45);
      nextButton.TabIndex = 15;
      nextButton.Text = "Next Screen";
      nextButton.UseVisualStyleBackColor = false;
      nextButton.Click += nextButton_Click;
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      label8.Location = new Point(29, 83);
      label8.Name = "label8";
      label8.Size = new Size(70, 28);
      label8.TabIndex = 11;
      label8.Text = "Idle %";
      // 
      // spyCombo
      // 
      spyCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      spyCombo.DropDownWidth = 156;
      spyCombo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      spyCombo.FormattingEnabled = true;
      spyCombo.Items.AddRange(new object[] { "0", "1 (Load Addr)", "2 (Deposit)", "3 (Dep; ++Addr)", "4 (Load Field)", "5 (++Address)", "6 (--Address)", "7 (Indirect)", "8", "9" });
      spyCombo.Location = new Point(89, 47);
      spyCombo.Name = "spyCombo";
      spyCombo.Size = new Size(106, 36);
      spyCombo.TabIndex = 17;
      // 
      // label11
      // 
      label11.AutoSize = true;
      label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      label11.Location = new Point(22, 128);
      label11.Name = "label11";
      label11.Size = new Size(98, 28);
      label11.TabIndex = 11;
      label11.Text = "Skip Bins";
      // 
      // label14
      // 
      label14.AutoSize = true;
      label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      label14.Location = new Point(21, 84);
      label14.Name = "label14";
      label14.Size = new Size(101, 28);
      label14.TabIndex = 11;
      label14.Text = "Res (nmi)";
      // 
      // label15
      // 
      label15.AutoSize = true;
      label15.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      label15.Location = new Point(47, 171);
      label15.Name = "label15";
      label15.Size = new Size(71, 28);
      label15.TabIndex = 11;
      label15.Text = "Pulses";
      // 
      // intRadarSelLabel
      // 
      intRadarSelLabel.AutoSize = true;
      intRadarSelLabel.BackColor = Color.White;
      intRadarSelLabel.BorderStyle = BorderStyle.Fixed3D;
      intRadarSelLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      intRadarSelLabel.Location = new Point(74, 36);
      intRadarSelLabel.Name = "intRadarSelLabel";
      intRadarSelLabel.Size = new Size(71, 30);
      intRadarSelLabel.TabIndex = 11;
      intRadarSelLabel.Text = "WR66";
      // 
      // label17
      // 
      label17.AutoSize = true;
      label17.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      label17.Location = new Point(64, 213);
      label17.Name = "label17";
      label17.Size = new Size(52, 28);
      label17.TabIndex = 11;
      label17.Text = "TNP";
      // 
      // zeroButton
      // 
      zeroButton.BackColor = SystemColors.Control;
      zeroButton.Location = new Point(17, 91);
      zeroButton.Name = "zeroButton";
      zeroButton.Size = new Size(74, 45);
      zeroButton.TabIndex = 15;
      zeroButton.Text = "Zero";
      zeroButton.UseVisualStyleBackColor = false;
      zeroButton.Click += zeroButton_Click;
      // 
      // runCheck
      // 
      runCheck.AutoSize = true;
      runCheck.Location = new Point(17, 41);
      runCheck.Name = "runCheck";
      runCheck.Size = new Size(75, 32);
      runCheck.TabIndex = 19;
      runCheck.Text = "Run";
      runCheck.UseVisualStyleBackColor = true;
      // 
      // spyButton
      // 
      spyButton.BackColor = SystemColors.Control;
      spyButton.Location = new Point(9, 42);
      spyButton.Name = "spyButton";
      spyButton.Size = new Size(74, 45);
      spyButton.TabIndex = 15;
      spyButton.Text = "Spy";
      spyButton.UseVisualStyleBackColor = false;
      // 
      // radarSelButton
      // 
      radarSelButton.BackColor = SystemColors.Control;
      radarSelButton.Location = new Point(110, 91);
      radarSelButton.Name = "radarSelButton";
      radarSelButton.Size = new Size(111, 48);
      radarSelButton.TabIndex = 20;
      radarSelButton.Text = "Radar Sel";
      radarSelButton.UseVisualStyleBackColor = false;
      radarSelButton.Click += radarSelButton_Click;
      // 
      // oswRadarSelLabel
      // 
      oswRadarSelLabel.AutoSize = true;
      oswRadarSelLabel.BackColor = Color.White;
      oswRadarSelLabel.BorderStyle = BorderStyle.Fixed3D;
      oswRadarSelLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      oswRadarSelLabel.Location = new Point(56, 25);
      oswRadarSelLabel.Name = "oswRadarSelLabel";
      oswRadarSelLabel.Size = new Size(71, 30);
      oswRadarSelLabel.TabIndex = 11;
      oswRadarSelLabel.Text = "WR66";
      // 
      // intSkipLabel
      // 
      intSkipLabel.BackColor = Color.White;
      intSkipLabel.BorderStyle = BorderStyle.Fixed3D;
      intSkipLabel.Location = new Point(118, 127);
      intSkipLabel.Name = "intSkipLabel";
      intSkipLabel.Size = new Size(68, 34);
      intSkipLabel.TabIndex = 12;
      intSkipLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // intResLabel
      // 
      intResLabel.BackColor = Color.White;
      intResLabel.BorderStyle = BorderStyle.Fixed3D;
      intResLabel.Location = new Point(118, 84);
      intResLabel.Name = "intResLabel";
      intResLabel.Size = new Size(68, 34);
      intResLabel.TabIndex = 12;
      intResLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // intPulsesLabel
      // 
      intPulsesLabel.BackColor = Color.White;
      intPulsesLabel.BorderStyle = BorderStyle.Fixed3D;
      intPulsesLabel.Location = new Point(118, 170);
      intPulsesLabel.Name = "intPulsesLabel";
      intPulsesLabel.Size = new Size(68, 34);
      intPulsesLabel.TabIndex = 12;
      intPulsesLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // intTNPLabel
      // 
      intTNPLabel.BackColor = Color.White;
      intTNPLabel.BorderStyle = BorderStyle.Fixed3D;
      intTNPLabel.Location = new Point(118, 213);
      intTNPLabel.Name = "intTNPLabel";
      intTNPLabel.Size = new Size(68, 34);
      intTNPLabel.TabIndex = 12;
      intTNPLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // groupBox3
      // 
      groupBox3.BackColor = Color.FromArgb(160, 192, 230);
      groupBox3.Controls.Add(intPulsesLabel);
      groupBox3.Controls.Add(label11);
      groupBox3.Controls.Add(label14);
      groupBox3.Controls.Add(label15);
      groupBox3.Controls.Add(intSkipLabel);
      groupBox3.Controls.Add(intResLabel);
      groupBox3.Controls.Add(intTNPLabel);
      groupBox3.Controls.Add(label17);
      groupBox3.Controls.Add(intRadarSelLabel);
      groupBox3.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      groupBox3.Location = new Point(12, 235);
      groupBox3.Name = "groupBox3";
      groupBox3.Size = new Size(209, 265);
      groupBox3.TabIndex = 21;
      groupBox3.TabStop = false;
      groupBox3.Text = "Integrator";
      // 
      // groupBox4
      // 
      groupBox4.BackColor = Color.FromArgb(160, 192, 230);
      groupBox4.Controls.Add(runCheck);
      groupBox4.Controls.Add(zeroButton);
      groupBox4.Controls.Add(radarSelButton);
      groupBox4.Controls.Add(esrCombo);
      groupBox4.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      groupBox4.Location = new Point(241, 293);
      groupBox4.Name = "groupBox4";
      groupBox4.Size = new Size(242, 151);
      groupBox4.TabIndex = 22;
      groupBox4.TabStop = false;
      groupBox4.Text = "A Scope";
      // 
      // groupBox5
      // 
      groupBox5.BackColor = Color.FromArgb(160, 192, 230);
      groupBox5.Controls.Add(spyButton);
      groupBox5.Controls.Add(spyCombo);
      groupBox5.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
      groupBox5.Location = new Point(12, 518);
      groupBox5.Name = "groupBox5";
      groupBox5.Size = new Size(209, 106);
      groupBox5.TabIndex = 23;
      groupBox5.TabStop = false;
      groupBox5.Text = "Spy";
      // 
      // panel1
      // 
      panel1.BackColor = Color.FromArgb(160, 192, 230);
      panel1.Controls.Add(nextButton);
      panel1.Controls.Add(oswRadarSelLabel);
      panel1.Controls.Add(label8);
      panel1.Controls.Add(sevenSegLabel);
      panel1.Location = new Point(12, 12);
      panel1.Name = "panel1";
      panel1.Size = new Size(209, 205);
      panel1.TabIndex = 24;
      // 
      // aToD1Track
      // 
      aToD1Track.LargeChange = 128;
      aToD1Track.Location = new Point(12, 643);
      aToD1Track.Maximum = 4095;
      aToD1Track.Name = "aToD1Track";
      aToD1Track.Size = new Size(471, 69);
      aToD1Track.SmallChange = 32;
      aToD1Track.TabIndex = 25;
      aToD1Track.TickStyle = TickStyle.TopLeft;
      aToD1Track.Value = 2048;
      // 
      // aToD2Track
      // 
      aToD2Track.LargeChange = 128;
      aToD2Track.Location = new Point(12, 694);
      aToD2Track.Maximum = 4095;
      aToD2Track.Name = "aToD2Track";
      aToD2Track.Size = new Size(471, 69);
      aToD2Track.SmallChange = 32;
      aToD2Track.TabIndex = 25;
      aToD2Track.TickStyle = TickStyle.TopLeft;
      aToD2Track.Value = 2048;
      // 
      // HRPForm
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackColor = Color.Gray;
      ClientSize = new Size(504, 755);
      Controls.Add(aToD2Track);
      Controls.Add(aToD1Track);
      Controls.Add(panel1);
      Controls.Add(groupBox5);
      Controls.Add(groupBox4);
      Controls.Add(groupBox3);
      Controls.Add(pictureBox1);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      Margin = new Padding(4);
      MaximizeBox = false;
      Name = "HRPForm";
      StartPosition = FormStartPosition.Manual;
      Text = "Hurricane Research Project & MIT";
      FormClosing += HRPForm_FormClosing;
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)wr66ElNumeric).EndInit();
      ((System.ComponentModel.ISupportInitialize)wr66AzNumeric).EndInit();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)wr73ElNumeric).EndInit();
      ((System.ComponentModel.ISupportInitialize)wr73AzNumeric).EndInit();
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      groupBox4.ResumeLayout(false);
      groupBox4.PerformLayout();
      groupBox5.ResumeLayout(false);
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)aToD1Track).EndInit();
      ((System.ComponentModel.ISupportInitialize)aToD2Track).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
    private Button tlsButton;
    private PictureBox pictureBox1;
    private GroupBox groupBox1;
    private NumericUpDown wr66ElNumeric;
    private Label label3;
    private Label label2;
    private GroupBox groupBox2;
    private NumericUpDown wr73ElNumeric;
    private Label label4;
    private Label label5;
    private Button nextButton;
    private ComboBox esrCombo;
    private Label label8;
    private ComboBox spyCombo;
    private Label label11;
    private Label label14;
    private Label label15;
    private Label label17;
    private Button zeroButton;
    private CheckBox runCheck;
    private Button spyButton;
    private Button radarSelButton;
    public Label wr73AzVelLabel;
    public Label wr73ElSetLabel;
    public Label oswRadarSelLabel;
    public Label intSkipLabel;
    public Label intResLabel;
    public Label intPulsesLabel;
    public Label intTNPLabel;
    public Label wr66AzVelLabel;
    public Label wr66ElVelLabel;
    private GroupBox groupBox3;
    private GroupBox groupBox4;
    private GroupBox groupBox5;
    private Panel panel1;
    private TrackBar aToD1Track;
    public Label intRadarSelLabel;
    public Label sevenSegLabel;
    private TrackBar aToD2Track;
    public NumericUpDown wr66AzNumeric;
    public NumericUpDown wr73AzNumeric;
  }
}