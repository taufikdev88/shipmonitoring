namespace shipmonitoring.Dashboard
{
    partial class Main
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
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.zedGraphWave = new ZedGraph.ZedGraphControl();
            this.zedGraphTemp = new ZedGraph.ZedGraphControl();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.txtStatusKoneksi = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.btnKoneksi = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.zedGraphPower = new ZedGraph.ZedGraphControl();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Controls.Add(this.zedGraphWave, 0, 2);
            this.tableLayoutMain.Controls.Add(this.zedGraphTemp, 0, 2);
            this.tableLayoutMain.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutMain.Controls.Add(this.zedGraphPower, 1, 1);
            this.tableLayoutMain.Controls.Add(this.gMapControl1, 0, 1);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 3;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Size = new System.Drawing.Size(966, 450);
            this.tableLayoutMain.TabIndex = 0;
            // 
            // zedGraphWave
            // 
            this.zedGraphWave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphWave.Location = new System.Drawing.Point(3, 248);
            this.zedGraphWave.Name = "zedGraphWave";
            this.zedGraphWave.ScrollGrace = 0D;
            this.zedGraphWave.ScrollMaxX = 0D;
            this.zedGraphWave.ScrollMaxY = 0D;
            this.zedGraphWave.ScrollMaxY2 = 0D;
            this.zedGraphWave.ScrollMinX = 0D;
            this.zedGraphWave.ScrollMinY = 0D;
            this.zedGraphWave.ScrollMinY2 = 0D;
            this.zedGraphWave.Size = new System.Drawing.Size(477, 199);
            this.zedGraphWave.TabIndex = 3;
            this.zedGraphWave.UseExtendedPrintDialog = true;
            // 
            // zedGraphTemp
            // 
            this.zedGraphTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphTemp.Location = new System.Drawing.Point(486, 248);
            this.zedGraphTemp.Name = "zedGraphTemp";
            this.zedGraphTemp.ScrollGrace = 0D;
            this.zedGraphTemp.ScrollMaxX = 0D;
            this.zedGraphTemp.ScrollMaxY = 0D;
            this.zedGraphTemp.ScrollMaxY2 = 0D;
            this.zedGraphTemp.ScrollMinX = 0D;
            this.zedGraphTemp.ScrollMinY = 0D;
            this.zedGraphTemp.ScrollMinY2 = 0D;
            this.zedGraphTemp.Size = new System.Drawing.Size(477, 199);
            this.zedGraphTemp.TabIndex = 2;
            this.zedGraphTemp.UseExtendedPrintDialog = true;
            // 
            // panelHeader
            // 
            this.tableLayoutMain.SetColumnSpan(this.panelHeader, 2);
            this.panelHeader.Controls.Add(this.progressBar1);
            this.panelHeader.Controls.Add(this.txtStatusKoneksi);
            this.panelHeader.Controls.Add(this.cmbPort);
            this.panelHeader.Controls.Add(this.btnKoneksi);
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(3, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(960, 34);
            this.panelHeader.TabIndex = 0;
            // 
            // txtStatusKoneksi
            // 
            this.txtStatusKoneksi.AutoSize = true;
            this.txtStatusKoneksi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusKoneksi.Location = new System.Drawing.Point(349, 11);
            this.txtStatusKoneksi.Name = "txtStatusKoneksi";
            this.txtStatusKoneksi.Size = new System.Drawing.Size(37, 13);
            this.txtStatusKoneksi.TabIndex = 2;
            this.txtStatusKoneksi.Text = "Status";
            // 
            // cmbPort
            // 
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(96, 5);
            this.cmbPort.MaxDropDownItems = 20;
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(135, 24);
            this.cmbPort.TabIndex = 1;
            this.cmbPort.Click += new System.EventHandler(this.CmbPort_Click);
            // 
            // btnKoneksi
            // 
            this.btnKoneksi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKoneksi.Location = new System.Drawing.Point(237, 3);
            this.btnKoneksi.Name = "btnKoneksi";
            this.btnKoneksi.Size = new System.Drawing.Size(106, 26);
            this.btnKoneksi.TabIndex = 1;
            this.btnKoneksi.Text = "Sambungkan";
            this.btnKoneksi.UseVisualStyleBackColor = true;
            this.btnKoneksi.Click += new System.EventHandler(this.BtnKoneksi_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pilih Koneksi :";
            // 
            // zedGraphPower
            // 
            this.zedGraphPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphPower.Location = new System.Drawing.Point(486, 43);
            this.zedGraphPower.Name = "zedGraphPower";
            this.zedGraphPower.ScrollGrace = 0D;
            this.zedGraphPower.ScrollMaxX = 0D;
            this.zedGraphPower.ScrollMaxY = 0D;
            this.zedGraphPower.ScrollMaxY2 = 0D;
            this.zedGraphPower.ScrollMinX = 0D;
            this.zedGraphPower.ScrollMinY = 0D;
            this.zedGraphPower.ScrollMinY2 = 0D;
            this.zedGraphPower.Size = new System.Drawing.Size(477, 199);
            this.zedGraphPower.TabIndex = 1;
            this.zedGraphPower.UseExtendedPrintDialog = true;
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(3, 43);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 2;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomEnabled = true;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(477, 199);
            this.gMapControl1.TabIndex = 4;
            this.gMapControl1.Zoom = 0D;
            this.gMapControl1.OnPositionChanged += new GMap.NET.PositionChanged(this.GMapControl1_OnPositionChanged);
            // 
            // serialPort
            // 
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort_DataReceived);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.ForeColor = System.Drawing.Color.White;
            this.progressBar1.Location = new System.Drawing.Point(867, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(84, 10);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Value = 100;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 450);
            this.Controls.Add(this.tableLayoutMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.Text = "Ship Monitor";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnKoneksi;
        private System.Windows.Forms.Label txtStatusKoneksi;
        private System.IO.Ports.SerialPort serialPort;
        private ZedGraph.ZedGraphControl zedGraphPower;
        private System.Windows.Forms.Timer timer1;
        private ZedGraph.ZedGraphControl zedGraphWave;
        private ZedGraph.ZedGraphControl zedGraphTemp;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}