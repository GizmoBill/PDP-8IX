namespace PDP_8
{
  partial class DECtapeForm
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
      components = new System.ComponentModel.Container();
      forwardButton = new Button();
      dectape0 = new DECtapeControl.DecTapeDriveControl();
      dectape1 = new DECtapeControl.DecTapeDriveControl();
      blockLabel = new Label();
      stopButton = new Button();
      reverseButton = new Button();
      unitNumeric0 = new NumericUpDown();
      label1 = new Label();
      newButton0 = new Button();
      loadButton0 = new Button();
      saveButton0 = new Button();
      unitNumeric1 = new NumericUpDown();
      label2 = new Label();
      newButton1 = new Button();
      loadButton1 = new Button();
      saveButton1 = new Button();
      timer1 = new System.Windows.Forms.Timer(components);
      paint0TimeLabel = new Label();
      paint1TimeLabel = new Label();
      unloadButton0 = new Button();
      unloadButton1 = new Button();
      ((System.ComponentModel.ISupportInitialize)unitNumeric0).BeginInit();
      ((System.ComponentModel.ISupportInitialize)unitNumeric1).BeginInit();
      SuspendLayout();
      // 
      // forwardButton
      // 
      forwardButton.AutoSize = true;
      forwardButton.Font = new Font("Segoe UI", 10F);
      forwardButton.Location = new Point(187, 714);
      forwardButton.Name = "forwardButton";
      forwardButton.Size = new Size(94, 38);
      forwardButton.TabIndex = 3;
      forwardButton.Text = "Forward";
      forwardButton.UseVisualStyleBackColor = true;
      forwardButton.Click += forwardButton_Click;
      // 
      // dectape0
      // 
      dectape0.BackColor = Color.Transparent;
      dectape0.BaseLinearSpeed = 250F;
      dectape0.Font = new Font("Consolas", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
      dectape0.LeftReelLabel = "";
      dectape0.Location = new Point(12, 61);
      dectape0.MinimumSize = new Size(220, 120);
      dectape0.Name = "dectape0";
      dectape0.ReelRadiusLogical = 50;
      dectape0.RightReelLabel = "MIT";
      dectape0.Size = new Size(442, 280);
      dectape0.TabIndex = 4;
      dectape0.TapeLoaded = false;
      dectape0.UseInternalTimer = false;
      // 
      // dectape1
      // 
      dectape1.BackColor = Color.Transparent;
      dectape1.BaseLinearSpeed = 250F;
      dectape1.Font = new Font("Consolas", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
      dectape1.LeftReelLabel = "";
      dectape1.Location = new Point(12, 413);
      dectape1.MinimumSize = new Size(220, 120);
      dectape1.Name = "dectape1";
      dectape1.ReelRadiusLogical = 50;
      dectape1.RightReelLabel = "HRP";
      dectape1.Size = new Size(442, 280);
      dectape1.TabIndex = 4;
      dectape1.TapeLoaded = false;
      dectape1.UseInternalTimer = false;
      // 
      // blockLabel
      // 
      blockLabel.BorderStyle = BorderStyle.Fixed3D;
      blockLabel.Location = new Point(296, 709);
      blockLabel.Name = "blockLabel";
      blockLabel.Size = new Size(104, 46);
      blockLabel.TabIndex = 5;
      blockLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // stopButton
      // 
      stopButton.AutoSize = true;
      stopButton.Font = new Font("Segoe UI", 10F);
      stopButton.Location = new Point(118, 714);
      stopButton.Name = "stopButton";
      stopButton.Size = new Size(63, 38);
      stopButton.TabIndex = 2;
      stopButton.Text = "Stop";
      stopButton.UseVisualStyleBackColor = true;
      stopButton.Click += stopButton_Click;
      // 
      // reverseButton
      // 
      reverseButton.AutoSize = true;
      reverseButton.Font = new Font("Segoe UI", 10F);
      reverseButton.Location = new Point(24, 714);
      reverseButton.Name = "reverseButton";
      reverseButton.Size = new Size(88, 38);
      reverseButton.TabIndex = 1;
      reverseButton.Text = "Reverse";
      reverseButton.UseVisualStyleBackColor = true;
      reverseButton.Click += reverseButton_Click;
      // 
      // unitNumeric0
      // 
      unitNumeric0.Location = new Point(65, 14);
      unitNumeric0.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
      unitNumeric0.Name = "unitNumeric0";
      unitNumeric0.Size = new Size(54, 39);
      unitNumeric0.TabIndex = 6;
      unitNumeric0.TextAlign = HorizontalAlignment.Right;
      unitNumeric0.Value = new decimal(new int[] { 1, 0, 0, 0 });
      unitNumeric0.ValueChanged += unit0Numeric_ValueChanged;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 18);
      label1.Name = "label1";
      label1.Size = new Size(58, 32);
      label1.TabIndex = 7;
      label1.Text = "Unit";
      // 
      // newButton0
      // 
      newButton0.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      newButton0.Location = new Point(136, 12);
      newButton0.Name = "newButton0";
      newButton0.Size = new Size(66, 41);
      newButton0.TabIndex = 8;
      newButton0.Text = "New";
      newButton0.UseVisualStyleBackColor = true;
      newButton0.Click += newButton0_Click;
      // 
      // loadButton0
      // 
      loadButton0.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      loadButton0.Location = new Point(213, 12);
      loadButton0.Name = "loadButton0";
      loadButton0.Size = new Size(65, 41);
      loadButton0.TabIndex = 8;
      loadButton0.Text = "Load";
      loadButton0.UseVisualStyleBackColor = true;
      loadButton0.Click += loadButton0_Click;
      // 
      // saveButton0
      // 
      saveButton0.Enabled = false;
      saveButton0.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      saveButton0.Location = new Point(289, 12);
      saveButton0.Name = "saveButton0";
      saveButton0.Size = new Size(61, 41);
      saveButton0.TabIndex = 8;
      saveButton0.Text = "Save";
      saveButton0.UseVisualStyleBackColor = true;
      saveButton0.Click += saveButton0_Click;
      // 
      // unitNumeric1
      // 
      unitNumeric1.Location = new Point(65, 368);
      unitNumeric1.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
      unitNumeric1.Name = "unitNumeric1";
      unitNumeric1.Size = new Size(54, 39);
      unitNumeric1.TabIndex = 6;
      unitNumeric1.TextAlign = HorizontalAlignment.Right;
      unitNumeric1.Value = new decimal(new int[] { 2, 0, 0, 0 });
      unitNumeric1.ValueChanged += unit0Numeric_ValueChanged;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(12, 370);
      label2.Name = "label2";
      label2.Size = new Size(58, 32);
      label2.TabIndex = 7;
      label2.Text = "Unit";
      // 
      // newButton1
      // 
      newButton1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      newButton1.Location = new Point(136, 361);
      newButton1.Name = "newButton1";
      newButton1.Size = new Size(66, 41);
      newButton1.TabIndex = 8;
      newButton1.Text = "New";
      newButton1.UseVisualStyleBackColor = true;
      newButton1.Click += newButton0_Click;
      // 
      // loadButton1
      // 
      loadButton1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      loadButton1.Location = new Point(213, 361);
      loadButton1.Name = "loadButton1";
      loadButton1.Size = new Size(65, 41);
      loadButton1.TabIndex = 8;
      loadButton1.Text = "Load";
      loadButton1.UseVisualStyleBackColor = true;
      loadButton1.Click += loadButton0_Click;
      // 
      // saveButton1
      // 
      saveButton1.Enabled = false;
      saveButton1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      saveButton1.Location = new Point(289, 361);
      saveButton1.Name = "saveButton1";
      saveButton1.Size = new Size(61, 41);
      saveButton1.TabIndex = 8;
      saveButton1.Text = "Save";
      saveButton1.UseVisualStyleBackColor = true;
      saveButton1.Click += saveButton0_Click;
      // 
      // timer1
      // 
      timer1.Interval = 30;
      timer1.Tick += timer1_Tick;
      // 
      // paint0TimeLabel
      // 
      paint0TimeLabel.BorderStyle = BorderStyle.Fixed3D;
      paint0TimeLabel.Location = new Point(21, 769);
      paint0TimeLabel.Name = "paint0TimeLabel";
      paint0TimeLabel.Size = new Size(104, 46);
      paint0TimeLabel.TabIndex = 5;
      paint0TimeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // paint1TimeLabel
      // 
      paint1TimeLabel.BorderStyle = BorderStyle.Fixed3D;
      paint1TimeLabel.Location = new Point(141, 769);
      paint1TimeLabel.Name = "paint1TimeLabel";
      paint1TimeLabel.Size = new Size(104, 46);
      paint1TimeLabel.TabIndex = 5;
      paint1TimeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // unloadButton0
      // 
      unloadButton0.Enabled = false;
      unloadButton0.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      unloadButton0.Location = new Point(361, 13);
      unloadButton0.Name = "unloadButton0";
      unloadButton0.Size = new Size(91, 41);
      unloadButton0.TabIndex = 8;
      unloadButton0.Text = "Unload";
      unloadButton0.UseVisualStyleBackColor = true;
      unloadButton0.Click += unloadButton0_Click;
      // 
      // unloadButton1
      // 
      unloadButton1.Enabled = false;
      unloadButton1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      unloadButton1.Location = new Point(361, 361);
      unloadButton1.Name = "unloadButton1";
      unloadButton1.Size = new Size(91, 41);
      unloadButton1.TabIndex = 8;
      unloadButton1.Text = "Unload";
      unloadButton1.UseVisualStyleBackColor = true;
      unloadButton1.Click += unloadButton0_Click;
      // 
      // DECtapeForm
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      ClientSize = new Size(471, 704);
      Controls.Add(saveButton1);
      Controls.Add(loadButton1);
      Controls.Add(unloadButton1);
      Controls.Add(unloadButton0);
      Controls.Add(saveButton0);
      Controls.Add(newButton1);
      Controls.Add(loadButton0);
      Controls.Add(label2);
      Controls.Add(newButton0);
      Controls.Add(unitNumeric1);
      Controls.Add(label1);
      Controls.Add(unitNumeric0);
      Controls.Add(paint1TimeLabel);
      Controls.Add(paint0TimeLabel);
      Controls.Add(blockLabel);
      Controls.Add(dectape1);
      Controls.Add(dectape0);
      Controls.Add(forwardButton);
      Controls.Add(stopButton);
      Controls.Add(reverseButton);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      Margin = new Padding(4);
      MaximizeBox = false;
      Name = "DECtapeForm";
      StartPosition = FormStartPosition.Manual;
      Text = "TU55";
      FormClosing += DECtapeForm_FormClosing;
      Shown += DECtapeForm_Shown;
      DoubleClick += DECtapeForm_DoubleClick;
      ((System.ComponentModel.ISupportInitialize)unitNumeric0).EndInit();
      ((System.ComponentModel.ISupportInitialize)unitNumeric1).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
    private Button forwardButton;
    private DECtapeControl.DecTapeDriveControl dectape0;
    private DECtapeControl.DecTapeDriveControl dectape1;
    private Label blockLabel;
    private Button stopButton;
    private Button reverseButton;
    private NumericUpDown unitNumeric0;
    private Label label1;
    private Button newButton0;
    private Button loadButton0;
    private Button saveButton0;
    private NumericUpDown unitNumeric1;
    private Label label2;
    private Button newButton1;
    private Button loadButton1;
    private Button saveButton1;
    private System.Windows.Forms.Timer timer1;
    private Label paint0TimeLabel;
    private Label paint1TimeLabel;
    private Button unloadButton0;
    private Button unloadButton1;
  }
}