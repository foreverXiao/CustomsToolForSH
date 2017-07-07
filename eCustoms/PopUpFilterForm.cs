using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class PopUpFilterForm : Form
    {
        fundeleFilter funFilter = null;
        public PopUpFilterForm()
        {
            InitializeComponent();
        }

        public PopUpFilterForm(fundeleFilter filter)
        {
            InitializeComponent();
            this.funFilter = filter;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.funFilter(this.lblFilterColumn.Text.Trim(), this.cmbFilterContent.Text.Trim());
        }
    }
    public delegate void fundeleFilter(string strColumnName, string strCondition);
}
