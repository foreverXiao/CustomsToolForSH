namespace eCustoms
{
    partial class HistoryBomDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryBomDataForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpBomTo = new System.Windows.Forms.DateTimePicker();
            this.dtpBomFrm = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.gbBomM = new System.Windows.Forms.GroupBox();
            this.dgvMasterBOM = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvView = new System.Windows.Forms.DataGridViewLinkColumn();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.gbBomD = new System.Windows.Forms.GroupBox();
            this.dgvDetailBOM = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbBomM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterBOM)).BeginInit();
            this.gbBomD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailBOM)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(1, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1232, 67);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.txtBatchNo);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnPreview);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpBomTo);
            this.panel1.Controls.Add(this.dtpBomFrm);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(2, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1228, 56);
            this.panel1.TabIndex = 9;
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtBatchNo.Location = new System.Drawing.Point(457, 15);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(100, 22);
            this.txtBatchNo.TabIndex = 6;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(690, 10);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 33);
            this.btnDownload.TabIndex = 8;
            this.btnDownload.Text = "Download   ";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPreview.Location = new System.Drawing.Point(574, 10);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(100, 33);
            this.btnPreview.TabIndex = 7;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(387, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Batch No";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(226, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "To";
            // 
            // dtpBomTo
            // 
            this.dtpBomTo.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpBomTo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpBomTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBomTo.Location = new System.Drawing.Point(254, 15);
            this.dtpBomTo.Name = "dtpBomTo";
            this.dtpBomTo.Size = new System.Drawing.Size(110, 23);
            this.dtpBomTo.TabIndex = 3;
            this.dtpBomTo.ValueChanged += new System.EventHandler(this.dtpBomTo_ValueChanged);
            this.dtpBomTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpBomTo_KeyUp);
            // 
            // dtpBomFrm
            // 
            this.dtpBomFrm.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpBomFrm.CustomFormat = "";
            this.dtpBomFrm.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpBomFrm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBomFrm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpBomFrm.Location = new System.Drawing.Point(112, 15);
            this.dtpBomFrm.Name = "dtpBomFrm";
            this.dtpBomFrm.Size = new System.Drawing.Size(110, 23);
            this.dtpBomFrm.TabIndex = 1;
            this.dtpBomFrm.ValueChanged += new System.EventHandler(this.dtpBomFrm_ValueChanged);
            this.dtpBomFrm.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpBomFrm_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(33, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date From";
            // 
            // gbBomM
            // 
            this.gbBomM.Controls.Add(this.dgvMasterBOM);
            this.gbBomM.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbBomM.ForeColor = System.Drawing.Color.Navy;
            this.gbBomM.Location = new System.Drawing.Point(2, 74);
            this.gbBomM.Name = "gbBomM";
            this.gbBomM.Size = new System.Drawing.Size(1230, 330);
            this.gbBomM.TabIndex = 13;
            this.gbBomM.TabStop = false;
            this.gbBomM.Text = "Master Data Information";
            // 
            // dgvMasterBOM
            // 
            this.dgvMasterBOM.AllowUserToAddRows = false;
            this.dgvMasterBOM.AllowUserToDeleteRows = false;
            this.dgvMasterBOM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMasterBOM.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvMasterBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMasterBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMasterBOM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.dgvDelete,
            this.dgvView});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMasterBOM.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMasterBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMasterBOM.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvMasterBOM.Location = new System.Drawing.Point(3, 19);
            this.dgvMasterBOM.Name = "dgvMasterBOM";
            this.dgvMasterBOM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvMasterBOM.RowTemplate.Height = 23;
            this.dgvMasterBOM.Size = new System.Drawing.Size(1224, 308);
            this.dgvMasterBOM.TabIndex = 12;
            this.dgvMasterBOM.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMasterBOM_CellMouseClick);
            this.dgvMasterBOM.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMasterBOM_ColumnHeaderMouseClick);
            this.dgvMasterBOM.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvMasterBOM_MouseUp);
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
            // dgvView
            // 
            this.dgvView.HeaderText = "";
            this.dgvView.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.dgvView.LinkColor = System.Drawing.Color.Blue;
            this.dgvView.Name = "dgvView";
            this.dgvView.Text = "View Detail";
            this.dgvView.UseColumnTextForLinkValue = true;
            this.dgvView.Width = 5;
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
            this.fakeLabel1.Location = new System.Drawing.Point(1, 63);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel1.TabIndex = 11;
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
            this.fakeLabel2.Location = new System.Drawing.Point(1, 404);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel2.TabIndex = 14;
            // 
            // gbBomD
            // 
            this.gbBomD.Controls.Add(this.dgvDetailBOM);
            this.gbBomD.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbBomD.ForeColor = System.Drawing.Color.Navy;
            this.gbBomD.Location = new System.Drawing.Point(2, 415);
            this.gbBomD.Name = "gbBomD";
            this.gbBomD.Size = new System.Drawing.Size(1230, 250);
            this.gbBomD.TabIndex = 16;
            this.gbBomD.TabStop = false;
            this.gbBomD.Text = "Detail Data Information";
            // 
            // dgvDetailBOM
            // 
            this.dgvDetailBOM.AllowUserToAddRows = false;
            this.dgvDetailBOM.AllowUserToDeleteRows = false;
            this.dgvDetailBOM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetailBOM.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvDetailBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDetailBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetailBOM.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDetailBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetailBOM.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvDetailBOM.Location = new System.Drawing.Point(3, 19);
            this.dgvDetailBOM.Name = "dgvDetailBOM";
            this.dgvDetailBOM.ReadOnly = true;
            this.dgvDetailBOM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDetailBOM.RowTemplate.Height = 23;
            this.dgvDetailBOM.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetailBOM.Size = new System.Drawing.Size(1224, 228);
            this.dgvDetailBOM.TabIndex = 15;
            // 
            // HistoryBomDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 665);
            this.Controls.Add(this.gbBomD);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.gbBomM);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HistoryBomDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOM History Data Operation Interface";
            this.Load += new System.EventHandler(this.HistoryBomDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbBomM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterBOM)).EndInit();
            this.gbBomD.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailBOM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbBomM;
        private UserControls.FakeLabel fakeLabel1;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.GroupBox gbBomD;
        private System.Windows.Forms.DataGridView dgvMasterBOM;
        private System.Windows.Forms.DataGridView dgvDetailBOM;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpBomTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpBomFrm;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDelete;
        private System.Windows.Forms.DataGridViewLinkColumn dgvView;
    }
}