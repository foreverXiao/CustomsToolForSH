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
    public partial class TotalGongDanDataForm : Form
    {      
        private LoginForm loginFrm = new LoginForm();
        private DataTable dataTable = new DataTable();
        private DataGridView dgvDetails = new DataGridView();
        SqlLib sqlLib = new SqlLib();

        private static TotalGongDanDataForm totalGongDataFrm;
        public TotalGongDanDataForm()
        {
            InitializeComponent();
        }
        public static TotalGongDanDataForm CreateInstance()
        {
            if (totalGongDataFrm == null || totalGongDataFrm.IsDisposed)
            {
                totalGongDataFrm = new TotalGongDanDataForm();
            }
            return totalGongDataFrm;
        }

        private void TotalGongDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";          
        }

        private void TotalGongDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void cmbGongDanNo_Enter(object sender, EventArgs e)
        {
            if (dataTable.Rows.Count == 0)
            {
                dataTable = sqlLib.GetDataTable(@"SELECT DISTINCT [GongDan No] FROM C_GongDan", "C_GongDan").Copy();
                DataRow GongDanRow = dataTable.NewRow();
                GongDanRow["GongDan No"] = String.Empty;
                dataTable.Rows.InsertAt(GongDanRow, 0);
                this.cmbGongDanNo.DisplayMember = this.cmbGongDanNo.ValueMember = "GongDan No";
                this.cmbGongDanNo.DataSource = dataTable;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string strGo = @"SELECT [Approved Date], [Batch No], [GongDan No], [FG No], [ESS/LINE], [Order No], [IE Type], [Order Category], [Destination], " + 
                            "[Total Ship Qty], [GongDan Qty], [Order Price], [Order Currency], [Total RM Qty], [Total RM Cost(USD)], [Drools Rate], [CHN Name], " + 
                            "[Created Date], [Creater], [Actual Start Date], [Actual Close Date], [BeiAnDan Used Qty], [BeiAnDan No], [BOM In Customs], " + 
                            "[Remark Date], [Remark], [PC Item] FROM C_GongDan WHERE";

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

            if (!String.IsNullOrEmpty(this.cmbGongDanNo.Text.ToString().Trim()))
            { strGo += " [GongDan No] = '" + this.cmbGongDanNo.Text.ToString().Trim().ToUpper() + "' AND"; }

            if (String.Compare(strGo.Substring(strGo.Trim().Length - 4, 4), " AND") == 0)
            { strGo = strGo.Remove(strGo.Length - 4); }
            if (String.Compare(strGo.Substring(strGo.Trim().Length - 6, 6), " WHERE") == 0)
            { strGo = strGo.Remove(strGo.Length - 6); }
            strGo += " ORDER BY [Approved Date] DESC, [GongDan No]";

            SqlConnection masterConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (masterConn.State == ConnectionState.Closed) { masterConn.Open(); }

            SqlDataAdapter masterAdapter = new SqlDataAdapter(strGo, masterConn);
            DataTable masterTable = new DataTable();
            masterTable.Clear();
            masterAdapter.Fill(masterTable);
            masterAdapter.Dispose();

            if (masterTable.Rows.Count == 0)
            {
                masterTable.Dispose();
                this.dgvMasterGongDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvMasterGongDan.DataSource = masterTable;
                this.dgvMasterGongDan.Columns[0].HeaderText = "全选"; 
                for (int i = 4; i < this.dgvMasterGongDan.ColumnCount - 1; i++)
                { this.dgvMasterGongDan.Columns[i].ReadOnly = true; }
                this.dgvMasterGongDan.Columns["IE Type"].ReadOnly = false;
                this.dgvMasterGongDan.Columns["GongDan No"].Frozen = true;
                this.dgvMasterGongDan.Columns["PC Item"].Visible = false;

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvMasterGongDan.EnableHeadersVisualStyles = false;
                this.dgvMasterGongDan.Columns["IE Type"].HeaderCell.Style = cellStyle;
            }

            if (this.dgvDetailGongDan.RowCount > 0) { this.dgvDetailGongDan.Columns.Clear(); }
            if (masterConn.State == ConnectionState.Open)
            {
                masterConn.Close();
                masterConn.Dispose();
            }
        }

        private void dgvMasterGongDan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvMasterGongDan.RowCount; i++) { this.dgvMasterGongDan[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvMasterGongDan.RowCount; i++) { this.dgvMasterGongDan[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvMasterGongDan.RowCount; i++)
                    {
                        if (String.Compare(this.dgvMasterGongDan[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvMasterGongDan[0, i].Value = true; }
                        else { this.dgvMasterGongDan[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
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
                { this.dgvMasterGongDan.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvMasterGongDan.RowCount)
                { this.dgvMasterGongDan.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvMasterGongDan.Columns[0].HeaderText = "全选"; }
            }
        }

        private void dgvMasterGongDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1) //Handle 'Delete' function in datagridview button
            {             
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    string strBeiAnDanQty = this.dgvMasterGongDan["BeiAnDan Used Qty", this.dgvMasterGongDan.CurrentCell.RowIndex].Value.ToString().Trim();
                    if (!String.IsNullOrEmpty(strBeiAnDanQty) && Convert.ToInt32(strBeiAnDanQty) > 0)
                    {
                        MessageBox.Show("The GongDan has generated BeiAnDan, reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    int iRow = this.dgvMasterGongDan.CurrentCell.RowIndex;
                    string strGongDanNo = this.dgvMasterGongDan["GongDan No", iRow].Value.ToString().Trim();
                    string strBatchNo = this.dgvMasterGongDan["Batch No", iRow].Value.ToString().Trim();
                    string strEssLine = this.dgvMasterGongDan["ESS/LINE", iRow].Value.ToString().Trim();

                    SqlConnection deleteConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (deleteConn.State == ConnectionState.Closed) { deleteConn.Open(); }
                    SqlCommand deleteComm = new SqlCommand();
                    deleteComm.Connection = deleteConn;

                    #region //Monitor And Control Multiple Users
                    deleteComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
                    string strUserName = Convert.ToString(deleteComm.ExecuteScalar());
                    if (!String.IsNullOrEmpty(strUserName))
                    {
                        if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                        {
                            MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            deleteComm.Dispose();
                            deleteConn.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        deleteComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                        deleteComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                        deleteComm.ExecuteNonQuery();
                        deleteComm.Parameters.RemoveAt("@UserName");
                    }
                    #endregion

                    #region //Update 'GongDan Used Qty' in C_BOM table
                    deleteComm.CommandType = CommandType.StoredProcedure;
                    deleteComm.CommandText = @"usp_UpdateGongDanUsedQty";
                    deleteComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    deleteComm.Parameters.AddWithValue("@BatchNo", strBatchNo);
                    deleteComm.Parameters.AddWithValue("@Judge", "DEL");
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.Clear();
                    #endregion

                    #region //Update 'Available RM Balance', 'GongDan Pending' in C_RMBalance table
                    deleteComm.CommandText = @"usp_UpdateRMBalanceByGongDan";
                    deleteComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    deleteComm.Parameters.AddWithValue("@Action", "DEL");
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.Clear();
                    #endregion

                    #region //Delete selected data in C_GongDan, C_GongDanDetail, E_GongDan and B_OrderFulfillment tables
                    deleteComm.CommandText = @"usp_DeleteDataForHistoricalGongDan";
                    deleteComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    deleteComm.Parameters.AddWithValue("@BatchNo", strBatchNo);
                    deleteComm.Parameters.AddWithValue("@EssLine", strEssLine);
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.Clear();
                    #endregion

                    deleteComm.CommandType = CommandType.Text;
                    deleteComm.Parameters.Clear();
                    deleteComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Dispose();
                    if (deleteConn.State == ConnectionState.Open)
                    {
                        deleteConn.Close();
                        deleteConn.Dispose();
                    }

                    this.dgvMasterGongDan.Rows.RemoveAt(this.dgvMasterGongDan.CurrentRow.Index);
                    this.dgvDetailGongDan.Columns.Clear();
                }
            }

            if (e.ColumnIndex == 2) //Handle 'Update' function to edit 'IE Type' column
            {
                if (MessageBox.Show("Are you sure to update 'IE Type'?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    int iBeiAnDanUsedQty = Convert.ToInt32(this.dgvMasterGongDan["BeiAnDan Used Qty", this.dgvMasterGongDan.CurrentCell.RowIndex].Value.ToString().Trim());
                    if (iBeiAnDanUsedQty > 0)
                    {
                        MessageBox.Show("The GongDan has generated BeiAnDan, reject to update.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    int iIndex = this.dgvMasterGongDan.CurrentRow.Index;
                    string strGongDanNo = this.dgvMasterGongDan["GongDan No", iIndex].Value.ToString().Trim();
                    string strFGNo = this.dgvMasterGongDan["FG No", iIndex].Value.ToString().Trim();
                    string strIEType = this.dgvMasterGongDan["IE Type", iIndex].Value.ToString().Trim().ToUpper();
                    string strRemark = this.dgvMasterGongDan["Remark", iIndex].Value.ToString().Trim().ToUpper();

                    SqlConnection updateConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (updateConn.State == ConnectionState.Closed) { updateConn.Open(); }
                    SqlCommand updateComm = new SqlCommand();
                    updateComm.Connection = updateConn;
                    updateComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = strIEType;
                    updateComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                    updateComm.Parameters.Add("@RemarkDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                    updateComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                    updateComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;

                    updateComm.CommandText = @"UPDATE C_GongDan SET [IE Type] = @IEType, [Remark] = @Remark, [Remark Date] = @RemarkDate WHERE [GongDan No] = @GongDanNo AND [FG No] = @FGNo";
                    SqlTransaction updateTrans = updateConn.BeginTransaction();
                    updateComm.Transaction = updateTrans;
                    try
                    {
                        updateComm.ExecuteNonQuery();
                        updateTrans.Commit();
                    }
                    catch (Exception)
                    {
                        updateTrans.Rollback();
                        updateTrans.Dispose();
                        throw;
                    }
                    finally 
                    { 
                        updateComm.Parameters.Clear();
                        updateComm.Dispose();
                        if (updateConn.State == ConnectionState.Open)
                        {
                            updateConn.Close();
                            updateConn.Dispose();
                        }
                    }
                    MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (e.ColumnIndex == 3) //Handle 'View Detail' function in datagridview textlink
            {                
                SqlConnection detailConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (detailConn.State == ConnectionState.Closed) { detailConn.Open(); }
                SqlCommand detailComm = new SqlCommand();
                detailComm.Connection = detailConn;

                detailComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.dgvMasterGongDan["GongDan No", this.dgvMasterGongDan.CurrentCell.RowIndex].Value.ToString().Trim();
                detailComm.CommandText = @"SELECT * FROM C_GongDanDetail WHERE [GongDan No] = @GongDanNo";

                SqlDataAdapter detailAdapter = new SqlDataAdapter();
                detailAdapter.SelectCommand = detailComm;
                DataTable detailTable = new DataTable();
                detailTable.Clear();
                detailAdapter.Fill(detailTable);
                detailAdapter.Dispose();

                this.dgvDetailGongDan.DataSource = detailTable;
                this.dgvDetailGongDan.Columns["Batch Path"].Visible = false;
                this.dgvDetailGongDan.Columns["GongDan No"].Visible = false;
                this.dgvDetailGongDan.Columns["Line No"].Frozen = true;

                detailComm.Parameters.Clear();
                detailComm.Dispose();
                if (detailConn.State == ConnectionState.Open)
                {
                    detailConn.Close();
                    detailConn.Dispose();
                }
            }

            if (e.ColumnIndex == 10) //IE Type comboBox value
            {
                int iIEType = this.dgvMasterGongDan.Columns["IE Type"].Index;
                if (this.dgvMasterGongDan.CurrentCell.ColumnIndex == iIEType)
                {
                    FunctionDGV_IETYPE();
                    dgvDetails.Width = 119;
                    dgvDetails.Height = 158;

                    Rectangle rec = this.dgvMasterGongDan.GetCellDisplayRectangle(9, this.dgvMasterGongDan.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvMasterGongDan.Columns[9].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvMasterGongDan.Location.Y > this.dgvMasterGongDan.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else
                    { dgvDetails.Top = rec.Top + this.dgvMasterGongDan.Location.Y; }

                    if (dgvDetails.RowCount > 0) { dgvDetails.Visible = true; }
                }
            }

            if (e.ColumnIndex != 10) { dgvDetails.Visible = false; }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvMasterGongDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (this.dgvMasterGongDan.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select the data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvMasterGongDan.Focus();
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

            if (this.dgvMasterGongDan.RowCount > 0)
            {
                int iActualRow = 0;
                for (int x = 0; x < this.dgvMasterGongDan.RowCount; x++)
                {
                    if (String.Compare(this.dgvMasterGongDan[0, x].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        excelComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.dgvMasterGongDan["GongDan No", x].Value.ToString().Trim();
                        excelComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = this.dgvMasterGongDan["FG No", x].Value.ToString().Trim();

                        excelComm.CommandText = @"SELECT T1.[Actual Start Date], T1.[Actual Close Date], T2.[Batch Path], T1.[Batch No], T1.[GongDan No], T1.[FG No], " + 
                                                 "T1.[BOM In Customs], T2.[Line No], T2.[Item No], T2.[Lot No], T2.[Inventory Type], T2.[RM Category], T1.[ESS/LINE], " +
                                                 "T1.[Order No], T1.[IE Type], T1.[Order Category], T1.[Destination], T1.[Total Ship Qty], T1.[GongDan Qty], " +
                                                 "T1.[Order Price], T1.[Order Currency], T1.[Total RM Qty], T2.[RM Price], T2.[RM Currency], " +
                                                 "CAST(T2.[RM Used Qty] AS decimal(18, 6)) AS [RM Used Qty], T1.[Total RM Cost(USD)], T2.[Consumption], T1.[Drools Rate], " +
                                                 "T2.[Drools Quota], T2.[Drools EHB], T2.[RM Customs Code], T2.[BGD No], T1.[CHN Name], T1.[BeiAnDan Used Qty], " + 
                                                 "T1.[BeiAnDan No], T1.[Created Date], T1.[Creater], T1.[Approved Date], T1.[Remark], T1.[Remark Date], T1.[PC Item] " + 
                                                 "FROM C_GongDan AS T1 LEFT OUTER JOIN C_GongDanDetail AS T2 ON T1.[GongDan No] = T2.[GongDan No] " + 
                                                 "WHERE T1.[GongDan No] = @GongDanNo AND T1.[FG No] = @FGNo";

                        excelAdapter.SelectCommand = excelComm;
                        excelTable.Clear();
                        excelAdapter.Fill(excelTable);
                        excelAdapter.Dispose();

                        for (int y = 0; y < excelTable.Rows.Count; y++)
                        {
                            iActualRow++;
                            excel.get_Range(excel.Cells[iActualRow + 1, 1], excel.Cells[iActualRow + 1, excelTable.Columns.Count]).NumberFormatLocal = "@"; 
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

        private void FunctionDGV_IETYPE()
        {
            DataTable ietypeTable = sqlLib.GetDataTable(@"SELECT * FROM B_IEType ORDER BY [IE Type]", "B_IEType").Copy();
            dgvDetails.DataSource = ietypeTable;
            this.dgvMasterGongDan.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);
        }

        private void DGV_Details_CellClick(object sender, EventArgs e)
        {
            int iIEType = this.dgvMasterGongDan.Columns["IE Type"].Index;

            if (this.dgvMasterGongDan.CurrentCell != null && this.dgvMasterGongDan.CurrentCell.ColumnIndex == iIEType)
            {
                string strIEType = dgvDetails["IE Type", dgvDetails.CurrentCell.RowIndex].Value.ToString().Trim();
                this.dgvMasterGongDan[iIEType, this.dgvMasterGongDan.CurrentCell.RowIndex].Value = strIEType;
            }
            dgvDetails.Visible = false;
        }
    }
}
