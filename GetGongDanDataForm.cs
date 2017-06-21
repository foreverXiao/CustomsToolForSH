using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class GetGongDanDataForm : Form
    {
        private DataTable gdTable = new DataTable();
        private DataTable RateTable = new DataTable();
        private DataGridView dgvDetails = new DataGridView();  
        private LoginForm loginFrm = new LoginForm();
        protected DataView dvFillDGV = new DataView();
        protected PopUpFilterForm filterFrm = null;
        string strFilter = null;
        bool bSwitch = false;

        private static GetGongDanDataForm getGongDanDataFrm;
        public GetGongDanDataForm()
        {
            InitializeComponent();
        }
        public static GetGongDanDataForm CreateInstance()
        {
            if (getGongDanDataFrm == null || getGongDanDataFrm.IsDisposed)
            {
                getGongDanDataFrm = new GetGongDanDataForm();
            }
            return getGongDanDataFrm;
        }

        private void GetGongDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            gdTable.Dispose();
            RateTable.Dispose();
        }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            this.dgvGongDanData.DataSource = DBNull.Value;

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            SqlComm.CommandText = @"SELECT COUNT(*) FROM M_DailyGongDanList WHERE [GD Pending] = 'false'"; 
            int iJudgeCount = Convert.ToInt32(SqlComm.ExecuteScalar());
            if (iJudgeCount > 0) //if we get some gongdan no. in table M_DailyGongDanList, then by reference to table C_BOM and C_BOMdetail to get ful set of data to fill up table M_DailyGongDan
            {
                SqlDataAdapter SqlAdapter = new SqlDataAdapter("SELECT * FROM V_QueryGongDanByList", SqlConn);
                gdTable.Reset();
                SqlAdapter.Fill(gdTable);
                SqlAdapter = new SqlDataAdapter("SELECT DISTINCT [GongDan No] FROM M_DailyGongDan", SqlConn);
                DataTable dtable = new DataTable();
                SqlAdapter.Fill(dtable);
                SqlAdapter.Dispose();
                // get gongdan no list from table M_DailyGongDanList, if the gongdan no is in this table M_DailyGongDanList, then delete them from table M_DailyGongDan
                if (dtable.Rows.Count > 0)
                {
                    string strGongDanList = null;
                    foreach (DataRow row in dtable.Rows)
                    {
                        string strGongDanNo = row[0].ToString().Trim();
                        DataRow[] dr = gdTable.Select("[GongDan No] = '" + strGongDanNo + "'");
                        if (dr.Length > 0) { strGongDanList += "'" + strGongDanNo + "',"; }                       
                    }
                    if (!String.IsNullOrEmpty(strGongDanList))
                    {
                        strGongDanList = strGongDanList.Remove(strGongDanList.Length - 1);
                        SqlComm.CommandText = @"DELETE FROM M_DailyGongDan WHERE [GongDan No] IN (" + strGongDanList + ")";
                        SqlComm.ExecuteNonQuery();
                    }
                }
                dtable.Dispose();

                gdTable.Columns.Add("Total RM Cost(USD)", typeof(decimal));
                gdTable.Columns["Total RM Cost(USD)"].DefaultValue = 0.0M;
                gdTable.Columns["Total RM Cost(USD)"].SetOrdinal(22);

                gdTable.Columns.Add("RM Used Qty", typeof(decimal));
                gdTable.Columns["RM Used Qty"].DefaultValue = 0.0M;
                gdTable.Columns["RM Used Qty"].SetOrdinal(27);

                gdTable.Columns.Add("Drools Quota", typeof(decimal));
                gdTable.Columns["Drools Quota"].DefaultValue = 0.0M;
                gdTable.Columns["Drools Quota"].SetOrdinal(28);

                for (int x = 0; x < gdTable.Rows.Count; x++)
                {
                    string strGongDanNo = gdTable.Rows[x]["GongDan No"].ToString().Trim();
                    int iGongDanQty = Convert.ToInt32(gdTable.Rows[x]["GongDan Qty"].ToString().Trim());
                    decimal dDroolsRate = Convert.ToDecimal(gdTable.Rows[x]["Qty Loss Rate"].ToString().Trim()) / 100.0M;
                    decimal dConsumption = Convert.ToDecimal(gdTable.Rows[x]["Consumption"].ToString().Trim());

                    decimal dRMUsedQty = 0.0M;
                    if (Decimal.Compare(dDroolsRate, 1.0M) == 0) { dRMUsedQty = 0.0M; }
                    else { dRMUsedQty = iGongDanQty * dConsumption / (1.0M - dDroolsRate); }
                    gdTable.Rows[x]["RM Used Qty"] = Math.Round(dRMUsedQty, 7);

                    decimal dRMPrice = 0.0M;
                    if (String.Compare(gdTable.Rows[x]["RM Currency"].ToString().Trim(), "USD") != 0)
                    { dRMPrice = Convert.ToDecimal(gdTable.Rows[x]["RM Price"].ToString().Trim()) * this.GetExchangeRate(gdTable.Rows[x]["RM Currency"].ToString().Trim()); }
                    else { dRMPrice = Convert.ToDecimal(gdTable.Rows[x]["RM Price"].ToString().Trim()); }
                    gdTable.Rows[x]["Total RM Cost(USD)"] = Math.Round(dRMPrice * dRMUsedQty, 2);

                    decimal dDroolsQuota = Math.Round(dRMUsedQty * dDroolsRate, 11);
                    gdTable.Rows[x]["Drools Quota"] = dDroolsQuota;

                    string strPCItem = gdTable.Rows[x]["PC Item"].ToString().Trim();
                    bool bPCItem = false;
                    if (String.IsNullOrEmpty(strPCItem) || String.Compare(strPCItem.ToUpper(), "FALSE") == 0) { bPCItem = false; }
                    else { bPCItem = true; }

                    SqlComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Actual Start Date"].ToString().Trim();
                    SqlComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Actual Close Date"].ToString().Trim();
                    SqlComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Batch Path"].ToString().Trim();
                    SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Batch No"].ToString().Trim();
                    SqlComm.Parameters.Add("@BOMinCustoms", SqlDbType.NVarChar).Value = gdTable.Rows[x]["BOM In Customs"].ToString().Trim();
                    SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Item No"].ToString().Trim();
                    SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Lot No"].ToString().Trim();
                    SqlComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Inventory Type"].ToString().Trim();
                    SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = gdTable.Rows[x]["RM Category"].ToString().Trim();
                    SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = gdTable.Rows[x]["RM Customs Code"].ToString().Trim();
                    SqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = gdTable.Rows[x]["BGD No"].ToString().Trim();
                    SqlComm.Parameters.Add("@ESSLINE", SqlDbType.NVarChar).Value = gdTable.Rows[x]["ESS/LINE"].ToString().Trim();
                    SqlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Order No"].ToString().Trim();
                    SqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = gdTable.Rows[x]["IE Type"].ToString().Trim();
                    SqlComm.Parameters.Add("@OrderCategory", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Order Category"].ToString().Trim();
                    SqlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Destination"].ToString().Trim();
                    SqlComm.Parameters.Add("@TotalShipQty", SqlDbType.Int).Value = iGongDanQty;
                    SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = iGongDanQty;
                    SqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(gdTable.Rows[x]["Order Price"].ToString().Trim());
                    SqlComm.Parameters.Add("@OrderCurrency", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Order Currency"].ToString().Trim();
                    SqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Convert.ToDecimal(gdTable.Rows[x]["Total RM Cost(USD)"].ToString().Trim());
                    SqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = gdTable.Rows[x]["RM Currency"].ToString().Trim();
                    SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(gdTable.Rows[x]["RM Price"].ToString().Trim());
                    SqlComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = dConsumption;
                    SqlComm.Parameters.Add("@RMUsedQty", SqlDbType.Decimal).Value = Math.Round(dRMUsedQty, 7);
                    SqlComm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = dDroolsQuota;
                    SqlComm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = dDroolsRate * 100.0M;
                    SqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = gdTable.Rows[x]["Drools EHB"].ToString().Trim();
                    SqlComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = gdTable.Rows[x]["CHN Name"].ToString().Trim();
                    SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(gdTable.Rows[x]["Created Date"].ToString().Trim());
                    SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                    SqlComm.Parameters.Add("@PCItem", SqlDbType.Bit).Value = bPCItem;
                    SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = gdTable.Rows[x]["GongDan No"].ToString().Trim();
                    SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = gdTable.Rows[x]["FG No"].ToString().Trim();
                    SqlComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(gdTable.Rows[x]["Line No"].ToString().Trim());

                    SqlComm.CommandText = @"INSERT INTO M_DailyGongDan([Actual Start Date], [Actual Close Date], [Batch Path], [Batch No], [BOM In Customs], [Item No], " +
                                           "[Lot No], [Inventory Type], [RM Category], [RM Customs Code], [BGD No], [ESS/LINE], [Order No], [IE Type], [Order Category], " +
                                           "[Destination], [Total Ship Qty], [GongDan Qty], [Order Price], [Order Currency], [Total RM Cost(USD)], [RM Currency], " +
                                           "[RM Price], [Consumption], [RM Used Qty], [Drools Quota], [Drools Rate], [Drools EHB], [CHN Name], [Created Date], [Creater], " +
                                           "[PC Item], [GongDan No], [FG No], [Line No]) VALUES(@ActualStartDate, @ActualCloseDate, @BatchPath, @BatchNo, @BOMinCustoms, " +
                                           "@ItemNo, @LotNo, @InventoryType, @RMCategory, @RMCustomsCode, @BGDNo, @ESSLINE, @OrderNo, @IEType, @OrderCategory, @Destination, " +
                                           "@TotalShipQty, @GongDanQty, @OrderPrice, @OrderCurrency, @TotalRMCost, @RMCurrency, @RMPrice, @Consumption, @RMUsedQty, " +
                                           "@DroolsQuota, @DroolsRate, @DroolsEHB, @CHNName, @CreatedDate, @Creater, @PCItem, @GongDanNo, @FGNo, @LineNo)";
                    SqlTransaction SqlTrans = SqlConn.BeginTransaction();
                    SqlComm.Transaction = SqlTrans;
                    try
                    {
                        SqlComm.ExecuteNonQuery();
                        SqlTrans.Commit();
                    }
                    catch (Exception)
                    {
                        SqlTrans.Rollback();
                        SqlTrans.Dispose();
                    }
                    finally
                    { SqlComm.Parameters.Clear(); }
                }               
            }
            else
            {
                MessageBox.Show("No data exist in GongDan List.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnGatherData.Focus();
            }

            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void btnOptimizeData_Click(object sender, EventArgs e)
        {//Futher fill up table M_DailyGongDan by updating Total RM Qty, Total RM Cost and Order Price for each Gongdan No, reference to table B_ExchangeRate and doing some calculation
            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            SqlDataAdapter SqlAdapter = new SqlDataAdapter("SELECT * FROM V_QueryGongDanByListMid", SqlConn);
            DataTable dTable = new DataTable();
            SqlAdapter.Fill(dTable);
            if (dTable.Rows.Count == 0)
            {
                SqlComm.Dispose();
                SqlConn.Dispose();
                dTable.Dispose();
                MessageBox.Show("Please gather gongdan data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            SqlAdapter = new SqlDataAdapter("SELECT DISTINCT [Grade], [Price] FROM B_FinancePrice", SqlConn);
            DataTable dtable = new DataTable();
            SqlAdapter.Fill(dtable);

            for (int y = 0; y < dTable.Rows.Count; y++)
            {
                decimal dOrderPrice = Convert.ToDecimal(dTable.Rows[y]["Order Price"].ToString().Trim());
                decimal dTotalRMQty = Convert.ToDecimal(dTable.Rows[y]["TotalRMQty"].ToString().Trim());
                decimal dTotalRMCost = Convert.ToDecimal(dTable.Rows[y]["TotalRMCost"].ToString().Trim());
                string strOrderCurr = dTable.Rows[y]["Order Currency"].ToString().Trim();
                string strGongDanNo = dTable.Rows[y]["GongDan No"].ToString().Trim();
                string strFGNo = dTable.Rows[y]["FG No"].ToString().Trim();
                if (dOrderPrice == 0.0M) //If RM cost per finished goods total quantity is higher than selling price, use the higher one of total RM cost and price by grade as selling price
                {
                    decimal dJudge1 = 0.0M;
                    decimal dJudge2 = Math.Round(dTotalRMCost / Convert.ToDecimal(dTable.Rows[y]["GongDan Qty"].ToString().Trim()), 4);
                    DataRow[] dr = dtable.Select("[Grade] = '" + strFGNo.Split('-')[0] + "'");
                    if (dr.Length > 0) { dJudge1 = Convert.ToDecimal(dr[0][1].ToString().Trim()); }

                    if (dJudge1 >= dJudge2) { dOrderPrice = dJudge1; }
                    else { dOrderPrice = dJudge2; }
                    if (String.Compare(strOrderCurr, "USD") != 0) { dOrderPrice = Math.Round(dOrderPrice / this.GetExchangeRate(strOrderCurr), 4); }
                }

                SqlComm.Parameters.Clear();
                SqlComm.Parameters.Add("@TotalRMQty", SqlDbType.Decimal).Value = dTotalRMQty;
                SqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = dTotalRMCost;
                SqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = dOrderPrice;
                SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                SqlComm.CommandText = @"UPDATE M_DailyGongDan SET [Total RM Qty] = @TotalRMQty, [Total RM Cost(USD)] = @TotalRMCost, [Order Price] = @OrderPrice " +
                                       "WHERE [GongDan No] = @GongDanNo";
                SqlComm.ExecuteNonQuery();
            }
            dtable.Dispose();
            dTable.Dispose();

            SqlComm.CommandText = @"DELETE FROM M_DailyGongDanList WHERE [GongDan No] IN (SELECT DISTINCT [GongDan No] FROM M_DailyGongDan)";
            SqlComm.ExecuteNonQuery();
            SqlAdapter = new SqlDataAdapter(@"SELECT * FROM M_DailyGongDan ORDER BY [GongDan No], [FG No], [Line No]", SqlConn);
            gdTable.Reset();
            SqlAdapter.Fill(gdTable);
            SqlAdapter.Dispose();

            gdTable.Columns["BOM In Customs"].SetOrdinal(30);
            gdTable.Columns.Remove("Created Date");
            gdTable.Columns.Remove("Creater");
            gdTable.Columns.Add("Old Item No", typeof(string));
            gdTable.Columns.Add("Old Lot No", typeof(string));
            gdTable.Columns.Add("IsNewLine", typeof(bool));
            gdTable.Columns.Add("Judge Price", typeof(bool));
            gdTable.Columns.Add("Judge Rcvd Date", typeof(bool));
            gdTable.Columns.Add("Judge RM Bal", typeof(bool));
            dvFillDGV = gdTable.DefaultView;

            this.dgvGongDanData.DataSource = dvFillDGV;
            this.dgvGongDanData.Columns[0].HeaderText = "全选";
            this.dgvGongDanData.Columns["Actual Start Date"].Visible = false;
            this.dgvGongDanData.Columns["Actual Close Date"].Visible = false;
            this.dgvGongDanData.Columns["Batch Path"].Visible = false;
            this.dgvGongDanData.Columns["Judge Price"].Visible = false;
            this.dgvGongDanData.Columns["Judge Rcvd Date"].Visible = false;
            this.dgvGongDanData.Columns["Judge RM Bal"].Visible = false;
            this.dgvGongDanData.Columns["GongDan No"].Frozen = true;
            this.CtrlColumnReadWrite();

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = Color.FromArgb(178, 235, 140);
            this.dgvGongDanData.EnableHeadersVisualStyles = false;
            this.dgvGongDanData.Columns["Lot No"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["Remark"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["Old Item No"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["Old Lot No"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["IsNewLine"].HeaderCell.Style = cellStyle;

            MessageBox.Show("Successfully update the related data for Total RM Qty, Total RM Cost and Order Price three fields.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private decimal GetExchangeRate(string strExchange)
        {
            decimal dExchangeRate = 0.0M;
            if (RateTable.Rows.Count == 0)
            {
                SqlConnection getConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (getConn.State == ConnectionState.Closed) { getConn.Open(); }
                SqlDataAdapter getAdapter = new SqlDataAdapter(@"SELECT * FROM B_ExchangeRate", getConn);
                getAdapter.Fill(RateTable);
                getAdapter.Dispose();
                if (getConn.State == ConnectionState.Open)
                {
                    getConn.Close();
                    getConn.Dispose();
                }
            }

            DataRow[] dr = RateTable.Select("[Object] = '" + strExchange.Trim() + ":USD'");
            if (dr.Length > 0) { dExchangeRate = Convert.ToDecimal(dr[0][1].ToString().Trim()); }
            return dExchangeRate;
        }

        private void btnCheckPrice_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.RowCount == 0)
            {
                MessageBox.Show("There is no data. Please extract data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnCheckPrice.Focus();
                return;
            }

            gdTable.Columns.Remove("Judge Price");
            gdTable.Columns.Add("Judge Price", typeof(bool));
            gdTable.Columns["Judge Price"].SetOrdinal(38);
            this.dgvGongDanData.DefaultCellStyle.BackColor = Color.Gainsboro;
            this.dgvGongDanData.Columns["Judge Price"].Visible = false;

            SqlConnection check1Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (check1Conn.State == ConnectionState.Closed) { check1Conn.Open(); }
            SqlDataAdapter check1Adapter = new SqlDataAdapter(@"SELECT DISTINCT [GongDan No], [Order Price], [Order Currency], [GongDan Qty], [Total RM Cost(USD)] FROM M_DailyGongDan", check1Conn);
            DataTable check1Table = new DataTable();
            check1Adapter.Fill(check1Table);
            check1Adapter.Dispose();
            check1Table.Columns.Add("IsMTO", typeof(bool));

            SqlCommand check1Comm = new SqlCommand();
            check1Comm.Connection = check1Conn;

            int iPriceCount = 0;
            for (int i = 0; i < check1Table.Rows.Count; i++)
            {//make sure order's selling amount should be no less than total RM cost, in another word we are making money instead of losing money
                decimal dOrderPrice = 0.0M;
                if (String.Compare(check1Table.Rows[i]["Order Currency"].ToString().Trim(), "USD") != 0)
                { dOrderPrice = Convert.ToDecimal(check1Table.Rows[i]["Order Price"].ToString().Trim()) * this.GetExchangeRate(check1Table.Rows[i]["Order Currency"].ToString().Trim()); }
                else { dOrderPrice = Convert.ToDecimal(check1Table.Rows[i]["Order Price"].ToString().Trim()); }
                decimal dTotalRMCost = Convert.ToDecimal(check1Table.Rows[i]["Total RM Cost(USD)"].ToString().Trim());
                decimal dTotalGDValue = Math.Round(dOrderPrice * Convert.ToInt32(check1Table.Rows[i]["GongDan Qty"].ToString().Trim()), 2);

                if (Decimal.Compare(dTotalRMCost, dTotalGDValue) == 1)
                {
                    iPriceCount++;
                    check1Table.Rows[i]["IsMTO"] = true;
                }
                check1Comm.Parameters.Clear();
            }
         
            if (iPriceCount == 0)
            {                
                MessageBox.Show("No abnormal data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvGongDanData.Focus();
                check1Comm.Dispose();
                check1Table.Clear();
                check1Table.Dispose();
                check1Conn.Close();
                check1Conn.Dispose();
                return;
            }
            else
            {               
                int iCheckRowCount = check1Table.Rows.Count;
                for (int m = 0; m < iCheckRowCount; m++)
                {
                    if (check1Table.Rows[m]["IsMTO"].ToString().Trim() != "True")
                    {
                        check1Table.Rows.RemoveAt(m);
                        m--;
                        iCheckRowCount--;
                    }
                }
                //Tell you how mnay Gongdan have issues with price that total RM cost is higher than selling amount
                if (MessageBox.Show("There are " + iPriceCount.ToString().Trim() + " abnormal data that Total RM Cost is greater than Total GongDan Value.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    for (int j = 0; j < check1Table.Rows.Count; j++)
                    {
                        for (int k = 0; k < this.dgvGongDanData.RowCount; k++)
                        {
                            if (String.Compare(this.dgvGongDanData["GongDan No", k].Value.ToString().Trim(), check1Table.Rows[j]["GongDan No"].ToString().Trim()) == 0)
                            {
                                DataGridViewRow dgvRow = this.dgvGongDanData.Rows.SharedRow(k);
                                dgvRow.DefaultCellStyle.BackColor = Color.LightPink;
                                this.dgvGongDanData["Judge Price", k].Value = true;
                            }
                        }
                    }
                }
                
                check1Comm.Dispose();
                check1Table.Clear();
                check1Table.Dispose();
                if (check1Conn.State == ConnectionState.Open)
                {
                    check1Conn.Close();
                    check1Conn.Dispose();
                } 
            }                
        }

        private void btnCheckRcvdDate_Click(object sender, EventArgs e)
        {//the purpose for this check is to find out those used without receving date recorded
            if (this.dgvGongDanData.RowCount == 0)
            {
                MessageBox.Show("There is no data. Please extract data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnCheckRcvdDate.Focus();
                return;
            }

            gdTable.Columns.Remove("Judge Rcvd Date");
            gdTable.Columns.Add("Judge Rcvd Date", typeof(bool));
            gdTable.Columns["Judge Rcvd Date"].SetOrdinal(39);
            this.dgvGongDanData.DefaultCellStyle.BackColor = Color.Gainsboro;
            this.dgvGongDanData.Columns["Judge Rcvd Date"].Visible = false;

            SqlConnection Check2Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Check2Conn.State == ConnectionState.Closed) { Check2Conn.Open(); }
            SqlCommand Check2Comm = new SqlCommand("SELECT * FROM V_CheckRcvdDateInGongDan", Check2Conn); //change the view to table probably there is a logic changed
            string strCount = Convert.ToString(Check2Comm.ExecuteScalar());
            if (String.IsNullOrEmpty(strCount))
            {
                MessageBox.Show("No abnormal data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvGongDanData.Focus();
                Check2Comm.Dispose();
                Check2Conn.Close();
                Check2Conn.Dispose();
                return;
            }
            else
            {
                SqlDataAdapter Check2Adapter = new SqlDataAdapter();
                Check2Adapter.SelectCommand = Check2Comm;
                DataTable Check2Table = new DataTable();
                Check2Adapter.Fill(Check2Table);
                Check2Adapter.Dispose();

                for (int i = 0; i < Check2Table.Rows.Count; i++)
                {
                    for (int j = 0; j < this.dgvGongDanData.RowCount; j++)
                    {
                        if (String.Compare(this.dgvGongDanData["Item No", j].Value.ToString().Trim(), Check2Table.Rows[i]["Item No"].ToString().Trim()) == 0 &&
                            String.Compare(this.dgvGongDanData["Lot No", j].Value.ToString().Trim(), Check2Table.Rows[i]["Lot No"].ToString().Trim()) == 0)
                        {
                            DataGridViewRow dgvRow = this.dgvGongDanData.Rows.SharedRow(j);
                            dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
                            this.dgvGongDanData["Judge Rcvd Date", j].Value = true;
                        }
                    }
                }

                MessageBox.Show("There are " + Check2Table.Rows.Count.ToString().Trim() + " data that Customs Receipt Date is blank.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvGongDanData.Focus();
                Check2Table.Clear();
                Check2Table.Dispose();
                Check2Comm.Dispose();
                if (Check2Conn.State == ConnectionState.Open)
                {
                    Check2Conn.Close();
                    Check2Conn.Dispose();
                }
            }
        }

        private void btnCheckBalance_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.RowCount == 0)
            {
                MessageBox.Show("There is no data. Please extract data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnCheckBalance.Focus();
                return;
            }

            gdTable.Columns.Remove("Judge RM Bal");
            gdTable.Columns.Add("Judge RM Bal", typeof(bool));
            gdTable.Columns["Judge RM Bal"].SetOrdinal(40);
            this.dgvGongDanData.DefaultCellStyle.BackColor = Color.Gainsboro;
            this.dgvGongDanData.Columns["Judge RM Bal"].Visible = false;

            SqlConnection Check3Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Check3Conn.State == ConnectionState.Closed) { Check3Conn.Open(); }
            SqlCommand Check3Comm = new SqlCommand("SELECT * FROM V_CheckBalanceInGongDan", Check3Conn);
            string strRMBalCount = Convert.ToString(Check3Comm.ExecuteScalar());
            if (String.IsNullOrEmpty(strRMBalCount))
            {
                MessageBox.Show("No abnormal data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvGongDanData.Focus();
                Check3Comm.Dispose();
                Check3Conn.Close();
                Check3Conn.Dispose();
                return;
            }
            else
            {
                SqlDataAdapter Check3Adapter = new SqlDataAdapter();
                Check3Adapter.SelectCommand = Check3Comm;
                DataTable Check3Table = new DataTable();
                Check3Adapter.Fill(Check3Table);
                Check3Adapter.Dispose();

                int iRMBalCount = 0;
                for (int i = 0; i < Check3Table.Rows.Count; i++)
                {
                    for (int j = 0; j < this.dgvGongDanData.RowCount; j++)
                    {
                        if (String.Compare(this.dgvGongDanData["RM Customs Code", j].Value.ToString().Trim(), Check3Table.Rows[i]["RM Customs Code"].ToString().Trim()) == 0 &&
                            String.Compare(this.dgvGongDanData["BGD No", j].Value.ToString().Trim(), Check3Table.Rows[i]["BGD No"].ToString().Trim()) == 0)
                        {
                            DataGridViewRow dgvRow = this.dgvGongDanData.Rows.SharedRow(j);
                            dgvRow.DefaultCellStyle.BackColor = Color.Khaki;
                            this.dgvGongDanData["Judge RM Bal", j].Value = true;
                            iRMBalCount++;
                        }
                    }
                }
                
                MessageBox.Show("There are " + iRMBalCount.ToString() + " data making RM Balance break out.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.dgvGongDanData.Focus();
                Check3Comm.Dispose();
                Check3Table.Clear();
                Check3Table.Dispose();
                if (Check3Conn.State == ConnectionState.Open)
                {
                    Check3Conn.Close();
                    Check3Conn.Dispose();
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            if (this.dgvGongDanData.Columns[0].HeaderText != "全选")
            {
                bool bJudge = false;
                int iRow = 2;
                for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanData[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        bJudge = true;
                        excel.get_Range(excel.Cells[iRow, 1], excel.Cells[iRow, this.dgvGongDanData.ColumnCount - 8]).NumberFormatLocal = "@";
                        for (int j = 4; j < this.dgvGongDanData.ColumnCount - 4; j++)
                        { excel.Cells[iRow, j - 3] = this.dgvGongDanData[j, i].Value.ToString().Trim(); }
                        iRow++;
                    }
                }

                if (bJudge)
                {
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanData.ColumnCount - 8]).NumberFormatLocal = "@";
                    for (int k = 4; k < this.dgvGongDanData.ColumnCount - 4; k++)
                    { excel.Cells[1, k - 3] = this.dgvGongDanData.Columns[k].HeaderText.ToString(); }

                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanData.ColumnCount - 8]).Font.Bold = true;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanData.ColumnCount - 8]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvGongDanData.ColumnCount - 8]).Font.Name = "Verdana";
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvGongDanData.ColumnCount - 8]).Font.Size = 9;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvGongDanData.ColumnCount - 8]).Borders.LineStyle = 1;
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Visible = true;
                }
            }
            else
            {
                excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvGongDanData.RowCount + 1, this.dgvGongDanData.ColumnCount - 8]).NumberFormatLocal = "@";
                for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
                {
                    for (int j = 4; j < this.dgvGongDanData.ColumnCount - 4; j++)
                    { excel.Cells[i + 2, j - 3] = this.dgvGongDanData[j, i].Value.ToString().Trim(); }
                }
                    
                for (int k = 4; k < this.dgvGongDanData.ColumnCount - 4; k++)
                { excel.Cells[1, k - 3] = this.dgvGongDanData.Columns[k].HeaderText.ToString(); }

               excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanData.ColumnCount - 8]).Font.Bold = true;
               excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanData.ColumnCount - 8]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
               excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvGongDanData.RowCount + 1, this.dgvGongDanData.ColumnCount - 8]).Font.Name = "Verdana";
               excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvGongDanData.RowCount + 1, this.dgvGongDanData.ColumnCount - 8]).Font.Size = 9;
               excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvGongDanData.RowCount + 1, this.dgvGongDanData.ColumnCount - 8]).Borders.LineStyle = 1;
               excel.Cells.EntireColumn.AutoFit();
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
            {
                MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnSearch.Focus();
                return;
            }
            try
            {
                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge);
                this.btnPreview_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upload error, please try again.\n" + ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void ImportExcelData(string strFilePath, bool bJudge)
        {
            string strConn;
            if (bJudge)
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else
            { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + "; Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", myConn);
            DataTable myTable = new DataTable();
            myAdapter.Fill(myTable);
            myAdapter = new OleDbDataAdapter("SELECT DISTINCT [GongDan No] FROM [Sheet1$]", myConn);
            DataTable myTbl = new DataTable();
            myAdapter.Fill(myTbl);
            myAdapter.Dispose();           
            myConn.Close();
            myConn.Dispose();         

            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myTable.Clear();
                myTable.Dispose();
                return;
            }
            DialogResult dlgR = MessageBox.Show("Please choose the upload condition:\n[Yes] : Mass update Lot Nos;\n[No] : Update existing all data;\n[Cancel] : Reject to upload.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dlgR == DialogResult.Cancel)
            {
                myTable.Dispose();
                return;
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;

                string strNote = null;
                if (dlgR == DialogResult.No)
                {
                    string strGongDan = null;
                    foreach (DataRow dr in myTbl.Rows)
                    { strGongDan += "'" + dr[0].ToString().Trim() + "',"; }                    
                    strGongDan = strGongDan.Remove(strGongDan.Length - 1);
                    sqlComm.CommandText = @"DELETE FROM M_DailyGongDan WHERE [GongDan No] IN (" + strGongDan + ")";
                    sqlComm.ExecuteNonQuery();
                    myTbl.Dispose();

                    SqlLib sqlLib = new SqlLib();
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        sqlComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = myTable.Rows[i]["Actual Start Date"].ToString().Trim();
                        sqlComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = myTable.Rows[i]["Actual Close Date"].ToString().Trim();
                        sqlComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = myTable.Rows[i]["Batch Path"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.GetBatchOrGongDanFormat(myTable.Rows[i]["Batch No"].ToString().Trim().ToUpper(), String.Empty);
                        sqlComm.Parameters.Add("@BOMinCustoms", SqlDbType.NVarChar).Value = myTable.Rows[i]["BOM In Customs"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Inventory Type"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Category"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Customs Code"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["BGD No"].ToString().Trim();
                        sqlComm.Parameters.Add("@ESSLINE", SqlDbType.NVarChar).Value = myTable.Rows[i]["ESS/LINE"].ToString().Trim();
                        sqlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Order No"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = myTable.Rows[i]["IE Type"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@OrderCategory", SqlDbType.NVarChar).Value = myTable.Rows[i]["Order Category"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = myTable.Rows[i]["Destination"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Total Ship Qty"].ToString().Trim())) { sqlComm.Parameters.Add("@TotalShipQty", SqlDbType.Int).Value = 0; }
                        else { sqlComm.Parameters.Add("@TotalShipQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Total Ship Qty"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["GongDan Qty"].ToString().Trim())) { sqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { sqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["GongDan Qty"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Order Price"].ToString().Trim())) { sqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["Order Price"].ToString().Trim()), 4); }
                        sqlComm.Parameters.Add("@OrderCurrency", SqlDbType.NVarChar).Value = myTable.Rows[i]["Order Currency"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Total RM Qty"].ToString().Trim())) { sqlComm.Parameters.Add("@TotalRMQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@TotalRMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(myTable.Rows[i]["Total RM Qty"].ToString().Trim()))), 6); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Total RM Cost(USD)"].ToString().Trim())) { sqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["Total RM Cost(USD)"].ToString().Trim()), 2); }
                        sqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Currency"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["RM Price"].ToString().Trim())) { sqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(myTable.Rows[i]["RM Price"].ToString().Trim()))), 6); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Consumption"].ToString().Trim())) { sqlComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(myTable.Rows[i]["Consumption"].ToString().Trim()))), 6); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["RM Used Qty"].ToString().Trim())) { sqlComm.Parameters.Add("@RMUsedQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@RMUsedQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(myTable.Rows[i]["RM Used Qty"].ToString().Trim()))), 7); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Drools Quota"].ToString().Trim())) { sqlComm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(myTable.Rows[i]["Drools Quota"].ToString().Trim()))), 8); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Drools Rate"].ToString().Trim())) { sqlComm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = 0.0M; }
                        else { sqlComm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(myTable.Rows[i]["Drools Rate"].ToString().Trim()))), 6); }
                        sqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = myTable.Rows[i]["Drools EHB"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = myTable.Rows[i]["CHN Name"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[i]["Remark"].ToString().Trim().ToUpper();
                        sqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                        sqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                        string strPCItem = myTable.Rows[i]["PC Item"].ToString().Trim();
                        if (String.IsNullOrEmpty(strPCItem) || String.Compare(strPCItem.ToUpper(), "FALSE") == 0) { sqlComm.Parameters.Add("@PCItem", SqlDbType.Bit).Value = false; }
                        else { sqlComm.Parameters.Add("@PCItem", SqlDbType.Bit).Value = true; }
                        sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.GetBatchOrGongDanFormat(String.Empty, myTable.Rows[i]["GongDan No"].ToString().Trim());
                        sqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["FG No"].ToString().Trim();
                        sqlComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Line No"].ToString().Trim());

                        sqlComm.CommandText = "INSERT INTO M_DailyGongDan([Actual Start Date], [Actual Close Date], [Batch Path], [Batch No], [BOM In Customs], " + 
                                              "[Item No], [Lot No], [Inventory Type], [RM Category], [RM Customs Code], [BGD No], [ESS/LINE], [Order No], [IE Type], " + 
                                              "[Order Category], [Destination], [Total Ship Qty], [GongDan Qty], [Order Price], [Order Currency], [Total RM Qty], " + 
                                              "[Total RM Cost(USD)], [RM Currency], [RM Price], [Consumption], [RM Used Qty], [Drools Quota], [Drools Rate], [Drools EHB], " + 
                                              "[CHN Name], [Remark], [Created Date], [Creater], [PC Item], [GongDan No], [FG No], [Line No]) VALUES(@ActualStartDate, " + 
                                              "@ActualCloseDate, @BatchPath, @BatchNo, @BOMinCustoms, @ItemNo, @LotNo, @InventoryType, @RMCategory, @RMCustomsCode, @BGDNo, " + 
                                              "@ESSLINE, @OrderNo, @IEType, @OrderCategory, @Destination, @TotalShipQty, @GongDanQty, @OrderPrice, @OrderCurrency, " + 
                                              "@TotalRMQty, @TotalRMCost, @RMCurrency, @RMPrice, @Consumption, @RMUsedQty, @DroolsQuota, @DroolsRate, @DroolsEHB, @CHNName, " + 
                                              "@Remark, @CreatedDate, @Creater, @PCItem, @GongDanNo, @FGNo, @LineNo)";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }
                    sqlLib.Dispose(0);
                }
                else
                {
                    SqlDataAdapter oneAdapter = new SqlDataAdapter(@"SELECT * FROM B_Drools", sqlConn);
                    DataTable dtDrools = new DataTable();
                    oneAdapter.Fill(dtDrools);
                    oneAdapter = new SqlDataAdapter(@"SELECT * FROM C_RMPurchase", sqlConn);
                    DataTable dtRMPurchase = new DataTable();
                    oneAdapter.Fill(dtRMPurchase);
                    oneAdapter.Dispose();
                   
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        string strNewRMCustomsCode = null, strNewBGDNo = null, strNewRMCategory = null, strNewRMCurrency = null, strNewDroolsEHB = null;
                        decimal dRMPrice = 0.0M, dTotalRMCost = 0.0M;

                        string strGongDanNo = myTable.Rows[i]["GongDan No"].ToString().Trim().ToUpper();
                        string strFGNo = myTable.Rows[i]["FG No"].ToString().Trim().ToUpper();
                        int iLineNo = Convert.ToInt32(myTable.Rows[i]["Line No"].ToString().Trim());
                        string strNewItemNo = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                        string strNewLotNo = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                        string strOldRMCustomsCode = myTable.Rows[i]["RM Customs Code"].ToString().Trim().ToUpper();
                        string strOldBGDNo = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                        string strChnName = myTable.Rows[i]["CHN Name"].ToString().Trim().ToUpper();
                        string strRemark = myTable.Rows[i]["Remark"].ToString().Trim().ToUpper();
                        decimal dRMUsedQty = Convert.ToDecimal(myTable.Rows[i]["RM Used Qty"].ToString().Trim());
                        string strOldItemNo = myTable.Rows[i]["Old Item No"].ToString().Trim().ToUpper();
                        string strOldLotNo = myTable.Rows[i]["Old Lot No"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(strOldItemNo)) { strOldItemNo = strNewItemNo; }
                        if (String.IsNullOrEmpty(strOldLotNo)) { strOldLotNo = strNewLotNo; }
                        string strOldRMCategory = myTable.Rows[i]["RM Category"].ToString().Trim().ToUpper();
                        string strOldDroolsEHB = myTable.Rows[i]["Drools EHB"].ToString().Trim().ToUpper();

                        if (String.Compare(strNewLotNo.Substring(6, 1).Trim().ToUpper(), "R") == 0) { strNewRMCategory = "RMB"; }
                        else { strNewRMCategory = "USD"; }
                        DataRow[] dr1 = dtRMPurchase.Select("[Item No] = '" + strNewItemNo + "' AND [Lot No] = '" + strNewLotNo + "'");
                        if (dr1.Length == 0) { strNote += "[GongDan No] = " + strGongDanNo + "\n"; continue; }
                        else
                        {
                            strNewRMCustomsCode = dr1[0]["RM Customs Code"].ToString().Trim();
                            strNewBGDNo = dr1[0]["BGD No"].ToString().Trim();
                            dRMPrice = Convert.ToDecimal(dr1[0]["RM Price(CIF)"].ToString().Trim());
                            strNewRMCurrency = dr1[0]["RM Currency"].ToString().Trim();
                        }

                        if (String.Compare(strOldRMCategory, "RMB") == 0 && String.Compare(strOldDroolsEHB.Split('-')[1].ToString().Trim(), "W") == 0)
                        {
                            //Since in BOM, SH Customs would change the Drools EHB from '-R' to '-W', not comply the rule(FG CHN Name & RM Category --> map Drools EHB).
                            //so in GongDan, once negative RM Balance happen, first to check the old data of RM Category and Drools EHB, verify whether comply the rule, 
                            //if so not, ensure the new Drools EHB euqal to the old Drools EHB.
                            strNewDroolsEHB = strOldDroolsEHB;
                        }
                        else
                        {
                            DataRow[] dr2 = dtDrools.Select("[FG CHN Name] = '" + strChnName + "' AND [RM Category] = '" + strNewRMCategory + "'");
                            if (dr2.Length > 0) { strNewDroolsEHB = dr2[0]["Drools EHB"].ToString().Trim(); }
                        }

                        DataRow[] dr4 = gdTable.Select("[GongDan No] = '" + strGongDanNo + "' AND [FG No] = '" + strFGNo + "' AND [Line No] = " + iLineNo + "");
                        foreach (DataRow dr in dr4)
                        {
                            dr["RM Customs Code"] = strNewRMCustomsCode;
                            dr["BGD No"] = strNewBGDNo;
                            dr["RM Category"] = strNewRMCategory;
                            dr["RM Price"] = dRMPrice;
                            dr["RM Currency"] = strNewRMCurrency;
                            dr["Drools EHB"] = strNewDroolsEHB;
                        }
                        gdTable.AcceptChanges();

                        DataRow[] dr3 = gdTable.Select("[GongDan No] = '" + strGongDanNo + "' AND [FG No] = '" + strFGNo + "'");
                        foreach (DataRow dr in dr3)
                        {
                            decimal dPrice = 0.0M;
                            if (String.Compare(dr["RM Currency"].ToString().Trim(), "USD") != 0)
                            { dPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim()) * this.GetExchangeRate(dr["RM Currency"].ToString().Trim()); }
                            else { dPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim()); }
                            dr["Total RM Cost(USD)"] = Math.Round(dPrice * Convert.ToDecimal(dr["RM Used Qty"].ToString().Trim()), 2);
                        }
                        dTotalRMCost = Convert.ToDecimal(gdTable.Compute("SUM([Total RM Cost(USD)])", "[GongDan No] = '" + strGongDanNo + "' AND [FG No] = '" + strFGNo + "'"));
                        foreach (DataRow dr in dr3) { dr["Total RM Cost(USD)"] = dTotalRMCost; }
                        gdTable.AcceptChanges();

                        sqlComm.Parameters.Clear();
                        sqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = dTotalRMCost;
                        sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                        sqlComm.CommandText = @"UPDATE M_DailyGongDan SET [Total RM Cost(USD)] = @TotalRMCost WHERE [GongDan No] = @GongDanNo";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                        sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strNewItemNo;
                        sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strNewLotNo;
                        sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strNewRMCustomsCode;
                        sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strNewBGDNo;
                        sqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = strNewRMCategory;
                        sqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = dRMPrice;
                        sqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strNewRMCurrency;
                        sqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = strNewDroolsEHB;
                        sqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                        sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                        sqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                        sqlComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = iLineNo;
                        sqlComm.CommandText = @"UPDATE M_DailyGongDan SET [Item No] = @ItemNo, [Lot No] = @LotNo, [RM Customs Code] = @RMCustomsCode, [BGD No] = @BGDNo, " + 
                                               "[RM Category] = @RMCategory, [RM Price] = @RMPrice, [RM Currency] = @RMCurrency, [Drools EHB] = @DroolsEHB, " + 
                                               "[Remark] = @Remark WHERE [GongDan No] = @GongDanNo AND [FG No] = @FGNo AND [Line No] = @LineNo";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                        sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                        sqlComm.Parameters.Add("@OldRMCustomsCode", SqlDbType.NVarChar).Value = strOldRMCustomsCode;
                        sqlComm.Parameters.Add("@OldBGDNo", SqlDbType.NVarChar).Value = strOldBGDNo;
                        sqlComm.Parameters.Add("@OldItemNo", SqlDbType.NVarChar).Value = strOldItemNo;
                        sqlComm.Parameters.Add("@OldLotNo", SqlDbType.NVarChar).Value = strOldLotNo;
                        sqlComm.Parameters.Add("@NewRMCustomsCode", SqlDbType.NVarChar).Value = strNewRMCustomsCode;
                        sqlComm.Parameters.Add("@NewBGDNo", SqlDbType.NVarChar).Value = strNewBGDNo;
                        sqlComm.Parameters.Add("@NewItemNo", SqlDbType.NVarChar).Value = strNewItemNo;
                        sqlComm.Parameters.Add("@NewLotNo", SqlDbType.NVarChar).Value = strNewLotNo;
                        sqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                        sqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                        sqlComm.CommandText = @"INSERT INTO C_GongDanAdjustBGD([GongDan No], [RM Customs Code(old)], [BGD No(old)], [Item No(old)], [Lot No(old)], " +
                                               "[RM Customs Code(new)], [BGD No(new)], [Item No(new)], [Lot No(new)], [Remark], [Created Date]) VALUES(@GongDanNo, " +
                                               "@OldRMCustomsCode, @OldBGDNo, @OldItemNo, @OldLotNo, @NewRMCustomsCode, @NewBGDNo, @NewItemNo, @NewLotNo, @Remark, @CreatedDate)";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }
                    dtDrools.Dispose();
                    dtRMPurchase.Dispose();
                }

                sqlComm.Dispose();               
                myTbl.Dispose();
                myTable.Dispose();
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }
                if (String.IsNullOrEmpty(strNote)) { MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else { MessageBox.Show("The following GongDans reject to upload, as inexistence:\n" + strNote, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private string GetBatchOrGongDanFormat(string strBatchNo, string strGongDanNo)
        {
            if (!String.IsNullOrEmpty(strBatchNo) && String.IsNullOrEmpty(strGongDanNo))
            {
                bool bJudgeBatch = strBatchNo.Contains("N");
                int iLength = 0;
                if (bJudgeBatch == false) { iLength = strBatchNo.Length; }
                else { iLength = strBatchNo.Split('N')[0].Length; }
                if (iLength == 3) { strBatchNo = "00000" + strBatchNo; }
                else if (iLength == 4) { strBatchNo = "0000" + strBatchNo; }
                else if (iLength == 5) { strBatchNo = "000" + strBatchNo; }
                else if (iLength == 6) { strBatchNo = "00" + strBatchNo; }
                else if (iLength == 7) { strBatchNo = "0" + strBatchNo; }
                return strBatchNo;
            }
            else if (String.IsNullOrEmpty(strBatchNo) && !String.IsNullOrEmpty(strGongDanNo))
            {
                bool bJudgeGongDan = strGongDanNo.Contains("N");
                int iLength = 0;
                if (bJudgeGongDan == false) { iLength = strGongDanNo.Split('-')[0].Length; }
                else { iLength = strGongDanNo.Split('N')[0].Length; }
                if (iLength == 3) { strGongDanNo = "00000" + strGongDanNo; }
                else if (iLength == 4) { strGongDanNo = "0000" + strGongDanNo; }
                else if (iLength == 5) { strGongDanNo = "000" + strGongDanNo; }
                else if (iLength == 6) { strGongDanNo = "00" + strGongDanNo; }
                else if (iLength == 7) { strGongDanNo = "0" + strGongDanNo; }
                return strGongDanNo;
            }
            else { return String.Empty; }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection browseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (browseConn.State == ConnectionState.Closed) { browseConn.Open(); }

            string strSQL = @"SELECT [Actual Start Date], [Actual Close Date], [Batch Path], [Batch No], [GongDan No], [FG No], [Line No], [Item No], [Lot No], " +
                             "[Inventory Type], [RM Category], [RM Customs Code], [BGD No], [ESS/LINE], [Order No], [IE Type], [Order Category], [Destination], " +
                             "[Total Ship Qty], [GongDan Qty], [Order Price], [Order Currency], [Total RM Qty], [Total RM Cost(USD)], [RM Currency], [RM Price], " +
                             "[Consumption], [RM Used Qty], [Drools Quota], [Drools Rate], [Drools EHB], [CHN Name], [BOM In Customs], [Remark], [PC Item] " +
                             "FROM M_DailyGongDan ORDER BY [GongDan No], [FG No], [Line No]";
            SqlDataAdapter browseAdapter = new SqlDataAdapter(strSQL, browseConn);
            DataTable myTable = new DataTable();
            browseAdapter.Fill(myTable);
            browseAdapter.Dispose();

            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                myTable.Clear();
                myTable.Dispose();
                browseConn.Close();
                browseConn.Dispose();
                this.btnPreview.Focus();
                return;
            }
            gdTable = myTable.Copy();
            myTable.Dispose();
                              
            gdTable.Columns.Add("Old Item No", typeof(string));
            gdTable.Columns.Add("Old Lot No", typeof(string));
            gdTable.Columns.Add("IsNewLine", typeof(bool));
            gdTable.Columns.Add("Judge Price", typeof(bool));
            gdTable.Columns.Add("Judge Rcvd Date", typeof(bool));
            gdTable.Columns.Add("Judge RM Bal", typeof(bool));
            dvFillDGV = gdTable.DefaultView;
            this.dgvGongDanData.DataSource = dvFillDGV;

            this.dgvGongDanData.Columns[0].HeaderText = "全选";          
            this.dgvGongDanData.Columns["Actual Start Date"].Visible = false;
            this.dgvGongDanData.Columns["Actual Close Date"].Visible = false;
            this.dgvGongDanData.Columns["Batch Path"].Visible = false;
            this.dgvGongDanData.Columns["Judge Price"].Visible = false;
            this.dgvGongDanData.Columns["Judge Rcvd Date"].Visible = false;
            this.dgvGongDanData.Columns["Judge RM Bal"].Visible = false;
            this.dgvGongDanData.Columns["GongDan No"].Frozen = true;
            this.CtrlColumnReadWrite();

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = Color.FromArgb(178, 235, 140);
            this.dgvGongDanData.EnableHeadersVisualStyles = false;
            this.dgvGongDanData.Columns["Lot No"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["Remark"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["Old Item No"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["Old Lot No"].HeaderCell.Style = cellStyle;
            this.dgvGongDanData.Columns["IsNewLine"].HeaderCell.Style = cellStyle;

            if (browseConn.State == ConnectionState.Open)
            {
                browseConn.Close();
                browseConn.Dispose();
            }
        }

        private void dgvGongDanData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvGongDanData.RowCount; i++) { this.dgvGongDanData[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvGongDanData.RowCount; i++) { this.dgvGongDanData[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
                    {
                        if (String.Compare(this.dgvGongDanData[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvGongDanData[0, i].Value = true; }

                        else { this.dgvGongDanData[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
                bSwitch = true;
            }
        }

        private void dgvGongDanData_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dgvGongDanData.RowCount == 0) { return; }

            int iRow = this.dgvGongDanData.CurrentRow.Index;
            string strGongDanNo = this.dgvGongDanData["GongDan No", iRow].Value.ToString().Trim().ToUpper();
            string strFGNo = this.dgvGongDanData["FG No", iRow].Value.ToString().Trim().ToUpper();
            if (e.ColumnIndex == 0)
            {
                if (String.IsNullOrEmpty(dvFillDGV.RowFilter) && bSwitch == false)
                {
                    DataRow[] dRow = dvFillDGV.Table.Select("[GongDan No] = '" + strGongDanNo + "' AND [FG No] = '" + strFGNo + "'", "[GongDan No], [FG No]");
                    bool bJudge = Convert.ToBoolean(this.dgvGongDanData[0, iRow].Value);
                    if (!bJudge)
                    {
                        foreach (DataRow dr in dRow)
                        { this.dgvGongDanData[0, dvFillDGV.Table.Rows.IndexOf(dr)].Value = true; }
                    }
                    else
                    {
                        foreach (DataRow dr in dRow)
                        { this.dgvGongDanData[0, dvFillDGV.Table.Rows.IndexOf(dr)].Value = false; }
                    }
                }
            }

            int iCount = 0;
            for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
            {
                if (String.Compare(this.dgvGongDanData[0, i].EditedFormattedValue.ToString(), "True") == 0)
                { iCount++; }
            }
            if (iCount < this.dgvGongDanData.RowCount && iCount > 0)
            { this.dgvGongDanData.Columns[0].HeaderText = "反选"; }

            else if (iCount == this.dgvGongDanData.RowCount)
            { this.dgvGongDanData.Columns[0].HeaderText = "取消全选"; }

            else if (iCount == 0)
            { this.dgvGongDanData.Columns[0].HeaderText = "全选"; }
            bSwitch = false;
        }

        private void dgvGongDanData_CellClick(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.ColumnIndex == 1) //Delete
            {
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SqlConnection delConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (delConn.State == ConnectionState.Closed) { delConn.Open(); }
                    SqlCommand delComm = new SqlCommand();
                    delComm.Connection = delConn;

                    int iRow = this.dgvGongDanData.CurrentRow.Index;
                    string strGongDanNo = this.dgvGongDanData["GongDan No", iRow].Value.ToString().Trim();
                    string strBatchNo = this.dgvGongDanData["Batch No", iRow].Value.ToString().Trim();
                    string strEssLine = this.dgvGongDanData["ESS/LINE", iRow].Value.ToString().Trim();
                                                    
                    DataRow[] dRow = dvFillDGV.Table.Select("[GongDan No] = '" + strGongDanNo + "'");
                    foreach (DataRow dr in dRow) { dr.Delete(); }
                    gdTable.AcceptChanges();

                    delComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                    delComm.CommandText = @"DELETE FROM M_DailyGongDan WHERE [GongDan No] = @GongDanNo";
                    SqlTransaction delTrans = delConn.BeginTransaction();
                    delComm.Transaction = delTrans;
                    try
                    {
                        delComm.ExecuteNonQuery();
                        delTrans.Commit();
                    }
                    catch (Exception)
                    {
                        delTrans.Rollback();
                        delTrans.Dispose();
                        throw;
                    }
                    finally { delComm.Parameters.Clear(); }

                    delComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    delComm.Parameters.Add("@ESSLINE", SqlDbType.NVarChar).Value = strEssLine;
                    delComm.CommandText = @"DELETE FROM B_OrderFulfillment WHERE [Batch No] = @BatchNo AND [ESS/LINE] = @ESSLINE";
                    delComm.ExecuteNonQuery();

                    delComm.Parameters.Clear();
                    delComm.Dispose();
                    if (delConn.State == ConnectionState.Open)
                    {
                        delConn.Close();
                        delConn.Dispose();
                    }
                }
            }

            if (e.ColumnIndex == 2) //Update
            {
                if (MessageBox.Show("Are you sure to update the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

                int iRow = this.dgvGongDanData.CurrentRow.Index;
                string strOldItemNo = this.dgvGongDanData["Old Item No", iRow].Value.ToString().Trim().ToUpper();
                string strOldLotNo = this.dgvGongDanData["Old Lot No", iRow].Value.ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(strOldItemNo) && String.IsNullOrEmpty(strOldLotNo))
                {
                    MessageBox.Show("[Old Item No] and [Old Lot No] columns cannot be empty.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.dgvGongDanData.Focus();
                    return;
                }

                string strIsNewLine = this.dgvGongDanData["IsNewLine", iRow].Value.ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(strIsNewLine) || String.Compare(strIsNewLine, "FALSE") == 0)
                {
                    string strNewRMCustomsCode = null, strNewBGDNo = null, strNewRMCategory = null, strNewRMCurrency = null, strNewDroolsEHB = null;
                    decimal dRMPrice = 0.0M, dTotalRMCost = 0.0M;

                    string strGongDanNo = this.dgvGongDanData["GongDan No", iRow].Value.ToString().Trim();
                    string strFGNo = this.dgvGongDanData["FG No", iRow].Value.ToString().Trim();
                    int iLineNo = Convert.ToInt32(this.dgvGongDanData["Line No", iRow].Value.ToString().Trim());
                    string strNewItemNo = this.dgvGongDanData["Item No", iRow].Value.ToString().Trim().ToUpper();
                    string strNewLotNo = this.dgvGongDanData["Lot No", iRow].Value.ToString().Trim().ToUpper();
                    string strOldRMCustomsCode = this.dgvGongDanData["RM Customs Code", iRow].Value.ToString().Trim();
                    string strOldBGDNo = this.dgvGongDanData["BGD No", iRow].Value.ToString().Trim();
                    string strCHNName = this.dgvGongDanData["CHN Name", iRow].Value.ToString().Trim();
                    string strRemark = this.dgvGongDanData["Remark", iRow].Value.ToString().Trim().ToUpper();
                    if (String.IsNullOrEmpty(strOldItemNo)) { strOldItemNo = strNewItemNo; }
                    if (String.IsNullOrEmpty(strOldLotNo)) { strOldLotNo = strNewLotNo; }
                    string strOldRMCategory = this.dgvGongDanData["RM Category", iRow].Value.ToString().Trim().ToUpper();
                    string strOldDroolsEHB = this.dgvGongDanData["Drools EHB", iRow].Value.ToString().Trim().ToUpper();

                    SqlConnection editConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (editConn.State == ConnectionState.Closed) { editConn.Open(); }
                    SqlCommand editComm = new SqlCommand();
                    editComm.Connection = editConn;

                    editComm.Parameters.Add("@NewItemNo", SqlDbType.NVarChar).Value = strNewItemNo;
                    editComm.Parameters.Add("@NewLotNo", SqlDbType.NVarChar).Value = strNewLotNo;
                    editComm.CommandText = @"SELECT [RM Customs Code], [BGD No], [RM Price(CIF)], [RM Currency] FROM C_RMPurchase WHERE [Item No] = @NewItemNo AND [Lot No] = @NewLotNo";
                    SqlDataReader editReader = editComm.ExecuteReader();
                    if (!editReader.HasRows)
                    {
                        MessageBox.Show("The new Item No & Lot No don't exist, please input a correct one.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        editComm.Dispose();
                        editConn.Close();
                        editConn.Dispose();
                        this.btnCheckBalance.Focus();
                        return;
                    }
                    else
                    {
                        while (editReader.Read())
                        {
                            strNewRMCustomsCode = editReader.GetValue(0).ToString().Trim();
                            strNewBGDNo = editReader.GetValue(1).ToString().Trim();
                            if (String.IsNullOrEmpty(editReader.GetValue(2).ToString().Trim())) { dRMPrice = 0.0M; }
                            else { dRMPrice = Convert.ToDecimal(editReader.GetValue(2).ToString().Trim()); }
                            strNewRMCurrency = editReader.GetValue(3).ToString().Trim();
                        }
                        editReader.Close();                     
                        editReader.Dispose();
                    }
                    editComm.Parameters.Clear();

                    if (String.Compare(strNewLotNo.Substring(6, 1).Trim().ToUpper(), "R") == 0) { strNewRMCategory = "RMB"; }
                    else { strNewRMCategory = "USD"; }

                    if (String.Compare(strOldRMCategory, "RMB") == 0 && String.Compare(strOldDroolsEHB.Split('-')[1].ToString().Trim(), "W") == 0)
                    { 
                        //Since in BOM, SH Customs would change the Drools EHB from '-R' to '-W', not comply the rule(FG CHN Name & RM Category --> map Drools EHB).
                        //so in GongDan, once negative RM Balance happen, first to check the old data of RM Category and Drools EHB, verify whether comply the rule, 
                        //if so not, ensure the new Drools EHB euqal to the old Drools EHB.
                        strNewDroolsEHB = strOldDroolsEHB; 
                    }
                    else
                    {
                        editComm.Parameters.Add("@ChineseName", SqlDbType.NVarChar).Value = strCHNName;
                        editComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = strNewRMCategory;
                        editComm.CommandText = @"SELECT [Drools EHB] FROM B_Drools WHERE [FG CHN Name] = @ChineseName AND [RM Category] = @RMCategory";
                        strNewDroolsEHB = Convert.ToString(editComm.ExecuteScalar());
                        editComm.Parameters.Clear();
                    }

                    #region //update Total RM Cost(USD)
                    this.dgvGongDanData["Item No", iRow].Value = strNewItemNo;
                    this.dgvGongDanData["RM Customs Code", iRow].Value = strNewRMCustomsCode;
                    this.dgvGongDanData["BGD No", iRow].Value = strNewBGDNo;
                    this.dgvGongDanData["RM Category", iRow].Value = strNewRMCategory;
                    this.dgvGongDanData["RM Price", iRow].Value = dRMPrice;
                    this.dgvGongDanData["RM Currency", iRow].Value = strNewRMCurrency;
                    this.dgvGongDanData["Drools EHB", iRow].Value = strNewDroolsEHB;
                    this.dgvGongDanData["Remark", iRow].Value = String.Empty;
                    this.dgvGongDanData["Old Item No", iRow].Value = String.Empty;
                    this.dgvGongDanData["Old Lot No", iRow].Value = String.Empty;
                    this.dgvGongDanData["Judge RM Bal", iRow].Value = false;
                    gdTable.AcceptChanges();

                    DataRow[] dtRow = gdTable.Select("[GongDan No] = '" + strGongDanNo + "'");
                    foreach (DataRow dr in dtRow)
                    {
                        decimal dPrice = 0.0M;
                        if (String.Compare(dr["RM Currency"].ToString().Trim(), "USD") != 0)
                        { dPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim()) * this.GetExchangeRate(dr["RM Currency"].ToString().Trim()); }
                        else { dPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim()); }
                        dr["Total RM Cost(USD)"] = Math.Round(dPrice * Convert.ToDecimal(dr["RM Used Qty"].ToString().Trim()), 2);
                    }
                    dTotalRMCost = (decimal)gdTable.Compute("SUM([Total RM Cost(USD)])", "[GongDan No] = '" + strGongDanNo + "'");
                    foreach (DataRow dr in dtRow) { dr["Total RM Cost(USD)"] = dTotalRMCost; }
                    gdTable.AcceptChanges();
                    editComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = dTotalRMCost;
                    editComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                    editComm.CommandText = @"UPDATE M_DailyGongDan SET [Total RM Cost(USD)] = @TotalRMCost WHERE [GongDan No] = @GongDanNo";
                    editComm.ExecuteNonQuery();
                    editComm.Parameters.Clear();
                    #endregion

                    editComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strNewLotNo;
                    editComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strNewRMCustomsCode;
                    editComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strNewBGDNo;
                    editComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = strNewRMCategory;
                    editComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = dRMPrice;
                    editComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strNewRMCurrency;
                    editComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = strNewDroolsEHB;
                    editComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                    editComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                    editComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                    editComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = iLineNo;

                    editComm.CommandText = @"UPDATE M_DailyGongDan SET [Lot No] = @LotNo, [RM Customs Code] = @RMCustomsCode, [BGD No] = @BGDNo, " +
                                            "[RM Category] = @RMCategory, [RM Price] = @RMPrice, [RM Currency] = @RMCurrency, [Drools EHB] = @DroolsEHB, " +
                                            "[Remark] = @Remark WHERE [GongDan No] = @GongDanNo AND [FG No] = @FGNo AND [Line No] = @LineNo";
                    editComm.ExecuteNonQuery();
                    editComm.Parameters.Clear();

                    editComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                    editComm.Parameters.Add("@OldRMCustomsCode", SqlDbType.NVarChar).Value = strOldRMCustomsCode;
                    editComm.Parameters.Add("@OldBGDNo", SqlDbType.NVarChar).Value = strOldBGDNo;
                    editComm.Parameters.Add("@OldItemNo", SqlDbType.NVarChar).Value = strOldItemNo;
                    editComm.Parameters.Add("@OldLotNo", SqlDbType.NVarChar).Value = strOldLotNo;
                    editComm.Parameters.Add("@NewRMCustomsCode", SqlDbType.NVarChar).Value = strNewRMCustomsCode;
                    editComm.Parameters.Add("@NewBGDNo", SqlDbType.NVarChar).Value = strNewBGDNo;
                    editComm.Parameters.Add("@NewItemNo", SqlDbType.NVarChar).Value = strNewItemNo;
                    editComm.Parameters.Add("@NewLotNo", SqlDbType.NVarChar).Value = strNewLotNo;
                    editComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                    editComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                    editComm.CommandText = @"INSERT INTO C_GongDanAdjustBGD([GongDan No], [RM Customs Code(old)], [BGD No(old)], [Item No(old)], [Lot No(old)], " +
                                            "[RM Customs Code(new)], [BGD No(new)], [Item No(new)], [Lot No(new)], [Remark], [Created Date]) VALUES(@GongDanNo, " +
                                            "@OldRMCustomsCode, @OldBGDNo, @OldItemNo, @OldLotNo, @NewRMCustomsCode, @NewBGDNo, @NewItemNo, @NewLotNo, @Remark, @CreatedDate)";
                    editComm.ExecuteNonQuery();
                    editComm.Parameters.Clear();
                    editComm.Dispose();
                    if (editConn.State == ConnectionState.Open)
                    {
                        editConn.Close();
                        editConn.Dispose();
                    }
                    MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
                else
                {
                    string strGongDanNo = this.dgvGongDanData["GongDan No", iRow].Value.ToString().Trim();
                    string strNewItemNo = this.dgvGongDanData["Item No", iRow].Value.ToString().Trim().ToUpper();
                    string strNewLotNo = this.dgvGongDanData["Lot No", iRow].Value.ToString().Trim().ToUpper();
                    string strNewRMCustomsCode = this.dgvGongDanData["RM Customs Code", iRow].Value.ToString().Trim();
                    string strNewBGDNo = this.dgvGongDanData["BGD No", iRow].Value.ToString().Trim();
                    string strRemark = this.dgvGongDanData["Remark", iRow].Value.ToString().Trim().ToUpper();
                    if (String.IsNullOrEmpty(strOldItemNo)) { strOldItemNo = strNewItemNo; }
                    if (String.IsNullOrEmpty(strOldLotNo)) { strOldLotNo = strNewLotNo; }
                    string strOldRMCustomsCode = null, strOldBGDNo = null;                   

                    SqlConnection editConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (editConn.State == ConnectionState.Closed) { editConn.Open(); }
                    SqlCommand editComm = new SqlCommand();
                    editComm.Connection = editConn;

                    editComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strOldItemNo;
                    editComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strOldLotNo;
                    editComm.CommandText = @"SELECT [RM Customs Code], [BGD No] FROM C_RMPurchase WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                    SqlDataReader editReader = editComm.ExecuteReader();
                    if (editReader.HasRows)
                    {
                        while (editReader.Read())
                        {
                            strOldRMCustomsCode = editReader.GetValue(0).ToString().Trim();
                            strOldBGDNo = editReader.GetValue(1).ToString().Trim();
                        }
                        editReader.Close();
                        editReader.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("The old Item No & Lot No are incorrect. Please input again.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        editReader.Close();
                        editReader.Dispose();
                        editComm.Parameters.Clear();
                        editComm.Dispose();
                        editConn.Close();
                        editConn.Dispose();
                        return;
                    }

                    this.dgvGongDanData["Remark", iRow].Value = String.Empty;
                    this.dgvGongDanData["Old Item No", iRow].Value = String.Empty;
                    this.dgvGongDanData["Old Lot No", iRow].Value = String.Empty;
                    this.dgvGongDanData["IsNewLine", iRow].Value = false;

                    editComm.Parameters.Clear();
                    editComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                    editComm.Parameters.Add("@OldRMCustomsCode", SqlDbType.NVarChar).Value = strOldRMCustomsCode;
                    editComm.Parameters.Add("@OldBGDNo", SqlDbType.NVarChar).Value = strOldBGDNo;
                    editComm.Parameters.Add("@OldItemNo", SqlDbType.NVarChar).Value = strOldItemNo;
                    editComm.Parameters.Add("@OldLotNo", SqlDbType.NVarChar).Value = strOldLotNo;
                    editComm.Parameters.Add("@NewRMCustomsCode", SqlDbType.NVarChar).Value = strNewRMCustomsCode;
                    editComm.Parameters.Add("@NewBGDNo", SqlDbType.NVarChar).Value = strNewBGDNo;
                    editComm.Parameters.Add("@NewItemNo", SqlDbType.NVarChar).Value = strNewItemNo;
                    editComm.Parameters.Add("@NewLotNo", SqlDbType.NVarChar).Value = strNewLotNo;
                    editComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark + " Is a new line";
                    editComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                    editComm.CommandText = @"INSERT INTO C_GongDanAdjustBGD([GongDan No], [RM Customs Code(old)], [BGD No(old)], [Item No(old)], [Lot No(old)], " +
                                            "[RM Customs Code(new)], [BGD No(new)], [Item No(new)], [Lot No(new)], [Remark], [Created Date]) VALUES(@GongDanNo, " +
                                            "@OldRMCustomsCode, @OldBGDNo, @OldItemNo, @OldLotNo, @NewRMCustomsCode, @NewBGDNo, @NewItemNo, @NewLotNo, @Remark, @CreatedDate)";
                    editComm.ExecuteNonQuery();
                    editComm.Parameters.Clear();
                    editComm.Dispose();
                    if (editConn.State == ConnectionState.Open)
                    {
                        editConn.Close();
                        editConn.Dispose();
                    }
                    MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (e.ColumnIndex == 3) //View Detail
            {
                if (this.dgvGongDanData.CurrentCell.ColumnIndex == this.dgvGongDanData.Columns[3].Index)
                {
                    string strRMCustomsCode = this.dgvGongDanData["RM Customs Code", this.dgvGongDanData.CurrentRow.Index].Value.ToString().Trim();
                    string strBGDNo = this.dgvGongDanData["BGD No", this.dgvGongDanData.CurrentRow.Index].Value.ToString().Trim();
                    int iCount = 0;
                    FunctionDGV_RMBalance(strRMCustomsCode, strBGDNo, out iCount);
                    dgvDetails.Width = 1042;
                    dgvDetails.Height = 42 + 23 * iCount;
                    if (dgvDetails.Height > (Int32)this.dgvGongDanData.Height / 2) { dgvDetails.Height = (Int32)this.dgvGongDanData.Height / 2; }

                    Rectangle rec = this.dgvGongDanData.GetCellDisplayRectangle(2, this.dgvGongDanData.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvGongDanData.Columns[2].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvGongDanData.Location.Y > this.dgvGongDanData.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else { dgvDetails.Top = rec.Top + this.dgvGongDanData.Location.Y; }

                    if (dgvDetails.RowCount > 0) { dgvDetails.Visible = true; }                  
                }
            }

            if (e.ColumnIndex == 12) //Lot No ComboBox Value
            {
                int iLotNo = this.dgvGongDanData.Columns["Lot No"].Index;
                if (this.dgvGongDanData.CurrentCell.ColumnIndex == iLotNo)
                {
                    string strItemNo = this.dgvGongDanData["Item No", this.dgvGongDanData.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                    string strRMEHB = this.dgvGongDanData["RM Customs Code", this.dgvGongDanData.CurrentRow.Index].Value.ToString().Trim();
                    int iCount = 0;
                    FunctionDGV_LotNo(strItemNo, strRMEHB, out iCount);
                    dgvDetails.Width = 206;
                    dgvDetails.Height = 23 * ++ iCount;
                    if (dgvDetails.Height > (Int32)this.dgvGongDanData.Height / 2) { dgvDetails.Height = (Int32)this.dgvGongDanData.Height / 2; }

                    Rectangle rec = this.dgvGongDanData.GetCellDisplayRectangle(11, this.dgvGongDanData.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvGongDanData.Columns[11].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvGongDanData.Location.Y > this.dgvGongDanData.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else
                    { dgvDetails.Top = rec.Top + this.dgvGongDanData.Location.Y; }

                    if (dgvDetails.RowCount > 0) { dgvDetails.Visible = true; }
                }
            }

            if (e.ColumnIndex != 3 && e.ColumnIndex != 12)
            {
                if (dgvDetails.Visible == true) 
                { dgvDetails.Visible = false; }
            }
        }

        private void CtrlColumnReadWrite()
        {
            for (int i = 4; i < this.dgvGongDanData.ColumnCount - 6; i++)
            { this.dgvGongDanData.Columns[i].ReadOnly = true; }      
            this.dgvGongDanData.Columns["Lot No"].ReadOnly = false;
            this.dgvGongDanData.Columns["Remark"].ReadOnly = false;
        }

        private void DGV_Details_CellClick(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.CurrentCell.ColumnIndex == this.dgvGongDanData.Columns[3].Index)
            {
                string strRMCustomsCode = dgvDetails["RM Customs Code", dgvDetails.CurrentRow.Index].Value.ToString().Trim();
                string strBGDNo = dgvDetails["BGD No", dgvDetails.CurrentRow.Index].Value.ToString().Trim();
                QueryRMBalance queryRMBalance = QueryRMBalance.CreateInstance();
                queryRMBalance.getRMCustomsCode = strRMCustomsCode;
                queryRMBalance.getBGDNo = strBGDNo;
                queryRMBalance.Show();
            }

            int iLotNo = this.dgvGongDanData.Columns["Lot No"].Index;
            if (this.dgvGongDanData.CurrentCell != null && this.dgvGongDanData.CurrentCell.ColumnIndex == iLotNo)
            {
                string strLotNo = dgvDetails["Lot No", dgvDetails.CurrentCell.RowIndex].Value.ToString().Trim();
                this.dgvGongDanData[iLotNo, this.dgvGongDanData.CurrentCell.RowIndex].Value = strLotNo;
            }
            dgvDetails.Visible = false;
        }
         
        private void FunctionDGV_RMBalance(string strRMCustomsCode, string strBGDNo, out int iCount)
        {
            SqlLib sqlLib = new SqlLib();
            string strSQL = @"SELECT T1.*, T2.[Available RM Balance] FROM (SELECT M1.*, M2.[Total RM Qty] FROM (SELECT [RM Customs Code], [BGD No], [RM Used Qty], " +
                             "[GongDan No], [FG No], [Line No], [Item No], [Lot No] FROM M_DailyGongDan) AS M1, (SELECT [RM Customs Code], [BGD No], SUM([RM Used Qty]) " +
                             "AS [Total RM Qty] FROM M_DailyGongDan GROUP BY [RM Customs Code], [BGD No]) AS M2 WHERE M2.[RM Customs Code] = M1.[RM Customs Code] AND " +
                             "M2.[BGD No] = M1.[BGD No] AND M1.[RM Customs Code] = '" + strRMCustomsCode + "' AND M1.[BGD No] = '" + strBGDNo + "') AS T1 LEFT JOIN " +
                             "C_RMBalance AS T2 ON T2.[RM Customs Code] = T1.[RM Customs Code] AND T2.[BGD No] = T1.[BGD No] ORDER BY T1.[RM Customs Code], " + 
                             "T1.[BGD No], T1.[GongDan No], T1.[Line No]";

            DataTable dataTable = sqlLib.GetDataTable(strSQL).Copy();
            sqlLib.Dispose();
            iCount = dataTable.Rows.Count;

            dataTable.Columns["Available RM Balance"].SetOrdinal(2);
            dataTable.Columns["Total RM Qty"].SetOrdinal(3);
            dgvDetails.DataSource = dataTable;
            this.dgvGongDanData.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);
        }

        private void FunctionDGV_LotNo(string strItemNo, string strRMEHB, out int iCount)
        {
            SqlLib sqllib = new SqlLib();
            DataTable lotnoTable = sqllib.GetDataTable(@"SELECT DISTINCT [Lot No] FROM C_RMPurchase WHERE [Item No] = '" + strItemNo + "' AND [RM Customs Code] = '" + strRMEHB + "'", "C_RMPurchase").Copy();
            sqllib.Dispose();
            iCount = lotnoTable.Rows.Count;

            dgvDetails.DataSource = lotnoTable;
            this.dgvGongDanData.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);
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

                    dvFillDGV.RowFilter = strFilter;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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

                    dvFillDGV.RowFilter = strFilter;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
        }

        private void tsmiCheckPrice_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim()))
                { strFilter += " AND [Judge Price] = True"; }
                else
                { strFilter = "[Judge Price] = True"; }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanData.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.LightPink;
            }
        }

        private void tsmiCheckRcvdDate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim()))
                { strFilter += " AND [Judge Rcvd Date] = True"; }
                else
                { strFilter = "[Judge Rcvd Date] = True"; }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanData.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
            }
        }

        private void tsmiCheckBalance_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim()))
                { strFilter += " AND [Judge RM Bal] = True"; }
                else
                { strFilter = "[Judge RM Bal] = True"; }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            for (int i = 0; i < this.dgvGongDanData.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanData.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.Khaki;
            }
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.CurrentCell != null)
            {
                string strColumnName = this.dgvGongDanData.Columns[this.dgvGongDanData.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter);
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dvFillDGV.ToTable(true, new string[] { strColumnName });
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
                dvFillDGV.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiBlankFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanData.Rows.Count > 0)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    { strFilter += @" AND [Order Price] = 0.0 OR ([RM Customs Code] = '' OR [RM Customs Code] IS NULL OR [BGD No] = '' OR [BGD No] IS NULL)"; }
                    else
                    { strFilter = @"[Order Price] = 0.0 OR ([RM Customs Code] = '' OR [RM Customs Code] IS NULL OR [BGD No] = '' OR [BGD No] IS NULL)"; }
                    dvFillDGV.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }
    }
}
