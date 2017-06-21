namespace SHCustomsSystem
{
    partial class GetBomDataForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetBomDataForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchPath = new System.Windows.Forms.TextBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnCheckPrice = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnExtract = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCheckQtyYield = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnDownload = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvBomList = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvUpdate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExceptionFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckQty = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckYield = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckPrice = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBlankFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiModifyLotNo = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.groupBox1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBomList)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel7);
            this.groupBox1.Controls.Add(this.panel5);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel6);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(2, -7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1230, 111);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel7.Controls.Add(this.btnSearch);
            this.panel7.Controls.Add(this.txtSearchPath);
            this.panel7.Controls.Add(this.btnUpload);
            this.panel7.Location = new System.Drawing.Point(903, 16);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(208, 86);
            this.panel7.TabIndex = 21;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.Location = new System.Drawing.Point(17, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 38);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "  Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchPath
            // 
            this.txtSearchPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSearchPath.ForeColor = System.Drawing.Color.Red;
            this.txtSearchPath.Location = new System.Drawing.Point(17, 10);
            this.txtSearchPath.Name = "txtSearchPath";
            this.txtSearchPath.Size = new System.Drawing.Size(170, 23);
            this.txtSearchPath.TabIndex = 7;
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpload.Location = new System.Drawing.Point(107, 38);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 38);
            this.btnUpload.TabIndex = 8;
            this.btnUpload.Text = " Upload";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.btnCheckPrice);
            this.panel5.Location = new System.Drawing.Point(679, 16);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(112, 86);
            this.panel5.TabIndex = 19;
            // 
            // btnCheckPrice
            // 
            this.btnCheckPrice.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckPrice.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckPrice.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckPrice.Image")));
            this.btnCheckPrice.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCheckPrice.Location = new System.Drawing.Point(14, 9);
            this.btnCheckPrice.Name = "btnCheckPrice";
            this.btnCheckPrice.Size = new System.Drawing.Size(80, 65);
            this.btnCheckPrice.TabIndex = 4;
            this.btnCheckPrice.Text = "⒊Check";
            this.btnCheckPrice.UseVisualStyleBackColor = true;
            this.btnCheckPrice.Click += new System.EventHandler(this.btnCheckPrice_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.btnExtract);
            this.panel4.Location = new System.Drawing.Point(559, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(120, 86);
            this.panel4.TabIndex = 18;
            // 
            // btnExtract
            // 
            this.btnExtract.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnExtract.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnExtract.Image = ((System.Drawing.Image)(resources.GetObject("btnExtract.Image")));
            this.btnExtract.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnExtract.Location = new System.Drawing.Point(13, 9);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(90, 65);
            this.btnExtract.TabIndex = 3;
            this.btnExtract.Text = "Extract Combine";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnCheckQtyYield);
            this.panel3.Location = new System.Drawing.Point(447, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(112, 86);
            this.panel3.TabIndex = 17;
            // 
            // btnCheckQtyYield
            // 
            this.btnCheckQtyYield.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnCheckQtyYield.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnCheckQtyYield.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckQtyYield.Image")));
            this.btnCheckQtyYield.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCheckQtyYield.Location = new System.Drawing.Point(14, 9);
            this.btnCheckQtyYield.Name = "btnCheckQtyYield";
            this.btnCheckQtyYield.Size = new System.Drawing.Size(80, 65);
            this.btnCheckQtyYield.TabIndex = 3;
            this.btnCheckQtyYield.Text = "⒈&&⒉Check";
            this.btnCheckQtyYield.UseVisualStyleBackColor = true;
            this.btnCheckQtyYield.Click += new System.EventHandler(this.btnCheckQtyYield_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.btnDownload);
            this.panel6.Location = new System.Drawing.Point(791, 16);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(112, 86);
            this.panel6.TabIndex = 20;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDownload.Location = new System.Drawing.Point(14, 9);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(80, 65);
            this.btnDownload.TabIndex = 5;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnBrowse);
            this.panel2.Location = new System.Drawing.Point(1111, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(112, 86);
            this.panel2.TabIndex = 16;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnBrowse.Location = new System.Drawing.Point(14, 9);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 65);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Preview";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(7, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 86);
            this.panel1.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Khaki;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(321, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 16);
            this.textBox1.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(16, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(296, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "⒊Price : Total RM Cost (USD) > Total FG Value";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.LightPink;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox4.Location = new System.Drawing.Point(321, 9);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 16);
            this.textBox4.TabIndex = 13;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Aquamarine;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox3.Location = new System.Drawing.Point(321, 33);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 16);
            this.textBox3.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "⒈Qty : Total Output Qty > SUM(RM Input Qty)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(16, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(303, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "⒉Yield : Total Output Qty / SUM(RM Input Qty)";
            // 
            // dgvBomList
            // 
            this.dgvBomList.AllowUserToAddRows = false;
            this.dgvBomList.AllowUserToDeleteRows = false;
            this.dgvBomList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBomList.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvBomList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBomList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvBomList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.dgvDelete,
            this.dgvUpdate});
            this.dgvBomList.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBomList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBomList.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvBomList.Location = new System.Drawing.Point(5, 24);
            this.dgvBomList.Name = "dgvBomList";
            this.dgvBomList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBomList.RowTemplate.Height = 23;
            this.dgvBomList.Size = new System.Drawing.Size(1220, 548);
            this.dgvBomList.TabIndex = 25;
            this.dgvBomList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBomList_CellClick);
            this.dgvBomList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBomList_ColumnHeaderMouseClick);
            this.dgvBomList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvBomList_MouseUp);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "全选";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 38;
            // 
            // dgvDelete
            // 
            this.dgvDelete.HeaderText = "";
            this.dgvDelete.Name = "dgvDelete";
            this.dgvDelete.Text = "Delete";
            this.dgvDelete.UseColumnTextForButtonValue = true;
            this.dgvDelete.Width = 5;
            // 
            // dgvUpdate
            // 
            this.dgvUpdate.HeaderText = "";
            this.dgvUpdate.Name = "dgvUpdate";
            this.dgvUpdate.Text = "Update";
            this.dgvUpdate.UseColumnTextForButtonValue = true;
            this.dgvUpdate.Width = 5;
            // 
            // cmsFilter
            // 
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter,
            this.tsmiExceptionFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(155, 114);
            // 
            // tsmiChooseFilter
            // 
            this.tsmiChooseFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiChooseFilter.Image")));
            this.tsmiChooseFilter.Name = "tsmiChooseFilter";
            this.tsmiChooseFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiChooseFilter.Text = "Choose Filter";
            this.tsmiChooseFilter.Click += new System.EventHandler(this.tsmiChooseFilter_Click);
            // 
            // tsmiExcludeFilter
            // 
            this.tsmiExcludeFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExcludeFilter.Image")));
            this.tsmiExcludeFilter.Name = "tsmiExcludeFilter";
            this.tsmiExcludeFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiExcludeFilter.Text = "Exclude Filter";
            this.tsmiExcludeFilter.Click += new System.EventHandler(this.tsmiExcludeFilter_Click);
            // 
            // tsmiRefreshFilter
            // 
            this.tsmiRefreshFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRefreshFilter.Image")));
            this.tsmiRefreshFilter.Name = "tsmiRefreshFilter";
            this.tsmiRefreshFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiRefreshFilter.Text = "Refresh Filter";
            this.tsmiRefreshFilter.Click += new System.EventHandler(this.tsmiRefreshFilter_Click);
            // 
            // tsmiRecordFilter
            // 
            this.tsmiRecordFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRecordFilter.Image")));
            this.tsmiRecordFilter.Name = "tsmiRecordFilter";
            this.tsmiRecordFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiRecordFilter.Text = "Record Filter";
            this.tsmiRecordFilter.Click += new System.EventHandler(this.tsmiRecordFilter_Click);
            // 
            // tsmiExceptionFilter
            // 
            this.tsmiExceptionFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheckQty,
            this.tsmiCheckYield,
            this.tsmiCheckPrice,
            this.tsmiBlankFilter,
            this.tsmiModifyLotNo});
            this.tsmiExceptionFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExceptionFilter.Image")));
            this.tsmiExceptionFilter.Name = "tsmiExceptionFilter";
            this.tsmiExceptionFilter.Size = new System.Drawing.Size(154, 22);
            this.tsmiExceptionFilter.Text = "Exception Filter";
            // 
            // tsmiCheckQty
            // 
            this.tsmiCheckQty.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheckQty.Image")));
            this.tsmiCheckQty.Name = "tsmiCheckQty";
            this.tsmiCheckQty.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheckQty.Text = "Check 1.";
            this.tsmiCheckQty.Click += new System.EventHandler(this.tsmiCheckQty_Click);
            // 
            // tsmiCheckYield
            // 
            this.tsmiCheckYield.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheckYield.Image")));
            this.tsmiCheckYield.Name = "tsmiCheckYield";
            this.tsmiCheckYield.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheckYield.Text = "Check 2.";
            this.tsmiCheckYield.Click += new System.EventHandler(this.tsmiCheckYield_Click);
            // 
            // tsmiCheckPrice
            // 
            this.tsmiCheckPrice.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCheckPrice.Image")));
            this.tsmiCheckPrice.Name = "tsmiCheckPrice";
            this.tsmiCheckPrice.Size = new System.Drawing.Size(148, 22);
            this.tsmiCheckPrice.Text = "Check 3.";
            this.tsmiCheckPrice.Click += new System.EventHandler(this.tsmiCheckPrice_Click);
            // 
            // tsmiBlankFilter
            // 
            this.tsmiBlankFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiBlankFilter.Image")));
            this.tsmiBlankFilter.Name = "tsmiBlankFilter";
            this.tsmiBlankFilter.Size = new System.Drawing.Size(148, 22);
            this.tsmiBlankFilter.Text = "Blank Filter";
            this.tsmiBlankFilter.Click += new System.EventHandler(this.tsmiBlankFilter_Click);
            // 
            // tsmiModifyLotNo
            // 
            this.tsmiModifyLotNo.Image = ((System.Drawing.Image)(resources.GetObject("tsmiModifyLotNo.Image")));
            this.tsmiModifyLotNo.Name = "tsmiModifyLotNo";
            this.tsmiModifyLotNo.Size = new System.Drawing.Size(148, 22);
            this.tsmiModifyLotNo.Text = "Modify LotNo";
            this.tsmiModifyLotNo.Click += new System.EventHandler(this.tsmiModifyLotNo_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fakeLabel1);
            this.groupBox2.Controls.Add(this.dgvBomList);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(2, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1230, 579);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
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
            this.fakeLabel1.Location = new System.Drawing.Point(0, -2);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1230, 11);
            this.fakeLabel1.TabIndex = 23;
            // 
            // GetBomDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 685);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetBomDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get BOM Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetBomDataForm_FormClosing);
            this.Load += new System.EventHandler(this.GetBomDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBomList)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.DataGridView dgvBomList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCheckQtyYield;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnDownload;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnCheckPrice;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchPath;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDelete;
        private System.Windows.Forms.DataGridViewButtonColumn dgvUpdate;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExceptionFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckQty;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckYield;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckPrice;
        private System.Windows.Forms.ToolStripMenuItem tsmiBlankFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiModifyLotNo;
    }
}