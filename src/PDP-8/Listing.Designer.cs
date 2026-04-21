namespace PDP_8
{
  partial class Listing
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
      listingText = new TextBox();
      SuspendLayout();
      // 
      // listingText
      // 
      listingText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      listingText.HideSelection = false;
      listingText.Location = new Point(0, 0);
      listingText.Multiline = true;
      listingText.Name = "listingText";
      listingText.ReadOnly = true;
      listingText.ScrollBars = ScrollBars.Vertical;
      listingText.Size = new Size(1038, 572);
      listingText.TabIndex = 0;
      // 
      // Listing
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      ClientSize = new Size(1040, 576);
      Controls.Add(listingText);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      Margin = new Padding(4);
      Name = "Listing";
      StartPosition = FormStartPosition.Manual;
      Text = "Listing";
      FormClosing += Listing_FormClosing;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private TextBox listingText;
  }
}