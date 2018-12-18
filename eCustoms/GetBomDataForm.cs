using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Core;

namespace eCustoms
{
    public partial class GetBomDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable dtExRate = new DataTable();
        protected DataView dvFillDGV = new DataView();
        protected DataTable dtFillDGV = new DataTable();
        protected PopUpFilterForm filterFrm = null;
        string strFilter = null;
        DataTable dtRM_ITEM_ListInBOMDetail = new DataTable();//Component item can be found in BOM detail table (only input items for every BOM)
        DataTable dtFGno_BatchNoListInBOM = new DataTable(); //reycle or intermediate FG can be found in FG BOM table (Only output item for every BOM)
        DataTable dtBatchNoListInBOM_but_Different_FG_Item = new DataTable(); //Batch no can be found in FG BOM table but FG item is different because FG item is changed to recycle C-code item

        public GetBomDataForm() { InitializeComponent(); }
        private static GetBomDataForm getBomDataFrm;
        public static GetBomDataForm CreateInstance()
        {
            if (getBomDataFrm == null || getBomDataFrm.IsDisposed) { getBomDataFrm = new GetBomDataForm(); }
            return getBomDataFrm;
        }

        private void GetBomDataForm_Load(object sender, EventArgs e)
        {
            SqlConnection exeConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (exeConn.State == ConnectionState.Closed) { exeConn.Open(); };
            SqlCommand exeComm = new SqlCommand();
            exeComm.Connection = exeConn;
            SqlDataAdapter exeAdapter = new SqlDataAdapter();
            exeComm.CommandText = @"SELECT DISTINCT [FG NO], [Batch No] FROM C_BOM";
            exeAdapter.SelectCommand = exeComm;
            //DataTable dtFGno_BatchNoListInBOM = new DataTable();
            exeAdapter.Fill(dtFGno_BatchNoListInBOM);
            exeComm.CommandText = @"SELECT DISTINCT [Item No] FROM C_BOMDetail";
            exeAdapter.SelectCommand = exeComm;
            //DataTable dtRM_ITEM_ListInBOMDetail = new DataTable();
            exeAdapter.Fill(dtRM_ITEM_ListInBOMDetail);
            exeComm.Dispose();
            exeAdapter.Dispose();
            if (exeConn.State == ConnectionState.Open) { exeConn.Close(); exeConn.Dispose(); };
        }

        private void GetBomDataForm_FormClosing(object sender, FormClosingEventArgs e)
        { dtExRate.Dispose(); dtFillDGV.Dispose(); dtRM_ITEM_ListInBOMDetail.Dispose(); dtFGno_BatchNoListInBOM.Dispose(); }

        private void dgvBOM_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvBOM.RowCount; i++) { this.dgvBOM[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvBOM.RowCount; i++) { this.dgvBOM[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvBOM.RowCount; i++)
                    {
                        if (String.Compare(this.dgvBOM[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvBOM[0, i].Value = true; }

                        else { this.dgvBOM[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }
            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvBOM_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvBOM.RowCount == 0) { return; }
            if (this.dgvBOM.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvBOM.RowCount; i++)
                {
                    if (String.Compare(this.dgvBOM[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvBOM.RowCount && iCount > 0)
                { this.dgvBOM.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvBOM.RowCount)
                { this.dgvBOM.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvBOM.Columns[0].HeaderText = "Select"; }
            }
        }

        private void dgvBOM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (MessageBox.Show("Are you sure to delete this BOM?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SqlConnection delConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (delConn.State == ConnectionState.Closed) { delConn.Open(); }
                    SqlCommand delComm = new SqlCommand();
                    delComm.Connection = delConn;

                    int iRow = this.dgvBOM.CurrentRow.Index;
                    string strBatchNo = this.dgvBOM["Batch No", iRow].Value.ToString().Trim();
                    string strFGNo = this.dgvBOM["FG No", iRow].Value.ToString().Trim();

                    //Remove the selected BOM data in DataGridView
                    DataRow[] dRow = dvFillDGV.Table.Select("[Batch No] = '" + strBatchNo + "' AND [FG No] = '" + strFGNo + "'");
                    foreach (DataRow dr in dRow) { dr.Delete(); }
                    dtFillDGV.AcceptChanges();

                    //Delete the selected BOM data in DataBase
                    delComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    delComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                    delComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
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
                MessageBox.Show("Please find out the uploading file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
          
            bool isLatestExcelFormatXLSX = this.txtPath.Text.ToLower().Contains(".xlsx");
            this.ImportExcelData(this.txtPath.Text.Trim(), isLatestExcelFormatXLSX);
            ComsumptionRateEuqalTo100Percent();// Attach this function here to make sure operator will not miss this step  on June.22.2017
          
        }

        private void llblMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When upload BOM data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                            "\n\tProcess Order No, \n\tActual Start Date, \n\tActual End Date, \n\tBatch No, \n\tFG No, \n\tFG Description, \n\tLine No, " +
                            "\n\tItem No, \n\tItem Description, \n\tLot No, \n\tInventory Type, \n\tFG Qty, \n\tRM Qty, " + 
                            "\n\tTotal Input Qty, \n\tDrools Qty.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ImportExcelData(string strFilePath, bool isLatestExcelFormatXLSX)
        {
            string strConn;
            if (isLatestExcelFormatXLSX) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$] WHERE [Batch No] IS NOT NULL AND [Batch No] <> ''", myConn);
            DataTable myTable = new DataTable();
            myDataAdapter.Fill(myTable);
            myDataAdapter.Dispose();
            myConn.Dispose();
            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                myTable.Dispose();
                return;
            }
            string[] strFieldName = {"Batch No"};
            SqlLib sqlLib = new SqlLib();
            DataTable dtBomList = sqlLib.SelectDistinct(myTable, strFieldName);
            sqlLib.Dispose(0);

            SqlConnection BomConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BomConn.State == ConnectionState.Closed) { BomConn.Open(); }
            SqlCommand BomComm = new SqlCommand();
            BomComm.Connection = BomConn;
            
            //Delete those Batch No. in the temporary table M_DailyBOM if the same Batch No. to be uploaded from Excel file
            SqlDataAdapter BomAdapter = new SqlDataAdapter("SELECT DISTINCT [Batch No] FROM M_DailyBOM", BomConn);
            DataTable dtDailyBom = new DataTable();
            BomAdapter.Fill(dtDailyBom);
            if (dtDailyBom.Rows.Count > 0)
            {
                for (int i = 0; i < dtDailyBom.Rows.Count; i++)
                {
                    string strBomName = dtDailyBom.Rows[i][0].ToString().Trim();
                    DataRow[] datarow = dtBomList.Select("[Batch No]='" + strBomName + "'");
                    if (datarow.Length > 0)
                    {
                        BomComm.CommandText = "DELETE FROM M_DailyBOM WHERE [Batch No] = '" + strBomName + "'";
                        BomComm.ExecuteNonQuery();
                    }
                }
            }
            dtDailyBom.Dispose();

            // Check if the Batch No to be uploaded is already exsiting in history BOM table, if yes, do not allow to upload BOM again.
            //BomAdapter = new SqlDataAdapter("SELECT DISTINCT [Batch No] FROM C_BOM", BomConn);
            //DataTable dtHistoryBom = new DataTable();
            //BomAdapter.Fill(dtHistoryBom);
            //BomAdapter.Dispose();
            // Check if BOM to be uploaded into system is alreay in BOM history record, if yes, do not upload BOM to system again
            for (int i = 0; i < dtBomList.Rows.Count; i++)
            {
                string strBomName = dtBomList.Rows[i][0].ToString().Trim();
                DataRow[] datarow = dtFGno_BatchNoListInBOM.Select("[Batch No]='" + strBomName + "'");
                if (datarow.Length > 0)
                {
                    DataRow[] drow = myTable.Select("[Batch No]='" + strBomName + "'");
                    {
                        foreach (DataRow dr in drow)
                        { myTable.Rows.Remove(dr); }
                    }
                    myTable.AcceptChanges();
                }
            }
            dtBomList.Dispose();
            //dtHistoryBom.Dispose();
            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no new BOM needed to generate. (On reason : Same BOM has been registered in BOM history)", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                myTable.Dispose();
                BomConn.Dispose();
                return;
            }

            
            myTable.Columns.Add("Batch Path", typeof(string));
            DataRow[] drRecycle = myTable.Select("[Item Description] LIKE '%BP2%'"); //remove it from the component list if it is scrap items like drools
            if (drRecycle.Length > 0)
            { 
                foreach (DataRow dr in drRecycle) { myTable.Rows.Remove(dr); };
                myTable.AcceptChanges();
            }
            
            DataTable dtReckList = myTable.Copy();
            //drRecycle = dtReckList.Select("[Item Description] LIKE '%-INTERMEDIATE' OR [Item Description] LIKE '%-COLOR-%'");
            drRecycle = dtReckList.Select("[Item Description] LIKE '%-INTERMEDIATE'");
            if (drRecycle.Length > 0) //if we have recyles or intermediates as input items in the BOM, or any over-production FG is changed to C-CODE
            {
                dtReckList.Columns.Add("IsMulti", typeof(string));
                dtReckList.Columns.Add("Total RM Qty", typeof(decimal));
                dtReckList.Columns.Add("IsFG", typeof(string));
                dtReckList.Columns.Add("YesNo", typeof(string));
                string strReckBom = null;
                foreach (DataRow dr in drRecycle) 
                { 
                    dr["YesNo"] = "TRUE";
                    strReckBom += "'" + dr["Lot No"].ToString().Trim().ToUpper() + "',";
                }
                DataRow[] datarow = dtReckList.Select("[YesNo] = '' OR [YesNo] IS NULL OR [YesNo] <> 'TRUE'");
                foreach (DataRow dr in datarow) { dtReckList.Rows.Remove(dr); };
                dtReckList.Columns.Remove("YesNo");
                dtReckList.AcceptChanges(); //dtReckList includes all the rows with intermediate FG item (added a sufix like  'INTERMEDIATE')
                //datarow = myTable.Select("[Item Description] LIKE '%-INTERMEDIATE' OR [Item Description] LIKE '%-COLOR-%'");
                datarow = myTable.Select("[Item Description] LIKE '%-INTERMEDIATE'");
                foreach (DataRow drow in datarow)
                {//myTable excludes all the rows with intermediate FG item with batch No found in BOM history (added a sufix like  'INTERMEDIATE')
                     myTable.Rows.Remove(drow);
                }; 
                myTable.AcceptChanges();



                strReckBom = strReckBom.Remove(strReckBom.Length - 1);
                string strSQL = "SELECT '' AS [Process Order No], '' AS [Actual Start Date], '' AS [Actual End Date], C_BOMDetail.[Batch No], '' AS [FG No], '' AS [FG Description], " +
                                "[Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [FG Qty], 0.0 AS [RM Qty],  [Total Input Qty], " +
                                " 0.0 AS [Drools Qty], '' AS [Batch Path], [Consumption] FROM C_BOMDetail LEFT JOIN C_BOM ON C_BOMDetail.[Batch No] = C_BOM.[Batch No] WHERE C_BOMDetail.[Batch No] IN (" + strReckBom + ") AND [Consumption] > 0";
                SqlDataAdapter BomAdp = new SqlDataAdapter(strSQL, BomConn);
                DataTable dtReckData = new DataTable();
                dtReckData = myTable.Clone();//Program will get data type as decimal instead of Int32 for Column [FG Qty], so added this sentence to make sure the new table is compatible with myTable on Jan.14.2017
                              
                BomAdp.Fill(dtReckData);//Get reycle or intermediate BOM from history BOM details                
                BomAdp.Dispose();

                for (int m = 0; m < dtReckList.Rows.Count; m++)
                {
                    string strBatchNo = dtReckList.Rows[m]["Lot No"].ToString().Trim().ToUpper();
                    DataRow[] drReckData = dtReckData.Select("[Batch No]='" + strBatchNo + "'");
                    if (drReckData.Length > 0)  // find out the history BOM to calculate recycle
                    {
                        foreach (DataRow dr in drReckData)
                        {
                            dr["Batch No"] = dtReckList.Rows[m]["Batch No"].ToString().Trim();
                            dr["Process Order No"] = dtReckList.Rows[m]["Process Order No"].ToString().Trim().ToUpper();
                            dr["Actual Start Date"] = dtReckList.Rows[m]["Actual Start Date"].ToString().Trim().ToUpper();
                            dr["Actual End Date"] = dtReckList.Rows[m]["Actual End Date"].ToString().Trim().ToUpper();
                            dr["FG No"] = dtReckList.Rows[m]["FG No"].ToString().Trim().ToUpper();
                            dr["FG Description"] = dtReckList.Rows[m]["FG Description"].ToString().Trim().ToUpper();
                            decimal dcFgQtyInSubBOMdetail = Convert.ToDecimal(dr["FG Qty"].ToString().Trim());
                            decimal dcTotalInputInSubBOMdetail = Convert.ToDecimal(dr["Total Input Qty"].ToString().Trim());
                            string strFgQty = dtReckList.Rows[m]["FG Qty"].ToString().Trim();
                            if (!String.IsNullOrEmpty(strFgQty)) { dr["FG Qty"] = Convert.ToInt32(strFgQty); }
                            string strTotalInputQty = dtReckList.Rows[m]["Total Input Qty"].ToString().Trim();
                            if (!String.IsNullOrEmpty(strTotalInputQty)) { dr["Total Input Qty"] = Math.Round(Convert.ToDecimal(double.Parse(strTotalInputQty)), 6); }
                            string strDroolsQty = dtReckList.Rows[m]["Drools Qty"].ToString().Trim();
                            if (!String.IsNullOrEmpty(strDroolsQty)) { dr["Drools Qty"] = Math.Round(Convert.ToDecimal(double.Parse(strDroolsQty)), 6); }
                            dr["Batch Path"] = "/" + dtReckList.Rows[m]["Batch No"].ToString().Trim() + "/" + strBatchNo;
                            decimal dConsumption = Math.Round(Convert.ToDecimal(double.Parse(dr["Consumption"].ToString().Trim())), 6);
                            //decimal dLossRate = Math.Round(Convert.ToDecimal(double.Parse(dr["Qty Loss Rate"].ToString().Trim())), 6); ;
                            //decimal dFgQty = Convert.ToDecimal(dtReckList.Rows[m]["FG Qty"].ToString().Trim());
                            decimal dReckQty = Math.Round(Convert.ToDecimal(double.Parse(dtReckList.Rows[m]["RM Qty"].ToString().Trim())), 6);
                            //decimal dTotalInputQty = Convert.ToDecimal(strTotalInputQty);
                            //dr["RM Qty"] = Math.Round(dTotalInputQty * dReckQty * dConsumption / (dTotalInputQty - dFgQty), 6);
                            dr["RM Qty"] = Math.Round(dReckQty * dConsumption * dcTotalInputInSubBOMdetail / dcFgQtyInSubBOMdetail, 6); //Revised on Mar.29.2017
                        }
                        dtReckData.AcceptChanges();                        
                    }
                    else  // use the BOM in the same FG to directly split the recycle qty into every existing RM
                    {
                        
                            String batchNo = dtReckList.Rows[m]["Batch No"].ToString();
                            decimal dTotalInputQty = 0.0M;
                            
                            decimal dReckQty = Math.Round(Convert.ToDecimal(double.Parse(dtReckList.Rows[m]["RM Qty"].ToString().Trim())), 6);
                            DataRow[] drMyTable = myTable.Select("[Batch No]='" + batchNo + "'");
                            foreach (DataRow dr in drMyTable)
                            {
                                dTotalInputQty += Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6);
                            }
                            foreach (DataRow dr in drMyTable)
                            {
                                decimal dRmQty = Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6);
                                //dr["RM Qty"] = Math.Round(Convert.ToDecimal(dRmQty * dTotalInputQty / (dTotalInputQty - dReckQty)), 6);
                                dr["RM Qty"] = Math.Round(Convert.ToDecimal(dRmQty * (1 + dReckQty / dTotalInputQty )), 6);
                                dr["Batch Path"] = "/" + strBatchNo + "/#";
                            }
                            myTable.AcceptChanges();
                        
                    }
                }
                dtReckList.Dispose();
                if (dtReckData.Rows.Count > 0)
                {
                    dtReckData.Columns.Remove("Consumption");
                    myTable.Merge(dtReckData);
                    dtReckData.Dispose();
                }
                myTable.AcceptChanges();
                DataView dv = myTable.DefaultView;
                dv.Sort = "Batch No ASC";
                myTable = dv.ToTable();

                string strBatchName = null;
                int iLineNo = 0;
                for (int n = 0; n < myTable.Rows.Count; n++)
                {
                    string strSameBatch = myTable.Rows[n]["Batch No"].ToString().Trim();
                    string strBatchPath = myTable.Rows[n]["Batch Path"].ToString().Trim();
                    if (String.Compare(strSameBatch, strBatchName) != 0)
                    {
                        strBatchName = strSameBatch;
                        iLineNo = 1;
                    }
                    else { iLineNo += 1; }
                    myTable.Rows[n]["Line No"] = iLineNo;
                    if (String.IsNullOrEmpty(strBatchPath)) { myTable.Rows[n]["Batch Path"] = "/" + strSameBatch + "/"; }
                }
                myTable.AcceptChanges();
            }
            dtReckList.Dispose();

            for (int j = 0; j < myTable.Rows.Count; j++)
            {
                BomComm.Parameters.Clear();
                BomComm.Parameters.Add("@ProcessOrderNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Process Order No"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = myTable.Rows[j]["Actual Start Date"].ToString().Trim();
                BomComm.Parameters.Add("@ActualEndDate", SqlDbType.NVarChar).Value = myTable.Rows[j]["Actual End Date"].ToString().Trim();
                string strBatchPath = myTable.Rows[j]["Batch Path"].ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(strBatchPath)) { strBatchPath = "/" + myTable.Rows[j]["Batch No"].ToString().Trim().ToUpper() + "/"; }
                BomComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = strBatchPath;
                BomComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Batch No"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["FG No"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@FgDesc", SqlDbType.NVarChar).Value = myTable.Rows[j]["FG Description"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[j]["Line No"].ToString().Trim());
                BomComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Item No"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = myTable.Rows[j]["Item Description"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Lot No"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@InvType", SqlDbType.NVarChar).Value = myTable.Rows[j]["Inventory Type"].ToString().Trim().ToUpper();
                BomComm.Parameters.Add("@RmCategory", SqlDbType.NVarChar).Value = string.Empty; // myTable.Rows[j]["RM Category"].ToString().Trim().ToUpper();
                string strFgQty = myTable.Rows[j]["FG Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strFgQty)) { BomComm.Parameters.Add("@FgQty", SqlDbType.Int).Value = 0; }
                else { BomComm.Parameters.Add("@FgQty", SqlDbType.Int).Value = Convert.ToInt32(strFgQty); }
                string strRmQty = myTable.Rows[j]["RM Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strRmQty)) { BomComm.Parameters.Add("@RmQty", SqlDbType.Decimal).Value = 0.0; }
                else { BomComm.Parameters.Add("@RmQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(strRmQty)), 6); }
                string strTotalInputQty = myTable.Rows[j]["Total Input Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strTotalInputQty)) { BomComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = 0.0; }
                else { BomComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(strTotalInputQty)), 6); }
                string strDroolsQty = myTable.Rows[j]["Drools Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strDroolsQty)) { BomComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = 0.0; }
                else { BomComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(strDroolsQty)), 6); }
                BomComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                BomComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                BomComm.CommandText = "INSERT INTO M_DailyBOM([Process Order No], [Actual Start Date], [Actual End Date], [Batch Path], [Batch No], [FG No], " + 
                                      "[FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [FG Qty], [RM Qty], " + 
                                      "[Total Input Qty], [Drools Qty], [Creater], [Created Date]) VALUES(@ProcessOrderNo, @ActualStartDate, @ActualEndDate, " + 
                                      "@BatchPath, @BatchNo, @FgNo, @FgDesc, @LineNo, @ItemNo, @ItemDesc, @LotNo, @InvType, @RmCategory, @FgQty, @RmQty, " + 
                                      "@TotalInputQty, @DroolsQty, @Creater, @CreatedDate)";
                BomComm.ExecuteNonQuery();
                BomComm.Parameters.Clear();
            }            
            BomComm.Dispose();
            BomConn.Dispose();
            this.GetDgvData(true);
        }

        private void GetDgvData(bool bJudge)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            string strSQL = null;
            if (bJudge == true)
            {
                strSQL = "SELECT [Batch Path], [Batch No], [FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], " +
                         "[RM Category], [FG Qty], [Total Input Qty], [Drools Qty], [RM Qty], [Actual Start Date], [Actual End Date], [Process Order No] " +
                         "FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            }
            else if (bJudge == false)
            {
                strSQL = "SELECT [Batch Path], [Batch No], [FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], " +
                         "[RM Category], [RM EHB], [BGD No], [FG Qty], [Total Input Qty], [Drools Qty], [Order Price(USD)], [Total RM Cost(USD)], [RM Qty], " +
                         "[RM Currency], [RM Price], [Consumption], [Qty Loss Rate], [HS Code], [CHN Name], [Drools EHB], [Process Order No], [Actual Start Date], " + 
                         "[Actual End Date] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            }
            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlDataAdapter Adapter = new SqlDataAdapter(strSQL, Conn);
            dtFillDGV.Clear();
            Adapter.Fill(dtFillDGV);
            Adapter.Dispose();
            dvFillDGV = dtFillDGV.DefaultView;
            if (dtFillDGV.Rows.Count == 0)
            {
                dtFillDGV.Clear();
                dtFillDGV.Dispose();
                this.dgvBOM.DataSource = DBNull.Value;
            }
            else
            {
                this.dgvBOM.DataSource = dvFillDGV;
                this.dgvBOM.Columns[0].HeaderText = "Select";
                this.dgvBOM.Columns["FG No"].Frozen = true;
            }
            if (Conn.State == ConnectionState.Open) { Conn.Close(); Conn.Dispose(); }
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

        private void Update_RMCurrency_RMPrice_BGDNO_RMEHB(SqlConnection exeConn, SqlCommand exeComm)
        {
            exeComm.CommandText = "SELECT [Batch No], [FG No], [Line No], [Item No], [Lot No], [RM EHB], [BGD No], [RM Price], [RM Currency] FROM M_DailyBOM";
            SqlDataAdapter exeAdap1 = new SqlDataAdapter();
            exeAdap1.SelectCommand = exeComm;
            DataTable dtRmEhb = new DataTable();
            exeAdap1.Fill(dtRmEhb);
            exeComm.CommandText = "SELECT DISTINCT [Item No], [Lot No], [RM EHB], [BGD No], [PO Currency] AS [RM Currency], [PO Unit Price] AS [RM Price] " + 
                                  "FROM C_RMReceiving ORDER BY [PO Currency]";
            exeAdap1 = new SqlDataAdapter();
            exeAdap1.SelectCommand = exeComm;
            DataTable dtRmRcvd = new DataTable();
            exeAdap1.Fill(dtRmRcvd);
            exeAdap1.Dispose();
            foreach (DataRow dr in dtRmEhb.Rows)
            {
                string strItemNo = dr["Item No"].ToString().Trim().ToUpper();
                string strLotNo = dr["Lot No"].ToString().Trim().ToUpper();
                DataRow[] drRmEhb = dtRmRcvd.Select("[Item No]='" + strItemNo + "' AND [Lot No]='" + strLotNo + "'");
                if (drRmEhb.Length > 0)
                {
                    string strRmCurr = drRmEhb[0]["RM Currency"].ToString().Trim().ToUpper();
                    string strRmCategory = "USD";
                    if (string.Compare(strRmCurr, "RMB") == 0 || string.Compare(strRmCurr, "CNY") == 0) { strRmCategory = "RMB"; }
                    exeComm.Parameters.Clear();
                    exeComm.Parameters.Add("@RmEhb", SqlDbType.NVarChar).Value = drRmEhb[0]["RM EHB"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@BgdNo", SqlDbType.NVarChar).Value = drRmEhb[0]["BGD No"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@RmCurr", SqlDbType.NVarChar).Value = strRmCurr;
                    exeComm.Parameters.Add("@RmPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(drRmEhb[0]["RM Price"].ToString().Trim());
                    exeComm.Parameters.Add("@RmCategory", SqlDbType.NVarChar).Value = strRmCategory;
                    exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                    exeComm.CommandText = "UPDATE M_DailyBOM SET [RM EHB] = @RmEhb, [BGD No] = @BgdNo, [RM Currency] = @RmCurr, [RM Price] = @RmPrice, " +
                                          "[RM Category] = @RmCategory WHERE [Batch No] = @BatchNo AND [FG No] = @FgNo AND [Line No] = @LineNo";
                    exeComm.ExecuteNonQuery();
                }
            }
            exeComm.Parameters.Clear();
            dtRmEhb.Dispose();
            dtRmRcvd.Dispose();
        }

        private void Update_TotalRMCost_OrderPrice_QtyLossRate(SqlConnection exeConn, SqlCommand exeComm)
        {
            exeComm.CommandText = "SELECT [Batch No], [FG No], [Line No], [FG Qty], [Total Input Qty], [RM Qty], [RM Currency], [RM Price], [Total RM Cost(USD)] " + 
                                  "FROM M_DailyBOM ORDER BY [Batch No]";
            SqlDataAdapter exeAdap2 = new SqlDataAdapter();
            exeAdap2.SelectCommand = exeComm;
            DataTable dtRmCost = new DataTable();
            exeAdap2.Fill(dtRmCost);

            foreach (DataRow dr in dtRmCost.Rows)
            {               
                string strRmCurr = dr["RM Currency"].ToString().Trim();
                decimal dRmPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim());
                if (String.Compare(strRmCurr, "USD") != 0) { dRmPrice = Math.Round(dRmPrice * this.GetExchangeRate(strRmCurr), 3); }
                dr["Total RM Cost(USD)"] = Math.Round(dRmPrice * Convert.ToDecimal(dr["RM Qty"].ToString().Trim()), 3);
            }
            dtRmCost.AcceptChanges();

            exeComm.CommandText = "SELECT DISTINCT [Batch No], [FG No] FROM M_DailyBOM";
            exeAdap2 = new SqlDataAdapter();
            exeAdap2.SelectCommand = exeComm;
            DataTable dtBatchList = new DataTable();
            exeAdap2.Fill(dtBatchList);
            exeAdap2.Dispose();
            foreach (DataRow dRow in dtBatchList.Rows)
            {
                string strBatchNo = dRow["Batch No"].ToString().Trim();
                string strFGNo = dRow["FG No"].ToString().Trim();
                decimal dTotalRMCost = (decimal)dtRmCost.Compute("SUM([Total RM Cost(USD)])", "[Batch No]='" + strBatchNo + "' AND [FG No]='" + strFGNo + "'");
                DataRow dr =  dtRmCost.Select("[Batch No]='" + strBatchNo + "' AND [FG No]='" + strFGNo + "'")[0];
                decimal dFgQty = Convert.ToDecimal(dr["FG Qty"].ToString().Trim());
                //decimal dTotalInputQty = Math.Round(Convert.ToDecimal(double.Parse(dr["Total Input Qty"].ToString().Trim())), 6);
                decimal dTotalInputQty = (decimal)dtRmCost.Compute("SUM([RM Qty])", "[Batch No]='" + strBatchNo + "' AND [FG No]='" + strFGNo + "'");
  
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(dTotalRMCost / dFgQty, 2);
                exeComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(dTotalRMCost, 2);
                exeComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = Math.Round((1 - dFgQty / dTotalInputQty) * 100.0M, 6);
                exeComm.Parameters.Add("@droolsQty", SqlDbType.Decimal).Value = dTotalInputQty - dFgQty;
                exeComm.Parameters.Add("@totalInputQty", SqlDbType.Decimal).Value = dTotalInputQty;
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                exeComm.CommandText = "UPDATE M_DailyBOM SET [Order Price(USD)] = @OrderPrice, [Total RM Cost(USD)] = @TotalRMCost, [Qty Loss Rate] = @QtyLossRate " +
                                      ", [Drools Qty] = @droolsQty , [Total Input Qty] = @totalInputQty WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                exeComm.ExecuteNonQuery();               
            }
            exeComm.Parameters.Clear();
            dtRmCost.Dispose();
            dtBatchList.Dispose();
        }

        private void Update_HSCode_ChnName_DroolsEHB_Consumption(SqlConnection exeConn, SqlCommand exeComm)
        {
            exeComm.CommandText = "SELECT DISTINCT M.[Batch No], M.[FG No], B.[HS Code], B.[FG CHN Name] FROM M_DailyBOM AS M LEFT JOIN (SELECT DISTINCT [Grade], " + 
                                  "[HS Code], [FG CHN Name] FROM B_HsCode) AS B ON SUBSTRING(M.[FG Description], 0, CHARINDEX('-', M.[FG Description], 0)) = B.[Grade]";
            SqlDataAdapter exeAdap3 = new SqlDataAdapter();
            exeAdap3.SelectCommand = exeComm;
            DataTable dtHsCode = new DataTable();
            exeAdap3.Fill(dtHsCode);
            foreach (DataRow dr in dtHsCode.Rows)
            {
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@HsCode", SqlDbType.NVarChar).Value = dr["HS Code"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@ChnName", SqlDbType.NVarChar).Value = dr["FG CHN Name"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                exeComm.CommandText = "UPDATE M_DailyBOM SET [HS Code] = @HsCode, [CHN Name] = @ChnName WHERE [Batch No] = @BatchNo AND [FG No] = @FgNo";
                exeComm.ExecuteNonQuery();
            }
            dtHsCode.Dispose();
            exeComm.CommandText = "SELECT M.[Batch No], M.[FG No], M.[Line No], M.[Total Input Qty], M.[RM Qty], B.[Drools EHB] FROM M_DailyBOM AS M " +
                                  "LEFT JOIN B_Drools AS B ON M.[RM Category] = B.[RM Category] AND M.[CHN Name] = B.[FG CHN Name]";
            exeAdap3.SelectCommand = exeComm;
            DataTable dtDrools = new DataTable();
            exeAdap3.Fill(dtDrools);
            exeAdap3.Dispose();
            foreach (DataRow dr in dtDrools.Rows)
            {
                decimal dTotalInputQty = Math.Round(Convert.ToDecimal(double.Parse(dr["Total Input Qty"].ToString().Trim())), 6);
                decimal dRmQty = Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6);
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(dRmQty / dTotalInputQty, 6);
                exeComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = dr["Drools EHB"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                exeComm.CommandText = "UPDATE M_DailyBOM SET [Consumption] = @Consumption, [Drools EHB] = @DroolsEHB WHERE [Batch No] = @BatchNo " + 
                                      "AND [FG No] = @FgNo AND [Line No] = @LineNo";
                exeComm.ExecuteNonQuery();
            }
            exeComm.Parameters.Clear();
            dtDrools.Dispose();
        }

        private void btnExtractData_Click(object sender, EventArgs e)
        {
            ComsumptionRateEuqalTo100Percent();
        }

        //Make sure all the consumption rate adds up to 1.0
        private void ComsumptionRateEuqalTo100Percent()
        {
            if (this.dgvBOM.RowCount == 0) { return; }
            SqlConnection exeConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (exeConn.State == ConnectionState.Closed) { exeConn.Open(); }
            SqlCommand exeComm = new SqlCommand();
            exeComm.Connection = exeConn;

            this.Update_RMCurrency_RMPrice_BGDNO_RMEHB(exeConn, exeComm);
            this.Update_TotalRMCost_OrderPrice_QtyLossRate(exeConn, exeComm);
            this.Update_HSCode_ChnName_DroolsEHB_Consumption(exeConn, exeComm);

            #region //Optimize Consumption, ensure this column's value equal to 1.0
            SqlDataAdapter exeAdapter = new SqlDataAdapter();
            exeComm.CommandText = @"SELECT * FROM V_QueryConsumption";
            exeAdapter.SelectCommand = exeComm;
            DataTable dtConsumption = new DataTable();
            exeAdapter.Fill(dtConsumption);
            exeAdapter.Dispose();
            for (int x = 0; x < dtConsumption.Rows.Count; x++)
            {
                decimal dSumConsump = Math.Round(Convert.ToDecimal(double.Parse(dtConsumption.Rows[x]["SUMConsumption"].ToString().Trim())), 6);
                decimal dConsump = Math.Round(Convert.ToDecimal(double.Parse(dtConsumption.Rows[x]["Consump"].ToString().Trim())), 6);
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = dConsump + 1.0M - dSumConsump;
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dtConsumption.Rows[x]["Batch No"].ToString().Trim();
                exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = dtConsumption.Rows[x]["FG No"].ToString().Trim();
                exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dtConsumption.Rows[x]["Line"].ToString().Trim());
                exeComm.CommandText = "UPDATE M_DailyBOM SET [Consumption] = @Consumption WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo AND [Line No] = @LineNo";
                exeComm.ExecuteNonQuery();
            }
            dtConsumption.Dispose();
            #endregion

            exeComm.Parameters.Clear();
            exeComm.Dispose();
            if (exeConn.State == ConnectionState.Open)
            {
                exeConn.Close();
                exeConn.Dispose();
            }
            if (MessageBox.Show("Successfully extract data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK) { this.GetDgvData(false); }
       
        }


        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvBOM.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            if (this.dgvBOM.Columns[0].HeaderText != "Select")
            {
                bool bJudge = false;
                int iRow = 2;
                for (int i = 0; i < this.dgvBOM.RowCount; i++)
                {
                    if (String.Compare(this.dgvBOM[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        bJudge = true;
                        for (int j = 3; j < this.dgvBOM.ColumnCount; j++)
                        { excel.Cells[iRow, j - 2] = "'" + this.dgvBOM[j, i].Value.ToString().Trim(); }
                        iRow++;
                    }
                }
                if (bJudge)
                {
                    for (int k = 3; k < this.dgvBOM.ColumnCount; k++)
                    { excel.Cells[1, k - 2] = this.dgvBOM.Columns[k].HeaderText.ToString(); }
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                    excel.Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < this.dgvBOM.RowCount; i++)
                {
                    for (int j = 3; j < this.dgvBOM.ColumnCount; j++)
                    { excel.Cells[i + 2, j - 2] = "'" + this.dgvBOM[j, i].Value.ToString().Trim(); }
                }

                for (int k = 3; k < this.dgvBOM.ColumnCount; k++)
                { excel.Cells[1, k - 2] = this.dgvBOM.Columns[k].HeaderText.ToString(); }

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                excel.Visible = true;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            SqlConnection BrowseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BrowseConn.State == ConnectionState.Closed) { BrowseConn.Open(); }

            string strBOMField = "SELECT [Batch Path], [Batch No], [FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], " +
                                 "[RM Category], [RM EHB], [BGD No], [FG Qty], [Total Input Qty], [Drools Qty], [Order Price(USD)], [Total RM Cost(USD)], [RM Qty], " +
                                 "[RM Currency], [RM Price], [Consumption], [Qty Loss Rate], [HS Code], [CHN Name], [Drools EHB], [Process Order No], [Actual Start Date], " +
                                 "[Actual End Date], [Creater], [Created Date] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            SqlDataAdapter BrowseAdapter = new SqlDataAdapter(strBOMField, BrowseConn);
            dtFillDGV.Clear();
            BrowseAdapter.Fill(dtFillDGV);
            dvFillDGV = dtFillDGV.DefaultView;
            if (dtFillDGV.Rows.Count == 0)
            {
                dtFillDGV.Clear();
                dtFillDGV.Dispose();
                this.dgvBOM.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvBOM.DataSource = dvFillDGV;
                this.dgvBOM.Columns[0].HeaderText = "Select";
                this.dgvBOM.Columns["FG No"].Frozen = true;
            }

            BrowseAdapter.Dispose();
            if (BrowseConn.State == ConnectionState.Open)
            {
                BrowseConn.Close();
                BrowseConn.Dispose();
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBOM.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBOM.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBOM.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBOM[strColumnName, this.dgvBOM.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
                if (this.dgvBOM.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBOM.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBOM.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBOM[strColumnName, this.dgvBOM.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
            if (this.dgvBOM.ColumnCount < 20) { this.GetDgvData(true); }
            else { this.GetDgvData(false); }
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvBOM.CurrentCell != null)
            {
                string strColumnName = this.dgvBOM.Columns[this.dgvBOM.CurrentCell.ColumnIndex].Name;
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
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(DateTime))
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

        private void tsmiBlankFieldFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvBOM.ColumnCount > 19 && this.dgvBOM.Columns[0].Visible == true)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    { strFilter += @" AND ([RM EHB] = '' OR [RM EHB] IS NULL OR [HS Code] = '' OR [HS Code] IS NULL OR [Drools EHB] = '' OR [Drools EHB] IS NULL)"; }
                    else { strFilter = @"([RM EHB] = '' OR [RM EHB] IS NULL OR [HS Code] = '' OR [HS Code] IS NULL OR [Drools EHB] = '' OR [Drools EHB] IS NULL)"; }
                    dvFillDGV.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void btnSearchBom_Click(object sender, EventArgs e)
        {
            OpenFileDialog openBomDlg = new OpenFileDialog();
            openBomDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openBomDlg.ShowDialog();
            this.txtPathBom.Text = openBomDlg.FileName;
        }

        private void btnUploadBom_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtPathBom.Text.Trim()))
            { MessageBox.Show("Please find out the uploading file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                bool bJudge = this.txtPathBom.Text.ToLower().Contains(".xlsx");
                this.ImportBom(this.txtPathBom.Text.Trim(), bJudge);
                ComsumptionRateEuqalTo100Percent();// Attach this function here to make sure operator will not miss this step  on July.17.2017
            }
            catch (Exception) { throw; }
        }

        public void ImportBom(string strFilePath, bool bJudge)
        {
            string strConn;
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection fzConn = new OleDbConnection(strConn);
            fzConn.Open();
            OleDbDataAdapter fzAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", fzConn);
            DataTable dtBomFz = new DataTable();
            fzAdapter.Fill(dtBomFz);
            fzConn.Dispose();
            if (dtBomFz.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtBomFz.Dispose();
                fzConn.Dispose();
                fzAdapter.Dispose();
                return;
            }
            
            SqlConnection BomFzConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BomFzConn.State == ConnectionState.Closed) { BomFzConn.Open(); }
            SqlCommand BomFzComm = new SqlCommand();
            BomFzComm.Connection = BomFzConn;
            BomFzComm.CommandText = "SELECT COUNT(*) FROM M_DailyBOM";
            int iCount = Convert.ToInt32(BomFzComm.ExecuteScalar());
            if (iCount > 0)
            {
                MessageBox.Show("System should submit daily BOM completely before regenerating Forzen BOM.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BomFzComm.Dispose();
                BomFzConn.Dispose();
                dtBomFz.Dispose();
                return;
            }
            BomFzComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dtBomFz.Rows[0]["Batch No"].ToString().Trim() + "%";
            BomFzComm.CommandText = "SELECT COUNT([Batch No]) FROM C_BOM WHERE [Batch No] LIKE @BatchNo";
            iCount = Convert.ToInt32(BomFzComm.ExecuteScalar());
            if (iCount >= 2)
            {
                MessageBox.Show("System already regenerated the forzen BOM, so refuse to do again.\nPlease note : allow to have a chance.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                BomFzComm.Dispose();
                BomFzConn.Dispose();
                dtBomFz.Dispose();
                return;
            }
            BomFzComm.Parameters.Clear();
            dtBomFz.Columns.Add("Batch Path", typeof(string));
            foreach (DataRow dr in dtBomFz.Rows)
            {               
                BomFzComm.Parameters.Add("@ProcessOrderNo", SqlDbType.NVarChar).Value = dr["Process Order No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = dr["Actual Start Date"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@ActualEndDate", SqlDbType.NVarChar).Value = dr["Actual End Date"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = "/" + dr["Batch No"].ToString().Trim().ToUpper() + "/FROZEN";
                BomFzComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper() + "R";
                BomFzComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@FgDesc", SqlDbType.NVarChar).Value = dr["FG Description"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                BomFzComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = dr["Item Description"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@InvType", SqlDbType.NVarChar).Value = dr["Inventory Type"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@RmCategory", SqlDbType.NVarChar).Value = dr["RM Category"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@FgQty", SqlDbType.Int).Value = Convert.ToInt32(dr["FG Qty"].ToString().Trim()); 
                BomFzComm.Parameters.Add("@RmQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6); 
                BomFzComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(dr["Total Input Qty"].ToString().Trim())), 6); 
                BomFzComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(dr["Drools Qty"].ToString().Trim())), 6); 
                BomFzComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                BomFzComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                BomFzComm.CommandText = "INSERT INTO M_DailyBOM([Process Order No], [Actual Start Date], [Actual End Date], [Batch Path], [Batch No], [FG No], " +
                                        "[FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [FG Qty], [RM Qty], " +
                                        "[Total Input Qty], [Drools Qty], [Creater], [Created Date]) VALUES(@ProcessOrderNo, @ActualStartDate, @ActualEndDate, " +
                                        "@BatchPath, @BatchNo, @FgNo, @FgDesc, @LineNo, @ItemNo, @ItemDesc, @LotNo, @InvType, @RmCategory, @FgQty, @RmQty, " +
                                        "@TotalInputQty, @DroolsQty, @Creater, @CreatedDate)";
                BomFzComm.ExecuteNonQuery();
                BomFzComm.Parameters.Clear();
            }
            BomFzComm.Dispose();
            BomFzConn.Dispose();
            dtBomFz.Dispose();
            this.GetDgvData(true);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxShow.Visible == false) { this.gBoxShow.Visible = true; }
            else { this.gBoxShow.Visible = false; this.txtPathRpt.Text = string.Empty; }
        }

        private void btnSearchRpt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openRptDlg = new OpenFileDialog();
            openRptDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openRptDlg.ShowDialog();
            this.txtPathRpt.Text = openRptDlg.FileName;
        }

        private void btnUploadRpt_Click(object sender, EventArgs e) //upload excel file containing information from COOISPI report which has all the inventory input and output per process order
        {
            if (String.IsNullOrEmpty(this.txtPathRpt.Text.Trim()))
            { MessageBox.Show("Please find out the uploading file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                bool isLatestExcelFormatXLSX = this.txtPathRpt.Text.ToLower().Contains(".xlsx");
                String messageF = this.TidyUpDailyBom(this.txtPathRpt.Text.Trim(), isLatestExcelFormatXLSX);
                if (!string.IsNullOrEmpty(messageF)) {MessageBox.Show(messageF, "Data Issues", MessageBoxButtons.OK, MessageBoxIcon.Warning); };
                
            }
            catch (Exception) { throw; }
        }

        public String TidyUpDailyBom(string strFilePath, bool isLatestExcelFormatXLSX) 
        {
            String message1 = String.Empty;
            DataTable dtMessage = new DataTable();
            dtMessage.Columns.Add("Information", typeof(string));
            dtMessage.Columns.Add("Process order", typeof(string));
            dtMessage.Columns.Add("RM Item No", typeof(string));
            dtMessage.Columns.Add("Lot No", typeof(string));
            string strConn;
            if (isLatestExcelFormatXLSX) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection RptConn = new OleDbConnection(strConn);
            RptConn.Open();
            strConn = "SELECT [Order], [Batch] AS [Batch No], [Material] AS [FG No], [Material description] AS [FG Description], [Unit of Entry (=ERFME)] AS [UOM], " +
                      "[Quantity in unit of entry (ERFME)] AS [FG Qty], 'Raw Material' AS [Inventory Type], [Movement Type], [Actual start time] AS [Actual Start Date], " +
                      "[Actual finish date] AS [Actual End Date] FROM [Sheet1$] WHERE [Movement Type] IN ('101', '102') AND [Unit of Entry (=ERFME)] IN ('G', 'KG')";
            OleDbDataAdapter RptAdapter = new OleDbDataAdapter(strConn, RptConn);
            DataTable dtBomRptM = new DataTable();
            dtBomRptM.Columns.Add("FG Qty", typeof(Int32));
            RptAdapter.Fill(dtBomRptM);//table dtBomRptM represents all output FGs (101 means quantity increase while 102 means qty decrease)
            dtBomRptM.Columns["FG Qty"].SetOrdinal(4);
            strConn = "SELECT [Order], [Material] AS [Item No], [Material description] AS [Item Description], [Batch] AS [Lot No], [Movement Type], [Unit of Entry (=ERFME)] AS [UOM], " +
                      "[Quantity in unit of entry (ERFME)] AS [RM Qty] FROM [Sheet1$] WHERE [Movement Type] IN ('261', '262') AND [Unit of Entry (=ERFME)] IN ('G', 'KG')";
            RptAdapter = new OleDbDataAdapter(strConn, RptConn);
            DataTable dtBomRptD = new DataTable();
            dtBomRptD.Columns.Add("RM Qty", typeof(decimal));
            RptAdapter.Fill(dtBomRptD);//table dtBomRptD represents all input RMs (101 means quantity increase while 102 means qty decrease)
            dtBomRptD.Columns["RM Qty"].SetOrdinal(4);
       

            RptAdapter.Dispose();
            RptConn.Dispose();
            if (dtBomRptM.Rows.Count == 0 || dtBomRptD.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtBomRptM.Dispose();
                dtBomRptD.Dispose();
                return message1;
            }

            DataRow[]  drow = dtBomRptM.Select("[Movement Type] = '102'");// FG qty decrease in case there is wrong FG output which needs to be corrected
            foreach (DataRow dr in drow)  //Apr.8.2017
            {
                dr["FG Qty"] = -Convert.ToInt32(dr["FG Qty"].ToString().Trim());
            };

            dtBomRptM.Columns.Remove("UOM");
            dtBomRptM.Columns.Remove("Movement Type");
            SqlLib lib = new SqlLib();
            string[] strFieldNameM = { "Order", "Batch No", "FG No", "FG Description", "Inventory Type", "Actual Start Date", "Actual End Date" };
            DataTable dtRptM = lib.SelectDistinct(dtBomRptM, strFieldNameM);
            dtRptM.Columns.Add("FG Qty", typeof(Int32));
            Regex regex1 = new Regex(@"(-\d\d){3}$"); // Strings look like '%-00-00-00' or '%-12-34-56',  Select method for DataTable does not work well with operator 'LIKE'
            String Temp1 = "";
            foreach (DataRow dr in dtRptM.Rows)
            {
                object obj = dtBomRptM.Compute("SUM([FG Qty])", "[Order]='" + dr["Order"].ToString().Trim() + "'");
                dr["FG Qty"] = Convert.ToInt32(obj.ToString().Trim());
                Temp1 = dr["FG Description"].ToString().Trim();
                if (!regex1.IsMatch(Temp1)) { dr["FG Description"] = Temp1  + "-BAG-99-99-99"; };
            }
            dtBomRptM.Dispose();  
            drow = dtRptM.Select("[FG Qty] <= 0");
            foreach (DataRow dr in drow) { dtRptM.Rows.Remove(dr); };
            //drow = dtRptM.Select("[FG Description] NOT LIKE '%-00'");
            //foreach (DataRow dr in drow) { dr["FG Description"] = dr["FG Description"].ToString().Trim() + "-BAG-99-99-99"; };


            dtBomRptM.AcceptChanges();

            drow = dtBomRptD.Select("[UOM] = 'G'");
            if (drow.Length > 0)
            {
                foreach (DataRow dr in drow)
                {
                    decimal dRmQty = Convert.ToDecimal(dr["RM Qty"].ToString().Trim());
                    dr["RM Qty"] = Math.Round(dRmQty / 1000, 6);
                    dr["UOM"] = "KG";
                }
            }
            drow = dtBomRptD.Select("[Movement Type] = '262'");
            foreach (DataRow dr in drow)  //Apr.8.2017
            {
                dr["RM Qty"] = - Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())),6);
            };


            dtBomRptD.Columns.Remove("UOM");
            dtBomRptD.Columns.Remove("Movement Type");
            string[] strFieldNameD = { "Order", "Item No", "Item Description", "Lot No" };
            DataTable dtRptD = lib.SelectDistinct(dtBomRptD, strFieldNameD);
            dtRptD.Columns.Add("RM Qty", typeof(decimal));
            foreach (DataRow dr in dtRptD.Rows)
            {
                object obj = dtBomRptD.Compute("SUM([RM Qty])", "[Order]='" + dr["Order"].ToString().Trim() + "' AND [Item No]='" + dr["Item No"].ToString().Trim() + "' AND [Lot No]='" + dr["Lot No"].ToString().Trim() + "'");
                dr["RM Qty"] = Math.Round(Convert.ToDecimal(double.Parse(obj.ToString().Trim())), 6);
            }
            dtBomRptD.Dispose();
            drow = dtRptD.Select("[RM Qty] <= 0");
            if (drow.Length > 0) { foreach (DataRow dr in drow) { dtRptD.Rows.Remove(dr); } };           
            dtRptD.Columns.Add("Line No", typeof(Int32));
            string strOrder = string.Empty;
            int iLineNo = 0;
            
            foreach (DataRow dr in dtRptD.Rows)
            {
                if (String.Compare(dr["Order"].ToString().Trim(), strOrder) != 0)
                {
                    iLineNo = 1;
                    dr["Line No"] = iLineNo;
                    strOrder = dr["Order"].ToString().Trim();
                }
                else
                {
                    iLineNo++;
                    dr["Line No"] = iLineNo;
                }
            }
            dtRptD.AcceptChanges();

            dtRptM.Columns.Add("Total Input Qty", typeof(decimal));
            dtRptM.Columns.Add("Drools Qty", typeof(decimal));
            foreach (DataRow dr in dtRptM.Rows)
            {
                decimal dFgQty = Math.Round(Convert.ToDecimal(double.Parse(dr["FG Qty"].ToString().Trim())), 6);
                DataRow[] datarow = dtRptD.Select("[Order]='" + dr["Order"].ToString().Trim() + "'");
                if (datarow.Length > 0)
                {
                    object obj = dtRptD.Compute("SUM([RM Qty])", "[Order]='" + dr["Order"].ToString().Trim() + "'");
                    dr["Total Input Qty"] = Math.Round(Convert.ToDecimal(double.Parse(obj.ToString().Trim())), 6);
                    dr["Drools Qty"] = Math.Round(Convert.ToDecimal(double.Parse(obj.ToString().Trim())), 6) - dFgQty;
                }
                else
                {
                    dr["Total Input Qty"] = 0;
                    dr["Drools Qty"] = 0;
                }
            }
            //Check if there is any wrong cases that Total input Qty is less than output FG Qty
            drow = dtRptM.Select("[Total Input Qty] <= [FG Qty]");
            foreach(DataRow dr1 in drow)
            {
                message1 += dr1["Order"].ToString()+";";
            };
            if (!String.IsNullOrEmpty(message1))
            {
                message1 = "\nIssues: Total input RM qty <= output FG qty for below process orders.\n" + message1; 
            };

            drow = dtRptM.Select("[Total Input Qty] = 0 OR [Drools Qty] <= 0");
            foreach (DataRow dr in drow) { dr.Delete(); };
            dtRptM.AcceptChanges();
            DataTable dtBomRpt = lib.MergeDataTable(dtRptD, dtRptM, "Order");
            lib.Dispose(0);
            dtRptM.Dispose();
            dtRptD.Dispose();
            dtBomRpt.Columns["Order"].ColumnName = "Process Order No";
            dtBomRpt.Columns["Actual Start Date"].SetOrdinal(1);
            dtBomRpt.Columns["Actual End Date"].SetOrdinal(2);
            dtBomRpt.Columns["Batch No"].SetOrdinal(3);
            dtBomRpt.Columns["FG No"].SetOrdinal(4);
            dtBomRpt.Columns["FG Description"].SetOrdinal(5);
            dtBomRpt.Columns["Line No"].SetOrdinal(6);
            dtBomRpt.Columns["Item No"].SetOrdinal(7);
            dtBomRpt.Columns["Item Description"].SetOrdinal(8);
            dtBomRpt.Columns["Lot No"].SetOrdinal(9);
            dtBomRpt.Columns["Inventory Type"].SetOrdinal(10);
            dtBomRpt.Columns["FG Qty"].SetOrdinal(11);
            dtBomRpt.Columns["RM Qty"].SetOrdinal(12);
            dtBomRpt.Columns["Total Input Qty"].SetOrdinal(13);
            drow = dtBomRpt.Select("([Batch No] IS NULL OR [Batch No] = '') OR ([Item No] IS NULL OR [Item No] = '')");
            foreach (DataRow dr in drow) { dtBomRpt.Rows.Remove(dr); };
            //if RM item No. can be found in BOM FG NO list (Table C_BOM), add a suffix to the field of item description for further special process
            foreach(DataRow itemNo1 in dtBomRpt.Rows) 
            {
                if (dtFGno_BatchNoListInBOM.Select("[FG No] = '" + itemNo1["Item No"].ToString() + "'").Length > 0)
                { itemNo1["Item Description"] = itemNo1["Item Description"].ToString().Trim() + "-INTERMEDIATE"; };//Add this one to make it applicable to both intermediate FG and recycle FG as input; Apr.8.2017
            };
            dtBomRpt.AcceptChanges();

            //June.26.2017 if item Batch No can be found in table C_BOM (FG BOM list) and relevant FG no. is different, need special mark for further action like replacing FG No.
            foreach (DataRow itemNo1 in dtBomRpt.Rows)
            {
                if (dtFGno_BatchNoListInBOM.Select("[Batch No] = '" + itemNo1["Lot No"].ToString() + "' And [FG No] <> '" + itemNo1["Item No"].ToString() + "'").Length > 0)
                {
                    if (regex1.IsMatch(itemNo1["Item Description"].ToString().Trim()))//check if Item description fits the cretiria of Finished Goods description (C-code description)
                    {
                        String abcExample1 = dtFGno_BatchNoListInBOM.Select("[Batch No] = '" + itemNo1["Lot No"].ToString() +"'")[0]["FG No"].ToString();
                        itemNo1["Item No"] = abcExample1;//Replace item no.
                        itemNo1["Item Description"] = itemNo1["Item Description"].ToString().Trim() + "-C-CODE-INTERMEDIATE"; 
                    }
                };
            };
            dtBomRpt.AcceptChanges();
            
            //Exceptional issues 1 to be reported out: cannot find Item No and Lot NO. in table C_BOM ([FG NO] and [Batch No])
            foreach (DataRow itemNo1LotNo1 in dtBomRpt.Select("[Item Description] LIKE '%-INTERMEDIATE'"))
            {
                if (dtFGno_BatchNoListInBOM.Select("[FG No] = '" + itemNo1LotNo1["Item No"].ToString() + "' And [Batch NO] = '" + itemNo1LotNo1["Lot No"].ToString() + "'").Length == 0)
                { 
                    message1 += "\nThe following RM Item No and Lot No cannot be found in BOM history: Process order;RM Item No;Lot No\n " + itemNo1LotNo1["Process Order No"] + ";" + itemNo1LotNo1["Item No"].ToString() + ";" + itemNo1LotNo1["Lot No"].ToString();
                    DataRow dr = dtMessage.NewRow();
                    dr[0] = "Process order, RM Item No and Lot No cannot be found in BOM history";
                    dr[1] = itemNo1LotNo1["Process Order No"].ToString();
                    dr[2] = itemNo1LotNo1["Item No"].ToString();
                    dr[3] = itemNo1LotNo1["Lot No"].ToString();
                    dtMessage.Rows.Add(dr);
                };
                //if we can find a revised batch in BOM history, replace old batch No. with a new one like 'XXXXXXXXXXR'
                if (dtFGno_BatchNoListInBOM.Select("[FG No] = '" + itemNo1LotNo1["Item No"].ToString() + "' And [Batch NO] = '" + itemNo1LotNo1["Lot No"].ToString() + "R'").Length > 0)
                {
                    message1 += "\n This batch has a revised on in BOM history with suffix 'R': " + itemNo1LotNo1["Lot No"].ToString() + "R;" + "\n " + itemNo1LotNo1["Process Order No"] + ";" + itemNo1LotNo1["Item No"].ToString() + ";" + itemNo1LotNo1["Lot No"].ToString();
                    DataRow dr = dtMessage.NewRow();
                    dr[0] = "This batch has a revised version in BOM history with suffix 'R': " + itemNo1LotNo1["Lot No"].ToString() + "R ";
                    dr[1] = itemNo1LotNo1["Process Order No"].ToString();
                    dr[2] = itemNo1LotNo1["Item No"].ToString();
                    dr[3] = itemNo1LotNo1["Lot No"].ToString();
                    dtMessage.Rows.Add(dr);
                };
            };
            //Exceptional issues2 to be reported out: RM or component item has the description like FG item description, but it cannot be found in both FG item list in table C_BOM and RM item list in table C_BOMDetail
            foreach (DataRow itemNo1LotNo1 in dtBomRpt.Select("[Item Description] NOT LIKE '%-INTERMEDIATE'"))
            {
                if (regex1.IsMatch(itemNo1LotNo1["Item Description"].ToString()))
                {
                    if (dtRM_ITEM_ListInBOMDetail.Select("[Item No] = '" + itemNo1LotNo1["Item No"].ToString() + "'").Length == 0)
                    {
                        message1 += "\nThe following RM Item No looks like finished goods item and cannot be found in both FG and RM item list: Process order;RM Item No;Lot No\n " + itemNo1LotNo1["Process Order No"].ToString() + ";" + itemNo1LotNo1["Item No"].ToString() + ";" + itemNo1LotNo1["Lot No"].ToString();
                        DataRow dr = dtMessage.NewRow();
                        dr[0] = "The following RM Item No looks like finished goods item and cannot be found in both FG and RM item list.";
                        dr[1] = itemNo1LotNo1["Process Order No"].ToString() ;
                        dr[2] = itemNo1LotNo1["Item No"].ToString();
                        dr[3] = itemNo1LotNo1["Lot No"].ToString();
                        dtMessage.Rows.Add(dr);
                    };
                }               
            };

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

            worksheet.Name = "Sheet1";
            for (int i = 0; i < dtBomRpt.Rows.Count; i++)
            {
                for (int j = 0; j < dtBomRpt.Columns.Count; j++)
                { worksheet.Cells[i + 2, j + 1] = "'" + dtBomRpt.Rows[i][j].ToString().Trim(); }
            }
            for (int k = 0; k < dtBomRpt.Columns.Count; k++) { worksheet.Cells[1, k + 1] = dtBomRpt.Columns[k].ColumnName.Trim(); }
            worksheet.Cells.EntireColumn.AutoFit();

            if (dtMessage.Rows.Count >0)
            {
                object missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, worksheet, missing, missing);
                worksheet.Name = "Warning message";
                for (int i = 0; i < dtMessage.Rows.Count; i++)
                {
                    for (int j = 0; j < dtMessage.Columns.Count; j++)
                    { worksheet.Cells[i + 2, j + 1] = "'" + dtMessage.Rows[i][j].ToString().Trim(); };
                }
                for (int k = 0; k < dtMessage.Columns.Count; k++) { worksheet.Cells[1, k + 1] = dtMessage.Columns[k].ColumnName.Trim(); };
                worksheet.Cells.EntireColumn.AutoFit();
            };
            

            excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            workbook.Worksheets[1].Activate();
            excel.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
            dtBomRpt.Dispose();

            return message1;
        }

        
    }
}