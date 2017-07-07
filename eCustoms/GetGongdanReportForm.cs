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
    public partial class GetGongdanReportForm : Form
    {
        DataTable dtGongDanDoc = new DataTable();
        DataTable dtGongDanList = new DataTable();       
        private LoginForm loginFrm = new LoginForm();

        private static GetGongdanReportForm getGongdanRptFrm;
        public GetGongdanReportForm() { InitializeComponent(); }   
        public static GetGongdanReportForm CreateInstance()
        {
            if (getGongdanRptFrm == null || getGongdanRptFrm.IsDisposed) { getGongdanRptFrm = new GetGongdanReportForm(); }
            return getGongdanRptFrm;
        }

        private void GetCustomsGongdanForm_Load(object sender, EventArgs e)
        { this.dtpApprovedDate.CustomFormat = " "; }
        private void GetCustomsGongdanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtGongDanList.Dispose();
            dtGongDanDoc.Dispose();
        }
        private void dtpApprovedDate_ValueChanged(object sender, EventArgs e)
        { this.dtpApprovedDate.CustomFormat = "M/dd/yyyy HH:mm"; }
        private void dtpApprovedDate_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpApprovedDate.CustomFormat = " "; } }

        private void btnGongDan_Click(object sender, EventArgs e)
        {
            SqlConnection gdConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gdConn.State == ConnectionState.Closed) { gdConn.Open(); }
            //string strSQL = "SELECT [GongDan No] AS [工单号], N'加工' AS [生产类型], [FG No] + '/' + [Batch No] AS [成品备件号], [GongDan Qty] AS [工单数量], " +
            //                "0 AS [项号], [RM EHB] AS [物料备件号], [RM Used Qty] AS [物料耗用数量], [CN] AS [原产国], [BGD No] AS [批次号], [BOM In Customs] FROM (" +
            //                "SELECT M.[GongDan No], SUBSTRING(M.[FG Description],0,CHARINDEX('-',M.[FG Description],CHARINDEX('-',M.[FG Description],0)+1)) AS [FG No], " + 
            //                "M.[Batch No], M.[GongDan Qty], [RM EHB], B.[CN], SUM(CAST(M.[RM Used Qty] AS decimal(18,6))) AS [RM Used Qty], M.[BGD No], " + 
            //                "M.[BOM In Customs] FROM M_DailyGongDan AS M LEFT OUTER JOIN B_Country AS B ON M.[Destination] = B.[EN] " + 
            //                "WHERE M.[RM Used Qty] > 0.0 GROUP BY M.[GongDan No], M.[FG Description], M.[Batch No], [GongDan Qty], M.[RM EHB], B.[CN], " + 
            //                "M.[BGD No], M.[BOM In Customs]) AS tbgd";
            string strSQL = "SELECT [GongDan No] AS [工单号], N'加工' AS [生产类型], [FG No] + '/' + [Batch No] AS [成品备件号], [GongDan Qty] AS [工单数量], " +
                            "0 AS [项号], [RM EHB] AS [物料备件号], [RM Used Qty] AS [物料耗用数量], [CN] AS [原产国], [BGD No] AS [批次号], [BOM In Customs] FROM ( " +
                            "SELECT M.[GongDan No], SUBSTRING(M.[FG Description],0,CHARINDEX('-',M.[FG Description],CHARINDEX('-',M.[FG Description],0)+1)) AS [FG No], " +
                            "M.[Batch No], M.[GongDan Qty], M.[RM EHB], N.[CN], SUM(CAST(M.[RM Used Qty] AS decimal(18,6))) AS [RM Used Qty], M.[BGD No], " +
                            "M.[BOM In Customs] FROM M_DailyGongDan AS M LEFT OUTER JOIN (SELECT C.[Item No], C.[Lot No], B.[CN] FROM C_RMReceiving AS C INNER JOIN B_Country AS B ON C.[Country of Origin] = B.[EN]) AS N ON M.[Item No] = N.[Item No] AND M.[Lot No] = N.[Lot No] " +
                            "WHERE M.[RM Used Qty] > 0.0 GROUP BY M.[GongDan No], M.[FG Description], M.[Batch No], M.[GongDan Qty], M.[RM EHB], N.[CN], " +
                            "M.[BGD No], M.[BOM In Customs]) AS tbgd";
            SqlDataAdapter gdAdapter = new SqlDataAdapter(strSQL, gdConn);
            dtGongDanDoc.Clear();
            gdAdapter.Fill(dtGongDanDoc);
            gdAdapter.Dispose();
            SqlLib sqlLib = new SqlLib();
            string[] strFieldName = { "工单号" };
            DataTable dtGongDanNo = sqlLib.SelectDistinct(dtGongDanDoc, strFieldName);
            sqlLib.Dispose(0);
            string strGongDan = "";
            int iLine = 0;
            foreach (DataRow drow in dtGongDanDoc.Rows)
            {
                string strGD = drow[0].ToString().Trim();
                if (String.Compare(strGongDan, strGD) == 0)
                {
                    iLine++;
                    drow["项号"] = iLine;
                }
                else
                {
                    strGongDan = strGD;
                    iLine = 1;
                    drow["项号"] = iLine;
                }
            }
            dtGongDanDoc.AcceptChanges();
            if (dtGongDanDoc.Rows.Count > 0)
            {
                this.dgvGongDan.DataSource = dtGongDanDoc;
                this.dgvGongDanList.DataSource = dtGongDanNo;
            }
            else
            {
                this.dgvGongDan.DataSource = DBNull.Value;
                this.dgvGongDanList.DataSource = DBNull.Value;
            }
            this.dgvGongDanList.Columns[0].HeaderText = "Select";
            if (gdConn.State == ConnectionState.Open) { gdConn.Close(); gdConn.Dispose(); }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDan.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (this.dgvGongDanList.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select the data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            this.GetGongDanData(); 
         
            Microsoft.Office.Interop.Excel.Application xls_GD = new Microsoft.Office.Interop.Excel.Application();
            xls_GD.Application.Workbooks.Add(true);
            for (int i = 0; i < dtGongDanList.Rows.Count; i++)
            {
                for (int j = 0; j < dtGongDanList.Columns.Count - 1; j++) 
                {
                    if (j == 8) { xls_GD.Cells[i + 2, j + 1] = "'" + dtGongDanList.Rows[i][j].ToString().Trim(); }
                    else { xls_GD.Cells[i + 2, j + 1] = dtGongDanList.Rows[i][j].ToString().Trim(); }                   
                }
                if (!String.IsNullOrEmpty(dtGongDanList.Rows[i][9].ToString().Trim())) { xls_GD.Cells[i + 2, 3] = dtGongDanList.Rows[i][9].ToString().Trim(); }
                xls_GD.Cells[i + 2, 7] = String.Format("{0:0.000000}", Convert.ToDecimal(dtGongDanList.Rows[i][6].ToString().Trim()));
            }
            for (int k = 0; k < dtGongDanList.Columns.Count - 1; k++) { xls_GD.Cells[1, k + 1] = dtGongDanList.Columns[k].ColumnName.ToString().Trim(); }

            xls_GD.Cells.EntireColumn.AutoFit();
            xls_GD.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            xls_GD.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xls_GD);
            xls_GD = null;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDan.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (this.dgvGongDanList.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select GongDan No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            this.GetGongDanData();
            SqlLib lib = new SqlLib();
            string[] strFieldName = { "工单号" }; //dtGongDanList.Columns[0].ColumnName.Trim() 
            DataTable dtGD = lib.SelectDistinct(dtGongDanList, strFieldName);
            lib.Dispose(0);
            
            SqlConnection apprGdConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (apprGdConn.State == ConnectionState.Closed) { apprGdConn.Open(); }
            SqlCommand apprGdComm = new SqlCommand();
            apprGdComm.Connection = apprGdConn;
            string strGongDanRecord = null;
            apprGdComm.CommandText = "SELECT * FROM B_MultiUser";
            string strUserName = Convert.ToString(apprGdComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait for him/her to finish the process.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    apprGdComm.Dispose();
                    apprGdConn.Dispose();
                    return;
                }
            }
            else
            {
                apprGdComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                apprGdComm.CommandText = "INSERT INTO B_MultiUser([UserName]) VALUES(@UserName)";
                apprGdComm.ExecuteNonQuery();
                apprGdComm.Parameters.Clear();
            }
            DateTime dtApproved = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm"));
            if (!String.IsNullOrEmpty(this.dtpApprovedDate.Text.Trim())) { dtApproved = Convert.ToDateTime(this.dtpApprovedDate.Value.ToString("M/d/yyyy HH:mm")); }
            for (int x = 0; x < dtGD.Rows.Count; x++)
            {
                string strGongDan = dtGD.Rows[x][0].ToString().Trim();
                apprGdComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;
                apprGdComm.CommandText = "SELECT DISTINCT [Actual Start Date], [Actual End Date], [Batch No], [BOM In Customs], [GongDan No], [FG No], [FG Description], " +
                                         "[IE Type], [Order No], [GongDan Qty], [Order Price], [Order Currency], CAST([Total RM Qty] AS decimal(18, 6)) AS [Total RM Qty], " + 
                                         "[Total RM Cost(USD)], CAST([Drools Rate] AS decimal(18,6)) AS [DroolsRate], [CHN Name], [Destination], '" + loginFrm.PublicUserName +
                                         "' AS [Creater], [Created Date], '" + dtApproved + "' AS [Approval Date] FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                SqlDataAdapter apprGdAdap = new SqlDataAdapter();
                apprGdAdap.SelectCommand = apprGdComm;
                DataTable dtApprGongDanM = new DataTable();
                apprGdAdap.Fill(dtApprGongDanM);
                string strFG = dtApprGongDanM.Rows[0]["FG No"].ToString().Trim();
                string strGongDanQty = dtApprGongDanM.Rows[0]["GongDan Qty"].ToString().Trim();

                apprGdComm.CommandText = "SELECT [Batch Path], [GongDan No], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], " +
                                         "[RM EHB], [BGD No], CAST([RM Used Qty] AS decimal(18, 6)) AS [RM Used Qty], [RM Currency], [RM Price], " + 
                                         "CAST([Consumption] AS decimal(18, 6)) AS [Consumption], CAST([Drools Quota] AS decimal(18, 8)) AS [Drools Quota], " +
                                         "[Drools EHB] FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                apprGdAdap.SelectCommand = apprGdComm;
                DataTable dtApprGongDanD = new DataTable();
                apprGdAdap.Fill(dtApprGongDanD);
                //apprGdComm.CommandText = "SELECT [GongDan No] AS [工单号], N'加工' AS [生产类型], [FG No] + '/' + [Batch No] AS [成品备件号], [GongDan Qty] AS [工单数量], " + 
                //                         "ROW_NUMBER() OVER (ORDER BY [RM EHB], [BGD No]) AS [项号], [RM EHB] AS [物料备件号], [RM Used Qty] AS [物料耗用数量], " + 
                //                         "[CN] AS [原产国], [BGD No] AS [批次号], [BOM In Customs], '" + dtApproved + "' AS [Created Date] FROM (" +
                //                         "SELECT M.[GongDan No], SUBSTRING(M.[FG Description],0,CHARINDEX('-',M.[FG Description],CHARINDEX('-',M.[FG Description],0)+1)) AS [FG No], " + 
                //                         "M.[Batch No], M.[GongDan Qty], M.[RM EHB], CAST(SUM(M.[RM Used Qty]) AS decimal(18, 6)) AS [RM Used Qty], B.[CN], " + 
                //                         "M.[BGD No], M.[BOM In Customs] FROM M_DailyGongDan AS M LEFT OUTER JOIN B_Country AS B ON M.[Destination] = B.[EN] " +
                //                         "WHERE [RM Used Qty] > 0.0 GROUP BY M.[GongDan No], M.[FG Description], M.[Batch No], M.[GongDan Qty], M.[RM EHB], B.[CN], " +
                //                         "M.[BGD No], M.[BOM In Customs] HAVING [GongDan No] = @GongDanNo) tbgdd";
                apprGdComm.CommandText = "SELECT [GongDan No] AS [工单号], N'加工' AS [生产类型], [FG No] + '/' + [Batch No] AS [成品备件号], [GongDan Qty] AS [工单数量], " +
                                         "ROW_NUMBER() OVER (ORDER BY [RM EHB], [BGD No]) AS [项号], [RM EHB] AS [物料备件号], [RM Used Qty] AS [物料耗用数量], " +
                                         "[CN] AS [原产国], [BGD No] AS [批次号], [BOM In Customs], '" + dtApproved + "' AS [Created Date] FROM (" +
                                         "SELECT M.[GongDan No], SUBSTRING(M.[FG Description],0,CHARINDEX('-',M.[FG Description],CHARINDEX('-',M.[FG Description],0)+1)) AS [FG No], " +
                                         "M.[Batch No], M.[GongDan Qty], M.[RM EHB], CAST(SUM(M.[RM Used Qty]) AS decimal(18, 6)) AS [RM Used Qty], N.[CN], " +
                                         "M.[BGD No], M.[BOM In Customs] FROM M_DailyGongDan AS M LEFT OUTER JOIN (SELECT C.[Item No], C.[Lot No], B.[CN] FROM C_RMReceiving AS C " + 
                                         "INNER JOIN B_Country AS B ON C.[Country of Origin] = B.[EN]) AS N ON M.[Item No] = N.[Item No] AND M.[Lot No] = N.[Lot No] " +
                                         "WHERE [RM Used Qty] > 0.0 GROUP BY M.[GongDan No], M.[FG Description], M.[Batch No], M.[GongDan Qty], M.[RM EHB], N.[CN], " +
                                         "M.[BGD No], M.[BOM In Customs] HAVING [GongDan No] = @GongDanNo) tbgdd";
                apprGdAdap.SelectCommand = apprGdComm;
                DataTable dtApprGongDanDoc = new DataTable();
                apprGdAdap.Fill(dtApprGongDanDoc);

                /*------ check and update 'RM-D' IEType to 'RMB-1418' or 'RMB-D' ------*/
                apprGdComm.CommandText = "SELECT [GongDan No], [IsAllocated] FROM (SELECT M.[GongDan No], B.[IsAllocated], MAX(M.[Line No]) Line, " + 
                                         "COUNT(M.[GongDan No]) CountNo FROM M_DailyGongDan AS M LEFT JOIN (SELECT DISTINCT [Grade], [IsAllocated] FROM B_HsCode) AS B " +
                                         "ON SUBSTRING(M.[FG Description], 0, CHARINDEX('-', M.[FG Description], 0)) = B.[Grade] " +
                                         "WHERE M.[IE Type] = 'RM-D' AND M.[RM Category] = 'USD' AND M.[Consumption] > 0.0 GROUP BY M.[GongDan No], " +
                                         "B.[IsAllocated] HAVING [GongDan No] = @GongDanNo) AS dtie WHERE Line = CountNo";
                //apprGdComm.CommandText = "SELECT [GongDan No], [IsAllocated] FROM (SELECT M.[GongDan No], B.[IsAllocated], MAX(M.[Line No]) Line, " +
                //                         "COUNT(M.[GongDan No]) CountNo FROM M_DailyGongDan AS M LEFT JOIN (SELECT DISTINCT [Legacy Code], [IsAllocated] FROM B_HS) AS B " +
                //                         "ON SUBSTRING(M.[FG Description],0,CHARINDEX('-',M.[FG Description],CHARINDEX('-',M.[FG Description],0)+1)) = B.[Legacy Code] " +
                //                         "WHERE M.[IE Type] = 'RM-D' AND M.[RM Category] = 'USD' AND M.[Consumption] > 0.0 GROUP BY M.[GongDan No], " +
                //                         "B.[IsAllocated] HAVING [GongDan No] = @GongDanNo) AS dtie WHERE Line = CountNo";
                apprGdAdap.SelectCommand = apprGdComm;
                DataTable dtChangeIE = new DataTable();
                apprGdAdap.Fill(dtChangeIE);
                apprGdAdap.Dispose();
                foreach(DataRow drChangeIE in dtChangeIE.Rows)
                {
                    string strGD = drChangeIE[0].ToString().Trim();
                    string strAllocated = drChangeIE[1].ToString().Trim();
                    DataRow[] drow = dtApprGongDanM.Select("[GongDan No] = '" + strGD + "'");
                    if (drow.Length > 0)
                    {
                        if (String.Compare(strAllocated, "True") == 0) 
                        { 
                            foreach (DataRow dr in drow) { dr["IE Type"] = "RMB-D"; }
                            strGongDanRecord += strGD + " : RMB-D\n";
                        }
                        else 
                        {
                            foreach (DataRow dr in drow) { dr["IE Type"] = "RMB-1418"; }
                            strGongDanRecord += strGD + " : RMB-1418\n";
                        }
                    }                 
                }                
                dtChangeIE.Dispose();
                dtApprGongDanM.AcceptChanges();

                #region //Update C_BOM table's column: GongDan Used Qty
                apprGdComm.Parameters.Clear();
                apprGdComm.CommandType = CommandType.StoredProcedure;
                apprGdComm.CommandText = @"usp_UpdateGongDanUsedQty";               
                apprGdComm.Parameters.AddWithValue("@GongDanNo", strGongDan);
                apprGdComm.Parameters.AddWithValue("@BatchNo", strGongDan.Split('-')[0].Trim());
                apprGdComm.Parameters.AddWithValue("@Judge", "ADD");
                apprGdComm.ExecuteNonQuery();
                apprGdComm.Parameters.Clear();
                #endregion
                #region //Handle C_GongDan, C_GongDanDetail, E_GongDan table's data
                apprGdComm.CommandText = @"usp_InsertGongDan";
                apprGdComm.Parameters.AddWithValue("@tvp_GongDanMaster", dtApprGongDanM);
                apprGdComm.ExecuteNonQuery();
                apprGdComm.Parameters.Clear();
                dtApprGongDanM.Dispose();

                apprGdComm.CommandText = @"usp_InsertGongDanDetail";
                apprGdComm.Parameters.AddWithValue("@tvp_GongDanDetail", dtApprGongDanD);
                apprGdComm.ExecuteNonQuery();
                apprGdComm.Parameters.Clear();
                dtApprGongDanD.Dispose();

                apprGdComm.CommandText = @"usp_InsertGongDanDoc";
                apprGdComm.Parameters.AddWithValue("@tvp_GongDanDoc", dtApprGongDanDoc);
                apprGdComm.ExecuteNonQuery();
                apprGdComm.Parameters.Clear();
                dtApprGongDanDoc.Dispose(); 
                #endregion                                                          
                #region //Update C_RMBalance table's column: Available RM Balance, GongDan Pending
                apprGdComm.CommandText = "usp_UpdateRMBalanceByGongDan";
                apprGdComm.Parameters.AddWithValue("@GongDanNo", strGongDan);
                apprGdComm.Parameters.AddWithValue("@Action", "ADD");
                apprGdComm.ExecuteNonQuery();
                apprGdComm.Parameters.Clear();
                #endregion
             
                apprGdComm.CommandType = CommandType.Text;
                apprGdComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;
                apprGdComm.CommandText = "DELETE FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                apprGdComm.ExecuteNonQuery();
                apprGdComm.Parameters.Clear();                
            }
            dtGD.Dispose();

            apprGdComm.CommandText = "DELETE FROM B_MultiUser";
            apprGdComm.ExecuteNonQuery();
            apprGdComm.Dispose();
            if (apprGdConn.State == ConnectionState.Open) { apprGdConn.Close(); apprGdConn.Dispose(); }
            string strComment = String.IsNullOrEmpty(strGongDanRecord) ? string.Empty : "\nBelow GongDan's (IE Type) convert to RMB-D or RMB-1418 from RM-D.\nPlease inform Logistics team and XFZ !!!" + strGongDanRecord;
            if (MessageBox.Show("Successfully approve." + strComment, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                dtGongDanList.Clear();
                this.btnGongDan_Click(sender, e); 
            }
        }

        private void GetGongDanData()
        {
            if (this.dgvGongDanList.Columns[0].HeaderText == "Reverse")
            {
                DataTable dtMid = new DataTable();
                dtMid.Columns.Add("GongDan", typeof(string));
                for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "False") == 0)
                    {
                        DataRow dr = dtMid.NewRow();
                        dr[0] = this.dgvGongDanList[1, i].Value.ToString().Trim();
                        dtMid.Rows.Add(dr);
                    }
                }
                dtGongDanList.Clear();
                dtGongDanList = dtGongDanDoc.Copy();
                foreach (DataRow drMid in dtMid.Rows)
                {
                    DataRow[] drow = dtGongDanList.Select("[工单号]='" + drMid[0].ToString().Trim() + "'");
                    foreach (DataRow dr in drow) { dtGongDanList.Rows.Remove(dr); }
                }
                dtGongDanList.AcceptChanges();
                dtMid.Dispose();
            }
            else 
            { 
                dtGongDanList.Clear(); 
                dtGongDanList = dtGongDanDoc.Copy(); 
            }
        }

        private void dgvGongDanList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvGongDanList.RowCount; i++) { this.dgvGongDanList[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvGongDanList.RowCount; i++) { this.dgvGongDanList[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
                    {
                        if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvGongDanList[0, i].Value = true; }
                        else { this.dgvGongDanList[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }
            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvGongDanList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvGongDanList.RowCount == 0) { return; }
            if (this.dgvGongDanList.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }
                if (iCount < this.dgvGongDanList.RowCount && iCount > 0)
                { this.dgvGongDanList.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvGongDanList.RowCount)
                { this.dgvGongDanList.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvGongDanList.Columns[0].HeaderText = "Select"; }
            }
        }      
    }
}

