namespace PDP_8
{
  partial class Tek611Form
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
      nonStoreCheck = new CheckBox();
      SuspendLayout();
      // 
      // nonStoreCheck
      // 
      nonStoreCheck.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      nonStoreCheck.AutoSize = true;
      nonStoreCheck.BackColor = Color.Black;
      nonStoreCheck.ForeColor = Color.White;
      nonStoreCheck.Location = new Point(706, 736);
      nonStoreCheck.Name = "nonStoreCheck";
      nonStoreCheck.Size = new Size(165, 29);
      nonStoreCheck.TabIndex = 0;
      nonStoreCheck.Text = "NonStore Mode";
      nonStoreCheck.UseVisualStyleBackColor = false;
      nonStoreCheck.CheckedChanged += nonStoreCheck_CheckedChanged;
      // 
      // Tek611Form
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackgroundImageLayout = ImageLayout.Zoom;
      ClientSize = new Size(871, 766);
      Controls.Add(nonStoreCheck);
      DoubleBuffered = true;
      Name = "Tek611Form";
      StartPosition = FormStartPosition.Manual;
      Text = "Tektronix 611 Storage Scope";
      FormClosing += Tek611Form_FormClosing;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private CheckBox nonStoreCheck;
  }
}