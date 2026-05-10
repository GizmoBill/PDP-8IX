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
      SuspendLayout();
      // 
      // colorCheck
      // 
      colorCheck.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      colorCheck.BackColor = Color.Black;
      colorCheck.ForeColor = Color.White;
      colorCheck.Location = new Point(818, 724);
      colorCheck.Name = "colorCheck";
      colorCheck.Size = new Size(95, 32);
      colorCheck.TabIndex = 0;
      colorCheck.Text = "Color";
      colorCheck.UseVisualStyleBackColor = false;
      colorCheck.CheckedChanged += colorCheck_CheckedChanged;
      // 
      // eraseButton
      // 
      eraseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      eraseButton.AutoSize = true;
      eraseButton.BackColor = Color.Black;
      eraseButton.ForeColor = Color.White;
      eraseButton.Location = new Point(826, 669);
      eraseButton.Name = "eraseButton";
      eraseButton.Size = new Size(87, 49);
      eraseButton.TabIndex = 1;
      eraseButton.Text = "Erase";
      eraseButton.UseVisualStyleBackColor = false;
      eraseButton.Click += eraseButton_Click;
      // 
      // PPIForm
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackgroundImageLayout = ImageLayout.Zoom;
      ClientSize = new Size(916, 760);
      Controls.Add(eraseButton);
      Controls.Add(colorCheck);
      DoubleBuffered = true;
      Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      Name = "PPIForm";
      StartPosition = FormStartPosition.Manual;
      Text = "PPI";
      FormClosing += PPIForm_FormClosing;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
    private CheckBox colorCheck;
    private Button eraseButton;
  }
}