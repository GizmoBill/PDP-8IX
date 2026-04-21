namespace PDP_8
{
  partial class FrontPanel
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
      lightShape0 = new ShapeControls.ShapeControl();
      lightShape1 = new ShapeControls.ShapeControl();
      lightShape2 = new ShapeControls.ShapeControl();
      lightShape3 = new ShapeControls.ShapeControl();
      white0Picture = new PictureBox();
      white1Picture = new PictureBox();
      tan0Picture = new PictureBox();
      tan1Picture = new PictureBox();
      ((System.ComponentModel.ISupportInitialize)white0Picture).BeginInit();
      ((System.ComponentModel.ISupportInitialize)white1Picture).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tan0Picture).BeginInit();
      ((System.ComponentModel.ISupportInitialize)tan1Picture).BeginInit();
      SuspendLayout();
      // 
      // lightShape0
      // 
      lightShape0.BackColor = Color.Transparent;
      lightShape0.CornerRadius = 6;
      lightShape0.FillColor = Color.White;
      lightShape0.Location = new Point(303, 176);
      lightShape0.Name = "lightShape0";
      lightShape0.Shape = ShapeControls.ShapeControl.ShapeKind.RoundedRectangle;
      lightShape0.Size = new Size(26, 26);
      lightShape0.TabIndex = 0;
      lightShape0.Text = "shapeControl1";
      // 
      // lightShape1
      // 
      lightShape1.BackColor = Color.Transparent;
      lightShape1.CornerRadius = 6;
      lightShape1.FillColor = Color.White;
      lightShape1.Location = new Point(1165, 487);
      lightShape1.Name = "lightShape1";
      lightShape1.Shape = ShapeControls.ShapeControl.ShapeKind.RoundedRectangle;
      lightShape1.Size = new Size(26, 26);
      lightShape1.TabIndex = 0;
      lightShape1.Text = "shapeControl1";
      // 
      // lightShape2
      // 
      lightShape2.BackColor = Color.Transparent;
      lightShape2.CornerRadius = 6;
      lightShape2.FillColor = Color.White;
      lightShape2.Location = new Point(1356, 176);
      lightShape2.Name = "lightShape2";
      lightShape2.Shape = ShapeControls.ShapeControl.ShapeKind.RoundedRectangle;
      lightShape2.Size = new Size(26, 26);
      lightShape2.TabIndex = 0;
      lightShape2.Text = "shapeControl1";
      // 
      // lightShape3
      // 
      lightShape3.BackColor = Color.Transparent;
      lightShape3.CornerRadius = 6;
      lightShape3.FillColor = Color.White;
      lightShape3.Location = new Point(1560, 254);
      lightShape3.Name = "lightShape3";
      lightShape3.Shape = ShapeControls.ShapeControl.ShapeKind.RoundedRectangle;
      lightShape3.Size = new Size(26, 26);
      lightShape3.TabIndex = 0;
      lightShape3.Text = "shapeControl1";
      // 
      // white0Picture
      // 
      white0Picture.Image = Properties.Resources.white0;
      white0Picture.Location = new Point(645, 419);
      white0Picture.Name = "white0Picture";
      white0Picture.Size = new Size(46, 118);
      white0Picture.SizeMode = PictureBoxSizeMode.Zoom;
      white0Picture.TabIndex = 1;
      white0Picture.TabStop = false;
      white0Picture.Visible = false;
      // 
      // white1Picture
      // 
      white1Picture.Image = Properties.Resources.white1;
      white1Picture.Location = new Point(753, 419);
      white1Picture.Name = "white1Picture";
      white1Picture.Size = new Size(46, 118);
      white1Picture.SizeMode = PictureBoxSizeMode.Zoom;
      white1Picture.TabIndex = 2;
      white1Picture.TabStop = false;
      white1Picture.Visible = false;
      // 
      // tan0Picture
      // 
      tan0Picture.Image = Properties.Resources.tan0;
      tan0Picture.Location = new Point(303, 614);
      tan0Picture.Name = "tan0Picture";
      tan0Picture.Size = new Size(46, 118);
      tan0Picture.SizeMode = PictureBoxSizeMode.Zoom;
      tan0Picture.TabIndex = 3;
      tan0Picture.TabStop = false;
      // 
      // tan1Picture
      // 
      tan1Picture.Image = Properties.Resources.tan1;
      tan1Picture.Location = new Point(1528, 614);
      tan1Picture.Name = "tan1Picture";
      tan1Picture.Size = new Size(46, 118);
      tan1Picture.SizeMode = PictureBoxSizeMode.Zoom;
      tan1Picture.TabIndex = 4;
      tan1Picture.TabStop = false;
      // 
      // FrontPanel
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackgroundImage = Properties.Resources.PDP_8_I_5;
      BackgroundImageLayout = ImageLayout.Zoom;
      ClientSize = new Size(1673, 832);
      Controls.Add(tan1Picture);
      Controls.Add(tan0Picture);
      Controls.Add(white1Picture);
      Controls.Add(white0Picture);
      Controls.Add(lightShape3);
      Controls.Add(lightShape2);
      Controls.Add(lightShape1);
      Controls.Add(lightShape0);
      DoubleBuffered = true;
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      Margin = new Padding(4);
      MaximizeBox = false;
      Name = "FrontPanel";
      StartPosition = FormStartPosition.Manual;
      Text = "FrontPanel";
      FormClosing += FrontPanel_FormClosing;
      Resize += FrontPanel_Resize;
      ((System.ComponentModel.ISupportInitialize)white0Picture).EndInit();
      ((System.ComponentModel.ISupportInitialize)white1Picture).EndInit();
      ((System.ComponentModel.ISupportInitialize)tan0Picture).EndInit();
      ((System.ComponentModel.ISupportInitialize)tan1Picture).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private ShapeControls.ShapeControl lightShape0;
    private ShapeControls.ShapeControl lightShape1;
    private ShapeControls.ShapeControl lightShape2;
    private ShapeControls.ShapeControl lightShape3;
    private PictureBox white0Picture;
    private PictureBox white1Picture;
    private PictureBox tan0Picture;
    private PictureBox tan1Picture;
  }
}