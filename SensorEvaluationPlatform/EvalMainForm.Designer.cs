namespace SensorEvaluationPlatform
{
    partial class EvalPlatform
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EvalPlatform));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbStart = new System.Windows.Forms.ToolStripButton();
            this.tsbConnect = new System.Windows.Forms.ToolStripButton();
            this.tsbLog = new System.Windows.Forms.ToolStripButton();
            this.tsbDarkMode = new System.Windows.Forms.ToolStripButton();
            this.tsbVer = new System.Windows.Forms.ToolStripTextBox();
            this.tslPoints = new System.Windows.Forms.ToolStripLabel();
            this.tslMem = new System.Windows.Forms.ToolStripLabel();
            this.tslCPUMax = new System.Windows.Forms.ToolStripLabel();
            this.tslCPU = new System.Windows.Forms.ToolStripLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbConnected = new System.Windows.Forms.CheckBox();
            this.lblConnected = new System.Windows.Forms.Label();
            this.lblComPort = new System.Windows.Forms.Label();
            this.cbPiSelect = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.tcGraphs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ttStartStop = new System.Windows.Forms.ToolTip(this.components);
            this.ttDevices = new System.Windows.Forms.ToolTip(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.fileSystemWatcher2 = new System.IO.FileSystemWatcher();
            this.fileSystemWatcher3 = new System.IO.FileSystemWatcher();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.process1 = new System.Diagnostics.Process();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tcGraphs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 17);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStart,
            this.tsbConnect,
            this.tsbLog,
            this.tsbDarkMode,
            this.tsbVer,
            this.tslPoints,
            this.tslMem,
            this.tslCPUMax,
            this.tslCPU});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1012, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbStart
            // 
            this.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStart.Image = global::SensorEvaluationPlatform.Properties.Resources.Start;
            this.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStart.Name = "tsbStart";
            this.tsbStart.Size = new System.Drawing.Size(23, 22);
            this.tsbStart.Text = "tsbStartStop";
            this.tsbStart.ToolTipText = "Start and stop data logging";
            this.tsbStart.Click += new System.EventHandler(this.tsbStart_Click);
            // 
            // tsbConnect
            // 
            this.tsbConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbConnect.Enabled = false;
            this.tsbConnect.Image = global::SensorEvaluationPlatform.Properties.Resources.Refresh;
            this.tsbConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConnect.Name = "tsbConnect";
            this.tsbConnect.Size = new System.Drawing.Size(23, 22);
            this.tsbConnect.Text = "Scan Devices";
            this.tsbConnect.ToolTipText = "Clear graph data";
            this.tsbConnect.Click += new System.EventHandler(this.tsbConnect_Click);
            // 
            // tsbLog
            // 
            this.tsbLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLog.Image = global::SensorEvaluationPlatform.Properties.Resources.LogRed;
            this.tsbLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLog.Name = "tsbLog";
            this.tsbLog.Size = new System.Drawing.Size(23, 22);
            this.tsbLog.Text = "toolStripButton1";
            this.tsbLog.ToolTipText = "Log future data to file";
            this.tsbLog.Click += new System.EventHandler(this.tsbLog_Click);
            // 
            // tsbDarkMode
            // 
            this.tsbDarkMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDarkMode.Image = global::SensorEvaluationPlatform.Properties.Resources.half_moon;
            this.tsbDarkMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDarkMode.Name = "tsbDarkMode";
            this.tsbDarkMode.Size = new System.Drawing.Size(23, 22);
            this.tsbDarkMode.Text = "Dark Mode";
            this.tsbDarkMode.Click += new System.EventHandler(this.tsbDarkMode_Click);
            // 
            // tsbVer
            // 
            this.tsbVer.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbVer.BackColor = System.Drawing.SystemColors.MenuBar;
            this.tsbVer.Name = "tsbVer";
            this.tsbVer.ReadOnly = true;
            this.tsbVer.Size = new System.Drawing.Size(90, 25);
            this.tsbVer.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tsbVer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tsbVer_KeyDown);
            // 
            // tslPoints
            // 
            this.tslPoints.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslPoints.Name = "tslPoints";
            this.tslPoints.Size = new System.Drawing.Size(68, 22);
            this.tslPoints.Text = "Total Points";
            this.tslPoints.Visible = false;
            // 
            // tslMem
            // 
            this.tslMem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslMem.Name = "tslMem";
            this.tslMem.Size = new System.Drawing.Size(52, 22);
            this.tslMem.Text = "Memory";
            this.tslMem.Visible = false;
            // 
            // tslCPUMax
            // 
            this.tslCPUMax.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslCPUMax.Name = "tslCPUMax";
            this.tslCPUMax.Size = new System.Drawing.Size(56, 22);
            this.tslCPUMax.Text = "CPU Max";
            this.tslCPUMax.Visible = false;
            // 
            // tslCPU
            // 
            this.tslCPU.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslCPU.Name = "tslCPU";
            this.tslCPU.Size = new System.Drawing.Size(43, 22);
            this.tslCPU.Text = "CPU %";
            this.tslCPU.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(18, 43);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(198, 76);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.72165F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.27835F));
            this.tableLayoutPanel2.Controls.Add(this.cbConnected, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblConnected, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblComPort, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbPiSelect, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(194, 72);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // cbConnected
            // 
            this.cbConnected.AutoCheck = false;
            this.cbConnected.AutoSize = true;
            this.cbConnected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbConnected.Location = new System.Drawing.Point(81, 3);
            this.cbConnected.Name = "cbConnected";
            this.cbConnected.Size = new System.Drawing.Size(110, 30);
            this.cbConnected.TabIndex = 4;
            this.cbConnected.UseVisualStyleBackColor = true;
            // 
            // lblConnected
            // 
            this.lblConnected.AutoSize = true;
            this.lblConnected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnected.Location = new System.Drawing.Point(3, 0);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(72, 36);
            this.lblConnected.TabIndex = 5;
            this.lblConnected.Text = "Connected:";
            this.lblConnected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblComPort
            // 
            this.lblComPort.AutoSize = true;
            this.lblComPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComPort.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComPort.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblComPort.Location = new System.Drawing.Point(3, 36);
            this.lblComPort.Name = "lblComPort";
            this.lblComPort.Size = new System.Drawing.Size(72, 36);
            this.lblComPort.TabIndex = 1;
            this.lblComPort.Text = "Devices:";
            this.lblComPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPiSelect
            // 
            this.cbPiSelect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbPiSelect.FormattingEnabled = true;
            this.cbPiSelect.Location = new System.Drawing.Point(81, 43);
            this.cbPiSelect.Name = "cbPiSelect";
            this.cbPiSelect.Size = new System.Drawing.Size(110, 21);
            this.cbPiSelect.TabIndex = 3;
            this.cbPiSelect.SelectedValueChanged += new System.EventHandler(this.cbPiSelect_SelectedValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tableLayoutPanel1.ColumnCount = 17;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.140941F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.136495F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.136495F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.14094F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.14094F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.14094F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.14094F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.14094F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.14094F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.148084F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.148084F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.148084F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.148084F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.148084F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.rtbConsole, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tcGraphs, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 18;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.143113F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142399F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142399F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142399F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142399F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142399F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1012, 651);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.SizeChanged += new System.EventHandler(this.tableLayoutPanel1_SizeChanged);
            // 
            // rtbConsole
            // 
            this.rtbConsole.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanel1.SetColumnSpan(this.rtbConsole, 11);
            this.rtbConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConsole.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConsole.Location = new System.Drawing.Point(237, 43);
            this.rtbConsole.Name = "rtbConsole";
            this.tableLayoutPanel1.SetRowSpan(this.rtbConsole, 2);
            this.rtbConsole.Size = new System.Drawing.Size(742, 76);
            this.rtbConsole.TabIndex = 4;
            this.rtbConsole.Text = "";
            // 
            // tcGraphs
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tcGraphs, 15);
            this.tcGraphs.Controls.Add(this.tabPage1);
            this.tcGraphs.Controls.Add(this.tabPage2);
            this.tcGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcGraphs.Location = new System.Drawing.Point(18, 140);
            this.tcGraphs.Name = "tcGraphs";
            this.tableLayoutPanel1.SetRowSpan(this.tcGraphs, 12);
            this.tcGraphs.SelectedIndex = 0;
            this.tcGraphs.Size = new System.Drawing.Size(961, 486);
            this.tcGraphs.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage1.Controls.Add(this.chart1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(953, 460);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Port 0";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(943, 450);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // tabPage2
            // 
            this.tabPage2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage2.Controls.Add(this.chart2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(958, 460);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Port 1";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(3, 3);
            this.chart2.Name = "chart2";
            this.chart2.Size = new System.Drawing.Size(948, 450);
            this.chart2.TabIndex = 0;
            this.chart2.Text = "chart2";
            this.chart2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseMove);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // fileSystemWatcher2
            // 
            this.fileSystemWatcher2.EnableRaisingEvents = true;
            this.fileSystemWatcher2.SynchronizingObject = this;
            // 
            // fileSystemWatcher3
            // 
            this.fileSystemWatcher3.EnableRaisingEvents = true;
            this.fileSystemWatcher3.SynchronizingObject = this;
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshGraphToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 26);
            // 
            // refreshGraphToolStripMenuItem
            // 
            this.refreshGraphToolStripMenuItem.Name = "refreshGraphToolStripMenuItem";
            this.refreshGraphToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.refreshGraphToolStripMenuItem.Text = "Refresh Graph";
            // 
            // EvalPlatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 651);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EvalPlatform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Amphenol Sensor Evaluation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EvalPlatform_FormClosing);
            this.Load += new System.EventHandler(this.EvalPlatform_Load);
            this.Resize += new System.EventHandler(this.EvalPlatform_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tcGraphs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbConnect;
        private System.Windows.Forms.ToolStripButton tsbStart;
        private System.Windows.Forms.ToolTip ttStartStop;
        private System.Windows.Forms.ToolTip ttDevices;
        private System.Windows.Forms.ToolStripButton tsbLog;
        private System.Windows.Forms.TabControl tcGraphs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.IO.FileSystemWatcher fileSystemWatcher2;
        private System.IO.FileSystemWatcher fileSystemWatcher3;
        private System.Windows.Forms.ToolStripButton tsbDarkMode;
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.ToolStripTextBox tsbVer;
        private System.Windows.Forms.ToolStripLabel tslPoints;
        private System.Windows.Forms.ToolStripLabel tslMem;
        private System.Windows.Forms.ToolStripLabel tslCPUMax;
        private System.Windows.Forms.ToolStripLabel tslCPU;
        private System.Windows.Forms.CheckBox cbConnected;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshGraphToolStripMenuItem;
        private System.Windows.Forms.Label lblComPort;
        private System.Windows.Forms.ComboBox cbPiSelect;
        private System.Windows.Forms.Label lblConnected;
    }
}

