namespace eCustoms
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.BasicInfo_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.BasicData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.RMReceiving_TMSI = new System.Windows.Forms.ToolStripMenuItem();
            this.RMBalance_TMSI = new System.Windows.Forms.ToolStripMenuItem();
            this.RMShareOut_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.BOM_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetBomData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetBomDocument_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.BomHistoryData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GongDan_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetGongDanList_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetGongDanData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetGongDanDocument_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GongDanHistoryData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.BeiAnDan_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetBeiAnDanData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetBeiAnDanDocument_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.BeiAnDanHistoryData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetBAD_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.PingDan_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetPingDanData_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.GetPD_TSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.lblName = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BasicInfo_TSMI,
            this.BOM_TSMI,
            this.GongDan_TSMI,
            this.BeiAnDan_TSMI,
            this.PingDan_TSMI,
            this.ExitTSMI});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1334, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // BasicInfo_TSMI
            // 
            this.BasicInfo_TSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BasicData_TSMI,
            this.RMReceiving_TMSI,
            this.RMBalance_TMSI,
            this.RMShareOut_TSMI});
            this.BasicInfo_TSMI.Name = "BasicInfo_TSMI";
            this.BasicInfo_TSMI.Size = new System.Drawing.Size(73, 20);
            this.BasicInfo_TSMI.Text = "Basic Info.";
            // 
            // BasicData_TSMI
            // 
            this.BasicData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("BasicData_TSMI.Image")));
            this.BasicData_TSMI.Name = "BasicData_TSMI";
            this.BasicData_TSMI.Size = new System.Drawing.Size(152, 22);
            this.BasicData_TSMI.Text = "Basic Data";
            this.BasicData_TSMI.Click += new System.EventHandler(this.BasicData_TSMI_Click);
            // 
            // RMReceiving_TMSI
            // 
            this.RMReceiving_TMSI.Image = ((System.Drawing.Image)(resources.GetObject("RMReceiving_TMSI.Image")));
            this.RMReceiving_TMSI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RMReceiving_TMSI.Name = "RMReceiving_TMSI";
            this.RMReceiving_TMSI.Size = new System.Drawing.Size(152, 22);
            this.RMReceiving_TMSI.Text = "RM Receiving";
            this.RMReceiving_TMSI.Click += new System.EventHandler(this.RMReceiving_TMSI_Click);
            // 
            // RMBalance_TMSI
            // 
            this.RMBalance_TMSI.Image = ((System.Drawing.Image)(resources.GetObject("RMBalance_TMSI.Image")));
            this.RMBalance_TMSI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RMBalance_TMSI.Name = "RMBalance_TMSI";
            this.RMBalance_TMSI.Size = new System.Drawing.Size(152, 22);
            this.RMBalance_TMSI.Text = "RM Balance";
            this.RMBalance_TMSI.Click += new System.EventHandler(this.RMBalance_TMSI_Click);
            // 
            // RMShareOut_TSMI
            // 
            this.RMShareOut_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("RMShareOut_TSMI.Image")));
            this.RMShareOut_TSMI.Name = "RMShareOut_TSMI";
            this.RMShareOut_TSMI.Size = new System.Drawing.Size(152, 22);
            this.RMShareOut_TSMI.Text = "RM Share Out";
            // 
            // BOM_TSMI
            // 
            this.BOM_TSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GetBomData_TSMI,
            this.GetBomDocument_TSMI,
            this.BomHistoryData_TSMI});
            this.BOM_TSMI.Name = "BOM_TSMI";
            this.BOM_TSMI.Size = new System.Drawing.Size(46, 20);
            this.BOM_TSMI.Text = "BOM";
            // 
            // GetBomData_TSMI
            // 
            this.GetBomData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetBomData_TSMI.Image")));
            this.GetBomData_TSMI.Name = "GetBomData_TSMI";
            this.GetBomData_TSMI.Size = new System.Drawing.Size(181, 22);
            this.GetBomData_TSMI.Text = "Get BOM Data";
            this.GetBomData_TSMI.Click += new System.EventHandler(this.GetBomData_TSMI_Click);
            // 
            // GetBomDocument_TSMI
            // 
            this.GetBomDocument_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetBomDocument_TSMI.Image")));
            this.GetBomDocument_TSMI.Name = "GetBomDocument_TSMI";
            this.GetBomDocument_TSMI.Size = new System.Drawing.Size(181, 22);
            this.GetBomDocument_TSMI.Text = "Get BOM Document";
            this.GetBomDocument_TSMI.Click += new System.EventHandler(this.GetBomDocument_TSMI_Click);
            // 
            // BomHistoryData_TSMI
            // 
            this.BomHistoryData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("BomHistoryData_TSMI.Image")));
            this.BomHistoryData_TSMI.Name = "BomHistoryData_TSMI";
            this.BomHistoryData_TSMI.Size = new System.Drawing.Size(181, 22);
            this.BomHistoryData_TSMI.Text = "BOM History Data";
            this.BomHistoryData_TSMI.Click += new System.EventHandler(this.BomHistoryData_TSMI_Click);
            // 
            // GongDan_TSMI
            // 
            this.GongDan_TSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GetGongDanList_TSMI,
            this.GetGongDanData_TSMI,
            this.GetGongDanDocument_TSMI,
            this.GongDanHistoryData_TSMI});
            this.GongDan_TSMI.Name = "GongDan_TSMI";
            this.GongDan_TSMI.Size = new System.Drawing.Size(69, 20);
            this.GongDan_TSMI.Text = "GongDan";
            // 
            // GetGongDanList_TSMI
            // 
            this.GetGongDanList_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetGongDanList_TSMI.Image")));
            this.GetGongDanList_TSMI.Name = "GetGongDanList_TSMI";
            this.GetGongDanList_TSMI.Size = new System.Drawing.Size(204, 22);
            this.GetGongDanList_TSMI.Text = "Get GongDan List";
            this.GetGongDanList_TSMI.Click += new System.EventHandler(this.GetGongDanList_TSMI_Click);
            // 
            // GetGongDanData_TSMI
            // 
            this.GetGongDanData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetGongDanData_TSMI.Image")));
            this.GetGongDanData_TSMI.Name = "GetGongDanData_TSMI";
            this.GetGongDanData_TSMI.Size = new System.Drawing.Size(204, 22);
            this.GetGongDanData_TSMI.Text = "Get GongDan Data";
            this.GetGongDanData_TSMI.Click += new System.EventHandler(this.GetGongDanData_TSMI_Click);
            // 
            // GetGongDanDocument_TSMI
            // 
            this.GetGongDanDocument_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetGongDanDocument_TSMI.Image")));
            this.GetGongDanDocument_TSMI.Name = "GetGongDanDocument_TSMI";
            this.GetGongDanDocument_TSMI.Size = new System.Drawing.Size(204, 22);
            this.GetGongDanDocument_TSMI.Text = "Get GongDan Document";
            this.GetGongDanDocument_TSMI.Click += new System.EventHandler(this.GetGongDanDocument_TSMI_Click);
            // 
            // GongDanHistoryData_TSMI
            // 
            this.GongDanHistoryData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GongDanHistoryData_TSMI.Image")));
            this.GongDanHistoryData_TSMI.Name = "GongDanHistoryData_TSMI";
            this.GongDanHistoryData_TSMI.Size = new System.Drawing.Size(204, 22);
            this.GongDanHistoryData_TSMI.Text = "GongDan History Data";
            this.GongDanHistoryData_TSMI.Click += new System.EventHandler(this.GongDanHistoryData_TSMI_Click);
            // 
            // BeiAnDan_TSMI
            // 
            this.BeiAnDan_TSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GetBeiAnDanData_TSMI,
            this.GetBeiAnDanDocument_TSMI,
            this.BeiAnDanHistoryData_TSMI,
            this.GetBAD_TSMI});
            this.BeiAnDan_TSMI.Name = "BeiAnDan_TSMI";
            this.BeiAnDan_TSMI.Size = new System.Drawing.Size(71, 20);
            this.BeiAnDan_TSMI.Text = "BeiAnDan";
            // 
            // GetBeiAnDanData_TSMI
            // 
            this.GetBeiAnDanData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetBeiAnDanData_TSMI.Image")));
            this.GetBeiAnDanData_TSMI.Name = "GetBeiAnDanData_TSMI";
            this.GetBeiAnDanData_TSMI.Size = new System.Drawing.Size(256, 22);
            this.GetBeiAnDanData_TSMI.Text = "Get BeiAnDan Data";
            this.GetBeiAnDanData_TSMI.Click += new System.EventHandler(this.GetBeiAnDanData_TSMI_Click);
            // 
            // GetBeiAnDanDocument_TSMI
            // 
            this.GetBeiAnDanDocument_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetBeiAnDanDocument_TSMI.Image")));
            this.GetBeiAnDanDocument_TSMI.Name = "GetBeiAnDanDocument_TSMI";
            this.GetBeiAnDanDocument_TSMI.Size = new System.Drawing.Size(256, 22);
            this.GetBeiAnDanDocument_TSMI.Text = "Get BeiAnDan Document";
            this.GetBeiAnDanDocument_TSMI.Click += new System.EventHandler(this.GetBeiAnDanDocument_TSMI_Click);
            // 
            // BeiAnDanHistoryData_TSMI
            // 
            this.BeiAnDanHistoryData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("BeiAnDanHistoryData_TSMI.Image")));
            this.BeiAnDanHistoryData_TSMI.Name = "BeiAnDanHistoryData_TSMI";
            this.BeiAnDanHistoryData_TSMI.Size = new System.Drawing.Size(256, 22);
            this.BeiAnDanHistoryData_TSMI.Text = "BeiAnDan History Data";
            this.BeiAnDanHistoryData_TSMI.Click += new System.EventHandler(this.BeiAnDanHistoryData_TSMI_Click);
            // 
            // GetBAD_TSMI
            // 
            this.GetBAD_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetBAD_TSMI.Image")));
            this.GetBAD_TSMI.Name = "GetBAD_TSMI";
            this.GetBAD_TSMI.Size = new System.Drawing.Size(256, 22);
            this.GetBAD_TSMI.Text = "Get BeiAnDan Data && Doc. (RM-D)";
            this.GetBAD_TSMI.Click += new System.EventHandler(this.GetBAD_TSMI_Click);
            // 
            // PingDan_TSMI
            // 
            this.PingDan_TSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GetPingDanData_TSMI,
            this.GetPD_TSMI});
            this.PingDan_TSMI.Name = "PingDan_TSMI";
            this.PingDan_TSMI.Size = new System.Drawing.Size(64, 20);
            this.PingDan_TSMI.Text = "PingDan";
            // 
            // GetPingDanData_TSMI
            // 
            this.GetPingDanData_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetPingDanData_TSMI.Image")));
            this.GetPingDanData_TSMI.Name = "GetPingDanData_TSMI";
            this.GetPingDanData_TSMI.Size = new System.Drawing.Size(209, 22);
            this.GetPingDanData_TSMI.Text = "Get PingDan Data";
            this.GetPingDanData_TSMI.Click += new System.EventHandler(this.GetPingDanData_TSMI_Click);
            // 
            // GetPD_TSMI
            // 
            this.GetPD_TSMI.Image = ((System.Drawing.Image)(resources.GetObject("GetPD_TSMI.Image")));
            this.GetPD_TSMI.Name = "GetPD_TSMI";
            this.GetPD_TSMI.Size = new System.Drawing.Size(209, 22);
            this.GetPD_TSMI.Text = "Get PingDan Data (RM-D)";
            this.GetPD_TSMI.Click += new System.EventHandler(this.GetPD_TSMI_Click);
            // 
            // ExitTSMI
            // 
            this.ExitTSMI.Name = "ExitTSMI";
            this.ExitTSMI.Size = new System.Drawing.Size(37, 20);
            this.ExitTSMI.Text = "Exit";
            this.ExitTSMI.Click += new System.EventHandler(this.ExitTSMI_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.ForeColor = System.Drawing.Color.Navy;
            this.lblName.Location = new System.Drawing.Point(1168, 4);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 17);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Johnnie Li";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.DarkGray;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTitle.Location = new System.Drawing.Point(468, 291);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(467, 27);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Welcome to Shanghai Customs Track System";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1334, 702);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SABIC : Shanghai Customs Track System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ExitTSMI;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolStripMenuItem BasicInfo_TSMI;
        private System.Windows.Forms.ToolStripMenuItem RMReceiving_TMSI;
        private System.Windows.Forms.ToolStripMenuItem RMBalance_TMSI;
        private System.Windows.Forms.ToolStripMenuItem BOM_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetBomData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetBomDocument_TSMI;
        private System.Windows.Forms.ToolStripMenuItem BomHistoryData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GongDan_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetGongDanList_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetGongDanData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetGongDanDocument_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GongDanHistoryData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem BasicData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem RMShareOut_TSMI;
        private System.Windows.Forms.ToolStripMenuItem BeiAnDan_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetBeiAnDanData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetBAD_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetBeiAnDanDocument_TSMI;
        private System.Windows.Forms.ToolStripMenuItem BeiAnDanHistoryData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem PingDan_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetPingDanData_TSMI;
        private System.Windows.Forms.ToolStripMenuItem GetPD_TSMI;
    }
}