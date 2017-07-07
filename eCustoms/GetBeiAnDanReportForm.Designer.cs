namespace eCustoms
{
    partial class GetBeiAnDanReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetBeiAnDanReportForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStatus = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbGroupID = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnApproved = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBeiAnDanNo = new System.Windows.Forms.TextBox();
            this.txtBeiAnDanID = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGatherDoc = new System.Windows.Forms.Button();
            this.cmbIEtype = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvBeiAnDanDoc = new System.Windows.Forms.DataGridView();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.btnReport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDanDoc)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(1, -7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1232, 78);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnReport);
            this.panel1.Controls.Add(this.btnStatus);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbGroupID);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.btnApproved);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtBeiAnDanNo);
            this.panel1.Controls.Add(this.txtBeiAnDanID);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnGatherDoc);
            this.panel1.Controls.Add(this.cmbIEtype);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(2, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1228, 66);
            this.panel1.TabIndex = 14;
            // 
            // btnStatus
            // 
            this.btnStatus.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStatus.ForeColor = System.Drawing.Color.Red;
            this.btnStatus.Location = new System.Drawing.Point(566, 31);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(48, 30);
            this.btnStatus.TabIndex = 7;
            this.btnStatus.Text = "status";
            this.btnStatus.UseVisualStyleBackColor = true;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label4.Location = new System.Drawing.Point(386, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Group ID";
            // 
            // cmbGroupID
            // 
            this.cmbGroupID.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.cmbGroupID.FormattingEnabled = true;
            this.cmbGroupID.ItemHeight = 16;
            this.cmbGroupID.Location = new System.Drawing.Point(452, 20);
            this.cmbGroupID.Name = "cmbGroupID";
            this.cmbGroupID.Size = new System.Drawing.Size(111, 24);
            this.cmbGroupID.TabIndex = 6;
            this.cmbGroupID.Enter += new System.EventHandler(this.cmbGroupID_Enter);
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(377, -8);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(2, 72);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            // 
            // btnApproved
            // 
            this.btnApproved.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApproved.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnApproved.Image = ((System.Drawing.Image)(resources.GetObject("btnApproved.Image")));
            this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnApproved.Location = new System.Drawing.Point(1115, 13);
            this.btnApproved.Name = "btnApproved";
            this.btnApproved.Size = new System.Drawing.Size(100, 36);
            this.btnApproved.TabIndex = 12;
            this.btnApproved.Text = "Approve  ";
            this.btnApproved.UseVisualStyleBackColor = true;
            this.btnApproved.Click += new System.EventHandler(this.btnApproved_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(865, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "BeiAnDan No";
            // 
            // txtBeiAnDanNo
            // 
            this.txtBeiAnDanNo.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtBeiAnDanNo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBeiAnDanNo.Location = new System.Drawing.Point(958, 20);
            this.txtBeiAnDanNo.Name = "txtBeiAnDanNo";
            this.txtBeiAnDanNo.Size = new System.Drawing.Size(150, 22);
            this.txtBeiAnDanNo.TabIndex = 11;
            // 
            // txtBeiAnDanID
            // 
            this.txtBeiAnDanID.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtBeiAnDanID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBeiAnDanID.Location = new System.Drawing.Point(708, 20);
            this.txtBeiAnDanID.Name = "txtBeiAnDanID";
            this.txtBeiAnDanID.Size = new System.Drawing.Size(150, 22);
            this.txtBeiAnDanID.TabIndex = 9;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(268, 13);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 36);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download  ";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(619, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "BeiAnDan ID";
            // 
            // btnGatherDoc
            // 
            this.btnGatherDoc.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGatherDoc.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGatherDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnGatherDoc.Image")));
            this.btnGatherDoc.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGatherDoc.Location = new System.Drawing.Point(163, 13);
            this.btnGatherDoc.Name = "btnGatherDoc";
            this.btnGatherDoc.Size = new System.Drawing.Size(100, 36);
            this.btnGatherDoc.TabIndex = 3;
            this.btnGatherDoc.Text = "BeiAnDan  ";
            this.btnGatherDoc.UseVisualStyleBackColor = true;
            this.btnGatherDoc.Click += new System.EventHandler(this.btnGatherDoc_Click);
            // 
            // cmbIEtype
            // 
            this.cmbIEtype.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.cmbIEtype.FormattingEnabled = true;
            this.cmbIEtype.Location = new System.Drawing.Point(68, 20);
            this.cmbIEtype.Name = "cmbIEtype";
            this.cmbIEtype.Size = new System.Drawing.Size(90, 24);
            this.cmbIEtype.TabIndex = 2;
            this.cmbIEtype.SelectedIndexChanged += new System.EventHandler(this.cmbIEtype_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "I/E Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvBeiAnDanDoc);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(1, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1232, 588);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // dgvBeiAnDanDoc
            // 
            this.dgvBeiAnDanDoc.AllowUserToAddRows = false;
            this.dgvBeiAnDanDoc.AllowUserToDeleteRows = false;
            this.dgvBeiAnDanDoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBeiAnDanDoc.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvBeiAnDanDoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBeiAnDanDoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBeiAnDanDoc.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBeiAnDanDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBeiAnDanDoc.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvBeiAnDanDoc.Location = new System.Drawing.Point(3, 19);
            this.dgvBeiAnDanDoc.Name = "dgvBeiAnDanDoc";
            this.dgvBeiAnDanDoc.ReadOnly = true;
            this.dgvBeiAnDanDoc.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBeiAnDanDoc.Size = new System.Drawing.Size(1226, 566);
            this.dgvBeiAnDanDoc.TabIndex = 17;
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
            this.fakeLabel2.Location = new System.Drawing.Point(1, 71);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel2.TabIndex = 16;
            // 
            // btnReport
            // 
            this.btnReport.Enabled = false;
            this.btnReport.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReport.ForeColor = System.Drawing.Color.Red;
            this.btnReport.Location = new System.Drawing.Point(566, 1);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(48, 30);
            this.btnReport.TabIndex = 21;
            this.btnReport.Text = "report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // GetBeiAnDanReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 665);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetBeiAnDanReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get BeiAnDan Document Operation Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetBeiAnDanReportForm_FormClosing);
            this.Load += new System.EventHandler(this.GetBeiAnDanReportForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeiAnDanDoc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.DataGridView dgvBeiAnDanDoc;
        private System.Windows.Forms.Button btnApproved;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBeiAnDanNo;
        private System.Windows.Forms.TextBox txtBeiAnDanID;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGatherDoc;
        private System.Windows.Forms.ComboBox cmbIEtype;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cmbGroupID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStatus;
        private System.Windows.Forms.Button btnReport;
    }
}