using System;
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

namespace SHCustomsSystem
{
    public partial class RMBalanceAdjustmentForm : Form
    {
        protected DataView dvFillDGV = new DataView();
        protected DataTable dtFillDGV = new DataTable();
        string strFilter = null;
        private DataTable dataTable1 = new DataTable();
        private DataTable dataTable2 = new DataTable();
        SqlLib sqlLib = new SqlLib();

        private LoginForm loginFrm = new LoginForm();  
        private static RMBalanceAdjustmentForm RMBalanceAdjustmentFrm;
        public RMBalanceAdjustmentForm()
        {
            InitializeComponent();
        }
        public static RMBalanceAdjustmentForm CreateInstance()
        {
            if (RMBalanceAdjustmentFrm == null || RMBalanceAdjustmentFrm.IsDisposed)
            {
                RMBalanceAdjustmentFrm = new RMBalanceAdjustmentForm();
            }
            return RMBalanceAdjustmentFrm;
        }

        private void RMBalanceAdjustmentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.dtFillDGV.Dispose();
            this.dataTable1.Dispose();
            this.dataTable2.Dispose();
            sqlLib.Dispose(0);
        }

        private void GetcmbBGDNoData()
        {
            this.dataTable2 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMBalance", "C_RMBalance").Copy();
            DataRow BGDNo_Dr = this.dataTable2.NewRow();
            BGDNo_Dr["BGD No"] = String.Empty;
            this.dataTable2.Rows.InsertAt(BGDNo_Dr, 0);
            this.cmbBGDNo.DisplayMember = this.cmbBGDNo.ValueMember = "BGD No";
            this.cmbBGDNo.DataSource = this.dataTable2;
        }

        private void cmbRMCustomsCode_Enter(object sender, EventArgs e)
        {
            if (this.dataTable1.Rows.Count == 0)
            {
                this.dataTable1 = sqlLib.GetDataTable(@"SELECT DISTINCT [RM Customs Code] FROM C_RMBalance", "C_RMBalance").Copy();
                DataRow RMCustomsCode_Dr = this.dataTable1.NewRow();
                RMCustomsCode_Dr["RM Customs Code"] = String.Empty;
                this.dataTable1.Rows.InsertAt(RMCustomsCode_Dr, 0);
                this.cmbRMCustomsCode.DisplayMember = this.cmbRMCustomsCode.ValueMember = "RM Customs Code";
                this.cmbRMCustomsCode.DataSource = this.dataTable1;
            }
        }

        private void cmbRMCustomsCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbRMCustomsCode.SelectedIndex > 0)
            {
                this.dataTable2.Reset();
                this.dataTable2 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMBalance WHERE [RM Customs Code] = '" + this.cmbRMCustomsCode.SelectedValue.ToString().Trim() + "'", "C_RMBalance").Copy();
                DataRow BGDNo_Dr = this.dataTable2.NewRow();
                BGDNo_Dr["BGD No"] = String.Empty;
                this.dataTable2.Rows.InsertAt(BGDNo_Dr, 0);
                this.cmbBGDNo.DisplayMember = this.cmbBGDNo.ValueMember = "BGD No";
                this.cmbBGDNo.DataSource = this.dataTable2;
                sqlLib.Dispose();
            }
            else { this.GetcmbBGDNoData(); }
        }

        private void cmbBGDNo_Enter(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbRMCustomsCode.Text.Trim()))
            { this.GetcmbBGDNoData(); }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            string strBrowse = @"SELECT * FROM C_RMBalance WHERE";
            if (!String.IsNullOrEmpty(this.cmbRMCustomsCode.Text.ToString().Trim()))
            { strBrowse += " [RM Customs Code] = '" + this.cmbRMCustomsCode.Text.ToString().Trim().ToUpper() + "' AND"; }
            if (!String.IsNullOrEmpty(this.cmbBGDNo.Text.ToString().Trim()))
            { strBrowse += " [BGD No] = '" + this.cmbBGDNo.Text.ToString().Trim().ToUpper() + "' AND"; }

            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 4, 4), " AND") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 4); }
            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 6, 6), " WHERE") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 6); }
            strBrowse += " ORDER BY [RM Customs Code], [BGD No]";

            SqlConnection RMBalConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (RMBalConn.State == ConnectionState.Closed) { RMBalConn.Open(); }

            SqlDataAdapter RMBalAdapter = new SqlDataAdapter(strBrowse, RMBalConn);
            dtFillDGV.Clear();
            RMBalAdapter.Fill(dtFillDGV);
            dvFillDGV = dtFillDGV.DefaultView;
            RMBalAdapter.Dispose();

            if (dtFillDGV.Rows.Count == 0)
            {
                dtFillDGV.Clear();
                dtFillDGV.Dispose();
                this.dgvRMBalance.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvRMBalance.DataSource = dvFillDGV;
                this.dgvRMBalance.Rows[0].HeaderCell.Value = 1;
            }

            if (RMBalConn.State == ConnectionState.Open)
            {
                RMBalConn.Close();
                RMBalConn.Dispose();
            }

            for (int i = 1; i < this.dgvRMBalance.ColumnCount; i++)
            { this.dgvRMBalance.Columns[i].ReadOnly = true; }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvRMBalance.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            if (this.dgvRMBalance.Columns[0].HeaderText != "全选")
            {
                Microsoft.Office.Interop.Excel.Application RMBalExcel = new Microsoft.Office.Interop.Excel.Application();
                RMBalExcel.Application.Workbooks.Add(true);

                bool bJudge = false;
                int iRow = 2;
                for (int i = 0; i < this.dgvRMBalance.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMBalance[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        bJudge = true;
                        RMBalExcel.get_Range(RMBalExcel.Cells[iRow, 1], RMBalExcel.Cells[iRow, this.dgvRMBalance.ColumnCount - 1]).NumberFormatLocal = "@";
                        for (int j = 1; j < this.dgvRMBalance.ColumnCount; j++)
                        { RMBalExcel.Cells[iRow, j] = this.dgvRMBalance[j, i].Value.ToString().Trim(); }
                        iRow++;
                    }
                }
                if (bJudge)
                {
                    RMBalExcel.get_Range(RMBalExcel.Cells[1, 1], RMBalExcel.Cells[1, this.dgvRMBalance.ColumnCount - 1]).NumberFormatLocal = "@";
                    for (int k = 1; k < this.dgvRMBalance.ColumnCount; k++)
                    { RMBalExcel.Cells[1, k] = this.dgvRMBalance.Columns[k].HeaderText.ToString(); }

                    RMBalExcel.get_Range(RMBalExcel.Cells[1, 1], RMBalExcel.Cells[1, this.dgvRMBalance.ColumnCount - 1]).Font.Bold = true;
                    RMBalExcel.get_Range(RMBalExcel.Cells[1, 1], RMBalExcel.Cells[1, this.dgvRMBalance.ColumnCount - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    RMBalExcel.get_Range(RMBalExcel.Cells[1, 1], RMBalExcel.Cells[iRow - 1, this.dgvRMBalance.ColumnCount - 1]).Font.Name = "Verdana";
                    RMBalExcel.get_Range(RMBalExcel.Cells[1, 1], RMBalExcel.Cells[iRow - 1, this.dgvRMBalance.ColumnCount - 1]).Font.Size = 9;
                    RMBalExcel.get_Range(RMBalExcel.Cells[1, 1], RMBalExcel.Cells[iRow - 1, this.dgvRMBalance.ColumnCount - 1]).Borders.LineStyle = 1;
                }

                RMBalExcel.Cells.EntireColumn.AutoFit();
                RMBalExcel.Visible = true;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(RMBalExcel);
                RMBalExcel = null;
            }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(dtFillDGV.Rows.Count / PageRow);
                if (iPageCount * PageRow < dtFillDGV.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\All RM Balance Data " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dtFillDGV.Columns.Count; n++)
                        { sb.Append(dtFillDGV.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < dtFillDGV.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < dtFillDGV.Columns.Count; j++)
                            {
                                if (j == 1) { sb.Append("'" + dtFillDGV.Rows[i][j].ToString().Trim() + "\t"); }
                                else { sb.Append(dtFillDGV.Rows[i][j].ToString().Trim() + "\t"); } 
                            }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    MessageBox.Show("Successfully generated all RM Balance data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
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
                MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnSearch.Focus();
                return;
            }

            SqlConnection oneConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (oneConn.State == ConnectionState.Closed) { oneConn.Open(); }
            SqlCommand oneComm = new SqlCommand();
            oneComm.Connection = oneConn;

            try
            {
                string strUserName = loginFrm.PublicUserName;
                oneComm.Parameters.Add("@LoginName", SqlDbType.NVarChar).Value = strUserName;
                oneComm.CommandText = @"SELECT [Group] FROM B_UserInfo WHERE [LoginName] = @LoginName";
                string strGroup = Convert.ToString(oneComm.ExecuteScalar()).Trim().ToUpper();
                if (String.Compare(strGroup, "RM") != 0) 
                {
                    if (String.Compare(strGroup, "ADMIN") != 0)
                    {
                        MessageBox.Show("You donot have access rights to adjust RM Balance data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                oneComm.Parameters.Clear();

                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge, oneConn, oneComm);
                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            catch (Exception)
            {
                MessageBox.Show("Upload error, please try again.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
            finally
            {
                oneComm.Dispose();
                if (oneConn.State == ConnectionState.Open)
                {
                    oneConn.Close();
                    oneConn.Dispose();
                }
            }
        }

        private void ImportExcelData(string strFilePath, bool bJudge, SqlConnection oneConn, SqlCommand oneComm)
        {
            string strConn;
            if (bJudge)
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else
            { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

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
                myAdapter.Dispose();
                myTable.Clear();
                myTable.Dispose();
                return;
            }

            /*------ Monitor And Control Multiple Users ------*/
            oneComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
            string strUserName = Convert.ToString(oneComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    oneComm.Dispose();
                    oneComm.Dispose();
                    return;
                }
            }
            else
            {
                oneComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                oneComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                oneComm.ExecuteNonQuery();
                oneComm.Parameters.RemoveAt("@UserName");
            }
          
            oneComm.CommandText = @"SELECT [RM Customs Code], [BGD No], [Customs Balance], [Available RM Balance] FROM C_RMBalance ORDER BY [RM Customs Code], [BGD No]";
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = oneComm;
            DataTable dTable = new DataTable(); 
            sqlAdapter.Fill(dTable);
            sqlAdapter.Dispose();

            for (int i = 0; i < myTable.Rows.Count; i++)
            {
                string strAdjAvailableQty = myTable.Rows[i]["ADJ Available Qty"].ToString().Trim();
                string strAdjCustomsQty = myTable.Rows[i]["ADJ Customs Qty"].ToString().Trim();
                decimal dAdjAvailableQty = 0.0M, dAdjCustomsQty = 0.0M;
                if (!String.IsNullOrEmpty(strAdjAvailableQty)) { dAdjAvailableQty = Math.Round(Convert.ToDecimal(strAdjAvailableQty), 6); }
                if (!String.IsNullOrEmpty(strAdjCustomsQty)) { dAdjCustomsQty = Math.Round(Convert.ToDecimal(strAdjCustomsQty), 6); }

                if (dAdjAvailableQty != 0.0M || dAdjCustomsQty != 0.0M)
                {
                    string strRMCustomsCode = myTable.Rows[i]["RM Customs Code"].ToString().Trim().ToUpper();
                    string strBGDNo = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                    DataRow[] dr = dTable.Select("[RM Customs Code] = '" + strRMCustomsCode + "' AND [BGD No] = '" + strBGDNo + "'");
                    if (dr.Length > 0)
                    {
                        string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dr[0]["Customs Balance"].ToString().Trim()));
                        string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dr[0]["Available RM Balance"].ToString().Trim()));
                        decimal dCustomsBalance = Convert.ToDecimal(strCustomsBal) + dAdjCustomsQty;
                        decimal dAvailableRMBalance = Convert.ToDecimal(strAvailRMBal) + dAdjAvailableQty;

                        oneComm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBalance;
                        oneComm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBalance;
                        oneComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        oneComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance " +
                                               "WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                        oneComm.ExecuteNonQuery();
                        oneComm.Parameters.Clear();

                        oneComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        oneComm.Parameters.Add("@AdjAvailableQty", SqlDbType.Decimal).Value = dAdjAvailableQty;
                        oneComm.Parameters.Add("@AdjCustomsQty", SqlDbType.Decimal).Value = dAdjCustomsQty;
                        oneComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                        oneComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                        oneComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[i]["Remark"].ToString().Trim().ToUpper();
                        oneComm.CommandText = @"INSERT INTO C_RMBalanceAdjustment([RM Customs Code], [BGD No], [ADJ Available Qty], [ADJ Customs Qty], [Created Date], " +
                                               "[Creater], [Remark]) VALUES(@RMCustomsCode, @BGDNo, @AdjAvailableQty, @AdjCustomsQty, @CreatedDate, @Creater, @Remark)";
                        oneComm.ExecuteNonQuery();
                        oneComm.Parameters.Clear();
                    }
                }
            }

            dTable.Clear();
            dTable.Dispose();
            myTable.Clear();
            myTable.Dispose();
            oneComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
            oneComm.ExecuteNonQuery();
        }

        private void dgvRMBalance_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvRMBalance.RowCount; i++) { this.dgvRMBalance[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvRMBalance.RowCount; i++) { this.dgvRMBalance[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvRMBalance.RowCount; i++)
                    {
                        if (String.Compare(this.dgvRMBalance[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvRMBalance[0, i].Value = true; }
                        else { this.dgvRMBalance[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvRMBalance_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvRMBalance.RowCount == 0) { return; }
            if (this.dgvRMBalance.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvRMBalance.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMBalance[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvRMBalance.RowCount && iCount > 0)
                { this.dgvRMBalance.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvRMBalance.RowCount)
                { this.dgvRMBalance.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvRMBalance.Columns[0].HeaderText = "全选"; }
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvRMBalance.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMBalance.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMBalance.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMBalance[strColumnName, this.dgvRMBalance.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
                if (this.dgvRMBalance.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMBalance.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMBalance.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMBalance[strColumnName, this.dgvRMBalance.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMBalance.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
        }

        private void llblMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When upload RM Balance Adjustment data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                            "\n\tRM Customs Code, \n\tBGD No, \n\tADJ Available Qty, \n\tADJ Customs Qty, \n\tRemark", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
