using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class PopUpCategoryForm : Form
    {
        private GetBomDataForm getBomDataFrm = null;
        public PopUpCategoryForm()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            getBomDataFrm = GetBomDataForm.CreateInstance();
            getBomDataFrm.PopUpCategory = this;
            getBomDataFrm.LotNo = this.txtLotNo.Text.ToString().Trim().ToUpper();
            getBomDataFrm.Func_UpdateLotNo();
        }
    }
}
