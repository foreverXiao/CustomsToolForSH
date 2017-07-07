using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace eCustoms
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

        private void RMReceiving_TMSI_Click(object sender, EventArgs e)
        {
            RMReceivingForm RMRcvdFrm = RMReceivingForm.CreateInstance();
            RMRcvdFrm.Activate();
            RMRcvdFrm.WindowState = FormWindowState.Normal;
            RMRcvdFrm.MdiParent = this;
            RMRcvdFrm.Show();
        }

        private void RMBalance_TMSI_Click(object sender, EventArgs e)
        {
            RMBalanceForm RMBalanceFrm = RMBalanceForm.CreateInstance();
            RMBalanceFrm.Activate();
            RMBalanceFrm.WindowState = FormWindowState.Normal;
            RMBalanceFrm.MdiParent = this;
            RMBalanceFrm.Show();
        }

        private void GetBomData_TSMI_Click(object sender, EventArgs e)
        {
            GetBomDataForm getBomDataFrm = GetBomDataForm.CreateInstance();
            getBomDataFrm.Activate();
            getBomDataFrm.WindowState = FormWindowState.Normal;
            getBomDataFrm.MdiParent = this;
            getBomDataFrm.Show();
        }

        private void GetBomDocument_TSMI_Click(object sender, EventArgs e)
        {
            GetBomReporForm getBomRptFrm = GetBomReporForm.CreateInstance();
            getBomRptFrm.Activate();
            getBomRptFrm.WindowState = FormWindowState.Normal;
            getBomRptFrm.MdiParent = this;
            getBomRptFrm.Show();
        }

        private void BomHistoryData_TSMI_Click(object sender, EventArgs e)
        {
            HistoryBomDataForm HistoryBomDataFrm = HistoryBomDataForm.CreateInstance();
            HistoryBomDataFrm.Activate();
            HistoryBomDataFrm.WindowState = FormWindowState.Normal;
            HistoryBomDataFrm.MdiParent = this;
            HistoryBomDataFrm.Show();
        }

        private void GetGongDanList_TSMI_Click(object sender, EventArgs e)
        {
            GetGongDanListForm getGongDanListFrm = GetGongDanListForm.CreateInstance();
            getGongDanListFrm.Activate();
            getGongDanListFrm.WindowState = FormWindowState.Normal;
            getGongDanListFrm.MdiParent = this;
            getGongDanListFrm.Show();
        }

        private void GetGongDanData_TSMI_Click(object sender, EventArgs e)
        {
            GetGongDanDataForm getGongDanDataFrm = GetGongDanDataForm.CreateInstance();
            getGongDanDataFrm.Activate();
            getGongDanDataFrm.WindowState = FormWindowState.Normal;
            getGongDanDataFrm.MdiParent = this;
            getGongDanDataFrm.Show();
        }

        private void GetGongDanDocument_TSMI_Click(object sender, EventArgs e)
        {
            GetGongdanReportForm getGongdanRptFrm = GetGongdanReportForm.CreateInstance();
            getGongdanRptFrm.Activate();
            getGongdanRptFrm.WindowState = FormWindowState.Normal;
            getGongdanRptFrm.MdiParent = this;
            getGongdanRptFrm.Show();
        }

        private void GongDanHistoryData_TSMI_Click(object sender, EventArgs e)
        {
            HistoryGongDanDataForm HistoryGongDanDataFrm = HistoryGongDanDataForm.CreateInstance();
            HistoryGongDanDataFrm.Activate();
            HistoryGongDanDataFrm.WindowState = FormWindowState.Normal;
            HistoryGongDanDataFrm.MdiParent = this;
            HistoryGongDanDataFrm.Show();
        }

        private void GetPD_TSMI_Click(object sender, EventArgs e)
        {
            GetDistributionPingDanDataForm GetDistributionPingDanDataFrm = GetDistributionPingDanDataForm.CreateInstance();
            GetDistributionPingDanDataFrm.Activate();
            GetDistributionPingDanDataFrm.WindowState = FormWindowState.Normal;
            GetDistributionPingDanDataFrm.MdiParent = this;
            GetDistributionPingDanDataFrm.Show();
        }

        private void GetBAD_TSMI_Click(object sender, EventArgs e)
        {
            GetDistributionBeiAnDanDataForm GetDistributionBeiAnDanDataFrm = GetDistributionBeiAnDanDataForm.CreateInstance();
            GetDistributionBeiAnDanDataFrm.Activate();
            GetDistributionBeiAnDanDataFrm.WindowState = FormWindowState.Normal;
            GetDistributionBeiAnDanDataFrm.MdiParent = this;
            GetDistributionBeiAnDanDataFrm.Show();
        }

        private void GetBeiAnDanData_TSMI_Click(object sender, EventArgs e)
        {
            GetBeiAnDanDataForm getBeiAnDanDataFrm = GetBeiAnDanDataForm.CreateInstance();
            getBeiAnDanDataFrm.Activate();
            getBeiAnDanDataFrm.WindowState = FormWindowState.Normal;
            getBeiAnDanDataFrm.MdiParent = this;
            getBeiAnDanDataFrm.Show();
        }

        private void GetBeiAnDanDocument_TSMI_Click(object sender, EventArgs e)
        {
            GetBeiAnDanReportForm getBeiAnDanRptFrm = GetBeiAnDanReportForm.CreateInstance();
            getBeiAnDanRptFrm.Activate();
            getBeiAnDanRptFrm.WindowState = FormWindowState.Normal;
            getBeiAnDanRptFrm.MdiParent = this;
            getBeiAnDanRptFrm.Show();
        }

        private void BeiAnDanHistoryData_TSMI_Click(object sender, EventArgs e)
        {
            HistoryBeiAnDanDataForm HistoryBeiAnDanDataFrm = HistoryBeiAnDanDataForm.CreateInstance();
            HistoryBeiAnDanDataFrm.Activate();
            HistoryBeiAnDanDataFrm.WindowState = FormWindowState.Normal;
            HistoryBeiAnDanDataFrm.MdiParent = this;
            HistoryBeiAnDanDataFrm.Show();
        }

        private void GetPingDanData_TSMI_Click(object sender, EventArgs e)
        {
            HistoryPingDanDataForm HistoryPingDanDataFrm = HistoryPingDanDataForm.CreateInstance();
            HistoryPingDanDataFrm.Activate();
            HistoryPingDanDataFrm.WindowState = FormWindowState.Normal;
            HistoryPingDanDataFrm.MdiParent = this;
            HistoryPingDanDataFrm.Show();
        }

        private void BasicData_TSMI_Click(object sender, EventArgs e)
        {
            BasicDataForm BasicDataFrm = BasicDataForm.CreateInstance();
            BasicDataFrm.Activate();
            BasicDataFrm.WindowState = FormWindowState.Normal;
            BasicDataFrm.MdiParent = this;
            BasicDataFrm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}