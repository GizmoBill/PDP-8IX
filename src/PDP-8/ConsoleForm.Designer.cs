namespace PDP_8
{
    partial class ConsoleForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
      menuStrip1 = new MenuStrip();
      fileToolStripMenuItem = new ToolStripMenuItem();
      newToolStripMenuItem = new ToolStripMenuItem();
      openToolStripMenuItem = new ToolStripMenuItem();
      saveToolStripMenuItem = new ToolStripMenuItem();
      saveAsToolStripMenuItem = new ToolStripMenuItem();
      toolStripSeparator3 = new ToolStripSeparator();
      oS8ToolStripMenuItem = new ToolStripMenuItem();
      toolStripSeparator1 = new ToolStripSeparator();
      loadListingToolStripMenuItem = new ToolStripMenuItem();
      readListingToolStripMenuItem = new ToolStripMenuItem();
      splitListingToolStripMenuItem = new ToolStripMenuItem();
      writeBinaryToolStripMenuItem = new ToolStripMenuItem();
      writeSourceToolStripMenuItem = new ToolStripMenuItem();
      toolStripSeparator2 = new ToolStripSeparator();
      coreDumpToolStripMenuItem = new ToolStripMenuItem();
      devicesToolStripMenuItem = new ToolStripMenuItem();
      tty1ConsoleToolStripMenuItem = new ToolStripMenuItem();
      tTY2ToolStripMenuItem = new ToolStripMenuItem();
      rK05ToolStripMenuItem = new ToolStripMenuItem();
      hRPToolStripMenuItem = new ToolStripMenuItem();
      tektronix611ToolStripMenuItem = new ToolStripMenuItem();
      optionsToolStripMenuItem = new ToolStripMenuItem();
      x10ToolStripMenuItem = new ToolStripMenuItem();
      showFrontPanelToolStripMenuItem = new ToolStripMenuItem();
      analyzeToolStripMenuItem = new ToolStripMenuItem();
      showListingToolStripMenuItem1 = new ToolStripMenuItem();
      toolStripSeparator4 = new ToolStripSeparator();
      recordToolStripMenuItem = new ToolStripMenuItem();
      recordOnlyBreaksToolStripMenuItem = new ToolStripMenuItem();
      firstEventToolStripMenuItem = new ToolStripMenuItem();
      nextEventToolStripMenuItem = new ToolStripMenuItem();
      previousEventToolStripMenuItem = new ToolStripMenuItem();
      clearEventsToolStripMenuItem = new ToolStripMenuItem();
      panel1 = new Panel();
      label9 = new Label();
      label10 = new Label();
      label8 = new Label();
      ionShape = new ShapeControls.ShapeControl();
      label7 = new Label();
      eventCountLabel = new Label();
      realTimeLabel = new Label();
      cycleCountLabel = new Label();
      disasmLabel = new Label();
      busyTimeLabel = new Label();
      cpuCycleTimeLabel = new Label();
      burstCycleTimeLabel = new Label();
      irqLabel = new Label();
      instrLabel = new Label();
      cycleLabel = new Label();
      switchText = new TextBox();
      instrButton = new Button();
      stepButton = new Button();
      stopButton = new Button();
      continueButton = new Button();
      depositButton = new Button();
      examineButton = new Button();
      loadAddrButton = new Button();
      startButton = new Button();
      xModeLabel = new Label();
      xRegLabel = new Label();
      ionLabel = new Label();
      maskLabel = new Label();
      mqLabel = new Label();
      acLabel = new Label();
      mbrLabel = new Label();
      marLabel = new Label();
      pcLabel = new Label();
      label6 = new Label();
      label5 = new Label();
      label4 = new Label();
      label3 = new Label();
      label2 = new Label();
      label1 = new Label();
      runTimer = new System.Windows.Forms.Timer(components);
      menuStrip1.SuspendLayout();
      panel1.SuspendLayout();
      SuspendLayout();
      // 
      // menuStrip1
      // 
      menuStrip1.ImageScalingSize = new Size(24, 24);
      menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, devicesToolStripMenuItem, optionsToolStripMenuItem, analyzeToolStripMenuItem });
      menuStrip1.Location = new Point(0, 0);
      menuStrip1.Name = "menuStrip1";
      menuStrip1.Padding = new Padding(7, 2, 0, 2);
      menuStrip1.Size = new Size(702, 33);
      menuStrip1.TabIndex = 0;
      menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator3, oS8ToolStripMenuItem, toolStripSeparator1, loadListingToolStripMenuItem, readListingToolStripMenuItem, splitListingToolStripMenuItem, writeBinaryToolStripMenuItem, writeSourceToolStripMenuItem, toolStripSeparator2, coreDumpToolStripMenuItem });
      fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      fileToolStripMenuItem.Size = new Size(54, 29);
      fileToolStripMenuItem.Text = "File";
      // 
      // newToolStripMenuItem
      // 
      newToolStripMenuItem.Name = "newToolStripMenuItem";
      newToolStripMenuItem.Size = new Size(270, 34);
      newToolStripMenuItem.Text = "New";
      newToolStripMenuItem.Click += newToolStripMenuItem_Click;
      // 
      // openToolStripMenuItem
      // 
      openToolStripMenuItem.Name = "openToolStripMenuItem";
      openToolStripMenuItem.Size = new Size(270, 34);
      openToolStripMenuItem.Text = "Open...";
      openToolStripMenuItem.Click += openToolStripMenuItem_Click;
      // 
      // saveToolStripMenuItem
      // 
      saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      saveToolStripMenuItem.Size = new Size(270, 34);
      saveToolStripMenuItem.Text = "Save";
      saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
      // 
      // saveAsToolStripMenuItem
      // 
      saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      saveAsToolStripMenuItem.Size = new Size(270, 34);
      saveAsToolStripMenuItem.Text = "SaveAs...";
      saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
      // 
      // toolStripSeparator3
      // 
      toolStripSeparator3.Name = "toolStripSeparator3";
      toolStripSeparator3.Size = new Size(267, 6);
      // 
      // oS8ToolStripMenuItem
      // 
      oS8ToolStripMenuItem.Name = "oS8ToolStripMenuItem";
      oS8ToolStripMenuItem.Size = new Size(270, 34);
      oS8ToolStripMenuItem.Text = "OS-8";
      oS8ToolStripMenuItem.Click += oS8ToolStripMenuItem_Click;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new Size(267, 6);
      // 
      // loadListingToolStripMenuItem
      // 
      loadListingToolStripMenuItem.Name = "loadListingToolStripMenuItem";
      loadListingToolStripMenuItem.Size = new Size(270, 34);
      loadListingToolStripMenuItem.Text = "Load Listing...";
      loadListingToolStripMenuItem.Click += loadListingToolStripMenuItem_Click;
      // 
      // readListingToolStripMenuItem
      // 
      readListingToolStripMenuItem.Name = "readListingToolStripMenuItem";
      readListingToolStripMenuItem.Size = new Size(270, 34);
      readListingToolStripMenuItem.Text = "Read Listing...";
      readListingToolStripMenuItem.Click += readListingToolStripMenuItem_Click;
      // 
      // splitListingToolStripMenuItem
      // 
      splitListingToolStripMenuItem.Name = "splitListingToolStripMenuItem";
      splitListingToolStripMenuItem.Size = new Size(270, 34);
      splitListingToolStripMenuItem.Text = "Split Listing...";
      splitListingToolStripMenuItem.Click += splitListingToolStripMenuItem_Click;
      // 
      // writeBinaryToolStripMenuItem
      // 
      writeBinaryToolStripMenuItem.Name = "writeBinaryToolStripMenuItem";
      writeBinaryToolStripMenuItem.Size = new Size(270, 34);
      writeBinaryToolStripMenuItem.Text = "Write Binary";
      writeBinaryToolStripMenuItem.Click += writeBinaryToolStripMenuItem_Click;
      // 
      // writeSourceToolStripMenuItem
      // 
      writeSourceToolStripMenuItem.Name = "writeSourceToolStripMenuItem";
      writeSourceToolStripMenuItem.Size = new Size(270, 34);
      writeSourceToolStripMenuItem.Text = "Write Source";
      writeSourceToolStripMenuItem.Click += writeSourceToolStripMenuItem_Click;
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new Size(267, 6);
      // 
      // coreDumpToolStripMenuItem
      // 
      coreDumpToolStripMenuItem.Name = "coreDumpToolStripMenuItem";
      coreDumpToolStripMenuItem.Size = new Size(270, 34);
      coreDumpToolStripMenuItem.Text = "Core Dump";
      coreDumpToolStripMenuItem.Click += coreDumpToolStripMenuItem_Click;
      // 
      // devicesToolStripMenuItem
      // 
      devicesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tty1ConsoleToolStripMenuItem, tTY2ToolStripMenuItem, rK05ToolStripMenuItem, hRPToolStripMenuItem, tektronix611ToolStripMenuItem });
      devicesToolStripMenuItem.Name = "devicesToolStripMenuItem";
      devicesToolStripMenuItem.Size = new Size(88, 29);
      devicesToolStripMenuItem.Text = "Devices";
      // 
      // tty1ConsoleToolStripMenuItem
      // 
      tty1ConsoleToolStripMenuItem.Name = "tty1ConsoleToolStripMenuItem";
      tty1ConsoleToolStripMenuItem.Size = new Size(219, 34);
      tty1ConsoleToolStripMenuItem.Text = "TTY1";
      tty1ConsoleToolStripMenuItem.Click += aSR38ConsoleToolStripMenuItem_Click;
      // 
      // tTY2ToolStripMenuItem
      // 
      tTY2ToolStripMenuItem.Name = "tTY2ToolStripMenuItem";
      tTY2ToolStripMenuItem.Size = new Size(219, 34);
      tTY2ToolStripMenuItem.Text = "TTY2";
      tTY2ToolStripMenuItem.Click += tTY2ToolStripMenuItem_Click;
      // 
      // rK05ToolStripMenuItem
      // 
      rK05ToolStripMenuItem.Name = "rK05ToolStripMenuItem";
      rK05ToolStripMenuItem.Size = new Size(219, 34);
      rK05ToolStripMenuItem.Text = "RK05";
      rK05ToolStripMenuItem.Click += rK05ToolStripMenuItem_Click;
      // 
      // hRPToolStripMenuItem
      // 
      hRPToolStripMenuItem.Name = "hRPToolStripMenuItem";
      hRPToolStripMenuItem.Size = new Size(219, 34);
      hRPToolStripMenuItem.Text = "HRP";
      hRPToolStripMenuItem.Click += hRPToolStripMenuItem_Click;
      // 
      // tektronix611ToolStripMenuItem
      // 
      tektronix611ToolStripMenuItem.Name = "tektronix611ToolStripMenuItem";
      tektronix611ToolStripMenuItem.Size = new Size(219, 34);
      tektronix611ToolStripMenuItem.Text = "Tektronix 611";
      tektronix611ToolStripMenuItem.Click += tektronix611ToolStripMenuItem_Click;
      // 
      // optionsToolStripMenuItem
      // 
      optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { x10ToolStripMenuItem, showFrontPanelToolStripMenuItem });
      optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      optionsToolStripMenuItem.Size = new Size(92, 29);
      optionsToolStripMenuItem.Text = "Options";
      // 
      // x10ToolStripMenuItem
      // 
      x10ToolStripMenuItem.CheckOnClick = true;
      x10ToolStripMenuItem.Name = "x10ToolStripMenuItem";
      x10ToolStripMenuItem.Size = new Size(251, 34);
      x10ToolStripMenuItem.Text = "10x";
      x10ToolStripMenuItem.Click += x10ToolStripMenuItem_Click;
      // 
      // showFrontPanelToolStripMenuItem
      // 
      showFrontPanelToolStripMenuItem.Name = "showFrontPanelToolStripMenuItem";
      showFrontPanelToolStripMenuItem.Size = new Size(251, 34);
      showFrontPanelToolStripMenuItem.Text = "Show Front Panel";
      showFrontPanelToolStripMenuItem.Click += showFrontPanelToolStripMenuItem_Click;
      // 
      // analyzeToolStripMenuItem
      // 
      analyzeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showListingToolStripMenuItem1, toolStripSeparator4, recordToolStripMenuItem, recordOnlyBreaksToolStripMenuItem, firstEventToolStripMenuItem, nextEventToolStripMenuItem, previousEventToolStripMenuItem, clearEventsToolStripMenuItem });
      analyzeToolStripMenuItem.Name = "analyzeToolStripMenuItem";
      analyzeToolStripMenuItem.Size = new Size(89, 29);
      analyzeToolStripMenuItem.Text = "Analyze";
      // 
      // showListingToolStripMenuItem1
      // 
      showListingToolStripMenuItem1.Name = "showListingToolStripMenuItem1";
      showListingToolStripMenuItem1.Size = new Size(290, 34);
      showListingToolStripMenuItem1.Text = "Show Listing";
      showListingToolStripMenuItem1.Click += showListingToolStripMenuItem1_Click;
      // 
      // toolStripSeparator4
      // 
      toolStripSeparator4.Name = "toolStripSeparator4";
      toolStripSeparator4.Size = new Size(287, 6);
      // 
      // recordToolStripMenuItem
      // 
      recordToolStripMenuItem.CheckOnClick = true;
      recordToolStripMenuItem.Name = "recordToolStripMenuItem";
      recordToolStripMenuItem.Size = new Size(290, 34);
      recordToolStripMenuItem.Text = "Record";
      recordToolStripMenuItem.Click += recordToolStripMenuItem_Click;
      // 
      // recordOnlyBreaksToolStripMenuItem
      // 
      recordOnlyBreaksToolStripMenuItem.CheckOnClick = true;
      recordOnlyBreaksToolStripMenuItem.Name = "recordOnlyBreaksToolStripMenuItem";
      recordOnlyBreaksToolStripMenuItem.Size = new Size(290, 34);
      recordOnlyBreaksToolStripMenuItem.Text = "Record Only Breaks";
      recordOnlyBreaksToolStripMenuItem.Click += recordOnlyBreaksToolStripMenuItem_Click;
      // 
      // firstEventToolStripMenuItem
      // 
      firstEventToolStripMenuItem.Name = "firstEventToolStripMenuItem";
      firstEventToolStripMenuItem.Size = new Size(290, 34);
      firstEventToolStripMenuItem.Text = "Last Event";
      firstEventToolStripMenuItem.Click += firstEventToolStripMenuItem_Click;
      // 
      // nextEventToolStripMenuItem
      // 
      nextEventToolStripMenuItem.Name = "nextEventToolStripMenuItem";
      nextEventToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
      nextEventToolStripMenuItem.Size = new Size(290, 34);
      nextEventToolStripMenuItem.Text = "Next Event";
      nextEventToolStripMenuItem.Click += nextEventToolStripMenuItem_Click;
      // 
      // previousEventToolStripMenuItem
      // 
      previousEventToolStripMenuItem.Name = "previousEventToolStripMenuItem";
      previousEventToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
      previousEventToolStripMenuItem.Size = new Size(290, 34);
      previousEventToolStripMenuItem.Text = "Previous Event";
      previousEventToolStripMenuItem.Click += previousEventToolStripMenuItem_Click;
      // 
      // clearEventsToolStripMenuItem
      // 
      clearEventsToolStripMenuItem.Name = "clearEventsToolStripMenuItem";
      clearEventsToolStripMenuItem.Size = new Size(290, 34);
      clearEventsToolStripMenuItem.Text = "Clear Events";
      clearEventsToolStripMenuItem.Click += clearEventsToolStripMenuItem_Click;
      // 
      // panel1
      // 
      panel1.BackColor = Color.Silver;
      panel1.Controls.Add(label9);
      panel1.Controls.Add(label10);
      panel1.Controls.Add(label8);
      panel1.Controls.Add(ionShape);
      panel1.Controls.Add(label7);
      panel1.Controls.Add(eventCountLabel);
      panel1.Controls.Add(realTimeLabel);
      panel1.Controls.Add(cycleCountLabel);
      panel1.Controls.Add(disasmLabel);
      panel1.Controls.Add(busyTimeLabel);
      panel1.Controls.Add(cpuCycleTimeLabel);
      panel1.Controls.Add(burstCycleTimeLabel);
      panel1.Controls.Add(irqLabel);
      panel1.Controls.Add(instrLabel);
      panel1.Controls.Add(cycleLabel);
      panel1.Controls.Add(switchText);
      panel1.Controls.Add(instrButton);
      panel1.Controls.Add(stepButton);
      panel1.Controls.Add(stopButton);
      panel1.Controls.Add(continueButton);
      panel1.Controls.Add(depositButton);
      panel1.Controls.Add(examineButton);
      panel1.Controls.Add(loadAddrButton);
      panel1.Controls.Add(startButton);
      panel1.Controls.Add(xModeLabel);
      panel1.Controls.Add(xRegLabel);
      panel1.Controls.Add(ionLabel);
      panel1.Controls.Add(maskLabel);
      panel1.Controls.Add(mqLabel);
      panel1.Controls.Add(acLabel);
      panel1.Controls.Add(mbrLabel);
      panel1.Controls.Add(marLabel);
      panel1.Controls.Add(pcLabel);
      panel1.Controls.Add(label6);
      panel1.Controls.Add(label5);
      panel1.Controls.Add(label4);
      panel1.Controls.Add(label3);
      panel1.Controls.Add(label2);
      panel1.Controls.Add(label1);
      panel1.Location = new Point(0, 36);
      panel1.Margin = new Padding(4, 3, 4, 3);
      panel1.Name = "panel1";
      panel1.Size = new Size(705, 481);
      panel1.TabIndex = 1;
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label9.Location = new Point(379, 195);
      label9.Name = "label9";
      label9.Size = new Size(43, 32);
      label9.TabIndex = 13;
      label9.Text = "SR";
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label10.Location = new Point(379, 246);
      label10.Name = "label10";
      label10.Size = new Size(74, 32);
      label10.TabIndex = 13;
      label10.Text = "Mask";
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label8.Location = new Point(448, 295);
      label8.Name = "label8";
      label8.Size = new Size(56, 32);
      label8.TabIndex = 13;
      label8.Text = "IRQ";
      // 
      // ionShape
      // 
      ionShape.BackColor = Color.Transparent;
      ionShape.CornerRadius = 12;
      ionShape.FillColor = Color.White;
      ionShape.Location = new Point(401, 298);
      ionShape.Name = "ionShape";
      ionShape.Shape = ShapeControls.ShapeControl.ShapeKind.Ellipse;
      ionShape.Size = new Size(32, 32);
      ionShape.TabIndex = 12;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label7.Location = new Point(279, 295);
      label7.Name = "label7";
      label7.Size = new Size(59, 32);
      label7.TabIndex = 11;
      label7.Text = "ION";
      // 
      // eventCountLabel
      // 
      eventCountLabel.BackColor = Color.White;
      eventCountLabel.BorderStyle = BorderStyle.Fixed3D;
      eventCountLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      eventCountLabel.Location = new Point(472, 405);
      eventCountLabel.Name = "eventCountLabel";
      eventCountLabel.Size = new Size(106, 48);
      eventCountLabel.TabIndex = 10;
      eventCountLabel.TextAlign = ContentAlignment.MiddleRight;
      // 
      // realTimeLabel
      // 
      realTimeLabel.BackColor = Color.White;
      realTimeLabel.BorderStyle = BorderStyle.Fixed3D;
      realTimeLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      realTimeLabel.Location = new Point(290, 405);
      realTimeLabel.Name = "realTimeLabel";
      realTimeLabel.Size = new Size(140, 48);
      realTimeLabel.TabIndex = 10;
      realTimeLabel.TextAlign = ContentAlignment.MiddleRight;
      // 
      // cycleCountLabel
      // 
      cycleCountLabel.BackColor = Color.White;
      cycleCountLabel.BorderStyle = BorderStyle.Fixed3D;
      cycleCountLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cycleCountLabel.Location = new Point(27, 405);
      cycleCountLabel.Name = "cycleCountLabel";
      cycleCountLabel.Size = new Size(242, 48);
      cycleCountLabel.TabIndex = 10;
      cycleCountLabel.TextAlign = ContentAlignment.MiddleRight;
      // 
      // disasmLabel
      // 
      disasmLabel.BackColor = Color.White;
      disasmLabel.BorderStyle = BorderStyle.Fixed3D;
      disasmLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      disasmLabel.Location = new Point(27, 341);
      disasmLabel.Name = "disasmLabel";
      disasmLabel.Size = new Size(656, 48);
      disasmLabel.TabIndex = 10;
      disasmLabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // busyTimeLabel
      // 
      busyTimeLabel.BackColor = Color.White;
      busyTimeLabel.BorderStyle = BorderStyle.Fixed3D;
      busyTimeLabel.Location = new Point(592, 271);
      busyTimeLabel.Name = "busyTimeLabel";
      busyTimeLabel.Size = new Size(91, 39);
      busyTimeLabel.TabIndex = 9;
      busyTimeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // cpuCycleTimeLabel
      // 
      cpuCycleTimeLabel.BackColor = Color.White;
      cpuCycleTimeLabel.BorderStyle = BorderStyle.Fixed3D;
      cpuCycleTimeLabel.Location = new Point(592, 165);
      cpuCycleTimeLabel.Name = "cpuCycleTimeLabel";
      cpuCycleTimeLabel.Size = new Size(91, 39);
      cpuCycleTimeLabel.TabIndex = 9;
      cpuCycleTimeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // burstCycleTimeLabel
      // 
      burstCycleTimeLabel.BackColor = Color.White;
      burstCycleTimeLabel.BorderStyle = BorderStyle.Fixed3D;
      burstCycleTimeLabel.Location = new Point(592, 218);
      burstCycleTimeLabel.Name = "burstCycleTimeLabel";
      burstCycleTimeLabel.Size = new Size(91, 39);
      burstCycleTimeLabel.TabIndex = 9;
      burstCycleTimeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // irqLabel
      // 
      irqLabel.BackColor = Color.White;
      irqLabel.BorderStyle = BorderStyle.Fixed3D;
      irqLabel.Location = new Point(510, 292);
      irqLabel.Name = "irqLabel";
      irqLabel.Size = new Size(36, 38);
      irqLabel.TabIndex = 8;
      irqLabel.Text = "0";
      irqLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // instrLabel
      // 
      instrLabel.BackColor = Color.White;
      instrLabel.BorderStyle = BorderStyle.Fixed3D;
      instrLabel.Location = new Point(573, 88);
      instrLabel.Name = "instrLabel";
      instrLabel.Size = new Size(110, 38);
      instrLabel.TabIndex = 8;
      instrLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // cycleLabel
      // 
      cycleLabel.BackColor = Color.White;
      cycleLabel.BorderStyle = BorderStyle.Fixed3D;
      cycleLabel.Location = new Point(573, 36);
      cycleLabel.Name = "cycleLabel";
      cycleLabel.Size = new Size(110, 38);
      cycleLabel.TabIndex = 8;
      cycleLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // switchText
      // 
      switchText.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      switchText.Location = new Point(422, 189);
      switchText.Name = "switchText";
      switchText.Size = new Size(124, 45);
      switchText.TabIndex = 7;
      switchText.Text = "000000";
      switchText.TextAlign = HorizontalAlignment.Center;
      switchText.TextChanged += switchText_TextChanged;
      switchText.KeyDown += switchText_KeyDown;
      // 
      // instrButton
      // 
      instrButton.BackColor = SystemColors.Control;
      instrButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      instrButton.Location = new Point(238, 239);
      instrButton.Name = "instrButton";
      instrButton.Size = new Size(129, 46);
      instrButton.TabIndex = 6;
      instrButton.Text = "Instr";
      instrButton.UseVisualStyleBackColor = false;
      instrButton.Click += instrButton_Click;
      // 
      // stepButton
      // 
      stepButton.BackColor = SystemColors.Control;
      stepButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      stepButton.Location = new Point(238, 186);
      stepButton.Name = "stepButton";
      stepButton.Size = new Size(129, 46);
      stepButton.TabIndex = 6;
      stepButton.Text = "Step";
      stepButton.UseVisualStyleBackColor = false;
      stepButton.Click += stepButton_Click;
      // 
      // stopButton
      // 
      stopButton.BackColor = SystemColors.Control;
      stopButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      stopButton.Location = new Point(238, 133);
      stopButton.Name = "stopButton";
      stopButton.Size = new Size(129, 46);
      stopButton.TabIndex = 6;
      stopButton.Text = "Stop";
      stopButton.UseVisualStyleBackColor = false;
      stopButton.Click += stopButton_Click;
      // 
      // continueButton
      // 
      continueButton.BackColor = SystemColors.Control;
      continueButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      continueButton.Location = new Point(238, 80);
      continueButton.Name = "continueButton";
      continueButton.Size = new Size(129, 46);
      continueButton.TabIndex = 6;
      continueButton.Text = "Continue";
      continueButton.UseVisualStyleBackColor = false;
      continueButton.Click += continueButton_Click;
      // 
      // depositButton
      // 
      depositButton.BackColor = SystemColors.Control;
      depositButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      depositButton.Location = new Point(385, 133);
      depositButton.Name = "depositButton";
      depositButton.Size = new Size(161, 46);
      depositButton.TabIndex = 6;
      depositButton.Text = "Deposit";
      depositButton.UseVisualStyleBackColor = false;
      depositButton.Click += depositButton_Click;
      // 
      // examineButton
      // 
      examineButton.BackColor = SystemColors.Control;
      examineButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      examineButton.Location = new Point(385, 80);
      examineButton.Name = "examineButton";
      examineButton.Size = new Size(161, 46);
      examineButton.TabIndex = 6;
      examineButton.Text = "Examine";
      examineButton.UseVisualStyleBackColor = false;
      examineButton.Click += examineButton_Click;
      // 
      // loadAddrButton
      // 
      loadAddrButton.BackColor = SystemColors.Control;
      loadAddrButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      loadAddrButton.Location = new Point(385, 28);
      loadAddrButton.Name = "loadAddrButton";
      loadAddrButton.Size = new Size(161, 46);
      loadAddrButton.TabIndex = 6;
      loadAddrButton.Text = "Load Addr";
      loadAddrButton.UseVisualStyleBackColor = false;
      loadAddrButton.Click += loadAddrButton_Click;
      // 
      // startButton
      // 
      startButton.BackColor = SystemColors.Control;
      startButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      startButton.Location = new Point(238, 28);
      startButton.Name = "startButton";
      startButton.Size = new Size(129, 46);
      startButton.TabIndex = 6;
      startButton.Text = "Start";
      startButton.UseVisualStyleBackColor = false;
      startButton.Click += startButton_Click;
      // 
      // xModeLabel
      // 
      xModeLabel.BackColor = Color.White;
      xModeLabel.BorderStyle = BorderStyle.Fixed3D;
      xModeLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      xModeLabel.Location = new Point(61, 287);
      xModeLabel.Name = "xModeLabel";
      xModeLabel.Size = new Size(31, 40);
      xModeLabel.TabIndex = 5;
      xModeLabel.Text = "0";
      xModeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // xRegLabel
      // 
      xRegLabel.BackColor = Color.White;
      xRegLabel.BorderStyle = BorderStyle.Fixed3D;
      xRegLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      xRegLabel.Location = new Point(112, 287);
      xRegLabel.Name = "xRegLabel";
      xRegLabel.Size = new Size(96, 40);
      xRegLabel.TabIndex = 5;
      xRegLabel.Text = "0000";
      xRegLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // ionLabel
      // 
      ionLabel.BackColor = Color.White;
      ionLabel.BorderStyle = BorderStyle.Fixed3D;
      ionLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      ionLabel.Location = new Point(344, 293);
      ionLabel.Name = "ionLabel";
      ionLabel.Size = new Size(51, 40);
      ionLabel.TabIndex = 5;
      ionLabel.Text = "00";
      ionLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // maskLabel
      // 
      maskLabel.BackColor = Color.White;
      maskLabel.BorderStyle = BorderStyle.Fixed3D;
      maskLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      maskLabel.Location = new Point(450, 241);
      maskLabel.Name = "maskLabel";
      maskLabel.Size = new Size(96, 40);
      maskLabel.TabIndex = 5;
      maskLabel.Text = "0000";
      maskLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // mqLabel
      // 
      mqLabel.BackColor = Color.White;
      mqLabel.BorderStyle = BorderStyle.Fixed3D;
      mqLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      mqLabel.Location = new Point(112, 234);
      mqLabel.Name = "mqLabel";
      mqLabel.Size = new Size(96, 40);
      mqLabel.TabIndex = 5;
      mqLabel.Text = "0000";
      mqLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // acLabel
      // 
      acLabel.BackColor = Color.White;
      acLabel.BorderStyle = BorderStyle.Fixed3D;
      acLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      acLabel.Location = new Point(96, 181);
      acLabel.Name = "acLabel";
      acLabel.Size = new Size(112, 40);
      acLabel.TabIndex = 5;
      acLabel.Text = "00000";
      acLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // mbrLabel
      // 
      mbrLabel.BackColor = Color.White;
      mbrLabel.BorderStyle = BorderStyle.Fixed3D;
      mbrLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      mbrLabel.Location = new Point(112, 128);
      mbrLabel.Name = "mbrLabel";
      mbrLabel.Size = new Size(96, 40);
      mbrLabel.TabIndex = 5;
      mbrLabel.Text = "0000";
      mbrLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // marLabel
      // 
      marLabel.BackColor = Color.White;
      marLabel.BorderStyle = BorderStyle.Fixed3D;
      marLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      marLabel.Location = new Point(112, 75);
      marLabel.Name = "marLabel";
      marLabel.Size = new Size(96, 40);
      marLabel.TabIndex = 5;
      marLabel.Text = "0000";
      marLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // pcLabel
      // 
      pcLabel.BackColor = Color.White;
      pcLabel.BorderStyle = BorderStyle.Fixed3D;
      pcLabel.Font = new Font("Consolas", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      pcLabel.Location = new Point(81, 22);
      pcLabel.Name = "pcLabel";
      pcLabel.Size = new Size(127, 40);
      pcLabel.TabIndex = 5;
      pcLabel.Text = "000000";
      pcLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label6.Location = new Point(27, 292);
      label6.Name = "label6";
      label6.Size = new Size(30, 32);
      label6.TabIndex = 4;
      label6.Text = "X";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label5.Location = new Point(27, 239);
      label5.Name = "label5";
      label5.Size = new Size(55, 32);
      label5.TabIndex = 4;
      label5.Text = "MQ";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label4.Location = new Point(17, 186);
      label4.Name = "label4";
      label4.Size = new Size(69, 32);
      label4.TabIndex = 3;
      label4.Text = "L/AC";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label3.Location = new Point(17, 133);
      label3.Name = "label3";
      label3.Size = new Size(68, 32);
      label3.TabIndex = 2;
      label3.Text = "MBR";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      label2.Location = new Point(16, 80);
      label2.Name = "label2";
      label2.Size = new Size(70, 32);
      label2.TabIndex = 1;
      label2.Text = "MAR";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
      label1.Location = new Point(39, 27);
      label1.Margin = new Padding(4, 0, 4, 0);
      label1.Name = "label1";
      label1.Size = new Size(44, 32);
      label1.TabIndex = 0;
      label1.Text = "PC";
      // 
      // runTimer
      // 
      runTimer.Interval = 3;
      runTimer.Tick += runTimer_Tick;
      // 
      // ConsoleForm
      // 
      AutoScaleDimensions = new SizeF(144F, 144F);
      AutoScaleMode = AutoScaleMode.Dpi;
      BackgroundImageLayout = ImageLayout.Zoom;
      ClientSize = new Size(702, 511);
      Controls.Add(panel1);
      Controls.Add(menuStrip1);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      Icon = (Icon)resources.GetObject("$this.Icon");
      MainMenuStrip = menuStrip1;
      Margin = new Padding(4, 3, 4, 3);
      MaximizeBox = false;
      Name = "ConsoleForm";
      StartPosition = FormStartPosition.Manual;
      Text = "PDP-8/I";
      FormClosing += ConsoleForm_FormClosing;
      Shown += ConsoleForm_Shown;
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem devicesToolStripMenuItem;
    private ToolStripMenuItem tty1ConsoleToolStripMenuItem;
    private ToolStripMenuItem optionsToolStripMenuItem;
    private Panel panel1;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private Label pcLabel;
    private Label mqLabel;
    private Label acLabel;
    private Label mbrLabel;
    private Label marLabel;
    private TextBox switchText;
    private Label cycleLabel;
    private System.Windows.Forms.Timer runTimer;
    private Label burstCycleTimeLabel;
    private ToolStripMenuItem loadListingToolStripMenuItem;
    private Label busyTimeLabel;
    private Label cpuCycleTimeLabel;
    private ToolStripMenuItem x10ToolStripMenuItem;
    private ToolStripMenuItem oS8ToolStripMenuItem;
    private Label disasmLabel;
    private ToolStripMenuItem coreDumpToolStripMenuItem;
    private ToolStripMenuItem tTY2ToolStripMenuItem;
    private ToolStripMenuItem rK05ToolStripMenuItem;
    private Label label6;
    private Label xRegLabel;
    private Label xModeLabel;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem splitListingToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem writeBinaryToolStripMenuItem;
    private ToolStripMenuItem writeSourceToolStripMenuItem;
    private Label instrLabel;
    private ShapeControls.ShapeControl ionShape;
    private Label label7;
    private Label label8;
    private Label irqLabel;
    private ToolStripMenuItem showFrontPanelToolStripMenuItem;
    public Button instrButton;
    public Button stepButton;
    public Button stopButton;
    public Button continueButton;
    public Button startButton;
    public Button depositButton;
    public Button examineButton;
    public Button loadAddrButton;
    private ToolStripMenuItem newToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem saveAsToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem analyzeToolStripMenuItem;
    private ToolStripMenuItem recordToolStripMenuItem;
    private ToolStripMenuItem firstEventToolStripMenuItem;
    private ToolStripMenuItem nextEventToolStripMenuItem;
    private ToolStripMenuItem previousEventToolStripMenuItem;
    private ToolStripMenuItem showListingToolStripMenuItem1;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem clearEventsToolStripMenuItem;
    private Label realTimeLabel;
    private Label cycleCountLabel;
    private Label eventCountLabel;
    private Label label9;
    private Label label10;
    private Label maskLabel;
    private Label ionLabel;
    private ToolStripMenuItem hRPToolStripMenuItem;
    private ToolStripMenuItem tektronix611ToolStripMenuItem;
    private ToolStripMenuItem recordOnlyBreaksToolStripMenuItem;
    private ToolStripMenuItem readListingToolStripMenuItem;
  }
}
