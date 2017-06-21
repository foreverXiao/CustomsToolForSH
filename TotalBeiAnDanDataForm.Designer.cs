namespace SHCustomsSystem
{
    partial class TotalBeiAnDanDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalBeiAnDanDataForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBoxEdit = new System.Windows.Forms.GroupBox();
            this.dtpCustomsReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpTaxPaidDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBeiAnDanNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.groupBoxDate = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxIEID = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBeiAnDanID = new System.Windows.Forms.ComboBox();
            this.cmbIEtype = new System.Windows.Forms.ComboBox();
            this.groupBoxSift = new System.Windows.Forms.GroupBox();
            this.cbCustomsReleaseDate = new System.Windows.Forms.CheckBox();
            this.cbTaxPaidDate = new System.Windows.Forms.CheckBox();
            this.cbBeiAnDanNo = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvBeiAnDanM = new System.Windows.Forms.DataGridView();
            this.dgvUpdate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvBeiAnDanD = new System.Windows.Forms.DataGridView();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBoxEdit.SuspendLayout();
            this.groupBoxDate.SuspendLayout();
            this.groupBoxIEID.SuspendLayout();
            this.groupBoxSift.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDanM)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDanD)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(2, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1290, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Controls.Add(this.groupBoxEdit);
            this.panel1.Controls.Add(this.btnPreview);
            this.panel1.Controls.Add(this.groupBoxDate);
            this.panel1.Controls.Add(this.groupBoxIEID);
            this.panel1.Controls.Add(this.groupBoxSift);
            this.panel1.Location = new System.Drawing.Point(3, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1284, 104);
            this.panel1.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDelete.Location = new System.Drawing.Point(1192, 20);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 60);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Delete  Data";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnUpdate.Location = new System.Drawing.Point(1106, 20);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 60);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update  Data";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // groupBoxEdit
            // 
            this.groupBoxEdit.Controls.Add(this.dtpCustomsReleaseDate);
            this.groupBoxEdit.Controls.Add(this.label7);
            this.groupBoxEdit.Controls.Add(this.dtpTaxPaidDate);
            this.groupBoxEdit.Controls.Add(this.label6);
            this.groupBoxEdit.Controls.Add(this.txtBeiAnDanNo);
            this.groupBoxEdit.Controls.Add(this.label5);
            this.groupBoxEdit.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxEdit.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBoxEdit.Location = new System.Drawing.Point(789, -5);
            this.groupBoxEdit.Name = "groupBoxEdit";
            this.groupBoxEdit.Size = new System.Drawing.Size(310, 100);
            this.groupBoxEdit.TabIndex = 12;
            this.groupBoxEdit.TabStop = false;
            // 
            // dtpCustomsReleaseDate
            // 
            this.dtpCustomsReleaseDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpCustomsReleaseDate.CustomFormat = "";
            this.dtpCustomsReleaseDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpCustomsReleaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCustomsReleaseDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpCustomsReleaseDate.Location = new System.Drawing.Point(159, 70);
            this.dtpCustomsReleaseDate.Name = "dtpCustomsReleaseDate";
            this.dtpCustomsReleaseDate.Size = new System.Drawing.Size(140, 23);
            this.dtpCustomsReleaseDate.TabIndex = 5;
            this.dtpCustomsReleaseDate.ValueChanged += new System.EventHandler(this.dtpCustomsReleaseDate_ValueChanged);
            this.dtpCustomsReleaseDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpCustomsReleaseDate_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Customs Release Date";
            // 
            // dtpTaxPaidDate
            // 
            this.dtpTaxPaidDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTaxPaidDate.Checked = false;
            this.dtpTaxPaidDate.CustomFormat = "";
            this.dtpTaxPaidDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTaxPaidDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTaxPaidDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpTaxPaidDate.Location = new System.Drawing.Point(159, 43);
            this.dtpTaxPaidDate.Name = "dtpTaxPaidDate";
            this.dtpTaxPaidDate.Size = new System.Drawing.Size(140, 23);
            this.dtpTaxPaidDate.TabIndex = 3;
            this.dtpTaxPaidDate.ValueChanged += new System.EventHandler(this.dtpTaxPaidDate_ValueChanged);
            this.dtpTaxPaidDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpTaxPaidDate_KeyUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Tax && Duty Paid Date";
            // 
            // txtBeiAnDanNo
            // 
            this.txtBeiAnDanNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBeiAnDanNo.Location = new System.Drawing.Point(109, 15);
            this.txtBeiAnDanNo.Name = "txtBeiAnDanNo";
            this.txtBeiAnDanNo.Size = new System.Drawing.Size(190, 23);
            this.txtBeiAnDanNo.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "BeiAnDan No";
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnPreview.Location = new System.Drawing.Point(702, 20);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(80, 60);
            this.btnPreview.TabIndex = 11;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // groupBoxDate
            // 
            this.groupBoxDate.Controls.Add(this.dtpTo);
            this.groupBoxDate.Controls.Add(this.dtpFrom);
            this.groupBoxDate.Controls.Add(this.label4);
            this.groupBoxDate.Controls.Add(this.label3);
            this.groupBoxDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxDate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBoxDate.Location = new System.Drawing.Point(514, 4);
            this.groupBoxDate.Name = "groupBoxDate";
            this.groupBoxDate.Size = new System.Drawing.Size(180, 90);
            this.groupBoxDate.TabIndex = 10;
            this.groupBoxDate.TabStop = false;
            this.groupBoxDate.Text = "BeiAnDan Date";
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(60, 54);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 23);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            this.dtpTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpTo_KeyUp);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.CustomFormat = "";
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpFrom.Location = new System.Drawing.Point(60, 22);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 23);
            this.dtpFrom.TabIndex = 2;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "From";
            // 
            // groupBoxIEID
            // 
            this.groupBoxIEID.Controls.Add(this.label2);
            this.groupBoxIEID.Controls.Add(this.label1);
            this.groupBoxIEID.Controls.Add(this.cmbBeiAnDanID);
            this.groupBoxIEID.Controls.Add(this.cmbIEtype);
            this.groupBoxIEID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxIEID.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBoxIEID.Location = new System.Drawing.Point(206, 4);
            this.groupBoxIEID.Name = "groupBoxIEID";
            this.groupBoxIEID.Size = new System.Drawing.Size(300, 90);
            this.groupBoxIEID.TabIndex = 9;
            this.groupBoxIEID.TabStop = false;
            this.groupBoxIEID.Text = "IE Type && BeiAnDan ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(15, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "BeiAnDan ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "IE Type";
            // 
            // cmbBeiAnDanID
            // 
            this.cmbBeiAnDanID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbBeiAnDanID.FormattingEnabled = true;
            this.cmbBeiAnDanID.Location = new System.Drawing.Point(105, 54);
            this.cmbBeiAnDanID.Name = "cmbBeiAnDanID";
            this.cmbBeiAnDanID.Size = new System.Drawing.Size(182, 25);
            this.cmbBeiAnDanID.TabIndex = 1;
            this.cmbBeiAnDanID.SelectedIndexChanged += new System.EventHandler(this.cmbBeiAnDanID_SelectedIndexChanged);
            this.cmbBeiAnDanID.Enter += new System.EventHandler(this.cmbBeiAnDanID_Enter);
            // 
            // cmbIEtype
            // 
            this.cmbIEtype.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbIEtype.FormattingEnabled = true;
            this.cmbIEtype.Location = new System.Drawing.Point(105, 22);
            this.cmbIEtype.Name = "cmbIEtype";
            this.cmbIEtype.Size = new System.Drawing.Size(111, 25);
            this.cmbIEtype.TabIndex = 0;
            this.cmbIEtype.SelectedIndexChanged += new System.EventHandler(this.cmbIEtype_SelectedIndexChanged);
            // 
            // groupBoxSift
            // 
            this.groupBoxSift.Controls.Add(this.cbCustomsReleaseDate);
            this.groupBoxSift.Controls.Add(this.cbTaxPaidDate);
            this.groupBoxSift.Controls.Add(this.cbBeiAnDanNo);
            this.groupBoxSift.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxSift.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBoxSift.Location = new System.Drawing.Point(8, 4);
            this.groupBoxSift.Name = "groupBoxSift";
            this.groupBoxSift.Size = new System.Drawing.Size(190, 90);
            this.groupBoxSift.TabIndex = 8;
            this.groupBoxSift.TabStop = false;
            this.groupBoxSift.Text = "No-generated Data to Sift";
            // 
            // cbCustomsReleaseDate
            // 
            this.cbCustomsReleaseDate.AutoSize = true;
            this.cbCustomsReleaseDate.Location = new System.Drawing.Point(16, 63);
            this.cbCustomsReleaseDate.Name = "cbCustomsReleaseDate";
            this.cbCustomsReleaseDate.Size = new System.Drawing.Size(163, 21);
            this.cbCustomsReleaseDate.TabIndex = 2;
            this.cbCustomsReleaseDate.Text = "Customs Release Date";
            this.cbCustomsReleaseDate.UseVisualStyleBackColor = true;
            this.cbCustomsReleaseDate.CheckedChanged += new System.EventHandler(this.cbCustomsReleaseDate_CheckedChanged);
            // 
            // cbTaxPaidDate
            // 
            this.cbTaxPaidDate.AutoSize = true;
            this.cbTaxPaidDate.Location = new System.Drawing.Point(16, 41);
            this.cbTaxPaidDate.Name = "cbTaxPaidDate";
            this.cbTaxPaidDate.Size = new System.Drawing.Size(162, 21);
            this.cbTaxPaidDate.TabIndex = 1;
            this.cbTaxPaidDate.Text = "Tax && Duty Paid Date";
            this.cbTaxPaidDate.UseVisualStyleBackColor = true;
            this.cbTaxPaidDate.CheckedChanged += new System.EventHandler(this.cbTaxPaidDate_CheckedChanged);
            // 
            // cbBeiAnDanNo
            // 
            this.cbBeiAnDanNo.AutoSize = true;
            this.cbBeiAnDanNo.Location = new System.Drawing.Point(16, 19);
            this.cbBeiAnDanNo.Name = "cbBeiAnDanNo";
            this.cbBeiAnDanNo.Size = new System.Drawing.Size(110, 21);
            this.cbBeiAnDanNo.TabIndex = 0;
            this.cbBeiAnDanNo.Text = "BeiAnDan No";
            this.cbBeiAnDanNo.UseVisualStyleBackColor = true;
            this.cbBeiAnDanNo.CheckedChanged += new System.EventHandler(this.cbBeiAnDanNo_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvBeiAnDanM);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(2, 124);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(639, 560);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Master";
            // 
            // dgvBeiAnDanM
            // 
            this.dgvBeiAnDanM.AllowUserToAddRows = false;
            this.dgvBeiAnDanM.AllowUserToDeleteRows = false;
            this.dgvBeiAnDanM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBeiAnDanM.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvBeiAnDanM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBeiAnDanM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvBeiAnDanM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvUpdate});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBeiAnDanM.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBeiAnDanM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBeiAnDanM.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvBeiAnDanM.Location = new System.Drawing.Point(3, 19);
            this.dgvBeiAnDanM.Name = "dgvBeiAnDanM";
            this.dgvBeiAnDanM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBeiAnDanM.Size = new System.Drawing.Size(633, 538);
            this.dgvBeiAnDanM.TabIndex = 0;
            this.dgvBeiAnDanM.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBeiAnDanM_CellMouseClick);
            // 
            // dgvUpdate
            // 
            this.dgvUpdate.HeaderText = "";
            this.dgvUpdate.Name = "dgvUpdate";
            this.dgvUpdate.Text = "Update";
            this.dgvUpdate.UseColumnTextForButtonValue = true;
            this.dgvUpdate.Width = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvBeiAnDanD);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.Navy;
            this.groupBox3.Location = new System.Drawing.Point(653, 124);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(639, 560);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Detail";
            // 
            // dgvBeiAnDanD
            // 
            this.dgvBeiAnDanD.AllowUserToAddRows = false;
            this.dgvBeiAnDanD.AllowUserToDeleteRows = false;
            this.dgvBeiAnDanD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBeiAnDanD.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvBeiAnDanD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBeiAnDanD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBeiAnDanD.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBeiAnDanD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBeiAnDanD.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvBeiAnDanD.Location = new System.Drawing.Point(3, 19);
            this.dgvBeiAnDanD.Name = "dgvBeiAnDanD";
            this.dgvBeiAnDanD.ReadOnly = true;
            this.dgvBeiAnDanD.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBeiAnDanD.Size = new System.Drawing.Size(633, 538);
            this.dgvBeiAnDanD.TabIndex = 0;
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(185)))), ((int)(((byte)(100)))));
            this.groupBox10.Location = new System.Drawing.Point(642, 121);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(10, 563);
            this.groupBox10.TabIndex = 9;
            this.groupBox10.TabStop = false;
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
            this.fakeLabel1.Location = new System.Drawing.Point(1, 116);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1292, 11);
            this.fakeLabel1.TabIndex = 10;
            // 
            // TotalBeiAnDanDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 685);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TotalBeiAnDanDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BeiAnDan Historical Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TotalBeiAnDanDataForm_FormClosing);
            this.Load += new System.EventHandler(this.TotalBeiAnDanDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBoxEdit.ResumeLayout(false);
            this.groupBoxEdit.PerformLayout();
            this.groupBoxDate.ResumeLayout(false);
            this.groupBoxDate.PerformLayout();
            this.groupBoxIEID.ResumeLayout(false);
            this.groupBoxIEID.PerformLayout();
            this.groupBoxSift.ResumeLayout(false);
            this.groupBoxSift.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDanM)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDanD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox10;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.GroupBox groupBoxSift;
        private System.Windows.Forms.CheckBox cbBeiAnDanNo;
        private System.Windows.Forms.CheckBox cbTaxPaidDate;
        private System.Windows.Forms.CheckBox cbCustomsReleaseDate;
        private System.Windows.Forms.GroupBox groupBoxIEID;
        private System.Windows.Forms.ComboBox cmbBeiAnDanID;
        private System.Windows.Forms.ComboBox cmbIEtype;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.GroupBox groupBoxEdit;
        private System.Windows.Forms.TextBox txtBeiAnDanNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpTaxPaidDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpCustomsReleaseDate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.DataGridView dgvBeiAnDanM;
        private System.Windows.Forms.DataGridView dgvBeiAnDanD;
        private System.Windows.Forms.DataGridViewButtonColumn dgvUpdate;
    }
}