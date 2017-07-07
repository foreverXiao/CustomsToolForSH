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
    public partial class HistoryGongDanDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private static HistoryGongDanDataForm HistoryGongDanDataFrm;
        public HistoryGongDanDataForm() { InitializeComponent(); }
        public static HistoryGongDanDataForm CreateInstance()
        {
            if (HistoryGongDanDataFrm == null || HistoryGongDanDataFrm.IsDisposed) { HistoryGongDanDataFrm = new HistoryGongDanDataForm(); }
            return HistoryGongDanDataFrm;
        }

        private void HistoryGongDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpGdFrom.CustomFormat = " ";
            this.dtpGdTo.CustomFormat = " ";
        }
        private void dtpGdFrom_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpGdFrom.CustomFormat = " "; } }
        private void dtpGdFrom_ValueChanged(object sender, EventArgs e)
        { this.dtpGdFrom.CustomFormat = null; }
        private void dtpGdTo_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpGdTo.CustomFormat = " "; } }
        private void dtpGdTo_ValueChanged(object sender, EventArgs e)
        { this.dtpGdTo.CustomFormat = null; }

        private void btnGdPreview_Click(object sender, EventArgs e)
        {
            string strGD = "SELECT [Approval Date], [Batch No], [GongDan No], [FG No], [FG Description], [IE Type], [Order No], [GongDan Qty], [Order Price], " +
                           "[Order Currency], [Total RM Qty], [Total RM Cost(USD)], [Drools Rate], [CHN Name], [Destination], [Creater], [Created Date], " +
                           "[Actual Start Date], [Actual End Date], [BeiAnDan Used Qty], [BOM In Customs] FROM C_GongDan WHERE";
            if (!String.IsNullOrEmpty(this.dtpGdTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpGdFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpGdFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpGdTo.Value.ToString("M/d/yyyy"))) == 1)
                    { MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    else { strGD += " [Approval Date] >= '" + Convert.ToDateTime(this.dtpGdFrom.Value.ToString("M/d/yyyy")) + "' AND [Approval Date] < '" + Convert.ToDateTime(this.dtpGdTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else { strGD += " [Approval Date] < '" + Convert.ToDateTime(this.dtpGdTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpGdFrom.Text.Trim()))
                { strGD += " [Approval Date] >= '" + Convert.ToDateTime(this.dtpGdFrom.Value.ToString("M/d/yyyy")) + "' AND"; }
            }
            string strBatchNo = this.txtBatchNo.Text.Trim().ToUpper();
            if (!String.IsNullOrEmpty(strBatchNo)) { strGD += " [Batch No] = '" + strBatchNo + "' AND"; }
            if (String.Compare(strGD.Substring(strGD.Trim().Length - 5, 5), "WHERE") == 0)
            { MessageBox.Show("Please select the filter condition.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            strGD = strGD.Remove(strGD.Length - 3).Trim() + " ORDER BY [Approval Date] DESC";

            SqlConnection ConnGDM = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnGDM.State == ConnectionState.Closed) { ConnGDM.Open(); }
            SqlDataAdapter AdapterGDM = new SqlDataAdapter(strGD, ConnGDM);
            DataTable dtGDM = new DataTable();
            dtGDM.Clear();
            AdapterGDM.Fill(dtGDM);
            AdapterGDM.Dispose();
            if (dtGDM.Rows.Count == 0)
            {
                dtGDM.Dispose();
                ConnGDM.Dispose();
                this.dgvMasterGongDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            this.dgvMasterGongDan.DataSource = dtGDM;
            this.dgvMasterGongDan.Columns[0].HeaderText = "Select";
            for (int i = 3; i < this.dgvMasterGongDan.ColumnCount; i++) { this.dgvMasterGongDan.Columns[i].ReadOnly = true; }
            this.dgvMasterGongDan.Columns["GongDan No"].Frozen = true;
            if (this.dgvDetailGongDan.RowCount > 0) { this.dgvDetailGongDan.Columns.Clear(); }
            if (ConnGDM.State == ConnectionState.Open) { ConnGDM.Close(); ConnGDM.Dispose(); }
        }  

        private void dgvMasterGongDan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvMasterGongDan.RowCount; i++) { this.dgvMasterGongDan[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvMasterGongDan.RowCount; i++) { this.dgvMasterGongDan[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvMasterGongDan.RowCount; i++)
                    {
                        if (String.Compare(this.dgvMasterGongDan[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvMasterGongDan[0, i].Value = true; }
                        else { this.dgvMasterGongDan[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }

            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvMasterGongDan_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvMasterGongDan.RowCount == 0) { return; }
            if (this.dgvMasterGongDan.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvMasterGongDan.RowCount; i++)
                {
                    if (String.Compare(this.dgvMasterGongDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }
                if (iCount < this.dgvMasterGongDan.RowCount && iCount > 0)
                { this.dgvMasterGongDan.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvMasterGongDan.RowCount)
                { this.dgvMasterGongDan.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvMasterGongDan.Columns[0].HeaderText = "Select"; }
            }
        }

        private void dgvMasterGongDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1) 
            {             
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    int iRow = this.dgvMasterGongDan.CurrentRow.Index;
                    string strBeiAnDanQty = this.dgvMasterGongDan["BeiAnDan Used Qty", iRow].Value.ToString().Trim();
                    if (Convert.ToInt32(strBeiAnDanQty) > 0)
                    { MessageBox.Show("The GongDan has generated BeiAnDan, reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
                    string strBatchNo = this.dgvMasterGongDan["Batch No", iRow].Value.ToString().Trim();
                    string strGongDanNo = this.dgvMasterGongDan["GongDan No", iRow].Value.ToString().Trim();
                    string strFgNo = this.dgvMasterGongDan["FG No", iRow].Value.ToString().Trim();
                    string strGongDanQty = this.dgvMasterGongDan["GongDan Qty", iRow].Value.ToString().Trim();

                    SqlConnection GdDelConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (GdDelConn.State == ConnectionState.Closed) { GdDelConn.Open(); }                    
                    SqlCommand GdDelComm = new SqlCommand();
                    GdDelComm.Connection = GdDelConn;

                    #region //Monitor And Control Multiple Users
                    GdDelComm.CommandText = "SELECT * FROM B_MultiUser";
                    string strUserName = Convert.ToString(GdDelComm.ExecuteScalar());
                    if (!String.IsNullOrEmpty(strUserName))
                    {
                        if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                        {
                            MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait for him/her to finish the process.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            GdDelComm.Dispose();
                            GdDelConn.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        GdDelComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                        GdDelComm.CommandText = @"INSERT INTO B_MultiUser([UserName]) VALUES(@UserName)";
                        GdDelComm.ExecuteNonQuery();
                        GdDelComm.Parameters.Clear();
                    }
                    #endregion
                    #region //Update 'GongDan Used Qty' in C_BOM table
                    GdDelComm.CommandType = CommandType.StoredProcedure;
                    GdDelComm.CommandText = @"usp_UpdateGongDanUsedQty";
                    GdDelComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    GdDelComm.Parameters.AddWithValue("@BatchNo", strBatchNo);
                    GdDelComm.Parameters.AddWithValue("@Judge", "DEL");
                    GdDelComm.ExecuteNonQuery();
                    GdDelComm.Parameters.Clear();
                    #endregion
                    #region //Delete selected data in C_GongDan, C_GongDanDetail, E_GongDan and L_GongDan_Fulfillment four tables
                    GdDelComm.CommandText = @"usp_DeleteHistoryGongDan";
                    GdDelComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    GdDelComm.ExecuteNonQuery();
                    GdDelComm.Parameters.Clear();
                    #endregion
                    #region //Update 'Available RM Balance', 'GongDan Pending' in C_RMBalance table
                    GdDelComm.CommandText = @"usp_UpdateRMBalanceByGongDan";
                    GdDelComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    GdDelComm.Parameters.AddWithValue("@Action", "DEL");
                    GdDelComm.ExecuteNonQuery();
                    GdDelComm.Parameters.Clear();
                    #endregion
                    
                    GdDelComm.CommandType = CommandType.Text;
                    GdDelComm.Parameters.Clear();
                    GdDelComm.CommandText = @"DELETE FROM B_MultiUser";
                    GdDelComm.ExecuteNonQuery();
                    GdDelComm.Dispose();
                    if (GdDelConn.State == ConnectionState.Open) { GdDelConn.Close(); GdDelConn.Dispose(); }
                    this.dgvMasterGongDan.Rows.RemoveAt(iRow);
                    this.dgvDetailGongDan.Columns.Clear();
                }
            }
            if (e.ColumnIndex == 2) 
            {                
                SqlConnection GdDtlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (GdDtlConn.State == ConnectionState.Closed) { GdDtlConn.Open(); }
                SqlCommand GdDtlComm = new SqlCommand();
                GdDtlComm.Connection = GdDtlConn;
                GdDtlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.dgvMasterGongDan["GongDan No", this.dgvMasterGongDan.CurrentCell.RowIndex].Value.ToString().Trim();
                GdDtlComm.CommandText = @"SELECT * FROM C_GongDanDetail WHERE [GongDan No] = @GongDanNo";

                SqlDataAdapter GdDtlAdapter = new SqlDataAdapter();
                GdDtlAdapter.SelectCommand = GdDtlComm;
                DataTable dtGdDtl = new DataTable();
                GdDtlAdapter.Fill(dtGdDtl);
                GdDtlAdapter.Dispose();
                this.dgvDetailGongDan.DataSource = DBNull.Value;
                this.dgvDetailGongDan.DataSource = dtGdDtl;
                this.dgvDetailGongDan.Columns["Batch Path"].Visible = false;
                this.dgvDetailGongDan.Columns["GongDan No"].Visible = false;
                this.dgvDetailGongDan.Columns["Line No"].Frozen = true;
                GdDtlComm.Parameters.Clear();
                GdDtlComm.Dispose();
                if (GdDtlConn.State == ConnectionState.Open) { GdDtlConn.Close(); GdDtlConn.Dispose(); }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvMasterGongDan.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (this.dgvMasterGongDan.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select the data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection GdDlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (GdDlConn.State == ConnectionState.Closed) { GdDlConn.Open(); }
            SqlCommand GdDlComm = new SqlCommand();
            GdDlComm.Connection = GdDlConn;
            
            DataTable dtGdDl = new DataTable();
            Microsoft.Office.Interop.Excel.Application GdDl_excel = new Microsoft.Office.Interop.Excel.Application();
            GdDl_excel.Application.Workbooks.Add(true);
            if (this.dgvMasterGongDan.RowCount > 0)
            {
                int iActualRow = 0;
                for (int x = 0; x < this.dgvMasterGongDan.RowCount; x++)
                {
                    //if (String.Compare(this.dgvMasterGongDan[0, x].EditedFormattedValue.ToString(), "True") == 0)
                    if (Convert.ToBoolean(this.dgvMasterGongDan[0, x].Value))
                    {
                        GdDlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.dgvMasterGongDan["GongDan No", x].Value.ToString().Trim();
                        GdDlComm.CommandText = "SELECT T1.[Actual Start Date], T1.[Actual End Date], T2.[Batch Path], T1.[Batch No], T1.[BOM In Customs], T1.[GongDan No], " +
                                               "T1.[FG No], T1.[FG Description], T2.[Line No], T2.[Item No], T2.[Item Description], T2.[Lot No], T2.[Inventory Type], " +
                                               "T2.[RM Category], T2.[RM EHB], T2.[BGD No], T1.[IE Type], T1.[Order No], T1.[GongDan Qty], T1.[Order Price], " +
                                               "T1.[Order Currency], T1.[Total RM Qty], T1.[Total RM Cost(USD)], T2.[RM Used Qty], T2.[RM Currency], T2.[RM Price], " +
                                               "T2.[Consumption], T2.[Drools Quota], T1.[Drools Rate], T1.[CHN Name], T2.[Drools EHB], T1.[Destination], T1.[Creater], " +
                                               "T1.[Created Date], T1.[Approval Date], T1.[BeiAnDan Used Qty] FROM C_GongDan AS T1 LEFT OUTER JOIN C_GongDanDetail AS T2 " + 
                                               "ON T1.[GongDan No] = T2.[GongDan No] WHERE T1.[GongDan No] = @GongDanNo";
                        SqlDataAdapter GdDlAdapter = new SqlDataAdapter();
                        GdDlAdapter.SelectCommand = GdDlComm;
                        dtGdDl.Clear();
                        GdDlAdapter.Fill(dtGdDl);
                        GdDlAdapter.Dispose();

                        for (int y = 0; y < dtGdDl.Rows.Count; y++)
                        {
                            iActualRow++;
                            //GdDl_excel.get_Range(GdDl_excel.Cells[iActualRow + 1, 1], GdDl_excel.Cells[iActualRow + 1, dtGdDl.Columns.Count]).NumberFormatLocal = "@"; 
                            for (int z = 0; z < dtGdDl.Columns.Count; z++) { GdDl_excel.Cells[iActualRow + 1, z + 1] = "'" + dtGdDl.Rows[y][z].ToString().Trim(); }
                        }
                        GdDlComm.Parameters.Clear();
                    }
                }
                //GdDl_excel.get_Range(GdDl_excel.Cells[1, 1], GdDl_excel.Cells[1, dtGdDl.Columns.Count]).NumberFormatLocal = "@";
                for (int k = 0; k < dtGdDl.Columns.Count; k++) { GdDl_excel.Cells[1, k + 1] = dtGdDl.Columns[k].ColumnName.Trim(); }

                //GdDl_excel.get_Range(GdDl_excel.Cells[1, 1], GdDl_excel.Cells[1, dtGdDl.Columns.Count]).Font.Bold = true;
                //GdDl_excel.get_Range(GdDl_excel.Cells[1, 1], GdDl_excel.Cells[1, dtGdDl.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                //GdDl_excel.get_Range(GdDl_excel.Cells[1, 1], GdDl_excel.Cells[iActualRow + 1, dtGdDl.Columns.Count]).Font.Name = "Verdana";
                //GdDl_excel.get_Range(GdDl_excel.Cells[1, 1], GdDl_excel.Cells[iActualRow + 1, dtGdDl.Columns.Count]).Font.Size = 9;
                GdDl_excel.Cells.EntireColumn.AutoFit();
                GdDl_excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                GdDl_excel.Visible = true;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(GdDl_excel);
            GdDl_excel = null;
            dtGdDl.Dispose();
            GdDlComm.Dispose();
            if (GdDlConn.State == ConnectionState.Open) { GdDlConn.Close(); GdDlConn.Dispose(); }
        }

       
    }
}
