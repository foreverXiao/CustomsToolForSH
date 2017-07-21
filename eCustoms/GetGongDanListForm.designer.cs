namespace eCustoms
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
            this.gBox = new System.Windows.Forms.GroupBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.gBoxShow = new System.Windows.Forms.GroupBox();
            this.btnDownloadDoc = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbtn3 = new System.Windows.Forms.RadioButton();
            this.rbtn2 = new System.Windows.Forms.RadioButton();
            this.rbtn1 = new System.Windows.Forms.RadioButton();
            this.dgvGongDanList = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvSplit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExceptionFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiQtyOverFlow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPriceZero = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCheckData = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDownloadData = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnGenerateList = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.llblMessage = new System.Windows.Forms.LinkLabel();
            this.btnExtractData = new System.Windows.Forms.Button();
            this.ckLE = new System.Windows.Forms.CheckBox();
            this.ckSD = new System.Windows.Forms.CheckBox();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSearchFile = new System.Windows.Forms.Button();
            this.gBox.SuspendLayout();
            this.gBoxShow.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanList)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gBox
            // 
            this.gBox.Controls.Add(this.btnShow);
            this.gBox.Controls.Add(this.gBoxShow);
            this.gBox.Controls.Add(this.dgvGongDanList);
            this.gBox.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gBox.ForeColor = System.Drawing.Color.Navy;
            this.gBox.Location = new System.Drawing.Point(2, 81);
            this.gBox.Name = "gBox";
            this.gBox.Size = new System.Drawing.Size(1231, 583);
            this.gBox.TabIndex = 21;
            this.gBox.TabStop = false;
            // 
            // btnShow
            // 
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShow.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShow.Location = new System.Drawing.Point(3, 19);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(41, 23);
            this.btnShow.TabIndex = 37;
            this.btnShow.Text = "*";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // gBoxShow
            // 
            this.gBoxShow.BackColor = System.Drawing.SystemColors.Control;
            this.gBoxShow.Controls.Add(this.btnDownloadDoc);
            this.gBoxShow.Controls.Add(this.label4);
            this.gBoxShow.Controls.Add(this.label5);
            this.gBoxShow.Controls.Add(this.label6);
            this.gBoxShow.Controls.Add(this.groupBox3);
            this.gBoxShow.ForeColor = System.Drawing.Color.Navy;
            this.gBoxShow.Location = new System.Drawing.Point(266, 20);
            this.gBoxShow.Name = "gBoxShow";
            this.gBoxShow.Size = new System.Drawing.Size(690, 72);
            this.gBoxShow.TabIndex = 36;
            this.gBoxShow.TabStop = false;
            this.gBoxShow.Text = "Query Order Fulfillment Data Status";
            this.gBoxShow.Visible = false;
            // 
            // btnDownloadDoc
            // 
            this.btnDownloadDoc.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownloadDoc.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownloadDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnDownloadDoc.Image")));
            this.btnDownloadDoc.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownloadDoc.Location = new System.Drawing.Point(575, 19);
            this.btnDownloadDoc.Name = "btnDownloadDoc";
            this.btnDownloadDoc.Size = new System.Drawing.Size(100, 40);
            this.btnDownloadDoc.TabIndex = 29;
            this.btnDownloadDoc.Text = " Download";
            this.btnDownloadDoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownloadDoc.UseVisualStyleBackColor = true;
            this.btnDownloadDoc.Click += new System.EventHandler(this.btnDownloadDoc_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(376, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 16);
            this.label4.TabIndex = 28;
            this.label4.Text = "(default interval: 90 days from now)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(376, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 16);
            this.label5.TabIndex = 27;
            this.label5.Text = "(as per \'Pass to IE Date\' to sift)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label6.Location = new System.Drawing.Point(11, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 17);
            this.label6.TabIndex = 26;
            this.label6.Text = "Max Interval:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbtn3);
            this.groupBox3.Controls.Add(this.rbtn2);
            this.groupBox3.Controls.Add(this.rbtn1);
            this.groupBox3.Location = new System.Drawing.Point(103, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(270, 46);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            // 
            // rbtn3
            // 
            this.rbtn3.AutoSize = true;
            this.rbtn3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.rbtn3.Location = new System.Drawing.Point(181, 16);
            this.rbtn3.Name = "rbtn3";
            this.rbtn3.Size = new System.Drawing.Size(79, 21);
            this.rbtn3.TabIndex = 3;
            this.rbtn3.TabStop = true;
            this.rbtn3.Text = "720 days";
            this.rbtn3.UseVisualStyleBackColor = true;
            // 
            // rbtn2
            // 
            this.rbtn2.AutoSize = true;
            this.rbtn2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.rbtn2.Location = new System.Drawing.Point(97, 16);
            this.rbtn2.Name = "rbtn2";
            this.rbtn2.Size = new System.Drawing.Size(79, 21);
            this.rbtn2.TabIndex = 2;
            this.rbtn2.TabStop = true;
            this.rbtn2.Text = "360 days";
            this.rbtn2.UseVisualStyleBackColor = true;
            // 
            // rbtn1
            // 
            this.rbtn1.AutoSize = true;
            this.rbtn1.Checked = true;
            this.rbtn1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.rbtn1.Location = new System.Drawing.Point(13, 16);
            this.rbtn1.Name = "rbtn1";
            this.rbtn1.Size = new System.Drawing.Size(79, 21);
            this.rbtn1.TabIndex = 1;
            this.rbtn1.TabStop = true;
            this.rbtn1.Text = "180 days";
            this.rbtn1.UseVisualStyleBackColor = true;
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
            this.dgvSplit});
            this.dgvGongDanList.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGongDanList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGongDanList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGongDanList.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvGongDanList.Location = new System.Drawing.Point(3, 19);
            this.dgvGongDanList.Name = "dgvGongDanList";
            this.dgvGongDanList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGongDanList.RowTemplate.Height = 23;
            this.dgvGongDanList.Size = new System.Drawing.Size(1225, 561);
            this.dgvGongDanList.TabIndex = 22;
            this.dgvGongDanList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGongDanList_CellClick);
            this.dgvGongDanList.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanList_CellMouseUp);
            this.dgvGongDanList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanList_ColumnHeaderMouseClick);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "Select";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 48;
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
            this.tsmiQtyOverFlow,
            this.tsmiPriceZero});
            this.tsmiExceptionFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExceptionFilter.Image")));
            this.tsmiExceptionFilter.Name = "tsmiExceptionFilter";
            this.tsmiExceptionFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiExceptionFilter.Text = "Exception Filter";
            // 
            // tsmiQtyOverFlow
            // 
            this.tsmiQtyOverFlow.Image = ((System.Drawing.Image)(resources.GetObject("tsmiQtyOverFlow.Image")));
            this.tsmiQtyOverFlow.Name = "tsmiQtyOverFlow";
            this.tsmiQtyOverFlow.Size = new System.Drawing.Size(152, 22);
            this.tsmiQtyOverFlow.Text = "Qty Overflow";
            this.tsmiQtyOverFlow.Click += new System.EventHandler(this.tsmiQtyOverFlow_Click);
            // 
            // tsmiPriceZero
            // 
            this.tsmiPriceZero.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPriceZero.Image")));
            this.tsmiPriceZero.Name = "tsmiPriceZero";
            this.tsmiPriceZero.Size = new System.Drawing.Size(152, 22);
            this.tsmiPriceZero.Text = "Unit Price Zero";
            this.tsmiPriceZero.Click += new System.EventHandler(this.tsmiPriceZero_Click);
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
            this.fakeLabel1.Location = new System.Drawing.Point(1, 78);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel1.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.btnCheckData);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(589, 1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(276, 76);
            this.panel4.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.label7.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label7.Location = new System.Drawing.Point(0, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 16);
            this.label7.TabIndex = 13;
            this.label7.Text = "2.Unit Price not equal to zero";
            // 
            // btnCheckData
            // 
            this.btnCheckData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckData.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckData.Image")));
            this.btnCheckData.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCheckData.Location = new System.Drawing.Point(162, 6);
            this.btnCheckData.Name = "btnCheckData";
            this.btnCheckData.Size = new System.Drawing.Size(100, 60);
            this.btnCheckData.TabIndex = 12;
            this.btnCheckData.Text = "Check Data";
            this.btnCheckData.UseVisualStyleBackColor = true;
            this.btnCheckData.Click += new System.EventHandler(this.btnCheckData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F);
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(10, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "total output Qty in BOM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F);
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(10, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "should be not greater than";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(2, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "1.GongDan Qty for shipping";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnDownloadData);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.btnPreview);
            this.panel3.Controls.Add(this.btnGenerateList);
            this.panel3.Controls.Add(this.groupBox5);
            this.panel3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.Location = new System.Drawing.Point(865, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(368, 76);
            this.panel3.TabIndex = 19;
            // 
            // btnDownloadData
            // 
            this.btnDownloadData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownloadData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownloadData.Image = ((System.Drawing.Image)(resources.GetObject("btnDownloadData.Image")));
            this.btnDownloadData.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDownloadData.Location = new System.Drawing.Point(255, 6);
            this.btnDownloadData.Name = "btnDownloadData";
            this.btnDownloadData.Size = new System.Drawing.Size(100, 60);
            this.btnDownloadData.TabIndex = 16;
            this.btnDownloadData.Text = "Dwonload";
            this.btnDownloadData.UseVisualStyleBackColor = true;
            this.btnDownloadData.Click += new System.EventHandler(this.btnDownloadData_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(244, -8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(2, 82);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnPreview.Location = new System.Drawing.Point(134, 6);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(100, 60);
            this.btnPreview.TabIndex = 15;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnGenerateList
            // 
            this.btnGenerateList.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGenerateList.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGenerateList.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateList.Image")));
            this.btnGenerateList.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnGenerateList.Location = new System.Drawing.Point(11, 6);
            this.btnGenerateList.Name = "btnGenerateList";
            this.btnGenerateList.Size = new System.Drawing.Size(100, 60);
            this.btnGenerateList.TabIndex = 14;
            this.btnGenerateList.Text = "  Generate    List";
            this.btnGenerateList.UseVisualStyleBackColor = true;
            this.btnGenerateList.Click += new System.EventHandler(this.btnGenerateList_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(122, -8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(2, 82);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.llblMessage);
            this.panel2.Controls.Add(this.btnExtractData);
            this.panel2.Controls.Add(this.ckLE);
            this.panel2.Controls.Add(this.ckSD);
            this.panel2.Controls.Add(this.btnUploadFile);
            this.panel2.Controls.Add(this.txtPath);
            this.panel2.Controls.Add(this.btnSearchFile);
            this.panel2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(588, 76);
            this.panel2.TabIndex = 8;
            // 
            // llblMessage
            // 
            this.llblMessage.AutoSize = true;
            this.llblMessage.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblMessage.ForeColor = System.Drawing.Color.Navy;
            this.llblMessage.LinkColor = System.Drawing.Color.Navy;
            this.llblMessage.Location = new System.Drawing.Point(372, 11);
            this.llblMessage.Name = "llblMessage";
            this.llblMessage.Size = new System.Drawing.Size(87, 17);
            this.llblMessage.TabIndex = 3;
            this.llblMessage.TabStop = true;
            this.llblMessage.Text = "Specification";
            this.llblMessage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblMessage_LinkClicked);
            // 
            // btnExtractData
            // 
            this.btnExtractData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExtractData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnExtractData.Image = ((System.Drawing.Image)(resources.GetObject("btnExtractData.Image")));
            this.btnExtractData.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnExtractData.Location = new System.Drawing.Point(474, 6);
            this.btnExtractData.Name = "btnExtractData";
            this.btnExtractData.Size = new System.Drawing.Size(100, 60);
            this.btnExtractData.TabIndex = 7;
            this.btnExtractData.Text = "Extract Data";
            this.btnExtractData.UseVisualStyleBackColor = true;
            this.btnExtractData.Click += new System.EventHandler(this.btnExtractData_Click);
            // 
            // ckLE
            // 
            this.ckLE.AutoSize = true;
            this.ckLE.Font = new System.Drawing.Font("Microsoft YaHei", 8.5F, System.Drawing.FontStyle.Bold);
            this.ckLE.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.ckLE.Location = new System.Drawing.Point(167, 11);
            this.ckLE.Name = "ckLE";
            this.ckLE.Size = new System.Drawing.Size(190, 21);
            this.ckLE.TabIndex = 2;
            this.ckLE.Text = "Outbound Delivery Report";
            this.ckLE.UseVisualStyleBackColor = true;
            this.ckLE.CheckedChanged += new System.EventHandler(this.ckLE_CheckedChanged);
            // 
            // ckSD
            // 
            this.ckSD.AutoSize = true;
            this.ckSD.Font = new System.Drawing.Font("Microsoft YaHei", 8.5F, System.Drawing.FontStyle.Bold);
            this.ckSD.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.ckSD.Location = new System.Drawing.Point(13, 11);
            this.ckSD.Name = "ckSD";
            this.ckSD.Size = new System.Drawing.Size(142, 21);
            this.ckSD.TabIndex = 1;
            this.ckSD.Text = "Sales Order Report";
            this.ckSD.UseVisualStyleBackColor = true;
            this.ckSD.CheckedChanged += new System.EventHandler(this.ckSD_CheckedChanged);
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUploadFile.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUploadFile.Image = ((System.Drawing.Image)(resources.GetObject("btnUploadFile.Image")));
            this.btnUploadFile.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUploadFile.Location = new System.Drawing.Point(366, 33);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(100, 33);
            this.btnUploadFile.TabIndex = 6;
            this.btnUploadFile.Text = "UploadFile   ";
            this.btnUploadFile.UseVisualStyleBackColor = true;
            this.btnUploadFile.Click += new System.EventHandler(this.btnUploadFile_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtPath.Location = new System.Drawing.Point(120, 39);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(240, 22);
            this.txtPath.TabIndex = 5;
            // 
            // btnSearchFile
            // 
            this.btnSearchFile.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearchFile.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearchFile.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchFile.Image")));
            this.btnSearchFile.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchFile.Location = new System.Drawing.Point(13, 33);
            this.btnSearchFile.Name = "btnSearchFile";
            this.btnSearchFile.Size = new System.Drawing.Size(100, 33);
            this.btnSearchFile.TabIndex = 4;
            this.btnSearchFile.Text = "SearchFile   ";
            this.btnSearchFile.UseVisualStyleBackColor = true;
            this.btnSearchFile.Click += new System.EventHandler(this.btnSearchFile_Click);
            // 
            // GetGongDanListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 665);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.gBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetGongDanListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get GongDan List Operation Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetGongDanListForm_FormClosing);
            this.gBox.ResumeLayout(false);
            this.gBoxShow.ResumeLayout(false);
            this.gBoxShow.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanList)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gBox;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.DataGridView dgvGongDanList;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearchFile;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.CheckBox ckSD;
        private System.Windows.Forms.CheckBox ckLE;
        private System.Windows.Forms.Button btnExtractData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel llblMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCheckData;
        private System.Windows.Forms.Button btnGenerateList;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDownloadData;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDelete;
        private System.Windows.Forms.DataGridViewButtonColumn dgvSplit;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExceptionFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiQtyOverFlow;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.GroupBox gBoxShow;
        private System.Windows.Forms.Button btnDownloadDoc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbtn3;
        private System.Windows.Forms.RadioButton rbtn2;
        private System.Windows.Forms.RadioButton rbtn1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem tsmiPriceZero;
    }
}