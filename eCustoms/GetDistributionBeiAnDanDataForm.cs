using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class GetDistributionBeiAnDanDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        DataTable dtGatherData = new DataTable();
        DataTable dtExtractDoc = new DataTable();
        DataTable dtExRate = new DataTable();
        DataTable dtHsCode = new DataTable();

        private static GetDistributionBeiAnDanDataForm GetDistributionBeiAnDanDataFrm;
        public GetDistributionBeiAnDanDataForm() { InitializeComponent(); }
        public static GetDistributionBeiAnDanDataForm CreateInstance()
        {
            if (GetDistributionBeiAnDanDataFrm == null || GetDistributionBeiAnDanDataFrm.IsDisposed)
            { GetDistributionBeiAnDanDataFrm = new GetDistributionBeiAnDanDataForm(); }
            return GetDistributionBeiAnDanDataFrm;
        }

        private void GetDistributionBeiAnDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";
            this.dtpTaxPaidDate.CustomFormat = " ";
            this.dtpCustomsReleaseDate.CustomFormat = " ";

            this.btnExtractDoc.Enabled = false;
            this.btnDownload.Enabled = false;
            this.btnReport.Enabled = false;
            this.btnUpdate.Enabled = false;
            this.btnDelete.Enabled = false;

            SqlLib sqlLib = new SqlLib();
            dtExRate = sqlLib.GetDataTable(@"SELECT [ObjectValue] AS [Rate] FROM B_SysInfo WHERE [ObjectName] = 'ExchangeRate:CNY'").Copy();
            dtHsCode = sqlLib.GetDataTable(@"SELECT DISTINCT [FG CHN Name] AS [CHN Name], [Duty Rate] FROM B_HsCode").Copy();
            sqlLib.Dispose(0);
        }
        private void GetDistributionBeiAnDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.dtGatherData.Dispose();
            this.dtExtractDoc.Dispose();
            this.dtExRate.Dispose();
            this.dtHsCode.Dispose();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e) { this.dtpFrom.CustomFormat = null; }
        private void dtpFrom_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; } }
        private void dtpTo_ValueChanged(object sender, EventArgs e) { this.dtpTo.CustomFormat = null; }
        private void dtpTo_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; } }
        private void dtpTaxPaidDate_ValueChanged(object sender, EventArgs e) { this.dtpTaxPaidDate.CustomFormat = "M/dd/yyyy HH:mm"; }
        private void dtpTaxPaidDate_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpTaxPaidDate.CustomFormat = " "; } }
        private void dtpCustomsReleaseDate_ValueChanged(object sender, EventArgs e) { this.dtpCustomsReleaseDate.CustomFormat = "M/dd/yyyy HH:mm"; }
        private void dtpCustomsReleaseDate_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpCustomsReleaseDate.CustomFormat = " "; } }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT P.[Group ID], P.[GongDan No], P.[PingDan Qty], P.[FG No], P.[FG EHB], P.[FG CHN Name] AS [CHN Name], P.[Order No], " +
                            "CAST(P.[PingDan Amount] / P.[PingDan Qty] AS decimal(18, 2)) AS [Selling Price], P.[PingDan Amount] AS [Selling Amount], " +
                            "G.[Approval Date] AS [GongDan Approval Date] FROM C_PingDan AS P INNER JOIN (SELECT [GongDan No], [Approval Date] FROM " +
                            "C_GongDan WHERE [IE Type] = 'RM-D') AS G ON P.[GongDan No] = G.[GongDan No] WHERE P.[IE Type] = 'RM-D' AND " +
                            "[PingDan ID] IS NOT NULL AND [PingDan No] IS NOT NULL AND [Gate Out Time] IS NOT NULL AND P.[GongDan No] NOT IN (" +
                            "SELECT [GongDan No] FROM C_BeiAnDan WHERE [IE Type] = 'RM-D') ORDER BY P.[Group ID], P.[GongDan No]";

            string strDoc = "SELECT P.[Group ID], '' AS [备案单号], N'成品内销（料件）' AS [单证类型], 0.0 AS [总毛重], N'保区进料料件' AS [贸易方式], '' AS [结转企业代码], " +
                            "0 AS [项号], P.[FG EHB] AS [备件号], P.[PingDan Qty] AS [数量], P.[PingDan Qty] AS [净重], 0.0 AS [毛重], P.[PingDan Amount] AS [金额], " +
                            "N'美元' AS [币制], N'中国' AS [原产地/目的地], P.[GongDan No] AS [工单/批次号], P.[PingDan ID] FROM C_PingDan AS P INNER JOIN (SELECT " +
                            "DISTINCT [GongDan No] FROM C_GongDan WHERE [IE Type] = 'RM-D') AS G ON P.[GongDan No] = G.[GongDan No] WHERE P.[IE Type] = 'RM-D' AND " +
                            "[PingDan ID] IS NOT NULL AND [PingDan No] IS NOT NULL AND [Gate Out Time] IS NOT NULL AND P.[GongDan No] NOT IN (SELECT [GongDan No] FROM " +
                            "C_BeiAnDan WHERE [IE Type] = 'RM-D') ORDER BY P.[Group ID], P.[GongDan No]";

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlDataAdapter sqlAdp = new SqlDataAdapter();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "usp_GetBeiAnDanForRMD";
            sqlComm.Parameters.AddWithValue("@SQL", strSQL);
            sqlAdp.SelectCommand = sqlComm;
            DataTable dtMid = new DataTable();
            sqlAdp.Fill(dtMid);
            if (dtMid.Rows.Count == 0)
            {
                sqlComm.Parameters.Clear();
                sqlComm.Dispose();
                sqlConn.Dispose();
                dtMid.Dispose();
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            sqlComm.Parameters.Clear();
            sqlComm.CommandText = "usp_GetBeiAnDanForRMD";
            sqlComm.Parameters.AddWithValue("@SQL", strDoc);
            sqlAdp.SelectCommand = sqlComm;
            dtExtractDoc.Clear();
            sqlAdp.Fill(dtExtractDoc);
            sqlComm.Parameters.Clear();
            sqlComm.CommandText = "usp_InsertBeiAnDanForRMD";
            sqlComm.Parameters.AddWithValue("@Creater", loginFrm.PublicUserName.ToUpper());
            sqlComm.Parameters.AddWithValue("@BeiAnDanDate", System.DateTime.Now);
            sqlComm.Parameters.AddWithValue("@tvp_BAD", dtMid);
            sqlAdp.SelectCommand = sqlComm;
            dtGatherData.Clear();
            sqlAdp.Fill(dtGatherData);
            sqlAdp.Dispose();
            dtMid.Dispose();
            sqlComm.Parameters.Clear();
            sqlComm.Dispose();
            sqlConn.Close();
            sqlConn.Dispose();

            this.dgvBeiAnDan.DataSource = dtGatherData;
            this.dgvBeiAnDan.Columns["GongDan No"].Frozen = true;
            this.btnExtractDoc.Enabled = true;
        }

        private void btnExtractDoc_Click(object sender, EventArgs e)
        {
            if (this.dtExtractDoc.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand(@"SELECT [ObjectValue] FROM B_SysInfo WHERE [ObjectName] = 'WeightRatio'", sqlConn);
            decimal dGwRatio = Convert.ToDecimal(sqlComm.ExecuteScalar());

            int iNumber = 0;
            string strGroupID = dtExtractDoc.Rows[0]["Group ID"].ToString().Trim();
            foreach (DataRow dr in dtExtractDoc.Rows)
            {
                dr["毛重"] = Convert.ToDecimal(dr["净重"].ToString()) * dGwRatio;
                if (String.Compare(dr["Group ID"].ToString().Trim(), strGroupID) != 0)
                {
                    decimal dTotalGW = (decimal)dtExtractDoc.Compute("SUM([毛重])", "[Group ID] = '" + strGroupID + "'");
                    DataRow[] dRow = dtExtractDoc.Select("[Group ID] = '" + strGroupID + "'");
                    foreach (DataRow row in dRow) { row["总毛重"] = dTotalGW; }
                    strGroupID = dr["Group ID"].ToString().Trim();
                    iNumber = 0;
                }
                dr["项号"] = ++iNumber;
            }
            decimal dSUMGW = (decimal)dtExtractDoc.Compute("SUM([毛重])", "[Group ID] = '" + strGroupID + "'");
            DataRow[] rows = dtExtractDoc.Select("[Group ID] = '" + strGroupID + "'");
            foreach (DataRow row in rows) { row["总毛重"] = dSUMGW; }

            dtExtractDoc.AcceptChanges();
            SqlLib lib = new SqlLib();
            string[] strFields = { "Group ID" };
            DataTable dtID = lib.SelectDistinct(dtExtractDoc, strFields);
            lib.Dispose(0);

            DataTable dtExcDoc = dtExtractDoc.Copy();
            dtExcDoc.Columns.Remove("PingDan ID");
            sqlComm.CommandType = CommandType.StoredProcedure;
            foreach (DataRow dr in dtID.Rows)
            {
                sqlComm.CommandText = @"usp_InsertBeiAnDanDocForRMD";
                sqlComm.Parameters.AddWithValue("@GroupID", dr[0].ToString());
                sqlComm.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm")));
                sqlComm.Parameters.AddWithValue("@tvp_BadDoc", dtExcDoc);
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();
            }
            dtID.Dispose();
            dtExcDoc.Dispose();

            this.dgvBeiAnDan.DataSource = dtExtractDoc;
            this.dgvBeiAnDan.Columns["备案单号"].Visible = false;
            this.btnDownload.Enabled = true;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dtExtractDoc.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlLib lib = new SqlLib();
            string[] strFields = { "Group ID", "备件号" };
            DataTable dtBadDoc = lib.SelectDistinct(dtExtractDoc, strFields);
            lib.Dispose(0);
            dtBadDoc.Columns.Add("数量", typeof(string));
            dtBadDoc.Columns.Add("毛重", typeof(string));
            dtBadDoc.Columns.Add("金额", typeof(string));
            foreach (DataRow dr in dtBadDoc.Rows)
            {
                string strGroupID = dr["Group ID"].ToString().Trim();
                string strEHB = dr["备件号"].ToString().Trim();
                dr["数量"] = dtExtractDoc.Compute("SUM([数量])", "[Group ID] = '" + strGroupID + "' AND [备件号] = '" + strEHB + "'").ToString();
                dr["毛重"] = dtExtractDoc.Compute("SUM([毛重])", "[Group ID] = '" + strGroupID + "' AND [备件号] = '" + strEHB + "'").ToString();
                dr["金额"] = dtExtractDoc.Compute("SUM([金额])", "[Group ID] = '" + strGroupID + "' AND [备件号] = '" + strEHB + "'").ToString();
            }
            dtBadDoc.AcceptChanges();

            Microsoft.Office.Interop.Excel.Application excel_Doc = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_Doc = excel_Doc.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_Doc = workbooks_Doc.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets[1];
            worksheet_Doc.Name = "BeiAnDan_Report";

            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtExtractDoc.Rows.Count + 1, dtExtractDoc.Columns.Count]).NumberFormatLocal = "@";
            for (int x = 0; x < dtExtractDoc.Rows.Count; x++)
            {
                for (int y = 0; y < dtExtractDoc.Columns.Count; y++)
                { worksheet_Doc.Cells[x + 2, y + 1] = "'" + dtExtractDoc.Rows[x][y].ToString().Trim(); }
            }
            for (int z = 0; z < dtExtractDoc.Columns.Count; z++)
            { worksheet_Doc.Cells[1, z + 1] = dtExtractDoc.Columns[z].ColumnName.ToString().Trim(); }

            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, dtExtractDoc.Columns.Count]).Font.Bold = true;
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtExtractDoc.Rows.Count + 1, dtExtractDoc.Columns.Count]).Font.Name = "Verdana";
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtExtractDoc.Rows.Count + 1, dtExtractDoc.Columns.Count]).Font.Size = 8;
            worksheet_Doc.Cells.EntireColumn.AutoFit();
            excel_Doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            object missing = System.Reflection.Missing.Value;
            worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);
            worksheet_Doc.Name = "New_BeiAnDan";
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtBadDoc.Rows.Count + 1, 14]).NumberFormatLocal = "@";
            string strRQ = System.DateTime.Now.ToString("yyyyMMdd");

            for (int a = 0; a < dtBadDoc.Rows.Count; a++)
            {
                worksheet_Doc.Cells[a + 2, 1] = dtBadDoc.Rows[a]["Group ID"].ToString().Trim();
                worksheet_Doc.Cells[a + 2, 2] = "C014";
                worksheet_Doc.Cells[a + 2, 3] = "3122442019";
                worksheet_Doc.Cells[a + 2, 4] = "0";
                worksheet_Doc.Cells[a + 2, 5] = "2";
                worksheet_Doc.Cells[a + 2, 6] = "0544";
                worksheet_Doc.Cells[a + 2, 7] = strRQ;
                worksheet_Doc.Cells[a + 2, 8] = dtBadDoc.Rows[a]["备件号"].ToString().Trim();
                worksheet_Doc.Cells[a + 2, 9] = dtBadDoc.Rows[a]["数量"].ToString().Trim();
                worksheet_Doc.Cells[a + 2, 10] = dtBadDoc.Rows[a]["数量"].ToString().Trim();
                worksheet_Doc.Cells[a + 2, 11] = dtBadDoc.Rows[a]["毛重"].ToString().Trim();
                worksheet_Doc.Cells[a + 2, 12] = dtBadDoc.Rows[a]["金额"].ToString().Trim();
                worksheet_Doc.Cells[a + 2, 13] = "502";
                worksheet_Doc.Cells[a + 2, 14] = "142";
            }
            worksheet_Doc.Cells[1, 1] = "企业内部编号";
            worksheet_Doc.Cells[1, 2] = "手册号";
            worksheet_Doc.Cells[1, 3] = "货主十位编码";
            worksheet_Doc.Cells[1, 4] = "是否加封";
            worksheet_Doc.Cells[1, 5] = "出区类型";
            worksheet_Doc.Cells[1, 6] = "贸易方式";
            worksheet_Doc.Cells[1, 7] = "出区日期";
            worksheet_Doc.Cells[1, 8] = "原始货物备件号";
            worksheet_Doc.Cells[1, 9] = "数量";
            worksheet_Doc.Cells[1, 10] = "净重";
            worksheet_Doc.Cells[1, 11] = "毛重";
            worksheet_Doc.Cells[1, 12] = "金额";
            worksheet_Doc.Cells[1, 13] = "币制";
            worksheet_Doc.Cells[1, 14] = "目的国";

            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 14]).Font.Bold = true;
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtBadDoc.Rows.Count + 1, 14]).Font.Name = "Verdana";
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtBadDoc.Rows.Count + 1, 14]).Font.Size = 8;
            worksheet_Doc.Cells.EntireColumn.AutoFit();
            excel_Doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            dtBadDoc.Dispose();
            excel_Doc.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_Doc);
            worksheet_Doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook_Doc);
            workbook_Doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Doc);
            excel_Doc = null;
            GC.Collect();           
        }

        private void cmbGroupID_Enter(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDan.RowCount > 0)
            {
                SqlLib lib = new SqlLib();
                string[] strFile = { "Group ID" };
                DataTable dtGroupID = lib.SelectDistinct(dtGatherData, strFile);
                lib.Dispose(0);

                DataRow dr = dtGroupID.NewRow();
                dr["Group ID"] = String.Empty;
                dtGroupID.Rows.InsertAt(dr, 0);
                this.cmbGroupID.DisplayMember = this.cmbGroupID.ValueMember = "Group ID";
                this.cmbGroupID.DataSource = dtGroupID;                
            }
        }

        private void cmbGroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbGroupID.SelectedIndex == 0) { this.btnReport.Enabled = false; }
            else { this.btnReport.Enabled = true; }
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDan.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            string strGroupID = this.cmbGroupID.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strGroupID))
            { MessageBox.Show("Please select Group ID.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            string strBeiAnDanID = this.txtBeiAnDanID.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strBeiAnDanID))
            { MessageBox.Show("Please input BeiAnDan ID.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = @"SELECT COUNT(*) FROM C_BeiAnDan WHERE [BeiAnDan ID] = '" + strBeiAnDanID + "'";
            int iCount = Convert.ToInt32(sqlComm.ExecuteScalar());
            if (iCount > 0)
            {
                MessageBox.Show("The BeiAnDan ID(" + strBeiAnDanID + ") already existed, please input a new one.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                sqlComm.Dispose();
                sqlConn.Dispose();
                return;
            }
            sqlComm.CommandText = @"UPDATE C_BeiAnDan SET [BeiAnDan ID] = '" + strBeiAnDanID + "' WHERE [Group ID] = '" + strGroupID + "'";
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
            sqlConn.Dispose();
            MessageBox.Show("Update BeiAnDan ID successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);           
            this.cmbGroupID.Text = String.Empty;
            this.txtBeiAnDanID.Text = String.Empty;
        }        

        private void cmbBeiAnDanID_Enter(object sender, EventArgs e)
        {
            if (this.dtGatherData.Rows.Count > 0)
            {
                SqlLib lib = new SqlLib();
                string[] strFile = { "BeiAnDan ID" };
                DataTable dtBadID = lib.SelectDistinct(dtGatherData, strFile);
                lib.Dispose(0);
                DataRow dr = dtBadID.NewRow();
                dr["BeiAnDan ID"] = String.Empty;
                dtBadID.Rows.InsertAt(dr, 0);
                this.cmbBeiAnDanID.DisplayMember = this.cmbBeiAnDanID.ValueMember = "BeiAnDan ID";
                this.cmbBeiAnDanID.DataSource = dtBadID;
            }
        }

        private void cmbBeiAnDanID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
            {
                this.btnUpdate.Enabled = true;
                this.btnDelete.Enabled = true;
            }
            else
            {
                this.btnUpdate.Enabled = false;
                this.btnDelete.Enabled = false;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strBrowse = " B.[IE Type] = 'RM-D'";
            if (this.cbCustomsReleaseDate.Checked == true) { strBrowse += " AND (B.[Customs Release Date] IS NULL OR B.[Customs Release Date] = '')"; }
            if (!String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim())) { strBrowse += " AND B.[BeiAnDan ID] = '" + this.cmbBeiAnDanID.Text.Trim().ToUpper() + "'"; }
            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1)
                    { MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    else { strBrowse += " AND B.[BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND B.[BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else { strBrowse += " AND B.[BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strBrowse += " AND B.[BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            string strSQL = "SELECT B.[Group ID], B.[GongDan No], B.[FG No], B.[FG EHB], B.[CHN Name], B.[Order No], B.[GongDan Qty], B.[Selling Price], " + 
                            "B.[Selling Amount], B.[GongDan Approval Date], B.[BeiAnDan ID], B.[BeiAnDan No], B.[BeiAnDan Date], B.[Creater], " + 
                            "B.[Tax & Duty Paid Date], B.[Customs Release Date], P.[PingDan ID] FROM C_BeiAnDan AS B INNER JOIN C_PingDan AS P " + 
                            "ON B.[GongDan No] = P.[GongDan No] WHERE" + strBrowse + " ORDER BY B.[Group ID], B.[GongDan No]";
            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlDataAdapter sqlAdp = new SqlDataAdapter(strSQL, sqlConn);
            dtGatherData.Clear();
            sqlAdp.Fill(dtGatherData);
            sqlAdp.Dispose();
            sqlConn.Dispose();

            if (dtGatherData.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvBeiAnDan.DataSource = DBNull.Value;
            }
            else
            {
                this.dgvBeiAnDan.DataSource = dtGatherData;
                this.dgvBeiAnDan.Columns["GongDan No"].Frozen = true;             
            }
        }
       
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to update the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            string strBeiAnDanID = this.cmbBeiAnDanID.Text.Trim().ToUpper();
            DataRow[] drow = dtGatherData.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'");
            string strCustomsReleaseDate = drow[0]["Customs Release Date"].ToString().Trim();
            bool bExist = false;
            if (!String.IsNullOrEmpty(strCustomsReleaseDate)) { bExist = true; }

            int iCount = 0;
            string strBeiAnDanNo = String.Empty, strTax = String.Empty, strRelease = String.Empty;
            if (!String.IsNullOrEmpty(this.txtBeiAnDanNo.Text.Trim()))
            {
                strBeiAnDanNo = this.txtBeiAnDanNo.Text.Trim().ToUpper();
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 1;
                    foreach (DataRow dr in drow) { dr["BeiAnDan No"] = strBeiAnDanNo; }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 2;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["BeiAnDan No"] = strBeiAnDanNo;
                        dr["Tax & Duty Paid Date"] = strTax;
                    }
                }
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 3;
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["BeiAnDan No"] = strBeiAnDanNo;
                        dr["Customs Release Date"] = strRelease;
                    }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 4;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["BeiAnDan No"] = strBeiAnDanNo;
                        dr["Tax & Duty Paid Date"] = strTax;
                        dr["Customs Release Date"] = strRelease;
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    MessageBox.Show("Please input BeiAnDan No and/or date time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 5;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Tax & Duty Paid Date"] = strTax; }
                }
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 6;
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Customs Release Date"] = strRelease; }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 7;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["Tax & Duty Paid Date"] = strTax;
                        dr["Customs Release Date"] = strRelease;
                    }
                }
            }
            dtGatherData.AcceptChanges();

            decimal dTotAmt = 0.0M;
            if (!bExist)
            {
                if (iCount == 3 || iCount == 4 || iCount == 6 || iCount == 7)
                {
                    decimal dRate = Convert.ToDecimal(dtExRate.Rows[0][0].ToString().Trim());
                    foreach (DataRow dr in dtGatherData.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'"))
                    {
                        string strDuty = dtHsCode.Select("[CHN Name] = '" + dr["CHN Name"].ToString() + "'")[0][1].ToString().Trim();
                        dTotAmt += Convert.ToDecimal(dr["Selling Amount"].ToString().Trim()) * (0.17M + 1.17M * Convert.ToDecimal(strDuty));
                    }
                    dTotAmt = Math.Round(dTotAmt / dRate, 2);
                }
            }

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlCommand Comm = new SqlCommand();
            Comm.Connection = Conn;
            Comm.CommandType = CommandType.StoredProcedure;
            Comm.CommandText = @"usp_UpdateBeiAnDanForRMD";
            Comm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
            Comm.Parameters.AddWithValue("@BeiAnDanNo", strBeiAnDanNo);
            Comm.Parameters.AddWithValue("@TaxDutyPaidDate", strTax);
            Comm.Parameters.AddWithValue("@CustomsReleaseDate", strRelease);
            Comm.Parameters.AddWithValue("@Count", iCount);
            Comm.Parameters.AddWithValue("@TotalAmount", dTotAmt);
            Comm.ExecuteNonQuery();
            Comm.Parameters.Clear();

            Comm.Dispose();
            Conn.Dispose();
            this.txtBeiAnDanNo.Text = string.Empty;
            MessageBox.Show("Update data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            string strBeiAnDanID = this.cmbBeiAnDanID.Text.Trim().ToUpper();
            DataRow[] drow = dtGatherData.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'");
            string strCustomsReleaseDate = drow[0]["Customs Release Date"].ToString().Trim();
            string strGroupID = drow[0]["Group ID"].ToString().Trim();
            if (!String.IsNullOrEmpty(strCustomsReleaseDate))
            { MessageBox.Show("Already 2nd release, reject to delete the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "usp_DeleteHistoryBeiAnDan";
            sqlComm.Parameters.AddWithValue("@GroupID", strGroupID);
            sqlComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
            sqlComm.Parameters.AddWithValue("@JudgeObj", "RM-D");
            sqlComm.ExecuteNonQuery();
            sqlComm.Parameters.Clear();
            sqlComm.Dispose();
            sqlConn.Dispose();

            foreach (DataRow dr in drow) { dr.Delete(); }
            dtGatherData.AcceptChanges();
            this.cmbBeiAnDanID.Text = String.Empty;
            MessageBox.Show("Delete the data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //private void btnDL_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
        //    if (this.dtGatherData.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

        //    Microsoft.Office.Interop.Excel.Application excel_Doc = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbooks workbooks_Doc = excel_Doc.Workbooks;
        //    Microsoft.Office.Interop.Excel.Workbook workbook_Doc = workbooks_Doc.Add(true);
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets[1];

        //    //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtGatherData.Rows.Count + 1, dtGatherData.Columns.Count]).NumberFormatLocal = "@";
        //    for (int x = 0; x < dtGatherData.Rows.Count; x++)
        //    {
        //        for (int y = 0; y < dtGatherData.Columns.Count; y++)
        //        { worksheet_Doc.Cells[x + 2, y + 1] = dtGatherData.Rows[x][y].ToString().Trim(); }
        //    }
        //    for (int z = 0; z < dtGatherData.Columns.Count; z++)
        //    { worksheet_Doc.Cells[1, z + 1] = dtGatherData.Columns[z].ColumnName.ToString().Trim(); }

        //    //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, dtGatherData.Columns.Count]).Font.Bold = true;
        //    //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtGatherData.Rows.Count + 1, dtGatherData.Columns.Count]).Font.Name = "Verdana";
        //    //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtGatherData.Rows.Count + 1, dtGatherData.Columns.Count]).Font.Size = 8;
        //    worksheet_Doc.Cells.EntireColumn.AutoFit();
        //    excel_Doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

        //    excel_Doc.Visible = true;
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_Doc);
        //    worksheet_Doc = null;
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook_Doc);
        //    workbook_Doc = null;
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Doc);
        //    excel_Doc = null;
        //    GC.Collect();
        //    this.btnDL.Visible = false;
        //}

        private void btnReport_Click(object sender, EventArgs e)
        {
            string strGroupID = this.cmbGroupID.Text.Trim().ToUpper();
            //if (strGroupID.Contains("O")) { strGroupID = strGroupID.Remove(strGroupID.Length - 2); }
            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;

            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_BeiAnDan_RMD_BomRpt";
            sqlComm.Parameters.AddWithValue("@GroupID", strGroupID);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtBomReport = new DataTable();
            sqlAdapter.Fill(dtBomReport);

            dtBomReport.Columns.Add("项号", typeof(Int32));
            dtBomReport.Columns.Add("耗用料件", typeof(decimal));
            dtBomReport.Columns["项号"].SetOrdinal(2);
            dtBomReport.Columns["耗用料件"].SetOrdinal(10);

            string strFGEHB = null;
            int iLineNo = 1;
            foreach (DataRow dr in dtBomReport.Rows)
            {
                string strEHB = dr[0].ToString().Trim();
                if (String.Compare(strFGEHB, strEHB) != 0) { strFGEHB = strEHB; iLineNo = 1; }
                else { iLineNo++; }
                dr[2] = iLineNo;

                decimal dFgQty = Convert.ToDecimal(double.Parse(dr[9].ToString().Trim()));
                decimal dConsumption = Convert.ToDecimal(double.Parse(dr[5].ToString().Trim()));
                decimal dQtyLossRate = Convert.ToDecimal(double.Parse(dr[6].ToString().Trim()));
                dr[10] = Math.Round(dFgQty * dConsumption / (1 - dQtyLossRate / 100), 6);
            }
            dtBomReport.AcceptChanges();

            sqlComm.CommandType = CommandType.Text;
            sqlComm.CommandText = @"SELECT [ObjectName] AS [Object], [ObjectValue] AS [Rate] FROM B_SysInfo WHERE [ObjectName] LIKE 'ExchangeRate%'";
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtRate = new DataTable();
            sqlAdapter.Fill(dtRate);
            DataRow[] drRMB = dtRate.Select("[Object] = 'ExchangeRate:CNY'");
            decimal dRMBRate = 0.0M;
            if (drRMB.Length == 1) { dRMBRate = Convert.ToDecimal(drRMB[0][1].ToString().Trim()); }

            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_BeiAnDan_RMD_BgdRpt";
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtBgdReport = new DataTable();
            sqlAdapter.Fill(dtBgdReport);

            DataRow[] dRow1 = dtBgdReport.Select("[RM Currency] <> 'USD'");
            if (dRow1.Length > 0)
            {
                foreach (DataRow dr in dRow1)
                {
                    string strObj = "ExchangeRate:" + dr["RM Currency"].ToString().Trim().ToUpper();
                    DataRow[] rows = dtRate.Select("[Object] = '" + strObj + "'");
                    if (rows.Length > 0)
                    {
                        decimal dRate = Convert.ToDecimal(float.Parse(rows[0][1].ToString().Trim()));
                        decimal dPrice = Convert.ToDecimal(dr["单价"].ToString().Trim());
                        dr["单价"] = Math.Round(dPrice * dRate, 4);
                    }
                    else { dr["单价"] = 0.0M; }
                }
                dtBgdReport.AcceptChanges();
            }
            dtBgdReport.Columns.Remove("RM Currency");

            sqlComm.CommandText = @"usp_BeiAnDan_RMD_InvoiceRpt_USD_Mid";
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtSort1 = new DataTable();
            sqlAdapter.Fill(dtSort1);

            DataTable dtSort2 = new DataTable();
            SqlLib sqlLib = new SqlLib();
            string[] str1 = { "GongDan No" };
            if (sqlLib.SelectDistinct(dtSort1, str1).Rows.Count > 0)
            {
                dtSort2.Columns.Add("RM EHB", typeof(String));
                dtSort2.Columns.Add("Country of Origin", typeof(String));
                foreach (DataRow dr in dtSort1.Rows)
                {
                    string strRMEHB = dr["RM EHB"].ToString().Trim();
                    string strOriginalCountry = dr["Country of Origin"].ToString().Trim();
                    DataRow[] rows = dtSort2.Select("[RM EHB] = '" + strRMEHB + "' AND [Country of Origin] = '" + strOriginalCountry + "'");
                    if (rows.Length == 0)
                    {
                        DataRow row = dtSort2.NewRow();
                        row[0] = strRMEHB;
                        row[1] = strOriginalCountry;
                        dtSort2.Rows.Add(row);
                    }
                }
            }
            dtSort2.AcceptChanges();
            dtSort1.Dispose();

            sqlComm.CommandText = @"usp_BeiAnDan_RMD_InvoiceRpt_USD";
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtInvUSD = new DataTable();
            sqlAdapter.Fill(dtInvUSD);

            DataRow[] dRow2 = dtInvUSD.Select("[RM Currency] <> 'USD'");
            if (dRow2.Length > 0)
            {
                foreach (DataRow dr in dRow2)
                {
                    string strObj = "ExchangeRate:" + dr["RM Currency"].ToString().Trim().ToUpper();
                    DataRow[] rows = dtRate.Select("[Object] = '" + strObj + "'");
                    if (rows.Length > 0)
                    {
                        decimal dRate = Convert.ToDecimal(double.Parse(rows[0][1].ToString().Trim()));
                        decimal dPrice = Convert.ToDecimal(dr["Unit Price(USD/KG)"].ToString().Trim());
                        dr["Unit Price(USD/KG)"] = Math.Round(dPrice * dRate, 4);
                    }
                    else { dr["Unit Price(USD/KG)"] = 0.0M; }
                }
                dtInvUSD.AcceptChanges();
            }
            dtRate.Dispose();
            dtInvUSD.Columns.Add("Amount(USD)", typeof(decimal));

            string[] strAppFG1 = { "成品中文名称" };
            DataTable dtAppFG1 = sqlLib.SelectDistinct(dtBomReport, strAppFG1);
            int iAppFG1 = dtAppFG1.Rows.Count;
            string[] strAppFG2 = { "成品中文名称", "成品数量", "成品备件号" };
            DataTable dtAppFG2 = sqlLib.SelectDistinct(dtBomReport, strAppFG2);
            string[] strAppDrools = { "Drools CH Name" };
            DataTable dtAppDrools = sqlLib.SelectDistinct(dtBomReport, strAppDrools);
            int iAppDrools = dtAppDrools.Rows.Count;
            string[] strColumns = { "EHB", "Country of Origin" };
            DataTable dtInvRpt1 = sqlLib.SelectDistinct(dtInvUSD, strColumns);

            foreach (DataRow dr in dtInvRpt1.Rows)
            {
                DataRow[] rows = dtInvUSD.Select("[EHB] = '" + dr[0].ToString().Trim() + "' AND [Country of Origin] = '" + dr[1].ToString().Trim() + "' AND [Unit Price(USD/KG)] <> 0.0");
                if (rows.Length > 1)
                {
                    int iRow = rows.Length, iCount = 1;
                    decimal dTotalNetWeight = 0.0M, dTotalCost = 0.0M, dAvgPrice = 0.0M;
                    foreach (DataRow row in rows)
                    {
                        dTotalNetWeight += Convert.ToDecimal(row["NW(KG)"].ToString().Trim());
                        dTotalCost += Math.Round(Convert.ToDecimal(row["NW(KG)"].ToString().Trim()) * (Convert.ToDecimal(row["Unit Price(USD/KG)"].ToString().Trim()) + 0.0001M), 5);
                    }

                    if (dTotalNetWeight != 0.0M) { dAvgPrice = Math.Round(dTotalCost / dTotalNetWeight, 4); }
                    else { dAvgPrice = 0.0M; }
                    foreach (DataRow row in rows)
                    {
                        if (iCount == iRow)
                        {
                            row["NW(KG)"] = dTotalNetWeight;
                            row["GW(KG)"] = dTotalNetWeight;
                            row["Unit Price(USD/KG)"] = dAvgPrice;
                            row["Amount(USD)"] = Math.Round(dTotalNetWeight * dAvgPrice, 4);
                        }
                        else { row.Delete(); iCount++; }
                    }
                }
                else
                {
                    decimal dNetWeight = Convert.ToDecimal(rows[0]["NW(KG)"].ToString().Trim());
                    decimal dUnitPrice = Math.Round(Convert.ToDecimal(rows[0]["Unit Price(USD/KG)"].ToString().Trim()) + 0.0001M, 4);
                    rows[0]["Unit Price(USD/KG)"] = dUnitPrice;
                    rows[0]["Amount(USD)"] = Math.Round(dNetWeight * dUnitPrice, 4);
                }
                dtInvUSD.AcceptChanges();
            }
            dtInvRpt1.Dispose();
            dtInvUSD.Columns.Remove("RM Currency");

            DataTable dtSort3 = dtInvUSD.Clone();
            foreach (DataRow rows in dtSort2.Rows)
            {
                string strRMCustomsCode = rows[0].ToString().Trim();
                string strOriginalCountry = rows[1].ToString().Trim();
                DataRow[] row = dtInvUSD.Select("[EHB] = '" + strRMCustomsCode + "' AND [Country of Origin] = '" + strOriginalCountry + "'");
                if (row.Length > 0)
                {
                    DataRow dr = dtSort3.NewRow();
                    dr["EHB"] = row[0]["EHB"].ToString().Trim();
                    dr["Name"] = row[0]["Name"].ToString().Trim();
                    dr["Description"] = row[0]["Description"].ToString().Trim();
                    dr["Country of Origin"] = row[0]["Country of Origin"].ToString().Trim();
                    dr["NW(KG)"] = row[0]["NW(KG)"].ToString().Trim();
                    dr["GW(KG)"] = row[0]["GW(KG)"].ToString().Trim();
                    string strUnitPrice = row[0]["Unit Price(USD/KG)"].ToString().Trim();
                    dr["Unit Price(USD/KG)"] = String.IsNullOrEmpty(strUnitPrice) ? 0.0M : Convert.ToDecimal(strUnitPrice);
                    string strAmount = row[0]["Amount(USD)"].ToString().Trim();
                    dr["Amount(USD)"] = String.IsNullOrEmpty(strAmount) ? 0.0M : Convert.ToDecimal(strAmount);
                    dtSort3.Rows.Add(dr);
                }
            }
            dtSort2.Dispose();
            dtInvUSD.Columns.Clear();
            dtInvUSD.Rows.Clear();
            dtInvUSD = dtSort3.Copy();
            dtSort3.Dispose();

            sqlComm.CommandText = @"usp_BeiAnDan_RMD_InvoiceRpt_RMB";
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtInvRMB = new DataTable();
            sqlAdapter.Fill(dtInvRMB);
            dtInvRMB.Columns.Add("Amount(USD)", typeof(decimal));

            string[] strColumn = { "EHB" };
            DataTable dtInvRpt2 = sqlLib.SelectDistinct(dtInvRMB, strColumn);
            sqlLib.Dispose(0);
            foreach (DataRow dr in dtInvRpt2.Rows)
            {
                DataRow[] rows = dtInvRMB.Select("[EHB] = '" + dr[0].ToString().Trim() + "'");
                if (rows.Length > 1)
                {
                    int iRow = rows.Length, iCount = 1;
                    decimal dTotalNetWeight = 0.0M, dTotalCost = 0.0M, dAvgPrice = 0.0M;
                    foreach (DataRow row in rows)
                    {
                        decimal dNetWeight = Convert.ToDecimal(Double.Parse(row["NW(KG)"].ToString().Trim()));
                        decimal dUnitPrice = 0.0M;
                        if (!String.IsNullOrEmpty(row["Unit Price(USD/KG)"].ToString().Trim()))
                        {
                            if (String.Compare(row["RM Currency"].ToString().Trim().ToUpper(), "CNY") == 0)
                            { dUnitPrice = Convert.ToDecimal(row["Unit Price(USD/KG)"].ToString().Trim()) * dRMBRate; }
                            else { dUnitPrice = Convert.ToDecimal(row["Unit Price(USD/KG)"].ToString().Trim()); }
                        }
                        else { dUnitPrice = 0.0M; }
                        dTotalNetWeight += dNetWeight;
                        dTotalCost += Math.Round(dNetWeight * dUnitPrice, 5);
                    }
                    dTotalCost = Math.Round(dTotalCost, 4);
                    if (dTotalNetWeight != 0.0M) { dAvgPrice = Math.Round(dTotalCost / dTotalNetWeight, 4); }
                    else { dAvgPrice = 0.0M; }
                    foreach (DataRow row in rows)
                    {
                        if (iCount == iRow)
                        {
                            row["NW(KG)"] = dTotalNetWeight;
                            row["Unit Price(USD/KG)"] = dAvgPrice;
                            row["Amount(USD)"] = dTotalCost;
                        }
                        else { row.Delete(); iCount++; }
                    }
                }
                else
                {
                    decimal dNetWeight = Convert.ToDecimal(rows[0]["NW(KG)"].ToString().Trim());
                    decimal dUnitPrice = 0.0M;
                    if (!String.IsNullOrEmpty(rows[0]["Unit Price(USD/KG)"].ToString().Trim()))
                    {
                        if (String.Compare(rows[0]["RM Currency"].ToString().Trim().ToUpper(), "CNY") == 0)
                        { dUnitPrice = Math.Round(Convert.ToDecimal(rows[0]["Unit Price(USD/KG)"].ToString().Trim()) * dRMBRate, 5); }
                        else { dUnitPrice = Convert.ToDecimal(rows[0]["Unit Price(USD/KG)"].ToString().Trim()); }
                    }
                    else { dUnitPrice = 0.0M; }
                    rows[0]["Unit Price(USD/KG)"] = Math.Round(dUnitPrice, 4);
                    rows[0]["Amount(USD)"] = Math.Round(dNetWeight * dUnitPrice, 4);
                }
                dtInvRMB.AcceptChanges();
            }
            dtInvRpt2.Dispose();
            dtInvRMB.Columns.Remove("RM Currency");

            sqlComm.CommandText = @"usp_BeiAnDan_RMD_GongDanInfoRpt";
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtGdInfo = new DataTable();
            sqlAdapter.Fill(dtGdInfo);
            sqlAdapter.Dispose();
            sqlComm.Parameters.RemoveAt("@GroupID");

            Microsoft.Office.Interop.Excel.Application excel_rpt = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_rpt = excel_rpt.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_rpt = workbooks_rpt.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets[1];
            worksheet_rpt.Name = this.cmbGroupID.Text.Trim().ToUpper().Replace("-", String.Empty);

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBomReport.Rows.Count + 1, 9]).NumberFormatLocal = "@";
            for (int x = 0; x < dtBomReport.Rows.Count; x++)
            {
                for (int y = 0; y < dtBomReport.Columns.Count - 3; y++)
                { worksheet_rpt.Cells[x + 2, y + 1] = "'" + dtBomReport.Rows[x][y].ToString().Trim(); }
                //avoid to generate the digit that the format is scientific notation for 'RM Used Qty' & 'Drools Quota' columns
                worksheet_rpt.Cells[x + 2, dtBomReport.Columns.Count - 2] = Math.Round(double.Parse(dtBomReport.Rows[x][dtBomReport.Columns.Count - 3].ToString().Trim()), 6);
                worksheet_rpt.Cells[x + 2, dtBomReport.Columns.Count - 1] = Math.Round(double.Parse(dtBomReport.Rows[x][dtBomReport.Columns.Count - 2].ToString().Trim()), 6);
            }

            for (int z = 0; z < dtBomReport.Columns.Count - 1; z++)
            { worksheet_rpt.Cells[1, z + 1] = dtBomReport.Columns[z].ColumnName.Trim().ToUpper(); }

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBomReport.Columns.Count - 1]).Font.Bold = true;
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBomReport.Columns.Count - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBomReport.Rows.Count + 1, dtBomReport.Columns.Count - 1]).Font.Name = "Verdana";
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBomReport.Rows.Count + 1, dtBomReport.Columns.Count - 1]).Font.Size = 9;
            worksheet_rpt.Cells.EntireColumn.AutoFit();
            excel_rpt.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            object missing = System.Reflection.Missing.Value;
            worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
            worksheet_rpt.Name = "Application Report";

            worksheet_rpt.Cells[1, 1] = "成品重量：";
            for (int i = 0; i < iAppFG1; i++)
            {
                string strFGName = dtAppFG1.Rows[i][0].ToString().Trim();
                worksheet_rpt.Cells[i + 2, 1] = strFGName;
                string strTotalFGQty = dtAppFG2.Compute("SUM([成品数量])", "[成品中文名称] = '" + strFGName + "'").ToString().Trim();
                decimal dTotalFGQty = Convert.ToDecimal(strTotalFGQty);
                worksheet_rpt.Cells[i + 2, 5] = dTotalFGQty.ToString() + " KG";
                worksheet_rpt.Cells[i + 2, 3] = Convert.ToString(Math.Ceiling(dTotalFGQty / 1000)) + " 托";
            }
            worksheet_rpt.Cells[iAppFG1 + 3, 1] = "产生边角料重量：";
            for (int j = 0; j < iAppDrools; j++)
            {
                string strDroolsName = dtAppDrools.Rows[j][0].ToString().Trim();
                worksheet_rpt.Cells[iAppFG1 + 4 + j, 1] = strDroolsName;
                string strTotalDroolsQty = dtBomReport.Compute("SUM([边角料])", "[Drools CH Name] = '" + strDroolsName + "'").ToString().Trim();
                decimal dTotalDroolsQty = Math.Round(Convert.ToDecimal(strTotalDroolsQty), 2);
                worksheet_rpt.Cells[iAppFG1 + 4 + j, 5] = dTotalDroolsQty.ToString() + " KG";
                worksheet_rpt.Cells[iAppFG1 + 4 + j, 3] = Convert.ToString(Math.Ceiling(dTotalDroolsQty / 250)) + " 托";
            }

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[iAppFG1 + iAppDrools + 3, 5]).Font.Name = "Verdana";
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[iAppFG1 + iAppDrools + 3, 5]).Font.Size = 9;
            worksheet_rpt.Cells.EntireColumn.AutoFit();
            excel_rpt.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            dtAppFG1.Dispose();
            dtAppFG2.Dispose();
            dtAppDrools.Dispose();
            dtBomReport.Dispose();

            missing = System.Reflection.Missing.Value;
            worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
            worksheet_rpt.Name = "BGD Report";

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBgdReport.Rows.Count + 1, 2]).NumberFormatLocal = "@";
            for (int a = 0; a < dtBgdReport.Rows.Count; a++)
            {
                for (int b = 0; b < dtBgdReport.Columns.Count; b++)
                { worksheet_rpt.Cells[a + 2, b + 1] = "'" + dtBgdReport.Rows[a][b].ToString().Trim(); }
            }
            for (int c = 0; c < dtBgdReport.Columns.Count; c++)
            { worksheet_rpt.Cells[1, c + 1] = dtBgdReport.Columns[c].ColumnName.Trim(); }

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBgdReport.Columns.Count]).Font.Bold = true;
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBgdReport.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBgdReport.Rows.Count + 1, dtBgdReport.Columns.Count]).Font.Name = "Verdana";
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBgdReport.Rows.Count + 1, dtBgdReport.Columns.Count]).Font.Size = 9;
            worksheet_rpt.Cells.EntireColumn.AutoFit();
            excel_rpt.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            dtBgdReport.Dispose();

            missing = System.Reflection.Missing.Value;
            worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
            worksheet_rpt.Name = "Invoice_USD Report";

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, dtInvUSD.Columns.Count]).NumberFormatLocal = "@";
            for (int f = 0; f < dtInvUSD.Rows.Count; f++)
            {
                for (int g = 0; g < dtInvUSD.Columns.Count; g++)
                { worksheet_rpt.Cells[f + 2, g + 1] = "'" + dtInvUSD.Rows[f][g].ToString().Trim(); }
            }
            for (int h = 0; h < dtInvUSD.Columns.Count; h++)
            { worksheet_rpt.Cells[1, h + 1] = dtInvUSD.Columns[h].ColumnName.Trim(); }

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtInvUSD.Columns.Count]).Font.Bold = true;
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, dtInvUSD.Columns.Count]).Font.Name = "Verdana";
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, dtInvUSD.Columns.Count]).Font.Size = 9;
            worksheet_rpt.Cells.EntireColumn.AutoFit();
            excel_rpt.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 3], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, 3]).ColumnWidth = 50;
            dtInvUSD.Dispose();

            missing = System.Reflection.Missing.Value;
            worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
            worksheet_rpt.Name = "Invoice_RMB Report";

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvRMB.Rows.Count + 1, dtInvRMB.Columns.Count]).NumberFormatLocal = "@";
            for (int k = 0; k < dtInvRMB.Rows.Count; k++)
            {
                for (int p = 0; p < dtInvRMB.Columns.Count; p++)
                { worksheet_rpt.Cells[k + 2, p + 1] = "'" + dtInvRMB.Rows[k][p].ToString().Trim(); }
            }
            for (int q = 0; q < dtInvRMB.Columns.Count; q++)
            { worksheet_rpt.Cells[1, q + 1] = dtInvRMB.Columns[q].ColumnName.Trim(); }

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtInvRMB.Columns.Count]).Font.Bold = true;
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvRMB.Rows.Count + 1, dtInvRMB.Columns.Count]).Font.Name = "Verdana";
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvRMB.Rows.Count + 1, dtInvRMB.Columns.Count]).Font.Size = 9;
            worksheet_rpt.Cells.EntireColumn.AutoFit();
            excel_rpt.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            dtInvRMB.Dispose();

            missing = System.Reflection.Missing.Value;
            worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
            worksheet_rpt.Name = "GD_Information Report";

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtGdInfo.Rows.Count + 1, dtGdInfo.Columns.Count]).NumberFormatLocal = "@";
            for (int r = 0; r < dtGdInfo.Rows.Count; r++)
            {
                for (int s = 0; s < dtGdInfo.Columns.Count; s++)
                { worksheet_rpt.Cells[r + 2, s + 1] = dtGdInfo.Rows[r][s].ToString().Trim(); }
            }
            for (int t = 0; t < dtGdInfo.Columns.Count; t++)
            { worksheet_rpt.Cells[1, t + 1] = dtGdInfo.Columns[t].ColumnName.Trim(); }

            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtGdInfo.Columns.Count]).Font.Bold = true;
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtGdInfo.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtGdInfo.Rows.Count + 1, dtGdInfo.Columns.Count]).Font.Name = "Verdana";
            //worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtGdInfo.Rows.Count + 1, dtGdInfo.Columns.Count]).Font.Size = 9;
            worksheet_rpt.Cells.EntireColumn.AutoFit();
            excel_rpt.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            dtGdInfo.Dispose();

            excel_rpt.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_rpt);
            worksheet_rpt = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_rpt);
            excel_rpt = null;
        }
    }
}
