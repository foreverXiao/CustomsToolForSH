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
    public partial class DroolsBalanceAdjustmentForm : Form
    {
        SqlLib sqlLib = new SqlLib();
        private LoginForm loginFrm = new LoginForm();
        private DataTable dt = new DataTable();
        private static DroolsBalanceAdjustmentForm DroolsBalanceAdjustmentFrm;
        public DroolsBalanceAdjustmentForm()
        {
            InitializeComponent();
        }
        public static DroolsBalanceAdjustmentForm CreateInstance()
        {
            if (DroolsBalanceAdjustmentFrm == null || DroolsBalanceAdjustmentFrm.IsDisposed)
            {
                DroolsBalanceAdjustmentFrm = new DroolsBalanceAdjustmentForm();
            }
            return DroolsBalanceAdjustmentFrm;
        }

        private void DroolsBalanceAdjustmentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.dt.Dispose();
            sqlLib.Dispose(0);
        }

        private void cmbDroolsEHB_Enter(object sender, EventArgs e)
        {
            if (this.dt.Rows.Count == 0)
            {
                this.dt = sqlLib.GetDataTable(@"SELECT [Drools EHB] FROM C_DroolsBalance", "C_DroolsBalance").Copy();
                DataRow dr = this.dt.NewRow();
                dr["Drools EHB"] = String.Empty;
                this.dt.Rows.InsertAt(dr, 0);
                this.cmbDroolsEHB.DisplayMember = this.cmbDroolsEHB.ValueMember = "Drools EHB";
                this.cmbDroolsEHB.DataSource = this.dt;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string strBrowse = @"SELECT * FROM C_DroolsBalance WHERE";
            if (!String.IsNullOrEmpty(this.cmbDroolsEHB.Text.ToString().Trim()))
            { strBrowse += " [Drools EHB] = '" + this.cmbDroolsEHB.Text.ToString().Trim().ToUpper() + "'"; }
            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 6, 6), " WHERE") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Length - 6); }

            SqlConnection DroolsBalConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (DroolsBalConn.State == ConnectionState.Closed) { DroolsBalConn.Open(); }

            SqlDataAdapter DroolsBalAdapter = new SqlDataAdapter(strBrowse, DroolsBalConn);
            DataTable dtDrools = new DataTable();
            DroolsBalAdapter.Fill(dtDrools);
            DroolsBalAdapter.Dispose();

            if (dtDrools.Rows.Count == 0)
            {
                dtDrools.Dispose();
                this.dgvDroolsBalance.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvDroolsBalance.DataSource = dtDrools;
                this.dgvDroolsBalance.Rows[0].HeaderCell.Value = 1;
            }

            if (DroolsBalConn.State == ConnectionState.Open)
            {
                DroolsBalConn.Close();
                DroolsBalConn.Dispose();
            }

            for (int i = 1; i < this.dgvDroolsBalance.ColumnCount; i++)
            { this.dgvDroolsBalance.Columns[i].ReadOnly = true; }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvDroolsBalance.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (this.dgvDroolsBalance.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select the data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvDroolsBalance.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application DroolsBalExcel = new Microsoft.Office.Interop.Excel.Application();
            DroolsBalExcel.Application.Workbooks.Add(true);

            bool bJudge = false;
            int iRow = 2;
            for (int i = 0; i < this.dgvDroolsBalance.RowCount; i++)
            {
                if (String.Compare(this.dgvDroolsBalance[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    bJudge = true;
                    DroolsBalExcel.get_Range(DroolsBalExcel.Cells[iRow, 1], DroolsBalExcel.Cells[iRow, this.dgvDroolsBalance.ColumnCount - 1]).NumberFormatLocal = "@";
                    for (int j = 1; j < this.dgvDroolsBalance.ColumnCount; j++)
                    { DroolsBalExcel.Cells[iRow, j] = this.dgvDroolsBalance[j, i].Value.ToString().Trim(); }
                    iRow++;
                }
            }

            if (bJudge)
            {
                DroolsBalExcel.get_Range(DroolsBalExcel.Cells[1, 1], DroolsBalExcel.Cells[1, this.dgvDroolsBalance.ColumnCount - 1]).NumberFormatLocal = "@";
                for (int k = 1; k < this.dgvDroolsBalance.ColumnCount; k++)
                { DroolsBalExcel.Cells[1, k] = this.dgvDroolsBalance.Columns[k].HeaderText.ToString(); }

                DroolsBalExcel.get_Range(DroolsBalExcel.Cells[1, 1], DroolsBalExcel.Cells[1, this.dgvDroolsBalance.ColumnCount - 1]).Font.Bold = true;
                DroolsBalExcel.get_Range(DroolsBalExcel.Cells[1, 1], DroolsBalExcel.Cells[1, this.dgvDroolsBalance.ColumnCount - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                DroolsBalExcel.get_Range(DroolsBalExcel.Cells[1, 1], DroolsBalExcel.Cells[iRow - 1, this.dgvDroolsBalance.ColumnCount - 1]).Font.Name = "Verdana";
                DroolsBalExcel.get_Range(DroolsBalExcel.Cells[1, 1], DroolsBalExcel.Cells[iRow - 1, this.dgvDroolsBalance.ColumnCount - 1]).Font.Size = 9;
                DroolsBalExcel.get_Range(DroolsBalExcel.Cells[1, 1], DroolsBalExcel.Cells[iRow - 1, this.dgvDroolsBalance.ColumnCount - 1]).Borders.LineStyle = 1;
                DroolsBalExcel.Cells.EntireColumn.AutoFit();
                DroolsBalExcel.Visible = true;
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(DroolsBalExcel);
            DroolsBalExcel = null;
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
                if (String.Compare(strGroup, "DROOLS") != 0)
                {
                    if (String.Compare(strGroup, "ADMIN") != 0)
                    {
                        MessageBox.Show("You donot have access rights to adjust Drools Balance data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                oneComm.Parameters.Clear();

                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge, oneConn, oneComm);
                if (this.dgvDroolsBalance.RowCount > 0) { this.btnBrowse_Click(sender, e); }
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
            DataTable oneTable = new DataTable();
            myAdapter.Fill(oneTable);
            myAdapter.Dispose();
            myConn.Close();
            myConn.Dispose();

            if (oneTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myAdapter.Dispose();
                oneTable.Clear();
                oneTable.Dispose();
                return;
            }

            oneComm.CommandText = @"SELECT * FROM C_DroolsBalance";
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = oneComm;
            DataTable dTable = new DataTable();
            sqlAdapter.Fill(dTable);
            sqlAdapter.Dispose();

            for (int i = 0; i < oneTable.Rows.Count; i++)
            {
                string strAdjustmentQty = oneTable.Rows[i]["Adjustment Qty"].ToString().Trim();
                decimal dAdjustmentQty = Math.Round(Convert.ToDecimal(strAdjustmentQty), 4);
                if (!String.IsNullOrEmpty(strAdjustmentQty) && dAdjustmentQty != 0.0M)
                {
                    string strDroolsEHB = oneTable.Rows[i]["Drools EHB"].ToString().Trim().ToUpper();
                    DataRow dr = dTable.Select("[Drools EHB] = '" + strDroolsEHB + "'")[0];
                    decimal dAvailableBalance = Convert.ToDecimal(dr["Available Balance"].ToString().Trim()) + dAdjustmentQty;

                    oneComm.Parameters.Add("@AvailableBalance", SqlDbType.Decimal).Value = dAvailableBalance;
                    oneComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = strDroolsEHB;
                    oneComm.CommandText = @"UPDATE C_DroolsBalance SET [Available Balance] = @AvailableBalance WHERE [Drools EHB] = @DroolsEHB";
                    oneComm.ExecuteNonQuery();
                    oneComm.Parameters.Clear();

                    oneComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = strDroolsEHB;
                    oneComm.Parameters.Add("@AdjustmentQty", SqlDbType.Decimal).Value = dAdjustmentQty;
                    oneComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = oneTable.Rows[i]["Remark"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                    oneComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));                  
                    oneComm.CommandText = @"INSERT INTO C_DroolsBalanceAdjustment([Drools EHB], [Adjustment Qty], [Remark], [Creater], [Created Date]) " +
                                           "VALUES(@DroolsEHB, @AdjustmentQty, @Remark, @Creater, @CreatedDate)";
                    oneComm.ExecuteNonQuery();
                    oneComm.Parameters.Clear();
                }
            }

            if (MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                dTable.Clear();
                dTable.Dispose();
                oneTable.Clear();
                oneTable.Dispose();
            }
        }

        private void llblMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When upload Drools Balance Adjustment data, please follow below fields name and sequence to list out in Excel.\n{Sheet1 as Excel default name}" +
                           "\n\tDrools EHB, \n\tAdjustment Qty, \n\tRemark", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvDroolsBalance_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvDroolsBalance.RowCount; i++) { this.dgvDroolsBalance[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvDroolsBalance.RowCount; i++) { this.dgvDroolsBalance[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvDroolsBalance.RowCount; i++)
                    {
                        if (String.Compare(this.dgvDroolsBalance[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvDroolsBalance[0, i].Value = true; }

                        else { this.dgvDroolsBalance[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvDroolsBalance_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvDroolsBalance.RowCount == 0) { return; }
            if (this.dgvDroolsBalance.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvDroolsBalance.RowCount; i++)
                {
                    if (String.Compare(this.dgvDroolsBalance[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvDroolsBalance.RowCount && iCount > 0)
                { this.dgvDroolsBalance.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvDroolsBalance.RowCount)
                { this.dgvDroolsBalance.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvDroolsBalance.Columns[0].HeaderText = "全选"; }
            }
        }
    }
}
