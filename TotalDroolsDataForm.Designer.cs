namespace SHCustomsSystem
{
    partial class TotalDroolsDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalDroolsDataForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.groupBoxCancel = new System.Windows.Forms.GroupBox();
            this.txtBADID = new System.Windows.Forms.TextBox();
            this.lblBADID = new System.Windows.Forms.Label();
            this.cbSinglyDelete = new System.Windows.Forms.CheckBox();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBoxEdit = new System.Windows.Forms.GroupBox();
            this.txtBeiAnDanNo = new System.Windows.Forms.TextBox();
            this.lblBeiAnDanNO = new System.Windows.Forms.Label();
            this.dtpReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTaxDate = new System.Windows.Forms.DateTimePicker();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblReleaseDate = new System.Windows.Forms.Label();
            this.lblTaxDate = new System.Windows.Forms.Label();
            this.groupBoxBrowse = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBeiAnDanNo = new System.Windows.Forms.CheckBox();
            this.cbReleaseDate = new System.Windows.Forms.CheckBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.cmbBeiAnDanID = new System.Windows.Forms.ComboBox();
            this.lblBeiAnDanID = new System.Windows.Forms.Label();
            this.cbTaxDate = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.dgvDrools = new System.Windows.Forms.DataGridView();
            this.dgvDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBoxCancel.SuspendLayout();
            this.groupBoxEdit.SuspendLayout();
            this.groupBoxBrowse.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrools)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(1, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 690);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.fakeLabel2);
            this.panel1.Controls.Add(this.groupBoxCancel);
            this.panel1.Controls.Add(this.fakeLabel1);
            this.panel1.Controls.Add(this.groupBoxEdit);
            this.panel1.Controls.Add(this.groupBoxBrowse);
            this.panel1.Location = new System.Drawing.Point(3, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 676);
            this.panel1.TabIndex = 0;
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
            this.fakeLabel2.Location = new System.Drawing.Point(-1, 555);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(232, 10);
            this.fakeLabel2.TabIndex = 11;
            // 
            // groupBoxCancel
            // 
            this.groupBoxCancel.Controls.Add(this.txtBADID);
            this.groupBoxCancel.Controls.Add(this.lblBADID);
            this.groupBoxCancel.Controls.Add(this.cbSinglyDelete);
            this.groupBoxCancel.Location = new System.Drawing.Point(1, 566);
            this.groupBoxCancel.Name = "groupBoxCancel";
            this.groupBoxCancel.Size = new System.Drawing.Size(228, 105);
            this.groupBoxCancel.TabIndex = 10;
            this.groupBoxCancel.TabStop = false;
            this.groupBoxCancel.Text = "Cancellation";
            // 
            // txtBADID
            // 
            this.txtBADID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBADID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtBADID.Location = new System.Drawing.Point(15, 70);
            this.txtBADID.Name = "txtBADID";
            this.txtBADID.Size = new System.Drawing.Size(160, 23);
            this.txtBADID.TabIndex = 4;
            // 
            // lblBADID
            // 
            this.lblBADID.AutoSize = true;
            this.lblBADID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBADID.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblBADID.Location = new System.Drawing.Point(12, 49);
            this.lblBADID.Name = "lblBADID";
            this.lblBADID.Size = new System.Drawing.Size(143, 17);
            this.lblBADID.TabIndex = 1;
            this.lblBADID.Text = "Drools BeiAnDan ID : ";
            // 
            // cbSinglyDelete
            // 
            this.cbSinglyDelete.AutoSize = true;
            this.cbSinglyDelete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSinglyDelete.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSinglyDelete.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.cbSinglyDelete.Location = new System.Drawing.Point(12, 24);
            this.cbSinglyDelete.Name = "cbSinglyDelete";
            this.cbSinglyDelete.Size = new System.Drawing.Size(207, 21);
            this.cbSinglyDelete.TabIndex = 0;
            this.cbSinglyDelete.Text = "Whether Only Delete A Row :";
            this.cbSinglyDelete.UseVisualStyleBackColor = true;
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
            this.fakeLabel1.Location = new System.Drawing.Point(-1, 317);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(232, 10);
            this.fakeLabel1.TabIndex = 9;
            // 
            // groupBoxEdit
            // 
            this.groupBoxEdit.Controls.Add(this.txtBeiAnDanNo);
            this.groupBoxEdit.Controls.Add(this.lblBeiAnDanNO);
            this.groupBoxEdit.Controls.Add(this.dtpReleaseDate);
            this.groupBoxEdit.Controls.Add(this.dtpTaxDate);
            this.groupBoxEdit.Controls.Add(this.btnUpdate);
            this.groupBoxEdit.Controls.Add(this.lblReleaseDate);
            this.groupBoxEdit.Controls.Add(this.lblTaxDate);
            this.groupBoxEdit.Location = new System.Drawing.Point(1, 328);
            this.groupBoxEdit.Name = "groupBoxEdit";
            this.groupBoxEdit.Size = new System.Drawing.Size(228, 226);
            this.groupBoxEdit.TabIndex = 8;
            this.groupBoxEdit.TabStop = false;
            this.groupBoxEdit.Text = "Edition";
            // 
            // txtBeiAnDanNo
            // 
            this.txtBeiAnDanNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBeiAnDanNo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtBeiAnDanNo.Location = new System.Drawing.Point(15, 45);
            this.txtBeiAnDanNo.Name = "txtBeiAnDanNo";
            this.txtBeiAnDanNo.Size = new System.Drawing.Size(160, 23);
            this.txtBeiAnDanNo.TabIndex = 17;
            // 
            // lblBeiAnDanNO
            // 
            this.lblBeiAnDanNO.AutoSize = true;
            this.lblBeiAnDanNO.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeiAnDanNO.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblBeiAnDanNO.Location = new System.Drawing.Point(12, 24);
            this.lblBeiAnDanNO.Name = "lblBeiAnDanNO";
            this.lblBeiAnDanNO.Size = new System.Drawing.Size(147, 17);
            this.lblBeiAnDanNO.TabIndex = 16;
            this.lblBeiAnDanNO.Text = "Drools BeiAnDan No : ";
            // 
            // dtpReleaseDate
            // 
            this.dtpReleaseDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpReleaseDate.CustomFormat = "";
            this.dtpReleaseDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpReleaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReleaseDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpReleaseDate.Location = new System.Drawing.Point(15, 141);
            this.dtpReleaseDate.Name = "dtpReleaseDate";
            this.dtpReleaseDate.Size = new System.Drawing.Size(120, 23);
            this.dtpReleaseDate.TabIndex = 15;
            this.dtpReleaseDate.ValueChanged += new System.EventHandler(this.dtpReleaseDate_ValueChanged);
            this.dtpReleaseDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpReleaseDate_KeyUp);
            // 
            // dtpTaxDate
            // 
            this.dtpTaxDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTaxDate.CustomFormat = "";
            this.dtpTaxDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTaxDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTaxDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpTaxDate.Location = new System.Drawing.Point(15, 93);
            this.dtpTaxDate.Name = "dtpTaxDate";
            this.dtpTaxDate.Size = new System.Drawing.Size(120, 23);
            this.dtpTaxDate.TabIndex = 14;
            this.dtpTaxDate.ValueChanged += new System.EventHandler(this.dtpTaxDate_ValueChanged);
            this.dtpTaxDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpTaxDate_KeyUp);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnUpdate.Location = new System.Drawing.Point(54, 175);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 40);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblReleaseDate
            // 
            this.lblReleaseDate.AutoSize = true;
            this.lblReleaseDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReleaseDate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblReleaseDate.Location = new System.Drawing.Point(12, 120);
            this.lblReleaseDate.Name = "lblReleaseDate";
            this.lblReleaseDate.Size = new System.Drawing.Size(151, 17);
            this.lblReleaseDate.TabIndex = 2;
            this.lblReleaseDate.Text = "Customs Release Date :";
            // 
            // lblTaxDate
            // 
            this.lblTaxDate.AutoSize = true;
            this.lblTaxDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxDate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTaxDate.Location = new System.Drawing.Point(12, 72);
            this.lblTaxDate.Name = "lblTaxDate";
            this.lblTaxDate.Size = new System.Drawing.Size(154, 17);
            this.lblTaxDate.TabIndex = 0;
            this.lblTaxDate.Text = "Tax && Duty Paid Date : ";
            // 
            // groupBoxBrowse
            // 
            this.groupBoxBrowse.Controls.Add(this.dtpTo);
            this.groupBoxBrowse.Controls.Add(this.dtpFrom);
            this.groupBoxBrowse.Controls.Add(this.label3);
            this.groupBoxBrowse.Controls.Add(this.label2);
            this.groupBoxBrowse.Controls.Add(this.label1);
            this.groupBoxBrowse.Controls.Add(this.cbBeiAnDanNo);
            this.groupBoxBrowse.Controls.Add(this.cbReleaseDate);
            this.groupBoxBrowse.Controls.Add(this.lblPrompt);
            this.groupBoxBrowse.Controls.Add(this.btnPreview);
            this.groupBoxBrowse.Controls.Add(this.cmbBeiAnDanID);
            this.groupBoxBrowse.Controls.Add(this.lblBeiAnDanID);
            this.groupBoxBrowse.Controls.Add(this.cbTaxDate);
            this.groupBoxBrowse.Location = new System.Drawing.Point(1, 1);
            this.groupBoxBrowse.Name = "groupBoxBrowse";
            this.groupBoxBrowse.Size = new System.Drawing.Size(228, 315);
            this.groupBoxBrowse.TabIndex = 0;
            this.groupBoxBrowse.TabStop = false;
            this.groupBoxBrowse.Text = "Browse";
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.CustomFormat = "";
            this.dtpTo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpTo.Location = new System.Drawing.Point(55, 174);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(120, 23);
            this.dtpTo.TabIndex = 20;
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
            this.dtpFrom.Location = new System.Drawing.Point(55, 146);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(120, 23);
            this.dtpFrom.TabIndex = 19;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(12, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "To ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(12, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 17;
            this.label2.Text = "From ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(12, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Drools BeiAnDan Date :";
            // 
            // cbBeiAnDanNo
            // 
            this.cbBeiAnDanNo.AutoSize = true;
            this.cbBeiAnDanNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBeiAnDanNo.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.cbBeiAnDanNo.Location = new System.Drawing.Point(15, 46);
            this.cbBeiAnDanNo.Name = "cbBeiAnDanNo";
            this.cbBeiAnDanNo.Size = new System.Drawing.Size(155, 21);
            this.cbBeiAnDanNo.TabIndex = 15;
            this.cbBeiAnDanNo.Text = "Drools BeiAnDan No";
            this.cbBeiAnDanNo.UseVisualStyleBackColor = true;
            this.cbBeiAnDanNo.CheckedChanged += new System.EventHandler(this.cbBeiAnDanNo_CheckedChanged);
            // 
            // cbReleaseDate
            // 
            this.cbReleaseDate.AutoSize = true;
            this.cbReleaseDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbReleaseDate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.cbReleaseDate.Location = new System.Drawing.Point(15, 98);
            this.cbReleaseDate.Name = "cbReleaseDate";
            this.cbReleaseDate.Size = new System.Drawing.Size(163, 21);
            this.cbReleaseDate.TabIndex = 14;
            this.cbReleaseDate.Text = "Customs Release Date";
            this.cbReleaseDate.UseVisualStyleBackColor = true;
            this.cbReleaseDate.CheckedChanged += new System.EventHandler(this.cbReleaseDate_CheckedChanged);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrompt.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblPrompt.Location = new System.Drawing.Point(12, 24);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(202, 17);
            this.lblPrompt.TabIndex = 13;
            this.lblPrompt.Text = "Per No-generated Data To Sift :";
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnPreview.Location = new System.Drawing.Point(54, 263);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(120, 40);
            this.btnPreview.TabIndex = 12;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // cmbBeiAnDanID
            // 
            this.cmbBeiAnDanID.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.cmbBeiAnDanID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBeiAnDanID.FormattingEnabled = true;
            this.cmbBeiAnDanID.Location = new System.Drawing.Point(15, 229);
            this.cmbBeiAnDanID.Name = "cmbBeiAnDanID";
            this.cmbBeiAnDanID.Size = new System.Drawing.Size(160, 25);
            this.cmbBeiAnDanID.TabIndex = 2;
            this.cmbBeiAnDanID.SelectedIndexChanged += new System.EventHandler(this.cmbBeiAnDanID_SelectedIndexChanged);
            this.cmbBeiAnDanID.Enter += new System.EventHandler(this.cmbBeiAnDanID_Enter);
            // 
            // lblBeiAnDanID
            // 
            this.lblBeiAnDanID.AutoSize = true;
            this.lblBeiAnDanID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeiAnDanID.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblBeiAnDanID.Location = new System.Drawing.Point(12, 208);
            this.lblBeiAnDanID.Name = "lblBeiAnDanID";
            this.lblBeiAnDanID.Size = new System.Drawing.Size(139, 17);
            this.lblBeiAnDanID.TabIndex = 1;
            this.lblBeiAnDanID.Text = "Drools BeiAnDan ID :";
            // 
            // cbTaxDate
            // 
            this.cbTaxDate.AutoSize = true;
            this.cbTaxDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTaxDate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.cbTaxDate.Location = new System.Drawing.Point(15, 72);
            this.cbTaxDate.Name = "cbTaxDate";
            this.cbTaxDate.Size = new System.Drawing.Size(162, 21);
            this.cbTaxDate.TabIndex = 0;
            this.cbTaxDate.Text = "Tax && Duty Paid Date";
            this.cbTaxDate.UseVisualStyleBackColor = true;
            this.cbTaxDate.CheckedChanged += new System.EventHandler(this.cbTaxDate_CheckedChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.dgvDrools);
            this.groupBox9.Location = new System.Drawing.Point(249, -5);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(1043, 690);
            this.groupBox9.TabIndex = 1;
            this.groupBox9.TabStop = false;
            // 
            // dgvDrools
            // 
            this.dgvDrools.AllowUserToAddRows = false;
            this.dgvDrools.AllowUserToDeleteRows = false;
            this.dgvDrools.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDrools.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvDrools.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDrools.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDrools.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDelete});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDrools.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDrools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDrools.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvDrools.Location = new System.Drawing.Point(3, 16);
            this.dgvDrools.Name = "dgvDrools";
            this.dgvDrools.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDrools.Size = new System.Drawing.Size(1037, 671);
            this.dgvDrools.TabIndex = 0;
            this.dgvDrools.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDrools_CellClick);
            // 
            // dgvDelete
            // 
            this.dgvDelete.HeaderText = "";
            this.dgvDelete.Name = "dgvDelete";
            this.dgvDelete.Text = "Delete";
            this.dgvDelete.UseColumnTextForButtonValue = true;
            this.dgvDelete.Width = 5;
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(185)))), ((int)(((byte)(100)))));
            this.groupBox10.Location = new System.Drawing.Point(240, -7);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(10, 692);
            this.groupBox10.TabIndex = 5;
            this.groupBox10.TabStop = false;
            // 
            // TotalDroolsDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 685);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TotalDroolsDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Drools Historical Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TotalDroolsDataForm_FormClosing);
            this.Load += new System.EventHandler(this.TotalDroolsDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBoxCancel.ResumeLayout(false);
            this.groupBoxCancel.PerformLayout();
            this.groupBoxEdit.ResumeLayout(false);
            this.groupBoxEdit.PerformLayout();
            this.groupBoxBrowse.ResumeLayout(false);
            this.groupBoxBrowse.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrools)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBoxBrowse;
        private System.Windows.Forms.CheckBox cbTaxDate;
        private System.Windows.Forms.Label lblBeiAnDanID;
        private System.Windows.Forms.ComboBox cmbBeiAnDanID;
        private System.Windows.Forms.Button btnPreview;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.GroupBox groupBoxEdit;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.CheckBox cbReleaseDate;
        private System.Windows.Forms.Label lblTaxDate;
        private System.Windows.Forms.Label lblReleaseDate;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox groupBoxCancel;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.CheckBox cbSinglyDelete;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.DataGridView dgvDrools;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDelete;
        private System.Windows.Forms.DateTimePicker dtpTaxDate;
        private System.Windows.Forms.DateTimePicker dtpReleaseDate;
        private System.Windows.Forms.Label lblBADID;
        private System.Windows.Forms.TextBox txtBADID;
        private System.Windows.Forms.CheckBox cbBeiAnDanNo;
        private System.Windows.Forms.Label lblBeiAnDanNO;
        private System.Windows.Forms.TextBox txtBeiAnDanNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
    }
}