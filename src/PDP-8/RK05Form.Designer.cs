namespace PDP_8
{
  partial class RK05Form
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
      pictureBox1 = new PictureBox();
      groupBox1 = new GroupBox();
      filename0Text = new TextBox();
      saveOnExit0Check = new CheckBox();
      saveAs0Button = new Button();
      save0Button = new Button();
      load0Button = new Button();
      groupBox2 = new GroupBox();
      filename1Text = new TextBox();
      saveOnExit1Check = new CheckBox();
      saveAs1Button = new Button();
      save1Button = new Button();
      load1Button = new Button();
      groupBox3 = new GroupBox();
      filename2Text = new TextBox();
      saveOnExit2Check = new CheckBox();
      saveAs2Button = new Button();
      save2Button = new Button();
      load2Button = new Button();
      groupBox4 = new GroupBox();
      filename3Text = new TextBox();
      saveOnExit3Check = new CheckBox();
      saveAs3Button = new Button();
      save3Button = new Button();
      load3Button = new Button();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      groupBox1.SuspendLayout();
      groupBox2.SuspendLayout();
      groupBox3.SuspendLayout();
      groupBox4.SuspendLayout();
      SuspendLayout();
      // 
      // pictureBox1
      // 
      pictureBox1.BackColor = Color.Transparent;
      pictureBox1.Image = Properties.Resources.decpack;
      pictureBox1.Location = new Point(0, 0);
      pictureBox1.Margin = new Padding(4);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(594, 186);
      pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      pictureBox1.TabIndex = 0;
      pictureBox1.TabStop = false;
      // 
      // groupBox1
      // 
      groupBox1.BackColor = Color.Silver;
      groupBox1.Controls.Add(filename0Text);
      groupBox1.Controls.Add(saveOnExit0Check);
      groupBox1.Controls.Add(saveAs0Button);
      groupBox1.Controls.Add(save0Button);
      groupBox1.Controls.Add(load0Button);
      groupBox1.Location = new Point(8, 194);
      groupBox1.Margin = new Padding(4);
      groupBox1.Name = "groupBox1";
      groupBox1.Padding = new Padding(4);
      groupBox1.Size = new Size(586, 143);
      groupBox1.TabIndex = 1;
      groupBox1.TabStop = false;
      groupBox1.Text = "Unit 0";
      // 
      // filename0Text
      // 
      filename0Text.BackColor = Color.White;
      filename0Text.Location = new Point(4, 88);
      filename0Text.Name = "filename0Text";
      filename0Text.ReadOnly = true;
      filename0Text.Size = new Size(562, 39);
      filename0Text.TabIndex = 4;
      filename0Text.Tag = "filename0";
      // 
      // saveOnExit0Check
      // 
      saveOnExit0Check.AutoSize = true;
      saveOnExit0Check.Location = new Point(397, 40);
      saveOnExit0Check.Name = "saveOnExit0Check";
      saveOnExit0Check.Size = new Size(169, 36);
      saveOnExit0Check.TabIndex = 3;
      saveOnExit0Check.Tag = "saveOnExit0";
      saveOnExit0Check.Text = "Save on Exit";
      saveOnExit0Check.UseVisualStyleBackColor = true;
      // 
      // saveAs0Button
      // 
      saveAs0Button.BackColor = SystemColors.Control;
      saveAs0Button.Location = new Point(240, 36);
      saveAs0Button.Name = "saveAs0Button";
      saveAs0Button.Size = new Size(114, 42);
      saveAs0Button.TabIndex = 2;
      saveAs0Button.Text = "SaveAs...";
      saveAs0Button.UseVisualStyleBackColor = false;
      saveAs0Button.Click += saveAs0Button_Click;
      // 
      // save0Button
      // 
      save0Button.BackColor = SystemColors.Control;
      save0Button.Enabled = false;
      save0Button.Location = new Point(122, 36);
      save0Button.Name = "save0Button";
      save0Button.Size = new Size(112, 42);
      save0Button.TabIndex = 1;
      save0Button.Text = "Save";
      save0Button.UseVisualStyleBackColor = false;
      save0Button.Click += save0Button_Click;
      // 
      // load0Button
      // 
      load0Button.BackColor = SystemColors.Control;
      load0Button.Location = new Point(4, 36);
      load0Button.Name = "load0Button";
      load0Button.Size = new Size(112, 42);
      load0Button.TabIndex = 0;
      load0Button.Text = "Load...";
      load0Button.UseVisualStyleBackColor = false;
      load0Button.Click += load0Button_Click;
      // 
      // groupBox2
      // 
      groupBox2.BackColor = Color.Silver;
      groupBox2.Controls.Add(filename1Text);
      groupBox2.Controls.Add(saveOnExit1Check);
      groupBox2.Controls.Add(saveAs1Button);
      groupBox2.Controls.Add(save1Button);
      groupBox2.Controls.Add(load1Button);
      groupBox2.Location = new Point(8, 345);
      groupBox2.Margin = new Padding(4);
      groupBox2.Name = "groupBox2";
      groupBox2.Padding = new Padding(4);
      groupBox2.Size = new Size(586, 143);
      groupBox2.TabIndex = 1;
      groupBox2.TabStop = false;
      groupBox2.Text = "Unit 1";
      // 
      // filename1Text
      // 
      filename1Text.BackColor = Color.White;
      filename1Text.Location = new Point(9, 88);
      filename1Text.Name = "filename1Text";
      filename1Text.ReadOnly = true;
      filename1Text.Size = new Size(562, 39);
      filename1Text.TabIndex = 5;
      filename1Text.Tag = "filename1";
      // 
      // saveOnExit1Check
      // 
      saveOnExit1Check.AutoSize = true;
      saveOnExit1Check.Location = new Point(397, 40);
      saveOnExit1Check.Name = "saveOnExit1Check";
      saveOnExit1Check.Size = new Size(169, 36);
      saveOnExit1Check.TabIndex = 3;
      saveOnExit1Check.Tag = "saveOnExit1";
      saveOnExit1Check.Text = "Save on Exit";
      saveOnExit1Check.UseVisualStyleBackColor = true;
      // 
      // saveAs1Button
      // 
      saveAs1Button.BackColor = SystemColors.Control;
      saveAs1Button.Location = new Point(240, 36);
      saveAs1Button.Name = "saveAs1Button";
      saveAs1Button.Size = new Size(114, 42);
      saveAs1Button.TabIndex = 2;
      saveAs1Button.Text = "SaveAs...";
      saveAs1Button.UseVisualStyleBackColor = false;
      saveAs1Button.Click += saveAs0Button_Click;
      // 
      // save1Button
      // 
      save1Button.BackColor = SystemColors.Control;
      save1Button.Enabled = false;
      save1Button.Location = new Point(122, 36);
      save1Button.Name = "save1Button";
      save1Button.Size = new Size(112, 42);
      save1Button.TabIndex = 1;
      save1Button.Text = "Save";
      save1Button.UseVisualStyleBackColor = false;
      save1Button.Click += save0Button_Click;
      // 
      // load1Button
      // 
      load1Button.BackColor = SystemColors.Control;
      load1Button.Location = new Point(4, 36);
      load1Button.Name = "load1Button";
      load1Button.Size = new Size(112, 42);
      load1Button.TabIndex = 0;
      load1Button.Text = "Load...";
      load1Button.UseVisualStyleBackColor = false;
      load1Button.Click += load0Button_Click;
      // 
      // groupBox3
      // 
      groupBox3.BackColor = Color.Silver;
      groupBox3.Controls.Add(filename2Text);
      groupBox3.Controls.Add(saveOnExit2Check);
      groupBox3.Controls.Add(saveAs2Button);
      groupBox3.Controls.Add(save2Button);
      groupBox3.Controls.Add(load2Button);
      groupBox3.Location = new Point(8, 496);
      groupBox3.Margin = new Padding(4);
      groupBox3.Name = "groupBox3";
      groupBox3.Padding = new Padding(4);
      groupBox3.Size = new Size(586, 143);
      groupBox3.TabIndex = 1;
      groupBox3.TabStop = false;
      groupBox3.Text = "Unit 2";
      // 
      // filename2Text
      // 
      filename2Text.BackColor = Color.White;
      filename2Text.Location = new Point(7, 88);
      filename2Text.Name = "filename2Text";
      filename2Text.ReadOnly = true;
      filename2Text.Size = new Size(562, 39);
      filename2Text.TabIndex = 6;
      filename2Text.Tag = "filename2";
      // 
      // saveOnExit2Check
      // 
      saveOnExit2Check.AutoSize = true;
      saveOnExit2Check.Location = new Point(397, 40);
      saveOnExit2Check.Name = "saveOnExit2Check";
      saveOnExit2Check.Size = new Size(169, 36);
      saveOnExit2Check.TabIndex = 3;
      saveOnExit2Check.Tag = "saveOnExit2";
      saveOnExit2Check.Text = "Save on Exit";
      saveOnExit2Check.UseVisualStyleBackColor = true;
      // 
      // saveAs2Button
      // 
      saveAs2Button.BackColor = SystemColors.Control;
      saveAs2Button.Location = new Point(240, 36);
      saveAs2Button.Name = "saveAs2Button";
      saveAs2Button.Size = new Size(114, 42);
      saveAs2Button.TabIndex = 2;
      saveAs2Button.Text = "SaveAs...";
      saveAs2Button.UseVisualStyleBackColor = false;
      saveAs2Button.Click += saveAs0Button_Click;
      // 
      // save2Button
      // 
      save2Button.BackColor = SystemColors.Control;
      save2Button.Enabled = false;
      save2Button.Location = new Point(122, 36);
      save2Button.Name = "save2Button";
      save2Button.Size = new Size(112, 42);
      save2Button.TabIndex = 1;
      save2Button.Text = "Save";
      save2Button.UseVisualStyleBackColor = false;
      save2Button.Click += save0Button_Click;
      // 
      // load2Button
      // 
      load2Button.BackColor = SystemColors.Control;
      load2Button.Location = new Point(4, 36);
      load2Button.Name = "load2Button";
      load2Button.Size = new Size(112, 42);
      load2Button.TabIndex = 0;
      load2Button.Text = "Load...";
      load2Button.UseVisualStyleBackColor = false;
      load2Button.Click += load0Button_Click;
      // 
      // groupBox4
      // 
      groupBox4.BackColor = Color.Silver;
      groupBox4.Controls.Add(filename3Text);
      groupBox4.Controls.Add(saveOnExit3Check);
      groupBox4.Controls.Add(saveAs3Button);
      groupBox4.Controls.Add(save3Button);
      groupBox4.Controls.Add(load3Button);
      groupBox4.Location = new Point(8, 647);
      groupBox4.Margin = new Padding(4);
      groupBox4.Name = "groupBox4";
      groupBox4.Padding = new Padding(4);
      groupBox4.Size = new Size(586, 143);
      groupBox4.TabIndex = 1;
      groupBox4.TabStop = false;
      groupBox4.Text = "Unit 3";
      // 
      // filename3Text
      // 
      filename3Text.BackColor = Color.White;
      filename3Text.Location = new Point(9, 88);
      filename3Text.Name = "filename3Text";
      filename3Text.ReadOnly = true;
      filename3Text.Size = new Size(562, 39);
      filename3Text.TabIndex = 6;
      filename3Text.Tag = "filename3";
      // 
      // saveOnExit3Check
      // 
      saveOnExit3Check.AutoSize = true;
      saveOnExit3Check.Location = new Point(397, 40);
      saveOnExit3Check.Name = "saveOnExit3Check";
      saveOnExit3Check.Size = new Size(169, 36);
      saveOnExit3Check.TabIndex = 3;
      saveOnExit3Check.Tag = "saveOnExit3";
      saveOnExit3Check.Text = "Save on Exit";
      saveOnExit3Check.UseVisualStyleBackColor = true;
      // 
      // saveAs3Button
      // 
      saveAs3Button.BackColor = SystemColors.Control;
      saveAs3Button.Location = new Point(240, 36);
      saveAs3Button.Name = "saveAs3Button";
      saveAs3Button.Size = new Size(114, 42);
      saveAs3Button.TabIndex = 2;
      saveAs3Button.Text = "SaveAs...";
      saveAs3Button.UseVisualStyleBackColor = false;
      saveAs3Button.Click += saveAs0Button_Click;
      // 
      // save3Button
      // 
      save3Button.BackColor = SystemColors.Control;
      save3Button.Enabled = false;
      save3Button.Location = new Point(122, 36);
      save3Button.Name = "save3Button";
      save3Button.Size = new Size(112, 42);
      save3Button.TabIndex = 1;
      save3Button.Text = "Save";
      save3Button.UseVisualStyleBackColor = false;
      save3Button.Click += save0Button_Click;
      // 
      // load3Button
      // 
      load3Button.BackColor = SystemColors.Control;
      load3Button.Location = new Point(4, 36);
      load3Button.Name = "load3Button";
      load3Button.Size = new Size(112, 42);
      load3Button.TabIndex = 0;
      load3Button.Text = "Load...";
      load3Button.UseVisualStyleBackColor = false;
      load3Button.Click += load0Button_Click;
      // 
      // RK05Form
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackColor = Color.Gray;
      ClientSize = new Size(601, 802);
      Controls.Add(groupBox4);
      Controls.Add(groupBox3);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(pictureBox1);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      Margin = new Padding(4);
      MaximizeBox = false;
      Name = "RK05Form";
      StartPosition = FormStartPosition.Manual;
      Text = "RK05";
      FormClosing += RK05Form_FormClosing;
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      groupBox4.ResumeLayout(false);
      groupBox4.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private PictureBox pictureBox1;
    private GroupBox groupBox1;
    private Button saveAs0Button;
    private Button save0Button;
    private Button load0Button;
    private CheckBox saveOnExit0Check;
    private GroupBox groupBox2;
    private CheckBox saveOnExit1Check;
    private Button saveAs1Button;
    private Button save1Button;
    private Button load1Button;
    private GroupBox groupBox3;
    private CheckBox saveOnExit2Check;
    private Button saveAs2Button;
    private Button save2Button;
    private Button load2Button;
    private GroupBox groupBox4;
    private CheckBox saveOnExit3Check;
    private Button saveAs3Button;
    private Button save3Button;
    private Button load3Button;
    private TextBox filename0Text;
    private TextBox filename1Text;
    private TextBox filename2Text;
    private TextBox filename3Text;
  }
}