namespace SHCustomsSystem
{
    partial class RMPurchaseAdjustmentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMPurchaseAdjustmentForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbBGDNo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbLotNo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbItemNo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvRMPurchase = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClickLink = new System.Windows.Forms.DataGridViewLinkColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBlankFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCustomsRcvdDate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVoucherID = new System.Windows.Forms.ToolStripMenuItem();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvRMAdjustment = new System.Windows.Forms.DataGridView();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAdjust = new System.Windows.Forms.Button();
            this.cmbAdjustment = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMPurchase)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMAdjustment)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.cmbBGDNo);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmbLotNo);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmbItemNo);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1232, 50);
            this.panel1.TabIndex = 2;
            // 
            // cmbBGDNo
            // 
            this.cmbBGDNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBGDNo.FormattingEnabled = true;
            this.cmbBGDNo.Location = new System.Drawing.Point(523, 11);
            this.cmbBGDNo.Name = "cmbBGDNo";
            this.cmbBGDNo.Size = new System.Drawing.Size(180, 25);
            this.cmbBGDNo.TabIndex = 18;
            this.cmbBGDNo.Enter += new System.EventHandler(this.cmbBGDNo_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label7.Location = new System.Drawing.Point(459, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 17);
            this.label7.TabIndex = 17;
            this.label7.Text = "BGD No";
            // 
            // cmbLotNo
            // 
            this.cmbLotNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLotNo.FormattingEnabled = true;
            this.cmbLotNo.Location = new System.Drawing.Point(272, 11);
            this.cmbLotNo.Name = "cmbLotNo";
            this.cmbLotNo.Size = new System.Drawing.Size(180, 25);
            this.cmbLotNo.TabIndex = 16;
            this.cmbLotNo.SelectedIndexChanged += new System.EventHandler(this.cmbLotNo_SelectedIndexChanged);
            this.cmbLotNo.Enter += new System.EventHandler(this.cmbLotNo_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label5.Location = new System.Drawing.Point(215, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "Lot No";
            // 
            // cmbItemNo
            // 
            this.cmbItemNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemNo.FormattingEnabled = true;
            this.cmbItemNo.Location = new System.Drawing.Point(87, 11);
            this.cmbItemNo.Name = "cmbItemNo";
            this.cmbItemNo.Size = new System.Drawing.Size(121, 25);
            this.cmbItemNo.TabIndex = 14;
            this.cmbItemNo.SelectedIndexChanged += new System.EventHandler(this.cmbItemNo_SelectedIndexChanged);
            this.cmbItemNo.Enter += new System.EventHandler(this.cmbItemNo_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label4.Location = new System.Drawing.Point(22, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Item No";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowse.Location = new System.Drawing.Point(1117, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(90, 33);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(989, 12);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 23);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            this.dtpTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpTo_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(958, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(710, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Created Date From";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(841, 12);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 23);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvRMPurchase);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(1, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1232, 472);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // dgvRMPurchase
            // 
            this.dgvRMPurchase.AllowUserToAddRows = false;
            this.dgvRMPurchase.AllowUserToDeleteRows = false;
            this.dgvRMPurchase.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRMPurchase.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvRMPurchase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRMPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRMPurchase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.ClickLink});
            this.dgvRMPurchase.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRMPurchase.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRMPurchase.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvRMPurchase.Location = new System.Drawing.Point(5, 22);
            this.dgvRMPurchase.Name = "dgvRMPurchase";
            this.dgvRMPurchase.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRMPurchase.RowTemplate.Height = 23;
            this.dgvRMPurchase.Size = new System.Drawing.Size(1222, 444);
            this.dgvRMPurchase.TabIndex = 0;
            this.dgvRMPurchase.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRMPurchase_CellClick);
            this.dgvRMPurchase.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRMPurchase_ColumnHeaderMouseClick);
            this.dgvRMPurchase.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvRMPurchase_MouseUp);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "全选";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 38;
            // 
            // ClickLink
            // 
            this.ClickLink.HeaderText = "";
            this.ClickLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ClickLink.Name = "ClickLink";
            this.ClickLink.Text = "Click";
            this.ClickLink.UseColumnTextForLinkValue = true;
            this.ClickLink.Width = 5;
            // 
            // cmsFilter
            // 
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter,
            this.tsmiBlankFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(144, 114);
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
            // tsmiBlankFilter
            // 
            this.tsmiBlankFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCustomsRcvdDate,
            this.tsmiVoucherID});
            this.tsmiBlankFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiBlankFilter.Image")));
            this.tsmiBlankFilter.Name = "tsmiBlankFilter";
            this.tsmiBlankFilter.Size = new System.Drawing.Size(143, 22);
            this.tsmiBlankFilter.Text = "Blank Filter";
            // 
            // tsmiCustomsRcvdDate
            // 
            this.tsmiCustomsRcvdDate.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCustomsRcvdDate.Image")));
            this.tsmiCustomsRcvdDate.Name = "tsmiCustomsRcvdDate";
            this.tsmiCustomsRcvdDate.Size = new System.Drawing.Size(189, 22);
            this.tsmiCustomsRcvdDate.Text = "Customs Rcvd Date";
            this.tsmiCustomsRcvdDate.Click += new System.EventHandler(this.tsmiCustomsRcvdDate_Click);
            // 
            // tsmiVoucherID
            // 
            this.tsmiVoucherID.Image = ((System.Drawing.Image)(resources.GetObject("tsmiVoucherID.Image")));
            this.tsmiVoucherID.Name = "tsmiVoucherID";
            this.tsmiVoucherID.Size = new System.Drawing.Size(189, 22);
            this.tsmiVoucherID.Text = "Voucher ID, Status etc";
            this.tsmiVoucherID.Click += new System.EventHandler(this.tsmiVoucherID_Click);
            // 
            // fakeLabel2
            // 
            this.fakeLabel2.BackColor = System.Drawing.SystemColors.Control;
            this.fakeLabel2.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.fakeLabel2.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel2.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel2.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel2.CImage = null;
            this.fakeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.fakeLabel2.LabelText = "";
            this.fakeLabel2.Location = new System.Drawing.Point(1, 51);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel2.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvRMAdjustment);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(1, 580);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1232, 103);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // dgvRMAdjustment
            // 
            this.dgvRMAdjustment.AllowUserToAddRows = false;
            this.dgvRMAdjustment.AllowUserToDeleteRows = false;
            this.dgvRMAdjustment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRMAdjustment.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvRMAdjustment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRMAdjustment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRMAdjustment.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRMAdjustment.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvRMAdjustment.Location = new System.Drawing.Point(5, 22);
            this.dgvRMAdjustment.Name = "dgvRMAdjustment";
            this.dgvRMAdjustment.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRMAdjustment.RowTemplate.Height = 23;
            this.dgvRMAdjustment.Size = new System.Drawing.Size(1222, 75);
            this.dgvRMAdjustment.TabIndex = 0;
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
            this.fakeLabel1.Location = new System.Drawing.Point(1, 577);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnAdjust);
            this.panel2.Controls.Add(this.cmbAdjustment);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this.btnUpload);
            this.panel2.Controls.Add(this.txtPath);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnDownload);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Location = new System.Drawing.Point(1, 526);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1232, 50);
            this.panel2.TabIndex = 12;
            // 
            // btnAdjust
            // 
            this.btnAdjust.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdjust.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnAdjust.Image = ((System.Drawing.Image)(resources.GetObject("btnAdjust.Image")));
            this.btnAdjust.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdjust.Location = new System.Drawing.Point(1111, 7);
            this.btnAdjust.Name = "btnAdjust";
            this.btnAdjust.Size = new System.Drawing.Size(90, 33);
            this.btnAdjust.TabIndex = 20;
            this.btnAdjust.Text = "Adjust";
            this.btnAdjust.UseVisualStyleBackColor = true;
            this.btnAdjust.Click += new System.EventHandler(this.btnAdjust_Click);
            // 
            // cmbAdjustment
            // 
            this.cmbAdjustment.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAdjustment.FormattingEnabled = true;
            this.cmbAdjustment.Items.AddRange(new object[] {
            "",
            "Edit-Other",
            "Edit-BGD",
            "Cancel"});
            this.cmbAdjustment.Location = new System.Drawing.Point(978, 11);
            this.cmbAdjustment.Name = "cmbAdjustment";
            this.cmbAdjustment.Size = new System.Drawing.Size(120, 25);
            this.cmbAdjustment.TabIndex = 19;
            this.cmbAdjustment.SelectedIndexChanged += new System.EventHandler(this.cmbAdjustment_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(894, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "Adjust Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(555, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(306, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "(Mass Modify : Customs Rcvd Date and so on 4 columns)";
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(144, -6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(2, 54);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpload.Location = new System.Drawing.Point(469, 7);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 33);
            this.btnUpload.TabIndex = 15;
            this.btnUpload.Text = " Upload";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.Location = new System.Drawing.Point(256, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(200, 23);
            this.txtPath.TabIndex = 14;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.Location = new System.Drawing.Point(164, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 33);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "  Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(26, 7);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 33);
            this.btnDownload.TabIndex = 12;
            this.btnDownload.Text = " Download";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(876, -6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(2, 54);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // RMPurchaseAdjustmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 685);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RMPurchaseAdjustmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RM Receiving Adjustment ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RMAdjustmentForm_FormClosing);
            this.Load += new System.EventHandler(this.RMAdjustmentForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMPurchase)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMAdjustment)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvRMPurchase;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvRMAdjustment;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cmbItemNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbLotNo;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.ComboBox cmbAdjustment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbBGDNo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewLinkColumn ClickLink;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiBlankFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomsRcvdDate;
        private System.Windows.Forms.ToolStripMenuItem tsmiVoucherID;
    }
}