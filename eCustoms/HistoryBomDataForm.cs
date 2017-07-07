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
    public partial class HistoryBomDataForm : Form
    {        
        private LoginForm loginFrm = new LoginForm();
        private static HistoryBomDataForm HistoryBomDataFrm;
        public HistoryBomDataForm() { InitializeComponent(); }
        public static HistoryBomDataForm CreateInstance()
        {
            if (HistoryBomDataFrm == null || HistoryBomDataFrm.IsDisposed) { HistoryBomDataFrm = new HistoryBomDataForm(); }
            return HistoryBomDataFrm;
        }

        private void HistoryBomDataForm_Load(object sender, EventArgs e)
        {
            this.dtpBomFrm.CustomFormat = " ";
            this.dtpBomTo.CustomFormat = " ";  
        }
        private void dtpBomFrm_ValueChanged(object sender, EventArgs e)
        { this.dtpBomFrm.CustomFormat = null; }
        private void dtpBomFrm_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpBomFrm.CustomFormat = " "; } }
        private void dtpBomTo_ValueChanged(object sender, EventArgs e)
        { this.dtpBomTo.CustomFormat = null; }
        private void dtpBomTo_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpBomTo.CustomFormat = " "; } }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strGo = "SELECT [Freeze], [Process Order No], [Batch No], [FG No], [FG Description], [FG Qty], [Total Input Qty], [Drools Qty], " +
                           "[Order Price(USD)], [Total RM Cost(USD)], [Qty Loss Rate], [HS Code], [CHN Name], [Creater], [Created Date], [Actual Start Date], " +
                           "[Actual End Date], [BOM In Customs], [GongDan Used Qty], [Remark] FROM C_BOM WHERE";
            if (!String.IsNullOrEmpty(this.dtpBomTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpBomFrm.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpBomFrm.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpBomTo.Value.ToString("M/d/yyyy"))) == 1)
                    { MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    else { strGo += " [Created Date] >= '" + Convert.ToDateTime(this.dtpBomFrm.Value.ToString("M/d/yyyy")) + "' AND [Created Date] < '" + Convert.ToDateTime(this.dtpBomTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else { strGo += " [Created Date] < '" + Convert.ToDateTime(this.dtpBomTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpBomFrm.Text.Trim()))
                { strGo += " [Created Date] >= '" + Convert.ToDateTime(this.dtpBomFrm.Value.ToString("M/d/yyyy")) + "' AND"; }
            }
            string strBatchNo = this.txtBatchNo.Text.Trim().ToUpper();
            if (!String.IsNullOrEmpty(strBatchNo)) { strGo += " [Batch No] = '" + strBatchNo + "' AND"; }
            if (String.Compare(strGo.Substring(strGo.Trim().Length - 5, 5), "WHERE") == 0)
            { MessageBox.Show("Please select the filter condition.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            strGo = strGo.Remove(strGo.Trim().Length - 3).Trim() + " ORDER BY [Created Date] DESC, [Batch No] ASC";

            SqlConnection BomMConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BomMConn.State == ConnectionState.Closed) { BomMConn.Open(); }
            SqlDataAdapter BomMAdapter = new SqlDataAdapter(strGo, BomMConn);
            DataTable dtBomM = new DataTable();
            dtBomM.Clear();
            BomMAdapter.Fill(dtBomM);
            BomMAdapter.Dispose();
            if (dtBomM.Rows.Count == 0)
            {
                dtBomM.Dispose();
                BomMConn.Dispose();
                this.dgvMasterBOM.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.dgvMasterBOM.DataSource = dtBomM;
            this.dgvMasterBOM.Columns[0].HeaderText = "Select";
            for (int i = 3; i < this.dgvMasterBOM.ColumnCount; i++) { this.dgvMasterBOM.Columns[i].ReadOnly = true; }
            this.dgvMasterBOM.Columns["FG No"].Frozen = true;
            if (this.dgvDetailBOM.RowCount > 0) { this.dgvDetailBOM.Columns.Clear(); }
            if (BomMConn.State == ConnectionState.Open) { BomMConn.Close(); BomMConn.Dispose(); }
        }

        private void dgvMasterBOM_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvMasterBOM.RowCount; i++) { this.dgvMasterBOM[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvMasterBOM.RowCount; i++) { this.dgvMasterBOM[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvMasterBOM.RowCount; i++)
                    {
                        if (String.Compare(this.dgvMasterBOM[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvMasterBOM[0, i].Value = true; }
                        else { this.dgvMasterBOM[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }

            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvMasterBOM_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvMasterBOM.RowCount == 0) { return; }
            if (this.dgvMasterBOM.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvMasterBOM.RowCount; i++)
                {
                    if (String.Compare(this.dgvMasterBOM[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }
                if (iCount < this.dgvMasterBOM.RowCount && iCount > 0)
                { this.dgvMasterBOM.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvMasterBOM.RowCount)
                { this.dgvMasterBOM.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvMasterBOM.Columns[0].HeaderText = "Select"; }
            }
        }

        private void dgvMasterBOM_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1) 
            {               
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    int iRow = this.dgvMasterBOM.CurrentRow.Index;
                    string strFreeze = this.dgvMasterBOM["Freeze", iRow].Value.ToString().Trim().ToUpper();
                    if (String.Compare(strFreeze, "TRUE") == 0)
                    { MessageBox.Show("The BOM has froze, reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

                    string strGongDanUsedQty = this.dgvMasterBOM["GongDan Used Qty", iRow].Value.ToString().Trim();
                    if (Convert.ToInt32(strGongDanUsedQty) > 0)
                    { MessageBox.Show("The BOM has generated GongDan, reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
                  
                    SqlConnection BomDelConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (BomDelConn.State == ConnectionState.Closed) { BomDelConn.Open(); }
                    SqlCommand BomDelComm = new SqlCommand();
                    BomDelComm.Connection = BomDelConn;

                    #region //Delete selected data in C_BOM, C_BOMDetail, E_Consumption and E_OriginalGoods four tables
                    string strBatchNo = this.dgvMasterBOM["Batch No", iRow].Value.ToString().Trim();
                    string strFGNo = this.dgvMasterBOM["FG Description", iRow].Value.ToString().Trim();
                    strFGNo = strFGNo.Split('-')[0] + "-" + strFGNo.Split('-')[1];
                    BomDelComm.CommandType = CommandType.StoredProcedure;
                    BomDelComm.CommandText = @"usp_DeleteHistoryBom";
                    BomDelComm.Parameters.AddWithValue("@BatchNo", strBatchNo);
                    BomDelComm.Parameters.AddWithValue("@FGEHB", strFGNo + '/' + strBatchNo);
                    BomDelComm.ExecuteNonQuery();
                    BomDelComm.Parameters.Clear();
                    #endregion

                    BomDelComm.Dispose();
                    if (BomDelConn.State == ConnectionState.Open) { BomDelConn.Close(); BomDelConn.Dispose(); }
                    this.dgvMasterBOM.Rows.RemoveAt(iRow);
                    this.dgvDetailBOM.Columns.Clear();
                }
            }
            if (e.ColumnIndex == 3)
            {
                string strFreeze = this.dgvMasterBOM["Freeze", this.dgvMasterBOM.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                if (String.Compare(strFreeze, "TRUE") == 0)
                { MessageBox.Show("System reject to release the frozen BOM.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show("Are you sure to freeze the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) { return; }

                SqlConnection freezeBomConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (freezeBomConn.State == ConnectionState.Closed) { freezeBomConn.Open(); }
                SqlCommand freezeBomComm = new SqlCommand();
                freezeBomComm.Connection = freezeBomConn;

                string strBatch = this.dgvMasterBOM["Batch No", this.dgvMasterBOM.CurrentRow.Index].Value.ToString().Trim();
                string strFG = this.dgvMasterBOM["FG No", this.dgvMasterBOM.CurrentRow.Index].Value.ToString().Trim();
                string strRemark = "/freeze by " + loginFrm.PublicUserName + System.DateTime.Now.ToString("M/d/yyyy");
                freezeBomComm.Parameters.Clear();
                freezeBomComm.Parameters.Add("@Freeze", SqlDbType.Bit).Value = true;
                freezeBomComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                freezeBomComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatch;
                freezeBomComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFG;
                freezeBomComm.CommandText = @"UPDATE C_BOM SET [Freeze] = @Freeze, [Remark] = @Remark WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                freezeBomComm.ExecuteNonQuery();
                freezeBomComm.Parameters.Clear();
                freezeBomComm.Dispose();
                if (freezeBomConn.State == ConnectionState.Open) { freezeBomConn.Close(); freezeBomConn.Dispose(); }

                this.dgvMasterBOM["Remark", this.dgvMasterBOM.CurrentRow.Index].Value = strRemark;
                this.dgvMasterBOM["Freeze", this.dgvMasterBOM.CurrentRow.Index].Value = true;
                this.dgvMasterBOM.CurrentRow.Selected = true;
            }
            if (e.ColumnIndex == 2) 
            {               
                SqlConnection BomDtlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (BomDtlConn.State == ConnectionState.Closed) { BomDtlConn.Open(); }
                SqlCommand BomDtlComm = new SqlCommand();
                BomDtlComm.Connection = BomDtlConn;
                BomDtlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvMasterBOM["Batch No", this.dgvMasterBOM.CurrentCell.RowIndex].Value.ToString().Trim();
                BomDtlComm.CommandText = @"SELECT * FROM C_BOMDetail WHERE [Batch No] = @BatchNo";

                SqlDataAdapter BomDtlAdapter = new SqlDataAdapter();
                BomDtlAdapter.SelectCommand = BomDtlComm;
                DataTable dtBomDtl = new DataTable();
                BomDtlAdapter.Fill(dtBomDtl);
                BomDtlAdapter.Dispose();
                this.dgvDetailBOM.DataSource = DBNull.Value;
                this.dgvDetailBOM.DataSource = dtBomDtl;
                this.dgvDetailBOM.Columns["Batch Path"].Visible = false;
                this.dgvDetailBOM.Columns["Batch No"].Visible = false;
                this.dgvDetailBOM.Columns["Line No"].Frozen = true;
                BomDtlComm.Parameters.Clear();
                BomDtlComm.Dispose();
                if (BomDtlConn.State == ConnectionState.Open) { BomDtlConn.Close(); BomDtlConn.Dispose(); }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvMasterBOM.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (this.dgvMasterBOM.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select the data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection BomDlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BomDlConn.State == ConnectionState.Closed) { BomDlConn.Open(); }
            SqlCommand BomDlComm = new SqlCommand();
            BomDlComm.Connection = BomDlConn;
            
            DataTable dtBomDl = new DataTable();
            Microsoft.Office.Interop.Excel.Application BomDl_exl = new Microsoft.Office.Interop.Excel.Application();
            BomDl_exl.Application.Workbooks.Add(true);
            if (this.dgvMasterBOM.RowCount > 0)
            {
                int iActualRow = 0;
                for (int x = 0; x < this.dgvMasterBOM.RowCount; x++)
                {
                    if (String.Compare(this.dgvMasterBOM[0, x].EditedFormattedValue.ToString(), "True") == 0) 
                    {
                        BomDlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvMasterBOM["Batch No", x].Value.ToString().Trim();
                        BomDlComm.CommandText = "SELECT T1.[Process Order No], T1.[Actual Start Date], T1.[Actual End Date], T2.[Batch Path], T1.[Batch No], " + 
                                                "T1.[FG No], T1.[FG Description], T2.[Line No], T2.[Item No], T2.[Item Description], T2.[Lot No], T2.[Inventory Type], " + 
                                                "T2.[RM Category], T1.[FG Qty], T1.[Total Input Qty], T1.[Drools Qty], T1.[Order Price(USD)], T1.[Total RM Cost(USD)], " +
                                                "T1.[Qty Loss Rate], T2.[RM EHB], T2.[BGD No], T2.[RM Qty], T2.[RM Currency], T2.[RM Price], T1.[HS Code], T1.[CHN Name], " + 
                                                "T2.[Consumption], T2.[Drools EHB], T1.[BOM In Customs], T1.[GongDan Used Qty], T1.[Creater], T1.[Created Date], " + 
                                                "T1.[Freeze], T1.[Remark] FROM C_BOM AS T1 LEFT OUTER JOIN C_BOMDetail AS T2 ON T1.[Batch No] = T2.[Batch No] " +
                                                "WHERE T1.[Batch No] = @BatchNo";
                        SqlDataAdapter BomDlAdapter = new SqlDataAdapter();
                        BomDlAdapter.SelectCommand = BomDlComm;
                        dtBomDl.Clear();
                        BomDlAdapter.Fill(dtBomDl);
                        BomDlAdapter.Dispose();

                        for (int y = 0; y < dtBomDl.Rows.Count; y++)
                        {
                            iActualRow++;
                            //BomDl_exl.get_Range(BomDl_exl.Cells[iActualRow + 1, 1], BomDl_exl.Cells[iActualRow + 1, dtBomDl.Columns.Count]).NumberFormatLocal = "@"; 
                            for (int z = 0; z < dtBomDl.Columns.Count; z++) { BomDl_exl.Cells[iActualRow + 1, z + 1] = "'" + dtBomDl.Rows[y][z].ToString().Trim(); }
                        }
                        BomDlComm.Parameters.Clear();
                    }
                }
                //BomDl_exl.get_Range(BomDl_exl.Cells[1, 1], BomDl_exl.Cells[1, dtBomDl.Columns.Count]).NumberFormatLocal = "@";
                for (int k = 0; k < dtBomDl.Columns.Count; k++) { BomDl_exl.Cells[1, k + 1] = dtBomDl.Columns[k].ColumnName.Trim(); }

                //BomDl_exl.get_Range(BomDl_exl.Cells[1, 1], BomDl_exl.Cells[1, dtBomDl.Columns.Count]).Font.Bold = true;
                //BomDl_exl.get_Range(BomDl_exl.Cells[1, 1], BomDl_exl.Cells[1, dtBomDl.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                //BomDl_exl.get_Range(BomDl_exl.Cells[1, 1], BomDl_exl.Cells[iActualRow + 1, dtBomDl.Columns.Count]).Font.Name = "Verdana";
                //BomDl_exl.get_Range(BomDl_exl.Cells[1, 1], BomDl_exl.Cells[iActualRow + 1, dtBomDl.Columns.Count]).Font.Size = 9;
                BomDl_exl.Cells.EntireColumn.AutoFit();
                BomDl_exl.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                BomDl_exl.Visible = true;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(BomDl_exl);
            BomDl_exl = null;
            dtBomDl.Dispose();
            BomDlComm.Dispose();
            if (BomDlConn.State == ConnectionState.Open) { BomDlConn.Close(); BomDlConn.Dispose(); }
        }
    }
}