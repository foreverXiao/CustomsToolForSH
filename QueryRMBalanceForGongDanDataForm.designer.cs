namespace SHCustomsSystem
{
    partial class QueryRMBalance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryRMBalance));
            this.dgvQueryRMBalance = new System.Windows.Forms.DataGridView();
            this.btnDownload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueryRMBalance)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvQueryRMBalance
            // 
            this.dgvQueryRMBalance.AllowUserToAddRows = false;
            this.dgvQueryRMBalance.AllowUserToDeleteRows = false;
            this.dgvQueryRMBalance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvQueryRMBalance.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvQueryRMBalance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQueryRMBalance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueryRMBalance.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQueryRMBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueryRMBalance.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvQueryRMBalance.Location = new System.Drawing.Point(0, 0);
            this.dgvQueryRMBalance.Name = "dgvQueryRMBalance";
            this.dgvQueryRMBalance.ReadOnly = true;
            this.dgvQueryRMBalance.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvQueryRMBalance.Size = new System.Drawing.Size(1334, 562);
            this.dgvQueryRMBalance.TabIndex = 3;
            this.dgvQueryRMBalance.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvQueryRMBalance_RowPostPaint);
            // 
            // btnDownload
            // 
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDownload.Location = new System.Drawing.Point(0, 0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(50, 23);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "*";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // QueryRMBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 562);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.dgvQueryRMBalance);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "QueryRMBalance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query GongDan Historical Data for Checking RM Balance";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.QueryRMBalance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueryRMBalance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQueryRMBalance;
        private System.Windows.Forms.Button btnDownload;
    }
}