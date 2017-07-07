namespace eCustoms
{
    partial class GetGongdanReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetGongdanReportForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpApprovedDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnGongDan = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvGongDan = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvGongDanList = new System.Windows.Forms.DataGridView();
            this.dgvCheckGD = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDan)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(1, -7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1232, 77);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.dtpApprovedDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnApprove);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnGongDan);
            this.panel1.Location = new System.Drawing.Point(2, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1228, 64);
            this.panel1.TabIndex = 9;
            // 
            // dtpApprovedDate
            // 
            this.dtpApprovedDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpApprovedDate.CustomFormat = "";
            this.dtpApprovedDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpApprovedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpApprovedDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpApprovedDate.Location = new System.Drawing.Point(1008, 19);
            this.dtpApprovedDate.Name = "dtpApprovedDate";
            this.dtpApprovedDate.Size = new System.Drawing.Size(150, 23);
            this.dtpApprovedDate.TabIndex = 4;
            this.dtpApprovedDate.ValueChanged += new System.EventHandler(this.dtpApprovedDate_ValueChanged);
            this.dtpApprovedDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpApprovedDate_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(906, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Approval Date";
            // 
            // btnApprove
            // 
            this.btnApprove.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApprove.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnApprove.Image = ((System.Drawing.Image)(resources.GetObject("btnApprove.Image")));
            this.btnApprove.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnApprove.Location = new System.Drawing.Point(742, 6);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(120, 50);
            this.btnApprove.TabIndex = 2;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDownload.Location = new System.Drawing.Point(592, 6);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(120, 50);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "Download  ";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnGongDan
            // 
            this.btnGongDan.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGongDan.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGongDan.Image = ((System.Drawing.Image)(resources.GetObject("btnGongDan.Image")));
            this.btnGongDan.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnGongDan.Location = new System.Drawing.Point(442, 6);
            this.btnGongDan.Name = "btnGongDan";
            this.btnGongDan.Size = new System.Drawing.Size(120, 50);
            this.btnGongDan.TabIndex = 0;
            this.btnGongDan.Text = "GongDan";
            this.btnGongDan.UseVisualStyleBackColor = true;
            this.btnGongDan.Click += new System.EventHandler(this.btnGongDan_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgvGongDan);
            this.groupBox4.Location = new System.Drawing.Point(2, 76);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1028, 588);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            // 
            // dgvGongDan
            // 
            this.dgvGongDan.AllowUserToAddRows = false;
            this.dgvGongDan.AllowUserToDeleteRows = false;
            this.dgvGongDan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvGongDan.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvGongDan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGongDan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGongDan.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGongDan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGongDan.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvGongDan.Location = new System.Drawing.Point(3, 16);
            this.dgvGongDan.Name = "dgvGongDan";
            this.dgvGongDan.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGongDan.RowTemplate.Height = 23;
            this.dgvGongDan.Size = new System.Drawing.Size(1022, 569);
            this.dgvGongDan.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvGongDanList);
            this.groupBox3.Location = new System.Drawing.Point(1043, 76);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(189, 588);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
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
            this.dgvCheckGD});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGongDanList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvGongDanList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGongDanList.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvGongDanList.Location = new System.Drawing.Point(3, 16);
            this.dgvGongDanList.Name = "dgvGongDanList";
            this.dgvGongDanList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGongDanList.RowTemplate.Height = 23;
            this.dgvGongDanList.Size = new System.Drawing.Size(183, 569);
            this.dgvGongDanList.TabIndex = 1;
            this.dgvGongDanList.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanList_CellMouseUp);
            this.dgvGongDanList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGongDanList_ColumnHeaderMouseClick);
            // 
            // dgvCheckGD
            // 
            this.dgvCheckGD.HeaderText = "Select";
            this.dgvCheckGD.Name = "dgvCheckGD";
            this.dgvCheckGD.Width = 43;
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
            this.fakeLabel1.Location = new System.Drawing.Point(1031, 71);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(11, 592);
            this.fakeLabel1.TabIndex = 29;
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
            this.fakeLabel2.Location = new System.Drawing.Point(1, 70);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel2.TabIndex = 30;
            // 
            // GetGongdanReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 665);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetGongdanReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get GongDan Document Operation Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetCustomsGongdanForm_FormClosing);
            this.Load += new System.EventHandler(this.GetCustomsGongdanForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDan)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGongDanList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgvGongDan;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnGongDan;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvGongDanList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpApprovedDate;
        private UserControls.FakeLabel fakeLabel1;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheckGD;

    }
}