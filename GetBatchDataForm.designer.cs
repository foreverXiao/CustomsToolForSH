namespace SHCustomsSystem
{
    partial class GetBatchDataForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetBatchDataForm));
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.txtDay = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvGetBatch = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtMulBatch = new System.Windows.Forms.TextBox();
            this.fakeLabel3 = new UserControls.FakeLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvUploadBatch = new System.Windows.Forms.DataGridView();
            this.btnSearchBatch = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.fakeLabel4 = new UserControls.FakeLabel();
            this.btnUploadBatch = new System.Windows.Forms.Button();
            this.fakeLabel5 = new UserControls.FakeLabel();
            this.fakeLabel6 = new UserControls.FakeLabel();
            this.btnGatherBatch = new System.Windows.Forms.Button();
            this.btnSaveOPM = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGetBatch)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploadBatch)).BeginInit();
            this.SuspendLayout();
            // 
            // fakeLabel1
            // 
            this.fakeLabel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fakeLabel1.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.fakeLabel1.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel1.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel1.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel1.CImage = null;
            this.fakeLabel1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fakeLabel1.LabelText = "Enter list of Batch Nos at below :";
            this.fakeLabel1.Location = new System.Drawing.Point(0, 45);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(216, 43);
            this.fakeLabel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(185)))), ((int)(((byte)(100)))));
            this.groupBox1.Location = new System.Drawing.Point(217, -7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(10, 691);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // fakeLabel2
            // 
            this.fakeLabel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fakeLabel2.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.fakeLabel2.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel2.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel2.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel2.CImage = null;
            this.fakeLabel2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fakeLabel2.LabelText = "Enter X                              to retrieve Batches that are closed since X " +
    "days ago.";
            this.fakeLabel2.Location = new System.Drawing.Point(228, 0);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1006, 43);
            this.fakeLabel2.TabIndex = 3;
            // 
            // txtDay
            // 
            this.txtDay.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDay.ForeColor = System.Drawing.Color.Red;
            this.txtDay.Location = new System.Drawing.Point(286, 11);
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(100, 23);
            this.txtDay.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dgvGetBatch);
            this.panel2.Location = new System.Drawing.Point(228, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1006, 436);
            this.panel2.TabIndex = 6;
            // 
            // dgvGetBatch
            // 
            this.dgvGetBatch.AllowUserToAddRows = false;
            this.dgvGetBatch.AllowUserToDeleteRows = false;
            this.dgvGetBatch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvGetBatch.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvGetBatch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGetBatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGetBatch.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGetBatch.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvGetBatch.Location = new System.Drawing.Point(3, 3);
            this.dgvGetBatch.Name = "dgvGetBatch";
            this.dgvGetBatch.ReadOnly = true;
            this.dgvGetBatch.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGetBatch.RowTemplate.Height = 23;
            this.dgvGetBatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGetBatch.Size = new System.Drawing.Size(998, 428);
            this.dgvGetBatch.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtMulBatch);
            this.panel1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(216, 595);
            this.panel1.TabIndex = 7;
            // 
            // txtMulBatch
            // 
            this.txtMulBatch.BackColor = System.Drawing.Color.Gainsboro;
            this.txtMulBatch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMulBatch.ForeColor = System.Drawing.Color.Navy;
            this.txtMulBatch.Location = new System.Drawing.Point(2, 1);
            this.txtMulBatch.Multiline = true;
            this.txtMulBatch.Name = "txtMulBatch";
            this.txtMulBatch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMulBatch.Size = new System.Drawing.Size(210, 591);
            this.txtMulBatch.TabIndex = 0;
            // 
            // fakeLabel3
            // 
            this.fakeLabel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fakeLabel3.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.fakeLabel3.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel3.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel3.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel3.CImage = null;
            this.fakeLabel3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.fakeLabel3.LabelText = "";
            this.fakeLabel3.Location = new System.Drawing.Point(228, 480);
            this.fakeLabel3.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel3.Name = "fakeLabel3";
            this.fakeLabel3.Size = new System.Drawing.Size(1006, 41);
            this.fakeLabel3.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.dgvUploadBatch);
            this.panel3.Location = new System.Drawing.Point(228, 521);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1006, 163);
            this.panel3.TabIndex = 9;
            // 
            // dgvUploadBatch
            // 
            this.dgvUploadBatch.AllowUserToAddRows = false;
            this.dgvUploadBatch.AllowUserToDeleteRows = false;
            this.dgvUploadBatch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvUploadBatch.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvUploadBatch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUploadBatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUploadBatch.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUploadBatch.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvUploadBatch.Location = new System.Drawing.Point(3, 3);
            this.dgvUploadBatch.Name = "dgvUploadBatch";
            this.dgvUploadBatch.ReadOnly = true;
            this.dgvUploadBatch.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvUploadBatch.RowTemplate.Height = 23;
            this.dgvUploadBatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUploadBatch.Size = new System.Drawing.Size(998, 155);
            this.dgvUploadBatch.TabIndex = 0;
            // 
            // btnSearchBatch
            // 
            this.btnSearchBatch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearchBatch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearchBatch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchBatch.Image")));
            this.btnSearchBatch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchBatch.Location = new System.Drawing.Point(354, 486);
            this.btnSearchBatch.Name = "btnSearchBatch";
            this.btnSearchBatch.Size = new System.Drawing.Size(110, 30);
            this.btnSearchBatch.TabIndex = 13;
            this.btnSearchBatch.Text = "Search Batch";
            this.btnSearchBatch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchBatch.UseVisualStyleBackColor = true;
            this.btnSearchBatch.Click += new System.EventHandler(this.btnSearchBatch_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.ForeColor = System.Drawing.Color.Red;
            this.txtPath.Location = new System.Drawing.Point(477, 489);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(315, 23);
            this.txtPath.TabIndex = 14;
            // 
            // fakeLabel4
            // 
            this.fakeLabel4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fakeLabel4.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.fakeLabel4.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel4.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel4.CFontColor = System.Drawing.Color.Red;
            this.fakeLabel4.CImage = null;
            this.fakeLabel4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fakeLabel4.LabelText = "";
            this.fakeLabel4.Location = new System.Drawing.Point(955, 480);
            this.fakeLabel4.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel4.Name = "fakeLabel4";
            this.fakeLabel4.Size = new System.Drawing.Size(279, 41);
            this.fakeLabel4.TabIndex = 15;
            // 
            // btnUploadBatch
            // 
            this.btnUploadBatch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUploadBatch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUploadBatch.Image = ((System.Drawing.Image)(resources.GetObject("btnUploadBatch.Image")));
            this.btnUploadBatch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUploadBatch.Location = new System.Drawing.Point(819, 486);
            this.btnUploadBatch.Name = "btnUploadBatch";
            this.btnUploadBatch.Size = new System.Drawing.Size(110, 30);
            this.btnUploadBatch.TabIndex = 10;
            this.btnUploadBatch.Text = "Import Batch";
            this.btnUploadBatch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadBatch.UseVisualStyleBackColor = true;
            this.btnUploadBatch.Click += new System.EventHandler(this.btnUploadBatch_Click);
            // 
            // fakeLabel5
            // 
            this.fakeLabel5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fakeLabel5.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.fakeLabel5.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel5.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel5.CFontColor = System.Drawing.Color.Navy;
            this.fakeLabel5.CImage = null;
            this.fakeLabel5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fakeLabel5.LabelText = "Specification  ";
            this.fakeLabel5.Location = new System.Drawing.Point(228, 480);
            this.fakeLabel5.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel5.Name = "fakeLabel5";
            this.fakeLabel5.Size = new System.Drawing.Size(100, 41);
            this.fakeLabel5.TabIndex = 21;
            this.fakeLabel5.Click += new System.EventHandler(this.fakeLabel5_Click);
            // 
            // fakeLabel6
            // 
            this.fakeLabel6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fakeLabel6.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.fakeLabel6.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel6.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel6.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel6.CImage = null;
            this.fakeLabel6.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fakeLabel6.LabelText = "Gather Batch Data :";
            this.fakeLabel6.Location = new System.Drawing.Point(0, 0);
            this.fakeLabel6.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel6.Name = "fakeLabel6";
            this.fakeLabel6.Size = new System.Drawing.Size(216, 43);
            this.fakeLabel6.TabIndex = 22;
            // 
            // btnGatherBatch
            // 
            this.btnGatherBatch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGatherBatch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGatherBatch.Image = ((System.Drawing.Image)(resources.GetObject("btnGatherBatch.Image")));
            this.btnGatherBatch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGatherBatch.Location = new System.Drawing.Point(136, 8);
            this.btnGatherBatch.Name = "btnGatherBatch";
            this.btnGatherBatch.Size = new System.Drawing.Size(50, 30);
            this.btnGatherBatch.TabIndex = 23;
            this.btnGatherBatch.Text = " Go";
            this.btnGatherBatch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGatherBatch.UseVisualStyleBackColor = true;
            this.btnGatherBatch.Click += new System.EventHandler(this.btnGatherBatch_Click);
            // 
            // btnSaveOPM
            // 
            this.btnSaveOPM.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveOPM.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSaveOPM.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveOPM.Image")));
            this.btnSaveOPM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveOPM.Location = new System.Drawing.Point(1067, 7);
            this.btnSaveOPM.Name = "btnSaveOPM";
            this.btnSaveOPM.Size = new System.Drawing.Size(125, 33);
            this.btnSaveOPM.TabIndex = 25;
            this.btnSaveOPM.Text = "Save OPM Data";
            this.btnSaveOPM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveOPM.UseVisualStyleBackColor = true;
            this.btnSaveOPM.Click += new System.EventHandler(this.btnSaveOPM_Click);
            // 
            // GetBatchDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 685);
            this.Controls.Add(this.btnSaveOPM);
            this.Controls.Add(this.btnGatherBatch);
            this.Controls.Add(this.fakeLabel6);
            this.Controls.Add(this.fakeLabel5);
            this.Controls.Add(this.fakeLabel4);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnSearchBatch);
            this.Controls.Add(this.btnUploadBatch);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.fakeLabel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtDay);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fakeLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetBatchDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get Batch Data";
            this.Load += new System.EventHandler(this.GetBatchForm_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGetBatch)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploadBatch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.TextBox txtDay;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvGetBatch;
        private System.Windows.Forms.Panel panel1;
        private UserControls.FakeLabel fakeLabel3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvUploadBatch;
        private System.Windows.Forms.TextBox txtMulBatch;
        private System.Windows.Forms.Button btnSearchBatch;
        private System.Windows.Forms.TextBox txtPath;
        private UserControls.FakeLabel fakeLabel4;
        private System.Windows.Forms.Button btnUploadBatch;
        private UserControls.FakeLabel fakeLabel5;
        private UserControls.FakeLabel fakeLabel6;
        private System.Windows.Forms.Button btnGatherBatch;
        private System.Windows.Forms.Button btnSaveOPM;
    }
}