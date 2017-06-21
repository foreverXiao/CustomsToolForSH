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
    public partial class TotalBomDataForm : Form
    {        
        private LoginForm loginFrm = new LoginForm();
        private DataTable dataTable = new DataTable();
        SqlLib sqlLib = new SqlLib();

        private static TotalBomDataForm totalBomDataFrm;
        public TotalBomDataForm()
        {
            InitializeComponent();
        }
        public static TotalBomDataForm CreateInstance()
        {
            if (totalBomDataFrm == null || totalBomDataFrm.IsDisposed)
            {
                totalBomDataFrm = new TotalBomDataForm();
            }
            return totalBomDataFrm;
        }

        private void TotalBomDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";          
        }

        private void TotalBomDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataTable.Dispose();
            sqlLib.Dispose(0);
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

        private void cmbBatchNo_Enter(object sender, EventArgs e)
        {
            if (dataTable.Rows.Count == 0)
            {
                dataTable = sqlLib.GetDataTable(@"SELECT DISTINCT [Batch No] FROM C_BOM", "C_BOM").Copy();
                DataRow BatchRow = dataTable.NewRow();
                BatchRow["Batch No"] = String.Empty;
                dataTable.Rows.InsertAt(BatchRow, 0);
                this.cmbBatchNo.DisplayMember = this.cmbBatchNo.ValueMember = "Batch No";
                this.cmbBatchNo.DataSource = dataTable;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string strGo = @"SELECT [Freeze], [Approved Date], [Batch No], [FG No], [FG Qty], [Order Price], [Order Currency], [Total Input Qty], " +
                            "[Total RM Cost(USD)], [HS Code], [CHN Name], [Qty Loss Rate], [Drools Qty], [Created Date], [Creater], [Actual Start Date], " + 
                            "[Actual Close Date], [GongDan Used Qty], [BOM In Customs], [Remark Date], [Remark] FROM C_BOM WHERE";

            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1)
                    { 
                        MessageBox.Show("Begin date is not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom.Focus();
                        return;
                    }
                    else
                    { strGo += " [Approved Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [Approved Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else
                { strGo += " [Approved Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strGo += " [Approved Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND"; }
            }

            if (!String.IsNullOrEmpty(this.cmbBatchNo.Text.ToString().Trim()))
            { strGo += " [Batch No] = '" + this.cmbBatchNo.Text.ToString().Trim().ToUpper() + "' AND"; }

            if (String.Compare(strGo.Substring(strGo.Trim().Length - 4, 4), " AND") == 0)
            { strGo = strGo.Remove(strGo.Trim().Length - 4); }
            if (String.Compare(strGo.Substring(strGo.Trim().Length - 6, 6), " WHERE") == 0)
            { strGo = strGo.Remove(strGo.Trim().Length - 6); }
            strGo += " ORDER BY [Approved Date] DESC, [Batch No], [FG No] ASC";

            SqlConnection masterConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (masterConn.State == ConnectionState.Closed) { masterConn.Open(); }

            SqlDataAdapter masterAdapter = new SqlDataAdapter(strGo, masterConn);
            DataTable masterTable = new DataTable();
            masterTable.Clear();
            masterAdapter.Fill(masterTable);
            masterAdapter.Dispose();

            if (masterTable.Rows.Count == 0)
            {
                masterTable.Clear();
                masterTable.Dispose();
                this.dgvMasterBOM.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            { 
                this.dgvMasterBOM.DataSource = masterTable;
                this.dgvMasterBOM.Columns[0].HeaderText = "全选"; 
                for (int i = 3; i < this.dgvMasterBOM.ColumnCount; i++)
                { this.dgvMasterBOM.Columns[i].ReadOnly = true; }
                this.dgvMasterBOM.Columns["FG No"].Frozen = true;
            }

            if (this.dgvDetailBOM.RowCount > 0) { this.dgvDetailBOM.Columns.Clear(); }
            if (masterConn.State == ConnectionState.Open)
            {
                masterConn.Close();
                masterConn.Dispose();
            }
        }

        private void dgvMasterBOM_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvMasterBOM.RowCount; i++) { this.dgvMasterBOM[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvMasterBOM.RowCount; i++) { this.dgvMasterBOM[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvMasterBOM.RowCount; i++)
                    {
                        if (String.Compare(this.dgvMasterBOM[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvMasterBOM[0, i].Value = true; }
                        else { this.dgvMasterBOM[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
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
                { this.dgvMasterBOM.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvMasterBOM.RowCount)
                { this.dgvMasterBOM.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvMasterBOM.Columns[0].HeaderText = "全选"; }
            }
        }

        private void dgvMasterBOM_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1) 
            {
                //Handle 'Delete' function in datagridview button
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    string strFreeze = this.dgvMasterBOM["Freeze", this.dgvMasterBOM.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                    if (String.Compare(strFreeze, "TRUE") == 0)
                    {
                        MessageBox.Show("The BOM has froze, reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    string strGongDanUsedQty = this.dgvMasterBOM["GongDan Used Qty", this.dgvMasterBOM.CurrentCell.RowIndex].Value.ToString().Trim();
                    if (!String.IsNullOrEmpty(strGongDanUsedQty) && Convert.ToInt32(strGongDanUsedQty) > 0)
                    {
                        MessageBox.Show("The BOM has generated GongDan, reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    string strBatchNo = this.dgvMasterBOM["Batch No", this.dgvMasterBOM.CurrentCell.RowIndex].Value.ToString().Trim();
                    string strFGNo = this.dgvMasterBOM["FG No", this.dgvMasterBOM.CurrentCell.RowIndex].Value.ToString().Trim();

                    SqlConnection deleteConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (deleteConn.State == ConnectionState.Closed) { deleteConn.Open(); }
                    SqlCommand deleteComm = new SqlCommand();
                    deleteComm.Connection = deleteConn;

                    #region //Delete selected data in C_BOM, C_BOMDetail, E_Consumption and E_OriginalGoods tables
                    deleteComm.CommandType = CommandType.StoredProcedure;
                    deleteComm.CommandText = @"usp_DeleteDataForHistoricalBOM";
                    deleteComm.Parameters.AddWithValue("@BatchNo", strBatchNo);
                    deleteComm.Parameters.AddWithValue("@FGEHB", strFGNo + '/' + strBatchNo);
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.Clear();
                    #endregion

                    deleteComm.Dispose();
                    if (deleteConn.State == ConnectionState.Open)
                    {
                        deleteConn.Close();
                        deleteConn.Dispose();
                    }

                    this.dgvMasterBOM.Rows.RemoveAt(this.dgvMasterBOM.CurrentRow.Index);
                    this.dgvDetailBOM.Columns.Clear();
                }
            }

            if (e.ColumnIndex == 2) 
            {
                //Handle 'View BOM' function in datagridview textlink
                SqlConnection detailConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (detailConn.State == ConnectionState.Closed) { detailConn.Open(); }
                SqlCommand detailComm = new SqlCommand();
                detailComm.Connection = detailConn;

                detailComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvMasterBOM["Batch No", this.dgvMasterBOM.CurrentCell.RowIndex].Value.ToString().Trim();
                detailComm.CommandText = @"SELECT * FROM C_BOMDetail WHERE [Batch No] = @BatchNo";

                SqlDataAdapter detailAdapter = new SqlDataAdapter();
                detailAdapter.SelectCommand = detailComm;
                DataTable detailTable = new DataTable();
                detailTable.Clear();
                detailAdapter.Fill(detailTable);
                detailAdapter.Dispose();

                this.dgvDetailBOM.DataSource = detailTable;
                this.dgvDetailBOM.Columns["Batch Path"].Visible = false;
                this.dgvDetailBOM.Columns["Batch No"].Visible = false;

                detailComm.Parameters.Clear();
                detailComm.Dispose();
                if (detailConn.State == ConnectionState.Open)
                {
                    detailConn.Close();
                    detailConn.Dispose();
                }
            }

            if (e.ColumnIndex == 3) 
            {
                if (MessageBox.Show("Are you sure to freeze/unfreeze the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) { return; }

                //Handle 'Freeze' function in datagridview checkbox
                SqlConnection freezeConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (freezeConn.State == ConnectionState.Closed) { freezeConn.Open(); }
                SqlCommand freezeComm = new SqlCommand();
                freezeComm.Connection = freezeConn;

                string strBatch = null, strFG = null, strRemark = null, strFreeze = null;
                DateTime dTime = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                strBatch = this.dgvMasterBOM["Batch No", this.dgvMasterBOM.CurrentRow.Index].Value.ToString().Trim();
                strFG = this.dgvMasterBOM["FG No", this.dgvMasterBOM.CurrentRow.Index].Value.ToString().Trim();

                freezeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatch;
                freezeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFG;
                freezeComm.CommandText = @"SELECT [Freeze] FROM C_BOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                string strBool = Convert.ToString(freezeComm.ExecuteScalar());
                freezeComm.CommandText = @"SELECT [Remark] FROM C_BOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                strRemark = Convert.ToString(freezeComm.ExecuteScalar());
                if (String.IsNullOrEmpty(strBool) || String.Compare(strBool, "False") == 0)
                {
                    strRemark += "/freeze by " + loginFrm.PublicUserName + dTime.ToString();
                    strFreeze = "True"; 
                }
                else
                {
                    strRemark += "/unfreeze by " + loginFrm.PublicUserName + dTime.ToString();
                    strFreeze = "False"; 
                }

                freezeComm.Parameters.Clear();
                freezeComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                freezeComm.Parameters.Add("@RemarkDate", SqlDbType.DateTime).Value = dTime;
                freezeComm.Parameters.Add("@Freeze", SqlDbType.Bit).Value = Convert.ToBoolean(strFreeze);
                freezeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatch;
                freezeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFG;
                freezeComm.CommandText = @"UPDATE C_BOM SET [Remark] = @Remark, [Remark Date] = @RemarkDate, [Freeze] = @Freeze WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";

                SqlTransaction freezeTrans = freezeConn.BeginTransaction();
                freezeComm.Transaction = freezeTrans;
                try
                {
                    freezeComm.ExecuteNonQuery();
                    freezeTrans.Commit();
          
                    this.dgvMasterBOM["Remark", this.dgvMasterBOM.CurrentRow.Index].Value = strRemark;
                    this.dgvMasterBOM["Remark Date", this.dgvMasterBOM.CurrentRow.Index].Value = dTime.Date;
                    this.dgvMasterBOM["Freeze", this.dgvMasterBOM.CurrentRow.Index].Value = Convert.ToBoolean(strFreeze);
                    this.dgvMasterBOM.CurrentRow.Selected = true;
                }
                catch (Exception)
                {
                    freezeTrans.Rollback();
                    freezeTrans.Dispose();
                    throw;
                }
                finally
                {
                    freezeComm.Parameters.Clear();
                    freezeComm.Dispose();
                    if (freezeConn.State == ConnectionState.Open)
                    {
                        freezeConn.Close();
                        freezeConn.Dispose();
                    }
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvMasterBOM.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (this.dgvMasterBOM.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select the data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvMasterBOM.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection excelConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (excelConn.State == ConnectionState.Closed) { excelConn.Open(); }

            SqlCommand excelComm = new SqlCommand();
            excelComm.Connection = excelConn;

            SqlDataAdapter excelAdapter = new SqlDataAdapter();
            DataTable excelTable = new DataTable();

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            if (this.dgvMasterBOM.RowCount > 0)
            {
                int iActualRow = 0;
                for (int x = 0; x < this.dgvMasterBOM.RowCount; x++)
                {
                    if (String.Compare(this.dgvMasterBOM[0, x].EditedFormattedValue.ToString(), "True") == 0) 
                    {
                        excelComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvMasterBOM["Batch No", x].Value.ToString().Trim();
                        excelComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = this.dgvMasterBOM["FG No", x].Value.ToString().Trim();

                        excelComm.CommandText = @"SELECT T1.[Actual Start Date], T1.[Actual Close Date], T2.[Batch Path], T1.[Batch No], T1.[FG No], T1.[BOM In Customs], " +  
                                                 "T2.[Line No], T2.[Item No], T2.[Lot No], T2.[Inventory Type], T2.[RM Category], T1.[FG Qty], T1.[Order Price], " +
                                                 "T1.[Order Currency], T1.[Total Input Qty], T2.[RM Price], T2.[RM Currency], T2.[RM Qty], T1.[Total RM Cost(USD)], " +
                                                 "T2.[RM Customs Code], T2.[BGD No], T2.[Consumption], T1.[Qty Loss Rate], T1.[Drools Qty], T2.[Drools EHB], T1.[HS Code], " +
                                                 "T1.[CHN Name], T1.[GongDan Used Qty], T2.[Note], T1.[Created Date], T1.[Creater], T1.[Approved Date], T1.[Freeze], " +
                                                 "T1.[Remark], T1.[Remark Date] FROM C_BOM AS T1 LEFT OUTER JOIN C_BOMDetail AS T2 ON T1.[Batch No] = T2.[Batch No] " +
                                                 "WHERE T1.[Batch No] = @BatchNo AND T1.[FG No] = @FGNo";

                        excelAdapter.SelectCommand = excelComm;
                        excelTable.Clear();
                        excelAdapter.Fill(excelTable);
                        excelAdapter.Dispose();

                        for (int y = 0; y < excelTable.Rows.Count; y++)
                        {
                            iActualRow++;
                            excel.get_Range(excel.Cells[iActualRow + 1, 1], excel.Cells[iActualRow + 1, excelTable.Columns.Count]).NumberFormatLocal = "@"; //set excel cells format as text
                            for (int z = 0; z < excelTable.Columns.Count; z++)
                            { excel.Cells[iActualRow + 1, z + 1] = excelTable.Rows[y][z].ToString().Trim(); }
                        }
                        excelComm.Parameters.Clear();
                    }
                }

                if (excelTable.Rows.Count == 0) { return; }
                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, excelTable.Columns.Count]).NumberFormatLocal = "@";
                for (int k = 0; k < excelTable.Columns.Count; k++)
                { excel.Cells[1, k + 1] = excelTable.Columns[k].ColumnName.Trim(); }

                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, excelTable.Columns.Count]).Font.Bold = true;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, excelTable.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, excelTable.Columns.Count]).Font.Name = "Verdana";
                excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, excelTable.Columns.Count]).Font.Size = 9;
                excel.Cells.EntireColumn.AutoFit();
                excel.Visible = true;
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;

            excelTable.Dispose();
            excelComm.Dispose();
            if (excelConn.State == ConnectionState.Open)
            {
                excelConn.Close();
                excelConn.Dispose();
            }
        }
    }
}