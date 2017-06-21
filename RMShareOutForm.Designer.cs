namespace SHCustomsSystem
{
    partial class RMShareOutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMShareOutForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarAdd = new System.Windows.Forms.ToolBarButton();
            this.toolBarEdit = new System.Windows.Forms.ToolBarButton();
            this.toolBarUpdate = new System.Windows.Forms.ToolBarButton();
            this.toolBarDelete = new System.Windows.Forms.ToolBarButton();
            this.toolBarCancel = new System.Windows.Forms.ToolBarButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvRMShareOut = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvClick = new System.Windows.Forms.DataGridViewLinkColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.txtPingDanStatus = new System.Windows.Forms.TextBox();
            this.txtPingDanNo = new System.Windows.Forms.TextBox();
            this.txtPingDanID = new System.Windows.Forms.TextBox();
            this.dtpPassGateDate = new System.Windows.Forms.DateTimePicker();
            this.dtpCustomsReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.dtpBeiAnDanDate = new System.Windows.Forms.DateTimePicker();
            this.txtBeiAnDanID = new System.Windows.Forms.TextBox();
            this.txtBeiAnDanNoOut = new System.Windows.Forms.TextBox();
            this.txtNetWeight = new System.Windows.Forms.TextBox();
            this.txtRMCHNName = new System.Windows.Forms.TextBox();
            this.dtpCreatedDate = new System.Windows.Forms.DateTimePicker();
            this.txtLotNo = new System.Windows.Forms.TextBox();
            this.txtItemNo = new System.Windows.Forms.TextBox();
            this.txtBeiAnDanNoIN = new System.Windows.Forms.TextBox();
            this.txtRMCustomsCode = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRMCustomsCode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbItemNo = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMShareOut)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.cmsFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarAdd,
            this.toolBarEdit,
            this.toolBarUpdate,
            this.toolBarDelete,
            this.toolBarCancel});
            this.toolBar.DropDownArrows = true;
            this.toolBar.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(1234, 47);
            this.toolBar.TabIndex = 1;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            // 
            // toolBarAdd
            // 
            this.toolBarAdd.ImageIndex = 0;
            this.toolBarAdd.Name = "toolBarAdd";
            this.toolBarAdd.Pushed = true;
            this.toolBarAdd.Text = "新增";
            this.toolBarAdd.ToolTipText = "Add";
            // 
            // toolBarEdit
            // 
            this.toolBarEdit.ImageIndex = 1;
            this.toolBarEdit.Name = "toolBarEdit";
            this.toolBarEdit.Pushed = true;
            this.toolBarEdit.Text = "编辑";
            this.toolBarEdit.ToolTipText = "Edit";
            // 
            // toolBarUpdate
            // 
            this.toolBarUpdate.ImageIndex = 2;
            this.toolBarUpdate.Name = "toolBarUpdate";
            this.toolBarUpdate.Pushed = true;
            this.toolBarUpdate.Text = "保存";
            this.toolBarUpdate.ToolTipText = "Update";
            // 
            // toolBarDelete
            // 
            this.toolBarDelete.ImageIndex = 3;
            this.toolBarDelete.Name = "toolBarDelete";
            this.toolBarDelete.Pushed = true;
            this.toolBarDelete.Text = "删除";
            this.toolBarDelete.ToolTipText = "Delete";
            // 
            // toolBarCancel
            // 
            this.toolBarCancel.ImageIndex = 4;
            this.toolBarCancel.Name = "toolBarCancel";
            this.toolBarCancel.Pushed = true;
            this.toolBarCancel.Text = "撤销";
            this.toolBarCancel.ToolTipText = "Cancel";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Add.ico");
            this.imageList.Images.SetKeyName(1, "Edit.ico");
            this.imageList.Images.SetKeyName(2, "Save.ico");
            this.imageList.Images.SetKeyName(3, "Delete.ico");
            this.imageList.Images.SetKeyName(4, "Cancel.ico");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvRMShareOut);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(2, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1230, 437);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // dgvRMShareOut
            // 
            this.dgvRMShareOut.AllowUserToAddRows = false;
            this.dgvRMShareOut.AllowUserToDeleteRows = false;
            this.dgvRMShareOut.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRMShareOut.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvRMShareOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRMShareOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRMShareOut.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.dgvClick});
            this.dgvRMShareOut.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRMShareOut.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRMShareOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRMShareOut.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvRMShareOut.Location = new System.Drawing.Point(3, 19);
            this.dgvRMShareOut.Name = "dgvRMShareOut";
            this.dgvRMShareOut.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRMShareOut.Size = new System.Drawing.Size(1224, 415);
            this.dgvRMShareOut.TabIndex = 0;
            this.dgvRMShareOut.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRMShareOut_CellMouseClick);
            this.dgvRMShareOut.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRMShareOut_ColumnHeaderMouseClick);
            this.dgvRMShareOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvRMShareOut_MouseUp);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "全选";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 38;
            // 
            // dgvClick
            // 
            this.dgvClick.HeaderText = "";
            this.dgvClick.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.dgvClick.Name = "dgvClick";
            this.dgvClick.Text = "Click";
            this.dgvClick.UseColumnTextForLinkValue = true;
            this.dgvClick.Width = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.txtRemark);
            this.groupBox2.Controls.Add(this.txtPingDanStatus);
            this.groupBox2.Controls.Add(this.txtPingDanNo);
            this.groupBox2.Controls.Add(this.txtPingDanID);
            this.groupBox2.Controls.Add(this.dtpPassGateDate);
            this.groupBox2.Controls.Add(this.dtpCustomsReleaseDate);
            this.groupBox2.Controls.Add(this.dtpBeiAnDanDate);
            this.groupBox2.Controls.Add(this.txtBeiAnDanID);
            this.groupBox2.Controls.Add(this.txtBeiAnDanNoOut);
            this.groupBox2.Controls.Add(this.txtNetWeight);
            this.groupBox2.Controls.Add(this.txtRMCHNName);
            this.groupBox2.Controls.Add(this.dtpCreatedDate);
            this.groupBox2.Controls.Add(this.txtLotNo);
            this.groupBox2.Controls.Add(this.txtItemNo);
            this.groupBox2.Controls.Add(this.txtBeiAnDanNoIN);
            this.groupBox2.Controls.Add(this.txtRMCustomsCode);
            this.groupBox2.Controls.Add(this.cmbType);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(2, 484);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1230, 200);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.ForeColor = System.Drawing.Color.Red;
            this.label27.Location = new System.Drawing.Point(335, 105);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(14, 17);
            this.label27.TabIndex = 41;
            this.label27.Text = "*";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(851, 105);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(14, 17);
            this.label22.TabIndex = 40;
            this.label22.Text = "*";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(508, 50);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(14, 17);
            this.label26.TabIndex = 39;
            this.label26.Text = "*";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(1021, 50);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(14, 17);
            this.label25.TabIndex = 38;
            this.label25.Text = "*";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.ForeColor = System.Drawing.Color.Red;
            this.label24.Location = new System.Drawing.Point(851, 50);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(14, 17);
            this.label24.TabIndex = 37;
            this.label24.Text = "*";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.ForeColor = System.Drawing.Color.Red;
            this.label23.Location = new System.Drawing.Point(680, 50);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(14, 17);
            this.label23.TabIndex = 36;
            this.label23.Text = "*";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(335, 50);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(14, 17);
            this.label21.TabIndex = 35;
            this.label21.Text = "*";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(165, 50);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 17);
            this.label20.TabIndex = 34;
            this.label20.Text = "*";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(538, 156);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(652, 23);
            this.txtRemark.TabIndex = 33;
            // 
            // txtPingDanStatus
            // 
            this.txtPingDanStatus.Location = new System.Drawing.Point(193, 156);
            this.txtPingDanStatus.Name = "txtPingDanStatus";
            this.txtPingDanStatus.Size = new System.Drawing.Size(140, 23);
            this.txtPingDanStatus.TabIndex = 32;
            // 
            // txtPingDanNo
            // 
            this.txtPingDanNo.Location = new System.Drawing.Point(23, 156);
            this.txtPingDanNo.Name = "txtPingDanNo";
            this.txtPingDanNo.Size = new System.Drawing.Size(140, 23);
            this.txtPingDanNo.TabIndex = 31;
            // 
            // txtPingDanID
            // 
            this.txtPingDanID.Location = new System.Drawing.Point(1050, 102);
            this.txtPingDanID.Name = "txtPingDanID";
            this.txtPingDanID.Size = new System.Drawing.Size(140, 23);
            this.txtPingDanID.TabIndex = 30;
            // 
            // dtpPassGateDate
            // 
            this.dtpPassGateDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPassGateDate.CustomFormat = "";
            this.dtpPassGateDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPassGateDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPassGateDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpPassGateDate.Location = new System.Drawing.Point(366, 156);
            this.dtpPassGateDate.Name = "dtpPassGateDate";
            this.dtpPassGateDate.Size = new System.Drawing.Size(140, 23);
            this.dtpPassGateDate.TabIndex = 29;
            this.dtpPassGateDate.ValueChanged += new System.EventHandler(this.dtpPassGateDate_ValueChanged);
            this.dtpPassGateDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpPassGateDate_KeyUp);
            // 
            // dtpCustomsReleaseDate
            // 
            this.dtpCustomsReleaseDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpCustomsReleaseDate.CustomFormat = "";
            this.dtpCustomsReleaseDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpCustomsReleaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCustomsReleaseDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpCustomsReleaseDate.Location = new System.Drawing.Point(879, 102);
            this.dtpCustomsReleaseDate.Name = "dtpCustomsReleaseDate";
            this.dtpCustomsReleaseDate.Size = new System.Drawing.Size(140, 23);
            this.dtpCustomsReleaseDate.TabIndex = 28;
            this.dtpCustomsReleaseDate.ValueChanged += new System.EventHandler(this.dtpCustomsReleaseDate_ValueChanged);
            this.dtpCustomsReleaseDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpCustomsReleaseDate_KeyUp);
            // 
            // dtpBeiAnDanDate
            // 
            this.dtpBeiAnDanDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpBeiAnDanDate.CustomFormat = "";
            this.dtpBeiAnDanDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpBeiAnDanDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBeiAnDanDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpBeiAnDanDate.Location = new System.Drawing.Point(709, 102);
            this.dtpBeiAnDanDate.Name = "dtpBeiAnDanDate";
            this.dtpBeiAnDanDate.Size = new System.Drawing.Size(140, 23);
            this.dtpBeiAnDanDate.TabIndex = 27;
            this.dtpBeiAnDanDate.ValueChanged += new System.EventHandler(this.dtpBeiAnDanDate_ValueChanged);
            this.dtpBeiAnDanDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpBeiAnDanDate_KeyUp);
            // 
            // txtBeiAnDanID
            // 
            this.txtBeiAnDanID.Location = new System.Drawing.Point(538, 102);
            this.txtBeiAnDanID.Name = "txtBeiAnDanID";
            this.txtBeiAnDanID.Size = new System.Drawing.Size(140, 23);
            this.txtBeiAnDanID.TabIndex = 26;
            // 
            // txtBeiAnDanNoOut
            // 
            this.txtBeiAnDanNoOut.Location = new System.Drawing.Point(366, 102);
            this.txtBeiAnDanNoOut.Name = "txtBeiAnDanNoOut";
            this.txtBeiAnDanNoOut.Size = new System.Drawing.Size(140, 23);
            this.txtBeiAnDanNoOut.TabIndex = 25;
            // 
            // txtNetWeight
            // 
            this.txtNetWeight.Location = new System.Drawing.Point(193, 102);
            this.txtNetWeight.Name = "txtNetWeight";
            this.txtNetWeight.Size = new System.Drawing.Size(140, 23);
            this.txtNetWeight.TabIndex = 24;
            // 
            // txtRMCHNName
            // 
            this.txtRMCHNName.Location = new System.Drawing.Point(23, 102);
            this.txtRMCHNName.Name = "txtRMCHNName";
            this.txtRMCHNName.Size = new System.Drawing.Size(140, 23);
            this.txtRMCHNName.TabIndex = 23;
            // 
            // dtpCreatedDate
            // 
            this.dtpCreatedDate.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpCreatedDate.CustomFormat = "";
            this.dtpCreatedDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpCreatedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCreatedDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpCreatedDate.Location = new System.Drawing.Point(879, 45);
            this.dtpCreatedDate.Name = "dtpCreatedDate";
            this.dtpCreatedDate.Size = new System.Drawing.Size(140, 23);
            this.dtpCreatedDate.TabIndex = 22;
            this.dtpCreatedDate.ValueChanged += new System.EventHandler(this.dtpCreatedDate_ValueChanged);
            this.dtpCreatedDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpCreatedDate_KeyUp);
            // 
            // txtLotNo
            // 
            this.txtLotNo.Location = new System.Drawing.Point(709, 45);
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.Size = new System.Drawing.Size(140, 23);
            this.txtLotNo.TabIndex = 21;
            // 
            // txtItemNo
            // 
            this.txtItemNo.Location = new System.Drawing.Point(538, 45);
            this.txtItemNo.Name = "txtItemNo";
            this.txtItemNo.Size = new System.Drawing.Size(140, 23);
            this.txtItemNo.TabIndex = 20;
            // 
            // txtBeiAnDanNoIN
            // 
            this.txtBeiAnDanNoIN.Location = new System.Drawing.Point(366, 45);
            this.txtBeiAnDanNoIN.Name = "txtBeiAnDanNoIN";
            this.txtBeiAnDanNoIN.Size = new System.Drawing.Size(140, 23);
            this.txtBeiAnDanNoIN.TabIndex = 19;
            // 
            // txtRMCustomsCode
            // 
            this.txtRMCustomsCode.Location = new System.Drawing.Point(193, 45);
            this.txtRMCustomsCode.Name = "txtRMCustomsCode";
            this.txtRMCustomsCode.Size = new System.Drawing.Size(140, 23);
            this.txtRMCustomsCode.TabIndex = 18;
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "",
            "料件退仓",
            "料件退运"});
            this.cmbType.Location = new System.Drawing.Point(23, 44);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(140, 25);
            this.cmbType.TabIndex = 17;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label19.Location = new System.Drawing.Point(538, 136);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(54, 17);
            this.label19.TabIndex = 16;
            this.label19.Text = "Remark";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label18.Location = new System.Drawing.Point(879, 23);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(88, 17);
            this.label18.TabIndex = 15;
            this.label18.Text = "Created Date";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label17.Location = new System.Drawing.Point(366, 136);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 17);
            this.label17.TabIndex = 14;
            this.label17.Text = "Pass Gate Date";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label16.Location = new System.Drawing.Point(193, 136);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(103, 17);
            this.label16.TabIndex = 13;
            this.label16.Text = "PingDan Status";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label15.Location = new System.Drawing.Point(23, 136);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 17);
            this.label15.TabIndex = 12;
            this.label15.Text = "PingDan No";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label14.Location = new System.Drawing.Point(1050, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 17);
            this.label14.TabIndex = 11;
            this.label14.Text = "PingDan ID";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label13.Location = new System.Drawing.Point(879, 80);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "Customs Release Date";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label12.Location = new System.Drawing.Point(709, 80);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(102, 17);
            this.label12.TabIndex = 9;
            this.label12.Text = "BeiAnDan Date";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label11.Location = new System.Drawing.Point(538, 80);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 17);
            this.label11.TabIndex = 8;
            this.label11.Text = "BeiAnDan ID";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label10.Location = new System.Drawing.Point(366, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(127, 17);
            this.label10.TabIndex = 7;
            this.label10.Text = "Exit - BeiAnDan No";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label9.Location = new System.Drawing.Point(193, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 17);
            this.label9.TabIndex = 6;
            this.label9.Text = "Net Weight";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label8.Location = new System.Drawing.Point(23, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 17);
            this.label8.TabIndex = 5;
            this.label8.Text = "RM CHN Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label7.Location = new System.Drawing.Point(709, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Lot No";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label6.Location = new System.Drawing.Point(538, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Item No";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label5.Location = new System.Drawing.Point(366, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Enter - BeiAnDan No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label4.Location = new System.Drawing.Point(193, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "RM Customs Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(23, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Type";
            // 
            // fakeLabel1
            // 
            this.fakeLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.fakeLabel1.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(193)))), ((int)(((byte)(140)))));
            this.fakeLabel1.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel1.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel1.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel1.CImage = null;
            this.fakeLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.fakeLabel1.LabelText = "";
            this.fakeLabel1.Location = new System.Drawing.Point(2, 481);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1230, 11);
            this.fakeLabel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(305, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "RM Customs Code";
            // 
            // cmbRMCustomsCode
            // 
            this.cmbRMCustomsCode.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRMCustomsCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbRMCustomsCode.FormattingEnabled = true;
            this.cmbRMCustomsCode.Location = new System.Drawing.Point(427, 16);
            this.cmbRMCustomsCode.Name = "cmbRMCustomsCode";
            this.cmbRMCustomsCode.Size = new System.Drawing.Size(100, 25);
            this.cmbRMCustomsCode.TabIndex = 6;
            this.cmbRMCustomsCode.SelectedIndexChanged += new System.EventHandler(this.cmbRMCustomsCode_SelectedIndexChanged);
            this.cmbRMCustomsCode.Enter += new System.EventHandler(this.cmbRMCustomsCode_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(554, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Item No";
            // 
            // cmbItemNo
            // 
            this.cmbItemNo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemNo.FormattingEnabled = true;
            this.cmbItemNo.Location = new System.Drawing.Point(614, 16);
            this.cmbItemNo.Name = "cmbItemNo";
            this.cmbItemNo.Size = new System.Drawing.Size(121, 25);
            this.cmbItemNo.TabIndex = 8;
            this.cmbItemNo.Enter += new System.EventHandler(this.cmbItemNo_Enter);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowse.Location = new System.Drawing.Point(765, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 30);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "    Preview";
            this.btnBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(893, 12);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 30);
            this.btnDownload.TabIndex = 10;
            this.btnDownload.Text = " Download";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // cmsFilter
            // 
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(144, 92);
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
            // tsmiRecordFilter
            // 
            this.tsmiRecordFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRecordFilter.Image")));
            this.tsmiRecordFilter.Name = "tsmiRecordFilter";
            this.tsmiRecordFilter.Size = new System.Drawing.Size(143, 22);
            this.tsmiRecordFilter.Text = "Record Filter";
            this.tsmiRecordFilter.Click += new System.EventHandler(this.tsmiRecordFilter_Click);
            // 
            // RMShareOutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 685);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.cmbItemNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbRMCustomsCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RMShareOutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RM Share Out ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RMShareOutForm_FormClosing);
            this.Load += new System.EventHandler(this.RMShareOutForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMShareOut)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.cmsFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolBar toolBar;
        private System.Windows.Forms.ToolBarButton toolBarDelete;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.DataGridView dgvRMShareOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRMCustomsCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbItemNo;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtLotNo;
        private System.Windows.Forms.TextBox txtItemNo;
        private System.Windows.Forms.TextBox txtBeiAnDanNoIN;
        private System.Windows.Forms.TextBox txtRMCustomsCode;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.DateTimePicker dtpCreatedDate;
        private System.Windows.Forms.TextBox txtNetWeight;
        private System.Windows.Forms.TextBox txtRMCHNName;
        private System.Windows.Forms.DateTimePicker dtpBeiAnDanDate;
        private System.Windows.Forms.TextBox txtBeiAnDanID;
        private System.Windows.Forms.TextBox txtBeiAnDanNoOut;
        private System.Windows.Forms.DateTimePicker dtpPassGateDate;
        private System.Windows.Forms.DateTimePicker dtpCustomsReleaseDate;
        private System.Windows.Forms.TextBox txtPingDanNo;
        private System.Windows.Forms.TextBox txtPingDanID;
        private System.Windows.Forms.TextBox txtPingDanStatus;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewLinkColumn dgvClick;
        private System.Windows.Forms.ToolBarButton toolBarAdd;
        private System.Windows.Forms.ToolBarButton toolBarEdit;
        private System.Windows.Forms.ToolBarButton toolBarUpdate;
        private System.Windows.Forms.ToolBarButton toolBarCancel;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
    }
}