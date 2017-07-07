namespace eCustoms
{
    partial class GetBomReporForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetBomReporForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnOriginalGoods = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnApprove = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbtnOriginalGoods = new System.Windows.Forms.RadioButton();
            this.btnDownload = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbtnConsumption = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnConsumption = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvConsmpt = new System.Windows.Forms.DataGridView();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvOrgGoods = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.btnUpdateBOMEHB = new System.Windows.Forms.Button();
            this.txtBOMEHB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.llblCheck = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsmpt)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgGoods)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(2, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 431);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.btnOriginalGoods);
            this.panel4.Location = new System.Drawing.Point(2, 212);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(206, 56);
            this.panel4.TabIndex = 11;
            // 
            // btnOriginalGoods
            // 
            this.btnOriginalGoods.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnOriginalGoods.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnOriginalGoods.Image = ((System.Drawing.Image)(resources.GetObject("btnOriginalGoods.Image")));
            this.btnOriginalGoods.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnOriginalGoods.Location = new System.Drawing.Point(39, 5);
            this.btnOriginalGoods.Name = "btnOriginalGoods";
            this.btnOriginalGoods.Size = new System.Drawing.Size(125, 44);
            this.btnOriginalGoods.TabIndex = 10;
            this.btnOriginalGoods.Text = " Original Goods  原始货物";
            this.btnOriginalGoods.UseVisualStyleBackColor = true;
            this.btnOriginalGoods.Click += new System.EventHandler(this.btnOriginalGoods_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnApprove);
            this.panel3.Location = new System.Drawing.Point(2, 372);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(206, 56);
            this.panel3.TabIndex = 18;
            // 
            // btnApprove
            // 
            this.btnApprove.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApprove.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnApprove.Image = ((System.Drawing.Image)(resources.GetObject("btnApprove.Image")));
            this.btnApprove.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnApprove.Location = new System.Drawing.Point(39, 7);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(125, 40);
            this.btnApprove.TabIndex = 17;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.rbtnOriginalGoods);
            this.panel2.Controls.Add(this.btnDownload);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this.rbtnConsumption);
            this.panel2.Location = new System.Drawing.Point(2, 268);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 104);
            this.panel2.TabIndex = 16;
            // 
            // rbtnOriginalGoods
            // 
            this.rbtnOriginalGoods.AutoSize = true;
            this.rbtnOriginalGoods.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtnOriginalGoods.ForeColor = System.Drawing.Color.Navy;
            this.rbtnOriginalGoods.Location = new System.Drawing.Point(25, 23);
            this.rbtnOriginalGoods.Name = "rbtnOriginalGoods";
            this.rbtnOriginalGoods.Size = new System.Drawing.Size(153, 21);
            this.rbtnOriginalGoods.TabIndex = 13;
            this.rbtnOriginalGoods.TabStop = true;
            this.rbtnOriginalGoods.Text = "Select Original Goods";
            this.rbtnOriginalGoods.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(39, 55);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(125, 40);
            this.btnDownload.TabIndex = 15;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.groupBox5.Location = new System.Drawing.Point(0, 43);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(202, 7);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            // 
            // rbtnConsumption
            // 
            this.rbtnConsumption.AutoSize = true;
            this.rbtnConsumption.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnConsumption.ForeColor = System.Drawing.Color.Navy;
            this.rbtnConsumption.Location = new System.Drawing.Point(25, 2);
            this.rbtnConsumption.Name = "rbtnConsumption";
            this.rbtnConsumption.Size = new System.Drawing.Size(141, 21);
            this.rbtnConsumption.TabIndex = 12;
            this.rbtnConsumption.TabStop = true;
            this.rbtnConsumption.Text = "Select Consumption";
            this.rbtnConsumption.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.llblCheck);
            this.panel1.Controls.Add(this.btnUpdateBOMEHB);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtBOMEHB);
            this.panel1.Controls.Add(this.txtBatchNo);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.btnConsumption);
            this.panel1.Location = new System.Drawing.Point(2, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(206, 203);
            this.panel1.TabIndex = 9;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.groupBox6.Location = new System.Drawing.Point(0, 76);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(202, 7);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            // 
            // btnConsumption
            // 
            this.btnConsumption.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnConsumption.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnConsumption.Image = ((System.Drawing.Image)(resources.GetObject("btnConsumption.Image")));
            this.btnConsumption.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnConsumption.Location = new System.Drawing.Point(39, 7);
            this.btnConsumption.Name = "btnConsumption";
            this.btnConsumption.Size = new System.Drawing.Size(125, 44);
            this.btnConsumption.TabIndex = 2;
            this.btnConsumption.Text = " Consumption   单  耗";
            this.btnConsumption.UseVisualStyleBackColor = true;
            this.btnConsumption.Click += new System.EventHandler(this.btnConsumption_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvConsmpt);
            this.groupBox3.Location = new System.Drawing.Point(224, -5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1008, 431);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            // 
            // dgvConsmpt
            // 
            this.dgvConsmpt.AllowUserToAddRows = false;
            this.dgvConsmpt.AllowUserToDeleteRows = false;
            this.dgvConsmpt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvConsmpt.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvConsmpt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvConsmpt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvConsmpt.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvConsmpt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConsmpt.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvConsmpt.Location = new System.Drawing.Point(3, 16);
            this.dgvConsmpt.Name = "dgvConsmpt";
            this.dgvConsmpt.ReadOnly = true;
            this.dgvConsmpt.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvConsmpt.RowTemplate.Height = 23;
            this.dgvConsmpt.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConsmpt.Size = new System.Drawing.Size(1002, 412);
            this.dgvConsmpt.TabIndex = 21;
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
            this.fakeLabel2.Location = new System.Drawing.Point(1, 426);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel2.TabIndex = 23;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgvOrgGoods);
            this.groupBox4.Location = new System.Drawing.Point(2, 431);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1230, 232);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            // 
            // dgvOrgGoods
            // 
            this.dgvOrgGoods.AllowUserToAddRows = false;
            this.dgvOrgGoods.AllowUserToDeleteRows = false;
            this.dgvOrgGoods.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvOrgGoods.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvOrgGoods.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrgGoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOrgGoods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrgGoods.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOrgGoods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrgGoods.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvOrgGoods.Location = new System.Drawing.Point(3, 16);
            this.dgvOrgGoods.Name = "dgvOrgGoods";
            this.dgvOrgGoods.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvOrgGoods.RowTemplate.Height = 23;
            this.dgvOrgGoods.Size = new System.Drawing.Size(1224, 213);
            this.dgvOrgGoods.TabIndex = 24;
            this.dgvOrgGoods.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOrgGoods_CellMouseUp);
            this.dgvOrgGoods.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOrgGoods_ColumnHeaderMouseClick);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "Select";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 43;
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
            this.fakeLabel1.Location = new System.Drawing.Point(212, -5);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(11, 431);
            this.fakeLabel1.TabIndex = 20;
            // 
            // btnUpdateBOMEHB
            // 
            this.btnUpdateBOMEHB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateBOMEHB.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpdateBOMEHB.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateBOMEHB.Image")));
            this.btnUpdateBOMEHB.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdateBOMEHB.Location = new System.Drawing.Point(39, 155);
            this.btnUpdateBOMEHB.Name = "btnUpdateBOMEHB";
            this.btnUpdateBOMEHB.Size = new System.Drawing.Size(125, 40);
            this.btnUpdateBOMEHB.TabIndex = 8;
            this.btnUpdateBOMEHB.Text = "Update EHB";
            this.btnUpdateBOMEHB.UseVisualStyleBackColor = true;
            this.btnUpdateBOMEHB.Click += new System.EventHandler(this.btnUpdateBOMEHB_Click);
            // 
            // txtBOMEHB
            // 
            this.txtBOMEHB.Location = new System.Drawing.Point(11, 130);
            this.txtBOMEHB.Name = "txtBOMEHB";
            this.txtBOMEHB.Size = new System.Drawing.Size(180, 20);
            this.txtBOMEHB.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(7, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "BOM In Customs";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(7, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Batch No";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Location = new System.Drawing.Point(75, 90);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(116, 20);
            this.txtBatchNo.TabIndex = 5;
            // 
            // llblCheck
            // 
            this.llblCheck.AutoSize = true;
            this.llblCheck.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.llblCheck.LinkColor = System.Drawing.Color.Navy;
            this.llblCheck.Location = new System.Drawing.Point(32, 56);
            this.llblCheck.Name = "llblCheck";
            this.llblCheck.Size = new System.Drawing.Size(141, 17);
            this.llblCheck.TabIndex = 1;
            this.llblCheck.TabStop = true;
            this.llblCheck.Text = "Check Duplicate BOM";
            this.llblCheck.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblCheck_LinkClicked);
            // 
            // GetBomReporForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 665);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetBomReporForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get BOM Document Operation Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetBomReporForm_FormClosing);
            this.Load += new System.EventHandler(this.GetBomReporForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsmpt)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgGoods)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnConsumption;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvConsmpt;
        private System.Windows.Forms.DataGridView dgvOrgGoods;
        private System.Windows.Forms.RadioButton rbtnOriginalGoods;
        private System.Windows.Forms.RadioButton rbtnConsumption;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnOriginalGoods;
        private System.Windows.Forms.GroupBox groupBox6;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.Button btnUpdateBOMEHB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBOMEHB;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.LinkLabel llblCheck;
    }
}