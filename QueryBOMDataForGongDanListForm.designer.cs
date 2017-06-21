namespace SHCustomsSystem
{
    partial class QueryBOMData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryBOMData));
            this.dgvQueryBOM = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewLinkColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueryBOM)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvQueryBOM
            // 
            this.dgvQueryBOM.AllowUserToAddRows = false;
            this.dgvQueryBOM.AllowUserToDeleteRows = false;
            this.dgvQueryBOM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvQueryBOM.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvQueryBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQueryBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvQueryBOM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueryBOM.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQueryBOM.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvQueryBOM.Location = new System.Drawing.Point(0, 30);
            this.dgvQueryBOM.Name = "dgvQueryBOM";
            this.dgvQueryBOM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvQueryBOM.Size = new System.Drawing.Size(1184, 532);
            this.dgvQueryBOM.TabIndex = 0;
            this.dgvQueryBOM.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQueryBOM_CellClick);
            // 
            // Selected
            // 
            this.Selected.HeaderText = "";
            this.Selected.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.Selected.Name = "Selected";
            this.Selected.Text = "Selected";
            this.Selected.UseColumnTextForLinkValue = true;
            this.Selected.Width = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblInfo);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 30);
            this.panel1.TabIndex = 1;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            this.lblInfo.Location = new System.Drawing.Point(20, 7);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(89, 17);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "There is no data.";
            this.lblInfo.Visible = false;
            // 
            // QueryBOMData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 562);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvQueryBOM);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Navy;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QueryBOMData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query Related BOM Data for GongDan List";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.QueryBOMData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueryBOM)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQueryBOM;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.DataGridViewLinkColumn Selected;
    }
}