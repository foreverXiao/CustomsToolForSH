namespace eCustoms
{
    partial class GetBomDocumentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetBomDocumentForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.btnUpdateBOMEHB = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBOMEHB = new System.Windows.Forms.TextBox();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnConsumption = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvConsumption = new System.Windows.Forms.DataGridView();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvOriginalGoods = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.llblCheck = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsumption)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOriginalGoods)).BeginInit();
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
            this.groupBox1.Size = new System.Drawing.Size(210, 442);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.btnOriginalGoods);
            this.panel4.Location = new System.Drawing.Point(2, 216);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(206, 62);
            this.panel4.TabIndex = 3;
            // 
            // btnOriginalGoods
            // 
            this.btnOriginalGoods.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnOriginalGoods.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnOriginalGoods.Image = ((System.Drawing.Image)(resources.GetObject("btnOriginalGoods.Image")));
            this.btnOriginalGoods.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnOriginalGoods.Location = new System.Drawing.Point(39, 5);
            this.btnOriginalGoods.Name = "btnOriginalGoods";
            this.btnOriginalGoods.Size = new System.Drawing.Size(125, 49);
            this.btnOriginalGoods.TabIndex = 1;
            this.btnOriginalGoods.Text = " Original Goods  原始货物";
            this.btnOriginalGoods.UseVisualStyleBackColor = true;
            this.btnOriginalGoods.Click += new System.EventHandler(this.btnOriginalGoods_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnApprove);
            this.panel3.Location = new System.Drawing.Point(2, 385);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(206, 54);
            this.panel3.TabIndex = 2;
            // 
            // btnApprove
            // 
            this.btnApprove.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApprove.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnApprove.Image = ((System.Drawing.Image)(resources.GetObject("btnApprove.Image")));
            this.btnApprove.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnApprove.Location = new System.Drawing.Point(39, 6);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(125, 40);
            this.btnApprove.TabIndex = 7;
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
            this.panel2.Location = new System.Drawing.Point(2, 279);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 106);
            this.panel2.TabIndex = 1;
            // 
            // rbtnOriginalGoods
            // 
            this.rbtnOriginalGoods.AutoSize = true;
            this.rbtnOriginalGoods.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtnOriginalGoods.ForeColor = System.Drawing.Color.Navy;
            this.rbtnOriginalGoods.Location = new System.Drawing.Point(25, 23);
            this.rbtnOriginalGoods.Name = "rbtnOriginalGoods";
            this.rbtnOriginalGoods.Size = new System.Drawing.Size(153, 21);
            this.rbtnOriginalGoods.TabIndex = 3;
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
            this.btnDownload.Location = new System.Drawing.Point(39, 59);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(125, 40);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.groupBox5.Location = new System.Drawing.Point(0, 45);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(202, 7);
            this.groupBox5.TabIndex = 0;
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
            this.rbtnConsumption.TabIndex = 2;
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
            this.panel1.Size = new System.Drawing.Size(206, 206);
            this.panel1.TabIndex = 0;
            // 
            // btnUpdateBOMEHB
            // 
            this.btnUpdateBOMEHB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateBOMEHB.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpdateBOMEHB.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateBOMEHB.Image")));
            this.btnUpdateBOMEHB.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdateBOMEHB.Location = new System.Drawing.Point(39, 158);
            this.btnUpdateBOMEHB.Name = "btnUpdateBOMEHB";
            this.btnUpdateBOMEHB.Size = new System.Drawing.Size(125, 40);
            this.btnUpdateBOMEHB.TabIndex = 6;
            this.btnUpdateBOMEHB.Text = "Update EHB";
            this.btnUpdateBOMEHB.UseVisualStyleBackColor = true;
            this.btnUpdateBOMEHB.Click += new System.EventHandler(this.btnUpdateBOMEHB_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(7, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "BOM In Customs";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(7, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Batch No";
            // 
            // txtBOMEHB
            // 
            this.txtBOMEHB.Location = new System.Drawing.Point(11, 132);
            this.txtBOMEHB.Name = "txtBOMEHB";
            this.txtBOMEHB.Size = new System.Drawing.Size(180, 20);
            this.txtBOMEHB.TabIndex = 3;
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Location = new System.Drawing.Point(75, 92);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(116, 20);
            this.txtBatchNo.TabIndex = 2;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.groupBox6.Location = new System.Drawing.Point(0, 78);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(202, 7);
            this.groupBox6.TabIndex = 1;
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
            this.btnConsumption.Size = new System.Drawing.Size(125, 49);
            this.btnConsumption.TabIndex = 0;
            this.btnConsumption.Text = " Consumption   单  耗";
            this.btnConsumption.UseVisualStyleBackColor = true;
            this.btnConsumption.Click += new System.EventHandler(this.btnConsumption_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvConsumption);
            this.groupBox3.Location = new System.Drawing.Point(224, -5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1008, 442);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // dgvConsumption
            // 
            this.dgvConsumption.AllowUserToAddRows = false;
            this.dgvConsumption.AllowUserToDeleteRows = false;
            this.dgvConsumption.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvConsumption.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvConsumption.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvConsumption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvConsumption.DefaultCellStyle = dataGridViewCellStyle21;
            this.dgvConsumption.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvConsumption.Location = new System.Drawing.Point(4, 12);
            this.dgvConsumption.Name = "dgvConsumption";
            this.dgvConsumption.ReadOnly = true;
            this.dgvConsumption.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvConsumption.RowTemplate.Height = 23;
            this.dgvConsumption.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConsumption.Size = new System.Drawing.Size(1000, 426);
            this.dgvConsumption.TabIndex = 0;
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
            this.fakeLabel2.Location = new System.Drawing.Point(2, 437);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1230, 11);
            this.fakeLabel2.TabIndex = 4;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgvOriginalGoods);
            this.groupBox4.Location = new System.Drawing.Point(2, 442);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1230, 244);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            // 
            // dgvOriginalGoods
            // 
            this.dgvOriginalGoods.AllowUserToAddRows = false;
            this.dgvOriginalGoods.AllowUserToDeleteRows = false;
            this.dgvOriginalGoods.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvOriginalGoods.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvOriginalGoods.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOriginalGoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOriginalGoods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck});
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOriginalGoods.DefaultCellStyle = dataGridViewCellStyle22;
            this.dgvOriginalGoods.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvOriginalGoods.Location = new System.Drawing.Point(4, 12);
            this.dgvOriginalGoods.Name = "dgvOriginalGoods";
            this.dgvOriginalGoods.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvOriginalGoods.RowTemplate.Height = 23;
            this.dgvOriginalGoods.Size = new System.Drawing.Size(1222, 228);
            this.dgvOriginalGoods.TabIndex = 0;
            this.dgvOriginalGoods.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOriginalGoods_CellMouseUp);
            this.dgvOriginalGoods.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOriginalGoods_ColumnHeaderMouseClick);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "全选";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 37;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(185)))), ((int)(((byte)(100)))));
            this.groupBox2.Location = new System.Drawing.Point(213, -9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(10, 446);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // llblCheck
            // 
            this.llblCheck.AutoSize = true;
            this.llblCheck.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.llblCheck.LinkColor = System.Drawing.Color.Navy;
            this.llblCheck.Location = new System.Drawing.Point(31, 59);
            this.llblCheck.Name = "llblCheck";
            this.llblCheck.Size = new System.Drawing.Size(141, 17);
            this.llblCheck.TabIndex = 7;
            this.llblCheck.TabStop = true;
            this.llblCheck.Text = "Check Duplicate BOM";
            this.llblCheck.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblCheck_LinkClicked);
            // 
            // GetCustomsBomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 687);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetCustomsBomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate BOM Document";
            this.groupBox1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsumption)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOriginalGoods)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnConsumption;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvConsumption;
        private System.Windows.Forms.DataGridView dgvOriginalGoods;
        private System.Windows.Forms.RadioButton rbtnOriginalGoods;
        private System.Windows.Forms.RadioButton rbtnConsumption;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnOriginalGoods;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtBOMEHB;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdateBOMEHB;
        private System.Windows.Forms.LinkLabel llblCheck;
    }
}