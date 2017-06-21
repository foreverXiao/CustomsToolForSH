using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class DataConversion : Form
    {
        private LoginForm loginFrm = new LoginForm();
        DataTable browseTbl = new DataTable();
        SqlLib sqlLib = new SqlLib();
        private SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
        private static DataConversion DataConversionFrm;
        public static DataConversion CreateInstance()
        {
            if (DataConversionFrm == null || DataConversionFrm.IsDisposed)
            {
                DataConversionFrm = new DataConversion();
            }
            return DataConversionFrm;
        }
        public DataConversion()
        {
            InitializeComponent();
        }

        private void DataConversion_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;
            string strUserName = loginFrm.PublicUserName;
            SqlComm.Parameters.Add("@LoginName", SqlDbType.NVarChar).Value = strUserName;
            SqlComm.CommandText = @"SELECT [Group] FROM B_UserInfo WHERE [LoginName] = @LoginName";
            string strGroup = Convert.ToString(SqlComm.ExecuteScalar()).Trim().ToUpper();
            if (String.Compare(strGroup, "ADMIN") == 0) { this.gBox.Visible = true; }
            SqlComm.Dispose();
            SqlConn.Close();
        }

        private void DataConversion_FormClosing(object sender, FormClosingEventArgs e)
        {
            browseTbl.Dispose();
            SqlConn.Dispose();
            sqlLib.Dispose(0);
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
                this.txtPath.Focus();
                return;
            }

            if (String.IsNullOrEmpty(this.cmbObject.Text.ToString().Trim()))
            {
                MessageBox.Show("Please select Object first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbObject.Focus();
                return;
            }

            string strConn = null;
            if (this.txtPath.Text.Contains(".xlsx"))
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + this.txtPath.Text.Trim() + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=0'"; }
            else
            { strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.txtPath.Text.Trim() + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=0'"; }

            OleDbConnection MyConn = new OleDbConnection(strConn);
            MyConn.Open();
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", MyConn);
            DataTable myTable = new DataTable();
            myDataAdapter.Fill(myTable);
            myDataAdapter.Dispose();
            MyConn.Close();
            MyConn.Dispose();

            if (myTable.Rows.Count > 0)
            {
                if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
                SqlCommand SqlComm = new SqlCommand();
                SqlComm.Connection = SqlConn;

                string strSQL = null;
                if (String.Compare(this.cmbObject.Text.Trim(), "BOM Detail") == 0) { strSQL = @"SELECT * FROM C_BOMDetail"; }
                else if (String.Compare(this.cmbObject.Text.Trim(), "BOM Master") == 0) { strSQL = @"SELECT * FROM C_BOM"; }
                else if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Detail") == 0) { strSQL = @"SELECT * FROM C_GongDanDetail"; }
                else if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Master") == 0) { strSQL = @"SELECT * FROM C_GongDan"; }
                else if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") == 0) { strSQL = @"SELECT * FROM C_BeiAnDan"; }
                else if (String.Compare(this.cmbObject.Text.Trim(), "Drools BeiAnDan") == 0) { strSQL = @"SELECT * FROM C_BeiAnDan_Drools"; }
                else if (String.Compare(this.cmbObject.Text.Trim(), "PingDan") == 0) { strSQL = @"SELECT * FROM C_PingDan"; }
                else { strSQL = @"SELECT * FROM B_OrderFulfillment"; }

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, SqlConn);
                DataTable dtName = new DataTable();
                sqlAdapter.Fill(dtName);
                sqlAdapter.Dispose();

                #region //Upload data to C_BOMDetail table
                if (String.Compare(this.cmbObject.Text.Trim(), "BOM Detail") == 0)
                {
                    string strBatchNo = null;
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        if (String.Compare(strBatchNo, myTable.Rows[i]["Batch No"].ToString().Trim().ToUpper()) != 0)
                        {
                            strBatchNo = myTable.Rows[i]["Batch No"].ToString().Trim().ToUpper();
                            DataRow[] dRow = dtName.Select("[Batch No] = '" + strBatchNo + "'");
                            if (dRow.Length > 0)
                            {
                                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                                SqlComm.CommandText = @"DELETE FROM C_BOMDetail WHERE [Batch No] = @BatchNo";
                                SqlComm.ExecuteNonQuery();
                                SqlComm.Parameters.RemoveAt("@BatchNo");
                            }
                        }

                        SqlComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = myTable.Rows[i]["Batch Path"].ToString().Trim();
                        SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Inventory Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Category"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Customs Code"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                        string strRMQty = myTable.Rows[i]["RM Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strRMQty)) { SqlComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strRMQty))), 7); }
                        SqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Currency"].ToString().Trim().ToUpper();
                        string strRMPrice = myTable.Rows[i]["RM Price"].ToString().Trim();
                        if (String.IsNullOrEmpty(strRMPrice)) { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strRMPrice))), 6); }
                        string strConsump = myTable.Rows[i]["Consumption"].ToString().Trim();
                        if (String.IsNullOrEmpty(strConsump)) { SqlComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strConsump))), 6); }
                        SqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = myTable.Rows[i]["Drools EHB"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@Note", SqlDbType.NVarChar).Value = myTable.Rows[i]["Note"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Batch No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Line No"].ToString().Trim());

                        SqlComm.CommandText = @"INSERT INTO C_BOMDetail([Batch Path], [Item No], [Lot No], [Inventory Type], [RM Category], [RM Customs Code], " +
                                               "[BGD No], [RM Qty], [RM Currency], [RM Price], [Consumption], [Drools EHB], [Note], [Batch No], [Line No]) " +
                                               "VALUES(@BatchPath, @ItemNo, @LotNo, @InventoryType, @RMCategory, @RMCustomsCode, @BGDNo, @RMQty, @RMCurrency, " +
                                               "@RMPrice, @Consumption, @DroolsEHB, @Note, @BatchNo, @LineNo)";
                        SqlTransaction SqlTransBOMDetail = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransBOMDetail;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransBOMDetail.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransBOMDetail.Rollback();
                            SqlTransBOMDetail.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                #region //Upload data to C_BOM table
                if (String.Compare(this.cmbObject.Text.Trim(), "BOM Master") == 0)
                {
                    string strBatchNo = null, strFGNo = null;
                    for (int j = 0; j < myTable.Rows.Count; j++)
                    {
                        if (String.Compare(strBatchNo, myTable.Rows[j]["Batch No"].ToString().Trim().ToUpper()) != 0 ||
                            String.Compare(strFGNo, myTable.Rows[j]["FG No"].ToString().Trim().ToUpper()) != 0)
                        {
                            strBatchNo = myTable.Rows[j]["Batch No"].ToString().Trim().ToUpper();
                            strFGNo = myTable.Rows[j]["FG No"].ToString().Trim().ToUpper();
                            DataRow[] dRow = dtName.Select("[Batch No] = '" + strBatchNo + "' AND [FG No] = '" + strFGNo + "'");
                            if (dRow.Length > 0)
                            {
                                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                                SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                                SqlComm.CommandText = @"DELETE FROM C_BOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                                SqlComm.ExecuteNonQuery();
                                SqlComm.Parameters.Clear();
                            }
                        }

                        SqlComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = myTable.Rows[j]["Actual Start Date"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = myTable.Rows[j]["Actual Close Date"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@BOMinCustoms", SqlDbType.NVarChar).Value = myTable.Rows[j]["BOM In Customs"].ToString().Trim().ToUpper();
                        string strFGQty = myTable.Rows[j]["FG Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strFGQty)) { SqlComm.Parameters.Add("@FGQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@FGQty", SqlDbType.Int).Value = Convert.ToInt32(strFGQty); }
                        string strOrderPrice = myTable.Rows[j]["Order Price"].ToString().Trim();
                        if (String.IsNullOrEmpty(strOrderPrice)) { SqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strOrderPrice), 4); }
                        SqlComm.Parameters.Add("@OrderCurrency", SqlDbType.NVarChar).Value = myTable.Rows[j]["Order Currency"].ToString().Trim().ToUpper();
                        string strTotalInput = myTable.Rows[j]["Total Input Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strTotalInput)) { SqlComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strTotalInput))), 11); }
                        string strTotalRMCost = myTable.Rows[j]["Total RM Cost(USD)"].ToString().Trim();
                        if (String.IsNullOrEmpty(strTotalRMCost)) { SqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strTotalRMCost))), 2); }
                        SqlComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = myTable.Rows[j]["HS Code"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = myTable.Rows[j]["CHN Name"].ToString().Trim().ToUpper();
                        string strDroolsRate = myTable.Rows[j]["Qty Loss Rate"].ToString().Trim();
                        if (String.IsNullOrEmpty(strDroolsRate)) { SqlComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strDroolsRate))), 6); }
                        string strDroolsQty = myTable.Rows[j]["Drools Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strDroolsQty)) { SqlComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strDroolsQty))), 11); }
                        string strCreatedDate = myTable.Rows[j]["Created Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strCreatedDate)) { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCreatedDate); }
                        SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = "Data Conversion";
                        string strGongDanUsedQty = myTable.Rows[j]["GongDan Used Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strGongDanUsedQty)) { SqlComm.Parameters.Add("@GongDanUsedQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@GongDanUsedQty", SqlDbType.Int).Value = Convert.ToInt32(strGongDanUsedQty); }
                        if (String.IsNullOrEmpty(strCreatedDate)) { SqlComm.Parameters.Add("@ApprovedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy")); }
                        else { SqlComm.Parameters.Add("@ApprovedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCreatedDate); }
                        SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[j]["Remark"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[j]["Remark Date"].ToString().Trim())) { SqlComm.Parameters.Add("@RemarkDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@RemarkDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[j]["Remark Date"].ToString().Trim()); }
                        string strFreeze = myTable.Rows[j]["Freeze"].ToString().Trim();
                        if (String.IsNullOrEmpty(strFreeze) || String.Compare(strFreeze.ToUpper(), "FALSE") == 0) { SqlComm.Parameters.Add("@Freeze", SqlDbType.Bit).Value = false; }
                        else { SqlComm.Parameters.Add("@Freeze", SqlDbType.Bit).Value = true; }
                        SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Batch No"].ToString().Trim();
                        SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["FG No"].ToString().Trim().ToUpper();

                        SqlComm.CommandText = @"INSERT INTO C_BOM([Actual Start Date], [Actual Close Date], [BOM In Customs], [FG Qty], [Order Price], " +
                                               "[Order Currency], [Total Input Qty], [Total RM Cost(USD)], [HS Code], [CHN Name], [Qty Loss Rate], [Drools Qty], " +
                                               "[Created Date], [Creater], [GongDan Used Qty], [Approved Date], [Remark], [Remark Date], [Freeze], [Batch No], " +
                                               "[FG No]) VALUES(@ActualStartDate, @ActualCloseDate, @BOMinCustoms, @FGQty, @OrderPrice, @OrderCurrency, " +
                                               "@TotalInputQty, @TotalRMCost, @HSCode, @CHNName, @QtyLossRate, @DroolsQty, @CreatedDate, @Creater, " +
                                               "@GongDanUsedQty, @ApprovedDate, @Remark, @RemarkDate, @Freeze, @BatchNo, @FGNo)";
                        SqlTransaction SqlTransBOMMaster = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransBOMMaster;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransBOMMaster.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransBOMMaster.Rollback();
                            SqlTransBOMMaster.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                #region //Upload data to C_GongDanDetail table
                if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Detail") == 0)
                {
                    string strGongDanNo = null;
                    for (int x = 0; x < myTable.Rows.Count; x++)
                    {
                        if (String.Compare(strGongDanNo, myTable.Rows[x]["GongDan No"].ToString().Trim().ToUpper()) != 0)
                        {
                            strGongDanNo = myTable.Rows[x]["GongDan No"].ToString().Trim().ToUpper();
                            DataRow[] dRow = dtName.Select("[GongDan No] = '" + strGongDanNo + "'");
                            if (dRow.Length > 0)
                            {
                                SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                                SqlComm.CommandText = @"DELETE FROM C_GongDanDetail WHERE [GongDan No] = @GongDanNo";
                                SqlComm.ExecuteNonQuery();
                                SqlComm.Parameters.RemoveAt("@GongDanNo");
                            }
                        }

                        SqlComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = myTable.Rows[x]["Batch Path"].ToString().Trim();
                        SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[x]["Item No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[x]["Lot No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = myTable.Rows[x]["Inventory Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = myTable.Rows[x]["RM Category"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = myTable.Rows[x]["RM Customs Code"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = myTable.Rows[x]["BGD No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = myTable.Rows[x]["RM Currency"].ToString().Trim().ToUpper();
                        string strRMPrice = myTable.Rows[x]["RM Price"].ToString().Trim();
                        if (String.IsNullOrEmpty(strRMPrice)) { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strRMPrice))), 6); }
                        string strConsump = myTable.Rows[x]["Consumption"].ToString().Trim();
                        if (String.IsNullOrEmpty(strConsump)) { SqlComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strConsump))), 6); }
                        string strRMUsedQty = myTable.Rows[x]["RM Used Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strRMUsedQty)) { SqlComm.Parameters.Add("@RMUsedQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@RMUsedQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strRMUsedQty))), 7); }
                        string strDroolsQuota = myTable.Rows[x]["Drools Quota"].ToString().Trim();
                        if (String.IsNullOrEmpty(strDroolsQuota)) { SqlComm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@DroolsQuota", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strDroolsQuota))), 8); }
                        SqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = myTable.Rows[x]["Drools EHB"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = myTable.Rows[x]["GongDan No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[x]["Line No"].ToString().Trim());

                        SqlComm.CommandText = @"INSERT INTO C_GongDanDetail([Batch Path], [Item No], [Lot No], [Inventory Type], [RM Category], [RM Customs Code], " +
                                               "[BGD No], [RM Currency], [RM Price], [Consumption], [RM Used Qty], [Drools Quota], [Drools EHB], [GongDan No], [Line No]) " +
                                               "VALUES( @BatchPath, @ItemNo, @LotNo, @InventoryType, @RMCategory, @RMCustomsCode, @BGDNo, @RMCurrency, @RMPrice, " +
                                               "@Consumption, @RMUsedQty, @DroolsQuota, @DroolsEHB, @GongDanNo, @LineNo)";
                        SqlTransaction SqlTransGDDetail = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransGDDetail;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransGDDetail.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransGDDetail.Rollback();
                            SqlTransGDDetail.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                #region //Upload data to C_GongDan table
                if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Master") == 0)
                {
                    string strGongDanNo = null, strFGNo = null;
                    for (int y = 0; y < myTable.Rows.Count; y++)
                    {
                        if (String.Compare(strGongDanNo, myTable.Rows[y]["GongDan No"].ToString().Trim().ToUpper()) != 0 ||
                            String.Compare(strFGNo, myTable.Rows[y]["FG No"].ToString().Trim().ToUpper()) != 0)
                        {
                            strGongDanNo = myTable.Rows[y]["GongDan No"].ToString().Trim().ToUpper();
                            strFGNo = myTable.Rows[y]["FG No"].ToString().Trim().ToUpper();
                            DataRow[] dRow = dtName.Select("[GongDan No] = '" + strGongDanNo + "' AND [FG No] = '" + strFGNo + "'");
                            if (dRow.Length > 0)
                            {
                                SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                                SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                                SqlComm.CommandText = @"DELETE FROM C_GongDan WHERE [GongDan No] = @GongDanNo AND [FG No] = @FGNo";
                                SqlComm.ExecuteNonQuery();
                                SqlComm.Parameters.Clear();
                            }
                        }

                        string strGongDan = myTable.Rows[y]["GongDan No"].ToString().Trim().ToUpper();
                        string strCreatedDate = myTable.Rows[y]["Created Date"].ToString().Trim();
                        SqlComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = myTable.Rows[y]["Actual Start Date"].ToString().Trim();
                        SqlComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = myTable.Rows[y]["Actual Close Date"].ToString().Trim();
                        SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = myTable.Rows[y]["Batch No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@BOMinCustoms", SqlDbType.NVarChar).Value = myTable.Rows[y]["BOM In Customs"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@ESSLINE", SqlDbType.NVarChar).Value = myTable.Rows[y]["ESS/LINE"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = myTable.Rows[y]["Order No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = myTable.Rows[y]["IE Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OrderCategory", SqlDbType.NVarChar).Value = myTable.Rows[y]["Order Category"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = myTable.Rows[y]["Destination"].ToString().Trim().ToUpper();
                        string strTotalShipQty = myTable.Rows[y]["Total Ship Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strTotalShipQty)) { SqlComm.Parameters.Add("@TotalShipQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@TotalShipQty", SqlDbType.Int).Value = Convert.ToInt32(strTotalShipQty); }
                        string strGongDanQty = myTable.Rows[y]["GongDan Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strGongDanQty)) { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGongDanQty); }
                        string strOrderPrice = myTable.Rows[y]["Order Price"].ToString().Trim();
                        if (String.IsNullOrEmpty(strOrderPrice)) { SqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strOrderPrice), 4); }
                        SqlComm.Parameters.Add("@OrderCurrency", SqlDbType.NVarChar).Value = myTable.Rows[y]["Order Currency"].ToString().Trim().ToUpper();
                        string strTotalRMQty = myTable.Rows[y]["Total RM Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strTotalRMQty)) { SqlComm.Parameters.Add("@TotalRMQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@TotalRMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strTotalRMQty))), 6); }
                        string strTotalRMCost = myTable.Rows[y]["Total RM Cost(USD)"].ToString().Trim();
                        if (String.IsNullOrEmpty(strTotalRMCost)) { SqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strTotalRMCost))), 2); }
                        string strDroolsRate = myTable.Rows[y]["Drools Rate"].ToString().Trim();
                        if (String.IsNullOrEmpty(strDroolsRate)) { SqlComm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@DroolsRate", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strDroolsRate))), 6); }
                        SqlComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = myTable.Rows[y]["CHN Name"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(strCreatedDate)) { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCreatedDate); }
                        if (strGongDan.Length > 8 && strGongDan.Substring(8, 1).Trim() == "-")
                        { SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName; }
                        else { SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = "Data Conversion"; }
                        if (String.IsNullOrEmpty(myTable.Rows[y]["BeiAnDan Used Qty"].ToString().Trim())) { SqlComm.Parameters.Add("@BeiAnDanUsedQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@BeiAnDanUsedQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[y]["BeiAnDan Used Qty"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(strCreatedDate)) { SqlComm.Parameters.Add("@ApprovedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy")); }
                        else { SqlComm.Parameters.Add("@ApprovedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCreatedDate); }                       
                        SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[y]["Remark"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[y]["Remark Date"].ToString().Trim())) { SqlComm.Parameters.Add("@RemarkDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@RemarkDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[y]["Remark Date"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[y]["PC Item"].ToString().Trim()) || String.Compare(myTable.Rows[y]["PC Item"].ToString().Trim().ToUpper(), "FALSE") == 0)
                        { SqlComm.Parameters.Add("@PCItem", SqlDbType.Bit).Value = false; }
                        else { SqlComm.Parameters.Add("@PCItem", SqlDbType.Bit).Value = true; }
                        SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;
                        SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = myTable.Rows[y]["FG No"].ToString().Trim().ToUpper();

                        SqlComm.CommandText = @"INSERT INTO C_GongDan([Actual Start Date], [Actual Close Date], [Batch No], [BOM In Customs], [ESS/LINE], [Order No], " +
                                               "[IE Type], [Order Category], [Destination], [Total Ship Qty], [GongDan Qty], [Order Price], [Order Currency], " +
                                               "[Total RM Qty], [Total RM Cost(USD)], [Drools Rate], [CHN Name], [Created Date], [Creater], [BeiAnDan Used Qty], " +
                                               "[Approved Date], [Remark], [Remark Date], [PC Item], [GongDan No], [FG No]) VALUES(@ActualStartDate, @ActualCloseDate, " +
                                               "@BatchNo, @BOMinCustoms, @ESSLINE, @OrderNo, @IEType, @OrderCategory, @Destination, @TotalShipQty, @GongDanQty, " +
                                               "@OrderPrice, @OrderCurrency, @TotalRMQty, @TotalRMCost, @DroolsRate, @CHNName, @CreatedDate, @Creater, @BeiAnDanUsedQty, " +
                                               "@ApprovedDate, @Remark, @RemarkDate, @PCItem, @GongDanNo, @FGNo)";
                        SqlTransaction SqlTransGDMaster = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransGDMaster;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransGDMaster.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransGDMaster.Rollback();
                            SqlTransGDMaster.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                #region //Upload data to C_BeiAnDan table
                if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") == 0)
                {
                    for (int m = 0; m < myTable.Rows.Count; m++)
                    {
                        string strGD = null;
                        if (String.Compare(strGD, myTable.Rows[m]["GongDan No"].ToString().Trim().ToUpper()) != 0)
                        {
                            strGD = myTable.Rows[m]["GongDan No"].ToString().Trim().ToUpper();
                            DataRow[] row = dtName.Select("[GongDan No] = '" + strGD + "'");
                            if (row.Length > 0)
                            {
                                SqlComm.Parameters.Add("@GD", SqlDbType.NVarChar).Value = strGD;
                                SqlComm.CommandText = @"DELETE FROM C_BeiAnDan WHERE [GongDan No] = @GD";
                                SqlComm.ExecuteNonQuery();
                                SqlComm.Parameters.RemoveAt("@GD");
                            }
                        }

                        SqlComm.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = myTable.Rows[m]["Group ID"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = myTable.Rows[m]["IE Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = myTable.Rows[m]["GongDan No"].ToString().Trim().ToUpper();
                        string strTotalShipQty = myTable.Rows[m]["Total Ship Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strTotalShipQty)) { SqlComm.Parameters.Add("@TotalShipQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@TotalShipQty", SqlDbType.Int).Value = Convert.ToInt32(strTotalShipQty); }
                        string strGongDanQty = myTable.Rows[m]["GongDan Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strGongDanQty)) { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGongDanQty); }
                        SqlComm.Parameters.Add("@FGEHB", SqlDbType.NVarChar).Value = myTable.Rows[m]["FG EHB"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = myTable.Rows[m]["CHN Name"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = myTable.Rows[m]["Order No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OrderCategory", SqlDbType.NVarChar).Value = myTable.Rows[m]["Order Category"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@EssLine", SqlDbType.NVarChar).Value = myTable.Rows[m]["ESS/LINE"].ToString().Trim().ToUpper();
                        string strSellingPrice = myTable.Rows[m]["Selling Price"].ToString().Trim();
                        if (String.IsNullOrEmpty(strSellingPrice)) { SqlComm.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strSellingPrice), 2); }
                        SqlComm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = myTable.Rows[m]["Currency"].ToString().Trim().ToUpper();
                        string strLocalTotalRMCost = myTable.Rows[m]["Local Total RM Cost"].ToString().Trim();
                        if (String.IsNullOrEmpty(strLocalTotalRMCost)) { SqlComm.Parameters.Add("@LocalTotalRMCost", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@LocalTotalRMCost", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strLocalTotalRMCost))), 2); }
                        string strSellingAmount = myTable.Rows[m]["Selling Amount"].ToString().Trim();
                        if (String.IsNullOrEmpty(strSellingAmount)) { SqlComm.Parameters.Add("@SellingAmount", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@SellingAmount", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strSellingAmount), 2); }
                        SqlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = myTable.Rows[m]["Destination"].ToString().Trim().ToUpper();
                        string strPassToIEDate = myTable.Rows[m]["Pass to IE Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strPassToIEDate)) { SqlComm.Parameters.Add("@PassToIEDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@PassToIEDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strPassToIEDate); }
                        string strGDApprovedDate = myTable.Rows[m]["GongDan Approved Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strGDApprovedDate)) { SqlComm.Parameters.Add("@GDApprovedDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@GDApprovedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGDApprovedDate); }
                        string strIERevAmt = myTable.Rows[m]["IE Rev Amt"].ToString().Trim();
                        if (String.IsNullOrEmpty(strIERevAmt)) { SqlComm.Parameters.Add("@IERevAmt", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@IERevAmt", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strIERevAmt), 2); }
                        string strOFRevAmt = myTable.Rows[m]["OF Rev Amt"].ToString().Trim();
                        if (String.IsNullOrEmpty(strOFRevAmt)) { SqlComm.Parameters.Add("@OFRevAmt", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@OFRevAmt", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strOFRevAmt), 2); }
                        string strCustomsTotalRMCost = myTable.Rows[m]["Customs Total RM Cost"].ToString().Trim();
                        if (String.IsNullOrEmpty(strCustomsTotalRMCost)) { SqlComm.Parameters.Add("@CustomsTotalRMCost", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@CustomsTotalRMCost", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strCustomsTotalRMCost), 2); }
                        SqlComm.Parameters.Add("@BeiAnDanID", SqlDbType.NVarChar).Value = myTable.Rows[m]["BeiAnDan ID"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@BeiAnDanNo", SqlDbType.NVarChar).Value = myTable.Rows[m]["BeiAnDan No"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[m]["BeiAnDan Date"].ToString().Trim())) { SqlComm.Parameters.Add("@BeiAnDanDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@BeiAnDanDate", SqlDbType.DateTime).Value = myTable.Rows[m]["BeiAnDan Date"].ToString().Trim().ToUpper(); }
                        SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = "Data Conversion";
                        SqlComm.Parameters.Add("@TaxDutyPaidDate", SqlDbType.NVarChar).Value = myTable.Rows[m]["Tax & Duty Paid Date"].ToString().Trim();
                        SqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.NVarChar).Value = myTable.Rows[m]["Customs Release Date"].ToString().Trim();
                        SqlComm.Parameters.Add("@IERemark", SqlDbType.NVarChar).Value = myTable.Rows[m]["IE Remark"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OFRemark", SqlDbType.NVarChar).Value = myTable.Rows[m]["OF Remark"].ToString().Trim().ToUpper();

                        SqlComm.CommandText = @"INSERT INTO C_BeiAnDan([Group ID], [IE Type], [GongDan No], [Total Ship Qty], [GongDan Qty], [FG EHB], [CHN Name], " +
                                               "[Order No], [Order Category], [ESS/LINE], [Selling Price], [Currency], [Local Total RM Cost], [Selling Amount], " +
                                               "[Destination], [Pass to IE Date], [GongDan Approved Date], [IE Rev Amt], [OF Rev Amt], [Customs Total RM Cost], " +
                                               "[BeiAnDan ID], [BeiAnDan No], [BeiAnDan Date], [Creater], [Tax & Duty Paid Date], [Customs Release Date], [IE Remark], " +
                                               "[OF Remark]) VALUES(@GroupID, @IEtype, @GongDanNo, @TotalShipQty, @GongDanQty, @FGEHB, @CHNName, @OrderNo, " +
                                               "@OrderCategory, @EssLine, @SellingPrice, @Currency, @LocalTotalRMCost, @SellingAmount, @Destination, @PassToIEDate, " +
                                               "@GDApprovedDate, @IERevAmt, @OFRevAmt, @CustomsTotalRMCost, @BeiAnDanID, @BeiAnDanNo, @BeiAnDanDate, @Creater, " +
                                               "@TaxDutyPaidDate, @CustomsReleaseDate, @IERemark, @OFRemark)";
                        SqlTransaction SqlTransBeiAnDan = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransBeiAnDan;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransBeiAnDan.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransBeiAnDan.Rollback();
                            SqlTransBeiAnDan.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                #region //Upload data to C_BeiAnDan_Drools table
                if (String.Compare(this.cmbObject.Text.Trim(), "Drools BeiAnDan") == 0)
                {
                    for (int k = 0; k < myTable.Rows.Count; k++)
                    {
                        string strGD = null;
                        if (String.Compare(strGD, myTable.Rows[k]["GongDan No"].ToString().Trim().ToUpper()) != 0)
                        {
                            strGD = myTable.Rows[k]["GongDan No"].ToString().Trim().ToUpper();
                            DataRow[] row = dtName.Select("[GongDan No] = '" + strGD + "'");
                            if (row.Length > 0)
                            {
                                SqlComm.Parameters.Add("@GD", SqlDbType.NVarChar).Value = strGD;
                                SqlComm.CommandText = @"DELETE FROM C_BeiAnDan_Drools WHERE [GongDan No] = @GD";
                                SqlComm.ExecuteNonQuery();
                                SqlComm.Parameters.RemoveAt("@GD");
                            }
                        }

                        SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = myTable.Rows[k]["GongDan No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@FGEHB", SqlDbType.NVarChar).Value = myTable.Rows[k]["FG EHB"].ToString().Trim().ToUpper();
                        string strDroolsQty = myTable.Rows[k]["Drools Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strDroolsQty)) { SqlComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(sqlLib.doubleFormat(Double.Parse(strDroolsQty))), 8); }
                        SqlComm.Parameters.Add("@DroolsCHNName", SqlDbType.NVarChar).Value = myTable.Rows[k]["Drools CHN Name"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = myTable.Rows[k]["Drools EHB"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@DroolsBeiAnDanID", SqlDbType.NVarChar).Value = myTable.Rows[k]["Drools BeiAnDan ID"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@DroolsBeiAnDanNo", SqlDbType.NVarChar).Value = myTable.Rows[k]["Drools BeiAnDan No"].ToString().Trim().ToUpper();
                        string strDroolsBADDate = myTable.Rows[k]["Drools BeiAnDan Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strDroolsBADDate)) { SqlComm.Parameters.Add("@DroolsBeiAnDanDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@DroolsBeiAnDanDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strDroolsBADDate); }
                        string strTaxDutyDate = myTable.Rows[k]["Tax & Duty Paid Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strTaxDutyDate)) { SqlComm.Parameters.Add("@TaxDutyPaidDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@TaxDutyPaidDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strTaxDutyDate); }
                        string strCustomsReleaseDate = myTable.Rows[k]["Customs Release Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strCustomsReleaseDate)) { SqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCustomsReleaseDate); }

                        SqlComm.CommandText = @"INSERT INTO C_BeiAnDan_Drools([GongDan No], [FG EHB], [Drools Qty], [Drools CHN Name], [Drools EHB], " +
                                               "[Drools BeiAnDan ID], [Drools BeiAnDan No], [Drools BeiAnDan Date], [Tax & Duty Paid Date], [Customs Release Date]) " +
                                               "VALUES(@GongDanNo, @FGEHB, @DroolsQty, @DroolsCHNName, @DroolsEHB, @DroolsBeiAnDanID, @DroolsBeiAnDanNo, " +
                                               "@DroolsBeiAnDanDate, @TaxDutyPaidDate, @CustomsReleaseDate)";
                        SqlTransaction SqlTransDroolsBAD = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransDroolsBAD;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransDroolsBAD.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransDroolsBAD.Rollback();
                            SqlTransDroolsBAD.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                #region //Upload data to C_PingDan table
                if (String.Compare(this.cmbObject.Text.Trim(), "PingDan") == 0)
                {
                    for (int n = 0; n < myTable.Rows.Count; n++)
                    {
                        string strGD = null, strGroupID = null;
                        if (String.Compare(strGD, myTable.Rows[n]["GongDan No"].ToString().Trim().ToUpper()) != 0 ||
                            String.Compare(strGroupID, myTable.Rows[n]["Group ID"].ToString().Trim().ToUpper()) != 0)
                        {
                            strGD = myTable.Rows[n]["GongDan No"].ToString().Trim().ToUpper();
                            strGroupID = myTable.Rows[n]["Group ID"].ToString().Trim().ToUpper();
                            DataRow[] row = dtName.Select("[GongDan No] = '" + strGD + "' AND [Group ID] = '" + strGroupID + "'");
                            if (row.Length > 0)
                            {
                                SqlComm.Parameters.Add("@GD", SqlDbType.NVarChar).Value = strGD;
                                SqlComm.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = strGroupID;
                                SqlComm.CommandText = @"DELETE FROM C_PingDan WHERE [GongDan No] = @GD AND [Group ID] = @GroupID";
                                SqlComm.ExecuteNonQuery();
                                SqlComm.Parameters.Clear();
                            }
                        }

                        SqlComm.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = myTable.Rows[n]["IE Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@BeiAnDanID", SqlDbType.NVarChar).Value = myTable.Rows[n]["BeiAnDan ID"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = myTable.Rows[n]["GongDan No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OrderCategory", SqlDbType.NVarChar).Value = myTable.Rows[n]["Order Category"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@EssLine", SqlDbType.NVarChar).Value = myTable.Rows[n]["ESS/LINE"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = myTable.Rows[n]["Order No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@FGEHB", SqlDbType.NVarChar).Value = myTable.Rows[n]["FG EHB"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@FGCHNName", SqlDbType.NVarChar).Value = myTable.Rows[n]["FG CHN Name"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = myTable.Rows[n]["Destination"].ToString().Trim().ToUpper();
                        string strGongDanQty = myTable.Rows[n]["GongDan Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strGongDanQty)) { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGongDanQty); }
                        SqlComm.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = myTable.Rows[n]["Group ID"].ToString().Trim().ToUpper();
                        string strPingDanQty = myTable.Rows[n]["PingDan Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strPingDanQty)) { SqlComm.Parameters.Add("@PingDanQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@PingDanQty", SqlDbType.Int).Value = Convert.ToInt32(strPingDanQty); }
                        SqlComm.Parameters.Add("@PingDanID", SqlDbType.NVarChar).Value = myTable.Rows[n]["PingDan ID"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@PingDanNo", SqlDbType.NVarChar).Value = myTable.Rows[n]["PingDan No"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[n]["Pass Gate Time"].ToString().Trim())) { SqlComm.Parameters.Add("@PassGateTime", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@PassGateTime", SqlDbType.DateTime).Value = myTable.Rows[n]["Pass Gate Time"].ToString().Trim(); }
                        if (String.IsNullOrEmpty(myTable.Rows[n]["PingDan Date"].ToString().Trim())) { SqlComm.Parameters.Add("@PingDanDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@PingDanDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[n]["PingDan Date"].ToString().Trim()); }
                        SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = "Data Conversion";
                        SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[n]["Remark"].ToString().Trim().ToUpper();

                        SqlComm.CommandText = @"INSERT INTO C_PingDan([IE Type], [BeiAnDan ID], [GongDan No], [Order Category], [ESS/LINE], [Order No], [FG EHB], " +
                                               "[FG CHN Name], [Destination], [GongDan Qty], [Group ID], [PingDan Qty], [PingDan ID], [PingDan No], [Pass Gate Time], " +
                                               "[PingDan Date], [Creater], [Remark]) VALUES(@IEtype, @BeiAnDanID, @GongDanNo, @OrderCategory, @EssLine, @OrderNo, " +
                                               "@FGEHB, @FGCHNName, @Destination, @GongDanQty, @GroupID, @PingDanQty, @PingDanID, @PingDanNo, @PassGateTime, " +
                                               "@PingDanDate, @Creater, @Remark)";
                        SqlTransaction SqlTransPingDan = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransPingDan;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransPingDan.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransPingDan.Rollback();
                            SqlTransPingDan.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                #region //Upload data to B_OrderFulfillment table
                if (String.Compare(this.cmbObject.Text.Trim(), "Order Fulfillment") == 0)
                {
                    SqlComm.CommandText = @"DELETE FROM B_OrderFulfillment";
                    SqlComm.ExecuteNonQuery();

                    for (int t = 0; t < myTable.Rows.Count; t++)
                    {
                        SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = myTable.Rows[t]["Batch No"].ToString().Trim().ToUpper();
                        string strOFInsDate = myTable.Rows[t]["OF Instruction Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strOFInsDate)) { SqlComm.Parameters.Add("@OFInstructionDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@OFInstructionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strOFInsDate); }
                        SqlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = myTable.Rows[t]["Order No"].ToString().Trim().ToUpper();
                        string strOrderQty = myTable.Rows[t]["Order Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strOrderQty)) { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = Convert.ToInt32(strOrderQty); }
                        string strGongDanQty = myTable.Rows[t]["GongDan Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strGongDanQty)) { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGongDanQty); }
                        SqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = myTable.Rows[t]["IE Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@EssLine", SqlDbType.NVarChar).Value = myTable.Rows[t]["ESS/LINE"].ToString().Trim().ToUpper();

                        SqlComm.CommandText = @"INSERT INTO B_OrderFulfillment([Batch No], [OF Instruction Date], [Order No], [Order Qty], [GongDan Qty], [IE Type], " +
                                               "[ESS/LINE]) VALUES(@BatchNo, @OFInstructionDate, @OrderNo, @OrderQty, @GongDanQty, @IEType, @EssLine)";
                        SqlTransaction SqlTransOF = SqlConn.BeginTransaction();
                        SqlComm.Transaction = SqlTransOF;
                        try
                        {
                            SqlComm.ExecuteNonQuery();
                            SqlTransOF.Commit();
                        }
                        catch (Exception)
                        {
                            SqlTransOF.Rollback();
                            SqlTransOF.Dispose();
                            throw;
                        }
                        finally { SqlComm.Parameters.Clear(); }
                    }
                }
                #endregion

                SqlComm.Dispose();
                dtName.Dispose();
            }
            MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            myTable.Clear();
            myTable.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
        }

        private void llinkPrompt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbObject.Text.ToString().Trim()))
            {
                MessageBox.Show("Please select the upload object.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbObject.Text = null;
                this.cmbObject.Focus();
                return;
            }

            if (String.Compare(this.cmbObject.Text.Trim(), "BOM Detail") == 0)
            {
                MessageBox.Show("When upload C_BOMDetail data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tBatch Path, \n\tBatch No, \n\tLine No, \n\tItem No, \n\tLot No, \n\tInventory Type, \n\tRM Category, " +
                                "\n\tRM Customs Code, \n\tBGD No, \n\tRM Qty, \n\tRM Currency, \n\tRM Price, \n\tConsumption, \n\tDrools EHB, " +
                                "\n\tNote", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (String.Compare(this.cmbObject.Text.Trim(), "BOM Master") == 0)
            {
                MessageBox.Show("When upload C_BOM data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tActual Start Date, \n\tActual Close Date, \n\tBatch No, \n\tFG No, \n\tBOM In Customs, \n\tFG Qty, " +
                                "\n\tOrder Price, \n\tOrder Currency, \n\tTotal Input Qty, \n\tTotal RM Cost(USD), \n\tHS Code, \n\tCHN Name, " +
                                "\n\tQty Loss Rate, \n\tDrools Qty, \n\tCreated Date, \n\tCreater, \n\tGongDan Used Qty, \n\tApproved Date, " +
                                "\n\tRemark, \n\tRemark Date, \n\tFreeze", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Detail") == 0)
            {
                MessageBox.Show("When upload C_GongDanDetail data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tBatch Path, \n\tGongDan No, \n\tLine No, \n\tItem No, \n\tLot No, \n\tInventory Type, \n\tRM Category, " +
                                "\n\tRM Customs Code, \n\tBGD No, \n\tRM Currency, \n\tRM Price, \n\tConsumption, \n\tRM Used Qty, \n\tDrools Quota, " +
                                "\n\tDrools EHB", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Master") == 0)
            {
                MessageBox.Show("When upload C_GongDan data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tActual Start Date, \n\tActual Close Date, \n\tBatch No, \n\tBOM In Customs, \n\tGongDan No, \n\tFG No, \n\tESS/LINE, " +
                                "\n\tOrder No, \n\tIE Type, \n\tOrder Category, \n\tDestination, \n\tTotal Ship Qty, \n\tGongDan Qty, \n\tOrder Price, " +
                                "\n\tOrder Currency, \n\tTotal RM Qty, \n\tTotal RM Cost(USD), \n\tDrools Rate, \n\tCHN Name, \n\tCreated Date, \n\tCreater, " +
                                "\n\tBeiAnDan Used Qty, \n\tApproved Date, \n\tRemark, \n\tRemark Date, \n\tPC Item", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") == 0)
            {
                MessageBox.Show("When upload C_BeiAnDan data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tGroup ID, \n\tIE Type, \n\tGongDan No, \n\tTotal Ship Qty, \n\tGongDan Qty, \n\tFG EHB, \n\tCHN Name, \n\tOrder No, " +
                                "\n\tOrder Category, \n\tESS/LINE, \n\tSelling Price, \n\tCurrency, \n\tLocal Total RM Cost, \n\tSelling Amount, \n\tDestination, " +
                                "\n\tPass to IE Date, \n\tGongDan Approved Date, \n\tIE Rev Amt, \n\tOF Rev Amt, \n\tCustoms Total RM Cost, \n\tBeiAnDan ID, " +
                                "\n\tBeiAnDan No, \n\tBeiAnDan Date, \n\tTax & Duty Paid Date, \n\tCustoms Release Date, \n\tIE Remark, \n\tOF Remark",
                                "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (String.Compare(this.cmbObject.Text.Trim(), "Drools BeiAnDan") == 0)
            {
                MessageBox.Show("When upload C_BeiAnDan_Drools data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tGongDan No, \n\tFG EHB, \n\tDrools Qty, \n\tDrools CHN Name, \n\tDrools EHB, \n\tDrools BeiAnDan ID, \n\tDrools BeiAnDan No, " +
                                "\n\tDrools BeiAnDan Date, \n\tTax & Duty Paid Date, \n\tCustoms Release Date", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (String.Compare(this.cmbObject.Text.Trim(), "PingDan") == 0)
            {
                MessageBox.Show("When upload C_PingDan data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tIE Type, \n\tBeiAnDan ID, \n\tGongDan No, \n\tOrder Category, \n\tESS/LINE, \n\tOrder No, \n\tFG EHB, \n\tFG CHN Name, " +
                                "\n\tDestination, \n\tGongDan Qty, \n\tGroup ID, \n\tPingDan Qty, \n\tPingDan ID, \n\tPingDan No, \n\tPass Gate Time, " +
                                "\n\tPingDan Date, \n\tRemark", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            if (String.Compare(this.cmbObject.Text.Trim(), "Order Fulfillment") == 0)
            {
                MessageBox.Show("When upload B_OrderFulfillment data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tBatch No, \n\tOF Instruction Date, \n\tOrder No, \n\tOrder Qty, \n\tGongDan Qty, \n\tIE Type, \n\tESS/LINE", "Prompt", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbObject.Text.ToString().Trim()))
            {
                MessageBox.Show("Please select the upload object first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbObject.Focus();
                return;
            }

            string strBrowse = null;
            if (String.Compare(this.cmbObject.Text.Trim(), "BOM Master") == 0)
            { strBrowse = @"SELECT * FROM C_BOM WHERE"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "BOM Detail") == 0)
            { strBrowse = @"SELECT C_BOMDetail.* FROM C_BOMDetail INNER JOIN C_BOM ON C_BOM.[Batch No] = C_BOMDetail.[Batch No] WHERE"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Master") == 0)
            { strBrowse = @"SELECT * FROM C_GongDan WHERE"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "GongDan Detail") == 0)
            { strBrowse = @"SELECT C_GongDanDetail.* FROM C_GongDanDetail INNER JOIN C_GongDan ON C_GongDan.[GongDan No] = C_GongDanDetail.[GongDan No] WHERE"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") == 0)
            { strBrowse = @"SELECT * FROM C_BeiAnDan WHERE"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "Drools BeiAnDan") == 0)
            { strBrowse = @"SELECT * FROM C_BeiAnDan_Drools"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "PingDan") == 0)
            { strBrowse = @"SELECT * FROM C_PingDan WHERE"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "Order Fulfillment") == 0)
            { strBrowse = @"SELECT * FROM B_OrderFulfillment"; }

            if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") != 0 &&
                String.Compare(this.cmbObject.Text.Trim(), "Drools BeiAnDan") != 0 &&
                String.Compare(this.cmbObject.Text.Trim(), "PingDan") != 0 && 
                String.Compare(this.cmbObject.Text.Trim(), "Order Fulfillment") != 0)
            {
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
            }
            if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") == 0)
            {
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
                        { strBrowse += " [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                    }
                    else
                    { strBrowse += " [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                    { strBrowse += " [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
                }
            }
            if (String.Compare(this.cmbObject.Text.Trim(), "Drools BeiAnDan") == 0)
            {
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
                        { strBrowse += " [Drools BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [Drools BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                    }
                    else
                    { strBrowse += " [Drools BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                    { strBrowse += " [Drools BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
                }
            }
            if (String.Compare(this.cmbObject.Text.Trim(), "PingDan") == 0)
            {
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
                        { strBrowse += " [PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                    }
                    else
                    { strBrowse += " [PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                    { strBrowse += " [PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
                }
            }

            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 6, 6), " WHERE") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Length - 6); }
            if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") == 0)
            { strBrowse += " ORDER BY [BeiAnDan Date] DESC, [IE Type], [BeiAnDan ID] ASC"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "Drools BeiAnDan") == 0)
            { strBrowse += " ORDER BY [Drools BeiAnDan Date] DESC, [GongDan No] ASC"; }
            if (String.Compare(this.cmbObject.Text.Trim(), "PingDan") == 0)
            { strBrowse += " ORDER BY [PingDan Date] DESC, [IE Type], [PingDan ID] ASC"; }

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlDataAdapter browseAdapter = new SqlDataAdapter(strBrowse, SqlConn);
            browseTbl.Rows.Clear();
            browseTbl.Columns.Clear();
            browseAdapter.Fill(browseTbl);
            browseAdapter.Dispose();

            if (browseTbl.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                browseTbl.Clear();
                this.dgvQueryData.DataSource = DBNull.Value;
                if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
                return;
            }
            else
            {
                this.dgvQueryData.DataSource = browseTbl;
                if (String.Compare(this.cmbObject.Text.Trim(), "BeiAnDan") == 0) { this.dgvQueryData.Columns[browseTbl.Columns.Count - 1].Visible = false; }
                MessageBox.Show("There are " + browseTbl.Rows.Count.ToString().Trim() + " lines data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = null;
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo.CustomFormat = null;
        }

        private void dtpFrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; }
        }

        private void dtpTo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; }
        }

        private void cmbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtPath.Text = String.Empty;
            this.dgvQueryData.DataSource = DBNull.Value;
        }

        //------------------- Update Historical Data -------------------//
        private void btnUpdateGDUsedQty_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to do this action ①?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = SqlConn;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_UpdateGongDanUsedQty", SqlConn);
            DataTable dtBOM = new DataTable();
            sqlAdapter.Fill(dtBOM);
            sqlAdapter.Dispose();

            foreach (DataRow dr in dtBOM.Rows)
            {
                int iGDUsedQty = Convert.ToInt32(dr["GD Qty"].ToString().Trim()) + Convert.ToInt32(dr["GongDan Used Qty"].ToString().Trim());
                sqlComm.Parameters.Add("@GDUsedQty", SqlDbType.Int).Value = iGDUsedQty;
                sqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim();
                sqlComm.CommandText = @"UPDATE C_BOM SET [GongDan Used Qty] = @GDUsedQty WHERE [Batch No] = @BatchNo";
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();
            }

            dtBOM.Dispose();
            sqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            MessageBox.Show("Update [GongDan Used Qty] in BOM successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBADUsedQty_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to do this action ②?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = SqlConn;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_UpdateBeiAnDanUsedQty", SqlConn);
            DataTable dtGD = new DataTable();
            sqlAdapter.Fill(dtGD);
            sqlAdapter.Dispose();

            foreach (DataRow dr in dtGD.Rows)
            {
                int iBADUsedQty = Convert.ToInt32(dr["GD Qty"].ToString().Trim()) + Convert.ToInt32(dr["BeiAnDan Used Qty"].ToString().Trim());
                sqlComm.Parameters.Add("@BADUsedQty", SqlDbType.Int).Value = iBADUsedQty;
                sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = dr["GongDan No"].ToString().Trim();
                sqlComm.CommandText = @"UPDATE C_GongDan SET [BeiAnDan Used Qty] = @BADUsedQty WHERE [GongDan No] = @GongDanNo";
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();
            }

            dtGD.Dispose();
            sqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            MessageBox.Show("Update [BeiAnDan Used Qty] in GongDan successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGDRMBal_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to do this action ③?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = SqlConn;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_UpdateGongDanRMBalance", SqlConn);
            DataTable dtGDRMBal = new DataTable();
            sqlAdapter.Fill(dtGDRMBal);
            sqlAdapter.Dispose();

            foreach (DataRow dr in dtGDRMBal.Rows)
            {
                decimal dRMQty = Math.Round(Convert.ToDecimal(dr["RM Qty"].ToString().Trim()), 6);
                sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["Available RM Balance"].ToString().Trim()) - dRMQty;
                sqlComm.Parameters.Add("@GDPending", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["GongDan Pending"].ToString().Trim()) + dRMQty;
                sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = dr["RM Customs Code"].ToString().Trim();
                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = dr["BGD No"].ToString().Trim();
                sqlComm.CommandText = @"UPDATE C_RMBalance SET [Available RM Balance] = @AvailRMBal, [GongDan Pending] = @GDPending " +
                                       "WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();
            }

            dtGDRMBal.Dispose();
            sqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            MessageBox.Show("Update RM Balance in GongDan successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPDUsedQty_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to do this action ④?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = SqlConn;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_UpdatePingDanUsedQty", SqlConn);
            DataTable dtBAD = new DataTable();
            sqlAdapter.Fill(dtBAD);
            sqlAdapter.Dispose();

            foreach (DataRow dr in dtBAD.Rows)
            {
                int iPDUsedQty = Convert.ToInt32(dr["PD Qty"].ToString().Trim()) + Convert.ToInt32(dr["PingDan Used Qty"].ToString().Trim());
                sqlComm.Parameters.Add("@PDUsedQty", SqlDbType.Int).Value = iPDUsedQty;
                sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = dr["GongDan No"].ToString().Trim();
                sqlComm.CommandText = @"UPDATE C_BeiAnDan SET [PingDan Used Qty] = @PDUsedQty WHERE [GongDan No] = @GongDanNo";
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();
            }

            dtBAD.Dispose();
            sqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            MessageBox.Show("Update [PingDan Used Qty] in BeiAnDan successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBADRMBal_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to do this action ⑤?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = SqlConn;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_UpdateBeiAnDanRMBalance", SqlConn);
            DataTable dtBADRMBal = new DataTable();
            sqlAdapter.Fill(dtBADRMBal);
            sqlAdapter.Dispose();

            foreach (DataRow dr in dtBADRMBal.Rows)
            {
                decimal dRMQty = Math.Round(Convert.ToDecimal(dr["RM Qty"].ToString().Trim()), 6);
                sqlComm.Parameters.Add("@CustomsBal", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["Customs Balance"].ToString().Trim()) - dRMQty;
                sqlComm.Parameters.Add("@GDPending", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["GongDan Pending"].ToString().Trim()) - dRMQty;
                sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = dr["RM Customs Code"].ToString().Trim();
                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = dr["BGD No"].ToString().Trim();
                sqlComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBal, [GongDan Pending] = @GDPending " +
                                       "WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();
            }

            dtBADRMBal.Dispose();
            sqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            MessageBox.Show("Update RM Balance in BeiAnDan successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdateDroolsBal_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to do this action ⑥?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = SqlConn;
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_UpdateBeiAnDanDroolsBalance", SqlConn);
            DataTable dtDroolsBal = new DataTable();
            sqlAdapter.Fill(dtDroolsBal);

            foreach (DataRow dr in dtDroolsBal.Rows)
            {
                decimal dDroolsQty = Math.Round(Convert.ToDecimal(dr["Drools Qty"].ToString().Trim()), 4);
                string strDroolsEHB = dr["EHB"].ToString().Trim();
                if (String.IsNullOrEmpty(strDroolsEHB))
                {
                    sqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = dr["Drools EHB"].ToString().Trim();
                    sqlComm.Parameters.Add("@InputQty", SqlDbType.Decimal).Value = dDroolsQty;
                    sqlComm.Parameters.Add("@AvailBal", SqlDbType.Decimal).Value = dDroolsQty;
                    sqlComm.CommandText = @"INSERT INTO C_DroolsBalance([Drools EHB], [Input Qty], [Available Balance]) VALUES(@DroolsEHB, @InputQty, @AvailBal)";
                }
                else
                {
                    sqlComm.Parameters.Add("@InputQty", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["Input Qty"].ToString().Trim()) + dDroolsQty;
                    sqlComm.Parameters.Add("@AvailBal", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["Available Balance"].ToString().Trim()) + dDroolsQty;
                    sqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = dr["Drools EHB"].ToString().Trim();
                    sqlComm.CommandText = @"UPDATE C_DroolsBalance SET [Input Qty] = @InputQty, [Available Balance] = @AvailBal WHERE [Drools EHB] = @DroolsEHB";
                }
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();
            }

            dtDroolsBal.Dispose();
            sqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            MessageBox.Show("Update Drools Balance in BeiAnDan successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdateOFData_Click(object sender, EventArgs e)
        {
            DialogResult dlgR = MessageBox.Show("Are you sure to do this action ⑦?\n[Yes]: Migrate the ESS/LINE of closed and 'A' as prefix;\n[No]: Migrate the ESS/LINE of containing '-' character;\n[Cancel]: Reject to operate anything.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgR == DialogResult.Cancel) { return; }
            else 
            {
                DateTime dtNow = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));              
                if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = SqlConn;

                if (dlgR == DialogResult.No)
                {
                    DateTime dtInterval1 = dtNow.AddDays(-360);
                    string strSQL1 = @"SELECT ROW_NUMBER() OVER (ORDER BY [Batch No], [ESS/LINE]) AS [Line No], *, '" + dtNow + "' AS [Backup Time] " +
                                      "FROM B_OrderFulfillment WHERE CHARINDEX('A', [ESS/LINE]) = 0 AND CHARINDEX('-', [ESS/LINE]) > 0 AND " +
                                      "CAST(FORMAT([OF Instruction Date], 'M/d/yyyy') AS datetime) <= '" + dtInterval1 + "'";
                    SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(strSQL1, SqlConn);
                    DataTable dtOF1 = new DataTable();
                    sqlAdapter1.Fill(dtOF1);
                    sqlAdapter1.Dispose();

                    if (dtOF1.Rows.Count > 0)
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.CommandText = @"usp_InsertOrderFulfillmentBackup";
                        sqlComm.Parameters.AddWithValue("@TVP_OF", dtOF1);
                        sqlComm.ExecuteNonQuery();                       
                        MessageBox.Show("Successfully migrate the Order Fulfillment Historical Data of containing '-' character.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else { MessageBox.Show("There is no the Order Fulfillment Historical Data of containing '-' character.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    dtOF1.Dispose();
                }
                else
                {
                    DateTime dtInterval2 = dtNow.AddDays(-180);
                    string strSQL2 = @"SELECT ROW_NUMBER() OVER (ORDER BY [Batch No], [ESS/LINE]) AS [Line No], *, '" + dtNow + "' AS [Backup Time] FROM B_OrderFulfillment " +
                                      "WHERE CHARINDEX('A', [ESS/LINE]) > 0 AND CAST(FORMAT([OF Instruction Date], 'M/d/yyyy') AS datetime) <= '" + dtInterval2 + "'";
                    SqlDataAdapter sqlAdapter2 = new SqlDataAdapter(strSQL2, SqlConn);
                    DataTable dtOF2 = new DataTable();
                    sqlAdapter2.Fill(dtOF2);

                    string strSQL3 = @"SELECT TOP(2000) [ESS/LINE], SUBSTRING([ESS/LINE], 1, CHARINDEX('/', [ESS/LINE]) - 1) AS cono, " +
                                      "SUBSTRING([ESS/LINE], CHARINDEX('/', [ESS/LINE]) + 1, LEN([ESS/LINE])) AS suffix FROM (SELECT DISTINCT [ESS/LINE] " +
                                      "FROM B_OrderFulfillment WHERE CHARINDEX('A', [ESS/LINE]) = 0 AND CHARINDEX('-', [ESS/LINE]) = 0 AND " +
                                      "CAST(FORMAT([OF Instruction Date], 'M/d/yyyy') AS datetime) <= '" + dtInterval2 + "') AS XX";
                    sqlAdapter2 = new SqlDataAdapter(strSQL3, SqlConn);
                    DataTable dtOF3 = new DataTable();
                    sqlAdapter2.Fill(dtOF3);
                    sqlAdapter2.Dispose();

                    OracleConnection OrclConn = new OracleConnection(SqlLib.StrEssOrclConnection);
                    if (OrclConn.State == ConnectionState.Closed) { OrclConn.Open(); }
                    OracleCommand OrclComm = new OracleCommand();
                    OrclComm.Connection = OrclConn;

                    DataTable dtOF4 = new DataTable();
                    dtOF4.Columns.Add("ESS/LINE", typeof(string));
                    foreach (DataRow dr in dtOF3.Rows)
                    {
                        OrclComm.CommandText = @"SELECT rowstatus FROM corow WHERE cono = '" + dr[1].ToString().Trim() + "' AND rowpos || '0' || rowseq = '" + dr[2].ToString().Trim() + "'";
                        string strResult = Convert.ToString(OrclComm.ExecuteScalar()).Trim();
                        if (!String.IsNullOrEmpty(strResult) && strResult == "961") 
                        {
                            DataRow row = dtOF4.NewRow();
                            row[0] = dr[0].ToString().Trim();
                            dtOF4.Rows.Add(row);                          
                        }
                    }
                    dtOF3.Dispose();
                    OrclComm.Dispose();
                    if (OrclConn.State == ConnectionState.Open)
                    {
                        OrclConn.Close();
                        OrclConn.Dispose();
                    }

                    string strEssLine = null;
                    if (dtOF4.Rows.Count > 0)
                    {                       
                        foreach (DataRow drow in dtOF4.Rows)
                        { strEssLine += "'" + drow[0].ToString().Trim() + "',"; }
                        strEssLine = strEssLine.Remove(strEssLine.Length - 1);

                        string strSQL5 = @"SELECT " + dtOF2.Rows.Count + " + ROW_NUMBER() OVER (ORDER BY [Batch No], [ESS/LINE]) AS [Line No], *, '" + dtNow +
                                          "' AS [Backup Time] FROM B_OrderFulfillment WHERE [ESS/LINE] IN (" + strEssLine + ")";
                        SqlDataAdapter sqlAdapter5 = new SqlDataAdapter(strSQL5, SqlConn);
                        DataTable dtOF5 = new DataTable();
                        sqlAdapter5.Fill(dtOF5);
                        sqlAdapter5.Dispose();

                        dtOF2.Merge(dtOF5);
                        dtOF5.Dispose();
                    }
                    dtOF4.Dispose();

                    if (dtOF2.Rows.Count > 0)
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.CommandText = @"usp_InsertOrderFulfillmentBackup";
                        sqlComm.Parameters.AddWithValue("@TVP_OF", dtOF2);
                        sqlComm.ExecuteNonQuery();
                        MessageBox.Show("Successfully migrate the Order Fulfillment Historical Data of closed and 'A' as prefix.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else { MessageBox.Show("There is no the Order Fulfillment Historical Data of closed and 'A' as prefix.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    dtOF2.Dispose();
                }

                sqlComm.Dispose();
                if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            }
        }

        //-------------------- Data Integrity Check --------------------//
        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxShow.Visible == false)
            { 
                this.gBoxShow.Visible = true;
                this.txtRMEHB.Text = string.Empty;
                this.txtBGDNO.Text = string.Empty;
                this.txtAvailRMBal.Text = string.Empty;
                this.dtpDate.CustomFormat = " ";
            }
            else
            { this.gBoxShow.Visible = false; }
        }

        private void btnDownloadGDList_Click(object sender, EventArgs e)
        {
            if (this.txtGDList.Lines.Length == 0)
            {
                MessageBox.Show("Please input GongDan No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtGDList.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }

            DataTable myGDtbl = new DataTable();
            myGDtbl.Columns.Add("RM Customs Code", typeof(string));
            myGDtbl.Columns.Add("BGD No", typeof(string));
            myGDtbl.Columns.Add("RM Qty", typeof(decimal));
            DataTable dtTotalRM = new DataTable();

            for (int i = 0; i < this.txtGDList.Lines.Length; i++)
            {
                //In order to gather the accurate data, only one by one to fetch GongDan
                string strSQL = @"SELECT [RM Customs Code], [BGD No], SUM(CAST([RM Used Qty] AS decimal(18,6))) AS [RM Qty] FROM C_GongDanDetail WHERE [GongDan No] = '"
                                  + this.txtGDList.Lines[i].ToString().Trim() + "' GROUP BY [RM Customs Code], [BGD No]";
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
                myGDtbl.Clear();
                sqlAdapter.Fill(myGDtbl);
                sqlAdapter.Dispose();

                if (dtTotalRM.Rows.Count == 0) { dtTotalRM = myGDtbl.Copy(); }
                else { dtTotalRM.Merge(myGDtbl); }
            }

            SqlLib sqlLib = new SqlLib();
            string[] str = { "RM Customs Code", "BGD No" };
            myGDtbl.Reset();
            myGDtbl = sqlLib.SelectDistinct(dtTotalRM, str);
            sqlLib.Dispose(0);
            myGDtbl.Columns.Add("RM Qty", typeof(decimal));
            foreach (DataRow drow in myGDtbl.Rows)
            {
                string strRMEHB = drow["RM Customs Code"].ToString().Trim();
                string strBGDNO = drow["BGD No"].ToString().Trim();
                decimal dRMQty = Convert.ToDecimal(dtTotalRM.Compute("SUM([RM Qty])", "[RM Customs Code] = '" + strRMEHB + "' AND [BGD No] = '" + strBGDNO + "'"));
                drow["RM Qty"] = dRMQty;
            }
            myGDtbl.AcceptChanges();
            dtTotalRM.Dispose();

            if (myGDtbl.Rows.Count == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Application.Workbooks.Add(true);

                excel.get_Range(excel.Cells[1, 1], excel.Cells[myGDtbl.Rows.Count + 1, myGDtbl.Columns.Count]).NumberFormatLocal = "@";
                for (int i = 0; i < myGDtbl.Rows.Count; i++)
                {
                    for (int j = 0; j < myGDtbl.Columns.Count; j++)
                    { excel.Cells[i + 2, j + 1] = myGDtbl.Rows[i][j].ToString().Trim(); }
                }

                for (int k = 0; k < myGDtbl.Columns.Count; k++)
                { excel.Cells[1, k + 1] = myGDtbl.Columns[k].ColumnName.Trim(); }

                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, myGDtbl.Columns.Count]).Font.Bold = true;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[myGDtbl.Rows.Count + 1, myGDtbl.Columns.Count]).Font.Name = "Verdana";
                excel.get_Range(excel.Cells[1, 1], excel.Cells[myGDtbl.Rows.Count + 1, myGDtbl.Columns.Count]).Font.Size = 9;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[myGDtbl.Rows.Count + 1, myGDtbl.Columns.Count]).Borders.LineStyle = 1;
                excel.Cells.EntireColumn.AutoFit();
                excel.Visible = true;

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
            }

            myGDtbl.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        private void btnDownloadGroupIDList_Click(object sender, EventArgs e)
        {
            if (this.txtGroupIDList.Lines.Length == 0)
            {
                MessageBox.Show("Please input Group ID.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtGroupIDList.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }

            string strGroupIDList = null;
            for (int i = 0; i < this.txtGroupIDList.Lines.Length; i++)
            { strGroupIDList += "'" + this.txtGroupIDList.Lines[i].ToString().Trim() + "',"; }
            strGroupIDList = strGroupIDList.Remove(strGroupIDList.Length - 1);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT DISTINCT [GongDan No] FROM C_BeiAnDan WHERE [Group ID] IN (" + strGroupIDList + ")", sqlConn);
            DataTable dtGongDanList = new DataTable();
            sqlAdapter.Fill(dtGongDanList);
            string strGongDanList = null;
            for (int j = 0; j < dtGongDanList.Rows.Count; j++)
            { strGongDanList += "'" + dtGongDanList.Rows[j][0].ToString().Trim() + "',"; }
            strGongDanList = strGongDanList.Remove(strGongDanList.Length - 1);
            dtGongDanList.Dispose();

            string strSQL = @"SELECT [RM Customs Code], [BGD No], SUM([RMUsedQty]) AS [RM Qty] FROM (SELECT [RM Customs Code], [BGD No], " +
                             "CAST([RM Used Qty] AS decimal(18, 6)) AS [RMUsedQty] FROM C_GongDanDetail  WHERE [RM Used Qty] > 0.0 AND [GongDan No] IN (" 
                             + strGongDanList + ")) AS X GROUP BY [RM Customs Code], [BGD No]";
            sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
            DataTable dtBeiAnDanList = new DataTable();
            sqlAdapter.Fill(dtBeiAnDanList);

            strSQL = @"SELECT [Drools EHB], SUM([DroolsQuota]) AS [Drools Qty] FROM (SELECT [Drools EHB], CAST([Drools Quota] AS decimal(18, 4)) AS [DroolsQuota] " +
                      "FROM C_GongDanDetail WHERE RIGHT([Drools EHB], 1) = 'W' AND [Drools Quota] > 0.0 AND [GongDan No] IN (" + strGongDanList + ")) AS Y GROUP BY [Drools EHB]";
            sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
            DataTable dtDroolsList = new DataTable();
            sqlAdapter.Fill(dtDroolsList);
            sqlAdapter.Dispose();

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            worksheet.Name = "BeiAnDan_RMBalance";

            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).NumberFormatLocal = "@";
            for (int i = 0; i < dtBeiAnDanList.Rows.Count; i++)
            {
                for (int j = 0; j < dtBeiAnDanList.Columns.Count; j++)
                { worksheet.Cells[i + 2, j + 1] = dtBeiAnDanList.Rows[i][j].ToString().Trim(); }
            }

            for (int k = 0; k < dtBeiAnDanList.Columns.Count; k++)
            { worksheet.Cells[1, k + 1] = dtBeiAnDanList.Columns[k].ColumnName.Trim(); }

            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, dtBeiAnDanList.Columns.Count]).Font.Bold = true;
            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).Font.Name = "Verdana";
            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).Font.Size = 9;
            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).Borders.LineStyle = 1;
            worksheet.Cells.EntireColumn.AutoFit();

            if (dtDroolsList.Rows.Count > 0)
            {
                object missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, missing, missing);
                worksheet.Name = "BeiAnDan_DroolsBalance";

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).NumberFormatLocal = "@";
                for (int i = 0; i < dtDroolsList.Rows.Count; i++)
                {
                    for (int j = 0; j < dtDroolsList.Columns.Count; j++)
                    { worksheet.Cells[i + 2, j + 1] = dtDroolsList.Rows[i][j].ToString().Trim(); }
                }

                for (int k = 0; k < dtDroolsList.Columns.Count; k++)
                { worksheet.Cells[1, k + 1] = dtDroolsList.Columns[k].ColumnName.Trim(); }

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, dtDroolsList.Columns.Count]).Font.Bold = true;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).Font.Name = "Verdana";
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).Font.Size = 9;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).Borders.LineStyle = 1;
                worksheet.Cells.EntireColumn.AutoFit();
            }

            excel.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            worksheet = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;

            dtBeiAnDanList.Dispose();
            dtDroolsList.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        private void btnQueryRMBalance_Click(object sender, EventArgs e)
        {
            string strBGDNO = this.txtBGDNO.Text.Trim().ToUpper();
            string strRMEHB = this.txtRMEHB.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strBGDNO) || String.IsNullOrEmpty(strRMEHB))
            {
                MessageBox.Show("Please input RM EHB and BGD NO meantime.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtRMEHB.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;

            decimal dAvailRMBal = 0.0M;
            sqlComm.CommandText = @"SELECT CAST(SUM([RM Used Qty]) AS decimal(18, 6)) FROM C_GongDanDetail WHERE [RM Customs Code] = '" + strRMEHB + "' AND [BGD No] = '" + strBGDNO + "'";
            SqlDataReader sqlReader = sqlComm.ExecuteReader();
            if (sqlReader.HasRows)
            {
                while (sqlReader.Read())
                {
                    dAvailRMBal = Convert.ToDecimal(Double.Parse(sqlReader.GetValue(0).ToString().Trim()));
                }
            }
            sqlReader.Close();
            sqlReader.Dispose();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }

            this.txtAvailRMBal.Text = dAvailRMBal.ToString().Trim();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpDate.CustomFormat = null;
        }

        private void dtpDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpDate.CustomFormat = " "; }
        }

        private void btnGDDownload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.dtpDate.Value.ToString().Trim()))
            {
                MessageBox.Show("Please input GongDan Approved Date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dtpDate.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.Parameters.Add("@ApprovedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dtpDate.Value.ToString("M/d/yyyy"));
            sqlComm.CommandText = @"SELECT [RM Customs Code], [BGD No], SUM([RMUsedQty]) AS [RM Qty] FROM (SELECT D.[RM Customs Code], D.[BGD No], " +
                                   "CAST(D.[RM Used Qty] AS decimal(18, 6)) AS [RMUsedQty] FROM C_GongDanDetail AS D INNER JOIN C_GongDan AS M ON " +
                                   "D.[GongDan No] = M.[GongDan No] WHERE FORMAT(M.[Approved Date], 'M/d/yyyy') = @ApprovedDate) AS X GROUP BY [RM Customs Code], [BGD No]";
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtGDList = new DataTable();
            sqlAdapter.Fill(dtGDList);
            sqlAdapter.Dispose();

            if (dtGDList.Rows.Count == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Application.Workbooks.Add(true);

                excel.get_Range(excel.Cells[1, 1], excel.Cells[dtGDList.Rows.Count + 1, dtGDList.Columns.Count]).NumberFormatLocal = "@";
                for (int i = 0; i < dtGDList.Rows.Count; i++)
                {
                    for (int j = 0; j < dtGDList.Columns.Count; j++)
                    { excel.Cells[i + 2, j + 1] = dtGDList.Rows[i][j].ToString().Trim(); }
                }

                for (int k = 0; k < dtGDList.Columns.Count; k++)
                { excel.Cells[1, k + 1] = dtGDList.Columns[k].ColumnName.Trim(); }

                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, dtGDList.Columns.Count]).Font.Bold = true;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[dtGDList.Rows.Count + 1, dtGDList.Columns.Count]).Font.Name = "Verdana";
                excel.get_Range(excel.Cells[1, 1], excel.Cells[dtGDList.Rows.Count + 1, dtGDList.Columns.Count]).Font.Size = 9;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[dtGDList.Rows.Count + 1, dtGDList.Columns.Count]).Borders.LineStyle = 1;
                excel.Cells.EntireColumn.AutoFit();
                excel.Visible = true;

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
            }

            dtGDList.Dispose();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        private void btnBADDownload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.dtpDate.Value.ToString().Trim()))
            {
                MessageBox.Show("Please input GongDan Approved Date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dtpDate.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dtpDate.Value.ToString("M/d/yyyy"));
            sqlComm.CommandText = @"SELECT [RM Customs Code], [BGD No], SUM([RMUsedQty]) AS [RM Qty] FROM (SELECT [RM Customs Code], [BGD No], " +
                                   "CAST([RM Used Qty] AS decimal(18, 6)) AS [RMUsedQty] FROM C_GongDanDetail WHERE [GongDan No] IN (SELECT DISTINCT [GongDan No] " +
                                   "FROM C_BeiAnDan WHERE FORMAT(CAST([Customs Release Date] AS datetime), 'M/d/yyyy') = @CustomsReleaseDate)) AS X " +
                                   "GROUP BY [RM Customs Code], [BGD No]";
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtBeiAnDanList = new DataTable();
            sqlAdapter.Fill(dtBeiAnDanList);
            sqlComm.CommandText = @"SELECT [Drools EHB], SUM([DroolsQuota]) AS [Drools Qty] FROM (SELECT [Drools EHB], CAST([Drools Quota] AS decimal(18, 4)) AS [DroolsQuota] " +
                                   "FROM C_GongDanDetail WHERE RIGHT([Drools EHB], 1) = 'W' AND [Drools Quota] > 0.0 AND [GongDan No] IN (SELECT DISTINCT [GongDan No] FROM " +
                                   "C_BeiAnDan_Drools WHERE FORMAT([Customs Release Date], 'M/d/yyyy') = @CustomsReleaseDate)) AS Y GROUP BY [Drools EHB]";
            sqlAdapter.SelectCommand = sqlComm;
            DataTable dtDroolsList = new DataTable();
            sqlAdapter.Fill(dtDroolsList);
            sqlAdapter.Dispose();

            if (dtBeiAnDanList.Rows.Count == 0 && dtDroolsList.Rows.Count == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                worksheet.Name = "BeiAnDan_RMBalance";

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).NumberFormatLocal = "@";
                for (int i = 0; i < dtBeiAnDanList.Rows.Count; i++)
                {
                    for (int j = 0; j < dtBeiAnDanList.Columns.Count; j++)
                    { worksheet.Cells[i + 2, j + 1] = dtBeiAnDanList.Rows[i][j].ToString().Trim(); }
                }

                for (int k = 0; k < dtBeiAnDanList.Columns.Count; k++)
                { worksheet.Cells[1, k + 1] = dtBeiAnDanList.Columns[k].ColumnName.Trim(); }

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, dtBeiAnDanList.Columns.Count]).Font.Bold = true;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).Font.Name = "Verdana";
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).Font.Size = 9;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtBeiAnDanList.Rows.Count + 1, dtBeiAnDanList.Columns.Count]).Borders.LineStyle = 1;
                worksheet.Cells.EntireColumn.AutoFit();

                object missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, missing, missing);
                worksheet.Name = "BeiAnDan_DroolsBalance";

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).NumberFormatLocal = "@";
                for (int i = 0; i < dtDroolsList.Rows.Count; i++)
                {
                    for (int j = 0; j < dtDroolsList.Columns.Count; j++)
                    { worksheet.Cells[i + 2, j + 1] = dtDroolsList.Rows[i][j].ToString().Trim(); }
                }

                for (int k = 0; k < dtDroolsList.Columns.Count; k++)
                { worksheet.Cells[1, k + 1] = dtDroolsList.Columns[k].ColumnName.Trim(); }

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, dtDroolsList.Columns.Count]).Font.Bold = true;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).Font.Name = "Verdana";
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).Font.Size = 9;
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dtDroolsList.Rows.Count + 1, dtDroolsList.Columns.Count]).Borders.LineStyle = 1;
                worksheet.Cells.EntireColumn.AutoFit();

                excel.Visible = true;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                worksheet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
            }

            dtBeiAnDanList.Dispose();
            dtDroolsList.Dispose();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        //------------------- BOM Calculate RM Usage -------------------//
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When upload the related data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                            "\n\t成品备件号\n\t成品数量", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void btnSearchPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtSearchPath.Text = openDlg.FileName;
        }

        private void btnUploadDownload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtSearchPath.Text.Trim()))
            {
                MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnSearch.Focus();
                return;
            }
            try
            {
                bool bJudge = this.txtSearchPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtSearchPath.Text.Trim(), bJudge);
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
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", myConn);
            DataTable myTable = new DataTable();
            myAdapter.Fill(myTable);
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

            DataTable dt1 = myTable.Copy();
            dt1.Columns.RemoveAt(1);
            dt1.Columns[0].ColumnName = "FG EHB";

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (myConn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlCommand Comm = new SqlCommand();
            Comm.Connection = Conn;
            Comm.CommandType = CommandType.StoredProcedure;
            Comm.CommandText = @"usp_ObtainRMUsageByBOM";
            Comm.Parameters.AddWithValue("@TVP_RMUsage", dt1);
            SqlDataAdapter Adpter = new SqlDataAdapter();
            Adpter.SelectCommand = Comm;
            DataTable dt2 = new DataTable();
            Adpter.Fill(dt2);
            Adpter.Dispose();
            Comm.Parameters.Clear();
            Comm.Dispose();
            dt1.Clear();
            dt1.Dispose();

            DataRow[] drUpdate = dt2.Select("[类型] = 0");
            if (drUpdate.Length > 0)
            {
                string strFGEHB = String.Empty;
                int iLineNo = 0;
                for (int iCol = 0; iCol < dt2.Rows.Count; iCol++)
                {
                    if (String.Compare(strFGEHB, dt2.Rows[iCol][1].ToString().Trim()) == 0) { dt2.Rows[iCol][2] = ++iLineNo; }
                    else
                    {
                        iLineNo = 1;
                        dt2.Rows[iCol][2] = iLineNo;
                        strFGEHB = dt2.Rows[iCol][1].ToString().Trim();
                    }
                }
            }
            dt2.Columns.RemoveAt(0);
            dt2.AcceptChanges();

            SqlLib Lib = new SqlLib();
            DataTable rmTable = Lib.MergeDataTable(dt2, myTable, "成品备件号");
            Lib.Dispose(0);
            myTable.Clear();
            myTable.Dispose();           
            dt2.Clear();
            dt2.Dispose();
            rmTable.Columns.Add("RM Usage", typeof(decimal));
            rmTable.Columns.Add("Drools Qty", typeof(decimal));

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.get_Range(excel.Cells[1, 1], excel.Cells[rmTable.Rows.Count + 1, 11]).NumberFormatLocal = "@";
            for (int i = 0; i < rmTable.Rows.Count; i++)
            {
                decimal dConsumption = Convert.ToDecimal(rmTable.Rows[i][3].ToString().Trim());
                decimal dQtyLossRate = Convert.ToDecimal(rmTable.Rows[i][4].ToString().Trim());
                decimal dFGQty = Convert.ToDecimal(rmTable.Rows[i][8].ToString().Trim());
                decimal dRMUsage = Math.Round(dFGQty * dConsumption / (1 - dQtyLossRate / 100), 6);
                decimal dDroolsQty = Math.Round(dFGQty * dConsumption * dQtyLossRate / (100 - dQtyLossRate), 6);

                excel.Cells[i + 2, 1] = rmTable.Rows[i][0].ToString().Trim();
                excel.Cells[i + 2, 2] = rmTable.Rows[i][1].ToString().Trim();
                excel.Cells[i + 2, 3] = rmTable.Rows[i][2].ToString().Trim();
                excel.Cells[i + 2, 4] = String.Format("{0:0.000000}", dConsumption);
                excel.Cells[i + 2, 5] = String.Format("{0:0.000000}", dQtyLossRate);
                excel.Cells[i + 2, 6] = String.Format("{0:0.000000}", dQtyLossRate);
                excel.Cells[i + 2, 7] = rmTable.Rows[i][6].ToString().Trim();
                excel.Cells[i + 2, 8] = rmTable.Rows[i][7].ToString().Trim();
                excel.Cells[i + 2, 9] = String.Format("{0:0.000000}", dFGQty);
                excel.Cells[i + 2, 10] = String.Format("{0:0.000000}", dRMUsage);
                excel.Cells[i + 2, 11] = String.Format("{0:0.000000}", dDroolsQty);
            }

            excel.Cells[1, 1] = "成品备件号";
            excel.Cells[1, 2] = "项号";
            excel.Cells[1, 3] = "原料备件号";
            excel.Cells[1, 4] = "净耗";
            excel.Cells[1, 5] = "数量损耗率(%)";
            excel.Cells[1, 6] = "重量损耗率(%)";
            excel.Cells[1, 7] = "注释";
            excel.Cells[1, 8] = "废料备件号";
            excel.Cells[1, 9] = "成品数量";
            excel.Cells[1, 10] = "RM Usage";
            excel.Cells[1, 11] = "Drools Qty";

            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 12]).Font.Bold = true;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[rmTable.Rows.Count + 1, 12]).Font.Name = "Verdana";
            excel.get_Range(excel.Cells[1, 1], excel.Cells[rmTable.Rows.Count + 1, 12]).Font.Size = 9;
            excel.Cells.EntireColumn.AutoFit();
            excel.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        //--------- Cancel Multi Account for RM Balance Control ---------//
        private void button1_Click(object sender, EventArgs e)
        {
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand("DELETE FROM B_MonitorMultipleUsers", SqlConn);
            sqlComm.ExecuteNonQuery();
            sqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open) { SqlConn.Close(); }
            MessageBox.Show("Successfully cancel multi account for RM Balance control.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
