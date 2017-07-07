using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class GetGongDanDataForm : Form
    {
        private DataTable dtGongDan = new DataTable();
        private DataTable dtExRate = new DataTable();
        private LoginForm loginFrm = new LoginForm();
        protected DataView dvGongDan = new DataView();
        protected PopUpFilterForm filterFrm = null;
        string strFilter = null;
        bool bSwitch = false;

        private static GetGongDanDataForm getGongDanDataFrm;
        public GetGongDanDataForm() { InitializeComponent(); }
        public static GetGongDanDataForm CreateInstance()
        {
            if (getGongDanDataFrm == null || getGongDanDataFrm.IsDisposed) { getGongDanDataFrm = new GetGongDanDataForm(); }
            return getGongDanDataFrm;
        }

        private void GetGongDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtGongDan.Dispose();
            dtExRate.Dispose();
        }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvGongDan.RowFilter = "";
            this.dgvGongDanData.DataSource = DBNull.Value;

            SqlConnection gdConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gdConn.State == ConnectionState.Closed) { gdConn.Open(); }
            SqlCommand gdComm = new SqlCommand();
            gdComm.Connection = gdConn;
            gdComm.CommandText = @"SELECT COUNT(*) FROM M_DailyGongDanList WHERE [GD Pending] = 0";
            int iCount = Convert.ToInt32(gdComm.ExecuteScalar());
            if (iCount == 0)
            {
                MessageBox.Show("There is no data in GongDan list.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                gdComm.Dispose();
                gdConn.Dispose();
                return;
            }           
            SqlDataAdapter gdAdapter = new SqlDataAdapter("SELECT * FROM V_QueryGongDan", gdConn);
            dtGongDan.Reset();
            gdAdapter.Fill(dtGongDan);
            gdAdapter = new SqlDataAdapter("SELECT DISTINCT [GongDan No] FROM M_DailyGongDan", gdConn);
            DataTable dtGDL = new DataTable();
            gdAdapter.Fill(dtGDL);
            // if gongdan is in both table M_DailyGongDan (dtGDL) and M_DailyGongDanList (dtGongDan), delete gongDan record in table M_DailyGongDan (dtGDL)
            if (dtGDL.Rows.Count > 0) 
            {
                string strGongDanList = null;
                foreach (DataRow row in dtGDL.Rows)
                {
                    string strGongDanNo = row[0].ToString().Trim();
                    DataRow[] dr = dtGongDan.Select("[GongDan No] = '" + strGongDanNo + "'");
                    if (dr.Length > 0) { strGongDanList += "'" + strGongDanNo + "',"; }
                }
                if (!String.IsNullOrEmpty(strGongDanList))
                {
                    strGongDanList = strGongDanList.Remove(strGongDanList.Length - 1);
                    gdComm.CommandText = "DELETE FROM M_DailyGongDan WHERE [GongDan No] IN (" + strGongDanList + ")";
                    gdComm.ExecuteNonQuery();
                }
            }
            dtGDL.Dispose();
            dtGongDan.Columns.Add("Total RM Cost(USD)", typeof(decimal));
            dtGongDan.Columns["Total RM Cost(USD)"].DefaultValue = 0.0M;
            dtGongDan.Columns["Total RM Cost(USD)"].SetOrdinal(20);
            dtGongDan.Columns.Add("RM Used Qty", typeof(decimal));
            dtGongDan.Columns["RM Used Qty"].DefaultValue = 0.0M;
            dtGongDan.Columns["RM Used Qty"].SetOrdinal(21);
            dtGongDan.Columns.Add("Drools Quota", typeof(decimal));
            dtGongDan.Columns["Drools Quota"].DefaultValue = 0.0M;
            dtGongDan.Columns["Drools Quota"].SetOrdinal(26);

            foreach(DataRow dr in dtGongDan.Rows)
            {
                string strGongDanNo = dr["GongDan No"].ToString().Trim();
                int iGongDanQty = Convert.ToInt32(dr["GongDan Qty"].ToString().Trim());
                decimal dDroolsRate = Convert.ToDecimal(dr["Qty Loss Rate"].ToString().Trim()) / 100.0M;
                decimal dConsumption = Convert.ToDecimal(dr["Consumption"].ToString().Trim());

                decimal dRmUsedQty = 0.0M;
                if (Decimal.Compare(dDroolsRate, 1.0M) == 0) { dRmUsedQty = 0.0M; }
                else { dRmUsedQty = iGongDanQty * dConsumption / (1.0M - dDroolsRate); }
                dr["RM Used Qty"] = Math.Round(dRmUsedQty, 6);

                decimal dRMPrice = 0.0M;
                if (String.Compare(dr["RM Currency"].ToString().Trim(), "USD") != 0)
                { dRMPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim()) * this.GetExchangeRate(dr["RM Currency"].ToString().Trim()); }
                else { dRMPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim()); }
                decimal dTotalRMCost = Math.Round(dRMPrice * dRmUsedQty, 2);
                dr["Total RM Cost(USD)"] = dTotalRMCost;

                //if order price is 0 (say for scrap sales), use total RM cost as selling price
                decimal dOrderPrice = Convert.ToDecimal(dr["Order Price"].ToString().Trim());
                //if (dOrderPrice == 0.0M) 
                //{                    
                //    //decimal totalRMcost = dtGongDan.Select("SE")
                //    //dOrderPrice = Math.Round(dTotalRMCost / Convert.ToDecimal(dr["GongDan Qty"].ToString().Trim()), 4);                 
                //}

                decimal dDroolsQuota = Math.Round(dRmUsedQty * dDroolsRate, 6);
                dr["Drools Quota"] = dDroolsQuota;

                gdComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = dr["Actual Start Date"].ToString().Trim();
                gdComm.Parameters.Add("@ActualEndDate", SqlDbType.NVarChar).Value = dr["Actual End Date"].ToString().Trim();
                gdComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = dr["Batch Path"].ToString().Trim();
                gdComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim();
                gdComm.Parameters.Add("@BomInCustoms", SqlDbType.NVarChar).Value = dr["BOM In Customs"].ToString().Trim();
                gdComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = dr["GongDan No"].ToString().Trim();
                gdComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim();
                gdComm.Parameters.Add("@FgDescription", SqlDbType.NVarChar).Value = dr["FG Description"].ToString().Trim();
                gdComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                gdComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim();
                gdComm.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = dr["Item Description"].ToString().Trim();
                gdComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim();
                gdComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = dr["Inventory Type"].ToString().Trim();
                gdComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = dr["RM Category"].ToString().Trim();
                gdComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = dr["RM EHB"].ToString().Trim();
                gdComm.Parameters.Add("@BgdNo", SqlDbType.NVarChar).Value = dr["BGD No"].ToString().Trim();
                gdComm.Parameters.Add("@IeType", SqlDbType.NVarChar).Value = dr["IE Type"].ToString().Trim();
                gdComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = dr["Order No"].ToString().Trim();
                gdComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = iGongDanQty;
                gdComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = dOrderPrice;
                gdComm.Parameters.Add("@OrderCurr", SqlDbType.NVarChar).Value = dr["Order Currency"].ToString().Trim();
                gdComm.Parameters.Add("@TotalRmCost", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["Total RM Cost(USD)"].ToString().Trim());
                gdComm.Parameters.Add("@RmUsedQty", SqlDbType.Decimal).Value = Math.Round(dRmUsedQty, 6);
                gdComm.Parameters.Add("@RMCurr", SqlDbType.NVarChar).Value = dr["RM Currency"].ToString().Trim();
                gdComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["RM Price"].ToString().Trim());
                gdComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = dConsumption;
                gdComm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = dDroolsQuota;
                gdComm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = dDroolsRate * 100.0M;
                gdComm.Parameters.Add("@ChnName", SqlDbType.NVarChar).Value = dr["CHN Name"].ToString().Trim();
                gdComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = dr["Drools EHB"].ToString().Trim();
                gdComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = dr["Destination"].ToString().Trim();                                                                                                                               
                gdComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                gdComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dr["Created Date"].ToString().Trim());
                                
                gdComm.CommandText = "INSERT INTO M_DailyGongDan([Actual Start Date], [Actual End Date], [Batch Path], [Batch No], [BOM In Customs], [GongDan No], " + 
                                     "[FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [RM EHB], " + 
                                     "[BGD No], [IE Type], [Order No], [GongDan Qty], [Order Price], [Order Currency], [Total RM Cost(USD)], [RM Used Qty], " + 
                                     "[RM Currency], [RM Price], [Consumption], [Drools Quota], [Drools Rate], [CHN Name], [Drools EHB], [Destination], [Creater], " + 
                                     "[Created Date]) VALUES(@ActualStartDate, @ActualEndDate, @BatchPath, @BatchNo, @BomInCustoms, @GongDanNo, @FgNo, @FgDescription, " + 
                                     "@LineNo, @ItemNo, @ItemDescription, @LotNo, @InventoryType, @RMCategory, @RMEHB, @BgdNo, @IeType, @OrderNo, @GongDanQty, " + 
                                     "@OrderPrice, @OrderCurr, @TotalRmCost, @RmUsedQty, @RMCurr, @RMPrice, @Consumption, @DroolsQuota, @DroolsRate, @ChnName, " + 
                                     "@DroolsEHB, @Destination, @Creater, @CreatedDate)";
                gdComm.ExecuteNonQuery();
                gdComm.Parameters.Clear();
            }

                        

            string strSQL = @"SELECT SUM([RM Used Qty]) AS TotalRmQty, SUM([Total RM Cost(USD)]) AS TotalRmCost, [GongDan No] FROM M_DailyGongDan GROUP BY [GongDan No]";
            gdAdapter = new SqlDataAdapter(strSQL, gdConn);
            DataTable dtGDC = new DataTable();
            gdAdapter.Fill(dtGDC);



            foreach (DataRow dr in dtGDC.Rows)
            {
                gdComm.Parameters.Add("@TotalRmQty", SqlDbType.Decimal).Value = Convert.ToDecimal(dr[0].ToString().Trim());
                gdComm.Parameters.Add("@TotalRmCost", SqlDbType.Decimal).Value = Convert.ToDecimal(dr[1].ToString().Trim());
                gdComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = dr[2].ToString().Trim().ToUpper();
                gdComm.CommandText = "UPDATE M_DailyGongDan SET [Total RM Qty] = @TotalRmQty, [Total RM Cost(USD)] = @TotalRmCost WHERE [GongDan No] = @GongDanNo";
                gdComm.ExecuteNonQuery();
                gdComm.Parameters.Clear();
            }
            dtGDC.Dispose();
            gdComm.CommandText = "DELETE FROM M_DailyGongDanList WHERE [GongDan No] IN (SELECT DISTINCT [GongDan No] FROM M_DailyGongDan)";
            gdComm.ExecuteNonQuery();

            //If RM cost per finished goods total quantity is higher than selling price, use total RM cost as selling price
            gdComm.CommandText = "Update [M_DailyGongDan]  Set [Order Price] = CONVERT(decimal(18,2),[Total RM Cost(USD)]/[GongDan Qty]/[ObjectValue]) FROM [M_DailyGongDan] LEFT JOIN [B_SysInfo] on substring([B_SysInfo].[ObjectName], CHARINDEX(':',[B_SysInfo].[ObjectName],0)+1,3) = [M_DailyGongDan].[Order Currency] WHERE [GongDan Qty] > 0 AND ([Order Price] <0.1) AND ([ObjectValue] IS NOT NULL)";
            //Update [M_DailyGongDan]  Set [Order Price] = CONVERT(decimal(18,2),[Total RM Cost(USD)]/[GongDan Qty]/[ObjectValue]) FROM [M_DailyGongDan] LEFT JOIN [B_SysInfo] on substring([B_SysInfo].[ObjectName], CHARINDEX(':',[B_SysInfo].[ObjectName],0)+1,3) = [M_DailyGongDan].[Order Currency] WHERE [GongDan Qty] > 0 AND ([Order Price] <0.1) AND ([ObjectValue] IS NOT NULL)
            gdComm.ExecuteNonQuery();

            gdComm.Dispose();

            gdAdapter = new SqlDataAdapter("SELECT * FROM M_DailyGongDan", gdConn);
            dtGongDan.Reset();
            gdAdapter.Fill(dtGongDan);
            gdAdapter.Dispose();
            dtGongDan.Columns.Remove("Created Date");
            dtGongDan.Columns.Remove("Creater");
            dvGongDan = dtGongDan.DefaultView;
            this.dgvGongDanData.DataSource = dvGongDan;
            this.dgvGongDanData.Columns[0].HeaderText = "Select";
            this.dgvGongDanData.Columns["Actual Start Date"].Visible = false;
            this.dgvGongDanData.Columns["Actual End Date"].Visible = false;
            this.dgvGongDanData.Columns["Batch Path"].Visible = false;
            this.dgvGongDanData.Columns["BOM In Customs"].Visible = false;
            this.dgvGongDanData.Columns["GongDan No"].Frozen = true;
            if (gdConn.State == ConnectionState.Open) { gdConn.Close(); gdConn.Dispose(); }
        }

        private decimal GetExchangeRate(string strExchange)
        {
            decimal dExchangeRate = 0.0M;
            if (dtExRate.Rows.Count == 0)
            {
                SqlConnection ConnExRate = new SqlConnection(SqlLib.StrSqlConnection);
                if (ConnExRate.State == ConnectionState.Closed) { ConnExRate.Open(); }
                SqlDataAdapter AdapterExRate = new SqlDataAdapter(@"SELECT [ObjectName], [ObjectValue] FROM B_SysInfo WHERE [ObjectName] LIKE 'ExchangeRate%'", ConnExRate);
                AdapterExRate.Fill(dtExRate);
                AdapterExRate.Dispose();
                if (ConnExRate.State == ConnectionState.Open) { ConnExRate.Close(); ConnExRate.Dispose(); }
            }
            DataRow[] dr = dtExRate.Select("[ObjectName]= 'ExchangeRate:" + strExchange.Trim() + "'");
            if (dr.Length > 0) { dExchangeRate = Convert.ToDecimal(dr[0][1].ToString().Trim()); }
            return dExchangeRate;
        }

        private void btnCheckRcvdDate_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.RowCount == 0)
            { MessageBox.Show("There is no data to check.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (dtGongDan.Columns.Contains("JudgeRcvdDate")) { dtGongDan.Columns.Remove("JudgeRcvdDate"); }
            dtGongDan.Columns.Add("JudgeRcvdDate", typeof(bool));
            dtGongDan.Columns["JudgeRcvdDate"].DefaultValue = false;
            this.dgvGongDanData.Columns["JudgeRcvdDate"].Visible = false;

            SqlConnection ConnCRD = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnCRD.State == ConnectionState.Closed) { ConnCRD.Open(); }
            string strSQL = "SELECT [GongDan No], [Line No] FROM M_DailyGongDan AS M LEFT JOIN C_RMReceiving AS C ON M.[Item No] = C.[Item No] AND " + 
                            "M.[Lot No] = C.[Lot No] AND M.[RM EHB] = C.[RM EHB] AND M.[BGD No] = C.[BGD No] WHERE C.[Customs Rcvd Date] IS NULL";
            SqlDataAdapter AdapterCRD = new SqlDataAdapter(strSQL, ConnCRD);
            DataTable dtCRD = new DataTable();
            AdapterCRD.Fill(dtCRD);
            AdapterCRD.Dispose();
            if (dtCRD.Rows.Count == 0)
            {
                MessageBox.Show("There is not abnormal data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtCRD.Dispose();
                ConnCRD.Dispose();
                return;
            }
            foreach(DataRow drow in dtCRD.Rows)
            {
                string strGongDanNo = drow[0].ToString().Trim();
                int iLineNo = Convert.ToInt32(drow[1].ToString().Trim());
                DataRow dr = dtGongDan.Select("[GongDan No]='" + strGongDanNo + "' AND [Line No]=" + iLineNo + "")[0];
                dr["JudgeRcvdDate"] = true;
            }
            dtGongDan.AcceptChanges();
            MessageBox.Show("There are " + dtCRD.Rows.Count + " abnormal data, please filter the datagrid to check.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            dtCRD.Dispose();
            if (ConnCRD.State == ConnectionState.Open) { ConnCRD.Close(); ConnCRD.Dispose(); }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);
            if (this.dgvGongDanData.Columns[0].HeaderText != "Select")
            {
                bool bJudge = false;
                int iRow = 2;
                for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanData[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        bJudge = true;
                        for (int j = 2; j < this.dgvGongDanData.ColumnCount - 2; j++)
                        { excel.Cells[iRow, j - 1] = "'" + this.dgvGongDanData[j, i].Value.ToString().Trim(); }
                        iRow++;
                    }
                }

                if (bJudge)
                {
                    for (int k = 2; k < this.dgvGongDanData.ColumnCount - 2; k++)
                    { excel.Cells[1, k - 1] = this.dgvGongDanData.Columns[k].HeaderText.ToString(); }

                    excel.Cells.EntireColumn.AutoFit();
                    excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                    excel.Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
                {
                    for (int j = 2; j < this.dgvGongDanData.ColumnCount - 2; j++)
                    { excel.Cells[i + 2, j - 1] = "'" + this.dgvGongDanData[j, i].Value.ToString().Trim(); }
                }
                    
                for (int k = 2; k < this.dgvGongDanData.ColumnCount - 2; k++)
                { excel.Cells[1, k - 1] = this.dgvGongDanData.Columns[k].HeaderText.ToString(); }

               excel.Cells.EntireColumn.AutoFit();
               excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
               excel.Visible = true;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtPath.Text = openDlg.FileName;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtPath.Text.Trim()))
            { MessageBox.Show("Please find out the uploading file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge);
                this.btnPreview_Click(sender, e);
            }
            catch (Exception ex)
            { MessageBox.Show("Upload error, please try again.\n" + ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); throw; }
        }

        private void ImportExcelData(string strFilePath, bool bJudge)
        {
            string strConn;
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + "; Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$] WHERE [GongDan No] IS NOT NULL AND [GongDan No] <> ''", myConn);
            DataTable dtFileGD = new DataTable();
            myAdapter.Fill(dtFileGD);
            myAdapter = new OleDbDataAdapter("SELECT DISTINCT [GongDan No] FROM [Sheet1$]", myConn);
            DataTable dtFileGDN = new DataTable();
            myAdapter.Fill(dtFileGDN);
            myAdapter.Dispose();
            myConn.Dispose();
            if (dtFileGD.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtFileGD.Dispose();
                dtFileGDN.Dispose();
                return;
            }
            if (MessageBox.Show("Are you sure to upload the file to batch update existing data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { dtFileGD.Dispose(); dtFileGDN.Dispose(); return; }

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlCommand Comm = new SqlCommand();
            Comm.Connection = Conn;

            string strGongDan = null;
            foreach (DataRow dr in dtFileGDN.Rows) { strGongDan += "'" + dr[0].ToString().Trim() + "',"; }
            strGongDan = strGongDan.Remove(strGongDan.Length - 1);
            Comm.CommandText = "DELETE FROM M_DailyGongDan WHERE [GongDan No] IN (" + strGongDan + ")";
            Comm.ExecuteNonQuery();
            dtFileGDN.Dispose();

            SqlLib sqlLib = new SqlLib();
            foreach(DataRow dr in dtFileGD.Rows)
            {
                Comm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = dr["Actual Start Date"].ToString().Trim();
                Comm.Parameters.Add("@ActualEndDate", SqlDbType.NVarChar).Value = dr["Actual End Date"].ToString().Trim();
                Comm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = dr["Batch Path"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@BomInCustoms", SqlDbType.NVarChar).Value = dr["BOM In Customs"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = dr["GongDan No"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim();
                Comm.Parameters.Add("@FgDescription", SqlDbType.NVarChar).Value = dr["FG Description"].ToString().Trim();
                Comm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = dr["Item Description"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = dr["Inventory Type"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = dr["RM Category"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = dr["RM EHB"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@BgdNo", SqlDbType.NVarChar).Value = dr["BGD No"].ToString().Trim();
                Comm.Parameters.Add("@IeType", SqlDbType.NVarChar).Value = dr["IE Type"].ToString().Trim();
                Comm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = dr["Order No"].ToString().Trim().ToUpper();
                string strGongDanQty = dr["GongDan Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strGongDanQty)) { Comm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                else { Comm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGongDanQty); }
                string strOrderPrice = dr["Order Price"].ToString().Trim();
                if (String.IsNullOrEmpty(strOrderPrice)) { Comm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strOrderPrice), 2); }
                Comm.Parameters.Add("@OrderCurr", SqlDbType.NVarChar).Value = dr["Order Currency"].ToString().Trim().ToUpper();
                string strTotalRmQty = dr["Total RM Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strTotalRmQty)) { Comm.Parameters.Add("@TotalRmQty", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@TotalRmQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(double.Parse(strTotalRmQty))), 6); }
                string strTotalRmCost = dr["Total RM Cost(USD)"].ToString().Trim();
                if (String.IsNullOrEmpty(strTotalRmCost)) { Comm.Parameters.Add("@TotalRmCost", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@TotalRmCost", SqlDbType.Decimal).Value = Convert.ToDecimal(strTotalRmCost); }
                string strRmUsedQty = dr["RM Used Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strRmUsedQty)) { Comm.Parameters.Add("@RmUsedQty", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@RmUsedQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(double.Parse(strRmUsedQty))), 6); }
                Comm.Parameters.Add("@RMCurr", SqlDbType.NVarChar).Value = dr["RM Currency"].ToString().Trim().ToUpper();
                string strRmPrice = dr["RM Price"].ToString().Trim();
                if (String.IsNullOrEmpty(strRmPrice)) { Comm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(strRmPrice); }
                string strConsmpt = dr["Consumption"].ToString().Trim();
                if (String.IsNullOrEmpty(strConsmpt)) { Comm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(double.Parse(strConsmpt))), 6); }
                string strDroolsQ = dr["Drools Quota"].ToString().Trim();
                if (String.IsNullOrEmpty(strDroolsQ)) { Comm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(double.Parse(strDroolsQ))), 6); }
                string strDroolsR = dr["Drools Rate"].ToString().Trim();
                if (String.IsNullOrEmpty(strDroolsR)) { Comm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = 0.0M; }
                else { Comm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(double.Parse(strDroolsR))), 6); }   
                Comm.Parameters.Add("@ChnName", SqlDbType.NVarChar).Value = dr["CHN Name"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = dr["Drools EHB"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = dr["Destination"].ToString().Trim().ToUpper();
                Comm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                Comm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                Comm.CommandText = "INSERT INTO M_DailyGongDan([Actual Start Date], [Actual End Date], [Batch Path], [Batch No], [BOM In Customs], [GongDan No], " +
                                   "[FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [RM EHB], " +
                                   "[BGD No], [IE Type], [Order No], [GongDan Qty], [Order Price], [Order Currency], [Total RM Qty], [Total RM Cost(USD)], " + 
                                   "[RM Used Qty], [RM Currency], [RM Price], [Consumption], [Drools Quota], [Drools Rate], [CHN Name], [Drools EHB], [Destination], " + 
                                   "[Creater], [Created Date]) VALUES(@ActualStartDate, @ActualEndDate, @BatchPath, @BatchNo, @BomInCustoms, @GongDanNo, @FgNo, " + 
                                   "@FgDescription, @LineNo, @ItemNo, @ItemDescription, @LotNo, @InventoryType, @RMCategory, @RMEHB, @BgdNo, @IeType, @OrderNo, " + 
                                   "@GongDanQty, @OrderPrice, @OrderCurr, @TotalRmQty, @TotalRmCost, @RmUsedQty, @RMCurr, @RMPrice, @Consumption, @DroolsQuota, " + 
                                   "@DroolsRate, @ChnName, @DroolsEHB, @Destination, @Creater, @CreatedDate)";
                Comm.ExecuteNonQuery();
                Comm.Parameters.Clear();
            }
            sqlLib.Dispose(0);
            dtFileGD.Dispose();
            Comm.Dispose();
            if (Conn.State == ConnectionState.Open) { Conn.Close(); Conn.Dispose(); }
            MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); 
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvGongDan.RowFilter = "";
            SqlConnection gdViewConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gdViewConn.State == ConnectionState.Closed) { gdViewConn.Open(); }
            SqlDataAdapter gdViewAdapter = new SqlDataAdapter("SELECT * FROM M_DailyGongDan", gdViewConn);
            DataTable dtViewGD = new DataTable();
            gdViewAdapter.Fill(dtViewGD);
            gdViewAdapter.Dispose();
            if (dtViewGD.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtViewGD.Dispose();
                gdViewConn.Dispose();
                return;
            }
            dtGongDan = dtViewGD.Copy();
            dtViewGD.Dispose();

            dvGongDan = dtGongDan.DefaultView;
            this.dgvGongDanData.DataSource = dvGongDan;
            this.dgvGongDanData.Columns[0].HeaderText = "Select";
            this.dgvGongDanData.Columns["Actual Start Date"].Visible = false;
            this.dgvGongDanData.Columns["Actual End Date"].Visible = false;
            this.dgvGongDanData.Columns["Batch Path"].Visible = false;
            this.dgvGongDanData.Columns["BOM In Customs"].Visible = false;
            this.dgvGongDanData.Columns["Creater"].Visible = false;
            this.dgvGongDanData.Columns["Created Date"].Visible = false;
            this.dgvGongDanData.Columns["GongDan No"].Frozen = true;
            if (gdViewConn.State == ConnectionState.Open) { gdViewConn.Close(); gdViewConn.Dispose(); }
        }

        private void dgvGongDanData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvGongDanData.RowCount; i++) { this.dgvGongDanData[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvGongDanData.RowCount; i++) { this.dgvGongDanData[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
                    {
                        if (String.Compare(this.dgvGongDanData[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvGongDanData[0, i].Value = true; }

                        else { this.dgvGongDanData[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
                bSwitch = true;
            }
        }

        private void dgvGongDanData_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dgvGongDanData.RowCount == 0) { return; }
            int iRow = this.dgvGongDanData.CurrentRow.Index;
            string strGongDanNo = this.dgvGongDanData["GongDan No", iRow].Value.ToString().Trim().ToUpper();
            if (e.ColumnIndex == 0)
            {
                if (String.IsNullOrEmpty(dvGongDan.RowFilter) && bSwitch == false)
                {
                    DataRow[] dRow = dvGongDan.Table.Select("[GongDan No] = '" + strGongDanNo + "'");
                    bool bJudge = Convert.ToBoolean(this.dgvGongDanData[0, iRow].Value);
                    if (!bJudge) { foreach (DataRow dr in dRow) { this.dgvGongDanData[0, dvGongDan.Table.Rows.IndexOf(dr)].Value = true; } }
                    else { foreach (DataRow dr in dRow) { this.dgvGongDanData[0, dvGongDan.Table.Rows.IndexOf(dr)].Value = false; } }
                }
            }

            int iCount = 0;
            for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
            {
                if (String.Compare(this.dgvGongDanData[0, i].EditedFormattedValue.ToString(), "True") == 0)
                { iCount++; }
            }
            if (iCount < this.dgvGongDanData.RowCount && iCount > 0)
            { this.dgvGongDanData.Columns[0].HeaderText = "Reverse"; }

            else if (iCount == this.dgvGongDanData.RowCount)
            { this.dgvGongDanData.Columns[0].HeaderText = "Cancel"; }

            else if (iCount == 0)
            { this.dgvGongDanData.Columns[0].HeaderText = "Select"; }
            bSwitch = false;
        }

        private void dgvGongDanData_CellClick(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.ColumnIndex == 1)
            {
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SqlConnection ConnDelGD = new SqlConnection(SqlLib.StrSqlConnection);
                    if (ConnDelGD.State == ConnectionState.Closed) { ConnDelGD.Open(); }
                    SqlCommand CommDelGD = new SqlCommand();
                    CommDelGD.Connection = ConnDelGD;

                    int iRow = this.dgvGongDanData.CurrentRow.Index;
                    string strGongDanNo = this.dgvGongDanData["GongDan No", iRow].Value.ToString().Trim();                                               
                    DataRow[] dRow = dvGongDan.Table.Select("[GongDan No] = '" + strGongDanNo + "'");
                    foreach (DataRow dr in dRow) { dr.Delete(); }
                    dtGongDan.AcceptChanges();

                    CommDelGD.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                    CommDelGD.CommandText = "DELETE FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                    CommDelGD.ExecuteNonQuery();
                    CommDelGD.CommandText = "DELETE FROM L_GongDan_Fulfillment WHERE [GongDan No] = @GongDanNo";
                    CommDelGD.ExecuteNonQuery();
                    CommDelGD.Parameters.Clear();
                    CommDelGD.Dispose();
                    if (ConnDelGD.State == ConnectionState.Open) { ConnDelGD.Close(); ConnDelGD.Dispose(); }
                    MessageBox.Show("Successfully done.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvGongDanData.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvGongDanData.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvGongDanData.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvGongDanData[strColumnName, this.dgvGongDanData.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] = " + strColumnText; }
                    }
                    dvGongDan.RowFilter = strFilter;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiExcludeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvGongDanData.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvGongDanData.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvGongDanData.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvGongDanData[strColumnName, this.dgvGongDanData.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvGongDanData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] <> " + strColumnText; }
                    }
                    dvGongDan.RowFilter = strFilter;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvGongDan.RowFilter = "";
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.CurrentCell != null)
            {
                string strColumnName = this.dgvGongDanData.Columns[this.dgvGongDanData.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter);
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dvGongDan.ToTable(true, new string[] { strColumnName });
                filterFrm.cmbFilterContent.DisplayMember = strColumnName;
                filterFrm.ShowDialog();
                fundeleFilter delefilter = new fundeleFilter(funfilter);
            }
        }

        private void funfilter(string strColumnName, string strCondition)
        {
            try
            {
                string strSymbol = filterFrm.cmbFilterSymbol.Text.ToString().Trim().ToUpper();
                if (strFilter.Trim() == "")
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvGongDanData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvGongDanData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvGongDanData.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvGongDanData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvGongDanData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvGongDanData.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                dvGongDan.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiCheckRcvdDate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim())) { strFilter += " AND [JudgeRcvdDate] = True"; }
                else { strFilter = "[JudgeRcvdDate] = True"; }
                dvGongDan.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanData.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.Khaki;
            }
        }
    }
}
