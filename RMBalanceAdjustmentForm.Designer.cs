namespace SHCustomsSystem
{
    partial class RMBalanceAdjustmentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMBalanceAdjustmentForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.llblMessage = new System.Windows.Forms.LinkLabel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cmbBGDNo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbRMCustomsCode = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvRMBalance = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.fakeLabel3 = new UserControls.FakeLabel();
            this.panel3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMBalance)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.llblMessage);
            this.panel3.Controls.Add(this.btnUpload);
            this.panel3.Controls.Add(this.txtPath);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.groupBox8);
            this.panel3.Controls.Add(this.cmbBGDNo);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.cmbRMCustomsCode);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.btnBrowse);
            this.panel3.Controls.Add(this.btnDownload);
            this.panel3.Location = new System.Drawing.Point(1, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1232, 56);
            this.panel3.TabIndex = 11;
            // 
            // llblMessage
            // 
            this.llblMessage.AutoSize = true;
            this.llblMessage.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblMessage.ForeColor = System.Drawing.Color.Navy;
            this.llblMessage.Location = new System.Drawing.Point(734, 18);
            this.llblMessage.Name = "llblMessage";
            this.llblMessage.Size = new System.Drawing.Size(87, 17);
            this.llblMessage.TabIndex = 11;
            this.llblMessage.TabStop = true;
            this.llblMessage.Text = "Specification";
            this.llblMessage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblMessage_LinkClicked);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpload.Location = new System.Drawing.Point(1128, 10);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 33);
            this.btnUpload.TabIndex = 7;
            this.btnUpload.Text = " Upload";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.Location = new System.Drawing.Point(917, 15);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(200, 23);
            this.txtPath.TabIndex = 6;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.Location = new System.Drawing.Point(826, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 33);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "  Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Location = new System.Drawing.Point(608, -7);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(2, 60);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            // 
            // cmbBGDNo
            // 
            this.cmbBGDNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbBGDNo.FormattingEnabled = true;
            this.cmbBGDNo.Location = new System.Drawing.Point(325, 14);
            this.cmbBGDNo.Name = "cmbBGDNo";
            this.cmbBGDNo.Size = new System.Drawing.Size(170, 25);
            this.cmbBGDNo.TabIndex = 2;
            this.cmbBGDNo.Enter += new System.EventHandler(this.cmbBGDNo_Enter);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label10.Location = new System.Drawing.Point(266, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 17);
            this.label10.TabIndex = 9;
            this.label10.Text = "BGD No";
            // 
            // cmbRMCustomsCode
            // 
            this.cmbRMCustomsCode.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbRMCustomsCode.FormattingEnabled = true;
            this.cmbRMCustomsCode.Location = new System.Drawing.Point(143, 14);
            this.cmbRMCustomsCode.Name = "cmbRMCustomsCode";
            this.cmbRMCustomsCode.Size = new System.Drawing.Size(111, 25);
            this.cmbRMCustomsCode.TabIndex = 1;
            this.cmbRMCustomsCode.SelectedIndexChanged += new System.EventHandler(this.cmbRMCustomsCode_SelectedIndexChanged);
            this.cmbRMCustomsCode.Enter += new System.EventHandler(this.cmbRMCustomsCode_Enter);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label9.Location = new System.Drawing.Point(21, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 17);
            this.label9.TabIndex = 8;
            this.label9.Text = "RM Customs Code";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowse.Location = new System.Drawing.Point(510, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 33);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = " Browse";
            this.btnBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(627, 10);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 33);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = " Download";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgvRMBalance);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox5.ForeColor = System.Drawing.Color.Navy;
            this.groupBox5.Location = new System.Drawing.Point(1, 61);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1232, 583);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            // 
            // dgvRMBalance
            // 
            this.dgvRMBalance.AllowUserToAddRows = false;
            this.dgvRMBalance.AllowUserToDeleteRows = false;
            this.dgvRMBalance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRMBalance.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvRMBalance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRMBalance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRMBalance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck});
            this.dgvRMBalance.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRMBalance.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRMBalance.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvRMBalance.Location = new System.Drawing.Point(5, 22);
            this.dgvRMBalance.Name = "dgvRMBalance";
            this.dgvRMBalance.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRMBalance.RowTemplate.Height = 23;
            this.dgvRMBalance.Size = new System.Drawing.Size(1222, 554);
            this.dgvRMBalance.TabIndex = 0;
            this.dgvRMBalance.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRMBalance_ColumnHeaderMouseClick);
            this.dgvRMBalance.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvRMBalance_MouseUp);
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
            this.tsmiRefreshFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(144, 70);
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
            // fakeLabel3
            // 
            this.fakeLabel3.BackColor = System.Drawing.SystemColors.Control;
            this.fakeLabel3.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.fakeLabel3.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel3.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel3.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel3.CImage = null;
            this.fakeLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.fakeLabel3.LabelText = "";
            this.fakeLabel3.Location = new System.Drawing.Point(0, 58);
            this.fakeLabel3.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel3.Name = "fakeLabel3";
            this.fakeLabel3.Size = new System.Drawing.Size(1234, 11);
            this.fakeLabel3.TabIndex = 12;
            // 
            // RMBalanceAdjustmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 646);
            this.Controls.Add(this.fakeLabel3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RMBalanceAdjustmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RM Balance Adjustment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RMBalanceAdjustmentForm_FormClosing);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMBalance)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.ComboBox cmbBGDNo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbRMCustomsCode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvRMBalance;
        private UserControls.FakeLabel fakeLabel3;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.LinkLabel llblMessage;
    }
}