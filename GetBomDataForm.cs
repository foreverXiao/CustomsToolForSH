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
    public partial class GetBomDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable RateTable = new DataTable();
        protected DataView dvFillDGV = new DataView();
        protected DataTable dtFillDGV = new DataTable();
        string strFilter = null;
        protected PopUpFilterForm filterFrm = null;

        private PopUpCategoryForm PopUpCategoryFrm = null;
        public PopUpCategoryForm PopUpCategory
        {
            get { return PopUpCategoryFrm; }
            set { PopUpCategoryFrm = value; }
        }

        private string strLotNo = null;
        public string LotNo
        {
            get { return strLotNo; }
            set { strLotNo = value; }
        }

        public GetBomDataForm()
        {
            InitializeComponent();
        }
        private static GetBomDataForm getBomListFrm;
        public static GetBomDataForm CreateInstance()
        {
            if (getBomListFrm == null || getBomListFrm.IsDisposed)
            {
                getBomListFrm = new GetBomDataForm();
            }
            return getBomListFrm;
        }

        private void GetBomDataForm_Load(object sender, EventArgs e)
        {
            this.GetDgvData(true);
            if (this.dgvBomList.RowCount > 0)
            {
                this.SetBoolRight(true);

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);

                this.dgvBomList.EnableHeadersVisualStyles = false;
                this.dgvBomList.Columns["Batch Path"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["Batch No"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["FG No"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["RM Qty"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["Line No"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["Item No"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["Lot No"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["FG Qty"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["Inventory Type"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["RM Category"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["Actual Start Date"].HeaderCell.Style = cellStyle;
                this.dgvBomList.Columns["Actual Close Date"].HeaderCell.Style = cellStyle;
            }
        }

        private void GetBomDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RateTable.Dispose();
        }

        private void dgvBomList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvBomList.RowCount; i++) { this.dgvBomList[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvBomList.RowCount; i++) { this.dgvBomList[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvBomList.RowCount; i++)
                    {
                        if (String.Compare(this.dgvBomList[0, i].EditedFormattedValue.ToString(),"False") == 0) { this.dgvBomList[0, i].Value = true; }

                        else { this.dgvBomList[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvBomList_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvBomList.RowCount == 0) { return; }
            if (this.dgvBomList.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvBomList.RowCount; i++)
                {
                    if (String.Compare(this.dgvBomList[0, i].EditedFormattedValue.ToString(),"True") == 0) 
                    { iCount++; } 
                }

                if (iCount < this.dgvBomList.RowCount && iCount > 0)
                { this.dgvBomList.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvBomList.RowCount)
                { this.dgvBomList.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvBomList.Columns[0].HeaderText = "全选"; }
            }
        }

        private void dgvBomList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {                   
                    SqlConnection delConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (delConn.State == ConnectionState.Closed) { delConn.Open(); }
                    SqlCommand delComm = new SqlCommand();
                    delComm.Connection = delConn;

                    int iRow = this.dgvBomList.CurrentRow.Index;
                    string strBatchNo = this.dgvBomList["Batch No", iRow].Value.ToString().Trim();
                    string strFGNo = this.dgvBomList["FG No", iRow].Value.ToString().Trim();
                    
                    //Remove the selected BOM data in DataGridView
                    DataRow[] dRow = dvFillDGV.Table.Select("[Batch No] = '" + strBatchNo + "' AND [FG No] = '" + strFGNo + "'");
                    foreach (DataRow dr in dRow) { dr.Delete(); }
                    dtFillDGV.AcceptChanges();

                    //Delete the selected BOM data in DataBase
                    delComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    delComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                    delComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
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
                    finally
                    {
                        delComm.Parameters.Clear();
                        delComm.Dispose();
                        if (delConn.State == ConnectionState.Open)
                        {
                            delConn.Close();
                            delConn.Dispose();
                        }
                    }
                }
            }
        }

        private void GetDgvData(bool bJudge) 
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }

            string strOLE = null;
            if (bJudge == true)
            {
                strOLE = @"SELECT [Batch Path], [Batch No], [FG No], [Line No], [Item No], [Lot No], [FG Qty], [RM Qty], [Total Input Qty], [Drools Qty], [Inventory Type], " +
                          "[RM Category], [Actual Start Date], [Actual Close Date], [Judge Qty], [Judge Yield] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            }
            else if (bJudge == false)
            {
                strOLE = @"SELECT [Batch Path], [Batch No], [FG No], [Line No], [Item No], [Lot No], [FG Qty], [Total Input Qty], [Drools Qty], [Inventory Type], " +
                          "[RM Category], [Order Price], [Order Currency], [Total RM Cost(USD)], [RM Qty], [RM Currency], [RM Price], [RM Customs Code], [BGD No], " +
                          "[Consumption], [Qty Loss Rate], [HS Code], [CHN Name], [Drools EHB], [Note], [Actual Start Date], [Actual Close Date], [Judge Qty], " +
                          "[Judge Yield], [Judge Price] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            }

            SqlDataAdapter SqlAdapter = new SqlDataAdapter(strOLE, SqlConn);
            dtFillDGV.Clear();
            SqlAdapter.Fill(dtFillDGV);
            dvFillDGV = dtFillDGV.DefaultView;
            if (dtFillDGV.Rows.Count == 0)
            {
                dtFillDGV.Clear();
                dtFillDGV.Dispose();
                this.dgvBomList.DataSource = DBNull.Value;
            }
            else
            {
                this.dgvBomList.DataSource = dvFillDGV;
                this.dgvBomList.Columns[0].HeaderText = "全选";
            }

            SqlAdapter.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void btnCheckQtyYield_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnCheckQtyYield.Focus();
                return;
            }

            SqlConnection myConnection = new SqlConnection(SqlLib.StrSqlConnection);
            if (myConnection.State == ConnectionState.Closed) { myConnection.Open(); }      
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = myConnection;

            myCommand.CommandText = @"SELECT * FROM B_YieldRatio"; 
            SqlDataAdapter myDataAdapter = new SqlDataAdapter();
            myDataAdapter.SelectCommand = myCommand;
            DataTable myDataTable = new DataTable();
            myDataAdapter.Fill(myDataTable);
            decimal dBeginRatio = Convert.ToDecimal(myDataTable.Rows[0][0].ToString().Trim());

            myCommand.CommandText = @"SELECT DISTINCT [Batch No], [FG No], [FG Qty], SUM([RM Qty]) AS [RMQty], MAX([Line No]), 'False' AS JudgeQty, 'False' AS JudgeYield FROM M_DailyBOM GROUP BY [Batch No], [FG No], [FG Qty]";
            myDataAdapter.SelectCommand = myCommand;
            myDataTable.Reset();
            myDataAdapter.Fill(myDataTable);
            myDataAdapter.Dispose();          

            int iExceptionQty = 0, iExceptionYield = 0;
            string strExceptionQty = null, strExceptionYield = null;
            bool bYield = false, bQty = false;
            for (int i = 0; i < myDataTable.Rows.Count; i++)
            {
                bQty = false;
                bYield = false; 
                decimal dFGQty = Convert.ToDecimal(myDataTable.Rows[i][2].ToString().Trim());
                decimal dTotalInputQty = Convert.ToDecimal(myDataTable.Rows[i][3].ToString().Trim());
                decimal dYieldRate = Math.Round(dFGQty / dTotalInputQty, 2);

                int iCompareQty = Decimal.Compare(dFGQty, dTotalInputQty);
                int iCompareYieldBegin = Decimal.Compare(dBeginRatio, dYieldRate);
                if (iCompareQty == 1 && iCompareYieldBegin != 1)
                {
                    bQty = true;
                    myDataTable.Rows[i]["JudgeQty"] = "True";
                    iExceptionQty++;
                    strExceptionQty += myDataTable.Rows[i][0].ToString().Trim() + "; ";
                }
                else if (iCompareQty != 1 && iCompareYieldBegin == 1)
                {
                    bYield = true;
                    myDataTable.Rows[i]["JudgeYield"] = "True";
                    iExceptionYield++;
                    strExceptionYield += myDataTable.Rows[i][0].ToString().Trim() + "; ";
                }

                myCommand.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = dTotalInputQty;
                myCommand.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = dTotalInputQty - dFGQty;
                myCommand.Parameters.Add("@JudgeQty", SqlDbType.Bit).Value = bQty;
                myCommand.Parameters.Add("@JudgeYiled", SqlDbType.Bit).Value = bYield;
                myCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = myDataTable.Rows[i][0].ToString().Trim();
                myCommand.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = myDataTable.Rows[i][1].ToString().Trim();
                myCommand.CommandText = @"UPDATE M_DailyBOM SET [Total Input Qty] = @TotalInputQty, [Drools Qty] = @DroolsQty, [Judge Qty] = @JudgeQty, [Judge Yield] = @JudgeYiled WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";

                SqlTransaction myTransaction = myConnection.BeginTransaction();
                myCommand.Transaction = myTransaction;
                try
                {
                    myCommand.ExecuteNonQuery();
                    myTransaction.Commit();
                }
                catch (Exception)
                {
                    myTransaction.Rollback();
                    myTransaction.Dispose();
                    throw;
                }
                finally
                { myCommand.Parameters.Clear(); }
            }      

            if (iExceptionQty == 0 && iExceptionYield == 0)
            {
                MessageBox.Show("No abnormal data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.GetDgvData(true);
                this.SetBoolRight(false);
                if (!String.IsNullOrEmpty(this.txtSearchPath.Text.Trim())) { this.txtSearchPath.Text = String.Empty; }
                return;
            }
            else
            {
                this.GetDgvData(true);
                this.SetBoolRight(false);             
                if (!String.IsNullOrEmpty(this.txtSearchPath.Text.Trim())) { this.txtSearchPath.Text = String.Empty; }

                string strRecord = null;
                if (iExceptionQty > 0 && iExceptionYield == 0)
                { strRecord = "There are " + iExceptionQty.ToString().Trim() + " Batch No that Total Output Qty greater than Total Input Qty:\n" + strExceptionQty; }
                else if (iExceptionQty == 0 && iExceptionYield > 0)
                { strRecord = "There are " + iExceptionYield.ToString().Trim() + " Batch No that Yield Ratio less than the minimum range:\n" + strExceptionYield; }
                else
                {
                    strRecord = "There are " + iExceptionQty.ToString().Trim() + " Batch No that Total Output Qty greater than Total Input Qty:\n" + strExceptionQty +
                                "\nThere are " + iExceptionYield.ToString().Trim() + " Batch No that Yield Ratio less than the minimum range:\n" + strExceptionYield;
                }
                MessageBox.Show(strRecord, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            myDataTable.Clear();
            myDataTable.Dispose();
            myCommand.Dispose();
            if (myConnection.State == ConnectionState.Open)
            {
                myConnection.Close();
                myConnection.Dispose();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            if (this.dgvBomList.Columns[0].HeaderText != "全选")
            {
                bool bJudge = false;
                int iRow = 2;
                for (int i = 0; i < this.dgvBomList.RowCount; i++)
                {
                    if (String.Compare(this.dgvBomList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        bJudge = true;
                        excel.get_Range(excel.Cells[iRow, 1], excel.Cells[iRow, this.dgvBomList.ColumnCount - 3]).NumberFormatLocal = "@";
                        for (int j = 3; j < this.dgvBomList.ColumnCount; j++)
                        { excel.Cells[iRow, j - 2] = this.dgvBomList[j, i].Value.ToString().Trim(); }
                        iRow++;
                    }
                }

                if (bJudge)
                {
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvBomList.ColumnCount - 3]).NumberFormatLocal = "@";
                    for (int k = 3; k < this.dgvBomList.ColumnCount; k++)
                    { excel.Cells[1, k - 2] = this.dgvBomList.Columns[k].HeaderText.ToString(); }

                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvBomList.ColumnCount - 3]).Font.Bold = true;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvBomList.ColumnCount - 3]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvBomList.ColumnCount - 3]).Font.Name = "Verdana";
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvBomList.ColumnCount - 3]).Font.Size = 9;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvBomList.ColumnCount - 3]).Borders.LineStyle = 1;
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Visible = true;
                }
            }
            else
            {
                excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvBomList.RowCount + 1, this.dgvBomList.ColumnCount - 3]).NumberFormatLocal = "@";
                for (int i = 0; i < this.dgvBomList.RowCount; i++)
                {
                    for (int j = 3; j < this.dgvBomList.ColumnCount; j++)
                    { excel.Cells[i + 2, j - 2] = this.dgvBomList[j, i].Value.ToString().Trim(); }
                }
              
                for (int k = 3; k < this.dgvBomList.ColumnCount; k++)
                { excel.Cells[1, k - 2] = this.dgvBomList.Columns[k].HeaderText.ToString(); }

                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvBomList.ColumnCount - 3]).Font.Bold = true;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvBomList.ColumnCount - 3]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvBomList.RowCount + 1, this.dgvBomList.ColumnCount - 3]).Font.Name = "Verdana";
                excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvBomList.RowCount + 1, this.dgvBomList.ColumnCount - 3]).Font.Size = 9;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvBomList.RowCount + 1, this.dgvBomList.ColumnCount - 3]).Borders.LineStyle = 1;
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
            this.txtSearchPath.Text = openDlg.FileName;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtSearchPath.Text.Trim()))
            {
                MessageBox.Show("Please select the uploading path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnSearch.Focus();
                return;
            }
            try
            {
                bool bJudge = this.txtSearchPath.Text.Contains(".xlsx");
                bool bDgvColumns;
                this.ImportExcelData(this.txtSearchPath.Text.Trim(), bJudge, out bDgvColumns);

                this.GetDgvData(bDgvColumns);
                this.SetBoolRight(true);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Upload error, please try again.\n" + ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void ImportExcelData(string strFilePath, bool bJudge, out bool bDgvColumnCount)
        {
            bDgvColumnCount = true;
            string strConn;
            if (bJudge)
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else
            { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$] where [Batch No] IS NOT NULL", myConn);
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

            SqlConnection oneConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (oneConn.State == ConnectionState.Closed) { oneConn.Open(); }
            SqlCommand oneComm = new SqlCommand();
            oneComm.Connection = oneConn;

            string strBatch = null;
            for (int i = 0; i < myTable.Rows.Count; i++)
            {
                string strBatchNo = this.GetBatchFormat(myTable.Rows[i]["Batch No"].ToString().Trim());
                string strFGNo = myTable.Rows[i]["FG No"].ToString().Trim();
                if (String.Compare(strBatch, strBatchNo) != 0)
                {
                    strBatch = strBatchNo;
                    oneComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    oneComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                    oneComm.CommandText = @"SELECT COUNT(*) FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                    int iCount = Convert.ToInt32(oneComm.ExecuteScalar());
                    if (iCount > 0)
                    {
                        oneComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                        oneComm.ExecuteNonQuery();
                    }
                }                            

                oneComm.Parameters.Clear();
                if (myTable.Columns.Count > 16) //Mapping GetDgvData(strName) function; basic data have 16 fields, all data have 30 fields;
                {
                    oneComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = myTable.Rows[i]["Actual Start Date"].ToString().Trim();
                    oneComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = myTable.Rows[i]["Actual Close Date"].ToString().Trim();
                    oneComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Inventory Type"].ToString().Trim();
                    oneComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Category"].ToString().Trim();
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Order Price"].ToString().Trim())) { oneComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Order Price"].ToString().Trim())), 4); }
                    oneComm.Parameters.Add("@OrderCurrency", SqlDbType.NVarChar).Value = myTable.Rows[i]["Order Currency"].ToString().Trim();
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Total Input Qty"].ToString().Trim())) { oneComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Total Input Qty"].ToString().Trim())), 11); }
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Total RM Cost(USD)"].ToString().Trim())) { oneComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Total RM Cost(USD)"].ToString().Trim())), 2); }
                    oneComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Customs Code"].ToString().Trim();
                    oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["BGD No"].ToString().Trim();
                    if (String.IsNullOrEmpty(myTable.Rows[i]["RM Qty"].ToString().Trim())) { oneComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["RM Qty"].ToString().Trim())), 11); }
                    oneComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Currency"].ToString().Trim();
                    if (String.IsNullOrEmpty(myTable.Rows[i]["RM Price"].ToString().Trim())) { oneComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["RM Price"].ToString().Trim())), 6); }
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Consumption"].ToString().Trim())) { oneComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Consumption"].ToString().Trim())), 6); }
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Qty Loss Rate"].ToString().Trim())) { oneComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Qty Loss Rate"].ToString().Trim())), 11); } //Equal to 'Drools Rate'                                  
                    oneComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["HS Code"].ToString().Trim();
                    oneComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = myTable.Rows[i]["CHN Name"].ToString().Trim();
                    oneComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = myTable.Rows[i]["Drools EHB"].ToString().Trim();
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Drools Qty"].ToString().Trim())) { oneComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Drools Qty"].ToString().Trim())), 11); }
                    oneComm.Parameters.Add("@Note", SqlDbType.NVarChar).Value = myTable.Rows[i]["Note"].ToString().Trim();
                    oneComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                    oneComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                }
                else
                {
                    oneComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = myTable.Rows[i]["Actual Start Date"].ToString().Trim();
                    oneComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = myTable.Rows[i]["Actual Close Date"].ToString().Trim();
                    oneComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Inventory Type"].ToString().Trim();
                    oneComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Category"].ToString().Trim();
                    if (String.IsNullOrEmpty(myTable.Rows[i]["RM Qty"].ToString().Trim())) { oneComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["RM Qty"].ToString().Trim())), 11); }
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Total Input Qty"].ToString().Trim())) { oneComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Total Input Qty"].ToString().Trim())), 11); }
                    if (String.IsNullOrEmpty(myTable.Rows[i]["Drools Qty"].ToString().Trim())) { oneComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(myTable.Rows[i]["Drools Qty"].ToString().Trim())), 11); }
                    oneComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                    oneComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                }

                oneComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = myTable.Rows[i]["Batch Path"].ToString().Trim();
                oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item No"].ToString().Trim();
                oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot No"].ToString().Trim();
                if (String.IsNullOrEmpty(myTable.Rows[i]["FG Qty"].ToString().Trim())) { oneComm.Parameters.Add("@FGQty", SqlDbType.Int).Value = 0; }
                else { oneComm.Parameters.Add("@FGQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["FG Qty"].ToString().Trim()); }
                oneComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                oneComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                oneComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Line No"].ToString().Trim());

                if (myTable.Columns.Count> 16)
                {
                    bDgvColumnCount = false;
                    oneComm.CommandText = @"INSERT INTO M_DailyBOM([Actual Start Date], [Actual Close Date], [Inventory Type], [RM Category], [Order Price], " +
                                           "[Order Currency], [Total Input Qty], [Total RM Cost(USD)], [RM Customs Code], [BGD No], [RM Qty], [RM Currency], [RM Price], " +
                                           "[Consumption], [Qty Loss Rate], [HS Code], [CHN Name], [Drools EHB], [Drools Qty], [Note], [Created Date], [Creater], " +
                                           "[Batch Path], [Item No], [Lot No], [FG Qty], [Judge Qty], [Judge Yield], [Judge Price], [Batch No], [FG No], [Line No])" +
                                           "VALUES(@ActualStartDate, @ActualCloseDate, @InventoryType, @RMCategory, @OrderPrice, @OrderCurrency, @TotalInputQty, " +
                                           "@TotalRMCost, @RMCustomsCode, @BGDNo, @RMQty, @RMCurrency, @RMPrice, @Consumption, @QtyLossRate, @HSCode, @CHNName, @DroolsEHB, " +
                                           "@DroolsQty, @Note, @CreatedDate, @Creater, @BatchPath, @ItemNo, @LotNo, @FGQty, 'False', 'False', 'False', @BatchNo, @FGNo, @LineNo)";
                }
                else
                {
                    bDgvColumnCount = true;
                    oneComm.CommandText = @"INSERT INTO M_DailyBOM([Actual Start Date], [Actual Close Date], [Inventory Type], [RM Category], [RM Qty], [Total Input Qty], " +
                                           "[Drools Qty], [Created Date], [Creater], [Batch Path], [Item No], [Lot No], [FG Qty], [Judge Qty], [Judge Yield], [Batch No], " +
                                           "[FG No], [Line No]) VALUES(@ActualStartDate, @ActualCloseDate, @InventoryType, @RMCategory, @RMQty, @TotalInputQty, " +
                                           "@DroolsQty, @CreatedDate, @Creater, @BatchPath, @ItemNo, @LotNo, @FGQty, 'False', 'False', @BatchNo, @FGNo, @LineNo)";
                }
                oneComm.ExecuteNonQuery();
                oneComm.Parameters.Clear();
            }

            oneComm.Dispose();
            if (MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                myTable.Clear();
                myTable.Dispose();
                if (oneConn.State == ConnectionState.Open)
                {
                    oneConn.Close();
                    oneConn.Dispose();
                }
            }       
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.RowCount == 0 || (this.dgvBomList.Columns[0].Visible == false && this.dgvBomList.ColumnCount < 20))
            {
                MessageBox.Show("Please click '¢±&¢²Check' button first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnCheckQtyYield.Focus();
                return;
            }

            SqlConnection exeConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (exeConn.State == ConnectionState.Closed) { exeConn.Open(); }
            SqlCommand exeComm = new SqlCommand();
            exeComm.Connection = exeConn;

            string strSQL = @"SELECT DISTINCT [Batch No], [FG No], COUNT([Line No]) AS MaxLine, [FG Qty], [Total Input Qty] FROM M_DailyBOM GROUP BY [Batch No], [FG No], [FG Qty], [Total Input Qty]";
            SqlDataAdapter exeAdapter = new SqlDataAdapter(strSQL, exeConn);
            DataTable exeTable = new DataTable();
            exeTable.Clear();
            exeAdapter.Fill(exeTable);         

            #region //Gather master columns data: Order Price, Order Currency, Qty Loss Rate, HS Code, CHN Name
            for (int i = 0; i < exeTable.Rows.Count; i++)
            {                                           
                decimal dPrice = 0.0M;
                string strCurrency = null, strHSCode = null, strCHNName = null;

                #region //Gather OrderPrice and OrderCurrency using three method recursively
                //Since exists the same BOM No, but the difference is whether existed suffix or not
                exeComm.Parameters.Clear();
                if (exeTable.Rows[i]["Batch No"].ToString().Trim().Length > 8)
                { exeComm.Parameters.Add("@BatchNoSuffix", SqlDbType.NVarChar).Value = exeTable.Rows[i]["Batch No"].ToString().Trim().Substring(0, 8); }
                else { exeComm.Parameters.Add("@BatchNoSuffix", SqlDbType.NVarChar).Value = exeTable.Rows[i]["Batch No"].ToString().Trim(); }
                exeComm.CommandText = @"SELECT COUNT(*) FROM A_eShipment WHERE SUBSTRING([LotNo], 1, 8) = @BatchNoSuffix"; //Note: not use @BatchNo
                int iLotNo = Convert.ToInt32(exeComm.ExecuteScalar());
                if (iLotNo > 0)
                {
                    exeComm.CommandText = @"SELECT [External Price], [Curr] FROM A_eShipment WHERE SUBSTRING([LotNo], 1, 8) = @BatchNoSuffix"; 
                    SqlDataReader exeReader1 = exeComm.ExecuteReader();
                    while (exeReader1.Read())
                    {
                        if (exeReader1.HasRows)
                        {
                            dPrice = Convert.ToDecimal(exeReader1.GetValue(0).ToString().Trim()); 
                            strCurrency = exeReader1.GetValue(1).ToString().Trim();
                        }
                    }
                    exeReader1.Close();
                    exeReader1.Dispose();
                }
                else
                {
                    exeComm.Parameters.Clear();
                    exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = exeTable.Rows[i]["FG No"].ToString().Trim();
                    exeComm.CommandText = @"SELECT COUNT(*) FROM A_eShipment WHERE SUBSTRING([PartNo], 1, LEN([PartNo]) - 4) = @FGNo";
                    int iPartNo = Convert.ToInt32(exeComm.ExecuteScalar());
                    if (iPartNo > 0)
                    {
                        exeComm.CommandText = @"SELECT MIN([External Price]), [Curr] FROM A_eShipment WHERE SUBSTRING([PartNo], 1, LEN([PartNo]) - 4) = @FGNo GROUP BY [Curr]";
                        SqlDataReader exeReader2 = exeComm.ExecuteReader();
                        while (exeReader2.Read())
                        {
                            if (exeReader2.HasRows)
                            {
                                dPrice = Convert.ToDecimal(exeReader2.GetValue(0).ToString().Trim()); 
                                strCurrency = exeReader2.GetValue(1).ToString().Trim();
                                if (String.Compare(strCurrency, "USD") == 0) break; //Set 'USD' as default value, if there is a variety of currency
                            }
                        }
                        exeReader2.Close();
                        exeReader2.Dispose();
                    }
                }
                if (dPrice == 0.0M)
                {
                    exeComm.Parameters.Clear();
                    exeComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = exeTable.Rows[i]["FG No"].ToString().Split('-').GetValue(0).ToString().Trim();
                    exeComm.CommandText = @"SELECT DISTINCT [Price] FROM B_FinancePrice WHERE [Grade] = @Grade ORDER BY [Price] DESC";
                    SqlDataReader exeReader3 = exeComm.ExecuteReader();
                    while (exeReader3.Read())
                    {
                        if (exeReader3.HasRows)
                        {
                            dPrice = Convert.ToDecimal(exeReader3.GetValue(0).ToString().Trim());
                            strCurrency = "USD";
                        }
                    }

                    if (String.IsNullOrEmpty(strCurrency)) { strCurrency = string.Empty; }
                    exeReader3.Close();
                    exeReader3.Dispose();
                }
                #endregion
            
                #region //Gather HS Code and Chinese Name
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = exeTable.Rows[i]["FG No"].ToString().Split('-').GetValue(0).ToString().Trim();
                exeComm.CommandText = @"SELECT [HS Code], [CHN Name] FROM B_HS WHERE [Grade] = @Grade";
                SqlDataReader exeReader4 = exeComm.ExecuteReader();
                while (exeReader4.Read())
                {
                    if (exeReader4.HasRows)
                    {
                        strHSCode = exeReader4.GetValue(0).ToString().Trim();
                        strCHNName = exeReader4.GetValue(1).ToString().Trim();
                    }
                }

                if (String.IsNullOrEmpty(strHSCode))
                { strHSCode = string.Empty; }
                if (String.IsNullOrEmpty(strCHNName))
                { strCHNName = string.Empty; }
                exeReader4.Close();
                exeReader4.Dispose();
                #endregion

                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(dPrice, 4);
                exeComm.Parameters.Add("@OrderCurrency", SqlDbType.NVarChar).Value = strCurrency;
                decimal dDroolsRate = 1 - Convert.ToDecimal(exeTable.Rows[i]["FG Qty"].ToString().Trim()) / Convert.ToDecimal(exeTable.Rows[i]["Total Input Qty"].ToString().Trim());
                exeComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = Math.Round(dDroolsRate * 100.0M, 11);
                exeComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = strHSCode;
                exeComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = strCHNName;
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = exeTable.Rows[i]["Batch No"].ToString().Trim();
                exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = exeTable.Rows[i]["FG No"].ToString().Trim();

                int iLine = Convert.ToInt32(exeTable.Rows[i]["MaxLine"].ToString().Trim());
                for (int j = 0; j < iLine; j++)
                {
                    exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = j + 1;
                    exeComm.CommandText = @"UPDATE M_DailyBOM SET [Order Price] = @OrderPrice, [Order Currency] = @OrderCurrency, [Qty Loss Rate] = @QtyLossRate, " +
                                           "[HS Code] = @HSCode, [CHN Name] = @CHNName WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo AND [Line No] = @LineNo";

                    SqlTransaction exeTrans = exeConn.BeginTransaction();
                    exeComm.Transaction = exeTrans;
                    try
                    {
                        exeComm.ExecuteNonQuery();
                        exeTrans.Commit();                    
                    }
                    catch (Exception)
                    {
                        exeTrans.Rollback();
                        exeTrans.Dispose();
                        throw;
                    }
                    finally
                    { exeComm.Parameters.RemoveAt("@LineNo"); }
                }
            }
            #endregion

            exeComm.CommandText = @"SELECT [Batch No], [FG No], [Line No], [Item No], [Lot No], [RM Category], [RM Qty], [Total Input Qty], [CHN Name], [RM Customs Code], " + 
                                   "[BGD No], [RM Currency], [Drools EHB], [Note], [RM Price] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            exeAdapter.SelectCommand = exeComm;
            exeTable.Reset();
            exeAdapter.Fill(exeTable);

            #region //Gather detail columns data: RM Customs Code, BGD No, RM Currency, RM Price, Drools EHB, Goods Type, Consumption.
            //Prompt: in C_RMPurchase table, contain 8A of BGD No, and Non-8A of BGD No too.
            for (int m = 0; m < exeTable.Rows.Count; m++)
            {
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = exeTable.Rows[m]["Batch No"].ToString().Trim();
                exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = exeTable.Rows[m]["FG No"].ToString().Trim();
                exeComm.CommandText = @"SELECT MAX([Line No]) FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                int iLineCount = Convert.ToInt32(exeComm.ExecuteScalar());

                for (int n = 0; n < iLineCount; n++)
                {
                    string strItemNo = exeTable.Rows[m + n]["Item No"].ToString().Trim();
                    string strLotNo = exeTable.Rows[m + n]["Lot No"].ToString().Trim();
                    string strRMCategory = exeTable.Rows[m + n]["RM Category"].ToString().Trim();
                    string strRMCustomsCode = exeTable.Rows[m + n]["RM Customs Code"].ToString().Trim();
                    string strBGDNo = exeTable.Rows[m + n]["BGD No"].ToString().Trim();
                    string strRMCurrency = exeTable.Rows[m + n]["RM Currency"].ToString().Trim();
                    string strDroolsEHB = exeTable.Rows[m + n]["Drools EHB"].ToString().Trim();
                    string strGoodsType = exeTable.Rows[m + n]["Note"].ToString().Trim();
                    string strRMPrice = exeTable.Rows[m + n]["RM Price"].ToString().Trim();
                    decimal dRMPrice = 0.0M;
                    if (String.IsNullOrEmpty(strRMPrice)) { dRMPrice = 0.0M; }
                    else { dRMPrice = Convert.ToDecimal(strRMPrice); }

                    #region //Gather RM Customs Code, BGD No, RM Currency and RM Price
                    exeComm.Parameters.Clear();
                    if (strLotNo.Contains("/"))
                    {
                        exeComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        exeComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        exeComm.CommandText = @"SELECT [RM Customs Code], [BGD No], [RM Price(CIF)], [RM Currency] FROM C_RMPurchase WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                        SqlDataReader exeReader5 = exeComm.ExecuteReader();
                        while (exeReader5.Read())
                        {
                            if (exeReader5.HasRows)
                            {
                                strRMCustomsCode = exeReader5.GetValue(0).ToString().Trim();
                                strBGDNo = exeReader5.GetValue(1).ToString().Trim();
                                string strRMPriceCIF = exeReader5.GetValue(2).ToString().Trim();
                                if (String.IsNullOrEmpty(strRMPriceCIF)) { dRMPrice = 0.0M; }
                                else { dRMPrice = Convert.ToDecimal(strRMPriceCIF); }
                                strRMCurrency = exeReader5.GetValue(3).ToString().Trim();
                            }
                        }
                        exeReader5.Close();
                        exeReader5.Dispose();
                    }
                    else
                    {
                        strBGDNo = "AAAAAAAA";
                        if (strLotNo.Length > 6)
                        {
                            string strLOTNO = strLotNo.Substring(6, 1).Trim().ToUpper();
                            if (strLOTNO.Contains("R")) { strRMCurrency = "RMB"; }
                            else if (strLOTNO.Contains("U") || strLOTNO.Contains("B") || strLOTNO.Contains("X")) { strRMCurrency = "USD"; }
                            else { strRMCurrency = String.Empty; }
                            exeComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                            exeComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strRMCurrency;
                            exeComm.CommandText = @"SELECT [RM Customs Code], [RM Price] FROM B_8A_Reference WHERE [Item No] = @ItemNo AND [RM Currency] = @RMCurrency";
                            SqlDataReader exeReader5 = exeComm.ExecuteReader();
                            while (exeReader5.Read())
                            {
                                if (exeReader5.HasRows)
                                {
                                    strRMCustomsCode = exeReader5.GetValue(0).ToString().Trim();
                                    if (String.IsNullOrEmpty(exeReader5.GetValue(1).ToString().Trim())) { dRMPrice = 0.0M; }
                                    else { dRMPrice = Convert.ToDecimal(exeReader5.GetValue(1).ToString().Trim()); }
                                }
                            }
                            exeReader5.Close();
                            exeReader5.Dispose();
                        }

                        if (strLotNo.Length == 6)
                        {
                            exeComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                            exeComm.CommandText = @"SELECT [Remark] FROM B_MTI_Prefix WHERE [Type] = 'LOT' AND [MTI Prefix] = @LotNo";
                            string strRemark = Convert.ToString(exeComm.ExecuteScalar());
                            exeComm.Parameters.Clear();
                            if (!String.IsNullOrEmpty(strRemark))
                            {                               
                                strRMCurrency = strRemark;
                                exeComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = exeTable.Rows[m + n]["Item No"].ToString().Trim();
                                exeComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strRMCurrency;
                                exeComm.CommandText = @"SELECT [RM Customs Code], [RM Price] FROM B_8A_Reference WHERE [Item No] = @ItemNo AND [RM Currency] = @RMCurrency";
                                SqlDataReader exeReader5 = exeComm.ExecuteReader();
                                while (exeReader5.Read())
                                {
                                    if (exeReader5.HasRows)
                                    {
                                        strRMCustomsCode = exeReader5.GetValue(0).ToString().Trim();
                                        if (String.IsNullOrEmpty(exeReader5.GetValue(1).ToString().Trim())) { dRMPrice = 0.0M; }
                                        else { dRMPrice = Convert.ToDecimal(exeReader5.GetValue(1).ToString().Trim()); }
                                    }
                                }
                                exeReader5.Close();
                                exeReader5.Dispose();
                            }
                            else
                            {
                                strRMCustomsCode = strLotNo;
                                exeComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                                exeComm.CommandText = @"SELECT [RM Currency], [RM Price] FROM B_8A_Reference WHERE [RM Customs Code] = @RMCustomsCode";
                                SqlDataReader exeReader5 = exeComm.ExecuteReader();
                                while (exeReader5.Read())
                                {
                                    if (exeReader5.HasRows)
                                    {
                                        strRMCurrency = exeReader5.GetValue(0).ToString().Trim();
                                        if (String.IsNullOrEmpty(exeReader5.GetValue(1).ToString().Trim())) { dRMPrice = 0.0M; }
                                        else { dRMPrice = Convert.ToDecimal(exeReader5.GetValue(1).ToString().Trim()); }
                                    }
                                }
                                exeReader5.Close();
                                exeReader5.Dispose();
                            }
                        }
                    }
                    #endregion            

                    #region //Gather Drools EHB
                    exeComm.Parameters.Clear();
                    if (!String.IsNullOrEmpty(strRMCategory))
                    {
                        exeComm.Parameters.Add("@ChineseName", SqlDbType.NVarChar).Value = exeTable.Rows[m + n]["CHN Name"].ToString().Trim();
                        exeComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = exeTable.Rows[m + n]["RM Category"].ToString().Trim();
                        exeComm.CommandText = @"SELECT [Drools EHB] FROM B_Drools WHERE [FG CHN Name] = @ChineseName AND [RM Category] = @RMCategory";
                        SqlDataReader exeReader6 = exeComm.ExecuteReader();
                        while (exeReader6.Read())
                        {
                            if (exeReader6.HasRows)
                            { strDroolsEHB = exeReader6.GetValue(0).ToString().Trim(); }
                        }
                        exeReader6.Close();
                        exeReader6.Dispose();
                    }
                    #endregion              

                    #region //Gather Goods Type --> Bonded / Non-bonded
                    if (!String.IsNullOrEmpty(strRMCategory))
                    {
                        if (String.Compare(strRMCategory, "USD") == 0) { strGoodsType = "保税料件"; }
                        else { strGoodsType = "非保料件"; }
                    }
                    else { strGoodsType = String.Empty; }

                    if (String.IsNullOrEmpty(strRMCustomsCode)) { strRMCustomsCode = String.Empty; }
                    if (String.IsNullOrEmpty(strBGDNo)) { strBGDNo = String.Empty; }
                    if (String.IsNullOrEmpty(strRMCurrency)) { strRMCurrency = String.Empty; }
                    if (String.IsNullOrEmpty(strDroolsEHB)) { strDroolsEHB = String.Empty; }
                    #endregion

                    exeComm.Parameters.Clear();
                    exeComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                    exeComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                    exeComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                    exeComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strRMCurrency;
                    decimal d1 = Convert.ToDecimal(exeTable.Rows[m + n]["RM Qty"].ToString().Trim());
                    decimal d2 = Convert.ToDecimal(Double.Parse(exeTable.Rows[m + n]["Total Input Qty"].ToString().Trim()));
                    exeComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(d1 / d2, 6);
                    exeComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(dRMPrice, 6);
                    exeComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = strDroolsEHB;
                    exeComm.Parameters.Add("@GoodsType", SqlDbType.NVarChar).Value = strGoodsType;
                    exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = exeTable.Rows[m]["Batch No"].ToString().Trim();
                    exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = exeTable.Rows[m]["FG No"].ToString().Trim();
                    exeComm.Parameters.Add("@LineCount", SqlDbType.Int).Value = n + 1;

                    exeComm.CommandText = @"UPDATE M_DailyBOM SET [Lot No] = @LotNo, [RM Customs Code] = @RMCustomsCode, [BGD No] = @BGDNo, " +
                                           "[RM Currency] = @RMCurrency, [Consumption] = @Consumption, [RM Price] = @RMPrice, [Drools EHB] = @DroolsEHB, " +
                                           "[Note] = @GoodsType WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo AND [Line No] = @LineCount";

                    SqlTransaction exeTrans2 = exeConn.BeginTransaction();
                    exeComm.Transaction = exeTrans2;
                    try
                    {
                        exeComm.ExecuteNonQuery();
                        exeTrans2.Commit();
                    }
                    catch (Exception)
                    {
                        exeTrans2.Rollback();
                        exeTrans2.Dispose();
                        throw;
                    }
                }
                m = m + iLineCount - 1;
            }
            #endregion

            #region //Optimize Consumption, ensure this column's value equal to 1.0
            exeComm.CommandText = @"SELECT * FROM V_QueryConsumption";
            exeAdapter.SelectCommand = exeComm;
            exeTable.Reset();
            exeAdapter.Fill(exeTable);

            exeComm.Parameters.Clear();
            for (int x = 0; x < exeTable.Rows.Count; x++)
            {
                decimal dSumConsump = Convert.ToDecimal(exeTable.Rows[x]["SUMConsumption"].ToString().Trim());
                decimal dConsump = Convert.ToDecimal(exeTable.Rows[x]["Consump"].ToString().Trim());
                exeComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = dConsump + 1.0M - dSumConsump;
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = exeTable.Rows[x]["Batch No"].ToString().Trim();
                exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = exeTable.Rows[x]["FG No"].ToString().Trim();
                exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(exeTable.Rows[x]["Line"].ToString().Trim());
                exeComm.CommandText = @"UPDATE M_DailyBOM SET [Consumption] = @Consumption WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo AND [Line No] = @LineNo";

                SqlTransaction exeTrans3 = exeConn.BeginTransaction();
                exeComm.Transaction = exeTrans3;
                try
                {
                    exeComm.ExecuteNonQuery();
                    exeTrans3.Commit();
                }
                catch (Exception)
                {
                    exeTrans3.Rollback();
                    exeTrans3.Dispose();
                    throw;
                }
                finally { exeComm.Parameters.Clear(); }
            }
            #endregion

            exeComm.Parameters.Clear();
            exeComm.Dispose();
            exeAdapter.Dispose();
            exeTable.Clear();
            exeTable.Dispose();
            if (exeConn.State == ConnectionState.Open)
            {
                exeConn.Close();
                exeConn.Dispose();
            }

            this.UpdateTotalRMCost();
            if (MessageBox.Show("Extract data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            { 
                this.GetDgvData(false);
                this.SetBoolRight(true);
            }
        }

        private void btnCheckPrice_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.RowCount == 0 || this.dgvBomList.ColumnCount < 20)
            {
                MessageBox.Show("Please click 'Extract Combine' button first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnExtract.Focus();
                return;
            }

            SqlConnection checkConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (checkConn.State == ConnectionState.Closed) { checkConn.Open(); }
            SqlDataAdapter checkAdapter = new SqlDataAdapter(@"SELECT DISTINCT [Batch No], [FG No], [Order Price], [Order Currency], [FG Qty], [Total RM Cost(USD)] FROM M_DailyBOM", checkConn);
            DataTable checkTable = new DataTable();
            checkTable.Clear();
            checkAdapter.Fill(checkTable);
            checkAdapter.Dispose();
            checkTable.Columns.Add("JudgeMTOPrice", typeof(bool));
            checkTable.Columns["JudgeMTOPrice"].DefaultValue = false;

            SqlCommand checkComm = new SqlCommand();
            checkComm.Connection = checkConn;
            checkComm.CommandText = @"SELECT [MTI Prefix] FROM B_MTI_Prefix WHERE [Type] = 'BOM'";
            SqlDataReader checkReader = checkComm.ExecuteReader();
            string strMTI = null;
            while(checkReader.Read())
            {
                if (checkReader.HasRows)
                { strMTI += "SUBSTRING([CustPO], 1, 3) <> '" + checkReader.GetValue(0).ToString().Trim() + "' AND "; }
            }
            checkReader.Close();
            checkReader.Dispose();
            strMTI = strMTI.Substring(0, strMTI.Trim().Length - 4);

            int iJudgeCost = 0;
            string strJudgeCost = null;
            for (int n = 0; n < checkTable.Rows.Count; n++)
            {
                checkComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = checkTable.Rows[n]["Batch No"].ToString().Trim();
                checkComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = checkTable.Rows[n]["FG No"].ToString().Trim();
                checkComm.CommandText = @"SELECT T1.[Batch No], T1.[FG No], T2.[CustPO] FROM (SELECT DISTINCT [Batch No], [FG No] FROM M_DailyBOM) AS T1, " +
                                         "(SELECT [CustPO], [LotNo] FROM A_eShipment WHERE " + strMTI + ") AS T2 " +
                                         "WHERE SUBSTRING(T1.[Batch No], 1, 8) = SUBSTRING(T2.[LotNo], 1, 8) AND T1.[Batch No] = @BatchNo AND T1.[FG No] = @FGNo";
                string strJudgeMTO = Convert.ToString(checkComm.ExecuteScalar());
                if (!String.IsNullOrEmpty(strJudgeMTO))
                {
                    decimal dOrderPrice = 0.0M;
                    if (String.Compare(checkTable.Rows[n]["Order Currency"].ToString().Trim(), "USD") != 0)
                    { dOrderPrice = Math.Round(Convert.ToDecimal(checkTable.Rows[n]["Order Price"].ToString().Trim()) * this.GetExchangeRate(checkTable.Rows[n]["Order Currency"].ToString().Trim()), 4); }
                    else
                    { dOrderPrice = Convert.ToDecimal(checkTable.Rows[n]["Order Price"].ToString().Trim()); }
                    decimal dTotalRMCost = Convert.ToDecimal(checkTable.Rows[n]["Total RM Cost(USD)"].ToString().Trim());
                    decimal dTotalFGValue = Math.Round(dOrderPrice * Convert.ToInt32(checkTable.Rows[n]["FG Qty"].ToString().Trim()), 2);

                    if (Decimal.Compare(dTotalRMCost, dTotalFGValue) == 1)
                    {
                        iJudgeCost++;
                        strJudgeCost += checkTable.Rows[n]["Batch No"].ToString().Trim() + "; ";
                        checkTable.Rows[n]["JudgeMTOPrice"] = true;

                        checkComm.CommandText = @"UPDATE M_DailyBOM SET [Judge Price] = 'True' WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                        checkComm.ExecuteNonQuery();
                    }
                }
                checkComm.Parameters.Clear();              
            }

            if (iJudgeCost == 0)
            {
                MessageBox.Show("No abnormal data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.SetBoolRight(false);
                if (!String.IsNullOrEmpty(this.txtSearchPath.Text.Trim())) { this.txtSearchPath.Text = String.Empty; }
                return;
            }
            else
            {
                this.SetBoolRight(false);
                if (!String.IsNullOrEmpty(this.txtSearchPath.Text.Trim())) { this.txtSearchPath.Text = String.Empty; }
                MessageBox.Show("There are " + iJudgeCost.ToString().Trim() + "Batch No that Total RM Cost is greater than Total FG Value for MTO:" + strJudgeCost, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            checkComm.Dispose();
            checkTable.Clear();
            checkTable.Dispose();
            if (checkConn.State == ConnectionState.Open)
            {
                checkConn.Close();
                checkConn.Dispose();
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

        private string GetBatchFormat(string strBatchNo)
        {
            bool bJudge = strBatchNo.Contains("N");
            int iLength = 0;
            if (bJudge == false) { iLength = strBatchNo.Length; }
            else { iLength = strBatchNo.Split('N')[0].Length; }
            if (iLength == 3) { strBatchNo = "00000" + strBatchNo; }
            else if (iLength == 4) { strBatchNo = "0000" + strBatchNo; }
            else if (iLength == 5) { strBatchNo = "000" + strBatchNo; }
            else if (iLength == 6) { strBatchNo = "00" + strBatchNo; }
            else if (iLength == 7) { strBatchNo = "0" + strBatchNo; }
            return strBatchNo;
        }

        private void SetBoolRight(bool bJudge)
        {
            if (this.dgvBomList.Columns[0].Visible == bJudge) { this.dgvBomList.Columns[0].Visible = !bJudge; }
            if (this.dgvBomList.Columns[1].Visible == bJudge) { this.dgvBomList.Columns[1].Visible = !bJudge; }

            this.dgvBomList.Columns[2].Visible = false;
            this.dgvBomList.Columns["Judge Qty"].Visible = false;
            this.dgvBomList.Columns["Judge Yield"].Visible = false;
            if (this.dgvBomList.ColumnCount > 19)
            { this.dgvBomList.Columns["Judge Price"].Visible = false; }

            for (int i = 3; i < this.dgvBomList.ColumnCount; i++)
            {
                if (!bJudge)
                { this.dgvBomList.Columns[i].ReadOnly = !bJudge; }
                else
                { this.dgvBomList.Columns[i].ReadOnly = bJudge; }
            }
        }

        private void UpdateTotalRMCost()
        {
            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;
            SqlComm.CommandText = @"SELECT [Batch No], [FG No], [Line No], [RM Qty], [RM Currency], [RM Price], [Total RM Cost(USD)] FROM M_DailyBOM ORDER BY [Batch No], [FG No]";
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();
            SqlAdapter.SelectCommand = SqlComm;
            DataTable SqlTable = new DataTable();
            SqlAdapter.Fill(SqlTable);

            foreach (DataRow dr in SqlTable.Rows)
            {
                decimal dRMPrice = 0.0M;
                if (String.Compare(dr["RM Currency"].ToString().Trim(), "USD") != 0)
                { dRMPrice = Math.Round(Convert.ToDecimal(dr["RM Price"].ToString().Trim()) * this.GetExchangeRate(dr["RM Currency"].ToString().Trim()), 12); }
                else { dRMPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim()); }
                dr["Total RM Cost(USD)"] = Math.Round(dRMPrice * Convert.ToDecimal(dr["RM Qty"].ToString().Trim()), 4);
            }
            SqlTable.AcceptChanges();

            SqlComm.CommandText = @"SELECT DISTINCT [Batch No], [FG No] FROM M_DailyBOM";
            SqlAdapter = new SqlDataAdapter();
            SqlAdapter.SelectCommand = SqlComm;
            DataTable dTable = new DataTable();
            SqlAdapter.Fill(dTable);
            SqlAdapter.Dispose();
            foreach (DataRow dRow in dTable.Rows)
            {
                string strBatchNo = dRow["Batch No"].ToString().Trim();
                string strFGNo = dRow["FG No"].ToString().Trim();
                decimal dTotalRMCost = (decimal)SqlTable.Compute("SUM([Total RM Cost(USD)])", "[Batch No] = '" + strBatchNo + "' AND [FG No] = '" + strFGNo + "'");

                SqlComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(dTotalRMCost, 2);
                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                SqlComm.CommandText = @"UPDATE M_DailyBOM SET [Total RM Cost(USD)] = @TotalRMCost WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                SqlComm.ExecuteNonQuery();
                SqlComm.Parameters.Clear();
            }

            SqlTable.Clear();
            SqlTable.Dispose();
            dTable.Clear();
            dTable.Dispose();
            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SqlConnection BrowseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BrowseConn.State == ConnectionState.Closed) { BrowseConn.Open(); }

            string strBOMField = @"SELECT [Batch Path], [Batch No], [FG No], [Line No], [Item No], [Lot No], [FG Qty], [Total Input Qty], [Drools Qty], [Inventory Type], " +
                                  "[RM Category], [Order Price], [Order Currency], [Total RM Cost(USD)], [RM Qty], [RM Currency], [RM Price], [RM Customs Code], [BGD No], " +
                                  "[Consumption], [Qty Loss Rate], [HS Code], [CHN Name], [Drools EHB], [Note], [Actual Start Date], [Actual Close Date], [Created Date], " + 
                                  "[Creater], [Judge Qty], [Judge Yield], [Judge Price] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            SqlDataAdapter BrowseAdapter = new SqlDataAdapter(strBOMField, BrowseConn);
            dtFillDGV.Clear();
            BrowseAdapter.Fill(dtFillDGV);
            dvFillDGV = dtFillDGV.DefaultView;
            if (dtFillDGV.Rows.Count == 0)
            {
                dtFillDGV.Clear();
                dtFillDGV.Dispose();
                this.dgvBomList.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvBomList.DataSource = dvFillDGV;
                this.dgvBomList.Columns[0].HeaderText = "全选"; 
            }

            BrowseAdapter.Dispose();
            if (BrowseConn.State == ConnectionState.Open)
            {
                BrowseConn.Close();
                BrowseConn.Dispose();
            }

            if (this.dgvBomList.RowCount > 0) { this.SetBoolRight(false); }            
            if (!String.IsNullOrEmpty(this.txtSearchPath.Text.Trim())) { this.txtSearchPath.Text = String.Empty; }
        }

        #region //Context Menu Strip function
        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBomList.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBomList.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBomList.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBomList[strColumnName, this.dgvBomList.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] = " + strColumnText; }
                    }
                }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiExcludeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBomList.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBomList.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBomList.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBomList[strColumnName, this.dgvBomList.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBomList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] <> " + strColumnText; }
                    }
                }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            if (this.dgvBomList.ColumnCount < 20) { this.GetDgvData(true); }
            else { this.GetDgvData(false); }
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.CurrentCell != null)
            {
                string strColumnName = this.dgvBomList.Columns[this.dgvBomList.CurrentCell.ColumnIndex].Name;
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
                        if (this.dgvBomList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBomList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBomList.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvBomList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBomList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBomList.Columns[strColumnName].ValueType == typeof(DateTime))
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

        private void tsmiCheckQty_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.Columns[0].Visible == true)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    { strFilter += " AND [Judge Qty] = True"; }
                    else
                    { strFilter = "[Judge Qty] = True"; }
                    dvFillDGV.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }

                for (int i = 0; i < this.dgvBomList.RowCount; i++)
                {
                    DataGridViewRow dgvRow = this.dgvBomList.Rows.SharedRow(i);
                    dgvRow.DefaultCellStyle.BackColor = Color.LightPink;
                }
            }
        }

        private void tsmiCheckYield_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.Columns[0].Visible == true)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    { strFilter += " AND [Judge Yield] = True"; }
                    else
                    { strFilter = "[Judge Yield] = True"; }
                    dvFillDGV.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }

                for (int i = 0; i < this.dgvBomList.RowCount; i++)
                {
                    DataGridViewRow dgvRow = this.dgvBomList.Rows.SharedRow(i);
                    dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
                }
            }
        }

        private void tsmiCheckPrice_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.ColumnCount > 19 && this.dgvBomList.Columns[0].Visible == true)
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

                for (int i = 0; i < this.dgvBomList.RowCount; i++)
                {
                    DataGridViewRow dgvRow = this.dgvBomList.Rows.SharedRow(i);
                    dgvRow.DefaultCellStyle.BackColor = Color.Khaki;
                }
            }
        }

        private void tsmiBlankFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvBomList.ColumnCount > 19 && this.dgvBomList.Columns[0].Visible == true)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        strFilter += @" AND ([Order Price] = 0.0 OR [RM Currency] = '' OR [RM Currency] IS NULL OR [RM Customs Code] = '' OR [RM Customs Code] IS NULL OR [BGD No] = '' OR 
                                       [BGD No] IS NULL OR [HS Code] = '' OR [HS Code] IS NULL OR [Drools EHB] = '' OR [Drools EHB] IS NULL OR [Note] = '' OR [Note] IS NULL)";
                    }
                    else
                    {
                        strFilter = @"([Order Price] = 0.0 OR [RM Currency] = '' OR [RM Currency] IS NULL OR [RM Customs Code] = '' OR [RM Customs Code] IS NULL OR [BGD No] = '' OR [BGD No] IS NULL OR 
                                      [HS Code] = '' OR [HS Code] IS NULL OR [Drools EHB] = '' OR [Drools EHB] IS NULL OR [Note] = '' OR [Note] IS NULL)";
                    }
                    dvFillDGV.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void tsmiModifyLotNo_Click(object sender, EventArgs e)
        {
            PopUpCategoryForm popupcategoryform = new PopUpCategoryForm();
            popupcategoryform.Show();
        }    

        public void Func_UpdateLotNo()
        {
            if (PopUpCategoryFrm != null)
            {
                PopUpCategoryFrm.Close();
                PopUpCategoryFrm = null;

                SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
                if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
                SqlCommand Comm = new SqlCommand();
                Comm.Connection = Conn;

                string strRMCategory = strLotNo.Substring(6, 1).ToUpper();
                if (String.Compare(strRMCategory, "R") == 0) { strRMCategory = "RMB"; }
                else { strRMCategory = "USD"; }
                for (int i = 0; i < this.dgvBomList.RowCount; i++)
                {
                    Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                    Comm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = strRMCategory;
                    Comm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvBomList["Batch No", i].Value.ToString();
                    Comm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = this.dgvBomList["FG No", i].Value.ToString();
                    Comm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(this.dgvBomList["Line No", i].Value.ToString());
                    Comm.CommandText = @"UPDATE M_DailyBOM SET [Lot No] = @LotNo, [RM Category] = @RMCategory WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo AND [Line No] = @LineNo";
                    Comm.ExecuteNonQuery();
                    Comm.Parameters.Clear();

                }
                
                Comm.Dispose();
                Conn.Close();
                Conn.Dispose();
                MessageBox.Show("Successfully batch modify Lot No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Activate();
                this.Refresh();
                return;
            }
        }
        #endregion
    }
}