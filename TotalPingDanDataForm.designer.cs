﻿namespace SHCustomsSystem
{
    partial class TotalPingDanDataForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalPingDanDataForm));
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.dgvPingDan = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvSplit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvSave = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbGroupID = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBoxEdit = new System.Windows.Forms.GroupBox();
            this.txtPingDanNo = new System.Windows.Forms.TextBox();
            this.dtpPassGateTime = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPingDanID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxSift = new System.Windows.Forms.GroupBox();
            this.cbPassGateTime = new System.Windows.Forms.CheckBox();
            this.cbPingDanNo = new System.Windows.Forms.CheckBox();
            this.cbPingDanID = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnMTI = new System.Windows.Forms.RadioButton();
            this.rbtnMTO = new System.Windows.Forms.RadioButton();
            this.btnGeneratePingDan = new System.Windows.Forms.Button();
            this.cmbIEType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPingDan)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxEdit.SuspendLayout();
            this.groupBoxSift.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.fakeLabel1.Location = new System.Drawing.Point(1, 114);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1292, 11);
            this.fakeLabel1.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDownload);
            this.groupBox2.Controls.Add(this.dgvPingDan);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(2, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1290, 566);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // btnDownload
            // 
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.Location = new System.Drawing.Point(3, 19);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(41, 23);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "*";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // dgvPingDan
            // 
            this.dgvPingDan.AllowUserToAddRows = false;
            this.dgvPingDan.AllowUserToDeleteRows = false;
            this.dgvPingDan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPingDan.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvPingDan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPingDan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPingDan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.dgvSplit,
            this.dgvSave});
            this.dgvPingDan.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPingDan.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPingDan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPingDan.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvPingDan.Location = new System.Drawing.Point(3, 19);
            this.dgvPingDan.Name = "dgvPingDan";
            this.dgvPingDan.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPingDan.Size = new System.Drawing.Size(1284, 544);
            this.dgvPingDan.TabIndex = 0;
            this.dgvPingDan.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPingDan_CellMouseClick);
            this.dgvPingDan.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPingDan_ColumnHeaderMouseClick);
            this.dgvPingDan.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvPingDan_MouseUp);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "全选";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 38;
            // 
            // dgvSplit
            // 
            this.dgvSplit.HeaderText = "";
            this.dgvSplit.Name = "dgvSplit";
            this.dgvSplit.Text = "Split";
            this.dgvSplit.UseColumnTextForButtonValue = true;
            this.dgvSplit.Width = 5;
            // 
            // dgvSave
            // 
            this.dgvSave.HeaderText = "";
            this.dgvSave.Name = "dgvSave";
            this.dgvSave.Text = "Save";
            this.dgvSave.UseColumnTextForButtonValue = true;
            this.dgvSave.Width = 5;
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
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.panel1);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox5.Location = new System.Drawing.Point(2, -6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1290, 120);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.txtPath);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.btnUpload);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Controls.Add(this.groupBoxEdit);
            this.panel1.Controls.Add(this.groupBoxSift);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(3, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1284, 106);
            this.panel1.TabIndex = 13;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(707, 69);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(105, 23);
            this.txtPath.TabIndex = 21;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.Location = new System.Drawing.Point(631, 66);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(74, 30);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = " Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowse.Location = new System.Drawing.Point(818, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 50);
            this.btnBrowse.TabIndex = 11;
            this.btnBrowse.Text = " Preview";
            this.btnBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpload.Location = new System.Drawing.Point(818, 66);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 30);
            this.btnUpload.TabIndex = 18;
            this.btnUpload.Text = " Upload";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbGroupID);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBox3.Location = new System.Drawing.Point(632, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(180, 58);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Group ID";
            // 
            // cmbGroupID
            // 
            this.cmbGroupID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbGroupID.FormattingEnabled = true;
            this.cmbGroupID.Location = new System.Drawing.Point(15, 22);
            this.cmbGroupID.Name = "cmbGroupID";
            this.cmbGroupID.Size = new System.Drawing.Size(150, 25);
            this.cmbGroupID.TabIndex = 0;
            this.cmbGroupID.SelectedIndexChanged += new System.EventHandler(this.cmbGroupID_SelectedIndexChanged);
            this.cmbGroupID.Enter += new System.EventHandler(this.cmbGroupID_Enter);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.Location = new System.Drawing.Point(1194, 54);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 40);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "  Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdate.Location = new System.Drawing.Point(1194, 10);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 40);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = " Update";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // groupBoxEdit
            // 
            this.groupBoxEdit.Controls.Add(this.txtPingDanNo);
            this.groupBoxEdit.Controls.Add(this.dtpPassGateTime);
            this.groupBoxEdit.Controls.Add(this.label7);
            this.groupBoxEdit.Controls.Add(this.label6);
            this.groupBoxEdit.Controls.Add(this.txtPingDanID);
            this.groupBoxEdit.Controls.Add(this.label5);
            this.groupBoxEdit.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxEdit.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBoxEdit.Location = new System.Drawing.Point(904, -3);
            this.groupBoxEdit.Name = "groupBoxEdit";
            this.groupBoxEdit.Size = new System.Drawing.Size(284, 100);
            this.groupBoxEdit.TabIndex = 13;
            this.groupBoxEdit.TabStop = false;
            // 
            // txtPingDanNo
            // 
            this.txtPingDanNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPingDanNo.Location = new System.Drawing.Point(120, 42);
            this.txtPingDanNo.Name = "txtPingDanNo";
            this.txtPingDanNo.Size = new System.Drawing.Size(150, 23);
            this.txtPingDanNo.TabIndex = 6;
            // 
            // dtpPassGateTime
            // 
            this.dtpPassGateTime.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPassGateTime.CustomFormat = "";
            this.dtpPassGateTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPassGateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPassGateTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpPassGateTime.Location = new System.Drawing.Point(120, 69);
            this.dtpPassGateTime.Name = "dtpPassGateTime";
            this.dtpPassGateTime.Size = new System.Drawing.Size(110, 23);
            this.dtpPassGateTime.TabIndex = 5;
            this.dtpPassGateTime.ValueChanged += new System.EventHandler(this.dtpPassGateTime_ValueChanged);
            this.dtpPassGateTime.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpPassGateTime_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Pass Gate Time";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "PingDan No";
            // 
            // txtPingDanID
            // 
            this.txtPingDanID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPingDanID.Location = new System.Drawing.Point(120, 15);
            this.txtPingDanID.Name = "txtPingDanID";
            this.txtPingDanID.Size = new System.Drawing.Size(150, 23);
            this.txtPingDanID.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "PingDan ID";
            // 
            // groupBoxSift
            // 
            this.groupBoxSift.Controls.Add(this.cbPassGateTime);
            this.groupBoxSift.Controls.Add(this.cbPingDanNo);
            this.groupBoxSift.Controls.Add(this.cbPingDanID);
            this.groupBoxSift.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxSift.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBoxSift.Location = new System.Drawing.Point(436, 2);
            this.groupBoxSift.Name = "groupBoxSift";
            this.groupBoxSift.Size = new System.Drawing.Size(190, 95);
            this.groupBoxSift.TabIndex = 9;
            this.groupBoxSift.TabStop = false;
            this.groupBoxSift.Text = "No-generated Data to Sift";
            // 
            // cbPassGateTime
            // 
            this.cbPassGateTime.AutoSize = true;
            this.cbPassGateTime.Location = new System.Drawing.Point(35, 67);
            this.cbPassGateTime.Name = "cbPassGateTime";
            this.cbPassGateTime.Size = new System.Drawing.Size(121, 21);
            this.cbPassGateTime.TabIndex = 2;
            this.cbPassGateTime.Text = "Pass Gate Time";
            this.cbPassGateTime.UseVisualStyleBackColor = true;
            // 
            // cbPingDanNo
            // 
            this.cbPingDanNo.AutoSize = true;
            this.cbPingDanNo.Location = new System.Drawing.Point(35, 43);
            this.cbPingDanNo.Name = "cbPingDanNo";
            this.cbPingDanNo.Size = new System.Drawing.Size(102, 21);
            this.cbPingDanNo.TabIndex = 1;
            this.cbPingDanNo.Text = "PingDan No";
            this.cbPingDanNo.UseVisualStyleBackColor = true;
            this.cbPingDanNo.CheckedChanged += new System.EventHandler(this.cbPingDanNo_CheckedChanged);
            // 
            // cbPingDanID
            // 
            this.cbPingDanID.AutoSize = true;
            this.cbPingDanID.Location = new System.Drawing.Point(35, 19);
            this.cbPingDanID.Name = "cbPingDanID";
            this.cbPingDanID.Size = new System.Drawing.Size(98, 21);
            this.cbPingDanID.TabIndex = 0;
            this.cbPingDanID.Text = "PingDan ID";
            this.cbPingDanID.UseVisualStyleBackColor = true;
            this.cbPingDanID.CheckedChanged += new System.EventHandler(this.cbPingDanID_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dtpTo);
            this.groupBox4.Controls.Add(this.dtpFrom);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.groupBox4.Location = new System.Drawing.Point(241, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(190, 95);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PingDan Date";
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(66, 57);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 23);
            this.dtpTo.TabIndex = 7;
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
            this.dtpFrom.Location = new System.Drawing.Point(66, 23);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 23);
            this.dtpFrom.TabIndex = 6;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "From";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnMTI);
            this.groupBox1.Controls.Add(this.rbtnMTO);
            this.groupBox1.Controls.Add(this.btnGeneratePingDan);
            this.groupBox1.Controls.Add(this.cmbIEType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // rbtnMTI
            // 
            this.rbtnMTI.AutoSize = true;
            this.rbtnMTI.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnMTI.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.rbtnMTI.Location = new System.Drawing.Point(168, 20);
            this.rbtnMTI.Name = "rbtnMTI";
            this.rbtnMTI.Size = new System.Drawing.Size(50, 21);
            this.rbtnMTI.TabIndex = 7;
            this.rbtnMTI.TabStop = true;
            this.rbtnMTI.Text = "MTI";
            this.rbtnMTI.UseVisualStyleBackColor = true;
            // 
            // rbtnMTO
            // 
            this.rbtnMTO.AutoSize = true;
            this.rbtnMTO.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtnMTO.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.rbtnMTO.Location = new System.Drawing.Point(168, 59);
            this.rbtnMTO.Name = "rbtnMTO";
            this.rbtnMTO.Size = new System.Drawing.Size(56, 21);
            this.rbtnMTO.TabIndex = 6;
            this.rbtnMTO.TabStop = true;
            this.rbtnMTO.Text = "MTO";
            this.rbtnMTO.UseVisualStyleBackColor = true;
            // 
            // btnGeneratePingDan
            // 
            this.btnGeneratePingDan.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneratePingDan.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGeneratePingDan.Image = ((System.Drawing.Image)(resources.GetObject("btnGeneratePingDan.Image")));
            this.btnGeneratePingDan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGeneratePingDan.Location = new System.Drawing.Point(12, 53);
            this.btnGeneratePingDan.Name = "btnGeneratePingDan";
            this.btnGeneratePingDan.Size = new System.Drawing.Size(150, 32);
            this.btnGeneratePingDan.TabIndex = 4;
            this.btnGeneratePingDan.Text = " Generate PingDan";
            this.btnGeneratePingDan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGeneratePingDan.UseVisualStyleBackColor = true;
            this.btnGeneratePingDan.Click += new System.EventHandler(this.btnGeneratePingDan_Click);
            // 
            // cmbIEType
            // 
            this.cmbIEType.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbIEType.FormattingEnabled = true;
            this.cmbIEType.Location = new System.Drawing.Point(71, 17);
            this.cmbIEType.Name = "cmbIEType";
            this.cmbIEType.Size = new System.Drawing.Size(90, 25);
            this.cmbIEType.TabIndex = 1;
            this.cmbIEType.SelectedIndexChanged += new System.EventHandler(this.cmbIEType_SelectedIndexChanged);
            this.cmbIEType.Enter += new System.EventHandler(this.cmbIEType_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IE Type";
            // 
            // TotalPingDanDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 685);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TotalPingDanDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get PingDan Data & Document";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TotalPingDanDataForm_FormClosing);
            this.Load += new System.EventHandler(this.TotalPingDanDataForm_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPingDan)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBoxEdit.ResumeLayout(false);
            this.groupBoxEdit.PerformLayout();
            this.groupBoxSift.ResumeLayout(false);
            this.groupBoxSift.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvPingDan;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBoxSift;
        private System.Windows.Forms.CheckBox cbPassGateTime;
        private System.Windows.Forms.CheckBox cbPingDanNo;
        private System.Windows.Forms.CheckBox cbPingDanID;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGeneratePingDan;
        private System.Windows.Forms.ComboBox cmbIEType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxEdit;
        private System.Windows.Forms.DateTimePicker dtpPassGateTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPingDanID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPingDanNo;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.RadioButton rbtnMTI;
        private System.Windows.Forms.RadioButton rbtnMTO;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbGroupID;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewButtonColumn dgvSplit;
        private System.Windows.Forms.DataGridViewButtonColumn dgvSave;
    }
}