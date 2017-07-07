namespace eCustoms
{
    partial class PopUpInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopUpInfoForm));
            this.dgvPopupInfo = new System.Windows.Forms.DataGridView();
            this.btnDownload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPopupInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPopupInfo
            // 
            this.dgvPopupInfo.AllowUserToAddRows = false;
            this.dgvPopupInfo.AllowUserToDeleteRows = false;
            this.dgvPopupInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPopupInfo.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvPopupInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPopupInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPopupInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPopupInfo.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvPopupInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvPopupInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPopupInfo.Name = "dgvPopupInfo";
            this.dgvPopupInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPopupInfo.Size = new System.Drawing.Size(344, 281);
            this.dgvPopupInfo.TabIndex = 1;
            // 
            // btnDownload
            // 
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.Location = new System.Drawing.Point(0, 0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(41, 23);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // PopUpInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 281);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.dgvPopupInfo);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopUpInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prompt";
            this.Load += new System.EventHandler(this.PopUpInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPopupInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPopupInfo;
        private System.Windows.Forms.Button btnDownload;
    }
}