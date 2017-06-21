namespace SHCustomsSystem
{
    partial class RMUnionQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMUnionQuery));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvRMUnionQuery = new System.Windows.Forms.DataGridView();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRMBalance = new System.Windows.Forms.CheckBox();
            this.cbRMPurchase = new System.Windows.Forms.CheckBox();
            this.cmbRMCustomsCode = new System.Windows.Forms.ComboBox();
            this.cmbItemNo = new System.Windows.Forms.ComboBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMUnionQuery)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvRMUnionQuery);
            this.groupBox2.Location = new System.Drawing.Point(1, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1232, 610);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // dgvRMUnionQuery
            // 
            this.dgvRMUnionQuery.AllowUserToAddRows = false;
            this.dgvRMUnionQuery.AllowUserToDeleteRows = false;
            this.dgvRMUnionQuery.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRMUnionQuery.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvRMUnionQuery.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRMUnionQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRMUnionQuery.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRMUnionQuery.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRMUnionQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRMUnionQuery.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvRMUnionQuery.Location = new System.Drawing.Point(3, 16);
            this.dgvRMUnionQuery.Name = "dgvRMUnionQuery";
            this.dgvRMUnionQuery.ReadOnly = true;
            this.dgvRMUnionQuery.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRMUnionQuery.Size = new System.Drawing.Size(1226, 591);
            this.dgvRMUnionQuery.TabIndex = 0;
            // 
            // cmsFilter
            // 
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(153, 114);
            // 
            // tsmiChooseFilter
            // 
            this.tsmiChooseFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiChooseFilter.Image")));
            this.tsmiChooseFilter.Name = "tsmiChooseFilter";
            this.tsmiChooseFilter.Size = new System.Drawing.Size(152, 22);
            this.tsmiChooseFilter.Text = "Choose Filter";
            this.tsmiChooseFilter.Click += new System.EventHandler(this.tsmiChooseFilter_Click);
            // 
            // tsmiExcludeFilter
            // 
            this.tsmiExcludeFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExcludeFilter.Image")));
            this.tsmiExcludeFilter.Name = "tsmiExcludeFilter";
            this.tsmiExcludeFilter.Size = new System.Drawing.Size(152, 22);
            this.tsmiExcludeFilter.Text = "Exclude Filter";
            this.tsmiExcludeFilter.Click += new System.EventHandler(this.tsmiExcludeFilter_Click);
            // 
            // tsmiRefreshFilter
            // 
            this.tsmiRefreshFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRefreshFilter.Image")));
            this.tsmiRefreshFilter.Name = "tsmiRefreshFilter";
            this.tsmiRefreshFilter.Size = new System.Drawing.Size(152, 22);
            this.tsmiRefreshFilter.Text = "Refresh Filter";
            this.tsmiRefreshFilter.Click += new System.EventHandler(this.tsmiRefreshFilter_Click);
            // 
            // tsmiRecordFilter
            // 
            this.tsmiRecordFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRecordFilter.Image")));
            this.tsmiRecordFilter.Name = "tsmiRecordFilter";
            this.tsmiRecordFilter.Size = new System.Drawing.Size(152, 22);
            this.tsmiRecordFilter.Text = "Record Filter";
            this.tsmiRecordFilter.Click += new System.EventHandler(this.tsmiRecordFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(1, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1232, 80);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbRMBalance);
            this.panel1.Controls.Add(this.cbRMPurchase);
            this.panel1.Controls.Add(this.cmbRMCustomsCode);
            this.panel1.Controls.Add(this.cmbItemNo);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnPreview);
            this.panel1.Controls.Add(this.fakeLabel1);
            this.panel1.Location = new System.Drawing.Point(2, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1228, 66);
            this.panel1.TabIndex = 1;
            // 
            // cbRMBalance
            // 
            this.cbRMBalance.AutoSize = true;
            this.cbRMBalance.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRMBalance.Location = new System.Drawing.Point(153, 26);
            this.cbRMBalance.Name = "cbRMBalance";
            this.cbRMBalance.Size = new System.Drawing.Size(15, 14);
            this.cbRMBalance.TabIndex = 20;
            this.cbRMBalance.UseVisualStyleBackColor = true;
            this.cbRMBalance.CheckedChanged += new System.EventHandler(this.cbRMBalance_CheckedChanged);
            // 
            // cbRMPurchase
            // 
            this.cbRMPurchase.AutoSize = true;
            this.cbRMPurchase.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRMPurchase.Location = new System.Drawing.Point(31, 26);
            this.cbRMPurchase.Name = "cbRMPurchase";
            this.cbRMPurchase.Size = new System.Drawing.Size(15, 14);
            this.cbRMPurchase.TabIndex = 19;
            this.cbRMPurchase.UseVisualStyleBackColor = true;
            this.cbRMPurchase.CheckedChanged += new System.EventHandler(this.cbRMPurchase_CheckedChanged);
            // 
            // cmbRMCustomsCode
            // 
            this.cmbRMCustomsCode.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRMCustomsCode.FormattingEnabled = true;
            this.cmbRMCustomsCode.Location = new System.Drawing.Point(606, 21);
            this.cmbRMCustomsCode.Name = "cmbRMCustomsCode";
            this.cmbRMCustomsCode.Size = new System.Drawing.Size(121, 25);
            this.cmbRMCustomsCode.TabIndex = 18;
            this.cmbRMCustomsCode.Enter += new System.EventHandler(this.cmbRMCustomsCode_Enter);
            // 
            // cmbItemNo
            // 
            this.cmbItemNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemNo.FormattingEnabled = true;
            this.cmbItemNo.Location = new System.Drawing.Point(339, 21);
            this.cmbItemNo.Name = "cmbItemNo";
            this.cmbItemNo.Size = new System.Drawing.Size(121, 25);
            this.cmbItemNo.TabIndex = 17;
            this.cmbItemNo.SelectedIndexChanged += new System.EventHandler(this.cmbItemNo_SelectedIndexChanged);
            this.cmbItemNo.Enter += new System.EventHandler(this.cmbItemNo_Enter);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(869, 17);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 32);
            this.btnDownload.TabIndex = 15;
            this.btnDownload.Text = " Download";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPreview.Location = new System.Drawing.Point(754, 17);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(100, 32);
            this.btnPreview.TabIndex = 14;
            this.btnPreview.Text = "   Preview";
            this.btnPreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // fakeLabel1
            // 
            this.fakeLabel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.fakeLabel1.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(193)))), ((int)(((byte)(140)))));
            this.fakeLabel1.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel1.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel1.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel1.CImage = null;
            this.fakeLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fakeLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.fakeLabel1.LabelText = "           RM Purchase         RM Balance        Item No                         " +
    "             RM Customs Code";
            this.fakeLabel1.Location = new System.Drawing.Point(0, 0);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1228, 66);
            this.fakeLabel1.TabIndex = 1;
            // 
            // RMUnionQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 685);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RMUnionQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RM Union Query";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RMUnionQuery_FormClosing);
            this.Load += new System.EventHandler(this.RMUnionQuery_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMUnionQuery)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvRMUnionQuery;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnPreview;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.ComboBox cmbRMCustomsCode;
        private System.Windows.Forms.ComboBox cmbItemNo;
        private System.Windows.Forms.CheckBox cbRMBalance;
        private System.Windows.Forms.CheckBox cbRMPurchase;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
    }
}