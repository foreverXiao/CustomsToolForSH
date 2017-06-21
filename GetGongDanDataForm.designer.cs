namespace SHCustomsSystem
{
    partial class GetGongDanDataForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetGongDanDataForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnCheckRcvdDate = new System.Windows.Forms.Button();
            this.btnCheckBalance = new System.Windows.Forms.Button();
            this.btnCheckPrice = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGatherData = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvGongDanData = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvUpdate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvLink = new System.Windows.Forms.DataGridViewLinkColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExceptionFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckPrice = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckRcvdDate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckBalance = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBlankFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.btnOptimizeData = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanData)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(2, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1290, 92);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnPreview);
            this.panel3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.Location = new System.Drawing.Point(1168, 11);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(114, 76);
            this.panel3.TabIndex = 9;
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnPreview.Location = new System.Drawing.Point(15, 11);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(80, 50);
            this.btnPreview.TabIndex = 22;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.btnUpload);
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.txtPath);
            this.panel4.Controls.Add(this.groupBox5);
            this.panel4.Controls.Add(this.btnDownload);
            this.panel4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(826, 11);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 76);
            this.panel4.TabIndex = 8;
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpload.Location = new System.Drawing.Point(245, 34);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 30);
            this.btnUpload.TabIndex = 25;
            this.btnUpload.Text = " Upload";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.Location = new System.Drawing.Point(133, 34);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Text = "  Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.ForeColor = System.Drawing.Color.Red;
            this.txtPath.Location = new System.Drawing.Point(134, 9);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(190, 23);
            this.txtPath.TabIndex = 23;
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(117, -8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(2, 82);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDownload.Location = new System.Drawing.Point(14, 11);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(90, 50);
            this.btnDownload.TabIndex = 20;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.btnCheckRcvdDate);
            this.panel2.Controls.Add(this.btnCheckBalance);
            this.panel2.Controls.Add(this.btnCheckPrice);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(150, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(676, 76);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(11, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(272, 17);
            this.label2.TabIndex = 30;
            this.label2.Text = "2. Check Customs Receipt Date is blank yet";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Aquamarine;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(292, 28);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 16);
            this.textBox2.TabIndex = 29;
            // 
            // btnCheckRcvdDate
            // 
            this.btnCheckRcvdDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckRcvdDate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckRcvdDate.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckRcvdDate.Image")));
            this.btnCheckRcvdDate.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCheckRcvdDate.Location = new System.Drawing.Point(507, 11);
            this.btnCheckRcvdDate.Name = "btnCheckRcvdDate";
            this.btnCheckRcvdDate.Size = new System.Drawing.Size(70, 50);
            this.btnCheckRcvdDate.TabIndex = 28;
            this.btnCheckRcvdDate.Text = "Check 2.";
            this.btnCheckRcvdDate.UseVisualStyleBackColor = true;
            this.btnCheckRcvdDate.Click += new System.EventHandler(this.btnCheckRcvdDate_Click);
            // 
            // btnCheckBalance
            // 
            this.btnCheckBalance.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckBalance.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckBalance.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckBalance.Image")));
            this.btnCheckBalance.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCheckBalance.Location = new System.Drawing.Point(589, 11);
            this.btnCheckBalance.Name = "btnCheckBalance";
            this.btnCheckBalance.Size = new System.Drawing.Size(70, 50);
            this.btnCheckBalance.TabIndex = 27;
            this.btnCheckBalance.Text = "Check 3.";
            this.btnCheckBalance.UseVisualStyleBackColor = true;
            this.btnCheckBalance.Click += new System.EventHandler(this.btnCheckBalance_Click);
            // 
            // btnCheckPrice
            // 
            this.btnCheckPrice.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckPrice.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckPrice.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckPrice.Image")));
            this.btnCheckPrice.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCheckPrice.Location = new System.Drawing.Point(425, 11);
            this.btnCheckPrice.Name = "btnCheckPrice";
            this.btnCheckPrice.Size = new System.Drawing.Size(70, 50);
            this.btnCheckPrice.TabIndex = 26;
            this.btnCheckPrice.Text = "Check 1.";
            this.btnCheckPrice.UseVisualStyleBackColor = true;
            this.btnCheckPrice.Click += new System.EventHandler(this.btnCheckPrice_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(409, -8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(2, 82);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Khaki;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(292, 47);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 16);
            this.textBox3.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(11, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(274, 17);
            this.label3.TabIndex = 23;
            this.label3.Text = "3. Check RM Balance via Customs# && BGD#";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "1. Total RM Cost > Total GongDan Value";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightPink;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(292, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 16);
            this.textBox1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnOptimizeData);
            this.panel1.Controls.Add(this.btnGatherData);
            this.panel1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(8, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(142, 76);
            this.panel1.TabIndex = 0;
            // 
            // btnGatherData
            // 
            this.btnGatherData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGatherData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGatherData.Image = ((System.Drawing.Image)(resources.GetObject("btnGatherData.Image")));
            this.btnGatherData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGatherData.Location = new System.Drawing.Point(14, 6);
            this.btnGatherData.Name = "btnGatherData";
            this.btnGatherData.Size = new System.Drawing.Size(112, 30);
            this.btnGatherData.TabIndex = 0;
            this.btnGatherData.Text = " Gather Data";
            this.btnGatherData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGatherData.UseVisualStyleBackColor = true;
            this.btnGatherData.Click += new System.EventHandler(this.btnGatherData_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvGongDanData);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(2, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1290, 592);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // dgvGongDanData
            // 
            this.dgvGongDanData.AllowUserToAddRows = false;
            this.dgvGongDanData.AllowUserToDeleteRows = false;
            this.dgvGongDanData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvGongDanData.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvGongDanData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGongDanData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGongDanData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.dgvDelete,
            this.dgvUpdate,
            this.dgvLink});
            this.dgvGongDanData.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGongDanData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGongDanData.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvGongDanData.Location = new System.Drawing.Point(4, 23);
            this.dgvGongDanData.Name = "dgvGongDanData";
            this.dgvGongDanData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGongDanData.RowTemplate.Height = 23;
            this.dgvGongDanData.Size = new System.Drawing.Size(1282, 564);
            this.dgvGongDanData.TabIndex = 0;
            this.dgvGongDanData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGongDanData_CellClick);
            this.dgvGongDanData.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanData_CellMouseUp);
            this.dgvGongDanData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanData_ColumnHeaderMouseClick);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "全选";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 38;
            // 
            // dgvDelete
            // 
            this.dgvDelete.HeaderText = "";
            this.dgvDelete.Name = "dgvDelete";
            this.dgvDelete.Text = "Delete";
            this.dgvDelete.UseColumnTextForButtonValue = true;
            this.dgvDelete.Width = 5;
            // 
            // dgvUpdate
            // 
            this.dgvUpdate.HeaderText = "";
            this.dgvUpdate.Name = "dgvUpdate";
            this.dgvUpdate.Text = "Update";
            this.dgvUpdate.UseColumnTextForButtonValue = true;
            this.dgvUpdate.Width = 5;
            // 
            // dgvLink
            // 
            this.dgvLink.HeaderText = "";
            this.dgvLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.dgvLink.Name = "dgvLink";
            this.dgvLink.Text = "View Detail";
            this.dgvLink.UseColumnTextForLinkValue = true;
            this.dgvLink.Width = 5;
            // 
            // cmsFilter
            // 
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter,
            this.tsmiExceptionFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(155, 114);
            // 
            // tsmiChooseFilter
            // 
            this.tsmiChooseFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiChooseFilter.Image")));
            this.tsmiChooseFilter.Name = "tsmiChooseFilter";
            this.tsmiChooseFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiChooseFilter.Text = "Choose Filter";
            this.tsmiChooseFilter.Click += new System.EventHandler(this.tsmiChooseFilter_Click);
            // 
            // tsmiExcludeFilter
            // 
            this.tsmiExcludeFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExcludeFilter.Image")));
            this.tsmiExcludeFilter.Name = "tsmiExcludeFilter";
            this.tsmiExcludeFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiExcludeFilter.Text = "Exclude Filter";
            this.tsmiExcludeFilter.Click += new System.EventHandler(this.tsmiExcludeFilter_Click);
            // 
            // tsmiRefreshFilter
            // 
            this.tsmiRefreshFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRefreshFilter.Image")));
            this.tsmiRefreshFilter.Name = "tsmiRefreshFilter";
            this.tsmiRefreshFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiRefreshFilter.Text = "Refresh Filter";
            this.tsmiRefreshFilter.Click += new System.EventHandler(this.tsmiRefreshFilter_Click);
            // 
            // tsmiRecordFilter
            // 
            this.tsmiRecordFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRecordFilter.Image")));
            this.tsmiRecordFilter.Name = "tsmiRecordFilter";
            this.tsmiRecordFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiRecordFilter.Text = "Record Filter";
            this.tsmiRecordFilter.Click += new System.EventHandler(this.tsmiRecordFilter_Click);
            // 
            // tsmiExceptionFilter
            // 
            this.tsmiExceptionFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheckPrice,
            this.tsmiCheckRcvdDate,
            this.tsmiCheckBalance,
            this.tsmiBlankFilter});
            this.tsmiExceptionFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExceptionFilter.Image")));
            this.tsmiExceptionFilter.Name = "tsmiExceptionFilter";
            this.tsmiExceptionFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiExceptionFilter.Text = "Exception Filter";
            // 
            // tsmiCheckPrice
            // 
            this.tsmiCheckPrice.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheckPrice.Image")));
            this.tsmiCheckPrice.Name = "tsmiCheckPrice";
            this.tsmiCheckPrice.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheckPrice.Text = "Check 1. Filter";
            this.tsmiCheckPrice.Click += new System.EventHandler(this.tsmiCheckPrice_Click);
            // 
            // tsmiCheckRcvdDate
            // 
            this.tsmiCheckRcvdDate.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheckRcvdDate.Image")));
            this.tsmiCheckRcvdDate.Name = "tsmiCheckRcvdDate";
            this.tsmiCheckRcvdDate.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheckRcvdDate.Text = "Check 2. Filter";
            this.tsmiCheckRcvdDate.Click += new System.EventHandler(this.tsmiCheckRcvdDate_Click);
            // 
            // tsmiCheckBalance
            // 
            this.tsmiCheckBalance.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheckBalance.Image")));
            this.tsmiCheckBalance.Name = "tsmiCheckBalance";
            this.tsmiCheckBalance.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheckBalance.Text = "Check 3. Filter";
            this.tsmiCheckBalance.Click += new System.EventHandler(this.tsmiCheckBalance_Click);
            // 
            // tsmiBlankFilter
            // 
            this.tsmiBlankFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiBlankFilter.Image")));
            this.tsmiBlankFilter.Name = "tsmiBlankFilter";
            this.tsmiBlankFilter.Size = new System.Drawing.Size(148, 22);
            this.tsmiBlankFilter.Text = "Blank Filter";
            this.tsmiBlankFilter.Click += new System.EventHandler(this.tsmiBlankFilter_Click);
            // 
            // fakeLabel1
            // 
            this.fakeLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.fakeLabel1.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.fakeLabel1.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel1.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel1.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel1.CImage = null;
            this.fakeLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.fakeLabel1.LabelText = "";
            this.fakeLabel1.Location = new System.Drawing.Point(2, 89);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1290, 11);
            this.fakeLabel1.TabIndex = 6;
            // 
            // btnOptimizeData
            // 
            this.btnOptimizeData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOptimizeData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnOptimizeData.Image = ((System.Drawing.Image)(resources.GetObject("btnOptimizeData.Image")));
            this.btnOptimizeData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOptimizeData.Location = new System.Drawing.Point(14, 38);
            this.btnOptimizeData.Name = "btnOptimizeData";
            this.btnOptimizeData.Size = new System.Drawing.Size(112, 30);
            this.btnOptimizeData.TabIndex = 1;
            this.btnOptimizeData.Text = " Update Data";
            this.btnOptimizeData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptimizeData.UseVisualStyleBackColor = true;
            this.btnOptimizeData.Click += new System.EventHandler(this.btnOptimizeData_Click);
            // 
            // GetGongDanDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 685);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetGongDanDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get GongDan Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetGongDanDataForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanData)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGatherData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvGongDanData;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCheckBalance;
        private System.Windows.Forms.Button btnCheckPrice;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDelete;
        private System.Windows.Forms.DataGridViewButtonColumn dgvUpdate;
        private System.Windows.Forms.DataGridViewLinkColumn dgvLink;
        private System.Windows.Forms.Button btnCheckRcvdDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExceptionFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckPrice;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckRcvdDate;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckBalance;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiBlankFilter;
        private System.Windows.Forms.Button btnOptimizeData;
    }
}