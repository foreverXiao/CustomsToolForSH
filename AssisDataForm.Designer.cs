namespace SHCustomsSystem
{
    partial class AssisDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssisDataForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gb = new System.Windows.Forms.GroupBox();
            this.llinkPrompt = new System.Windows.Forms.LinkLabel();
            this.cmbUploadObj = new System.Windows.Forms.ComboBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvQueryData = new System.Windows.Forms.DataGridView();
            this.gb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueryData)).BeginInit();
            this.SuspendLayout();
            // 
            // gb
            // 
            this.gb.Controls.Add(this.llinkPrompt);
            this.gb.Controls.Add(this.cmbUploadObj);
            this.gb.Controls.Add(this.lblPrompt);
            this.gb.Controls.Add(this.btnBrowse);
            this.gb.Controls.Add(this.btnUpload);
            this.gb.Controls.Add(this.label2);
            this.gb.Controls.Add(this.btnSearch);
            this.gb.Controls.Add(this.txtFilePath);
            this.gb.Controls.Add(this.label1);
            this.gb.Location = new System.Drawing.Point(67, 2);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(1100, 152);
            this.gb.TabIndex = 0;
            this.gb.TabStop = false;
            // 
            // llinkPrompt
            // 
            this.llinkPrompt.AutoSize = true;
            this.llinkPrompt.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.llinkPrompt.Location = new System.Drawing.Point(934, 87);
            this.llinkPrompt.Name = "llinkPrompt";
            this.llinkPrompt.Size = new System.Drawing.Size(87, 17);
            this.llinkPrompt.TabIndex = 8;
            this.llinkPrompt.TabStop = true;
            this.llinkPrompt.Text = "Specification";
            this.llinkPrompt.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llinkPrompt_LinkClicked);
            // 
            // cmbUploadObj
            // 
            this.cmbUploadObj.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbUploadObj.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbUploadObj.FormattingEnabled = true;
            this.cmbUploadObj.Location = new System.Drawing.Point(244, 29);
            this.cmbUploadObj.Name = "cmbUploadObj";
            this.cmbUploadObj.Size = new System.Drawing.Size(180, 25);
            this.cmbUploadObj.TabIndex = 1;
            this.cmbUploadObj.Enter += new System.EventHandler(this.cmbUploadObj_Enter);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrompt.ForeColor = System.Drawing.Color.Red;
            this.lblPrompt.Location = new System.Drawing.Point(244, 117);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(71, 19);
            this.lblPrompt.TabIndex = 7;
            this.lblPrompt.Text = "unvisible...";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowse.Location = new System.Drawing.Point(827, 81);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(85, 29);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.Location = new System.Drawing.Point(721, 81);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(85, 29);
            this.btnUpload.TabIndex = 5;
            this.btnUpload.Text = "  Import";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9.5F);
            this.label2.Location = new System.Drawing.Point(196, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Path :";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(615, 81);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 29);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "   Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFilePath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFilePath.Location = new System.Drawing.Point(244, 83);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(350, 23);
            this.txtFilePath.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9.5F);
            this.label1.Location = new System.Drawing.Point(71, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the upload object : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvQueryData);
            this.groupBox1.Location = new System.Drawing.Point(67, 157);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1100, 525);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // dgvQueryData
            // 
            this.dgvQueryData.AllowUserToAddRows = false;
            this.dgvQueryData.AllowUserToDeleteRows = false;
            this.dgvQueryData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvQueryData.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvQueryData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQueryData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueryData.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQueryData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueryData.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvQueryData.Location = new System.Drawing.Point(3, 16);
            this.dgvQueryData.Name = "dgvQueryData";
            this.dgvQueryData.ReadOnly = true;
            this.dgvQueryData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvQueryData.RowTemplate.Height = 23;
            this.dgvQueryData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQueryData.Size = new System.Drawing.Size(1094, 506);
            this.dgvQueryData.TabIndex = 0;
            // 
            // AssisDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 685);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AssisDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assistant Data";
            this.Load += new System.EventHandler(this.AssisDataForm_Load);
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueryData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvQueryData;
        private System.Windows.Forms.ComboBox cmbUploadObj;
        private System.Windows.Forms.LinkLabel llinkPrompt;
    }
}