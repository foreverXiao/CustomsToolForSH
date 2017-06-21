using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SHCustomsSystem
{
    public partial class MainForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        public MainForm()
        {
            InitializeComponent();
            this.lblName.Text = "Login: " + loginFrm.PublicUserName;
        }

        private void ExitTSMI_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.lblTitle.Visible == true) { this.lblTitle.Visible = false; }
        }

        private void AssistantDataTSMI_Click(object sender, EventArgs e)
        {
            AssisDataForm assisForm = AssisDataForm.CreateInstance();
            assisForm.Activate();
            assisForm.WindowState = FormWindowState.Normal;
            assisForm.MdiParent = this;
            assisForm.Show();
        }

        private void OriginalDataTSMI_Click(object sender, EventArgs e)
        {
            OrigDataForm origForm = OrigDataForm.CreateInstance();
            origForm.Activate();
            origForm.WindowState = FormWindowState.Normal;
            origForm.MdiParent = this;
            origForm.Show();
        }

        private void tsmiRMBalanceAdjust_Click(object sender, EventArgs e)
        {
            RMBalanceAdjustmentForm RMBalanceAdjustmentFrm = RMBalanceAdjustmentForm.CreateInstance();
            RMBalanceAdjustmentFrm.Activate();
            RMBalanceAdjustmentFrm.WindowState = FormWindowState.Normal;
            RMBalanceAdjustmentFrm.MdiParent = this;
            RMBalanceAdjustmentFrm.Show();
        }

        private void tsmiRMPurchaseAdjust_Click(object sender, EventArgs e)
        {
            RMPurchaseAdjustmentForm RMPurchaseAdjustmentFrm = RMPurchaseAdjustmentForm.CreateInstance();
            RMPurchaseAdjustmentFrm.Activate();
            RMPurchaseAdjustmentFrm.WindowState = FormWindowState.Normal;
            RMPurchaseAdjustmentFrm.MdiParent = this;
            RMPurchaseAdjustmentFrm.Show();
        }

        private void tsmiDroolsBalanceAdjust_Click(object sender, EventArgs e)
        {
            DroolsBalanceAdjustmentForm DroolsBalanceAdjustmentFrm = DroolsBalanceAdjustmentForm.CreateInstance();
            DroolsBalanceAdjustmentFrm.Activate();
            DroolsBalanceAdjustmentFrm.WindowState = FormWindowState.Normal;
            DroolsBalanceAdjustmentFrm.MdiParent = this;
            DroolsBalanceAdjustmentFrm.Show();
        }

        private void GetBatchDataTSMI_Click(object sender, EventArgs e)
        {
            GetBatchDataForm getBatchDataFrm = GetBatchDataForm.CreateInstance();
            getBatchDataFrm.Activate();
            getBatchDataFrm.WindowState = FormWindowState.Normal;
            getBatchDataFrm.MdiParent = this;
            getBatchDataFrm.Show();
        }

        private void GetBOMDataTSMI_Click(object sender, EventArgs e)
        {
            GetBomDataForm getBomDataFrm = GetBomDataForm.CreateInstance();
            getBomDataFrm.Activate();
            getBomDataFrm.WindowState = FormWindowState.Normal;
            getBomDataFrm.MdiParent = this;
            getBomDataFrm.Show();
        }

        private void CustomsBOMTSMI_Click(object sender, EventArgs e)
        {
            GetCustomsBomForm getCustomsBomFrm = GetCustomsBomForm.CreateInstance();
            getCustomsBomFrm.Activate();
            getCustomsBomFrm.WindowState = FormWindowState.Normal;
            getCustomsBomFrm.MdiParent = this;
            getCustomsBomFrm.Show();
        }

        private void TotalBOMDataTSMI_Click(object sender, EventArgs e)
        {
            TotalBomDataForm totalBomDataFrm = TotalBomDataForm.CreateInstance();
            totalBomDataFrm.Activate();
            totalBomDataFrm.WindowState = FormWindowState.Normal;
            totalBomDataFrm.MdiParent = this;
            totalBomDataFrm.Show();
        }

        private void GetGongDanDataMSTI_Click(object sender, EventArgs e)
        {
            GetGongDanDataForm getGongDanDataFrm = GetGongDanDataForm.CreateInstance();
            getGongDanDataFrm.Activate();
            getGongDanDataFrm.WindowState = FormWindowState.Normal;
            getGongDanDataFrm.MdiParent = this;
            getGongDanDataFrm.Show();
        }

        private void CustomsGongDanMSTI_Click(object sender, EventArgs e)
        {
            GetCustomsGongdanForm getCustomsGongdanFrm = GetCustomsGongdanForm.CreateInstance();
            getCustomsGongdanFrm.Activate();
            getCustomsGongdanFrm.WindowState = FormWindowState.Normal;
            getCustomsGongdanFrm.MdiParent = this;
            getCustomsGongdanFrm.Show();
        }

        private void TotalGongDanTSMI_Click(object sender, EventArgs e)
        {
            TotalGongDanDataForm totalGongDataFrm = TotalGongDanDataForm.CreateInstance();
            totalGongDataFrm.Activate();
            totalGongDataFrm.WindowState = FormWindowState.Normal;
            totalGongDataFrm.MdiParent = this;
            totalGongDataFrm.Show();
        }

        private void DataConversionMSTI_Click(object sender, EventArgs e)
        {
            DataConversion DataConversionFrm = DataConversion.CreateInstance();
            DataConversionFrm.Activate();
            DataConversionFrm.WindowState = FormWindowState.Normal;
            DataConversionFrm.MdiParent = this;
            DataConversionFrm.Show();
        }

        private void GetDroolsDataDocMSTI_Click(object sender, EventArgs e)
        {
            GetDroolsDataDocForm getDroolsDataDocFrm = GetDroolsDataDocForm.CreateInstance();
            getDroolsDataDocFrm.Activate();
            getDroolsDataDocFrm.WindowState = FormWindowState.Normal;
            getDroolsDataDocFrm.MdiParent = this;
            getDroolsDataDocFrm.Show();
        }

        private void TotalDroolsTSMI_Click(object sender, EventArgs e)
        {
            TotalDroolsDataForm totalDroolsDataFrm = TotalDroolsDataForm.CreateInstance();
            totalDroolsDataFrm.Activate();
            totalDroolsDataFrm.WindowState = FormWindowState.Normal;
            totalDroolsDataFrm.MdiParent = this;
            totalDroolsDataFrm.Show();
        }

        private void GetBeiAnDanDataMSTI_Click(object sender, EventArgs e)
        {
            GetBeiAnDanDataForm getBeiAnDanDataFrm = GetBeiAnDanDataForm.CreateInstance();
            getBeiAnDanDataFrm.Activate();
            getBeiAnDanDataFrm.WindowState = FormWindowState.Normal;
            getBeiAnDanDataFrm.MdiParent = this;
            getBeiAnDanDataFrm.Show();
        }

        private void CustomsBeiAnDanMSTI_Click(object sender, EventArgs e)
        {
            GetCustomsBeiAnDanForm getCustomsBeiAnDanFrm = GetCustomsBeiAnDanForm.CreateInstance();
            getCustomsBeiAnDanFrm.Activate();
            getCustomsBeiAnDanFrm.WindowState = FormWindowState.Normal;
            getCustomsBeiAnDanFrm.MdiParent = this;
            getCustomsBeiAnDanFrm.Show();
        }

        private void TotalBeiAnDanTSMI_Click(object sender, EventArgs e)
        {
            TotalBeiAnDanDataForm totalBeiAnDanDataFrm = TotalBeiAnDanDataForm.CreateInstance();
            totalBeiAnDanDataFrm.Activate();
            totalBeiAnDanDataFrm.WindowState = FormWindowState.Normal;
            totalBeiAnDanDataFrm.MdiParent = this;
            totalBeiAnDanDataFrm.Show();
        }

        private void getPinDanDataTSMI_Click(object sender, EventArgs e)
        {
            TotalPingDanDataForm totalPingDanDataFrm = TotalPingDanDataForm.CreateInstance();
            totalPingDanDataFrm.Activate();
            totalPingDanDataFrm.WindowState = FormWindowState.Normal;
            totalPingDanDataFrm.MdiParent = this;
            totalPingDanDataFrm.Show();
        }

        private void GetGongDanListTSMI_Click(object sender, EventArgs e)
        {
            GetGongDanListForm getGongDanListFrm = GetGongDanListForm.CreateInstance();
            getGongDanListFrm.Activate();
            getGongDanListFrm.WindowState = FormWindowState.Normal;
            getGongDanListFrm.MdiParent = this;
            getGongDanListFrm.Show();
        }

        private void EditBeiAnDanTSMI_Click(object sender, EventArgs e)
        {
            EditBeiAnDanForOF getEditionBeiAnDanFrm = EditBeiAnDanForOF.CreateInstance();
            getEditionBeiAnDanFrm.Activate();
            getEditionBeiAnDanFrm.WindowState = FormWindowState.Normal;
            getEditionBeiAnDanFrm.MdiParent = this;
            getEditionBeiAnDanFrm.Show();
        }

        private void tsmiRMShareOut_Click(object sender, EventArgs e)
        {
            RMShareOutForm RMShareOutFrm = RMShareOutForm.CreateInstance();
            RMShareOutFrm.Activate();
            RMShareOutFrm.WindowState = FormWindowState.Normal;
            RMShareOutFrm.MdiParent = this;
            RMShareOutFrm.Show();
        }

        private void RMUnionQueryMSTI_Click(object sender, EventArgs e)
        {
            RMUnionQuery RMUnionQueryFrm = RMUnionQuery.CreateInstance();
            RMUnionQueryFrm.Activate();
            RMUnionQueryFrm.WindowState = FormWindowState.Normal;
            RMUnionQueryFrm.MdiParent = this;
            RMUnionQueryFrm.Show();
        }

        private void SummaryReportsMSTI_Click(object sender, EventArgs e)
        {
            SummaryReportsForm SummaryRptFrm = SummaryReportsForm.CreateInstance();
            SummaryRptFrm.Activate();
            SummaryRptFrm.WindowState = FormWindowState.Normal;
            SummaryRptFrm.MdiParent = this;
            SummaryRptFrm.Show();
        }

        private void GetPingDanDataTSMI_RM_D_Click(object sender, EventArgs e)
        {
            GetPingDanDataForm_RM_D PingDanDataFrm = GetPingDanDataForm_RM_D.CreateInstance();
            PingDanDataFrm.Activate();
            PingDanDataFrm.WindowState = FormWindowState.Normal;
            PingDanDataFrm.MdiParent = this;
            PingDanDataFrm.Show();
        }

        private void GetBeiAnDanDataTSMI_RM_D_Click(object sender, EventArgs e)
        {
            GetBeiAnDanDataForm_RM_D BADDataFrm_RMD = GetBeiAnDanDataForm_RM_D.CreateInstance();
            BADDataFrm_RMD.Activate();
            BADDataFrm_RMD.WindowState = FormWindowState.Normal;
            BADDataFrm_RMD.MdiParent = this;
            BADDataFrm_RMD.Show();
        }
    }
}