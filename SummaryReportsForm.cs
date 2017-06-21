using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class SummaryReportsForm : Form
    {
        private static SummaryReportsForm SummaryRptFrm;
        public static SummaryReportsForm CreateInstance()
        {
            if (SummaryRptFrm == null || SummaryRptFrm.IsDisposed)
            {
                SummaryRptFrm = new SummaryReportsForm();
            }
            return SummaryRptFrm;
        }
        public SummaryReportsForm()
        {
            InitializeComponent();
        }
       
        private void SummaryReportsForm_Load(object sender, EventArgs e)
        {
            //Query Adjustment Report
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";

            //RM In Second Release Report 
            this.dtpFrom1.CustomFormat = " ";
            this.dtpTo1.CustomFormat = " ";
            this.dtpFrom2.CustomFormat = " ";
            this.dtpTo2.CustomFormat = " ";

            //BOM Total
            this.dtpFrom3.CustomFormat = " ";
            this.dtpTo3.CustomFormat = " ";

            //EHB BOM 
            this.dtpFrom4.CustomFormat = " ";
            this.dtpTo4.CustomFormat = " ";

            //EHB RM Share Out 
            this.dtpFrom5.CustomFormat = " ";
            this.dtpTo5.CustomFormat = " ";
            this.dtpFrom6.CustomFormat = " ";
            this.dtpTo6.CustomFormat = " ";

            //EHB FG Out ( BeiAnDan ) & RMB PingDan
            this.dtpFrom7.CustomFormat = " ";
            this.dtpTo7.CustomFormat = " ";
            this.dtpPassGateFrom.CustomFormat = " ";
            this.dtpPassGateTo.CustomFormat = " ";

            //GongDan RM Usage State & GongDan Balance
            this.dtpFrom8.CustomFormat = " ";
            this.dtpTo8.CustomFormat = " ";

            //RM Balance By RM Customs Code
            this.dtpFrom9.CustomFormat = " ";
            this.dtpTo9.CustomFormat = " ";

            //RM Balance By BGD No
            this.dtpFrom10.CustomFormat = " ";
            this.dtpTo10.CustomFormat = " ";

            //RM Distribution in e-Handbook (PingDan)
            this.dtpFrom11.CustomFormat = " ";
            this.dtpTo11.CustomFormat = " ";
        }

        #region //Query Adjustment & Document Report
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = null;
        }

        private void dtpFrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo.CustomFormat = null;
        }

        private void dtpTo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strObject = this.cmbObject.Text.Trim();
            if (String.IsNullOrEmpty(strObject))
            {
                MessageBox.Show("Please select the query object first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cmbObject.Focus();
                return;
            }

            string strBrowse = null;
            if (String.Compare(strObject, "Adjust BGD") == 0) { strBrowse = @"SELECT * FROM C_GongDanAdjustBGD WHERE "; }
            else if (String.Compare(strObject, "Adjust RM Balance") == 0) { strBrowse = @"SELECT * FROM C_RMBalanceAdjustment WHERE "; }
            else if (String.Compare(strObject, "Adjust RM Receiving") == 0) { strBrowse = @"SELECT * FROM C_RMPurchaseAdjustment WHERE "; }
            else if (String.Compare(strObject, "Adjust Drools Balance") == 0) { strBrowse = @"SELECT * FROM C_DroolsBalanceAdjustment WHERE "; }
            else if (String.Compare(strObject, "Consumption Doc") == 0) { strBrowse = @"SELECT * FROM E_Consumption WHERE "; }
            else if (String.Compare(strObject, "Original Goods Doc") == 0) { strBrowse = @"SELECT * FROM E_OriginalGoods WHERE "; }
            else if (String.Compare(strObject, "GongDan Doc") == 0) { strBrowse = @"SELECT * FROM E_GongDan WHERE "; }
            else if (String.Compare(strObject, "BeiAnDan Doc") == 0) { strBrowse = @"SELECT * FROM E_BeiAnDan WHERE "; }
            else { strBrowse = @"SELECT * FROM B_OrderFulfillment_Backup WHERE "; }

            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for created date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom.Focus();
                        return;
                    }
                    else
                    { strBrowse += " [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [Created Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse += " [Created Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strBrowse += " [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 6, 6), " WHERE") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 6); }
            if (String.Compare(strObject, "BeiAnDan Doc") == 0) { strBrowse += " ORDER BY [Created Date] DESC, [Group ID], [项号] ASC"; }
            else if (String.Compare(strObject, "Order Fulfillment Backup") == 0)
            {
                strBrowse = strBrowse.Replace("Created Date", "Backup Time");
                strBrowse += " ORDER BY [Backup Time] DESC";
            }
            else { strBrowse += " ORDER BY [Created Date] DESC"; }

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlDataAdapter browseAdapter = new SqlDataAdapter(strBrowse, SqlConn);
            DataTable dtBrowse = new DataTable();
            browseAdapter.Fill(dtBrowse);
            browseAdapter.Dispose();

            if (dtBrowse.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(dtBrowse.Rows.Count / PageRow);
                if (iPageCount * PageRow < dtBrowse.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\" + strObject + " Report " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dtBrowse.Columns.Count; n++)
                        { sb.Append(dtBrowse.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        if (String.Compare(strObject, "Adjust BGD") == 0)
                        {
                            for (int i = (m - 1) * PageRow; i < dtBrowse.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dtBrowse.Columns.Count; j++)
                                {
                                    if (j == 2 || j == 6) { sb.Append("'" + dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                    else { sb.Append(dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                }
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else if (String.Compare(strObject, "Adjust RM Balance") == 0 || String.Compare(strObject, "Adjust RM Receiving") == 0)
                        {
                            for (int i = (m - 1) * PageRow; i < dtBrowse.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dtBrowse.Columns.Count; j++)
                                {
                                    if (j == 1) { sb.Append("'" + dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                    else { sb.Append(dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                }
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else if(String.Compare(strObject, "Original Goods Doc") == 0)
                        {
                            for (int i = (m - 1) * PageRow; i < dtBrowse.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dtBrowse.Columns.Count; j++)
                                {
                                    if (j == 4) { sb.Append("'" + dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                    else { sb.Append(dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                }
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else if (String.Compare(strObject, "GongDan Doc") == 0)
                        {
                            for (int i = (m - 1) * PageRow; i < dtBrowse.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dtBrowse.Columns.Count; j++)
                                {
                                    if (j == 8) { sb.Append("'" + dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                    else { sb.Append(dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                }
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else if (String.Compare(strObject, "BeiAnDan Doc") == 0 || String.Compare(strObject, "Order Fulfillment Backup") == 0)
                        {
                            for (int i = (m - 1) * PageRow; i < dtBrowse.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dtBrowse.Columns.Count; j++)
                                {
                                    if (j == 0) { sb.Append("'" + dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                    else { sb.Append(dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                }
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else
                        {
                            for (int i = (m - 1) * PageRow; i < dtBrowse.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dtBrowse.Columns.Count; j++)
                                { sb.Append(dtBrowse.Rows[i][j].ToString().Trim() + "\t"); }
                                sb.Append(Environment.NewLine);
                            }
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    MessageBox.Show("Successfully generated '" + strObject + " Report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }

            dtBrowse.Clear();
            dtBrowse.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }
        #endregion

        #region //RM In Second Release Report
        private void dtpFrom1_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom1.CustomFormat = null;
        }

        private void dtpFrom1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom1.CustomFormat = " "; }
        }

        private void dtpTo1_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo1.CustomFormat = null;
        }

        private void dtpTo1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo1.CustomFormat = " "; }
        }

        private void dtpFrom2_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom2.CustomFormat = null;
        }

        private void dtpFrom2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom2.CustomFormat = " "; }
        }

        private void dtpTo2_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo2.CustomFormat = null;
        }

        private void dtpTo2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo2.CustomFormat = " "; }
        }

        private void btnDownload1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }          

            string strBrowse1 = null;
            if (!String.IsNullOrEmpty(this.dtpTo1.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom1.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom1.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo1.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for 2nd release date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom1.Focus();
                        return;
                    }
                    else
                    { strBrowse1 += " [2nd Release Date] >= '" + Convert.ToDateTime(this.dtpFrom1.Value.ToString("M/d/yyyy")) + "' AND [2nd Release Date] < '" + Convert.ToDateTime(this.dtpTo1.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else
                { strBrowse1 += " [2nd Release Date] < '" + Convert.ToDateTime(this.dtpTo1.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom1.Text.Trim()))
                { strBrowse1 += " [2nd Release Date] >= '" + Convert.ToDateTime(this.dtpFrom1.Value.ToString("M/d/yyyy")) + "' AND"; }
            }

            if (!String.IsNullOrEmpty(this.dtpTo2.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom2.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom2.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo2.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for pass gate time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom2.Focus();
                        return;
                    }
                    else
                    { strBrowse1 += " [Pass Gate Time] >= '" + Convert.ToDateTime(this.dtpFrom2.Value.ToString("M/d/yyyy")) + "' AND [Pass Gate Time] < '" + Convert.ToDateTime(this.dtpTo2.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else
                { strBrowse1 += " [Pass Gate Time] < '" + Convert.ToDateTime(this.dtpTo2.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom2.Text.Trim()))
                { strBrowse1 += " [Pass Gate Time] >= '" + Convert.ToDateTime(this.dtpFrom2.Value.ToString("M/d/yyyy")) + "' AND"; }
            }

            string strSQL1 = @"SELECT [Transaction Type] AS [Type], [BGD No], [Customs Entry No], [RM Customs Code], [RM CHN Name], [PO Invoice Qty], [Amount], " + 
                              "[RM Currency], [Original Country], CAST([Customs Rcvd Date] AS datetime) AS [2nd Release Date], [Voucher ID] AS [PingDan ID], " +
                              "[Voucher Status] AS [PingDan Status], [Gate PassThrough Date] AS [Pass Gate Time], [Remark] FROM C_RMPurchase " +
                              "WHERE CHARINDEX('/', [Customs Rcvd Date]) > 0";
            string strSQL_M = @"SELECT [Type], [BGD No], [Customs Entry No], [RM Customs Code], [RM CHN Name], [PO Invoice Qty], [Amount], [RM Currency], " +
                               "[Original Country], [2nd Release Date], [PingDan ID], [PingDan Status], [Pass Gate Time], [Remark] FROM C_RMPurchase_Manual";
            SqlConnection SqlConn1 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn1.State == ConnectionState.Closed) { SqlConn1.Open(); }
            SqlDataAdapter browseAdapter1 = new SqlDataAdapter(strSQL1, SqlConn1);
            DataTable dtBrowse1 = new DataTable();
            browseAdapter1.Fill(dtBrowse1);
            browseAdapter1 = new SqlDataAdapter(strSQL_M, SqlConn1);
            DataTable dtBrowse_M = new DataTable();
            browseAdapter1.Fill(dtBrowse_M);
            browseAdapter1.Dispose();
            if (dtBrowse_M.Rows.Count > 0) { dtBrowse1.Merge(dtBrowse_M); }
            dtBrowse_M.Dispose();

            DataView dv = dtBrowse1.DefaultView;
            if (!String.IsNullOrEmpty(strBrowse1)) { strBrowse1 = strBrowse1.Remove(strBrowse1.Length - 4); }
            dv.RowFilter = strBrowse1;
            DataTable dtMiddle1 = dv.ToTable().Copy();
            dtBrowse1.Clear();
            dtBrowse1.Dispose();

            if (dtMiddle1.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(dtMiddle1.Rows.Count / PageRow);
                if (iPageCount * PageRow < dtMiddle1.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\RM In Second Release Report " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dtMiddle1.Columns.Count; n++)
                        { sb.Append(dtMiddle1.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < dtMiddle1.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < dtMiddle1.Columns.Count; j++)
                            {
                                if (j == 1) { sb.Append("'" + dtMiddle1.Rows[i][j].ToString().Trim() + "\t"); }
                                else if (j == 9 || j == 12)
                                {
                                    string strObj = dtMiddle1.Rows[i][j].ToString().Trim();
                                    if (!String.IsNullOrEmpty(strObj)) { strObj = strObj.Substring(0, strObj.Length - 11); }
                                    sb.Append(strObj + "\t");
                                }
                                else { sb.Append(dtMiddle1.Rows[i][j].ToString().Trim() + "\t"); }
                            }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    MessageBox.Show("Successfully generated 'RM In Second Release Report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }

            dtMiddle1.Dispose();           
            if (SqlConn1.State == ConnectionState.Open)
            {
                SqlConn1.Close();
                SqlConn1.Dispose();
            }
        }
        #endregion

        #region //BOM Total
        private void dtpFrom3_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom3.CustomFormat = null;
        }

        private void dtpFrom3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom3.CustomFormat = " "; }
        }

        private void dtpTo3_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo3.CustomFormat = null;
        }

        private void dtpTo3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo3.CustomFormat = " "; }
        }

        private void btnDownload2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strBrowse2 = @"SELECT M.[Actual Start Date], M.[Actual Close Date], M.[Batch No], M.[FG No], D.[Line No], D.[Item No], D.[Lot No], M.[FG Qty], " + 
                                 "D.[RM Qty], D.[Inventory Type], D.[RM Category], D.[Consumption], M.[Qty Loss Rate], D.[RM Customs Code], M.[HS Code], " + 
                                 "CASE WHEN M.[BOM In Customs] <> '' AND M.[BOM In Customs] IS NOT NULL THEN M.[BOM In Customs] ELSE M.[FG No] + '/' + M.[Batch No] END " + 
                                 "AS [FG EHB], D.[Drools EHB], M.[CHN Name], D.[Note], M.[Approved Date] AS [BOM Date] FROM C_BOMDetail AS D INNER JOIN C_BOM AS M ON " + 
                                 "D.[Batch No] = M.[Batch No]";
            if (!String.IsNullOrEmpty(this.dtpTo3.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom3.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom3.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo3.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for created date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom3.Focus();
                        return;
                    }
                    else
                    { strBrowse2 += " WHERE M.[Approved Date] >= '" + Convert.ToDateTime(this.dtpFrom3.Value.ToString("M/d/yyyy")) + "' AND M.[Approved Date] < '" + Convert.ToDateTime(this.dtpTo3.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse2 += " WHERE M.[Approved Date] < '" + Convert.ToDateTime(this.dtpTo3.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom3.Text.Trim()))
                { strBrowse2 += " WHERE M.[Approved Date] >= '" + Convert.ToDateTime(this.dtpFrom3.Value.ToString("M/d/yyyy")) + "'"; }
            }

            SqlConnection SqlConn2 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn2.State == ConnectionState.Closed) { SqlConn2.Open(); }
            SqlDataAdapter browseAdapter2 = new SqlDataAdapter(strBrowse2, SqlConn2);
            DataTable dtBrowse2 = new DataTable();
            browseAdapter2.Fill(dtBrowse2);
            browseAdapter2.Dispose();

            if (dtBrowse2.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(dtBrowse2.Rows.Count / PageRow);
                if (iPageCount * PageRow < dtBrowse2.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\BOM Total " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dtBrowse2.Columns.Count; n++)
                        { sb.Append(dtBrowse2.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);
                        
                        for (int i = (m - 1) * PageRow; i < dtBrowse2.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < dtBrowse2.Columns.Count; j++)
                            {
                                if (j == 0 || j == 1 || j == 19)
                                {
                                    string strObj = dtBrowse2.Rows[i][j].ToString().Trim();
                                    if (!String.IsNullOrEmpty(strObj)) { strObj = strObj.Substring(0, strObj.Length - 11); }
                                    sb.Append(strObj + "\t");
                                }
                                else if (j == 2) { sb.Append("'" + dtBrowse2.Rows[i][j].ToString().Trim() + "\t"); }
                                else { sb.Append(dtBrowse2.Rows[i][j].ToString().Trim() + "\t"); }
                            }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    MessageBox.Show("Successfully generated 'BOM Total report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }

            dtBrowse2.Clear();
            dtBrowse2.Dispose();
            if (SqlConn2.State == ConnectionState.Open)
            {
                SqlConn2.Close();
                SqlConn2.Dispose();
            }
        }
        #endregion

        #region //EHB BOM
        private void dtpFrom4_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom4.CustomFormat = null;
        }

        private void dtpFrom4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom4.CustomFormat = " "; }
        }

        private void dtpTo4_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo4.CustomFormat = null;
        }

        private void dtpTo4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo4.CustomFormat = " "; }
        }

        private void btnDownload3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }           

            string strBrowse3 = null;
            if (!String.IsNullOrEmpty(this.dtpTo4.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom4.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom4.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo4.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for created date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom4.Focus();
                        return;
                    }
                    else
                    { strBrowse3 += " WHERE E.[Created Date] >= '" + Convert.ToDateTime(this.dtpFrom4.Value.ToString("M/d/yyyy")) + "' AND E.[Created Date] < '" + Convert.ToDateTime(this.dtpTo4.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse3 += " WHERE E.[Created Date] < '" + Convert.ToDateTime(this.dtpTo4.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom4.Text.Trim()))
                { strBrowse3 += " WHERE E.[Created Date] >= '" + Convert.ToDateTime(this.dtpFrom4.Value.ToString("M/d/yyyy")) + "'"; }
            }

            string strSQL2 = @"SELECT CASE WHEN E.[BOM In Customs] = '' OR E.[BOM In Customs] IS NULL THEN E.[成品备件号] ELSE E.[BOM In Customs] END AS [成品备件号], " +
                              "E.[项号], E.[原料备件号], E.[净耗], E.[数量损耗率(%)], E.[重量损耗率(%)], E.[注释], E.[废料备件号], C.[CHN Name] AS [成品中文名称], C.[HS Code] " +
                              "FROM E_Consumption AS E LEFT OUTER JOIN C_BOM AS C ON SUBSTRING(E.[成品备件号], CHARINDEX('/', E.[成品备件号]) + 1, LEN(E.[成品备件号])) = " +
                              "C.[Batch No] " + strBrowse3; 

            SqlConnection SqlConn3 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn3.State == ConnectionState.Closed) { SqlConn3.Open(); }
            SqlDataAdapter browseAdapter3 = new SqlDataAdapter(strSQL2, SqlConn3);
            DataTable dtBrowse3 = new DataTable();
            browseAdapter3.Fill(dtBrowse3);

            if (dtBrowse3.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                browseAdapter3.Dispose();
                dtBrowse3.Dispose();
                SqlConn3.Close();
                SqlConn3.Dispose();
                return;
            }

            browseAdapter3 = new SqlDataAdapter(@"SELECT DISTINCT [RM Customs Code] AS [原料备件号], [RM CHN Name] AS [料件中文名称] FROM C_RMPurchase ORDER BY [RM Customs Code]", SqlConn3);
            DataTable dtMiddle2 = new DataTable();
            browseAdapter3.Fill(dtMiddle2);
            browseAdapter3.Dispose();

            SqlLib sqlLib1 = new SqlLib();
            DataTable dtMiddle3 = sqlLib1.MergeDataTable(dtBrowse3, dtMiddle2, "原料备件号");
            sqlLib1.Dispose(0);
            dtBrowse3.Dispose();
            dtMiddle2.Dispose();
          
            int PageRow = 65536;
            int iPageCount = (int)(dtMiddle3.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtMiddle3.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\EHB BOM " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtMiddle3.Columns.Count; n++)
                    { sb.Append(dtMiddle3.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < dtMiddle3.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtMiddle3.Columns.Count; j++)
                        { sb.Append(dtMiddle3.Rows[i][j].ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);
                    }

                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                MessageBox.Show("Successfully generated 'EHB BOM report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            dtMiddle3.Clear();
            dtMiddle3.Dispose();
            if (SqlConn3.State == ConnectionState.Open)
            {
                SqlConn3.Close();
                SqlConn3.Dispose();
            }
        }
        #endregion

        #region //EHB RM Share Out
        private void dtpFrom5_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom5.CustomFormat = null;
        }

        private void dtpFrom5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom5.CustomFormat = " "; }
        }

        private void dtpTo5_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo5.CustomFormat = null;
        }

        private void dtpTo5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo5.CustomFormat = " "; }
        }

        private void dtpFrom6_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom6.CustomFormat = null;
        }

        private void dtpFrom6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom6.CustomFormat = " "; }
        }

        private void dtpTo6_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo6.CustomFormat = null;
        }

        private void dtpTo6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo6.CustomFormat = " "; }
        }

        private void btnDownload4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strBrowse4 = @"SELECT [类型], [出境备案单号], [备案ID], [中文品名], [Item No], [净重], [进境备案单号], [放行日期审核时间], [原料备件号], [凭单ID], " +
                                 "[出区凭单], [备案时间], [凭单状态], [卡口过机日期], [Remark], [Created Date] FROM C_RMShareOut WHERE";

            if (!String.IsNullOrEmpty(this.dtpTo5.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom5.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom5.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo5.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for 2nd release date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom5.Focus();
                        return;
                    }
                    else
                    { strBrowse4 += " [放行日期审核时间] >= '" + Convert.ToDateTime(this.dtpFrom5.Value.ToString("M/d/yyyy")) + "' AND [放行日期审核时间] < '" + Convert.ToDateTime(this.dtpTo5.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else
                { strBrowse4 += " [放行日期审核时间] < '" + Convert.ToDateTime(this.dtpTo5.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom5.Text.Trim()))
                { strBrowse4 += " [放行日期审核时间] >= '" + Convert.ToDateTime(this.dtpFrom5.Value.ToString("M/d/yyyy")) + "' AND"; }
            }

            if (!String.IsNullOrEmpty(this.dtpTo6.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom6.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom6.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo6.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for pass gate time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom6.Focus();
                        return;
                    }
                    else
                    { strBrowse4 += " [卡口过机日期] >= '" + Convert.ToDateTime(this.dtpFrom6.Value.ToString("M/d/yyyy")) + "' AND [卡口过机日期] < '" + Convert.ToDateTime(this.dtpTo6.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else
                { strBrowse4 += " [卡口过机日期] < '" + Convert.ToDateTime(this.dtpTo6.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom6.Text.Trim()))
                { strBrowse4 += " [卡口过机日期] >= '" + Convert.ToDateTime(this.dtpFrom6.Value.ToString("M/d/yyyy")) + "' AND"; }
            }

            if (String.Compare(strBrowse4.Substring(strBrowse4.Trim().Length - 6, 6), " WHERE") == 0)
            { strBrowse4 = strBrowse4.Remove(strBrowse4.Trim().Length - 6); }
            if (String.Compare(strBrowse4.Substring(strBrowse4.Trim().Length - 4, 4), " AND") == 0)
            { strBrowse4 = strBrowse4.Remove(strBrowse4.Trim().Length - 4); }
            strBrowse4 += " ORDER BY [Created Date] DESC";

            SqlConnection SqlConn4 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn4.State == ConnectionState.Closed) { SqlConn4.Open(); }
            SqlDataAdapter browseAdapter4 = new SqlDataAdapter(strBrowse4, SqlConn4);
            DataTable dtBrowse4 = new DataTable();
            browseAdapter4.Fill(dtBrowse4);
            browseAdapter4.Dispose();

            if (dtBrowse4.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(dtBrowse4.Rows.Count / PageRow);
                if (iPageCount * PageRow < dtBrowse4.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\EHB RM Share Out Report " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dtBrowse4.Columns.Count; n++)
                        { sb.Append(dtBrowse4.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < dtBrowse4.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < dtBrowse4.Columns.Count; j++)
                            {
                                if (j == 1 || j == 6 || j == 10) { sb.Append("'" + dtBrowse4.Rows[i][j].ToString().Trim() + "\t"); }
                                else if (j == 7 || j == 11 || j == 13 || j == 15)
                                {
                                    string strObj = dtBrowse4.Rows[i][j].ToString().Trim().Trim();
                                    if (!String.IsNullOrEmpty(strObj)) { strObj = strObj.Substring(0, strObj.Trim().Length - 11); }
                                    sb.Append(strObj + "\t");
                                }
                                else { sb.Append(dtBrowse4.Rows[i][j].ToString().Trim() + "\t"); }
                            }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    MessageBox.Show("Successfully generated 'EHB RM Share Out Report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }

            dtBrowse4.Clear();
            dtBrowse4.Dispose();
            if (SqlConn4.State == ConnectionState.Open)
            {
                SqlConn4.Close();
                SqlConn4.Dispose();
            }
        }
        #endregion

        #region //EHB FG Out ( BeiAnDan ) & RMB PingDan
        private void dtpFrom7_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom7.CustomFormat = null;
        }

        private void dtpFrom7_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom7.CustomFormat = " "; }
        }

        private void dtpTo7_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo7.CustomFormat = null;
        }

        private void dtpTo7_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo7.CustomFormat = " "; }
        }

        private void btnDownload5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strSQL3 = @"SELECT B.[BeiAnDan No] AS [备案单号], B.[Order No] AS [订单号], B.[ESS/LINE], B.[BeiAnDan ID] AS [备案单ID], B.[CHN Name] AS [中文品名], " +
                              "B.[GongDan No] AS [工单号], CAST(B.[Customs Release Date] AS datetime) AS [放行日期审核时间], B.[FG EHB] AS [成品备件号], P.[PingDan ID] AS " + 
                              "[凭单ID], P.[PingDan No] AS [凭单号], P.[PingDan Qty] AS [凭单净重], B.[GongDan Qty] AS [备案单净重], P.[PingDan Date] AS [凭单日期], " +
                              "P.[Pass Gate Time] AS [卡口过机日期], IIF(B.[OF Rev Amt] > 0.0, B.[OF Rev Amt], IIF(B.[IE Rev Amt] > 0.0, B.[IE Rev Amt], B.[Selling Amount])) " +
                              "AS [Amount], B.[Order Category], B.[IE Remark] + ' / ' + B.[IE Remark_2] + ' / ' + B.[OF Remark] + ' / ' + B.[OF Remark_2] AS [Remark], " +
                              "B.[IE Type] AS [Type] FROM C_PingDan AS P RIGHT JOIN C_BeiAnDan AS B ON P.[GongDan No] = B.[GongDan No] " + 
                              "WHERE B.[Customs Release Date] IS NOT NULL AND B.[Customs Release Date] <> ''";

            string strBrowse5 = null;
            if (!String.IsNullOrEmpty(this.dtpTo7.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom7.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom7.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo7.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for 2nd release date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom7.Focus();
                        return;
                    }
                    else
                    { strBrowse5 += " AND CAST([Customs Release Date] AS datetime) >= '" + Convert.ToDateTime(this.dtpFrom7.Value.ToString("M/d/yyyy")) + "' AND CAST([Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo7.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse5 += " AND CAST([Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo7.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom7.Text.Trim()))
                { strBrowse5 += " AND CAST([Customs Release Date] AS datetime) >= '" + Convert.ToDateTime(this.dtpFrom7.Value.ToString("M/d/yyyy")) + "'"; }
            }
            if (!String.IsNullOrEmpty(strBrowse5)) { strSQL3 += strBrowse5 + " ORDER BY B.[BeiAnDan ID], B.[GongDan No]"; }

            SqlConnection SqlConn5 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn5.State == ConnectionState.Closed) { SqlConn5.Open(); }
            SqlDataAdapter browseAdapter5 = new SqlDataAdapter(strSQL3, SqlConn5);
            DataTable dtBrowse5 = new DataTable();
            browseAdapter5.Fill(dtBrowse5);
            browseAdapter5.Dispose();

            if (dtBrowse5.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtBrowse5.Dispose();
                SqlConn5.Close();
                SqlConn5.Dispose();
                return;
            }

            DataTable dtMiddle4 = new DataTable();
            dtMiddle4.Columns.Add("Type", typeof(string));
            dtMiddle4.Columns.Add("类型", typeof(string));
            DataRow dr = dtMiddle4.NewRow();
            dr[0] = "1418";
            dr[1] = "加工转仓储";
            dtMiddle4.Rows.Add(dr);

            dr = dtMiddle4.NewRow();
            dr[0] = "BLP";
            dr[1] = "成品出口";
            dtMiddle4.Rows.Add(dr);

            dr = dtMiddle4.NewRow();
            dr[0] = "EXPORT";
            dr[1] = "成品出口";
            dtMiddle4.Rows.Add(dr);

            dr = dtMiddle4.NewRow();
            dr[0] = "RMB";
            dr[1] = "成品内销";
            dtMiddle4.Rows.Add(dr);

            dr = dtMiddle4.NewRow();
            dr[0] = "RMB-1418";
            dr[1] = "加工转仓储";
            dtMiddle4.Rows.Add(dr);

            dr = dtMiddle4.NewRow();
            dr[0] = "RMB-D";
            dr[1] = "加工转仓储";
            dtMiddle4.Rows.Add(dr);

            dr = dtMiddle4.NewRow();
            dr[0] = "RM-D";
            dr[1] = "分送集报";
            dtMiddle4.Rows.Add(dr);

            SqlLib sqlLib2 = new SqlLib();
            DataTable dtMiddle5 = sqlLib2.MergeDataTable(dtBrowse5, dtMiddle4, "Type");
            sqlLib2.Dispose(0);
            dtMiddle4.Dispose();
            dtBrowse5.Dispose();
            dtMiddle5.Columns.Remove("Type");
            dtMiddle5.Columns["类型"].SetOrdinal(0);

            int PageRow = 65536;
            int iPageCount = (int)(dtMiddle5.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtMiddle5.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\EHB FG Out ( BeiAnDan ) " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtMiddle5.Columns.Count; n++)
                    { sb.Append(dtMiddle5.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < dtMiddle5.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtMiddle5.Columns.Count; j++)
                        {
                            if (j == 1 || j == 6 || j == 10) { sb.Append("'" + dtMiddle5.Rows[i][j].ToString().Trim() + "\t"); }
                            else if (j == 2) { sb.Append(System.Text.RegularExpressions.Regex.Replace(dtMiddle5.Rows[i][j].ToString().Trim(), @"\s", "") + "\t"); }
                            else if (j == 7 || j == 13 || j == 14)
                            {
                                string strObj = dtMiddle5.Rows[i][j].ToString().Trim();
                                if (!String.IsNullOrEmpty(strObj)) { strObj = strObj.Substring(0, strObj.Trim().Length - 11); }
                                sb.Append(strObj + "\t");
                            }
                            else { sb.Append(dtMiddle5.Rows[i][j].ToString().Trim() + "\t"); }
                        }
                        sb.Append(Environment.NewLine);
                    }

                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                MessageBox.Show("Successfully generated 'EHB FG Out ( BeiAnDan ) report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            dtMiddle5.Clear();
            dtMiddle5.Dispose();
            if (SqlConn5.State == ConnectionState.Open)
            {
                SqlConn5.Close();
                SqlConn5.Dispose();
            }
        }

        private void dtpPassGateFrom_ValueChanged(object sender, EventArgs e)
        {
            this.dtpPassGateFrom.CustomFormat = null;
        }

        private void dtpPassGateFrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpPassGateFrom.CustomFormat = " "; }
        }

        private void dtpPassGateTo_ValueChanged(object sender, EventArgs e)
        {
            this.dtpPassGateTo.CustomFormat = null;
        }

        private void dtpPassGateTo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpPassGateTo.CustomFormat = " "; }
        }

        private void btnDownloadPD_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strSQLPD = @"SELECT N'成品内销' AS [类型], B.[BeiAnDan No] AS [备案单号], B.[Order No] AS [订单号], B.[ESS/LINE], B.[BeiAnDan ID] AS [备案单ID], " +
                               "B.[CHN Name] AS [中文品名], B.[GongDan No] AS [工单号], CAST(B.[Customs Release Date] AS datetime) AS [放行日期审核时间], B.[FG EHB] " +
                               "AS [成品备件号], P.[PingDan ID] AS [凭单ID], P.[PingDan No] AS [凭单号], P.[PingDan Qty] AS [凭单净重], B.[GongDan Qty] AS [备案单净重], " + 
                               "P.[PingDan Date] AS [凭单日期], P.[Pass Gate Time] AS [卡口过机日期], " +
                               "IIF(B.[OF Rev Amt] > 0.0, B.[OF Rev Amt], IIF(B.[IE Rev Amt] > 0.0, B.[IE Rev Amt], B.[Selling Amount])) AS [Amount], B.[Order Category] " +
                               "FROM C_PingDan AS P LEFT OUTER JOIN C_BeiAnDan AS B ON P.[GongDan No] = B.[GongDan No] WHERE P.[IE Type] = 'RMB'";

            string strBrowsePD = null;
            if (!String.IsNullOrEmpty(this.dtpPassGateTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpPassGateFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpPassGateFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpPassGateTo.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for 2nd release date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpPassGateFrom.Focus();
                        return;
                    }
                    else
                    { strBrowsePD += " AND P.[Pass Gate Time] >= '" + Convert.ToDateTime(this.dtpPassGateFrom.Value.ToString("M/d/yyyy")) + "' AND P.[Pass Gate Time] < '" + Convert.ToDateTime(this.dtpPassGateTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowsePD += " AND P.[Pass Gate Time] < '" + Convert.ToDateTime(this.dtpPassGateTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpPassGateFrom.Text.Trim()))
                { strBrowsePD += " AND P.[Pass Gate Time] >= '" + Convert.ToDateTime(this.dtpPassGateFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }
            if (!String.IsNullOrEmpty(strBrowsePD)) { strSQLPD += strBrowsePD + " ORDER BY B.[BeiAnDan ID], B.[GongDan No]"; }

            SqlConnection SqlConnPD = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConnPD.State == ConnectionState.Closed) { SqlConnPD.Open(); }
            SqlDataAdapter browseAdapterPD = new SqlDataAdapter(strSQLPD, SqlConnPD);
            DataTable dtBrowsePD = new DataTable();
            browseAdapterPD.Fill(dtBrowsePD);
            browseAdapterPD.Dispose();

            if (dtBrowsePD.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtBrowsePD.Dispose();
                SqlConnPD.Close();
                SqlConnPD.Dispose();
                return;
            }

            int PageRow = 65536;
            int iPageCount = (int)(dtBrowsePD.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtBrowsePD.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\RMB PingDan " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtBrowsePD.Columns.Count; n++)
                    { sb.Append(dtBrowsePD.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < dtBrowsePD.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtBrowsePD.Columns.Count; j++)
                        {
                            if (j == 1 || j == 6 || j == 10) { sb.Append("'" + dtBrowsePD.Rows[i][j].ToString().Trim() + "\t"); }
                            else if (j == 2) { sb.Append(System.Text.RegularExpressions.Regex.Replace(dtBrowsePD.Rows[i][j].ToString().Trim(), @"\s", "") + "\t"); }
                            else if (j == 7 || j == 13 || j == 14)
                            {
                                string strObj = dtBrowsePD.Rows[i][j].ToString().Trim();
                                if (!String.IsNullOrEmpty(strObj)) { strObj = strObj.Substring(0, strObj.Trim().Length - 11); }
                                sb.Append(strObj + "\t");
                            }
                            else { sb.Append(dtBrowsePD.Rows[i][j].ToString().Trim() + "\t"); }
                        }
                        sb.Append(Environment.NewLine);
                    }

                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                MessageBox.Show("Successfully generated 'RMB PingDan report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            if (SqlConnPD.State == ConnectionState.Open)
            {
                SqlConnPD.Close();
                SqlConnPD.Dispose();
            }
        }
        #endregion

        #region //GongDan RM Usage State & GongDan Balance
        private void dtpFrom8_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom8.CustomFormat = null;
        }

        private void dtpFrom8_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom8.CustomFormat = " "; }
        }

        private void dtpTo8_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo8.CustomFormat = null;
        }

        private void dtpTo8_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo8.CustomFormat = " "; }
        }

        private void btnDownload6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strBrowse6 = @"SELECT M.[GongDan No] AS [工单号], M.[Batch No], CASE WHEN M.[BOM In Customs]<> '' AND M.[BOM In Customs] IS NOT NULL THEN " +
                                 "M.[BOM In Customs] ELSE M.[FG No] + '/' + M.[Batch No] END AS [成品备件号], M.[GongDan Qty] AS [工单数量], D.[Line No] AS [项号], " + 
                                 "D.[RM Customs Code] AS [原料备件号], D.[BGD No] AS 批次号, D.[RM Used Qty] AS [原料耗用数量], M.[Created Date] AS [工单创建日期], " + 
                                 "M.[Approved Date] AS [工单完成日期], B.[Customs Release Date] AS [备案单二放日期], M.Remark AS [工单Remark], " + 
                                 "CASE WHEN B.[Customs Release Date] <> '' And B.[Customs Release Date] IS NOT NULL THEN M.[GongDan Qty] - M.[BeiAnDan Used Qty] " + 
                                 "ELSE NULL END AS [二放后工单成品剩余数量] FROM C_BeiAnDan AS B RIGHT JOIN (C_GongDanDetail AS D RIGHT JOIN C_GongDan AS M ON " + 
                                 "D.[GongDan No] = M.[GongDan No]) ON B.[GongDan No] = M.[GongDan No]";

            if (!String.IsNullOrEmpty(this.dtpTo8.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom8.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom8.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo8.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for gongdan created date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom8.Focus();
                        return;
                    }
                    else
                    { strBrowse6 += " WHERE [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom8.Value.ToString("M/d/yyyy")) + "' AND [Created Date] < '" + Convert.ToDateTime(this.dtpTo8.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse6 += " WHERE [Created Date] < '" + Convert.ToDateTime(this.dtpTo8.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom8.Text.Trim()))
                { strBrowse6 += " WHERE [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom8.Value.ToString("M/d/yyyy")) + "'"; }
            }
            strBrowse6 += " ORDER BY M.[GongDan No], D.[Line No]";

            SqlConnection SqlConn6 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn6.State == ConnectionState.Closed) { SqlConn6.Open(); }
            SqlDataAdapter browseAdapter6 = new SqlDataAdapter(strBrowse6, SqlConn6);
            DataTable dtBrowse6 = new DataTable();
            browseAdapter6.Fill(dtBrowse6);

            if (dtBrowse6.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                browseAdapter6.Dispose();
                dtBrowse6.Dispose();
                SqlConn6.Close();
                SqlConn6.Dispose();
                return;
            }

            strBrowse6 = @"SELECT [GongDan No] AS [工单号], ([GongDan Qty] - [PingDan Used Qty]) AS [出区后工单成品剩余数量] FROM (SELECT [GongDan No], [GongDan Qty], " + 
                          "SUM([PingDan Qty]) AS [PingDan Used Qty] FROM C_PingDan WHERE [Pass Gate Time] IS NOT NULL GROUP BY [GongDan No], [GongDan Qty]) AS X";
            browseAdapter6 = new SqlDataAdapter(strBrowse6, SqlConn6);
            DataTable dtMiddle6 = new DataTable();
            browseAdapter6.Fill(dtMiddle6);
            browseAdapter6.Dispose();

            SqlLib sqlLib3 = new SqlLib();
            string[] str = { "工单号", "Batch No", "成品备件号", "工单数量", "工单创建日期", "工单完成日期", "备案单二放日期", "工单Remark", "二放后工单成品剩余数量" };
            DataTable dtMiddle7 = sqlLib3.SelectDistinct(dtBrowse6, str);
            DataTable dtMiddle8 = sqlLib3.MergeDataTable(dtMiddle7, dtMiddle6, "工单号");
            sqlLib3.Dispose(0);
            dtMiddle6.Dispose();
            dtMiddle7.Dispose();

            dtMiddle8.Columns.Add("原料耗用数量", typeof(decimal));
            dtMiddle8.Columns["原料耗用数量"].SetOrdinal(4);
            foreach (DataRow dr in dtMiddle8.Rows)
            {
                decimal dRMQty = Convert.ToDecimal(dtBrowse6.Compute("SUM([原料耗用数量])", "[工单号] = '" + dr[0].ToString().Trim() + "'").ToString().Trim());
                dr[4] = Math.Round(dRMQty, 6);
            }
            dtMiddle8.AcceptChanges();

            int PageRow = 65536;
            int iPageCount = (int)(dtBrowse6.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtBrowse6.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\GongDan RM Usage State " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtBrowse6.Columns.Count - 1; n++)
                    { sb.Append(dtBrowse6.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < dtBrowse6.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtBrowse6.Columns.Count - 1; j++)
                        {
                            if (j == 0 || j == 1 || j == 6) { sb.Append("'" + dtBrowse6.Rows[i][j].ToString().Trim() + "\t"); }
                            else if (j == 8 || j == 9 || j == 10)
                            {
                                string strObj = dtBrowse6.Rows[i][j].ToString().Trim();
                                if (!String.IsNullOrEmpty(strObj)) { strObj = strObj.Substring(0, strObj.Trim().Length - 11); }
                                sb.Append(strObj + "\t");
                            }
                            else { sb.Append(dtBrowse6.Rows[i][j].ToString().Trim() + "\t"); }
                        }
                        sb.Append(Environment.NewLine);
                    }

                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }

                iPageCount = (int)(dtMiddle8.Rows.Count / PageRow);
                if (iPageCount * PageRow < dtMiddle8.Rows.Count) { iPageCount += 1; }
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\GongDan Balance " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtMiddle8.Columns.Count; n++)
                    { sb.Append(dtMiddle8.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < dtMiddle8.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtMiddle8.Columns.Count; j++)
                        {
                            if (j == 0 || j == 1) { sb.Append("'" + dtMiddle8.Rows[i][j].ToString().Trim() + "\t"); }
                            else if (j == 5 || j == 6 || j == 7)
                            {
                                string strObj = dtMiddle8.Rows[i][j].ToString().Trim();
                                if (!String.IsNullOrEmpty(strObj)) { strObj = strObj.Substring(0, strObj.Trim().Length - 11); }
                                sb.Append(strObj + "\t");
                            }
                            else { sb.Append(dtMiddle8.Rows[i][j].ToString().Trim() + "\t"); }
                        }
                        sb.Append(Environment.NewLine);
                    }

                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                MessageBox.Show("Successfully generated 'GongDan RM Usage State & GongDan Balance report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            dtBrowse6.Clear();
            dtBrowse6.Dispose();
            if (SqlConn6.State == ConnectionState.Open)
            {
                SqlConn6.Close();
                SqlConn6.Dispose();
            }
        }
        #endregion

        #region //RM Balance By RM Customs Code
        private void dtpFrom9_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom9.CustomFormat = null;
        }

        private void dtpFrom9_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom9.CustomFormat = " "; }
        }

        private void dtpTo9_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo9.CustomFormat = null;
        }

        private void dtpTo9_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo9.CustomFormat = " "; }
        }

        private void btnDownload7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strBrowse7 = null, strBrowse8 = null;
            if (!String.IsNullOrEmpty(this.dtpTo9.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom9.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom9.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo9.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for 2nd release date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom9.Focus();
                        return;
                    }
                    else
                    { 
                        strBrowse7 += " [2nd Release Date] >= '" + Convert.ToDateTime(this.dtpFrom9.Value.ToString("M/d/yyyy")) + "' AND [2nd Release Date] < '" + Convert.ToDateTime(this.dtpTo9.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                        strBrowse8 += " AND [放行日期审核时间] >= '" + Convert.ToDateTime(this.dtpFrom9.Value.ToString("M/d/yyyy")) + "' AND [放行日期审核时间] < '" + Convert.ToDateTime(this.dtpTo9.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                    }
                }
                else
                { 
                    strBrowse7 += " [2nd Release Date] < '" + Convert.ToDateTime(this.dtpTo9.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                    strBrowse8 += " AND [放行日期审核时间] < '" + Convert.ToDateTime(this.dtpTo9.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; 
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom9.Text.Trim()))
                { 
                    strBrowse7 += " [2nd Release Date] >= '" + Convert.ToDateTime(this.dtpFrom9.Value.ToString("M/d/yyyy")) + "'";
                    strBrowse8 += " AND [放行日期审核时间] >= '" + Convert.ToDateTime(this.dtpFrom9.Value.ToString("M/d/yyyy")) + "'"; 
                }
            }

            string strSQL4 = @"SELECT [RM Customs Code] AS [备件号], [RM CHN Name] AS [中文品名], CASE WHEN CHARINDEX('R', [Lot No]) > 0 THEN  N'非保料件' ELSE " + 
                              "N'保税料件' END AS [货物类型], CASE WHEN [Transaction Type] = N'非保' THEN [PO Invoice Qty] ELSE 0.0 END AS [PO Invoice Qty1], " +
                              "CASE WHEN [Transaction Type] = N'进境' THEN [PO Invoice Qty] ELSE 0.0 END AS [PO Invoice Qty2], CASE WHEN [Transaction Type] = N'区内入库' " + 
                              "THEN [PO Invoice Qty] ELSE 0.0 END AS [PO Invoice Qty3], CAST([Customs Rcvd Date] AS datetime) AS [2nd Release Date] FROM C_RMPurchase " + 
                              "WHERE CHARINDEX('/', [Customs Rcvd Date]) > 0";

            string strSQL5 = @"SELECT [RM Customs Code] AS [备件号], [RM CHN Name] AS [中文品名], [Goods Type]AS [货物类型], CASE WHEN [Type] = N'非保' THEN [PO Invoice Qty] " + 
                              "ELSE 0.0 END AS [PO Invoice Qty1], CASE WHEN [Type] = N'进境' THEN [PO Invoice Qty] ELSE 0.0 END AS [PO Invoice Qty2], CASE WHEN " + 
                              "[Type] = N'区内入库' THEN [PO Invoice Qty] ELSE 0.0 END AS [PO Invoice Qty3], [2nd Release Date] FROM C_RMPurchase_Manual";

            SqlConnection SqlConn7 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn7.State == ConnectionState.Closed) { SqlConn7.Open(); }
            SqlDataAdapter SqlAdapter7 = new SqlDataAdapter(strSQL4, SqlConn7);
            DataTable dtBrowse7 = new DataTable();
            SqlAdapter7.Fill(dtBrowse7);
            SqlAdapter7 = new SqlDataAdapter(strSQL5, SqlConn7);
            DataTable dtMerge = new DataTable();
            SqlAdapter7.Fill(dtMerge);
            if(dtMerge.Rows.Count > 0) { dtBrowse7.Merge(dtMerge); }
            dtMerge.Dispose();
            DataView dv = dtBrowse7.DefaultView;
            dv.RowFilter = strBrowse7;
            DataTable dtMiddle9 = dv.ToTable().Copy();
            dtBrowse7.Clear();
            dtBrowse7.Dispose();

            if (dtMiddle9.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SqlAdapter7.Dispose();
                dtMiddle9.Dispose();
                SqlConn7.Close();
                SqlConn7.Dispose();
                return;
            }

            dtMiddle9.Columns.Remove("2nd Release Date");
            SqlLib sqlLib4 = new SqlLib();
            string[] str1 = { "备件号", "中文品名", "货物类型" };
            DataTable dtMiddle10 = sqlLib4.SelectDistinct(dtMiddle9, str1);
            dtMiddle10.Columns.Add("非保进区", typeof(decimal));
            dtMiddle10.Columns.Add("料件报关进区", typeof(decimal));
            dtMiddle10.Columns.Add("料件结转转入", typeof(decimal));
            foreach (DataRow dr in dtMiddle10.Rows)
            {
                dr["非保进区"] = Convert.ToDecimal(dtMiddle9.Compute("SUM([PO Invoice Qty1])", @"[备件号] = '" + dr[0].ToString() + "' AND [中文品名] = '" + dr[1].ToString() 
                                                                      + "' AND [货物类型] = '" + dr[2].ToString() + "'").ToString());
                dr["料件报关进区"] = Convert.ToDecimal(dtMiddle9.Compute("SUM([PO Invoice Qty2])", @"[备件号] = '" + dr[0].ToString() + "' AND [中文品名] = '" + dr[1].ToString()
                                                                      + "' AND [货物类型] = '" + dr[2].ToString() + "'").ToString());
                dr["料件结转转入"] = Convert.ToDecimal(dtMiddle9.Compute("SUM([PO Invoice Qty3])", @"[备件号] = '" + dr[0].ToString() + "' AND [中文品名] = '" + dr[1].ToString()
                                                                      + "' AND [货物类型] = '" + dr[2].ToString() + "'").ToString());
            }
            dtMiddle9.Clear();
            dtMiddle9.Dispose();
            dtMiddle10.Columns.Add("期初库存", typeof(decimal));
            dtMiddle10.Columns.Add("料件进库", typeof(decimal));
            dtMiddle10.Columns.Add("料件出库", typeof(decimal));
            dtMiddle10.Columns.Add("成品出库耗用", typeof(decimal));
            dtMiddle10.Columns.Add("后续补税(料件)", typeof(decimal));
            dtMiddle10.Columns.Add("库存", typeof(decimal));
            dtMiddle10.Columns.Add("是否注销", typeof(string));

            dtMiddle10.Columns["期初库存"].SetOrdinal(2);
            dtMiddle10.Columns["料件进库"].SetOrdinal(3);
            dtMiddle10.Columns["料件出库"].SetOrdinal(4);
            dtMiddle10.Columns["成品出库耗用"].SetOrdinal(5);
            dtMiddle10.Columns["后续补税(料件)"].SetOrdinal(6);
            dtMiddle10.Columns["库存"].SetOrdinal(7);
            dtMiddle10.Columns["是否注销"].SetOrdinal(9);

            string strSQL6 = @"SELECT [备件号], [货物类型2], SUM([净重1]) AS [料件退运], SUM([净重2]) AS [料件退仓] FROM (SELECT B.[原料备件号] AS [备件号], " +
                              "C.[Goods Type] AS [货物类型2], CASE WHEN B.[类型] = N'料件退运' THEN B.[净重] ELSE 0 END AS [净重1], CASE WHEN B.[类型] = N'料件退仓' " +
                              "THEN B.[净重] ELSE 0 END AS [净重2] FROM C_RMShareOut AS B LEFT JOIN B_ChnDescription AS C ON  B.[原料备件号] = C.[RM Customs Code] " +
                              "WHERE B.[放行日期审核时间] IS NOT NULL " + strBrowse8 + ") AS X GROUP BY [备件号], [货物类型2]";
            SqlAdapter7 = new SqlDataAdapter(strSQL6, SqlConn7);
            DataTable dtMiddle11 = new DataTable();
            SqlAdapter7.Fill(dtMiddle11);

            DataTable dtMiddle12 = new DataTable();
            if (dtMiddle11.Rows.Count == 0)
            {
                dtMiddle10.Columns.Add("料件退运", typeof(decimal));
                dtMiddle10.Columns.Add("料件退仓", typeof(decimal));
                dtMiddle12 = dtMiddle10.Copy();
            }
            else 
            { 
                dtMiddle12 = sqlLib4.MergeDataTable2(dtMiddle10, dtMiddle11, "备件号", "货物类型2");
                dtMiddle12.Columns.Remove("货物类型2");
                DataRow[] drow = dtMiddle11.Select("[AsIs] IS NULL");
                if (drow.Length > 0)
                {
                    foreach (DataRow row in drow)
                    {
                        DataRow dr = dtMiddle12.NewRow();
                        string strRMEHB = row["备件号"].ToString().Trim();
                        string strCHName = null;
                        DataRow[] datar = dtMiddle12.Select("[备件号] = '" + strRMEHB + "'");
                        if (datar.Length > 0) { strCHName = datar[0]["中文品名"].ToString().Trim(); }
                        dr["备件号"] = strRMEHB;
                        dr["中文品名"] = String.IsNullOrEmpty(strCHName) ? String.Empty : strCHName;
                        dr["货物类型"] = row["货物类型2"].ToString().Trim();
                        dr["非保进区"] = 0.0M;
                        dr["料件报关进区"] = 0.0M;
                        dr["料件结转转入"] = 0.0M;
                        dr["料件退运"] = Convert.ToDecimal(row["料件退运"].ToString().Trim());
                        dr["料件退仓"] = Convert.ToDecimal(row["料件退仓"].ToString().Trim());
                        dtMiddle12.Rows.Add(dr);
                    }
                    dtMiddle12.AcceptChanges();
                }
            }
            dtMiddle10.Clear();
            dtMiddle10.Dispose();
            dtMiddle11.Clear();
            dtMiddle11.Dispose();

            string strSQL7 = @"SELECT * FROM (SELECT D.[RM Customs Code] AS [备件号], C.[Goods Type] AS [货物类型3], CASE WHEN B.[IE Type] IN ('EXPORT', 'BLP') " +
                              "THEN D.[RM Used Qty] ELSE 0 END AS [RM Used Qty1], CASE WHEN B.[IE Type] = N'RMB' THEN D.[RM Used Qty] ELSE 0 END AS [RM Used Qty2], " +
                              "CASE WHEN B.[IE Type] IN ('1418', 'RMB-1418', 'RMB-D') THEN D.[RM Used Qty] ELSE 0 END AS [RM Used Qty3], " +
                              "CAST(B.[Customs Release Date] AS datetime) AS [2nd Release Date] FROM C_GongDanDetail AS D LEFT OUTER JOIN B_ChnDescription AS C ON " +
                              "D.[RM Customs Code] = C.[RM Customs Code] INNER JOIN C_BeiAnDan AS B ON D.[GongDan No] = B.[GongDan No] " +
                              "WHERE B.[Customs Release Date] <> '' AND B.[Customs Release Date] IS NOT NULL) AS XX WHERE " + strBrowse7;
            SqlAdapter7 = new SqlDataAdapter(strSQL7, SqlConn7);
            DataTable dtMiddle13 = new DataTable();
            SqlAdapter7.Fill(dtMiddle13);

            dtMiddle13.Columns.Remove("2nd Release Date");
            string[] str2 = { "备件号", "货物类型3" };
            DataTable dtMiddle14 = sqlLib4.SelectDistinct(dtMiddle13, str2);
            dtMiddle14.Columns.Add("成品报关出区耗用", typeof(decimal));
            dtMiddle14.Columns.Add("成品内销(料件)耗用", typeof(decimal));
            dtMiddle14.Columns.Add("成品移库耗用", typeof(decimal));

            foreach (DataRow row in dtMiddle14.Rows)
            {
                string strGoodsType = String.IsNullOrEmpty(row[1].ToString().Trim()) ? String.Empty : row[1].ToString().Trim();
                row["成品报关出区耗用"] = Math.Round(Convert.ToDecimal(dtMiddle13.Compute("SUM([RM Used Qty1])", "[备件号] = '" + row[0].ToString()
                                         + "' AND [货物类型3] = '" + strGoodsType + "'")), 6);
                row["成品内销(料件)耗用"] = Math.Round(Convert.ToDecimal(dtMiddle13.Compute("SUM([RM Used Qty2])", "[备件号] = '" + row[0].ToString()
                                           + "' AND [货物类型3] = '" + strGoodsType + "'")), 6);
                row["成品移库耗用"] = Math.Round(Convert.ToDecimal(dtMiddle13.Compute("SUM([RM Used Qty3])", "[备件号] = '" + row[0].ToString()
                                     + "' AND [货物类型3] = '" + strGoodsType + "'")), 6);
            }
            dtMiddle14.AcceptChanges();
            dtMiddle13.Clear();
            dtMiddle13.Dispose();

            DataTable dtMiddle15 = sqlLib4.MergeDataTable2(dtMiddle12, dtMiddle14, "备件号", "货物类型3");
            sqlLib4.Dispose(0);
            dtMiddle12.Clear();
            dtMiddle12.Dispose();
            dtMiddle15.Columns.Remove("货物类型3");
            dtMiddle15.Columns["成品报关出区耗用"].SetOrdinal(13);
            dtMiddle15.Columns["成品内销(料件)耗用"].SetOrdinal(14);
            dtMiddle15.Columns["成品移库耗用"].SetOrdinal(16);
            DataRow[] datarow = dtMiddle14.Select("[AsIs] IS NULL");
            if (datarow.Length > 0)
            {
                foreach (DataRow row in datarow)
                {
                    DataRow dr = dtMiddle15.NewRow();
                    string strRMEHB = row["备件号"].ToString().Trim();
                    string strCHName = null;
                    DataRow[] datar = dtMiddle15.Select("[备件号] = '" + strRMEHB + "'");
                    if (datar.Length > 0) { strCHName = datar[0]["中文品名"].ToString().Trim(); }
                    dr["备件号"] = strRMEHB;
                    dr["中文品名"] = String.IsNullOrEmpty(strCHName) ? String.Empty : strCHName;
                    dr["货物类型"] = row["货物类型3"].ToString().Trim();
                    dr["非保进区"] = 0.0M;
                    dr["料件报关进区"] = 0.0M;
                    dr["料件结转转入"] = 0.0M;
                    dr["成品报关出区耗用"] = Convert.ToDecimal(row["成品报关出区耗用"].ToString().Trim());
                    dr["成品内销(料件)耗用"] = Convert.ToDecimal(row["成品内销(料件)耗用"].ToString().Trim());
                    dr["成品移库耗用"] = Convert.ToDecimal(row["成品移库耗用"].ToString().Trim());
                    dtMiddle15.Rows.Add(dr);
                }
            }
            dtMiddle15.AcceptChanges();
            dtMiddle14.Clear();
            dtMiddle14.Dispose();

            DataView dataview = dtMiddle15.DefaultView;
            dataview.Sort = "备件号 ASC";
            dtMiddle15 = dataview.ToTable();

            foreach (DataRow dr in dtMiddle15.Rows)
            {
                string strType1 = dr["非保进区"].ToString().Trim();
                string strType2 = dr["料件报关进区"].ToString().Trim();
                string strType3 = dr["料件结转转入"].ToString().Trim();
                decimal dType1 = String.IsNullOrEmpty(strType1) ? 0.0M : Convert.ToDecimal(strType1);
                decimal dType2 = String.IsNullOrEmpty(strType2) ? 0.0M : Convert.ToDecimal(strType2);
                decimal dType3 = String.IsNullOrEmpty(strType3) ? 0.0M : Convert.ToDecimal(strType3);

                string strTY = dr["料件退运"].ToString().Trim();
                string strTC = dr["料件退仓"].ToString().Trim();
                decimal dTY = String.IsNullOrEmpty(strTY) ? 0.0M : Convert.ToDecimal(strTY);
                decimal dTC = String.IsNullOrEmpty(strTC) ? 0.0M : Convert.ToDecimal(strTC);

                string strIE1 = dr["成品报关出区耗用"].ToString().Trim();
                string strIE2 = dr["成品内销(料件)耗用"].ToString().Trim();
                string strIE3 = dr["成品移库耗用"].ToString().Trim();
                decimal dIE1 = String.IsNullOrEmpty(strIE1) ? 0.0M : Convert.ToDecimal(strIE1);
                decimal dIE2 = String.IsNullOrEmpty(strIE2) ? 0.0M : Convert.ToDecimal(strIE2);
                decimal dIE3 = String.IsNullOrEmpty(strIE3) ? 0.0M : Convert.ToDecimal(strIE3);

                dr["期初库存"] = 0.0M;
                dr["料件进库"] = dType1 + dType2 + dType3;
                dr["料件出库"] = dTY + dTC;
                dr["成品出库耗用"] = dIE1 + dIE2 + dIE3;
                dr["后续补税(料件)"] = 0.0M;
                dr["库存"] = 0.0M;
                dr["是否注销"] = "否";
                dr["料件退运"] = dTY;
                dr["料件退仓"] = dTC;
                dr["成品报关出区耗用"] = dIE1;
                dr["成品内销(料件)耗用"] = dIE2;
                dr["成品移库耗用"] = dIE3;
            }
            dtMiddle15.AcceptChanges();

            int PageRow = 65536;
            int iPageCount = (int)(dtMiddle15.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtMiddle15.Rows.Count) { iPageCount += 1; }
            for (int m = 1; m <= iPageCount; m++)
            {
                string strPath = System.Windows.Forms.Application.StartupPath + "\\RM Balance By RM Customs Code " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                if (File.Exists(strPath)) { File.Delete(strPath); }
                Thread.Sleep(1000);
                StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                StringBuilder sb = new StringBuilder();
                for (int n = 0; n < dtMiddle15.Columns.Count; n++)
                { sb.Append(dtMiddle15.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                sb.Append(Environment.NewLine);

                for (int i = (m - 1) * PageRow; i < dtMiddle15.Rows.Count && i < m * PageRow; i++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    for (int j = 0; j < dtMiddle15.Columns.Count; j++)
                    { sb.Append(dtMiddle15.Rows[i][j].ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);
                }

                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            MessageBox.Show("Successfully generated 'RM Balance By RM Customs Code report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dtMiddle15.Clear();
            dtMiddle15.Dispose();
            if (SqlConn7.State == ConnectionState.Open)
            {
                SqlConn7.Close();
                SqlConn7.Dispose();
            }
        }
        #endregion

        #region //RM Balance By BGD No
        private void dtpFrom10_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom10.CustomFormat = null;
        }

        private void dtpFrom10_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; }
        }

        private void dtpTo10_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo10.CustomFormat = null;
        }

        private void dtpTo10_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo10.CustomFormat = " "; }
        }

        private void btnDownload8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strBrowse9 = null, strBrowse10 = null, strBrowse11 = null, strBrowse12 = null;
            if (!String.IsNullOrEmpty(this.dtpTo10.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom10.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo10.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for pingdan pass gate time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom10.Focus();
                        return;
                    }
                    else
                    {
                        strBrowse9 += " WHERE [Pass Gate Time] >= '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "' AND [Pass Gate Time] < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                        strBrowse10 += " AND CAST([Customs Release Date] AS datetime) >= '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "' AND CAST([Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; 
                        strBrowse11 += " AND [放行日期审核时间] >= '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "' AND [放行日期审核时间] < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                        strBrowse12 += " AND [Gate PassThrough Date] >= '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "' AND [Gate PassThrough Date] < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                    }                     
                }
                else
                {
                    strBrowse9 += " WHERE [Pass Gate Time] < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                    strBrowse10 += " AND CAST([Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; 
                    strBrowse11 += " AND [放行日期审核时间] < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                    strBrowse12 += " AND [Gate PassThrough Date] < '" + Convert.ToDateTime(this.dtpTo10.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'";
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom10.Text.Trim()))
                {
                    strBrowse9 += " WHERE [Pass Gate Time] >= '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "'";
                    strBrowse10 += " AND CAST([Customs Release Date] AS datetime) '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "'"; 
                    strBrowse11 += " AND [放行日期审核时间] >= '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "'";
                    strBrowse12 += " AND [Gate PassThrough Date] >= '" + Convert.ToDateTime(this.dtpFrom10.Value.ToString("M/d/yyyy")) + "'";
                }
            }

            string strSQL9 = @"SELECT [BGD No] + '/' + [RM Customs Code] AS [批次备件号], " +
                              "SUM(CAST(([RM Used Qty] * [PingDan Qty] / [GongDan Qty]) AS decimal(18,6))) AS [生产耗用数量] FROM (SELECT D.[RM Customs Code], D.[BGD No], " +
                              "D.[RM Used Qty], T.[GongDan Qty], T.[PingDan Qty] from C_GongDanDetail AS D INNER JOIN (SELECT P.[GongDan No], P.[GongDan Qty], " +
                              "P.[PingDan Qty] FROM C_PingDan AS P INNER JOIN C_BeiAnDan AS C ON P.[GongDan No] = C.[GongDan No] " + strBrowse9 + strBrowse10 + 
                              ") AS T ON D.[GongDan No] = T.[GongDan No]) AS X GROUP BY [BGD No], [RM Customs Code]";

            string strSQL8 = @"SELECT [批次备件号], SUM([退运数量]) AS [退运数量], SUM([退仓数量]) AS [退仓数量] FROM (SELECT [进境备案单号] + '/' + [原料备件号] AS [批次备件号], " + 
                              "CASE WHEN [类型] = N'料件退运' THEN [净重] ELSE 0.0 END AS [退运数量], CASE WHEN [类型] = N'料件退仓' THEN [净重] ELSE 0.0 END AS [退仓数量] FROM " + 
                              "C_RMShareOut WHERE [放行日期审核时间] IS NOT NULL" + strBrowse11 + ") AS XX GROUP BY [批次备件号]";

            string strSQL7 = @"SELECT [批次备件号], [单证类型], [货物类型], [中文品名], MAX([单价]) AS [单价], [币制], SUM([期初库存]) AS [期初库存], " +
                              "SUM([进库数量]) AS [进库数量] FROM (SELECT [BGD No] + '/' + [RM Customs Code] AS [批次备件号], [Transaction Type] AS [单证类型], " +
                              "[Goods Type] AS [货物类型], [RM CHN Name] AS [中文品名], MAX([RM Price(CIF)]) AS [单价], [RM Currency] AS [币制], " +
                              "CASE WHEN [BGD No] = N'AAAAAAAA' THEN SUM([PO Invoice Qty]) ELSE 0.0 END AS [期初库存], CASE WHEN [BGD No] <> N'AAAAAAAA' " +
                              "THEN SUM([PO Invoice Qty]) ELSE 0.0 END AS [进库数量] FROM (SELECT [BGD No], [RM Customs Code], [Transaction Type], [RM CHN Name], " +
                              "CASE WHEN CHARINDEX('R', [Lot No]) > 0 THEN N'非保料件' ELSE N'保税料件' END AS [Goods Type], [RM Price(CIF)], [RM Currency], " +
                              "[Original Country], [PO Invoice Qty] FROM C_RMPurchase WHERE CHARINDEX('/', [Customs Rcvd Date]) > 0 " + strBrowse12 + 
                              ") AS X GROUP BY [BGD No], [RM Customs Code], [Transaction Type], [RM CHN Name], [Goods Type], [RM Currency], [Original Country] " +
                              "UNION " +
                              "SELECT [BGD No] + '/' + [RM Customs Code] AS [批次备件号], [Transaction Type] AS [单证类型], [Goods Type] AS [货物类型], " +
                              "[RM CHN Name] AS [中文品名], MAX([RM Price(CIF)]) AS [单价], [RM Currency] AS [币制], CASE WHEN [BGD No] = N'AAAAAAAA' THEN " +
                              "SUM([PO Invoice Qty]) ELSE 0.0 END AS [期初库存], CASE WHEN [BGD No] <> N'AAAAAAAA' THEN " +
                              "SUM([PO Invoice Qty]) ELSE 0.0 END AS [进库数量] FROM (SELECT [BGD No], [RM Customs Code], [Type] AS [Transaction Type], [RM CHN Name], " +
                              "[Goods Type], [RM Price(CIF)], [RM Currency], [Original Country], [PO Invoice Qty] FROM C_RMPurchase_Manual " + strBrowse9 + 
                              ") AS Y GROUP BY [BGD No], [RM Customs Code], [Transaction Type], [RM CHN Name], [Goods Type], [RM Currency], [Original Country]) AS Z " +
                              "GROUP BY [批次备件号], [单证类型], [货物类型], [中文品名], [币制]";

            SqlConnection SqlConn8 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn8.State == ConnectionState.Closed) { SqlConn8.Open(); }
            SqlDataAdapter SqlAdater8 = new SqlDataAdapter(strSQL7, SqlConn8);
            DataTable dtBrowse8 = new DataTable();
            SqlAdater8.Fill(dtBrowse8);

            if (dtBrowse8.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SqlAdater8.Dispose();
                dtBrowse8.Clear();
                dtBrowse8.Dispose();
                SqlConn8.Close();
                SqlConn8.Dispose();
                return;
            }

            SqlAdater8 = new SqlDataAdapter(strSQL8, SqlConn8);
            DataTable dtBrowse9 = new DataTable();
            SqlAdater8.Fill(dtBrowse9);
            SqlAdater8 = new SqlDataAdapter(strSQL9, SqlConn8);
            DataTable dtBrowse10 = new DataTable();
            SqlAdater8.Fill(dtBrowse10);
            SqlAdater8.Dispose();

            SqlLib sqlLib5 = new SqlLib();
            DataTable dtMiddle16 = sqlLib5.MergeDataTable(dtBrowse8, dtBrowse9, "批次备件号");
            dtBrowse8.Clear();
            dtBrowse8.Dispose();
            dtBrowse9.Clear();
            dtBrowse9.Dispose();
            DataTable dtMiddle17 = sqlLib5.MergeDataTable(dtMiddle16, dtBrowse10, "批次备件号");
            dtMiddle16.Clear();
            dtMiddle16.Dispose();
            sqlLib5.Dispose(0);
            dtMiddle17.Columns["退仓数量"].SetOrdinal(10);
            dtMiddle17.Columns["退运数量"].SetOrdinal(9);
            dtMiddle17.Columns["生产耗用数量"].SetOrdinal(8);

            int PageRow = 65536;
            int iPageCount = (int)(dtMiddle17.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtMiddle17.Rows.Count) { iPageCount += 1; }
            for (int m = 1; m <= iPageCount; m++)
            {
                string strPath = System.Windows.Forms.Application.StartupPath + "\\RM Balance By BGD No " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                if (File.Exists(strPath)) { File.Delete(strPath); }
                Thread.Sleep(1000);
                StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                StringBuilder sb = new StringBuilder();
                for (int n = 0; n < dtMiddle17.Columns.Count; n++)
                { sb.Append(dtMiddle17.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                sb.Append(Environment.NewLine);

                for (int i = (m - 1) * PageRow; i < dtMiddle17.Rows.Count && i < m * PageRow; i++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    for (int j = 0; j < dtMiddle17.Columns.Count; j++)
                    { sb.Append(dtMiddle17.Rows[i][j].ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);
                }

                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            MessageBox.Show("Successfully generated 'RM Balance By BGD No report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dtMiddle17.Clear();
            dtMiddle17.Dispose();
            if (SqlConn8.State == ConnectionState.Open)
            {
                SqlConn8.Close();
                SqlConn8.Dispose();
            }
        }
        #endregion

        #region //RM Distribution in e-Handbook ( PingDan )
        private void dtpFrom11_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom11.CustomFormat = null;
        }

        private void dtpFrom11_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom11.CustomFormat = " "; }
        }

        private void dtpTo11_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo11.CustomFormat = null;
        }

        private void dtpTo11_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo11.CustomFormat = " "; }
        }

        private void btnDownload9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            string strBrowse10 = null;
            if (!String.IsNullOrEmpty(this.dtpTo11.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom11.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom11.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo11.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom11.Focus();
                        return;
                    }
                    else
                    { strBrowse10 += " AND C.[PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom11.Value.ToString("M/d/yyyy")) + "' AND C.[PingDan Date] < '" + Convert.ToDateTime(this.dtpTo11.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse10 += " AND C.[PingDan Date] < '" + Convert.ToDateTime(this.dtpTo11.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom11.Text.Trim()))
                { strBrowse10 += " AND C.[PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom11.Value.ToString("M/d/yyyy")) + "'"; }
            }
            string strSQL10 = @"SELECT C.*, B.[Duty Rate] FROM C_PingDan AS C LEFT JOIN (SELECT DISTINCT [CHN Name], [Duty Rate] FROM B_HS) AS B ON " +
                               "C.[FG CHN Name] = B.[CHN Name] WHERE C.[IE Type] = 'RM-D'" + strBrowse10 + " ORDER BY C.[Group ID], C.[GongDan No]";

            SqlConnection SqlConn10 = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn10.State == ConnectionState.Closed) { SqlConn10.Open(); }
            SqlDataAdapter browseAdapter10 = new SqlDataAdapter(strSQL10, SqlConn10);
            DataTable dtBrowse10 = new DataTable();
            browseAdapter10.Fill(dtBrowse10);
            browseAdapter10.Dispose();

            if (dtBrowse10.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtBrowse10.Dispose();
                SqlConn10.Close();
                SqlConn10.Dispose();
                return;
            }

            int PageRow = 65536;
            int iPageCount = (int)(dtBrowse10.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtBrowse10.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\RM Distribution in e-Handbook (PingDan) " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtBrowse10.Columns.Count; n++)
                    { sb.Append(dtBrowse10.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < dtBrowse10.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtBrowse10.Columns.Count; j++)
                        {
                            sb.Append(dtBrowse10.Rows[i][j].ToString().Trim() + "\t"); 
                        }
                        sb.Append(Environment.NewLine);
                    }

                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                MessageBox.Show("Successfully generated 'RM Distribution in e-Handbook (PingDan) report'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            dtBrowse10.Clear();
            dtBrowse10.Dispose();
            if (SqlConn10.State == ConnectionState.Open)
            {
                SqlConn10.Close();
                SqlConn10.Dispose();
            }
        }
        #endregion
    }
}
