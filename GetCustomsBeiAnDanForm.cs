﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class GetCustomsBeiAnDanForm : Form
    {
        DataTable dt = new DataTable();
        DataTable dtName = new DataTable();
        private LoginForm loginFrm = new LoginForm();

        private static GetCustomsBeiAnDanForm getCustomsBeiAnDanFrm;
        public GetCustomsBeiAnDanForm()
        {
            InitializeComponent();
        }
        public static GetCustomsBeiAnDanForm CreateInstance()
        {
            if (getCustomsBeiAnDanFrm == null || getCustomsBeiAnDanFrm.IsDisposed)
            {
                getCustomsBeiAnDanFrm = new GetCustomsBeiAnDanForm();
            }
            return getCustomsBeiAnDanFrm;
        }

        private void GetCustomsBeiAnDanForm_Load(object sender, EventArgs e)
        {
            SqlLib sqlLib = new SqlLib();
            dt = sqlLib.GetDataTable(@"SELECT * FROM B_IEType WHERE [IE Type] <> 'RM-D'", "B_IEType").Copy();
            DataRow dr = dt.NewRow();
            dr["IE Type"] = String.Empty;
            dt.Rows.InsertAt(dr, 0);
            this.cmbIEtype.DisplayMember = this.cmbIEtype.ValueMember = "IE Type";
            this.cmbIEtype.DataSource = dt;
            sqlLib.Dispose();
        }

        private void GetCustomsBeiAnDanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dt.Dispose();
            dtName.Dispose();
        }

        private void cmbIEtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Compare(this.cmbIEtype.Text.Trim(), "1418") == 0 ||
                String.Compare(this.cmbIEtype.Text.Trim(), "RMB-1418") == 0 ||
                String.Compare(this.cmbIEtype.Text.Trim(), "RMB-D") == 0)
            {
                this.txtBeiAnDanNo.Enabled = true;
                this.btnReport.Enabled = false; 
            }
            else if (String.Compare(this.cmbIEtype.Text.Trim(), "BLP") == 0 ||
                     String.Compare(this.cmbIEtype.Text.Trim(), "EXPORT") == 0 ||
                     String.Compare(this.cmbIEtype.Text.Trim(), "RMB") == 0)
            {
                this.txtBeiAnDanNo.Enabled = false;
                if (String.Compare(this.cmbIEtype.Text.Trim(), "RMB") == 0) { this.btnReport.Enabled = true; }
            }
            else
            {
                this.txtBeiAnDanNo.Enabled = true;
                this.btnReport.Enabled = false; 
            }

            this.cmbGroupID.Text = String.Empty;
            this.txtBeiAnDanID.Text = String.Empty;
            this.txtBeiAnDanNo.Text = String.Empty;
            this.cmbGroupID.DataSource = null;
        }

        private void btnGatherDoc_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEtype.Text.Trim()))
            {
                MessageBox.Show("Please select IE Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbIEtype.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = @"SELECT * FROM B_WeightRatio";
            decimal dRatio = Convert.ToDecimal(sqlComm.ExecuteScalar());

            string strIEtype = this.cmbIEtype.Text.Trim().ToUpper();
            if (String.Compare(strIEtype, "EXPORT") == 0)
            {
                sqlComm.CommandText = @"SELECT [Group ID], '' AS [备案单号], '' AS [单证类型], '0.00' AS [总毛重], '' AS [贸易方式], '' AS [结转企业代码], 0 AS [项号], " +
                                       "[FG EHB] AS [备件号], [GongDan Qty] AS [数量], [GongDan Qty] AS [净重], '0.00' AS [毛重], CASE WHEN [OF Rev Amt] > 0.0 THEN " +
                                       "[OF Rev Amt] ELSE CASE WHEN [IE Rev Amt] > 0.0 THEN [IE Rev Amt] ELSE [Selling Amount] END END AS [金额], N'美元' AS [币制], " +
                                       "[Destination] AS [原产地/目的地], [GongDan No] AS [工单/批次号], [IE Type], [Is Generated Doc], [Generated Doc Date] " +
                                       "FROM M_PendingBeiAnDan_Export WHERE [IE Type] = 'EXPORT' AND [Pending] = 'False' ORDER BY [Group ID], [GongDan No]";
            }
            else
            {
                sqlComm.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = strIEtype;
                sqlComm.CommandText = @"SELECT [Group ID], '' AS [备案单号], '' AS [单证类型], '0.00' AS [总毛重], '' AS [贸易方式], '' AS [结转企业代码], 0 AS [项号], " +
                                       "[FG EHB] AS [备件号], [GongDan Qty] AS [数量], [GongDan Qty] AS [净重], '0.00' AS [毛重], CASE WHEN [OF Rev Amt] > 0.0 THEN " +
                                       "[OF Rev Amt] ELSE CASE WHEN [IE Rev Amt] > 0.0 THEN [IE Rev Amt] ELSE [Selling Amount] END END AS [金额], N'美元' AS [币制], " +
                                       "[Destination] AS [原产地/目的地], [GongDan No] AS [工单/批次号], [IE Type], [Is Generated Doc], [Generated Doc Date] " +
                                       "FROM M_DailyBeiAnDan WHERE [IE Type] = @IEtype ORDER BY [Group ID], [GongDan No]";
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            dtName.Rows.Clear();
            dtName.Columns.Clear();
            dtName.Columns.Add("总毛重", typeof(decimal));
            dtName.Columns.Add("毛重", typeof(decimal));
            sqlAdapter.Fill(dtName);
            sqlAdapter = new SqlDataAdapter(@"SELECT [Short Name], [Address] FROM B_Address", sqlConn);
            DataTable dtDestination = new DataTable();
            sqlAdapter.Fill(dtDestination);
            sqlAdapter.Dispose();
            sqlComm.Parameters.Clear();
            dtName.Columns["总毛重"].SetOrdinal(4);
            dtName.Columns["毛重"].SetOrdinal(10);

            if (dtName.Rows.Count > 0)
            {
                int iNumber = 0;
                string strGroupID = dtName.Rows[0]["Group ID"].ToString().Trim();
                foreach (DataRow dr in dtName.Rows)
                {
                    if (String.Compare(strIEtype, "1418") == 0 ||
                        String.Compare(strIEtype, "RMB-1418") == 0 ||
                        String.Compare(strIEtype, "RMB-D") == 0)
                    {
                        dr["单证类型"] = "成品移库";
                        dr["贸易方式"] = "转库";
                        dr["结转企业代码"] = "1418";
                        dr["原产地/目的地"] = "中国";
                    }
                    else if (String.Compare(strIEtype, "RMB") == 0)
                    {
                        dr["单证类型"] = "成品内销（料件）";
                        dr["贸易方式"] = "保区进料料件";
                        dr["结转企业代码"] = String.Empty;
                        dr["原产地/目的地"] = "中国";
                    }
                    else
                    {
                        dr["单证类型"] = "成品报关出区";
                        dr["贸易方式"] = "进料对口";
                        dr["结转企业代码"] = String.Empty;
                        if (String.Compare(strIEtype, "EXPORT") == 0)
                        {
                            DataRow[] drCount = dtDestination.Select("[Short Name] = '" + dr["原产地/目的地"].ToString().Trim() + "'");
                            if (drCount.Length > 0) { dr["原产地/目的地"] = drCount[0][1].ToString().Trim(); }
                            else { dr["原产地/目的地"] = String.Empty; }
                        }
                        else { dr["原产地/目的地"] = "中国"; } //IE Type = BLP
                    }
                    dr["毛重"] = Convert.ToDecimal(dr["净重"].ToString()) * dRatio;
                    if (String.Compare(dr["Group ID"].ToString().Trim(), strGroupID) != 0)
                    {
                        decimal dTotalGW = (decimal)dtName.Compute("SUM([毛重])", "[Group ID] = '" + strGroupID + "'");
                        DataRow[] dRow = dtName.Select("[Group ID] = '" + strGroupID + "'");
                        foreach (DataRow row in dRow) { row["总毛重"] = dTotalGW; }
                        strGroupID = dr["Group ID"].ToString().Trim();
                        iNumber = 0;
                    }
                    dr["项号"] = ++iNumber;
                }
                decimal dSUMGW = (decimal)dtName.Compute("SUM([毛重])", "[Group ID] = '" + strGroupID + "'");
                DataRow[] rows = dtName.Select("[Group ID] = '" + strGroupID + "'");
                foreach (DataRow row in rows) { row["总毛重"] = dSUMGW; }

                this.dgvBeiAnDanDoc.DataSource = DBNull.Value;
                this.dgvBeiAnDanDoc.DataSource = dtName;
                this.dgvBeiAnDanDoc.Columns["备案单号"].Visible = false;
                this.dgvBeiAnDanDoc.Columns["Group ID"].Frozen = true;
            }
            else
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtName.Rows.Clear();
                dtName.Columns.Clear();
                this.dgvBeiAnDanDoc.DataSource = DBNull.Value;
            }

            dtDestination.Dispose();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDanDoc.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDanDoc.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlLib SQLLib = new SqlLib();
            string[] strFile = { "Group ID", "Is Generated Doc" };
            DataTable dtbl = SQLLib.SelectDistinct(dtName, strFile);
            SQLLib.Dispose(0);

            DataRow[] drow = dtbl.Select("[Is Generated Doc] = 'true'");
            foreach (DataRow dr in drow)
            { if (Boolean.Parse(dr["Is Generated Doc"].ToString().Trim())) { dr.Delete(); } }
            if (dtbl.Rows.Count == 0)
            {
                MessageBox.Show("There is not new data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnDownload.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = @"SELECT * FROM B_WeightRatio";
            decimal dRatio = Convert.ToDecimal(sqlComm.ExecuteScalar());
            SqlDataAdapter sqlAdp = new SqlDataAdapter(@"SELECT DISTINCT [Address] AS [原产地], [Code] AS [目的国] FROM B_Address", sqlConn);
            DataTable dtCode = new DataTable();
            sqlAdp.Fill(dtCode);
            dtName.Columns["原产地/目的地"].ColumnName = "原产地";
            SqlLib lib = new SqlLib();
            DataTable dtCopy = lib.MergeDataTable(dtName, dtCode, "原产地");            
            dtCode.Clear();
            dtCode.Dispose();
            dtName.Columns["原产地"].ColumnName = "原产地/目的地";
            DataRow[] drDGV = dtCopy.Select("[Is Generated Doc] = 'false'");
            DataTable myTbl = dtName.Clone();
            myTbl.Columns.Add("目的国", typeof(string));
            foreach (DataRow dr in drDGV) { myTbl.ImportRow(dr); }
            string[] strFields = { "Group ID", "备件号" };
            DataTable dtBAD = lib.SelectDistinct(myTbl, strFields);
            lib.Dispose(0);

            Microsoft.Office.Interop.Excel.Application excel_Doc = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_Doc = excel_Doc.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_Doc = workbooks_Doc.Add(true);           
            Microsoft.Office.Interop.Excel.Worksheet worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets[1];
            worksheet_Doc.Name = "BeiAnDan_Report";

            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[drDGV.Length + 1, drDGV[0].Table.Columns.Count - 4]).NumberFormatLocal = "@";
            for (int x = 0; x < drDGV.Length; x++)
            {
                for (int y = 0; y < drDGV[0].Table.Columns.Count - 4; y++)
                { worksheet_Doc.Cells[x + 2, y + 1] = drDGV[x][y].ToString().Trim(); }
            }

            for (int z = 0; z < this.dgvBeiAnDanDoc.ColumnCount - 3; z++)
            { worksheet_Doc.Cells[1, z + 1] = this.dgvBeiAnDanDoc.Columns[z].HeaderText.ToString().Trim(); }

            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, drDGV[0].Table.Columns.Count - 4]).Font.Bold = true;
            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, drDGV[0].Table.Columns.Count - 4]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[drDGV.Length + 1, drDGV[0].Table.Columns.Count - 4]).Font.Name = "Verdana";
            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[drDGV.Length + 1, drDGV[0].Table.Columns.Count - 4]).Font.Size = 9;
            worksheet_Doc.Cells.EntireColumn.AutoFit();
            dtCopy.Clear();
            dtCopy.Dispose();

            string strIEtype = this.cmbIEtype.Text.Trim().ToUpper();
            object missing = System.Reflection.Missing.Value;
            if (String.Compare(strIEtype, "1418") != 0 && String.Compare(strIEtype, "RMB-1418") != 0 && String.Compare(strIEtype, "RMB-D") != 0)
            {
                dtBAD.Columns.Add("数量", typeof(string));
                dtBAD.Columns.Add("毛重", typeof(string));
                dtBAD.Columns.Add("金额", typeof(string));
                dtBAD.Columns.Add("目的国", typeof(string));
                foreach(DataRow dr in dtBAD.Rows)
                {
                    string strGroupID = dr["Group ID"].ToString().Trim();
                    string strEHB = dr["备件号"].ToString().Trim();
                    dr["数量"] = myTbl.Compute("SUM([数量])", "[Group ID] = '" + strGroupID + "' AND [备件号] = '" + strEHB + "'").ToString();
                    dr["毛重"] = myTbl.Compute("SUM([毛重])", "[Group ID] = '" + strGroupID + "' AND [备件号] = '" + strEHB + "'").ToString();
                    dr["金额"] = myTbl.Compute("SUM([金额])", "[Group ID] = '" + strGroupID + "' AND [备件号] = '" + strEHB + "'").ToString();
                    dr["目的国"] = myTbl.Select("[Group ID] = '" + strGroupID + "' AND [备件号] = '" + strEHB + "'")[0]["目的国"].ToString();
                }
                dtBAD.AcceptChanges();
          
                worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);
                worksheet_Doc.Name = "New_BeiAnDan";
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtBAD.Rows.Count + 1, 14]).NumberFormatLocal = "@";
                string strHandBook = "C014", strHostCode = "3122442019", strBZ = "502", strJF = null, strCQ = null, strMY = null, strRQ = System.DateTime.Now.ToString("yyyyMMdd");
                if (String.Compare(strIEtype, "EXPORT") == 0 || String.Compare(strIEtype, "BLP") == 0) { strJF = "1"; strCQ = "3"; strMY = "0615"; }
                else { strJF = "0"; strCQ = "2"; strMY = "0544"; }

                for (int a = 0; a < dtBAD.Rows.Count; a++)
                {
                    worksheet_Doc.Cells[a + 2, 1] = dtBAD.Rows[a]["Group ID"].ToString().Trim();
                    worksheet_Doc.Cells[a + 2, 2] = strHandBook;
                    worksheet_Doc.Cells[a + 2, 3] = strHostCode;
                    worksheet_Doc.Cells[a + 2, 4] = strJF;
                    worksheet_Doc.Cells[a + 2, 5] = strCQ;
                    worksheet_Doc.Cells[a + 2, 6] = strMY;
                    worksheet_Doc.Cells[a + 2, 7] = strRQ;
                    worksheet_Doc.Cells[a + 2, 8] = dtBAD.Rows[a]["备件号"].ToString().Trim();
                    worksheet_Doc.Cells[a + 2, 9] = dtBAD.Rows[a]["数量"].ToString().Trim();
                    worksheet_Doc.Cells[a + 2, 10] = dtBAD.Rows[a]["数量"].ToString().Trim();
                    worksheet_Doc.Cells[a + 2, 11] = dtBAD.Rows[a]["毛重"].ToString().Trim();
                    worksheet_Doc.Cells[a + 2, 12] = dtBAD.Rows[a]["金额"].ToString().Trim();
                    worksheet_Doc.Cells[a + 2, 13] = strBZ;
                    worksheet_Doc.Cells[a + 2, 14] = dtBAD.Rows[a]["目的国"].ToString().Trim();
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

                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 14]).Font.Bold = true;
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtBAD.Rows.Count + 1, 14]).Font.Name = "Verdana";
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[dtBAD.Rows.Count + 1, 14]).Font.Size = 9;
                worksheet_Doc.Cells.EntireColumn.AutoFit();
            }
            myTbl.Clear();
            myTbl.Dispose();
            dtBAD.Clear();
            dtBAD.Dispose();
           
            if (String.Compare(strIEtype, "1418") == 0 || String.Compare(strIEtype, "RMB-1418") == 0 || String.Compare(strIEtype, "RMB-D") == 0)
            {               
                string strGroupID = null;
                for (int j = 0; j < dtbl.Rows.Count; j++)
                {strGroupID += "'" + dtbl.Rows[j][0].ToString().Trim() + "',";}
                strGroupID = strGroupID.Remove(strGroupID.Length - 1);
                string strSQL1 = @"SELECT [Group ID], SUM([GongDan Qty]) AS [GD QTY], SUM([Amount]) AS [Amount], [FG EHB], [Package Code] FROM (SELECT M.[Group ID], " + 
                                  "M.[GongDan Qty], CASE WHEN M.[OF Rev Amt] > 0.0 THEN M.[OF Rev Amt] ELSE CASE WHEN M.[IE Rev Amt] > 0.0 THEN M.[IE Rev Amt] ELSE " + 
                                  "M.[Selling Amount] END END AS [Amount], M.[FG EHB], B.[Package Code] FROM M_DailyBeiAnDan AS M, B_HS AS B WHERE " +
                                  "SUBSTRING(M.[FG No], 0, CHARINDEX('-', M.[FG No])) = B.[Grade] AND M.[Group ID] IN (" + strGroupID + ") AND M.[Is Generated Doc] = 'False') " +
                                  "AS XX GROUP BY [Group ID], [FG EHB], [Package Code]";
                string strSQL2 = @"SELECT M.[Group ID], M.[ESS/LINE], M.[Order No] AS [PO#], M.[FG No], M.[GongDan Qty] AS [GD QTY], M.[Batch No] AS [Lot#], " + 
                                  "CASE WHEN M.[OF Rev Amt] > 0.0 THEN M.[OF Rev Amt] ELSE CASE WHEN M.[IE Rev Amt] > 0.0 THEN M.[IE Rev Amt] ELSE M.[Selling Amount] " + 
                                  "END END AS [Amount], M.[Selling Price] AS [Price], M.[IE Type] AS [TO IE Type], M.[FG EHB], M.[GongDan No] AS [GD No], " + 
                                  "M.[CHN Name] AS [FG CH Name], B.[Package Code] FROM M_DailyBeiAnDan AS M, B_HS AS B WHERE " + 
                                  "SUBSTRING(M.[FG No], 0, CHARINDEX('-', M.[FG No])) = B.[Grade] AND M.[Group ID] IN (" + strGroupID + ") AND M.[Is Generated Doc] = 'False'";
                sqlAdp = new SqlDataAdapter(strSQL1, sqlConn);
                DataTable myTbl1 = new DataTable();
                sqlAdp.Fill(myTbl1);
                sqlAdp = new SqlDataAdapter(strSQL2, sqlConn);
                DataTable myTbl2 = new DataTable();
                sqlAdp.Fill(myTbl2);

                foreach (DataRow dr in myTbl2.Rows)
                {
                    decimal dGDQty = Convert.ToDecimal(dr["GD QTY"].ToString().Trim());
                    decimal dAmount = Convert.ToDecimal(dr["Amount"].ToString().Trim());
                    dr["Price"] = Math.Round(dAmount / dGDQty, 2);
                }
                myTbl2.AcceptChanges();

                if (myTbl1.Rows.Count > 0)
                {
                    missing = System.Reflection.Missing.Value;
                    worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);
                    worksheet_Doc.Name = "成品移库";

                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[myTbl1.Rows.Count + 1, 17]).NumberFormatLocal = "@";
                    worksheet_Doc.Cells[1, 1] = "企业内部编号";
                    worksheet_Doc.Cells[1, 2] = "仓库号/手册号";
                    worksheet_Doc.Cells[1, 3] = "收货单位十位编码";
                    worksheet_Doc.Cells[1, 4] = "收货单位名称";
                    worksheet_Doc.Cells[1, 5] = "收货仓库号/收货手册号";
                    worksheet_Doc.Cells[1, 6] = "单证类型";
                    worksheet_Doc.Cells[1, 7] = "贸易方式";
                    worksheet_Doc.Cells[1, 8] = "原始货物备件号";
                    worksheet_Doc.Cells[1, 9] = "转出数量";
                    worksheet_Doc.Cells[1, 10] = "转出计量单位";
                    worksheet_Doc.Cells[1, 11] = "数量";
                    worksheet_Doc.Cells[1, 12] = "净重";
                    worksheet_Doc.Cells[1, 13] = "毛重";
                    worksheet_Doc.Cells[1, 14] = "金额";
                    worksheet_Doc.Cells[1, 15] = "币制";
                    worksheet_Doc.Cells[1, 16] = "原产国";
                    worksheet_Doc.Cells[1, 17] = "标签";
                    for (int h = 0; h < myTbl1.Rows.Count; h++)
                    {
                        worksheet_Doc.Cells[h + 2, 1] = myTbl1.Rows[h]["Group ID"].ToString().Trim();
                        worksheet_Doc.Cells[h + 2, 2] = "C014";
                        worksheet_Doc.Cells[h + 2, 3] = "3122442019";
                        worksheet_Doc.Cells[h + 2, 4] = "上海新发展国际物流有限公司";
                        worksheet_Doc.Cells[h + 2, 5] = "1418";
                        worksheet_Doc.Cells[h + 2, 6] = "2";
                        worksheet_Doc.Cells[h + 2, 7] = "1";
                        worksheet_Doc.Cells[h + 2, 8] = myTbl1.Rows[h]["FG EHB"].ToString().Trim();
                        int iPackageCode = Convert.ToInt32(myTbl1.Rows[h]["Package Code"].ToString().Trim());
                        int iGDQTY = Convert.ToInt32(myTbl1.Rows[h]["GD QTY"].ToString().Trim());
                        if (String.Compare(strIEtype, "1418") == 0) { worksheet_Doc.Cells[h + 2, 9] = iGDQTY.ToString(); }
                        else
                        {
                            if (iPackageCode == 0) { worksheet_Doc.Cells[h + 2, 9] = "0"; }
                            else { worksheet_Doc.Cells[h + 2, 9] = Math.Round(Convert.ToDecimal(iGDQTY * 1.0M / (iPackageCode * 1.0M)), 2).ToString(); }
                        }
                        worksheet_Doc.Cells[h + 2, 10] = String.Compare(strIEtype, "1418") == 0 ? "035" : "125";
                        worksheet_Doc.Cells[h + 2, 11] = iGDQTY.ToString();
                        worksheet_Doc.Cells[h + 2, 12] = iGDQTY.ToString();
                        worksheet_Doc.Cells[h + 2, 13] = Math.Round(iGDQTY * dRatio, 2).ToString();
                        worksheet_Doc.Cells[h + 2, 14] = myTbl1.Rows[h]["Amount"].ToString().Trim();
                        worksheet_Doc.Cells[h + 2, 15] = "502";
                        worksheet_Doc.Cells[h + 2, 16] = "142";
                        worksheet_Doc.Cells[h + 2, 17] = String.Empty;
                    }
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 17]).Font.Bold = true;
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 17]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[myTbl1.Rows.Count + 1, 17]).Font.Name = "Verdana";
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[myTbl1.Rows.Count + 1, 17]).Font.Size = 9;
                    worksheet_Doc.Cells.EntireColumn.AutoFit();
                }
                myTbl1.Dispose();

                 if (myTbl2.Rows.Count > 0)
                 {
                    missing = System.Reflection.Missing.Value;
                    worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);
                    worksheet_Doc.Name = "XFZ_Report";

                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[myTbl2.Rows.Count + 1, myTbl2.Columns.Count - 2]).NumberFormatLocal = "@";
                    for (int m = 0; m < myTbl2.Rows.Count; m++)
                    {
                        for (int n = 1; n < myTbl2.Columns.Count - 1; n++)
                        { worksheet_Doc.Cells[m + 2, n] = myTbl2.Rows[m][n].ToString().Trim(); }
                    }

                    for (int k = 1; k < myTbl2.Columns.Count - 1; k++)
                    { worksheet_Doc.Cells[1, k] = myTbl2.Columns[k].ColumnName.ToString().Trim(); }

                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, myTbl2.Columns.Count - 2]).Font.Bold = true;
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, myTbl2.Columns.Count - 2]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[myTbl2.Rows.Count + 1, myTbl2.Columns.Count - 2]).Font.Name = "Verdana";
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[myTbl2.Rows.Count + 1, myTbl2.Columns.Count - 2]).Font.Size = 9;
                    worksheet_Doc.Cells.EntireColumn.AutoFit();
                }
                myTbl2.Dispose();

                if (String.Compare(strIEtype, "RMB-1418") == 0)
                {
                    sqlComm.CommandText = @"SELECT [Group ID], [FG No], [CHN Name], N'中国' AS [Original Country], [GongDan Qty] AS [Net Weight], CASE WHEN [OF Rev Amt] > 0.0 " +
                                           "THEN [OF Rev Amt] ELSE CASE WHEN [IE Rev Amt] > 0.0 THEN [IE Rev Amt] ELSE [Selling Amount] END END AS [Amount] " +
                                           "FROM M_DailyBeiAnDan WHERE [IE Type] = 'RMB-1418' AND [Is Generated Doc] = 'False'";
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                    sqlAdapter.SelectCommand = sqlComm;
                    DataTable tbl = new DataTable();
                    sqlAdapter.Fill(tbl);
                    sqlAdapter.Dispose();
                    tbl.Columns.Add("Gross Weight", typeof(decimal));
                    tbl.Columns.Add("Price", typeof(decimal));
                    tbl.Columns["Gross Weight"].SetOrdinal(5);
                    tbl.Columns["Price"].SetOrdinal(6);

                    foreach (DataRow dr in tbl.Rows)
                    {
                        dr["Gross Weight"] = Convert.ToInt32(dr["Net Weight"].ToString().Trim()) * dRatio;
                        dr["Price"] = Math.Round(Convert.ToDecimal(dr["Amount"].ToString().Trim()) / Convert.ToDecimal(dr["Net Weight"].ToString().Trim()), 2);
                    }
                    tbl.AcceptChanges();

                    SqlLib sqllib = new SqlLib();
                    string[] strName = { "FG No", "CHN Name", "Price" };
                    DataTable table1 = sqllib.SelectDistinct(tbl, strName);
                    foreach (DataRow dr in table1.Rows)
                    {
                        string strFGNo = dr["FG No"].ToString().Trim();
                        string strCHNName = dr["CHN Name"].ToString().Trim();
                        decimal dPrice = Convert.ToDecimal(dr["Price"].ToString().Trim());
                        DataRow[] row = tbl.Select("[FG No] = '" + strFGNo + "' AND [CHN Name] = '" + strCHNName + "' AND [Price] = " + dPrice + "");
                        if (row.Length > 1)
                        {
                            int iTotalNW = Convert.ToInt32(tbl.Compute("SUM([Net Weight])", "[FG No] = '" + strFGNo + "' AND [CHN Name] = '" + strCHNName + "' AND [Price] = " + dPrice + ""));
                            decimal dTotalGW = Convert.ToDecimal(tbl.Compute("SUM([Gross Weight])", "[FG No] = '" + strFGNo + "' AND [CHN Name] = '" + strCHNName + "' AND [Price] = " + dPrice + ""));
                            int iRow = row.Length, icount = 1;
                            foreach (DataRow r in row)
                            {
                                if (icount == iRow)
                                {
                                    r["Net Weight"] = iTotalNW;
                                    r["Gross Weight"] = dTotalGW;
                                    r["Amount"] = Convert.ToDecimal(r["Price"].ToString().Trim()) * iTotalNW;
                                }
                                else
                                {
                                    r.Delete();
                                    icount++;
                                }
                            }
                        }
                        else { row[0]["Amount"] = Math.Round(Convert.ToDecimal(row[0]["Price"].ToString().Trim()) * Convert.ToDecimal(row[0]["Net Weight"].ToString().Trim()), 2); }
                    }
                    table1.Dispose();
                    tbl.AcceptChanges();

                    if (tbl.Rows.Count > 0)
                    {
                        missing = System.Reflection.Missing.Value;
                        worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);
                        worksheet_Doc.Name = "RMB-1418";

                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[tbl.Rows.Count + 1, tbl.Columns.Count]).NumberFormatLocal = "@";
                        for (int m = 0; m < tbl.Rows.Count; m++)
                        {
                            for (int n = 0; n < tbl.Columns.Count; n++)
                            { worksheet_Doc.Cells[m + 2, n + 1] = tbl.Rows[m][n].ToString().Trim(); }
                        }

                        for (int k = 0; k < tbl.Columns.Count; k++)
                        { worksheet_Doc.Cells[1, k + 1] = tbl.Columns[k].ColumnName.ToString().Trim(); }

                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, tbl.Columns.Count]).Font.Bold = true;
                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[tbl.Rows.Count + 1, tbl.Columns.Count]).Font.Name = "Verdana";
                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[tbl.Rows.Count + 1, tbl.Columns.Count]).Font.Size = 9;
                        worksheet_Doc.Cells.EntireColumn.AutoFit();
                    }

                    string[] str = { "CHN Name" };
                    DataTable table2 = sqllib.SelectDistinct(tbl, str);
                    sqllib.Dispose(0);
                    table2.Columns.Add("Net Weight", typeof(Int32));
                    table2.Columns.Add("Amount", typeof(Decimal));
                    foreach (DataRow dr in table2.Rows)
                    {
                        string strCHNName = dr["CHN Name"].ToString().Trim();
                        dr["Net Weight"] = Convert.ToInt32(tbl.Compute("SUM([Net Weight])", "[CHN Name] = '" + strCHNName + "'"));
                        dr["Amount"] = Convert.ToDecimal(tbl.Compute("SUM([Amount])", "[CHN Name] = '" + strCHNName + "'"));
                    }
                    table2.AcceptChanges();
                    tbl.Dispose();

                    if (table2.Rows.Count > 0)
                    {
                        missing = System.Reflection.Missing.Value;
                        worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);
                        worksheet_Doc.Name = "PO";

                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[table2.Rows.Count + 1, table2.Columns.Count]).NumberFormatLocal = "@";
                        for (int m = 0; m < table2.Rows.Count; m++)
                        {
                            for (int n = 0; n < table2.Columns.Count; n++)
                            { worksheet_Doc.Cells[m + 2, n + 1] = table2.Rows[m][n].ToString().Trim(); }
                        }

                        for (int k = 0; k < table2.Columns.Count; k++)
                        { worksheet_Doc.Cells[1, k + 1] = table2.Columns[k].ColumnName.ToString().Trim(); }

                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, table2.Columns.Count]).Font.Bold = true;
                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[table2.Rows.Count + 1, table2.Columns.Count]).Font.Name = "Verdana";
                        worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[table2.Rows.Count + 1, table2.Columns.Count]).Font.Size = 9;
                        worksheet_Doc.Cells.EntireColumn.AutoFit();
                    }
                    table2.Dispose();
                }         
            }
            sqlAdp.Dispose();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            dtbl.Dispose();         
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
            if (this.dgvBeiAnDanDoc.RowCount > 0 && 
                this.cmbGroupID.Enabled == true)
            {
                SqlLib SqlLib = new SqlLib();
                string[] strFile = { "Group ID" };
                DataTable dTable = SqlLib.SelectDistinct(dtName, strFile);
                DataRow dr = dTable.NewRow();
                dr["Group ID"] = String.Empty;
                dTable.Rows.InsertAt(dr, 0);
                this.cmbGroupID.DisplayMember = this.cmbGroupID.ValueMember = "Group ID";
                this.cmbGroupID.DataSource = dTable;
                SqlLib.Dispose(0);
            }
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDanDoc.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDanDoc.Focus();
                return;
            }

            string strGroupID = this.cmbGroupID.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strGroupID))
            {
                MessageBox.Show("Please select Group ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbGroupID.Focus();
                return;
            }

            string strBeiAnDanID = this.txtBeiAnDanID.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strBeiAnDanID))
            {
                MessageBox.Show("Please input BeiAnDan ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtBeiAnDanID.Focus();
                return;
            }

            if (this.txtBeiAnDanNo.Enabled == true && 
                String.IsNullOrEmpty(this.txtBeiAnDanNo.Text.Trim()))
            {
                MessageBox.Show("Please input BeiAnDan No first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtBeiAnDanNo.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = @"SELECT count(*) FROM C_BeiAnDan WHERE [BeiAnDan ID] = '" + strBeiAnDanID + "'";
            int iCount = Convert.ToInt32(sqlComm.ExecuteScalar());
            if (iCount > 0)
            {
                MessageBox.Show("The BeiAnDan ID(" + strBeiAnDanID + ") already existed, please input a new one.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                sqlComm.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
                return;
            }
            sqlComm.CommandType = CommandType.StoredProcedure;

            string strIEtype = this.cmbIEtype.Text.Trim().ToUpper();                      
            string strBeiAnDanNo = null;
            if (this.txtBeiAnDanNo.Enabled == false) { strBeiAnDanNo = String.Empty; }
            else { strBeiAnDanNo = this.txtBeiAnDanNo.Text.Trim().ToUpper(); }
            DateTime dtNow = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm"));
            DataView dv = dtName.DefaultView;
            dv.RowFilter = "[Group ID] = '" + strGroupID + "'";
            
            sqlComm.CommandText = @"usp_InsertBeiAnDanDoc";
            sqlComm.Parameters.AddWithValue("@GroupID", strGroupID);
            sqlComm.Parameters.AddWithValue("@BeiAnDanNo", strBeiAnDanNo);
            sqlComm.Parameters.AddWithValue("@CreatedDate", dtNow);
            sqlComm.Parameters.AddWithValue("@TVP_Doc", dv.ToTable());
            sqlComm.ExecuteNonQuery();
            sqlComm.Parameters.Clear();
            dv.RowFilter = string.Empty;

            sqlComm.CommandText = @"usp_InsertBeiAnDan";
            sqlComm.Parameters.AddWithValue("@IEType", strIEtype);
            sqlComm.Parameters.AddWithValue("@GroupID", strGroupID);
            sqlComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
            sqlComm.Parameters.AddWithValue("@BeiAnDanNo", strBeiAnDanNo);
            sqlComm.Parameters.AddWithValue("@BeiAnDanDate", dtNow);
            sqlComm.Parameters.AddWithValue("@Creater", loginFrm.PublicUserName);
            sqlComm.ExecuteNonQuery();
            sqlComm.Parameters.Clear();

            DataRow[] dRow = dtName.Select("[Group ID] = '" + strGroupID + "'");
            foreach (DataRow dr in dRow) { dr.Delete(); }
            dtName.AcceptChanges();          
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }

            if (dtName.Rows.Count == 0) { this.dgvBeiAnDanDoc.DataSource = DBNull.Value; }
            this.cmbGroupID.Text = String.Empty;
            this.txtBeiAnDanID.Text = String.Empty;
            if (this.txtBeiAnDanNo.Enabled == true) { txtBeiAnDanNo.Text = String.Empty; }          
            MessageBox.Show("Successfully approve.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDanDoc.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnStatus.Focus();
                return;
            }
            string strStatus = null;
            DialogResult dlgR = MessageBox.Show("Do you want to update the BeiAnDan status or revoke the BeiAnDan status?\n\t[Yes]: update;  [No]: revoke;  [Cancel]: reject", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgR == DialogResult.Cancel) { return; }
            else { strStatus = dlgR.ToString(); }

            string strObject = null;
            bool bStatus = false;
            if (String.Compare(this.cmbIEtype.Text.Trim(), "EXPORT") == 0) { strObject = "EXPORT"; }
            else { strObject = "OTHER"; bStatus = true; }
            if (strStatus == "Yes") { bStatus = true; }
            else { bStatus = false; }
            string strNow = System.DateTime.Now.ToString("M/d/yyyy");

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_UpdateBeiAnDanStatus";
            sqlComm.Parameters.AddWithValue("@Status", bStatus);
            sqlComm.Parameters.AddWithValue("@Object", strObject);
            sqlComm.Parameters.AddWithValue("@GeneratedDocDate", strNow);
            
            string strGroupID = this.cmbGroupID.Text.Trim();
            if (!String.IsNullOrEmpty(strGroupID))
            {
                DataRow[] drow = dtName.Select("[Group ID] = '" + strGroupID.ToUpper() + "'");
                foreach (DataRow dr in drow)
                {
                    dr["Is Generated Doc"] = bStatus;
                    dr["Generated Doc Date"] = strNow;
                }
                dtName.AcceptChanges();
              
                sqlComm.Parameters.AddWithValue("@GroupID", strGroupID);               
                sqlComm.Parameters.AddWithValue("@Judge", "FALSE");
            }
            else
            {
                string[] strColumn = { "Group ID", "Is Generated Doc" };
                SqlLib SQLLIB = new SqlLib();
                DataTable dtGroupID = SQLLIB.SelectDistinct(dtName, strColumn);
                SQLLIB.Dispose(0);
                DataRow[] datarow;
                if (strStatus == "Yes") { datarow = dtGroupID.Select("[Is Generated Doc] = 'False'"); }
                else { datarow = dtGroupID.Select("[Is Generated Doc] = 'True'"); }
                string strGroupId = null;
                foreach (DataRow dr in datarow)
                { strGroupId += "'" + dr["Group ID"].ToString().Trim() + "',"; }
                strGroupId = strGroupId.Remove(strGroupId.Length - 1);

                DataRow[] drow = dtName.Select("[Group ID] IN (" + strGroupId + ")");
                foreach (DataRow row in drow)
                {
                    row["Is Generated Doc"] = bStatus;
                    row["Generated Doc Date"] = strNow;
                }
                dtName.AcceptChanges();
                dtGroupID.Dispose();

                sqlComm.Parameters.AddWithValue("@GroupID", strGroupId);
                sqlComm.Parameters.AddWithValue("@Judge", "TRUE");                
            }
            sqlComm.ExecuteNonQuery();
            sqlComm.Parameters.Clear();
            MessageBox.Show("Done.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDanDoc.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDanDoc.Focus();
                return;
            }

            if (String.IsNullOrEmpty(this.cmbGroupID.Text.Trim()))
            {
                MessageBox.Show("Please select Group ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbGroupID.Focus();
                return;
            }

            if (String.Compare(this.cmbIEtype.Text.Trim().ToUpper(), "RMB") == 0)
            {
                string strGroupID = this.cmbGroupID.Text.Trim().ToUpper();
                SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;

                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = @"usp_BeiAnDan_BOMReport";
                sqlComm.Parameters.AddWithValue("@GroupID", strGroupID);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlComm;
                DataTable dtBOMReport = new DataTable();
                sqlAdapter.Fill(dtBOMReport);

                dtBOMReport.Columns.Add("项号", typeof(Int32));
                dtBOMReport.Columns.Add("耗用料件", typeof(decimal));
                dtBOMReport.Columns["项号"].SetOrdinal(2);
                dtBOMReport.Columns["耗用料件"].SetOrdinal(10);

                string strFGEHB = null;
                int iLineNo = 1;
                foreach (DataRow dr in dtBOMReport.Rows)
                {
                    string strEHB = dr[0].ToString().Trim();
                    if (String.Compare(strFGEHB, strEHB) != 0)
                    {
                        strFGEHB = strEHB;                       
                        iLineNo = 1;
                    }
                    else { iLineNo++; }
                    dr[2] = iLineNo;

                    decimal dFGQty = Convert.ToDecimal(double.Parse(dr[9].ToString().Trim()));
                    decimal dConsumption = Convert.ToDecimal(double.Parse(dr[5].ToString().Trim()));
                    decimal dQtyLossRate = Convert.ToDecimal(double.Parse(dr[6].ToString().Trim()));
                    dr[10] = Math.Round(dFGQty * dConsumption / (1 - dQtyLossRate / 100), 6);
                }
                dtBOMReport.AcceptChanges();

                sqlComm.CommandType = CommandType.Text;
                sqlComm.CommandText = @"SELECT [Object], [Rate] FROM B_ExchangeRate";
                sqlAdapter.SelectCommand = sqlComm;
                DataTable dtRate = new DataTable();
                sqlAdapter.Fill(dtRate);
                DataRow[] drRMB = dtRate.Select("[Object] = 'RMB:USD'");
                decimal dRMBRate = 0.0M;
                if (drRMB.Length == 1) { dRMBRate = Convert.ToDecimal(drRMB[0][1].ToString().Trim()); }

                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = @"usp_BeiAnDan_BGDReport";
                sqlAdapter.SelectCommand = sqlComm;
                DataTable dtBGDReport = new DataTable();
                sqlAdapter.Fill(dtBGDReport);

                DataRow[] dRow1 = dtBGDReport.Select("[RM Currency] <> 'USD'");
                if (dRow1.Length > 0)
                {
                    foreach (DataRow dr in dRow1)
                    {
                        string strObj = dr["RM Currency"].ToString().Trim().ToUpper() + ":USD";
                        DataRow[] rows = dtRate.Select("[Object] = '" + strObj + "'");
                        if (rows.Length > 0)
                        {
                            decimal dRate = Convert.ToDecimal(float.Parse(rows[0][1].ToString().Trim()));
                            decimal dPrice = Convert.ToDecimal(dr["单价"].ToString().Trim());
                            dr["单价"] = Math.Round(dPrice * dRate, 4);
                        }
                        else { dr["单价"] = 0.0M; } 
                    }
                    dtBGDReport.AcceptChanges();
                }
                dtBGDReport.Columns.Remove("RM Currency");

                sqlComm.CommandText = @"usp_BeiAnDan_InvoiceUSDReport_Mid";
                sqlAdapter.SelectCommand = sqlComm;
                DataTable dtSort1 = new DataTable();
                sqlAdapter.Fill(dtSort1);

                DataTable dtSort2 = new DataTable();
                SqlLib sqlLib = new SqlLib();
                string[] str1 = { "GongDan No" };
                if (sqlLib.SelectDistinct(dtSort1, str1).Rows.Count > 0)
                {
                    dtSort2.Columns.Add("RM Customs Code", typeof(String));
                    dtSort2.Columns.Add("Original Country", typeof(String));
                    foreach (DataRow dr in dtSort1.Rows)
                    {
                        string strRMCustomsCode = dr["RM Customs Code"].ToString().Trim();
                        string strOriginalCountry = dr["Original Country"].ToString().Trim();
                        DataRow[] rows = dtSort2.Select("[RM Customs Code] = '" + strRMCustomsCode + "' AND [Original Country] = '" + strOriginalCountry + "'");
                        if (rows.Length == 0)
                        {
                            DataRow row = dtSort2.NewRow();
                            row[0] = strRMCustomsCode;
                            row[1] = strOriginalCountry;
                            dtSort2.Rows.Add(row);
                        }
                    }
                }
                dtSort2.AcceptChanges();
                dtSort1.Dispose();

                sqlComm.CommandText = @"usp_BeiAnDan_InvoiceUSDReport";
                sqlAdapter.SelectCommand = sqlComm;
                DataTable dtInvUSD = new DataTable();
                sqlAdapter.Fill(dtInvUSD);

                DataRow[] dRow2 = dtInvUSD.Select("[RM Currency] <> 'USD'");
                if (dRow2.Length > 0)
                {
                    foreach (DataRow dr in dRow2)
                    {
                        string strObj = dr["RM Currency"].ToString().Trim().ToUpper() + ":USD";
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

                string[] strAppFG1 = { "成品中文名称"};
                DataTable dtAppFG1 = sqlLib.SelectDistinct(dtBOMReport, strAppFG1);
                int iAppFG1 = dtAppFG1.Rows.Count;
                string[] strAppFG2 = { "成品中文名称", "成品数量", "成品备件号" };
                DataTable dtAppFG2 = sqlLib.SelectDistinct(dtBOMReport, strAppFG2);
                string[] strAppDrools = { "Drools CH Name" };
                DataTable dtAppDrools = sqlLib.SelectDistinct(dtBOMReport, strAppDrools);
                int iAppDrools = dtAppDrools.Rows.Count;
                string[] strColumns = { "EHB", "Original Country" };
                DataTable dtInvRpt1 = sqlLib.SelectDistinct(dtInvUSD, strColumns);

                foreach (DataRow dr in dtInvRpt1.Rows)
                {
                    DataRow[] rows = dtInvUSD.Select("[EHB] = '" + dr[0].ToString().Trim() + "' AND [Original Country] = '" + dr[1].ToString().Trim() + "' AND [Unit Price(USD/KG)] <> 0.0");
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
                            else
                            {
                                row.Delete();
                                iCount++;
                            }
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
                    DataRow[] row = dtInvUSD.Select("[EHB] = '" + strRMCustomsCode + "' AND [Original Country] = '" + strOriginalCountry + "'");
                    if (row.Length > 0)
                    {
                        DataRow dr = dtSort3.NewRow();
                        dr["EHB"] = row[0]["EHB"].ToString().Trim();
                        dr["Name"] = row[0]["Name"].ToString().Trim();
                        dr["Description"] = row[0]["Description"].ToString().Trim();
                        dr["Original Country"] = row[0]["Original Country"].ToString().Trim();
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

                sqlComm.CommandText = @"usp_BeiAnDan_InvoiceRMBReport";
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
                                if (String.Compare(row["RM Currency"].ToString().Trim().ToUpper(), "RMB") == 0)
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
                            else
                            {
                                row.Delete();
                                iCount++;
                            }
                        }
                    }
                    else
                    {
                        decimal dNetWeight = Convert.ToDecimal(rows[0]["NW(KG)"].ToString().Trim());
                        decimal dUnitPrice = 0.0M;
                        if (!String.IsNullOrEmpty(rows[0]["Unit Price(USD/KG)"].ToString().Trim()))
                        {
                            if (String.Compare(rows[0]["RM Currency"].ToString().Trim().ToUpper(), "RMB") == 0)
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

                sqlComm.CommandText = @"usp_BeiAnDan_GongDanInfoReport";
                sqlAdapter.SelectCommand = sqlComm;
                DataTable dtGDInfo = new DataTable();
                sqlAdapter.Fill(dtGDInfo);
                sqlAdapter.Dispose();
                sqlComm.Parameters.RemoveAt("@GroupID");

                Microsoft.Office.Interop.Excel.Application excel_rpt = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks workbooks_rpt = excel_rpt.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook_rpt = workbooks_rpt.Add(true);
                Microsoft.Office.Interop.Excel.Worksheet worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets[1];
                worksheet_rpt.Name = this.cmbGroupID.Text.Trim().ToUpper().Replace("-", String.Empty);

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBOMReport.Rows.Count + 1, 9]).NumberFormatLocal = "@";
                for (int x = 0; x < dtBOMReport.Rows.Count; x++)
                {
                    for (int y = 0; y < dtBOMReport.Columns.Count - 3; y++)
                    { worksheet_rpt.Cells[x + 2, y + 1] = dtBOMReport.Rows[x][y].ToString().Trim(); }
                    //avoid to generate the digit that the format is scientific notation for 'RM Used Qty' & 'Drools Quota' columns
                    worksheet_rpt.Cells[x + 2, dtBOMReport.Columns.Count - 2] = Math.Round(double.Parse(dtBOMReport.Rows[x][dtBOMReport.Columns.Count - 3].ToString().Trim()), 6);
                    worksheet_rpt.Cells[x + 2, dtBOMReport.Columns.Count - 1] = Math.Round(double.Parse(dtBOMReport.Rows[x][dtBOMReport.Columns.Count - 2].ToString().Trim()), 6);
                }

                for (int z = 0; z < dtBOMReport.Columns.Count - 1; z++)
                { worksheet_rpt.Cells[1, z + 1] = dtBOMReport.Columns[z].ColumnName.Trim().ToUpper(); }

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBOMReport.Columns.Count - 1]).Font.Bold = true;
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBOMReport.Columns.Count - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBOMReport.Rows.Count + 1, dtBOMReport.Columns.Count - 1]).Font.Name = "Verdana";
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBOMReport.Rows.Count + 1, dtBOMReport.Columns.Count - 1]).Font.Size = 9;
                worksheet_rpt.Cells.EntireColumn.AutoFit();

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
                    string strTotalDroolsQty = dtBOMReport.Compute("SUM([边角料])", "[Drools CH Name] = '" + strDroolsName + "'").ToString().Trim();
                    decimal dTotalDroolsQty = Math.Round(Convert.ToDecimal(strTotalDroolsQty), 2);
                    worksheet_rpt.Cells[iAppFG1 + 4 + j, 5] = dTotalDroolsQty.ToString() + " KG";
                    worksheet_rpt.Cells[iAppFG1 + 4 + j, 3] = Convert.ToString(Math.Ceiling(dTotalDroolsQty / 250)) + " 托";
                }

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[iAppFG1 + iAppDrools + 3, 5]).Font.Name = "Verdana";
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[iAppFG1 + iAppDrools + 3, 5]).Font.Size = 9;
                worksheet_rpt.Cells.EntireColumn.AutoFit();
                dtAppFG1.Dispose();
                dtAppFG2.Dispose();
                dtAppDrools.Dispose();
                dtBOMReport.Dispose();

                missing = System.Reflection.Missing.Value;
                worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
                worksheet_rpt.Name = "BGD Report";

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBGDReport.Rows.Count + 1, 2]).NumberFormatLocal = "@";
                for (int a = 0; a < dtBGDReport.Rows.Count; a++)
                {
                    for (int b = 0; b < dtBGDReport.Columns.Count; b++)
                    { worksheet_rpt.Cells[a + 2, b + 1] = dtBGDReport.Rows[a][b].ToString().Trim(); }
                }
                for (int c = 0; c < dtBGDReport.Columns.Count; c++)
                { worksheet_rpt.Cells[1, c + 1] = dtBGDReport.Columns[c].ColumnName.Trim(); }

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBGDReport.Columns.Count]).Font.Bold = true;
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtBGDReport.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBGDReport.Rows.Count + 1, dtBGDReport.Columns.Count]).Font.Name = "Verdana";
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtBGDReport.Rows.Count + 1, dtBGDReport.Columns.Count]).Font.Size = 9;
                worksheet_rpt.Cells.EntireColumn.AutoFit();
                dtBGDReport.Dispose();

                missing = System.Reflection.Missing.Value;
                worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
                worksheet_rpt.Name = "Invoice_USD Report";

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, dtInvUSD.Columns.Count]).NumberFormatLocal = "@";
                for (int f = 0; f < dtInvUSD.Rows.Count; f++)
                {
                    for (int g = 0; g < dtInvUSD.Columns.Count; g++)
                    { worksheet_rpt.Cells[f + 2, g + 1] = dtInvUSD.Rows[f][g].ToString().Trim(); }
                }

                for (int h = 0; h < dtInvUSD.Columns.Count; h++)
                { worksheet_rpt.Cells[1, h + 1] = dtInvUSD.Columns[h].ColumnName.Trim(); }

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtInvUSD.Columns.Count]).Font.Bold = true;
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, dtInvUSD.Columns.Count]).Font.Name = "Verdana";
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, dtInvUSD.Columns.Count]).Font.Size = 9;
                worksheet_rpt.Cells.EntireColumn.AutoFit();
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 3], worksheet_rpt.Cells[dtInvUSD.Rows.Count + 1, 3]).ColumnWidth = 50;
                dtInvUSD.Dispose();

                missing = System.Reflection.Missing.Value;
                worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
                worksheet_rpt.Name = "Invoice_RMB Report";

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvRMB.Rows.Count + 1, dtInvRMB.Columns.Count]).NumberFormatLocal = "@";
                for (int k = 0; k < dtInvRMB.Rows.Count; k++)
                {
                    for (int p = 0; p < dtInvRMB.Columns.Count; p++)
                    { worksheet_rpt.Cells[k + 2, p + 1] = dtInvRMB.Rows[k][p].ToString().Trim(); }
                }

                for (int q = 0; q < dtInvRMB.Columns.Count; q++)
                { worksheet_rpt.Cells[1, q + 1] = dtInvRMB.Columns[q].ColumnName.Trim(); }

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtInvRMB.Columns.Count]).Font.Bold = true;
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvRMB.Rows.Count + 1, dtInvRMB.Columns.Count]).Font.Name = "Verdana";
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtInvRMB.Rows.Count + 1, dtInvRMB.Columns.Count]).Font.Size = 9;
                worksheet_rpt.Cells.EntireColumn.AutoFit();
                dtInvRMB.Dispose();

                missing = System.Reflection.Missing.Value;
                worksheet_rpt = (Microsoft.Office.Interop.Excel.Worksheet)workbook_rpt.Worksheets.Add(missing, missing, missing, missing);
                worksheet_rpt.Name = "GD_Information Report";

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtGDInfo.Rows.Count + 1, dtGDInfo.Columns.Count]).NumberFormatLocal = "@";
                for (int r = 0; r < dtGDInfo.Rows.Count; r++)
                {
                    for (int s = 0; s < dtGDInfo.Columns.Count; s++)
                    { worksheet_rpt.Cells[r + 2, s + 1] = dtGDInfo.Rows[r][s].ToString().Trim(); }
                }

                for (int t = 0; t < dtGDInfo.Columns.Count; t++)
                { worksheet_rpt.Cells[1, t + 1] = dtGDInfo.Columns[t].ColumnName.Trim(); }

                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtGDInfo.Columns.Count]).Font.Bold = true;
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[1, dtGDInfo.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtGDInfo.Rows.Count + 1, dtGDInfo.Columns.Count]).Font.Name = "Verdana";
                worksheet_rpt.get_Range(worksheet_rpt.Cells[1, 1], worksheet_rpt.Cells[dtGDInfo.Rows.Count + 1, dtGDInfo.Columns.Count]).Font.Size = 9;
                worksheet_rpt.Cells.EntireColumn.AutoFit();
                dtGDInfo.Dispose();

                excel_rpt.Visible = true;

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_rpt);
                worksheet_rpt = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_rpt);
                excel_rpt = null;
            }
        }
    }
}
