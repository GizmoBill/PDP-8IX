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
      label1 = new Label();
      oswLabel = new Label();
      tls2Numeric = new NumericUpDown();
      tlsButton = new Button();
      pictureBox1 = new PictureBox();
      groupBox1 = new GroupBox();
      wr66ElNumeric = new NumericUpDown();
      wr66AzNumeric = new NumericUpDown();
      label3 = new Label();
      label2 = new Label();
      groupBox2 = new GroupBox();
      wr73ElNumeric = new NumericUpDown();
      wr73AzNumeric = new NumericUpDown();
      label4 = new Label();
      label5 = new Label();
      label6 = new Label();
      wr66ServoLowLabel = new Label();
      wr66ServoHighLabel = new Label();
      label9 = new Label();
      label10 = new Label();
      label7 = new Label();
      icw1Label = new Label();
      icw2Label = new Label();
      label12 = new Label();
      label13 = new Label();
      ((System.ComponentModel.ISupportInitialize)tls2Numeric).BeginInit();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)wr66ElNumeric).BeginInit();
      ((System.ComponentModel.ISupportInitialize)wr66AzNumeric).BeginInit();
      groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)wr73ElNumeric).BeginInit();
      ((System.ComponentModel.ISupportInitialize)wr73AzNumeric).BeginInit();
      SuspendLayout();
      // 
      // sevenSegLabel
      // 
      sevenSegLabel.BackColor = Color.White;
      sevenSegLabel.BorderStyle = BorderStyle.Fixed3D;
      sevenSegLabel.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      sevenSegLabel.Location = new Point(62, 166);
      sevenSegLabel.Name = "sevenSegLabel";
      sevenSegLabel.Size = new Size(56, 53);
      sevenSegLabel.TabIndex = 10;
      sevenSegLabel.Text = "00";
      sevenSegLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      label1.Location = new Point(12, 21);
      label1.Name = "label1";
      label1.Size = new Size(69, 32);
      label1.TabIndex = 11;
      label1.Text = "OSW";
      // 
      // oswLabel
      // 
      oswLabel.BackColor = Color.White;
      oswLabel.BorderStyle = BorderStyle.Fixed3D;
      oswLabel.Location = new Point(96, 21);
      oswLabel.Name = "oswLabel";
      oswLabel.Size = new Size(68, 34);
      oswLabel.TabIndex = 12;
      oswLabel.Text = "0000";
      // 
      // tls2Numeric
      // 
      tls2Numeric.Location = new Point(96, 92);
      tls2Numeric.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
      tls2Numeric.Name = "tls2Numeric";
      tls2Numeric.Size = new Size(68, 39);
      tls2Numeric.TabIndex = 13;
      tls2Numeric.TextAlign = HorizontalAlignment.Center;
      // 
      // tlsButton
      // 
      tlsButton.BackColor = SystemColors.Control;
      tlsButton.Location = new Point(12, 88);
      tlsButton.Name = "tlsButton";
      tlsButton.Size = new Size(69, 45);
      tlsButton.TabIndex = 14;
      tlsButton.Text = "TLS";
      tlsButton.UseVisualStyleBackColor = false;
      tlsButton.Click += tlsButton_Click;
      // 
      // pictureBox1
      // 
      pictureBox1.Image = Properties.Resources._160406_F_LA452_003;
      pictureBox1.Location = new Point(243, 357);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(142, 165);
      pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      pictureBox1.TabIndex = 15;
      pictureBox1.TabStop = false;
      // 
      // groupBox1
      // 
      groupBox1.BackColor = Color.FromArgb(224, 224, 224);
      groupBox1.Controls.Add(wr66ElNumeric);
      groupBox1.Controls.Add(wr66AzNumeric);
      groupBox1.Controls.Add(label3);
      groupBox1.Controls.Add(label2);
      groupBox1.Location = new Point(190, 9);
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new Size(191, 140);
      groupBox1.TabIndex = 16;
      groupBox1.TabStop = false;
      groupBox1.Text = "WR66";
      // 
      // wr66ElNumeric
      // 
      wr66ElNumeric.DecimalPlaces = 1;
      wr66ElNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
      wr66ElNumeric.Location = new Point(71, 81);
      wr66ElNumeric.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
      wr66ElNumeric.Name = "wr66ElNumeric";
      wr66ElNumeric.Size = new Size(100, 39);
      wr66ElNumeric.TabIndex = 1;
      wr66ElNumeric.TextAlign = HorizontalAlignment.Right;
      wr66ElNumeric.Value = new decimal(new int[] { 5, 0, 0, 65536 });
      // 
      // wr66AzNumeric
      // 
      wr66AzNumeric.DecimalPlaces = 1;
      wr66AzNumeric.Location = new Point(71, 36);
      wr66AzNumeric.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
      wr66AzNumeric.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
      wr66AzNumeric.Name = "wr66AzNumeric";
      wr66AzNumeric.Size = new Size(100, 39);
      wr66AzNumeric.TabIndex = 1;
      wr66AzNumeric.TextAlign = HorizontalAlignment.Right;
      wr66AzNumeric.ValueChanged += wr66AzNumeric_ValueChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(28, 83);
      label3.Name = "label3";
      label3.Size = new Size(37, 32);
      label3.TabIndex = 0;
      label3.Text = "EL";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(28, 38);
      label2.Name = "label2";
      label2.Size = new Size(44, 32);
      label2.TabIndex = 0;
      label2.Text = "AZ";
      // 
      // groupBox2
      // 
      groupBox2.BackColor = Color.FromArgb(224, 224, 224);
      groupBox2.Controls.Add(wr73ElNumeric);
      groupBox2.Controls.Add(wr73AzNumeric);
      groupBox2.Controls.Add(label4);
      groupBox2.Controls.Add(label5);
      groupBox2.Location = new Point(190, 166);
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new Size(191, 140);
      groupBox2.TabIndex = 16;
      groupBox2.TabStop = false;
      groupBox2.Text = "WR73";
      // 
      // wr73ElNumeric
      // 
      wr73ElNumeric.DecimalPlaces = 1;
      wr73ElNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
      wr73ElNumeric.Location = new Point(71, 81);
      wr73ElNumeric.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
      wr73ElNumeric.Name = "wr73ElNumeric";
      wr73ElNumeric.Size = new Size(100, 39);
      wr73ElNumeric.TabIndex = 1;
      wr73ElNumeric.TextAlign = HorizontalAlignment.Right;
      wr73ElNumeric.Value = new decimal(new int[] { 5, 0, 0, 65536 });
      // 
      // wr73AzNumeric
      // 
      wr73AzNumeric.DecimalPlaces = 1;
      wr73AzNumeric.Location = new Point(71, 36);
      wr73AzNumeric.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
      wr73AzNumeric.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
      wr73AzNumeric.Name = "wr73AzNumeric";
      wr73AzNumeric.Size = new Size(100, 39);
      wr73AzNumeric.TabIndex = 1;
      wr73AzNumeric.TextAlign = HorizontalAlignment.Right;
      wr73AzNumeric.ValueChanged += wr66AzNumeric_ValueChanged;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(28, 83);
      label4.Name = "label4";
      label4.Size = new Size(37, 32);
      label4.TabIndex = 0;
      label4.Text = "EL";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(28, 38);
      label5.Name = "label5";
      label5.Size = new Size(44, 32);
      label5.TabIndex = 0;
      label5.Text = "AZ";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      label6.Location = new Point(21, 400);
      label6.Name = "label6";
      label6.Size = new Size(143, 32);
      label6.TabIndex = 11;
      label6.Text = "WR66 Servo";
      // 
      // wr66ServoLowLabel
      // 
      wr66ServoLowLabel.BackColor = Color.White;
      wr66ServoLowLabel.BorderStyle = BorderStyle.Fixed3D;
      wr66ServoLowLabel.Location = new Point(77, 443);
      wr66ServoLowLabel.Name = "wr66ServoLowLabel";
      wr66ServoLowLabel.Size = new Size(68, 34);
      wr66ServoLowLabel.TabIndex = 12;
      wr66ServoLowLabel.Text = "0000";
      // 
      // wr66ServoHighLabel
      // 
      wr66ServoHighLabel.BackColor = Color.White;
      wr66ServoHighLabel.BorderStyle = BorderStyle.Fixed3D;
      wr66ServoHighLabel.Location = new Point(77, 490);
      wr66ServoHighLabel.Name = "wr66ServoHighLabel";
      wr66ServoHighLabel.Size = new Size(68, 34);
      wr66ServoHighLabel.TabIndex = 12;
      wr66ServoHighLabel.Text = "0000";
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Location = new Point(16, 444);
      label9.Name = "label9";
      label9.Size = new Size(51, 32);
      label9.TabIndex = 17;
      label9.Text = "low";
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Location = new Point(9, 490);
      label10.Name = "label10";
      label10.Size = new Size(62, 32);
      label10.TabIndex = 17;
      label10.Text = "high";
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      label7.Location = new Point(21, 254);
      label7.Name = "label7";
      label7.Size = new Size(119, 32);
      label7.TabIndex = 11;
      label7.Text = "Integrator";
      // 
      // icw1Label
      // 
      icw1Label.BackColor = Color.White;
      icw1Label.BorderStyle = BorderStyle.Fixed3D;
      icw1Label.Location = new Point(77, 297);
      icw1Label.Name = "icw1Label";
      icw1Label.Size = new Size(68, 34);
      icw1Label.TabIndex = 12;
      icw1Label.Text = "0000";
      // 
      // icw2Label
      // 
      icw2Label.BackColor = Color.White;
      icw2Label.BorderStyle = BorderStyle.Fixed3D;
      icw2Label.Location = new Point(77, 344);
      icw2Label.Name = "icw2Label";
      icw2Label.Size = new Size(68, 34);
      icw2Label.TabIndex = 12;
      icw2Label.Text = "0000";
      // 
      // label12
      // 
      label12.AutoSize = true;
      label12.Location = new Point(11, 299);
      label12.Name = "label12";
      label12.Size = new Size(70, 32);
      label12.TabIndex = 17;
      label12.Text = "ICW1";
      // 
      // label13
      // 
      label13.AutoSize = true;
      label13.Location = new Point(9, 344);
      label13.Name = "label13";
      label13.Size = new Size(70, 32);
      label13.TabIndex = 17;
      label13.Text = "ICW2";
      // 
      // HRPForm
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackColor = Color.FromArgb(157, 195, 230);
      ClientSize = new Size(397, 544);
      Controls.Add(label13);
      Controls.Add(label10);
      Controls.Add(label12);
      Controls.Add(label9);
      Controls.Add(pictureBox1);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(tlsButton);
      Controls.Add(tls2Numeric);
      Controls.Add(icw2Label);
      Controls.Add(wr66ServoHighLabel);
      Controls.Add(icw1Label);
      Controls.Add(wr66ServoLowLabel);
      Controls.Add(oswLabel);
      Controls.Add(label7);
      Controls.Add(label6);
      Controls.Add(label1);
      Controls.Add(sevenSegLabel);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      Margin = new Padding(4);
      MaximizeBox = false;
      Name = "HRPForm";
      StartPosition = FormStartPosition.Manual;
      Text = "Hurricane Research Project";
      FormClosing += HRPForm_FormClosing;
      ((System.ComponentModel.ISupportInitialize)tls2Numeric).EndInit();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)wr66ElNumeric).EndInit();
      ((System.ComponentModel.ISupportInitialize)wr66AzNumeric).EndInit();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)wr73ElNumeric).EndInit();
      ((System.ComponentModel.ISupportInitialize)wr73AzNumeric).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label sevenSegLabel;
    private Label label1;
    private Label oswLabel;
    private NumericUpDown tls2Numeric;
    private Button tlsButton;
    private PictureBox pictureBox1;
    private GroupBox groupBox1;
    private NumericUpDown wr66ElNumeric;
    private NumericUpDown wr66AzNumeric;
    private Label label3;
    private Label label2;
    private GroupBox groupBox2;
    private NumericUpDown wr73ElNumeric;
    private NumericUpDown wr73AzNumeric;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label wr66ServoLowLabel;
    private Label wr66ServoHighLabel;
    private Label label9;
    private Label label10;
    private Label label7;
    private Label icw1Label;
    private Label icw2Label;
    private Label label12;
    private Label label13;
  }
}