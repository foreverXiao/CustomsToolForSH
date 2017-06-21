namespace SHCustomsSystem
{
    partial class GetDroolsDataDocForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetDroolsDataDocForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.dgvDroolsData = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvDroolsDoc = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCheckBalance = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnGatherDoc = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.txtBeiAnDanID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnGatherData = new System.Windows.Forms.Button();
            this.gBoxShow = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnDownload2 = new System.Windows.Forms.Button();
            this.btnDownload1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnUploadDownload = new System.Windows.Forms.Button();
            this.txtSearchPath = new System.Windows.Forms.TextBox();
            this.btnSearchPath = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDroolsData)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDroolsDoc)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gBoxShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnShow);
            this.groupBox1.Controls.Add(this.dgvDroolsData);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(1, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(720, 600);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnShow
            // 
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShow.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShow.Location = new System.Drawing.Point(3, 19);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(41, 23);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "*";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // dgvDroolsData
            // 
            this.dgvDroolsData.AllowUserToAddRows = false;
            this.dgvDroolsData.AllowUserToDeleteRows = false;
            this.dgvDroolsData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDroolsData.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvDroolsData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDroolsData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDroolsData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck});
            this.dgvDroolsData.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDroolsData.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvDroolsData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDroolsData.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvDroolsData.Location = new System.Drawing.Point(3, 19);
            this.dgvDroolsData.Name = "dgvDroolsData";
            this.dgvDroolsData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDroolsData.Size = new System.Drawing.Size(714, 578);
            this.dgvDroolsData.TabIndex = 1;
            this.dgvDroolsData.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDroolsData_CellMouseUp);
            this.dgvDroolsData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDroolsData_ColumnHeaderMouseClick);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvDroolsDoc);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(733, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(520, 600);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // dgvDroolsDoc
            // 
            this.dgvDroolsDoc.AllowUserToAddRows = false;
            this.dgvDroolsDoc.AllowUserToDeleteRows = false;
            this.dgvDroolsDoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDroolsDoc.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvDroolsDoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDroolsDoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDroolsDoc.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDroolsDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDroolsDoc.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvDroolsDoc.Location = new System.Drawing.Point(3, 19);
            this.dgvDroolsDoc.Name = "dgvDroolsDoc";
            this.dgvDroolsDoc.ReadOnly = true;
            this.dgvDroolsDoc.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDroolsDoc.Size = new System.Drawing.Size(514, 578);
            this.dgvDroolsDoc.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(185)))), ((int)(((byte)(100)))));
            this.groupBox3.Location = new System.Drawing.Point(722, 80);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(10, 606);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
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
            this.fakeLabel1.Location = new System.Drawing.Point(0, 79);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1252, 10);
            this.fakeLabel1.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panel1);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(1, -3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1251, 82);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnCheckBalance);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.btnGatherDoc);
            this.panel1.Controls.Add(this.btnPreview);
            this.panel1.Controls.Add(this.btnApprove);
            this.panel1.Controls.Add(this.txtBeiAnDanID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnGatherData);
            this.panel1.Location = new System.Drawing.Point(5, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1240, 62);
            this.panel1.TabIndex = 6;
            // 
            // btnCheckBalance
            // 
            this.btnCheckBalance.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckBalance.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckBalance.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCheckBalance.Location = new System.Drawing.Point(533, 9);
            this.btnCheckBalance.Name = "btnCheckBalance";
            this.btnCheckBalance.Size = new System.Drawing.Size(104, 40);
            this.btnCheckBalance.TabIndex = 18;
            this.btnCheckBalance.Text = "Check Balance";
            this.btnCheckBalance.UseVisualStyleBackColor = true;
            this.btnCheckBalance.Click += new System.EventHandler(this.btnCheckBalance_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.Location = new System.Drawing.Point(270, 9);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 40);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete Data";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(775, -7);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(2, 66);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(394, -7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(2, 66);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            // 
            // btnGatherDoc
            // 
            this.btnGatherDoc.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGatherDoc.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGatherDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnGatherDoc.Image")));
            this.btnGatherDoc.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGatherDoc.Location = new System.Drawing.Point(418, 9);
            this.btnGatherDoc.Name = "btnGatherDoc";
            this.btnGatherDoc.Size = new System.Drawing.Size(100, 40);
            this.btnGatherDoc.TabIndex = 8;
            this.btnGatherDoc.Text = " Gather Doc";
            this.btnGatherDoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGatherDoc.UseVisualStyleBackColor = true;
            this.btnGatherDoc.Click += new System.EventHandler(this.btnGatherDoc_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPreview.Location = new System.Drawing.Point(155, 9);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(100, 40);
            this.btnPreview.TabIndex = 7;
            this.btnPreview.Text = "    Preview";
            this.btnPreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApprove.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnApprove.Image = ((System.Drawing.Image)(resources.GetObject("btnApprove.Image")));
            this.btnApprove.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnApprove.Location = new System.Drawing.Point(1097, 9);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(100, 40);
            this.btnApprove.TabIndex = 6;
            this.btnApprove.Text = "   Approve";
            this.btnApprove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // txtBeiAnDanID
            // 
            this.txtBeiAnDanID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBeiAnDanID.ForeColor = System.Drawing.Color.Red;
            this.txtBeiAnDanID.Location = new System.Drawing.Point(932, 18);
            this.txtBeiAnDanID.Name = "txtBeiAnDanID";
            this.txtBeiAnDanID.Size = new System.Drawing.Size(150, 23);
            this.txtBeiAnDanID.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(796, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Drools BeiAnDan ID";
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(653, 9);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 40);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = " Download";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnGatherData
            // 
            this.btnGatherData.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGatherData.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGatherData.Image = ((System.Drawing.Image)(resources.GetObject("btnGatherData.Image")));
            this.btnGatherData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGatherData.Location = new System.Drawing.Point(40, 9);
            this.btnGatherData.Name = "btnGatherData";
            this.btnGatherData.Size = new System.Drawing.Size(100, 40);
            this.btnGatherData.TabIndex = 0;
            this.btnGatherData.Text = "Gather Data";
            this.btnGatherData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGatherData.UseVisualStyleBackColor = true;
            this.btnGatherData.Click += new System.EventHandler(this.btnGatherData_Click);
            // 
            // gBoxShow
            // 
            this.gBoxShow.Controls.Add(this.linkLabel1);
            this.gBoxShow.Controls.Add(this.btnUploadDownload);
            this.gBoxShow.Controls.Add(this.txtSearchPath);
            this.gBoxShow.Controls.Add(this.btnSearchPath);
            this.gBoxShow.Controls.Add(this.label7);
            this.gBoxShow.Controls.Add(this.groupBox7);
            this.gBoxShow.Controls.Add(this.btnDownload2);
            this.gBoxShow.Controls.Add(this.btnDownload1);
            this.gBoxShow.Controls.Add(this.label6);
            this.gBoxShow.Controls.Add(this.label5);
            this.gBoxShow.Controls.Add(this.dtpTo);
            this.gBoxShow.Controls.Add(this.dtpFrom);
            this.gBoxShow.Controls.Add(this.label4);
            this.gBoxShow.Controls.Add(this.label3);
            this.gBoxShow.Controls.Add(this.label2);
            this.gBoxShow.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gBoxShow.ForeColor = System.Drawing.Color.Navy;
            this.gBoxShow.Location = new System.Drawing.Point(330, 94);
            this.gBoxShow.Name = "gBoxShow";
            this.gBoxShow.Size = new System.Drawing.Size(600, 200);
            this.gBoxShow.TabIndex = 8;
            this.gBoxShow.TabStop = false;
            this.gBoxShow.Text = "Capture The Other Drools BeiAnDan Data";
            this.gBoxShow.Visible = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Location = new System.Drawing.Point(242, 26);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(2, 100);
            this.groupBox7.TabIndex = 18;
            this.groupBox7.TabStop = false;
            // 
            // btnDownload2
            // 
            this.btnDownload2.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload2.Image")));
            this.btnDownload2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload2.Location = new System.Drawing.Point(462, 87);
            this.btnDownload2.Name = "btnDownload2";
            this.btnDownload2.Size = new System.Drawing.Size(100, 30);
            this.btnDownload2.TabIndex = 17;
            this.btnDownload2.Text = " Download";
            this.btnDownload2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload2.UseVisualStyleBackColor = true;
            this.btnDownload2.Click += new System.EventHandler(this.btnDownload2_Click);
            // 
            // btnDownload1
            // 
            this.btnDownload1.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload1.Image")));
            this.btnDownload1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload1.Location = new System.Drawing.Point(462, 46);
            this.btnDownload1.Name = "btnDownload1";
            this.btnDownload1.Size = new System.Drawing.Size(100, 30);
            this.btnDownload1.TabIndex = 16;
            this.btnDownload1.Text = " Download";
            this.btnDownload1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload1.UseVisualStyleBackColor = true;
            this.btnDownload1.Click += new System.EventHandler(this.btnDownload1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label6.Location = new System.Drawing.Point(256, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(204, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "RMB IE Type (USD or RMB RM) : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label5.Location = new System.Drawing.Point(256, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Non-RMB IE Type && RMB RM :";
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(79, 92);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 23);
            this.dtpTo.TabIndex = 13;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            this.dtpTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpTo_KeyUp);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(79, 57);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 23);
            this.dtpFrom.TabIndex = 12;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Location = new System.Drawing.Point(35, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(35, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "From";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(27, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "BeiAnDan Customs Release Date :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(274, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "Manually insert Drools Data to adjust balance";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel1.LinkColor = System.Drawing.Color.Navy;
            this.linkLabel1.Location = new System.Drawing.Point(488, 162);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(87, 17);
            this.linkLabel1.TabIndex = 23;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Specification";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btnUploadDownload
            // 
            this.btnUploadDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUploadDownload.ForeColor = System.Drawing.Color.Navy;
            this.btnUploadDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnUploadDownload.Image")));
            this.btnUploadDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUploadDownload.Location = new System.Drawing.Point(394, 156);
            this.btnUploadDownload.Name = "btnUploadDownload";
            this.btnUploadDownload.Size = new System.Drawing.Size(80, 30);
            this.btnUploadDownload.TabIndex = 22;
            this.btnUploadDownload.Text = " Upload";
            this.btnUploadDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadDownload.UseVisualStyleBackColor = true;
            this.btnUploadDownload.Click += new System.EventHandler(this.btnUploadDownload_Click);
            // 
            // txtSearchPath
            // 
            this.txtSearchPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSearchPath.ForeColor = System.Drawing.Color.Red;
            this.txtSearchPath.Location = new System.Drawing.Point(113, 160);
            this.txtSearchPath.Name = "txtSearchPath";
            this.txtSearchPath.Size = new System.Drawing.Size(270, 23);
            this.txtSearchPath.TabIndex = 21;
            // 
            // btnSearchPath
            // 
            this.btnSearchPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearchPath.ForeColor = System.Drawing.Color.Navy;
            this.btnSearchPath.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchPath.Image")));
            this.btnSearchPath.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchPath.Location = new System.Drawing.Point(22, 156);
            this.btnSearchPath.Name = "btnSearchPath";
            this.btnSearchPath.Size = new System.Drawing.Size(80, 30);
            this.btnSearchPath.TabIndex = 20;
            this.btnSearchPath.Text = "  Search";
            this.btnSearchPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchPath.UseVisualStyleBackColor = true;
            this.btnSearchPath.Click += new System.EventHandler(this.btnSearchPath_Click);
            // 
            // GetDroolsDataDocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 685);
            this.Controls.Add(this.gBoxShow);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetDroolsDataDocForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get Drools Data & Document";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetDroolsDataDocForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDroolsData)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDroolsDoc)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gBoxShow.ResumeLayout(false);
            this.gBoxShow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvDroolsData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.DataGridView dgvDroolsDoc;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnGatherDoc;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.TextBox txtBeiAnDanID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnGatherData;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.Button btnCheckBalance;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox gBoxShow;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDownload2;
        private System.Windows.Forms.Button btnDownload1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btnUploadDownload;
        private System.Windows.Forms.TextBox txtSearchPath;
        private System.Windows.Forms.Button btnSearchPath;
    }
}