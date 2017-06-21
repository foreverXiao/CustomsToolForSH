namespace SHCustomsSystem
{
    partial class GetBeiAnDanDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetBeiAnDanDataForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnUpdateData = new System.Windows.Forms.Button();
            this.btnCheckAmount = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAdjustIEtype = new System.Windows.Forms.Button();
            this.btnRemoveData = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGatherData = new System.Windows.Forms.Button();
            this.cmbIEtype = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gBoxShow = new System.Windows.Forms.GroupBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnShowRMB = new System.Windows.Forms.Button();
            this.dgvBeiAnDan = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.gBoxShow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDan)).BeginInit();
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
            this.panel4.Controls.Add(this.btnTransfer);
            this.panel4.Controls.Add(this.btnDownload);
            this.panel4.Controls.Add(this.btnPreview);
            this.panel4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(1055, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(232, 76);
            this.panel4.TabIndex = 3;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Enabled = false;
            this.btnTransfer.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTransfer.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnTransfer.Image = ((System.Drawing.Image)(resources.GetObject("btnTransfer.Image")));
            this.btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTransfer.Location = new System.Drawing.Point(114, 37);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(100, 30);
            this.btnTransfer.TabIndex = 2;
            this.btnTransfer.Text = "   Transfer";
            this.btnTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Enabled = false;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(114, 6);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 30);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = " Download";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPreview.Location = new System.Drawing.Point(15, 6);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(90, 60);
            this.btnPreview.TabIndex = 0;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnUpdateData);
            this.panel3.Controls.Add(this.btnCheckAmount);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.Location = new System.Drawing.Point(585, 13);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(470, 76);
            this.panel3.TabIndex = 2;
            // 
            // btnUpdateData
            // 
            this.btnUpdateData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdateData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpdateData.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateData.Image")));
            this.btnUpdateData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdateData.Location = new System.Drawing.Point(342, 16);
            this.btnUpdateData.Name = "btnUpdateData";
            this.btnUpdateData.Size = new System.Drawing.Size(110, 40);
            this.btnUpdateData.TabIndex = 6;
            this.btnUpdateData.Text = " Update Data";
            this.btnUpdateData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateData.UseVisualStyleBackColor = true;
            this.btnUpdateData.Click += new System.EventHandler(this.btnUpdateData_Click);
            // 
            // btnCheckAmount
            // 
            this.btnCheckAmount.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckAmount.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckAmount.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckAmount.Image")));
            this.btnCheckAmount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCheckAmount.Location = new System.Drawing.Point(211, 16);
            this.btnCheckAmount.Name = "btnCheckAmount";
            this.btnCheckAmount.Size = new System.Drawing.Size(120, 40);
            this.btnCheckAmount.TabIndex = 5;
            this.btnCheckAmount.Text = "Check Amount";
            this.btnCheckAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckAmount.UseVisualStyleBackColor = true;
            this.btnCheckAmount.Click += new System.EventHandler(this.btnCheckAmount_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(12, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Amount";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Local Total RM Cost > Selling";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightPink;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(74, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 16);
            this.textBox1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnAdjustIEtype);
            this.panel2.Controls.Add(this.btnRemoveData);
            this.panel2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(325, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(260, 76);
            this.panel2.TabIndex = 1;
            // 
            // btnAdjustIEtype
            // 
            this.btnAdjustIEtype.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdjustIEtype.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnAdjustIEtype.Image = ((System.Drawing.Image)(resources.GetObject("btnAdjustIEtype.Image")));
            this.btnAdjustIEtype.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdjustIEtype.Location = new System.Drawing.Point(133, 16);
            this.btnAdjustIEtype.Name = "btnAdjustIEtype";
            this.btnAdjustIEtype.Size = new System.Drawing.Size(110, 40);
            this.btnAdjustIEtype.TabIndex = 1;
            this.btnAdjustIEtype.Text = "Adjust IEtype";
            this.btnAdjustIEtype.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdjustIEtype.UseVisualStyleBackColor = true;
            this.btnAdjustIEtype.Click += new System.EventHandler(this.btnAdjustIEtype_Click);
            // 
            // btnRemoveData
            // 
            this.btnRemoveData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRemoveData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnRemoveData.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveData.Image")));
            this.btnRemoveData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRemoveData.Location = new System.Drawing.Point(14, 16);
            this.btnRemoveData.Name = "btnRemoveData";
            this.btnRemoveData.Size = new System.Drawing.Size(110, 40);
            this.btnRemoveData.TabIndex = 0;
            this.btnRemoveData.Text = "Remove Data";
            this.btnRemoveData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveData.UseVisualStyleBackColor = true;
            this.btnRemoveData.Click += new System.EventHandler(this.btnRemoveData_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnGatherData);
            this.panel1.Controls.Add(this.cmbIEtype);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(5, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 76);
            this.panel1.TabIndex = 0;
            // 
            // btnGatherData
            // 
            this.btnGatherData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGatherData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGatherData.Image = ((System.Drawing.Image)(resources.GetObject("btnGatherData.Image")));
            this.btnGatherData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGatherData.Location = new System.Drawing.Point(202, 16);
            this.btnGatherData.Name = "btnGatherData";
            this.btnGatherData.Size = new System.Drawing.Size(100, 40);
            this.btnGatherData.TabIndex = 2;
            this.btnGatherData.Text = "Gather Data";
            this.btnGatherData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGatherData.UseVisualStyleBackColor = true;
            this.btnGatherData.Click += new System.EventHandler(this.btnGatherData_Click);
            // 
            // cmbIEtype
            // 
            this.cmbIEtype.FormattingEnabled = true;
            this.cmbIEtype.Location = new System.Drawing.Point(68, 24);
            this.cmbIEtype.Name = "cmbIEtype";
            this.cmbIEtype.Size = new System.Drawing.Size(121, 25);
            this.cmbIEtype.TabIndex = 1;
            this.cmbIEtype.SelectedIndexChanged += new System.EventHandler(this.cmbIEtype_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IE Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gBoxShow);
            this.groupBox2.Controls.Add(this.btnShowRMB);
            this.groupBox2.Controls.Add(this.dgvBeiAnDan);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(2, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1290, 590);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // gBoxShow
            // 
            this.gBoxShow.Controls.Add(this.btnUpload);
            this.gBoxShow.Controls.Add(this.txtPath);
            this.gBoxShow.Controls.Add(this.btnSearch);
            this.gBoxShow.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gBoxShow.ForeColor = System.Drawing.Color.Navy;
            this.gBoxShow.Location = new System.Drawing.Point(375, 12);
            this.gBoxShow.Name = "gBoxShow";
            this.gBoxShow.Size = new System.Drawing.Size(540, 100);
            this.gBoxShow.TabIndex = 5;
            this.gBoxShow.TabStop = false;
            this.gBoxShow.Text = "Upload \'EXPORT PENDING\' data to mass update";
            this.gBoxShow.UseCompatibleTextRendering = true;
            this.gBoxShow.Visible = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpload.Location = new System.Drawing.Point(429, 36);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(90, 30);
            this.btnUpload.TabIndex = 32;
            this.btnUpload.Text = "  Upload";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.ForeColor = System.Drawing.Color.Red;
            this.txtPath.Location = new System.Drawing.Point(115, 40);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(300, 23);
            this.txtPath.TabIndex = 31;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.Location = new System.Drawing.Point(17, 36);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 30);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.Text = "   Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnShowRMB
            // 
            this.btnShowRMB.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowRMB.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowRMB.Location = new System.Drawing.Point(4, 21);
            this.btnShowRMB.Name = "btnShowRMB";
            this.btnShowRMB.Size = new System.Drawing.Size(41, 23);
            this.btnShowRMB.TabIndex = 2;
            this.btnShowRMB.Text = "*";
            this.btnShowRMB.UseVisualStyleBackColor = true;
            this.btnShowRMB.Visible = false;
            this.btnShowRMB.Click += new System.EventHandler(this.btnShowRMB_Click);
            // 
            // dgvBeiAnDan
            // 
            this.dgvBeiAnDan.AllowUserToAddRows = false;
            this.dgvBeiAnDan.AllowUserToDeleteRows = false;
            this.dgvBeiAnDan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBeiAnDan.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvBeiAnDan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBeiAnDan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvBeiAnDan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck});
            this.dgvBeiAnDan.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBeiAnDan.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBeiAnDan.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvBeiAnDan.Location = new System.Drawing.Point(4, 21);
            this.dgvBeiAnDan.Name = "dgvBeiAnDan";
            this.dgvBeiAnDan.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBeiAnDan.Size = new System.Drawing.Size(1282, 564);
            this.dgvBeiAnDan.TabIndex = 0;
            this.dgvBeiAnDan.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBeiAnDan_CellMouseClick);
            this.dgvBeiAnDan.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBeiAnDan_ColumnHeaderMouseClick);
            this.dgvBeiAnDan.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvBeiAnDan_MouseUp);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "全选";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 38;
            // 
            // cmsFilter
            // 
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(144, 92);
            // 
            // tsmiChooseFilter
            // 
            this.tsmiChooseFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiChooseFilter.Image")));
            this.tsmiChooseFilter.Name = "tsmiChooseFilter";
            this.tsmiChooseFilter.Size = new System.Drawing.Size(143, 22);
            this.tsmiChooseFilter.Text = "Choose Filter";
            this.tsmiChooseFilter.Click += new System.EventHandler(this.tsmiChooseFilter_Click);
            // 
            // tsmiExcludeFilter
            // 
            this.tsmiExcludeFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExcludeFilter.Image")));
            this.tsmiExcludeFilter.Name = "tsmiExcludeFilter";
            this.tsmiExcludeFilter.Size = new System.Drawing.Size(143, 22);
            this.tsmiExcludeFilter.Text = "Exclude Filter";
            this.tsmiExcludeFilter.Click += new System.EventHandler(this.tsmiExcludeFilter_Click);
            // 
            // tsmiRefreshFilter
            // 
            this.tsmiRefreshFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRefreshFilter.Image")));
            this.tsmiRefreshFilter.Name = "tsmiRefreshFilter";
            this.tsmiRefreshFilter.Size = new System.Drawing.Size(143, 22);
            this.tsmiRefreshFilter.Text = "Refresh Filter";
            this.tsmiRefreshFilter.Click += new System.EventHandler(this.tsmiRefreshFilter_Click);
            // 
            // tsmiRecordFilter
            // 
            this.tsmiRecordFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRecordFilter.Image")));
            this.tsmiRecordFilter.Name = "tsmiRecordFilter";
            this.tsmiRecordFilter.Size = new System.Drawing.Size(143, 22);
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
            this.fakeLabel1.Location = new System.Drawing.Point(1, 90);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1292, 11);
            this.fakeLabel1.TabIndex = 4;
            // 
            // GetBeiAnDanDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 685);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetBeiAnDanDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get BeiAnDan Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetBeiAnDanDataForm_FormClosing);
            this.Load += new System.EventHandler(this.GetBeiAnDanDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gBoxShow.ResumeLayout(false);
            this.gBoxShow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDan)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.DataGridView dgvBeiAnDan;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbIEtype;
        private System.Windows.Forms.Button btnGatherData;
        private System.Windows.Forms.Button btnRemoveData;
        private System.Windows.Forms.Button btnAdjustIEtype;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCheckAmount;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnUpdateData;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnShowRMB;
        private System.Windows.Forms.GroupBox gBoxShow;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnUpload;
    }
}