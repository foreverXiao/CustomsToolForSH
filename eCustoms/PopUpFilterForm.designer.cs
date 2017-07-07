namespace eCustoms
{
    partial class PopUpFilterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopUpFilterForm));
            this.label1 = new System.Windows.Forms.Label();
            this.lblFilterColumn = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbFilterSymbol = new System.Windows.Forms.ComboBox();
            this.cmbFilterContent = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(29, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter Column:";
            // 
            // lblFilterColumn
            // 
            this.lblFilterColumn.AutoSize = true;
            this.lblFilterColumn.Location = new System.Drawing.Point(161, 20);
            this.lblFilterColumn.Name = "lblFilterColumn";
            this.lblFilterColumn.Size = new System.Drawing.Size(29, 13);
            this.lblFilterColumn.TabIndex = 1;
            this.lblFilterColumn.Text = "Filter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(29, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "operative symbol:";
            // 
            // cmbFilterSymbol
            // 
            this.cmbFilterSymbol.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFilterSymbol.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFilterSymbol.FormattingEnabled = true;
            this.cmbFilterSymbol.Items.AddRange(new object[] {
            "=",
            ">",
            ">=",
            "<",
            "<=",
            "LIKE",
            "NOT LIKE"});
            this.cmbFilterSymbol.Location = new System.Drawing.Point(152, 52);
            this.cmbFilterSymbol.Name = "cmbFilterSymbol";
            this.cmbFilterSymbol.Size = new System.Drawing.Size(100, 21);
            this.cmbFilterSymbol.TabIndex = 3;
            this.cmbFilterSymbol.Text = "=";
            // 
            // cmbFilterContent
            // 
            this.cmbFilterContent.FormattingEnabled = true;
            this.cmbFilterContent.Location = new System.Drawing.Point(32, 96);
            this.cmbFilterContent.Name = "cmbFilterContent";
            this.cmbFilterContent.Size = new System.Drawing.Size(220, 21);
            this.cmbFilterContent.TabIndex = 3;
            // 
            // btnFilter
            // 
            this.btnFilter.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnFilter.Image")));
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFilter.Location = new System.Drawing.Point(32, 136);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(220, 24);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // PopUpFilterForm
            // 
            this.AcceptButton = this.btnFilter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 178);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.cmbFilterContent);
            this.Controls.Add(this.cmbFilterSymbol);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFilterColumn);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopUpFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filter Records";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox cmbFilterSymbol;
        public System.Windows.Forms.ComboBox cmbFilterContent;
        private System.Windows.Forms.Button btnFilter;
        public System.Windows.Forms.Label lblFilterColumn;
    }
}