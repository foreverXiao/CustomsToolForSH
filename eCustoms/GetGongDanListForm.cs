using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class GetGongDanListForm : Form
    {
        DataTable dtGongDanList = new DataTable();
        DataTable dtIE = new DataTable();
        protected DataView dvGongDanList = new DataView();       
        private DataGridView dgvDetails = new DataGridView();
        protected PopUpFilterForm filterFrm = null;
        string strFilter = null;
       
        private static GetGongDanListForm getGongDanListFrm;
        public GetGongDanListForm() { InitializeComponent(); }
        public static GetGongDanListForm CreateInstance()
        {
            if (getGongDanListFrm == null || getGongDanListFrm.IsDisposed) { getGongDanListFrm = new GetGongDanListForm(); }
            return getGongDanListFrm;
        }

        private void GetGongDanListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtGongDanList.Dispose();
            dtIE.Dispose();
            dgvDetails.Dispose();
        }

        private void llblMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool bSD = this.ckSD.Checked;
            bool bLE = this.ckLE.Checked;
            if (bSD == false && bLE == false)
            { MessageBox.Show("Please select checkbox object first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (bSD)
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tOrder No, \n\tOrder Price, \n\tOrder Currency, \n\tDestination (English Short Name), " +
                                "\n\tPartNo, \n\tDelivery SLOC", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (bLE)
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tPurchasing Document, \n\tItem, \n\tDelivery Qty, \n\tDelivery No, \n\tBatch No, \n\tShipto Party", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ckSD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckSD.Checked) { this.ckLE.Enabled = false; }
            else { this.ckLE.Enabled = true; }
            this.ckLE.Checked = false;
        }

        private void ckLE_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckLE.Checked) { this.ckSD.Enabled = false; }
            else { this.ckSD.Enabled = true; }
            this.ckSD.Checked = false;
        }

        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtPath.Text = openDlg.FileName;
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            if (this.ckSD.Checked == false && this.ckLE.Checked == false)
            { MessageBox.Show("Please select checkbox object first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (String.IsNullOrEmpty(this.txtPath.Text.Trim()))
            { MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                bool bJudge = this.txtPath.Text.ToLower().Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge);
            }
            catch (Exception) { MessageBox.Show("Upload error, please try again.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); throw; }
        }

        private void ImportExcelData(string strFilePath, bool bJudge)
        {
            string strConn;
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            string strSQL = null;
            if (this.ckSD.Checked) {strSQL = "SELECT [Order No],[Order Price],[Order Currency],[Destination],[PartNo],[Delivery SLOC] FROM [Sheet1$] WHERE [Delivery SLOC] IS NOT NULL AND [Delivery SLOC] <> ''"; }
            if (this.ckLE.Checked) { strSQL = "SELECT [Purchasing Document],[Item],[Delivery Qty],[Delivery No],[Batch No],[Shipto Party] FROM [Sheet1$] WHERE [Batch No] IS NOT NULL AND [Batch No] <> ''"; }
            OleDbDataAdapter myAdapter = new OleDbDataAdapter(strSQL, myConn);
            DataTable myTable = new DataTable();
            myAdapter.Fill(myTable);
            myAdapter.Dispose();
            myConn.Dispose();
            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myTable.Dispose();
                return;
            }

            SqlConnection ConnFileGDL = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnFileGDL.State == ConnectionState.Closed) { ConnFileGDL.Open(); }
            SqlCommand CommFileGDL = new SqlCommand();
            CommFileGDL.Connection = ConnFileGDL;
            CommFileGDL.CommandType = CommandType.StoredProcedure;
            if (this.ckSD.Checked)
            {
                foreach (DataRow dr in myTable.Rows)
                {
                    string[] strPartNo = dr["PartNo"].ToString().Trim().Split('-');
                    dr["PartNo"] = strPartNo[0] + "-" + strPartNo[1];
                }
                myTable.AcceptChanges();
                
                CommFileGDL.CommandText = @"usp_InsertSalesOrderRpt";
                CommFileGDL.Parameters.AddWithValue("@tvp_SalesOrderRpt", myTable);
                CommFileGDL.Parameters.AddWithValue("@CreatedDate", System.DateTime.Now.ToString("yyyy-MM-dd"));
                CommFileGDL.ExecuteNonQuery();
                CommFileGDL.Parameters.Clear();
            }
            if (this.ckLE.Checked)
            {
                CommFileGDL.CommandText = @"usp_InsertOutboundDeliveryRpt";
                CommFileGDL.Parameters.AddWithValue("@tvp_OutboundDeliveryRpt", myTable);
                CommFileGDL.Parameters.AddWithValue("@CreatedDate", System.DateTime.Now.ToString("yyyy-MM-dd"));
                CommFileGDL.ExecuteNonQuery();
                CommFileGDL.Parameters.Clear();
            }
            CommFileGDL.Dispose();
            if (ConnFileGDL.State == ConnectionState.Open) { ConnFileGDL.Close(); ConnFileGDL.Dispose(); }
            MessageBox.Show("Successfully upload the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.btnCheckData.Enabled = true;
        }

        private void btnExtractData_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount > 0)
            {
                dtGongDanList.Columns.Clear();
                dtGongDanList.Rows.Clear();
                this.dgvGongDanList.DataSource = DBNull.Value;
            }
            strFilter = "";
            dvGongDanList.RowFilter = "";
            dtGongDanList.Rows.Clear();
            dtGongDanList.Columns.Clear();

            SqlConnection gdlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gdlConn.State == ConnectionState.Closed) { gdlConn.Open(); }
            SqlCommand gdlComm = new SqlCommand();
            gdlComm.Connection = gdlConn;
            gdlComm.CommandText = "SELECT COUNT(*) FROM M_DailyGongDanList";
            int iCount = Convert.ToInt32(gdlComm.ExecuteScalar());
            if (iCount > 0)
            {
                MessageBox.Show("Daily GongDan list should be used out by GongDan before generating the new.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gdlComm.Dispose();
                gdlConn.Dispose();
                return;
            }
            gdlComm.CommandText = "SELECT COUNT(*) FROM M_DailyGongDan";
            iCount = Convert.ToInt32(gdlComm.ExecuteScalar());
            if (iCount > 0)
            {
                MessageBox.Show("System should submit daily gongdan completely before generating the new GongDan list.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gdlComm.Dispose();
                gdlConn.Dispose();
                return;
            }
            gdlComm.CommandText = "SELECT A1.[Batch No], '' AS [GongDan No], '' AS [IE Type], A1.[Delivery Qty] AS [GongDan Qty], 0 AS [Avail Qty], A2.[Order Price], " +
                                  "A2.[Order Currency], A2.[Order No], A2.[PartNo], A2.[Destination], A1.[Delivery No], A2.[Delivery SLOC], A1.[Shipto Party] FROM " +
                                  "A_OutboundDeliveryRpt AS A1 INNER JOIN A_SalesOrderRpt AS A2 ON A1.[Purchasing Document] = A2.[Order No] ORDER BY A1.[Batch No]";
            SqlDataAdapter gdlAdapter = new SqlDataAdapter();
            gdlAdapter.SelectCommand = gdlComm;
            gdlAdapter.Fill(dtGongDanList);
            if (dtGongDanList.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                gdlAdapter.Dispose();
                gdlComm.Dispose();
                gdlConn.Dispose();
                dtGongDanList.Rows.Clear();
                dtGongDanList.Columns.Clear();
                return;
            }
            gdlComm.CommandText = "SELECT [Batch No], [FG Qty] - [GongDan Used Qty] AS [Avail Qty] FROM C_BOM WHERE Freeze = 0 AND [FG Qty] - [GongDan Used Qty] > 0";
            gdlAdapter = new SqlDataAdapter();
            gdlAdapter.SelectCommand = gdlComm;
            DataTable dtBomList = new DataTable();
            gdlAdapter.Fill(dtBomList);
            foreach (DataRow dr in dtGongDanList.Rows)
            {
                string strBatchNo = dr["Batch No"].ToString().Trim().ToUpper();
                DataRow[] drow = dtBomList.Select("[Batch No] LIKE '" + strBatchNo + "%'");
                if (drow.Length == 0) { dr.Delete(); }
                else 
                { 
                    dr["Avail Qty"] = Convert.ToInt32(drow[0]["Avail Qty"].ToString().Trim());
                    if (drow[0][0].ToString().Trim().Contains("R")) { dr["Batch No"] = drow[0][0].ToString().Trim(); }   //update the frozen BOM
                }          
            }
            DataRow[] datarow = dtGongDanList.Select("[Batch No] IS NULL");
            foreach (DataRow dr in datarow) { dr.Delete(); }
            dtGongDanList.AcceptChanges();
            dtBomList.Dispose();
            if (dtGongDanList.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                gdlAdapter.Dispose();
                gdlComm.Dispose();
                gdlConn.Dispose();
                dtGongDanList.Columns.Clear();
                return;
            }
            gdlComm.CommandText = "SELECT DISTINCT [Batch No], [Delivery No] FROM L_GongDan_Fulfillment";
            gdlAdapter = new SqlDataAdapter();
            gdlAdapter.SelectCommand = gdlComm;
            DataTable dtHistoryGDL = new DataTable();
            gdlAdapter.Fill(dtHistoryGDL);
            int iRowCount = dtGongDanList.Rows.Count;
            for (int i = 0; i < iRowCount; i++)
            {
                string strBatchNo = dtGongDanList.Rows[i]["Batch No"].ToString().Trim().ToUpper();
                string strDeliveryNo = dtGongDanList.Rows[i]["Delivery No"].ToString().Trim().ToUpper();
                DataRow[] drow = dtHistoryGDL.Select("[Batch No]='" + strBatchNo + "' AND [Delivery No]='" + strDeliveryNo + "'");
                if (drow.Length > 0) 
                { 
                    dtGongDanList.Rows.RemoveAt(i);
                    iRowCount--;
                    i--;
                }
            }
            dtGongDanList.AcceptChanges();
            dtHistoryGDL.Dispose();
            if (dtGongDanList.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                gdlAdapter.Dispose();
                gdlComm.Dispose();
                gdlConn.Dispose();
                dtGongDanList.Columns.Clear();
                return;
            }           
            string strBatchNoList = null;
            foreach (DataRow dr in dtGongDanList.Rows) { strBatchNoList += "'" + dr["Batch No"].ToString().Trim() + "',"; }
            strBatchNoList = strBatchNoList.Remove(strBatchNoList.Trim().Length - 1);
            //gdlComm.CommandText = "SELECT [Batch No], MAX([GongDan No]) AS [GongDan No] FROM C_GongDan GROUP BY [Batch No] HAVING [Batch No] IN (" + strBatchNoList + ")";
            gdlComm.CommandText = "SELECT [Batch No], max(convert(int, RIGHT([GongDan no],CHARINDEX('-',REVERSE([GongDan no]),0)-1))) AS [GongDan No] FROM C_GongDan GROUP BY [Batch No] HAVING [Batch No] IN (" + strBatchNoList + ")"; //June.29.2017
            gdlAdapter = new SqlDataAdapter();
            gdlAdapter.SelectCommand = gdlComm;
            DataTable dtMaxGD = new DataTable();
            gdlAdapter.Fill(dtMaxGD);
            gdlAdapter.Dispose();
            strBatchNoList = null;
            string strGongDanNo = null;
            foreach (DataRow dr in dtGongDanList.Rows)
            {
                string strBatchNo = dr["Batch No"].ToString().Trim();
                if (String.Compare(strBatchNoList, strBatchNo) == 0)
                {
                    string strLineNo = strGongDanNo.Trim().Split('-')[1];
                    int iLineNo = Convert.ToInt32(strLineNo) + 1;
                    strGongDanNo = strBatchNo + "-" + iLineNo.ToString().Trim();
                    dr["GongDan No"] = strGongDanNo;
                }
                else
                {
                    DataRow[] drow = dtMaxGD.Select("[Batch No]='" + strBatchNo + "'");
                    if (drow.Length == 0) { dr["GongDan No"] = strBatchNo + "-1"; }
                    else
                    {
                        //string strLineNo = drow[0]["GongDan No"].ToString().Trim().Split('-')[1];
                        string strLineNo = drow[0]["GongDan No"].ToString(); //June.20.2017 get the maximum number of Gongdan NO
                        int iLineNo = Convert.ToInt32(strLineNo) + 1;
                        dr["GongDan No"] = strBatchNo + "-" + iLineNo.ToString().Trim();
                    }
                    strBatchNoList = strBatchNo;
                    strGongDanNo = dr["GongDan No"].ToString().Trim();
                }
            }
            dtGongDanList.AcceptChanges();
            dtMaxGD.Dispose();
            DataView dv = dtGongDanList.DefaultView;
            dv.Sort = "Batch No ASC";
            dtGongDanList = dv.ToTable();
            string[] strFieldName = { "GongDan No" };
            SqlLib sqlLib = new SqlLib();
            DataTable dtListGD = sqlLib.SelectDistinct(dtGongDanList, strFieldName);
            sqlLib.Dispose(0);
            if (dtListGD.Rows.Count < dtGongDanList.Rows.Count)
            {
                foreach (DataRow dr in dtListGD.Rows)
                {
                    string strGD = dr[0].ToString().Trim();
                    DataRow[] drow = dtGongDanList.Select("[GongDan No]='" + strGD + "'");
                    int iLineNum = Convert.ToInt32(strGD.Split('-')[1]);
                    if (drow.Length > 1)
                    {
                        for (int i = 1; i < drow.Length; i++)
                        {
                            iLineNum = iLineNum + 1;
                            drow[i]["GongDan No"] = strGD.Split('-')[0] + "-" + iLineNum.ToString();
                        }
                    }
                }
            }
            dtGongDanList.AcceptChanges();
            dtListGD.Dispose();

            DataRow[] drBLP = dtGongDanList.Select("[Shipto Party]='000077441'");
            if (drBLP.Length > 0) { foreach (DataRow dr in drBLP) { dr["IE Type"] = "BLP"; } }           
            DataRow[] drExport = dtGongDanList.Select("[Delivery SLOC]='PC23' AND [Order Currency]<>'CNY' AND [Destination]<>'CN'");
            if (drExport.Length > 0) { foreach (DataRow dr in drExport) { dr["IE Type"] = "EXPORT"; } }
            DataRow[] drRMD = dtGongDanList.Select("[Delivery SLOC]='PC23' AND [Order Currency]='CNY' AND [Destination]='CN'");
            if (drRMD.Length > 0) { foreach (DataRow dr in drRMD) { dr["IE Type"] = "RM-D"; } }
            DataRow[] dr1418 = dtGongDanList.Select("[Delivery SLOC]='PC80' AND [Order Currency]<>'CNY' AND [Destination]='CN'");
            if (dr1418.Length > 0) { foreach (DataRow dr in dr1418) { dr["IE Type"] = "1418"; } }
            //DataRow[] drRMB = dtGongDan.Select("[Delivery SLOC]='PC80' AND [Order Currency]='CNY' AND [Destination]='CN'");
            //if (drRMB.Length > 0) { foreach (DataRow dr in drRMB) { dr["IE Type"] = "RMB-1418"; } }
            dtGongDanList.Columns.Remove("Delivery SLOC");
            dtGongDanList.Columns.Remove("Shipto Party");
            dtGongDanList.AcceptChanges();

            dtGongDanList.Columns.Add("GD Pending", typeof(bool));
            dtGongDanList.Columns.Add("JudgeQty", typeof(bool));
            dtGongDanList.Columns.Add("JudgePrice", typeof(bool));
            dtGongDanList.Columns["GD Pending"].DefaultValue = false;
            dtGongDanList.Columns["JudgeQty"].DefaultValue = false;
            dtGongDanList.Columns["JudgePrice"].DefaultValue = false;
            dvGongDanList = dtGongDanList.DefaultView;
            this.dgvGongDanList.DataSource = dvGongDanList;
            this.dgvGongDanList.Columns["Delivery No"].Visible = false;
            this.dgvGongDanList.Columns["JudgeQty"].Visible = false;
            this.dgvGongDanList.Columns["JudgePrice"].Visible = false;
            this.dgvGongDanList.Columns["GongDan No"].Frozen = true;
            this.dgvGongDanList.Columns["Avail Qty"].ReadOnly = true;
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

        public void dgvGongDanList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1) //Delete
            {
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (this.dgvGongDanList.Columns[0].HeaderText.Trim() == "Select")
                    { MessageBox.Show("Please choose to delete the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    if (this.dgvGongDanList.Columns.Contains("GD Pending"))
                    {
                        SqlConnection ConnDelGDL = new SqlConnection(SqlLib.StrSqlConnection);
                        ConnDelGDL.Open();
                        SqlCommand CommDelGDL = new SqlCommand();
                        CommDelGDL.Connection = ConnDelGDL;

                        string strGongDanList = null;
                        int iRow = this.dgvGongDanList.Rows.Count;
                        for (int i = 0; i < iRow; i++)
                        {
                            if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                            { strGongDanList += "'" + this.dgvGongDanList["GongDan No", i].Value.ToString().Trim() + "',"; }                           
                        }
                        if (!String.IsNullOrEmpty(strGongDanList))
                        {
                            strGongDanList = strGongDanList.Remove(strGongDanList.Length - 1);
                            CommDelGDL.CommandText = "DELETE FROM M_DailyGongDanList WHERE [GongDan No] IN (" + strGongDanList + ")";
                            CommDelGDL.ExecuteNonQuery();
                            CommDelGDL.CommandText = "DELETE FROM L_GongDan_Fulfillment WHERE [GongDan No] IN (" + strGongDanList + ")";
                            CommDelGDL.ExecuteNonQuery();
                        }
                        CommDelGDL.Dispose();
                        ConnDelGDL.Dispose();
                    }

                    int iRows = this.dgvGongDanList.Rows.Count;
                    for (int j = 0; j < iRows; j++)
                    {
                        if (String.Compare(this.dgvGongDanList[0, j].EditedFormattedValue.ToString(), "True") == 0)
                        {
                            this.dgvGongDanList.Rows.RemoveAt(j);
                            j--;
                            iRows--;
                        }
                    }
                }
            }
            if (e.ColumnIndex == 2) //Split
            {
                int iCurrentRow = this.dgvGongDanList.CurrentRow.Index;
                string strGongDan = this.dgvGongDanList["GongDan No", iCurrentRow].Value.ToString().Trim();
                string strGongDanQty = this.dgvGongDanList["GongDan Qty", iCurrentRow].Value.ToString().Trim();
                string strOrderPrice = this.dgvGongDanList["Order Price", iCurrentRow].Value.ToString().Trim();
                               
                DataRow dRow = dtGongDanList.NewRow();
                dRow["Batch No"] = this.dgvGongDanList["Batch No", iCurrentRow].Value.ToString().Trim();
                dRow["GongDan No"] = strGongDan.Split('-')[0] + "-" + (Convert.ToInt32(strGongDan.Split('-')[1]) + 1).ToString().Trim();
                dRow["IE Type"] = this.dgvGongDanList["IE Type", iCurrentRow].Value.ToString().Trim().ToUpper();                
                if (String.IsNullOrEmpty(strGongDanQty)) { dRow["GongDan Qty"] = 0; }
                else { dRow["GongDan Qty"] = Convert.ToInt32(strGongDanQty); }
                if (this.dgvGongDanList.Columns.Contains("Avail Qty"))
                {
                    string strAvailQty = this.dgvGongDanList["Avail Qty", iCurrentRow].Value.ToString().Trim();
                    if (String.IsNullOrEmpty(strAvailQty)) { dRow["Avail Qty"] = 0; }
                    else { dRow["Avail Qty"] = Convert.ToInt32(strAvailQty); }
                }                
                if (String.IsNullOrEmpty(strOrderPrice)) { dRow["Order Price"] = 0.0M; }
                else { dRow["Order Price"] = Convert.ToDecimal(strOrderPrice); }
                dRow["Order Currency"] = this.dgvGongDanList["Order Currency", iCurrentRow].Value.ToString().Trim().ToUpper();  
                dRow["Order No"] = this.dgvGongDanList["Order No", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["PartNo"] = this.dgvGongDanList["PartNo", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["Destination"] = this.dgvGongDanList["Destination", iCurrentRow].Value.ToString().Trim().ToUpper();                                    
                dRow["Delivery No"] = this.dgvGongDanList["Delivery No", iCurrentRow].Value.ToString().Trim().ToUpper();
                if (this.dgvGongDanList.Columns.Contains("GD Pending"))
                {
                    string strGdPending = this.dgvGongDanList["GD Pending", iCurrentRow].Value.ToString().Trim().ToUpper();
                    if (String.IsNullOrEmpty(strGdPending) || String.Compare(strGdPending, "FALSE") == 0) { dRow["GD Pending"] = false; }
                    else { dRow["GD Pending"] = true; }
                }
                dRow["JudgeQty"] = false;
                dRow["JudgePrice"] = false;
                dtGongDanList.Rows.Add(dRow);

                DataView dv = dtGongDanList.DefaultView;
                dv.Sort = "GongDan No";
                this.dgvGongDanList.DataSource = dtGongDanList;
            }
            if (e.ColumnIndex == 5) //IE Type comboBox value
            {
                int iIeType = this.dgvGongDanList.Columns["IE Type"].Index;
                if (this.dgvGongDanList.CurrentCell.ColumnIndex == iIeType)
                {
                    FunctionDGV_IETYPE();
                    dgvDetails.Width = 119;
                    dgvDetails.Height = 180;

                    Rectangle rec = this.dgvGongDanList.GetCellDisplayRectangle(4, this.dgvGongDanList.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvGongDanList.Columns[4].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvGongDanList.Location.Y > this.dgvGongDanList.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else { dgvDetails.Top = rec.Top + this.dgvGongDanList.Location.Y; }

                    if (dtIE.Rows.Count > 0) { dgvDetails.Visible = true; }
                }
            }
            if (e.ColumnIndex != 5) { dgvDetails.Visible = false; }
        }

        private void DGV_Details_CellClick(object sender, EventArgs e)
        {
            int iIeType = this.dgvGongDanList.Columns["IE Type"].Index;
            if (this.dgvGongDanList.CurrentCell != null && this.dgvGongDanList.CurrentCell.ColumnIndex == iIeType)
            {
                string strIE = dgvDetails["IE Type", dgvDetails.CurrentCell.RowIndex].Value.ToString().Trim();
                this.dgvGongDanList[iIeType, this.dgvGongDanList.CurrentCell.RowIndex].Value = strIE;
            }
            dgvDetails.Visible = false;     
        }

        private void FunctionDGV_IETYPE()
        {
            if (dtIE.Rows.Count == 0)
            {
                dtIE.Clear();
                SqlLib sqllib = new SqlLib();
                dtIE = sqllib.GetDataTable(@"SELECT [ObjectValue] AS [IE Type] FROM B_SysInfo WHERE [ObjectName] = 'IE Type'").Copy();
                sqllib.Dispose(0);
            }
            dgvDetails.DataSource = dtIE;
            this.dgvGongDanList.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);         
        }

        private void btnGenerateList_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (this.dgvGongDanList.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select data to generate GongDan list.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            DialogResult dr = MessageBox.Show("If you want to insert new data, please click Yes button, else click No button to update the data.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Cancel) { return; }
            SqlConnection gdlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gdlConn.State == ConnectionState.Closed) { gdlConn.Open(); }
            SqlCommand gdlComm = new SqlCommand();
            gdlComm.Connection = gdlConn;
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                DateTime dtNow = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm"));
                int iRowCount = this.dgvGongDanList.RowCount;
                for (int i = 0; i < iRowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        string strBatchNo = this.dgvGongDanList["Batch No", i].Value.ToString().Trim().ToUpper();
                        string strGongDan = this.dgvGongDanList["GongDan No", i].Value.ToString().Trim().ToUpper();
                        string strIeType = this.dgvGongDanList["IE Type", i].Value.ToString().Trim().ToUpper();
                        string strGdQty = this.dgvGongDanList["GongDan Qty", i].Value.ToString().Trim();
                        string strOrderPrice = this.dgvGongDanList["Order Price", i].Value.ToString().Trim();
                        string strOrderCurr = this.dgvGongDanList["Order Currency", i].Value.ToString().Trim().ToUpper();
                        string strOrderNo = this.dgvGongDanList["Order No", i].Value.ToString().Trim().ToUpper();
                        string strPartNo = this.dgvGongDanList["PartNo", i].Value.ToString().Trim().ToUpper();
                        string strDestination = this.dgvGongDanList["Destination", i].Value.ToString().Trim().ToUpper();
                        string strDeliveryNo = this.dgvGongDanList["Delivery No", i].Value.ToString().Trim().ToUpper();
                        string strGdPending = this.dgvGongDanList["GD Pending", i].Value.ToString().Trim().ToUpper();

                        gdlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                        gdlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;
                        gdlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = strIeType;
                        if (String.IsNullOrEmpty(strGdQty)) { gdlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { gdlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGdQty); }
                        if (String.IsNullOrEmpty(strOrderPrice)) { gdlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { gdlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(strOrderPrice); }
                        gdlComm.Parameters.Add("@OrderCurr", SqlDbType.NVarChar).Value = strOrderCurr;
                        gdlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = strOrderNo;
                        gdlComm.Parameters.Add("@PartNo", SqlDbType.NVarChar).Value = strPartNo;
                        gdlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = strDestination;
                        gdlComm.Parameters.Add("@DeliveryNo", SqlDbType.NVarChar).Value = strDeliveryNo;
                        if (String.IsNullOrEmpty(strIeType))
                        { gdlComm.Parameters.Add("@GdPending", SqlDbType.Bit).Value = true; }
                        else if (String.IsNullOrEmpty(strGdPending) || String.Compare(strGdPending, "FALSE") == 0)
                        { gdlComm.Parameters.Add("@GdPending", SqlDbType.Bit).Value = false; }
                        else { gdlComm.Parameters.Add("@GdPending", SqlDbType.Bit).Value = true; }
                        gdlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = dtNow;

                        gdlComm.CommandText = "INSERT INTO M_DailyGongDanList([Batch No], [GongDan No], [IE Type], [GongDan Qty], [Order Price], [Order Currency], " +
                                              "[Order No], [PartNo], [Destination], [Delivery No], [GD Pending], [Created Date]) VALUES(@BatchNo, @GongDanNo, @IEType, " +
                                              "@GongDanQty, @OrderPrice, @OrderCurr, @OrderNo, @PartNo, @Destination, @DeliveryNo, @GdPending, @CreatedDate)";
                        gdlComm.ExecuteNonQuery();
                        gdlComm.Parameters.Clear();

                        gdlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                        gdlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;
                        gdlComm.Parameters.Add("@DeliveryNo", SqlDbType.NVarChar).Value = strDeliveryNo;
                        gdlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = dtNow;
                        gdlComm.CommandText = "INSERT INTO L_GongDan_Fulfillment([Batch No], [GongDan No], [Delivery No], [Created Date]) " +
                                              "VALUES(@BatchNo, @GongDanNo, @DeliveryNo, @CreatedDate)";
                        gdlComm.ExecuteNonQuery();
                        gdlComm.Parameters.Clear();

                        this.dgvGongDanList.Rows.RemoveAt(i);
                        i--;
                        iRowCount--;
                    }
                }
            }
            else
            {
                int iRowCount = this.dgvGongDanList.RowCount;
                for (int i = 0; i < iRowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        string strBatchNo = this.dgvGongDanList["Batch No", i].Value.ToString().Trim().ToUpper();
                        string strGongDan = this.dgvGongDanList["GongDan No", i].Value.ToString().Trim().ToUpper();
                        string strIeType = this.dgvGongDanList["IE Type", i].Value.ToString().Trim().ToUpper();
                        string strGdQty = this.dgvGongDanList["GongDan Qty", i].Value.ToString().Trim();
                        string strOrderPrice = this.dgvGongDanList["Order Price", i].Value.ToString().Trim();
                        string strOrderCurr = this.dgvGongDanList["Order Currency", i].Value.ToString().Trim().ToUpper();
                        string strOrderNo = this.dgvGongDanList["Order No", i].Value.ToString().Trim().ToUpper();
                        string strPartNo = this.dgvGongDanList["PartNo", i].Value.ToString().Trim().ToUpper();
                        string strDestination = this.dgvGongDanList["Destination", i].Value.ToString().Trim().ToUpper();
                        string strDeliveryNo = this.dgvGongDanList["Delivery No", i].Value.ToString().Trim().ToUpper();
                        string strGdPending = this.dgvGongDanList["GD Pending", i].Value.ToString().Trim().ToUpper();

                        gdlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;                      
                        gdlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = strIeType;
                        gdlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGdQty); 
                        gdlComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(strOrderPrice); 
                        gdlComm.Parameters.Add("@OrderCurr", SqlDbType.NVarChar).Value = strOrderCurr;
                        gdlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = strOrderNo;
                        gdlComm.Parameters.Add("@PartNo", SqlDbType.NVarChar).Value = strPartNo;
                        gdlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = strDestination;
                        gdlComm.Parameters.Add("@DeliveryNo", SqlDbType.NVarChar).Value = strDeliveryNo;
                        if (String.IsNullOrEmpty(strGdPending) || String.Compare(strGdPending, "FALSE") == 0)
                        { gdlComm.Parameters.Add("@GdPending", SqlDbType.Bit).Value = false; }
                        else { gdlComm.Parameters.Add("@GdPending", SqlDbType.Bit).Value = true; }
                        gdlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDan;

                        gdlComm.CommandText = "UPDATE M_DailyGongDanList SET [Batch No] = @BatchNo, [IE Type] = @IEType, [GongDan Qty] = @GongDanQty, " +
                                              "[Order Price] = @OrderPrice, [Order Currency] = @OrderCurr, [Order No] = @OrderNo, [PartNo] = @PartNo, " +
                                              "[Destination] = @Destination, [Delivery No] = @DeliveryNo, [GD Pending] = @GdPending WHERE [GongDan No] = @GongDanNo";
                        gdlComm.ExecuteNonQuery();
                        gdlComm.Parameters.Clear();
                    }
                }
                MessageBox.Show("Update data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            gdlComm.Dispose();
            if (gdlConn.State == ConnectionState.Open) { gdlConn.Close(); gdlConn.Dispose(); }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvGongDanList.RowFilter = "";
            this.btnCheckData.Enabled = false;
            SqlConnection gdlViewConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gdlViewConn.State == ConnectionState.Closed) { gdlViewConn.Open(); }
            string strSQL = @"SELECT [Batch No], [GongDan No], [IE Type], [GongDan Qty], [Order Price], [Order Currency], [Order No], [PartNo], [Destination], " +
                             "[Delivery No], [GD Pending] FROM M_DailyGongDanList ORDER BY [GongDan No]";
            SqlDataAdapter gdlViewAdapter = new SqlDataAdapter(strSQL, gdlViewConn);
            DataTable dtViewGDL = new DataTable();
            gdlViewAdapter.Fill(dtViewGDL);
            gdlViewAdapter.Dispose();
            if (dtViewGDL.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtViewGDL.Dispose();
                gdlViewConn.Dispose();
                return;
            }
            dtGongDanList.Rows.Clear();
            dtGongDanList.Columns.Clear();
            dtGongDanList = dtViewGDL.Copy();
            dtViewGDL.Dispose();
            dtGongDanList.Columns.Add("JudgeQty", typeof(bool));
            dtGongDanList.Columns.Add("JudgePrice", typeof(bool));
            dtGongDanList.Columns["JudgeQty"].DefaultValue = false;
            dtGongDanList.Columns["JudgePrice"].DefaultValue = false;

            dvGongDanList = dtGongDanList.DefaultView;
            this.dgvGongDanList.DataSource = dvGongDanList;
            this.dgvGongDanList.Columns[0].HeaderText = "Select";
            this.dgvGongDanList.Columns["Delivery No"].Visible = false;
            this.dgvGongDanList.Columns["JudgeQty"].Visible = false;
            this.dgvGongDanList.Columns["JudgePrice"].Visible = false;
            this.dgvGongDanList.Columns["GongDan No"].Frozen = true;
            if (gdlViewConn.State == ConnectionState.Open) { gdlViewConn.Close(); gdlViewConn.Dispose(); }
        }

        private void btnDownloadData_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (this.dgvGongDanList.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application gdl_xls = new Microsoft.Office.Interop.Excel.Application();
            gdl_xls.Application.Workbooks.Add(true);
            int iRow = 2;
            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    //gdl_xls.get_Range(gdl_xls.Cells[iRow, 1], gdl_xls.Cells[iRow, this.dgvGongDanList.ColumnCount - 4]).NumberFormatLocal = "@";
                    for (int j = 4; j < this.dgvGongDanList.ColumnCount - 2; j++)
                    { gdl_xls.Cells[iRow, j - 3] = this.dgvGongDanList[j, i].Value.ToString().Trim(); }
                    iRow++;
                }
            }
            //gdl_xls.get_Range(gdl_xls.Cells[1, 1], gdl_xls.Cells[1, this.dgvGongDanList.ColumnCount - 4]).NumberFormatLocal = "@";
            for (int k = 4; k < this.dgvGongDanList.ColumnCount - 2; k++)
            { gdl_xls.Cells[1, k - 3] = this.dgvGongDanList.Columns[k].HeaderText.ToString(); }

            //gdl_xls.get_Range(gdl_xls.Cells[1, 1], gdl_xls.Cells[1, this.dgvGongDanList.ColumnCount - 4]).Font.Bold = true;
            //gdl_xls.get_Range(gdl_xls.Cells[1, 1], gdl_xls.Cells[1, this.dgvGongDanList.ColumnCount - 4]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            //gdl_xls.get_Range(gdl_xls.Cells[1, 1], gdl_xls.Cells[iRow - 1, this.dgvGongDanList.ColumnCount - 4]).Font.Name = "Verdana";
            //gdl_xls.get_Range(gdl_xls.Cells[1, 1], gdl_xls.Cells[iRow - 1, this.dgvGongDanList.ColumnCount - 4]).Font.Size = 9;
            //gdl_xls.get_Range(gdl_xls.Cells[1, 1], gdl_xls.Cells[iRow - 1, this.dgvGongDanList.ColumnCount - 4]).Borders.LineStyle = 1;
            gdl_xls.Cells.EntireColumn.AutoFit();
            gdl_xls.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            gdl_xls.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(gdl_xls);
            gdl_xls = null;
        }

        private void tsmiQtyOverFlow_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim())) { strFilter += " AND [JudgeQty] = True"; }
                else { strFilter = "[JudgeQty] = True"; }
                dvGongDanList.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
            }
        }

        private void tsmiPriceZero_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim())) { strFilter += " AND [JudgePrice] = True"; }
                else { strFilter = "[JudgePrice] = True"; }
                dvGongDanList.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvGongDanList.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvGongDanList.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvGongDanList.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvGongDanList[strColumnName, this.dgvGongDanList.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] = " + strColumnText; }
                    }
                }
                dvGongDanList.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiExcludeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvGongDanList.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvGongDanList.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvGongDanList.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvGongDanList[strColumnName, this.dgvGongDanList.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] <> " + strColumnText; }
                    }
                }
                dvGongDanList.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvGongDanList.RowFilter = "";            
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.CurrentCell != null)
            {
                string strColumnName = this.dgvGongDanList.Columns[this.dgvGongDanList.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter); 
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dvGongDanList.ToTable(true, new string[]{strColumnName});
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
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string)) 
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }                                                                 
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; } 
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }                
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                dvGongDanList.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex) 
            { MessageBox.Show(ex.Message); }
        }

        private void btnCheckData_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!String.IsNullOrEmpty(dvGongDanList.RowFilter))
            { MessageBox.Show("Check data should make sure the datagrid without filter.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string[] strFieldName = { "Batch No" };
            SqlLib sqlLib = new SqlLib();
            DataTable dtCheck = sqlLib.SelectDistinct(dtGongDanList, strFieldName);
            sqlLib.Dispose(0);
            foreach (DataRow dr in dtCheck.Rows)
            {
                DataRow[] drow = dtGongDanList.Select("[Batch No]='" + dr[0].ToString().Trim() + "'", "[Gongdan No] ASC"); //Added sorting logic on May.16.2017
                if (drow.Length > 1)
                {
                    int iAvailQty = Convert.ToInt32(drow[0]["Avail Qty"].ToString().Trim());
                    for (int i = 1; i < drow.Length; i++)
                    {
                        int iGongDanQty = Convert.ToInt32(drow[i-1]["GongDan Qty"].ToString().Trim());
                        iAvailQty = iAvailQty - iGongDanQty;
                        drow[i]["Avail Qty"] = iAvailQty;
                    }
                }
            }
            dtGongDanList.AcceptChanges();
            dtCheck.Dispose();
            int iCheck = 0;
            DataRow[] drCheck = dtGongDanList.Select("[GongDan Qty] > [Avail Qty]");
            if (drCheck.Length > 0)
            {
                foreach (DataRow dr in drCheck) { dr["JudgeQty"] = true; }
                dtGongDanList.AcceptChanges();
                iCheck = drCheck.Length;             
            }
            drCheck = dtGongDanList.Select("[Order Price] > 0 AND [JudgePrice] = 1");
            if (drCheck.Length > 0)
            {
                foreach (DataRow dr in drCheck) { dr["JudgePrice"] = false; }
                dtGongDanList.AcceptChanges();
            }
            drCheck = dtGongDanList.Select("[Order Price] = 0");
            if (drCheck.Length > 0)
            {
                foreach (DataRow dr in drCheck) { dr["JudgePrice"] = true; }
                dtGongDanList.AcceptChanges();              
                iCheck += drCheck.Length;
            }
            
            if (iCheck > 0)
            {
                this.dgvGongDanList.Refresh();
                MessageBox.Show("Attention:  " + iCheck.ToString() + " abnormal data, please filter the datagrid to check Gongdan available quantity or order price.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else { MessageBox.Show("There is not abnormal data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxShow.Visible == false) { this.gBoxShow.Visible = true; }
            else { this.gBoxShow.Visible = false; }
        }

        private void btnDownloadDoc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            DateTime dNow = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
            DateTime dInterval;
            if (this.rbtn1.Checked == true) { dInterval = dNow.AddDays(-181); }
            else if (this.rbtn2.Checked == true) { dInterval = dNow.AddDays(-361); }
            else if (this.rbtn3.Checked == true) { dInterval = dNow.AddDays(-721); }
            else { dInterval = dNow.AddDays(-91); }

            SqlConnection downloadConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (downloadConn.State == ConnectionState.Closed) { downloadConn.Open(); }
            SqlCommand downloadComm = new SqlCommand();
            downloadComm.Connection = downloadConn;
            downloadComm.CommandType = CommandType.StoredProcedure;
            downloadComm.CommandText = @"usp_QueryOrderFulfillmentData";
            downloadComm.Parameters.AddWithValue("@SQL", dInterval);
            SqlDataAdapter downloadAdapter = new SqlDataAdapter();
            downloadAdapter.SelectCommand = downloadComm;
            DataTable myName = new DataTable();
            downloadAdapter.Fill(myName);
            downloadAdapter.Dispose();
            downloadComm.Dispose();
            if (downloadConn.State == ConnectionState.Open)
            {
                downloadConn.Close();
                downloadConn.Dispose();
            }

            if (myName.Rows.Count == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(myName.Rows.Count / PageRow);
                if (iPageCount * PageRow < myName.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\Query_OrderFulfillment_DataStatus" + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); 
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < myName.Columns.Count; n++)
                        { sb.Append(myName.Columns[n].ColumnName.Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < myName.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < myName.Columns.Count; j++) { sb.Append("'" + myName.Rows[i][j].ToString().Trim() + "\t"); }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    MessageBox.Show("Successfully download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            myName.Dispose();
        }
    }
}