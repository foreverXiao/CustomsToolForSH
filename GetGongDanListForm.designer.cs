namespace SHCustomsSystem
{
    partial class GetGongDanListForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetGongDanListForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblDownload = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPCItem = new System.Windows.Forms.Button();
            this.btnCheckQty = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCheck = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGatherData = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvGongDanList = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvSplit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvLink = new System.Windows.Forms.DataGridViewLinkColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExceptionFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheck1Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheck2Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheck3Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanList)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(2, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1290, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.lblDownload);
            this.panel4.Controls.Add(this.btnUpload);
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.txtPath);
            this.panel4.Controls.Add(this.groupBox4);
            this.panel4.Controls.Add(this.btnDownload);
            this.panel4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(680, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(348, 76);
            this.panel4.TabIndex = 8;
            // 
            // lblDownload
            // 
            this.lblDownload.AutoSize = true;
            this.lblDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblDownload.Location = new System.Drawing.Point(225, 41);
            this.lblDownload.Name = "lblDownload";
            this.lblDownload.Size = new System.Drawing.Size(15, 17);
            this.lblDownload.TabIndex = 31;
            this.lblDownload.Text = "¤";
            this.lblDownload.Click += new System.EventHandler(this.lblDownload_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpload.Location = new System.Drawing.Point(248, 34);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 30);
            this.btnUpload.TabIndex = 30;
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
            this.btnSearch.Location = new System.Drawing.Point(136, 34);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.TabIndex = 29;
            this.btnSearch.Text = "  Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.ForeColor = System.Drawing.Color.Red;
            this.txtPath.Location = new System.Drawing.Point(137, 9);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(190, 23);
            this.txtPath.TabIndex = 28;
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(120, -7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(2, 82);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDownload.Location = new System.Drawing.Point(16, 11);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(90, 50);
            this.btnDownload.TabIndex = 26;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnBrowse);
            this.panel3.Controls.Add(this.btnGenerate);
            this.panel3.Controls.Add(this.groupBox5);
            this.panel3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.Location = new System.Drawing.Point(1028, 13);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(256, 76);
            this.panel3.TabIndex = 7;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnBrowse.Location = new System.Drawing.Point(158, 11);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 50);
            this.btnBrowse.TabIndex = 20;
            this.btnBrowse.Text = "Preview";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGenerate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGenerate.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerate.Image")));
            this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnGenerate.Location = new System.Drawing.Point(15, 11);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(110, 50);
            this.btnGenerate.TabIndex = 19;
            this.btnGenerate.Text = "Generate List";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(141, -8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(2, 82);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnPCItem);
            this.panel2.Controls.Add(this.btnCheckQty);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnCheck);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(142, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(538, 76);
            this.panel2.TabIndex = 1;
            // 
            // btnPCItem
            // 
            this.btnPCItem.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPCItem.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPCItem.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnPCItem.Location = new System.Drawing.Point(449, 37);
            this.btnPCItem.Name = "btnPCItem";
            this.btnPCItem.Size = new System.Drawing.Size(70, 30);
            this.btnPCItem.TabIndex = 24;
            this.btnPCItem.Text = "PC Item";
            this.btnPCItem.UseVisualStyleBackColor = true;
            this.btnPCItem.Click += new System.EventHandler(this.btnPCItem_Click);
            // 
            // btnCheckQty
            // 
            this.btnCheckQty.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckQty.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckQty.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckQty.Image")));
            this.btnCheckQty.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCheckQty.Location = new System.Drawing.Point(449, 5);
            this.btnCheckQty.Name = "btnCheckQty";
            this.btnCheckQty.Size = new System.Drawing.Size(70, 30);
            this.btnCheckQty.TabIndex = 23;
            this.btnCheckQty.Text = "Check3.";
            this.btnCheckQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckQty.UseVisualStyleBackColor = true;
            this.btnCheckQty.Click += new System.EventHandler(this.btnCheckQty_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label5.Location = new System.Drawing.Point(13, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(212, 17);
            this.label5.TabIndex = 22;
            this.label5.Text = "3. SUM(GongDan Qty) >= FG Qty";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Khaki;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(233, 48);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 16);
            this.textBox2.TabIndex = 22;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Aquamarine;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(233, 28);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 16);
            this.textBox3.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(13, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(208, 17);
            this.label3.TabIndex = 20;
            this.label3.Text = "2. The Batch No. has multi-BOM.";
            // 
            // btnCheck
            // 
            this.btnCheck.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheck.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheck.Image = ((System.Drawing.Image)(resources.GetObject("btnCheck.Image")));
            this.btnCheck.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCheck.Location = new System.Drawing.Point(368, 11);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(70, 50);
            this.btnCheck.TabIndex = 16;
            this.btnCheck.Text = "Check 1.&&2.";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(351, -8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(2, 82);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "1. The BOM has not created yet.";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightPink;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(233, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 16);
            this.textBox1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnGatherData);
            this.panel1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(6, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(136, 76);
            this.panel1.TabIndex = 0;
            // 
            // btnGatherData
            // 
            this.btnGatherData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGatherData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGatherData.Image = ((System.Drawing.Image)(resources.GetObject("btnGatherData.Image")));
            this.btnGatherData.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnGatherData.Location = new System.Drawing.Point(16, 11);
            this.btnGatherData.Name = "btnGatherData";
            this.btnGatherData.Size = new System.Drawing.Size(100, 50);
            this.btnGatherData.TabIndex = 0;
            this.btnGatherData.Text = "Gather List";
            this.btnGatherData.UseVisualStyleBackColor = true;
            this.btnGatherData.Click += new System.EventHandler(this.btnGatherData_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvGongDanList);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(2, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1290, 592);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // dgvGongDanList
            // 
            this.dgvGongDanList.AllowUserToAddRows = false;
            this.dgvGongDanList.AllowUserToDeleteRows = false;
            this.dgvGongDanList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvGongDanList.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvGongDanList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGongDanList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGongDanList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.dgvDelete,
            this.dgvSplit,
            this.dgvLink});
            this.dgvGongDanList.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGongDanList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGongDanList.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvGongDanList.Location = new System.Drawing.Point(4, 23);
            this.dgvGongDanList.Name = "dgvGongDanList";
            this.dgvGongDanList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGongDanList.RowTemplate.Height = 23;
            this.dgvGongDanList.Size = new System.Drawing.Size(1282, 564);
            this.dgvGongDanList.TabIndex = 0;
            this.dgvGongDanList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGongDanList_CellClick);
            this.dgvGongDanList.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanList_CellMouseUp);
            this.dgvGongDanList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanList_ColumnHeaderMouseClick);
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
            // dgvSplit
            // 
            this.dgvSplit.HeaderText = "";
            this.dgvSplit.Name = "dgvSplit";
            this.dgvSplit.Text = "Split";
            this.dgvSplit.UseColumnTextForButtonValue = true;
            this.dgvSplit.Width = 5;
            // 
            // dgvLink
            // 
            this.dgvLink.HeaderText = "";
            this.dgvLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.dgvLink.Name = "dgvLink";
            this.dgvLink.Text = "View BOM";
            this.dgvLink.UseColumnTextForLinkValue = true;
            this.dgvLink.Width = 5;
            // 
            // cmsFilter
            // 
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExceptionFilter,
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(155, 114);
            // 
            // tsmiExceptionFilter
            // 
            this.tsmiExceptionFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheck1Filter,
            this.tsmiCheck2Filter,
            this.tsmiCheck3Filter});
            this.tsmiExceptionFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExceptionFilter.Image")));
            this.tsmiExceptionFilter.Name = "tsmiExceptionFilter";
            this.tsmiExceptionFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiExceptionFilter.Text = "Exception Filter";
            // 
            // tsmiCheck1Filter
            // 
            this.tsmiCheck1Filter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheck1Filter.Image")));
            this.tsmiCheck1Filter.Name = "tsmiCheck1Filter";
            this.tsmiCheck1Filter.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheck1Filter.Text = "Check 1. Filter";
            this.tsmiCheck1Filter.Click += new System.EventHandler(this.tsmiCheck1Filter_Click);
            // 
            // tsmiCheck2Filter
            // 
            this.tsmiCheck2Filter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheck2Filter.Image")));
            this.tsmiCheck2Filter.Name = "tsmiCheck2Filter";
            this.tsmiCheck2Filter.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheck2Filter.Text = "Check 2. Filter";
            this.tsmiCheck2Filter.Click += new System.EventHandler(this.tsmiCheck2Filter_Click);
            // 
            // tsmiCheck3Filter
            // 
            this.tsmiCheck3Filter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheck3Filter.Image")));
            this.tsmiCheck3Filter.Name = "tsmiCheck3Filter";
            this.tsmiCheck3Filter.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheck3Filter.Text = "Check 3. Filter";
            this.tsmiCheck3Filter.Click += new System.EventHandler(this.tsmiCheck3Filter_Click);
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
            this.fakeLabel1.TabIndex = 3;
            // 
            // GetGongDanListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 685);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetGongDanListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get GongDan List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetGongDanListForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanList)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.Button btnGatherData;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvGongDanList;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCheckQty;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ToolStripMenuItem tsmiExceptionFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheck1Filter;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheck2Filter;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheck3Filter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.Label lblDownload;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDelete;
        private System.Windows.Forms.DataGridViewButtonColumn dgvSplit;
        private System.Windows.Forms.DataGridViewLinkColumn dgvLink;
        private System.Windows.Forms.Button btnPCItem;
    }
}