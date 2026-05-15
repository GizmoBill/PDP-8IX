namespace PDP_8
{
  partial class PPIForm
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
      colorCheck = new CheckBox();
      eraseButton = new Button();
      az0Label = new Label();
      azLabel = new Label();
      el0Label = new Label();
      elLabel = new Label();
      SuspendLayout();
      // 
      // colorCheck
      // 
      colorCheck.Anchor = AnchorStyles.None;
      colorCheck.AutoSize = true;
      colorCheck.BackColor = SystemColors.Control;
      colorCheck.ForeColor = SystemColors.ControlText;
      colorCheck.Location = new Point(680, 700);
      colorCheck.Name = "colorCheck";
      colorCheck.Size = new Size(86, 32);
      colorCheck.TabIndex = 0;
      colorCheck.Text = "Color";
      colorCheck.UseVisualStyleBackColor = false;
      colorCheck.CheckedChanged += colorCheck_CheckedChanged;
      // 
      // eraseButton
      // 
      eraseButton.Anchor = AnchorStyles.None;
      eraseButton.AutoSize = true;
      eraseButton.BackColor = SystemColors.Control;
      eraseButton.ForeColor = SystemColors.ControlText;
      eraseButton.Location = new Point(694, 656);
      eraseButton.Name = "eraseButton";
      eraseButton.Size = new Size(72, 38);
      eraseButton.TabIndex = 1;
      eraseButton.Text = "Erase";
      eraseButton.UseVisualStyleBackColor = false;
      eraseButton.Click += eraseButton_Click;
      // 
      // az0Label
      // 
      az0Label.Anchor = AnchorStyles.None;
      az0Label.Font = new Font("Segoe UI", 10F);
      az0Label.Location = new Point(84, 83);
      az0Label.Name = "az0Label";
      az0Label.Size = new Size(110, 28);
      az0Label.TabIndex = 2;
      az0Label.Text = "Azimuth";
      az0Label.TextAlign = ContentAlignment.TopCenter;
      // 
      // azLabel
      // 
      azLabel.Anchor = AnchorStyles.None;
      azLabel.BackColor = Color.Black;
      azLabel.BorderStyle = BorderStyle.Fixed3D;
      azLabel.Font = new Font("Consolas", 16F);
      azLabel.ForeColor = Color.FromArgb(255, 255, 192);
      azLabel.Location = new Point(97, 115);
      azLabel.Name = "azLabel";
      azLabel.Size = new Size(110, 39);
      azLabel.TabIndex = 3;
      azLabel.Text = "123.4";
      azLabel.TextAlign = ContentAlignment.MiddleRight;
      // 
      // el0Label
      // 
      el0Label.Anchor = AnchorStyles.None;
      el0Label.Font = new Font("Segoe UI", 10F);
      el0Label.Location = new Point(480, 83);
      el0Label.Name = "el0Label";
      el0Label.Size = new Size(94, 28);
      el0Label.TabIndex = 2;
      el0Label.Text = "Elevation";
      // 
      // elLabel
      // 
      elLabel.Anchor = AnchorStyles.None;
      elLabel.BackColor = Color.Black;
      elLabel.BorderStyle = BorderStyle.Fixed3D;
      elLabel.Font = new Font("Consolas", 16F);
      elLabel.ForeColor = Color.FromArgb(255, 255, 192);
      elLabel.Location = new Point(515, 115);
      elLabel.Name = "elLabel";
      elLabel.Size = new Size(94, 39);
      elLabel.TabIndex = 3;
      elLabel.Text = "12.3";
      elLabel.TextAlign = ContentAlignment.MiddleRight;
      // 
      // PPIForm
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackgroundImageLayout = ImageLayout.Zoom;
      ClientSize = new Size(778, 744);
      Controls.Add(elLabel);
      Controls.Add(azLabel);
      Controls.Add(el0Label);
      Controls.Add(az0Label);
      Controls.Add(eraseButton);
      Controls.Add(colorCheck);
      DoubleBuffered = true;
      Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      MinimumSize = new Size(250, 250);
      Name = "PPIForm";
      StartPosition = FormStartPosition.Manual;
      Text = "PPI";
      FormClosing += PPIForm_FormClosing;
      Shown += PPIForm_Shown;
      Paint += PPIForm_Paint;
      Resize += PPIForm_Resize;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
    private CheckBox colorCheck;
    private Button eraseButton;
    private Label az0Label;
    private Label azLabel;
    private Label el0Label;
    private Label elLabel;
  }
}