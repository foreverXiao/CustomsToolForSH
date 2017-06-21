using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class GetCustomsGongdanForm : Form
    {
        DataTable dTble = new DataTable();
        DataTable sqlTable = new DataTable();
        private LoginForm loginFrm = new LoginForm();

        private static GetCustomsGongdanForm getCustomsGongdanFrm;
        public GetCustomsGongdanForm()
        {
            InitializeComponent();
        }     
        public static GetCustomsGongdanForm CreateInstance()
        {
            if (getCustomsGongdanFrm == null || getCustomsGongdanFrm.IsDisposed)
            {
                getCustomsGongdanFrm = new GetCustomsGongdanForm();
            }
            return getCustomsGongdanFrm;
        }

        private void GetCustomsGongdanForm_Load(object sender, EventArgs e)
        {
            this.dtpApprovedDate.CustomFormat = " ";
        }

        private void GetCustomsGongdanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dTble.Dispose();
            sqlTable.Dispose();
        }

        private void dtpApprovedDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpApprovedDate.CustomFormat = "M/dd/yyyy HH:mm";
        }

        private void dtpApprovedDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpApprovedDate.CustomFormat = " "; }
        }

        private void btnGongDan_Click(object sender, EventArgs e)
        {
            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }

            string strSQL1 = @"SELECT G.[GongDan No] AS [工单号], N'加工' AS [生产类型], G.[FG No] + '/' + G.[Batch No] AS [成品备件号], G.[GongDan Qty] AS [工单数量], " +
                              "1 AS [项号], G.[RM Customs Code] AS [物料备件号], R.[Original Country] AS [原产国], 0 AS [美元金额], SUM(CAST(G.[RM Used Qty] AS decimal(18,6))) " +
                              "AS [物料耗用数量], G.[BGD No] AS [批次号], G.[BOM In Customs] FROM M_DailyGongDan AS G LEFT OUTER JOIN C_RMPurchase AS R ON " +
                              "G.[Item No] = R.[Item No] AND G.[Lot No] = R.[Lot No] WHERE G.[RM Used Qty] > 0.0 GROUP BY G.[GongDan No], G.[FG No] + '/' + [Batch No], " +
                              "[GongDan Qty], G.[RM Customs Code], R.[Original Country], G.[BGD No], G.[BOM In Customs] ORDER BY G.[GongDan No], G.[RM Customs Code], " +
                              "R.[Original Country], G.[BGD No]";
            string strSQL2 = @"SELECT DISTINCT [GongDan No] AS [工单号] FROM M_DailyGongDan";
            SqlDataAdapter SqlAdapter = new SqlDataAdapter(strSQL1, SqlConn);
            sqlTable.Clear();
            SqlAdapter.Fill(sqlTable);
            SqlAdapter.Dispose();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(strSQL2, SqlConn);
            DataTable sqlTbl = new DataTable();
            sqlTbl.Clear();
            sqlAdp.Fill(sqlTbl);
            sqlAdp.Dispose();

            int iLineNo = 0;
            string strGongDan = null;
            for (int i = 0; i < sqlTable.Rows.Count; i++)
            {
                if (String.Compare(sqlTable.Rows[i]["工单号"].ToString().Trim(), strGongDan) == 0) { iLineNo++; }
                else
                {
                    strGongDan = sqlTable.Rows[i]["工单号"].ToString().Trim();
                    iLineNo = 1;
                }
                sqlTable.Rows[i]["项号"] = iLineNo;
            }

            this.dgvGongDan.DataSource = sqlTable;
            this.dgvGongDan.Columns["美元金额"].Visible = false;
            this.dgvGDList.DataSource = sqlTbl;          
            this.dgvGDList.Columns[0].HeaderText = "全选"; 
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnDownload.Focus();
                return;
            }

            if (this.dgvGDList.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select 工单号.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnDownload.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            this.GetGongDanData(); 
         
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            excel.get_Range(excel.Cells[1, 1], excel.Cells[dTble.Rows.Count + 1, dTble.Columns.Count - 1]).NumberFormatLocal = "@";
            for (int i = 0; i < dTble.Rows.Count; i++)
            {
                for (int j = 0; j < dTble.Columns.Count - 1; j++)
                { excel.Cells[i + 2, j + 1] = dTble.Rows[i][j].ToString().Trim(); }
                if (!String.IsNullOrEmpty(dTble.Rows[i][10].ToString().Trim()))  //Mapping 'BOM In Customs'
                { excel.Cells[i + 2, 3] = dTble.Rows[i][10].ToString().Trim(); }
                excel.Cells[i + 2, 9] = String.Format("{0:0.000000}", Convert.ToDecimal(dTble.Rows[i][8].ToString().Trim()));
            }

            for (int k = 0; k < dTble.Columns.Count - 1; k++)
            { excel.Cells[1, k + 1] = dTble.Columns[k].ColumnName.ToString().Trim(); }

            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, dTble.Columns.Count - 1]).Font.Bold = true;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, dTble.Columns.Count - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            excel.get_Range(excel.Cells[1, 1], excel.Cells[dTble.Rows.Count + 1, dTble.Columns.Count - 1]).Font.Name = "Verdana";
            excel.get_Range(excel.Cells[1, 1], excel.Cells[dTble.Rows.Count + 1, dTble.Columns.Count - 1]).Font.Size = 9;
            excel.Cells.EntireColumn.AutoFit();
            excel.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnApprove.Focus();
                return;
            }           

            if (this.dgvGDList.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select 工单号.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvGDList.Focus();
                return;
            }

            this.GetGongDanData();
            SqlLib lib = new SqlLib();
            string[] strName = { "工单号" };
            DataTable dtGD = lib.SelectDistinct(dTble, strName);
            lib.Dispose(0);
            
            SqlConnection approvedConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (approvedConn.State == ConnectionState.Closed) { approvedConn.Open(); }
            SqlCommand approvedComm = new SqlCommand();
            approvedComm.Connection = approvedConn;
            string strGDcmp = null, strGdCmp = null;

            /*------ Monitor And Control Multiple Users ------*/
            approvedComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
            string strUserName = Convert.ToString(approvedComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    approvedComm.Dispose();
                    approvedConn.Dispose();
                    this.btnApprove.Focus();
                    return;
                }
            }
            else
            {
                approvedComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                approvedComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                approvedComm.ExecuteNonQuery();
                approvedComm.Parameters.RemoveAt("@UserName");
            }

            DateTime dtApproved;
            if (String.IsNullOrEmpty(this.dtpApprovedDate.Text.Trim())) { dtApproved = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm")); }
            else { dtApproved = Convert.ToDateTime(this.dtpApprovedDate.Value.ToString("M/d/yyyy HH:mm")); }
            for (int x = 0; x < dtGD.Rows.Count; x++)
            {
                string strGongDan = dtGD.Rows[x]["工单号"].ToString().Trim();
                approvedComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;
                approvedComm.CommandText = @"SELECT DISTINCT [Actual Start Date], [Actual Close Date], [Batch No], [BOM In Customs], [GongDan No], [FG No], [ESS/LINE], " +
                                            "[Order No], [IE Type], [Order Category], [Destination], [Total Ship Qty], [GongDan Qty], [Order Price], [Order Currency], " +
                                            "CAST([Total RM Qty] AS decimal(18,6)) AS [TotalRMQty], [Total RM Cost(USD)], CAST([Drools Rate] AS decimal(18,6)) AS " +
                                            "[DroolsRate], [CHN Name], [Created Date], '" + loginFrm.PublicUserName + "' AS [Creater], '" + dtApproved + "' AS " +
                                            "[Approved Date], [PC Item] FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";

                SqlDataAdapter approvedAdapter = new SqlDataAdapter();
                approvedAdapter.SelectCommand = approvedComm;
                DataTable approvedTable1 = new DataTable();
                approvedAdapter.Fill(approvedTable1); // Use to insert data to table C_GongDan

                approvedComm.CommandText = @"SELECT [Batch Path], [GongDan No], [Line No], [Item No], [Lot No], [Inventory Type], [RM Category], [RM Customs Code], " +
                                            "[BGD No], [RM Currency], [RM Price], [Consumption], [RM Used Qty], CAST([Drools Quota] AS decimal(18, 8)) AS [DroolsQuota], " +
                                            "[Drools EHB] FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                approvedAdapter.SelectCommand = approvedComm;
                DataTable approvedTable2 = new DataTable();
                approvedAdapter.Fill(approvedTable2);

                approvedComm.CommandText = @"SELECT G.[GongDan No] AS [工单号], N'加工' AS [生产类型], G.[FG No] + '/' + G.[Batch No] AS [成品备件号], " +
                                            "G.[GongDan Qty] AS [工单数量], ROW_NUMBER() OVER (ORDER BY G.[RM Customs Code], G.[BGD No]) AS [项号], " +
                                            "G.[RM Customs Code] AS [物料备件号], CAST(SUM(G.[RM Used Qty]) AS decimal(18,6)) AS [物料耗用数量], " +
                                            "R.[Original Country] AS [原产国], G.[BGD No] AS [批次号], G.[BOM In Customs], '" + dtApproved + "' AS [Created Date] " +
                                            "FROM M_DailyGongDan AS G LEFT OUTER JOIN C_RMPurchase AS R ON G.[Item No] = R.[Item No] AND G.[Lot No] = R.[Lot No] " +
                                            "WHERE [RM Used Qty] > 0.0 GROUP BY G.[GongDan No], G.[FG No] + '/' + G.[Batch No], G.[GongDan Qty], G.[RM Customs Code], " +
                                            "R.[Original Country], G.[BGD No], G.[BOM In Customs] HAVING [GongDan No] = @GongDanNo " +
                                            "ORDER BY G.[RM Customs Code], G.[BGD No], R.[Original Country]";
                approvedAdapter.SelectCommand = approvedComm;
                DataTable approvedTable3 = new DataTable();
                approvedAdapter.Fill(approvedTable3);

                /*------ check and update 'RMB' IEType to 'RMB-1418' or 'RMB-D' ------*/
                approvedComm.CommandText = @"SELECT X.[GongDan No], X.[RM Category], B.[Is Allocated], X.[GongDan Qty] FROM (SELECT DISTINCT [GongDan No], CASE WHEN " +
                                            "CHARINDEX('-', [FG No]) > 0 THEN SUBSTRING([FG No], 1, CHARINDEX('-', [FG No]) - 1) ELSE [FG No] END AS [Grade], [RM Category], " +
                                            "[GongDan Qty] FROM M_DailyGongDan WHERE [IE Type] = 'RMB' AND [Consumption] > 0.0) AS X LEFT OUTER JOIN B_Allocation AS B " + 
                                            "ON X.[Grade] = B.[Grade] WHERE X.[GongDan No] = @GongDanNo";
                approvedAdapter.SelectCommand = approvedComm;
                DataTable approvedTable4 = new DataTable();
                approvedAdapter.Fill(approvedTable4);              

                /*------ check and update 'RM-D' IEType to 'RMB-D' ------*/
                approvedComm.CommandText = @"SELECT [GongDan No] FROM (SELECT [GongDan No], MAX([Line No]) Line, COUNT([GongDan No]) CountNo FROM M_DailyGongDan " +
                                            "WHERE [IE Type] = 'RM-D' AND [RM Category] = 'USD' AND [Consumption] > 0.0 GROUP BY [GongDan No], [IE Type] " + 
                                            "HAVING [GongDan No] = @GongDanNo) AS X WHERE Line = CountNo";
                approvedAdapter.SelectCommand = approvedComm;
                DataTable approvedTable5 = new DataTable();
                approvedAdapter.Fill(approvedTable5);
                approvedAdapter.Dispose();

                string strGDcmp1 = null;
                foreach(DataRow dr in approvedTable4.Rows)
                {
                    string strGDcmp2 = dr["GongDan No"].ToString().Trim();
                    if (String.Compare(strGDcmp1, strGDcmp2) != 0)
                    {
                        strGDcmp1 = strGDcmp2; 
                        DataRow[] drLen = approvedTable4.Select("[GongDan No] = '" + strGDcmp1 + "'");
                        if (drLen.Length == 1)
                        {
                            if (String.Compare(drLen[0]["RM Category"].ToString().Trim(), "USD") == 0)
                            {
                                string strAllocated = drLen[0]["Is Allocated"].ToString().Trim();
                                if (String.IsNullOrEmpty(strAllocated) || String.Compare(strAllocated.ToUpper(), "FALSE") == 0)
                                {
                                    DataRow drow = approvedTable1.Select("[GongDan No] = '" + strGDcmp1 + "'")[0];
                                    drow["IE Type"] = "RMB-1418";
                                    strGDcmp += strGDcmp1 + " : RMB-1418;\n";
                                }
                                else
                                {
                                    int iGongDanQty = Convert.ToInt32(drLen[0]["GongDan Qty"].ToString().Trim());
                                    DataRow drow = approvedTable1.Select("[GongDan No] = '" + strGDcmp1 + "'")[0];
                                    drow["IE Type"] = "RMB-D";
                                    strGDcmp += strGDcmp1 + " : RMB-D;\n";

                                    //------ 2016-07-14 cancel this limit option ------//
                                    //if (iGongDanQty <= 1000)  //Policy: if less than or equal to 1000, the IE Type is 'RMB-D'; else 'RMB-1418'
                                    //{
                                    //    DataRow drow = approvedTable1.Select("[GongDan No] = '" + strGDcmp1 + "'")[0];
                                    //    drow["IE Type"] = "RMB-D";
                                    //    strGDcmp += strGDcmp1 + " : RMB-D;\n";
                                    //}
                                    //else
                                    //{
                                    //    DataRow drow = approvedTable1.Select("[GongDan No] = '" + strGDcmp1 + "'")[0];
                                    //    drow["IE Type"] = "RMB-1418";
                                    //    strGDcmp += strGDcmp1 + " : RMB-1418;\n";
                                    //}
                                }
                                approvedTable1.AcceptChanges();
                            }
                        }
                    }
                }
                approvedTable4.Dispose();

                foreach(DataRow dr in approvedTable5.Rows)
                {
                    string strGDNO = dr[0].ToString().Trim();
                    DataRow[] drow = approvedTable1.Select("[GongDan No] = '" + strGDNO + "'");
                    if (drow.Length > 0)
                    {
                        foreach (DataRow datarow in drow)
                        { datarow["IE Type"] = "RMB-D"; }
                        strGdCmp += strGDNO + " : RMB-1418;\n";
                    }                 
                }
                approvedTable1.AcceptChanges();
                approvedTable5.Dispose();
                approvedComm.Parameters.Clear();

                //Update C_BOM table's column: GongDan Used Qty
                #region 
                approvedComm.CommandType = CommandType.StoredProcedure;
                approvedComm.CommandText = @"usp_UpdateGongDanUsedQty";
                approvedComm.Parameters.AddWithValue("@GongDanNo", strGongDan);
                approvedComm.Parameters.AddWithValue("@BatchNo", strGongDan.Split('-')[0].Trim());
                approvedComm.Parameters.AddWithValue("@Judge", "ADD");
                approvedComm.ExecuteNonQuery();
                approvedComm.Parameters.Clear();
                #endregion

                //Handle C_GongDan, C_GongDanDetail, E_GongDan table's data
                #region 
                approvedComm.Parameters.Clear();               
                approvedComm.CommandText = @"usp_InsertGongDan";
                approvedComm.Parameters.AddWithValue("@TVP_Master", approvedTable1); // Insert data to table C_GongDan
                approvedComm.ExecuteNonQuery();
                approvedComm.Parameters.Clear();

                approvedComm.CommandText = @"usp_InsertGongDanDetail"; // Insert data to table C_GongDanDetail
                approvedComm.Parameters.AddWithValue("@TVP_Detail", approvedTable2);
                approvedComm.ExecuteNonQuery();
                approvedComm.Parameters.Clear();

                approvedComm.CommandText = @"usp_InsertGongDanDoc";
                approvedComm.Parameters.AddWithValue("@TVP_Doc", approvedTable3);// Insert data to table E_GongDan
                approvedComm.ExecuteNonQuery();
                approvedComm.Parameters.Clear();
                #endregion
                approvedTable1.Dispose();
                approvedTable2.Dispose();
                approvedTable3.Dispose();

                //Update C_RMBalance table's column: Available RM Balance, GongDan Pending
                #region 
                approvedComm.CommandText = @"usp_UpdateRMBalanceByGongDan";
                approvedComm.Parameters.AddWithValue("@GongDanNo", strGongDan);
                approvedComm.Parameters.AddWithValue("@Action", "ADD");
                approvedComm.ExecuteNonQuery();
                approvedComm.Parameters.Clear();
                #endregion
             
                approvedComm.CommandType = CommandType.Text;
                approvedComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;
                approvedComm.CommandText = @"DELETE FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                SqlTransaction approvedTran = approvedConn.BeginTransaction();
                approvedComm.Transaction = approvedTran;
                try
                {
                    approvedComm.ExecuteNonQuery();
                    approvedTran.Commit();
                }
                catch (Exception)
                {
                    approvedTran.Rollback();
                    approvedTran.Dispose();
                    try
                    {
                        approvedComm.CommandText = @"DELETE FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                        approvedComm.ExecuteNonQuery();
                        approvedComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                        approvedComm.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                }
                finally { approvedComm.Parameters.Clear(); }                 
            }
            dtGD.Dispose();

            /*------ once the RM EHB is 100159, referred to the FG for anti-dump tax, IE Type should be 'RMB-D' ------*/
            //approvedComm.CommandText = @"UPDATE M_DailyGongDan SET [IE Type] = 'RMB-D' WHERE [GongDan No] IN (SELECT DISTINCT [GongDan No] FROM M_DailyGongDan WHERE [RM Customs Code] = '100159')";
            //approvedComm.ExecuteNonQuery();
            approvedComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
            approvedComm.ExecuteNonQuery();
            approvedComm.Dispose();
            if (approvedConn.State == ConnectionState.Open)
            {
                approvedConn.Close();
                approvedConn.Dispose();
            }

            string str = String.IsNullOrEmpty(strGDcmp) ? string.Empty : "\nBelow GongDan's IE Type is RMB into RMB-1418 or RMB-D.\n" + strGDcmp;
            str += String.IsNullOrEmpty(strGdCmp) ? string.Empty : "\nBelow GongDan's IE Type is RM-D into RMB-D.\n" + strGdCmp;
            if (MessageBox.Show("Successfully approve." + str, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                dTble.Clear();
                this.btnGongDan_Click(sender, e); 
            }
        }

        private void GetGongDanData()
        {
            DataTable myTable = new DataTable();          
            dTble.Columns.Clear();
            dTble.Rows.Clear();
            dTble.Columns.Add("GD", typeof(string));
            if (this.dgvGDList.Columns[0].HeaderText == "反选")
            {
                for (int x = 0; x < this.dgvGDList.RowCount; x++)
                {
                    if (String.Compare(this.dgvGDList[0, x].EditedFormattedValue.ToString(), "False") == 0)
                    {
                        DataRow dr = dTble.NewRow();
                        dr["GD"] = this.dgvGDList[1, x].Value.ToString().Trim();
                        dTble.Rows.Add(dr);
                    }
                }

                myTable.Clear();
                myTable = sqlTable.Copy();
                for (int y = 0; y < dTble.Rows.Count; y++)
                {
                    int iRow = myTable.Rows.Count;
                    for (int z = 0; z < iRow; z++)
                    {
                        if (String.Compare(dTble.Rows[y][0].ToString(), myTable.Rows[z]["工单号"].ToString()) == 0)
                        {
                            myTable.Rows.RemoveAt(z);
                            iRow--;
                            z--;
                        }
                    }
                }
            }

            if (this.dgvGDList.Columns[0].HeaderText == "取消全选") 
            {
                myTable.Clear();
                myTable = sqlTable.Copy(); 
            }

            dTble.Clear();
            dTble = myTable.Copy();
            myTable.Clear();
            myTable.Dispose();
        }

        private void dgvGDList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvGDList.RowCount; i++) { this.dgvGDList[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvGDList.RowCount; i++) { this.dgvGDList[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvGDList.RowCount; i++)
                    {
                        if (String.Compare(this.dgvGDList[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvGDList[0, i].Value = true; }

                        else { this.dgvGDList[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvGDList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvGDList.RowCount == 0) { return; }
            if (this.dgvGDList.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvGDList.RowCount; i++)
                {
                    if (String.Compare(this.dgvGDList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvGDList.RowCount && iCount > 0)
                { this.dgvGDList.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvGDList.RowCount)
                { this.dgvGDList.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvGDList.Columns[0].HeaderText = "全选"; }
            }
        }
    }
}
