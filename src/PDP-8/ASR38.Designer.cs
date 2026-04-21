namespace PDP_8
{
  partial class ASR38
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
      kbShape = new ShapeControls.ShapeControl();
      kbLabel = new Label();
      label2 = new Label();
      prShape = new ShapeControls.ShapeControl();
      prLabel = new Label();
      label3 = new Label();
      printSpeedNumeric = new NumericUpDown();
      statusPanel = new Panel();
      vt100Check = new CheckBox();
      paperText = new TeletypeRichTextBox();
      ((System.ComponentModel.ISupportInitialize)printSpeedNumeric).BeginInit();
      statusPanel.SuspendLayout();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(18, 18);
      label1.Name = "label1";
      label1.Size = new Size(42, 32);
      label1.TabIndex = 1;
      label1.Text = "KB";
      // 
      // kbShape
      // 
      kbShape.BackColor = Color.Transparent;
      kbShape.CornerRadius = 12;
      kbShape.FillColor = Color.LightBlue;
      kbShape.Location = new Point(78, 18);
      kbShape.Name = "kbShape";
      kbShape.Shape = ShapeControls.ShapeControl.ShapeKind.Ellipse;
      kbShape.Size = new Size(32, 32);
      kbShape.TabIndex = 2;
      kbShape.TabStop = false;
      kbShape.Text = "shapeControl1";
      // 
      // kbLabel
      // 
      kbLabel.BorderStyle = BorderStyle.Fixed3D;
      kbLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      kbLabel.Location = new Point(129, 17);
      kbLabel.Name = "kbLabel";
      kbLabel.Size = new Size(75, 40);
      kbLabel.TabIndex = 3;
      kbLabel.Text = "000";
      kbLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(222, 18);
      label2.Name = "label2";
      label2.Size = new Size(41, 32);
      label2.TabIndex = 1;
      label2.Text = "PR";
      // 
      // prShape
      // 
      prShape.BackColor = Color.Transparent;
      prShape.CornerRadius = 12;
      prShape.FillColor = Color.LightBlue;
      prShape.Location = new Point(282, 18);
      prShape.Name = "prShape";
      prShape.Shape = ShapeControls.ShapeControl.ShapeKind.Ellipse;
      prShape.Size = new Size(32, 32);
      prShape.TabIndex = 2;
      prShape.TabStop = false;
      prShape.Text = "shapeControl1";
      // 
      // prLabel
      // 
      prLabel.BorderStyle = BorderStyle.Fixed3D;
      prLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      prLabel.Location = new Point(333, 17);
      prLabel.Name = "prLabel";
      prLabel.Size = new Size(75, 40);
      prLabel.TabIndex = 3;
      prLabel.Text = "000";
      prLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(665, 23);
      label3.Name = "label3";
      label3.Size = new Size(134, 32);
      label3.TabIndex = 4;
      label3.Text = "Log10(CPS)";
      // 
      // printSpeedNumeric
      // 
      printSpeedNumeric.Location = new Point(825, 18);
      printSpeedNumeric.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
      printSpeedNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
      printSpeedNumeric.Name = "printSpeedNumeric";
      printSpeedNumeric.Size = new Size(62, 39);
      printSpeedNumeric.TabIndex = 5;
      printSpeedNumeric.TabStop = false;
      printSpeedNumeric.Tag = "log10cps";
      printSpeedNumeric.TextAlign = HorizontalAlignment.Center;
      printSpeedNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
      printSpeedNumeric.ValueChanged += printSpeedNumeric_ValueChanged;
      // 
      // statusPanel
      // 
      statusPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      statusPanel.Controls.Add(vt100Check);
      statusPanel.Controls.Add(label3);
      statusPanel.Controls.Add(printSpeedNumeric);
      statusPanel.Controls.Add(label1);
      statusPanel.Controls.Add(kbShape);
      statusPanel.Controls.Add(prLabel);
      statusPanel.Controls.Add(label2);
      statusPanel.Controls.Add(kbLabel);
      statusPanel.Controls.Add(prShape);
      statusPanel.Location = new Point(0, 0);
      statusPanel.Name = "statusPanel";
      statusPanel.Size = new Size(1154, 77);
      statusPanel.TabIndex = 6;
      // 
      // vt100Check
      // 
      vt100Check.AutoSize = true;
      vt100Check.Location = new Point(435, 17);
      vt100Check.Name = "vt100Check";
      vt100Check.Size = new Size(107, 36);
      vt100Check.TabIndex = 6;
      vt100Check.TabStop = false;
      vt100Check.Text = "VT100";
      vt100Check.UseVisualStyleBackColor = true;
      vt100Check.CheckedChanged += vt100Check_CheckedChanged;
      // 
      // paperText
      // 
      paperText.AcceptsTab = true;
      paperText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      paperText.DetectUrls = false;
      paperText.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      paperText.Location = new Point(0, 78);
      paperText.Name = "paperText";
      paperText.ReadOnly = true;
      paperText.Size = new Size(1145, 1063);
      paperText.TabIndex = 7;
      paperText.TabStop = false;
      paperText.Text = "";
      paperText.Click += paperText_Click;
      paperText.KeyDown += paperText_KeyDown;
      paperText.KeyPress += paperText_KeyPress;
      // 
      // ASR38
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      ClientSize = new Size(1152, 1146);
      Controls.Add(paperText);
      Controls.Add(statusPanel);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      Margin = new Padding(4, 3, 4, 3);
      Name = "ASR38";
      StartPosition = FormStartPosition.Manual;
      Text = "ASR38";
      FormClosing += ASR38_FormClosing;
      Shown += ASR38_Shown;
      ((System.ComponentModel.ISupportInitialize)printSpeedNumeric).EndInit();
      statusPanel.ResumeLayout(false);
      statusPanel.PerformLayout();
      ResumeLayout(false);
    }

    #endregion
    private Label label1;
    private ShapeControls.ShapeControl kbShape;
    private Label kbLabel;
    private Label label2;
    private ShapeControls.ShapeControl prShape;
    private Label prLabel;
    private Label label3;
    private NumericUpDown printSpeedNumeric;
    private Panel statusPanel;
    private TeletypeRichTextBox paperText;
    private CheckBox vt100Check;
  }
}