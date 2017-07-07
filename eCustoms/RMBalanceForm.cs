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

namespace eCustoms
{
    public partial class RMBalanceForm : Form
    {
        SqlLib sqlLib = new SqlLib();
        private LoginForm loginFrm = new LoginForm();  
        private static RMBalanceForm RMBalanceFrm;
        public RMBalanceForm() { InitializeComponent(); }
        public static RMBalanceForm CreateInstance()
        {
            if (RMBalanceFrm == null || RMBalanceFrm.IsDisposed) { RMBalanceFrm = new RMBalanceForm(); }
            return RMBalanceFrm;
        }

        private void RMBalanceAdjustmentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlLib.Dispose(0);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string strFieldName = this.cmbFieldName.Text.Trim();
            string strTxtField = this.txtFieldName.Text.Trim();
            if (String.IsNullOrEmpty(strFieldName)) {
                MessageBox.Show("Please select field name first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (String.IsNullOrEmpty(strTxtField)) {
                MessageBox.Show("Please input the value of " + strFieldName + " field.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string strBrowse = @"SELECT * FROM C_RMBalance WHERE [" + strFieldName + "] = '" + strTxtField + "' ORDER BY [Item No], [Lot No], [RM EHB], [BGD No]";
            SqlConnection RMBalConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (RMBalConn.State == ConnectionState.Closed) { RMBalConn.Open(); }
            SqlDataAdapter RMBalAdapter = new SqlDataAdapter(strBrowse, RMBalConn);
            DataTable dtFillRmBal = new DataTable();
            RMBalAdapter.Fill(dtFillRmBal);
            RMBalAdapter.Dispose();

            if (dtFillRmBal.Rows.Count == 0)
            {
                dtFillRmBal.Clear();
                dtFillRmBal.Dispose();
                this.dgvRMBalance.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvRMBalance.DataSource = dtFillRmBal;
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
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            SqlConnection ConnDL = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnDL.State == ConnectionState.Closed) { ConnDL.Open(); }
            SqlDataAdapter AdapterDL = new SqlDataAdapter("SELECT * FROM C_RMBalance", ConnDL);
            DataTable dtDL = new DataTable();
            AdapterDL.Fill(dtDL);
            AdapterDL.Dispose();
            if (dtDL.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConnDL.Dispose(); 
                dtDL.Dispose();
                return;
            }

            int PageRow = 1048576;
            int iPageCount = (int)(dtDL.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtDL.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\RM Balance Data " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode);
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtDL.Columns.Count; n++)
                    { sb.Append(dtDL.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < dtDL.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtDL.Columns.Count; j++) { sb.Append("'" + dtDL.Rows[i][j].ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);
                    }
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                MessageBox.Show("Completely download all RM Balance data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            finally { ConnDL.Dispose(); dtDL.Dispose(); }
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
                oneComm.CommandText = "SELECT [Group] FROM " + FUVs.tableOfUsers + " WHERE [LoginName] = @LoginName";
                string strGroup = Convert.ToString(oneComm.ExecuteScalar()).Trim().ToUpper();
                if (String.Compare(strGroup, "ADMIN") != 0) {
                    MessageBox.Show("You DO NOT have access rights to adjust RM Balance data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
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
            string strConn = null;
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", myConn);
            DataTable myTable = new DataTable();
            myAdapter.Fill(myTable);
            myAdapter.Dispose();
            myConn.Dispose();          

            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myAdapter.Dispose();
                myTable.Clear();
                myTable.Dispose();
                return;
            }

            oneComm.CommandText = @"SELECT * FROM B_MultiUser";
            string strUserName = Convert.ToString(oneComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait for his/her to finish the process.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    oneComm.Dispose();
                    oneConn.Dispose();
                    return;
                }
            }
            else
            {
                oneComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                oneComm.CommandText = @"INSERT INTO B_MultiUser([UserName]) VALUES(@UserName)";
                oneComm.ExecuteNonQuery();
                oneComm.Parameters.RemoveAt("@UserName");
            }        
            oneComm.CommandText = @"SELECT [Item No], [Lot No], [RM EHB], [BGD No], [Customs Balance], [Available RM Balance] FROM C_RMBalance";
            SqlDataAdapter oneAdapter = new SqlDataAdapter();
            oneAdapter.SelectCommand = oneComm;
            DataTable dtRmBal = new DataTable(); 
            oneAdapter.Fill(dtRmBal);
            oneAdapter.Dispose();

            for (int i = 0; i < myTable.Rows.Count; i++)
            {
                string strAdjAvailableQty = myTable.Rows[i]["ADJ Available Qty"].ToString().Trim();
                string strAdjCustomsQty = myTable.Rows[i]["ADJ Customs Qty"].ToString().Trim();
                decimal dAdjAvailableQty = 0.0M, dAdjCustomsQty = 0.0M;
                if (!String.IsNullOrEmpty(strAdjAvailableQty)) { dAdjAvailableQty = Math.Round(Convert.ToDecimal(strAdjAvailableQty), 6); }
                if (!String.IsNullOrEmpty(strAdjCustomsQty)) { dAdjCustomsQty = Math.Round(Convert.ToDecimal(strAdjCustomsQty), 6); }

                if (dAdjAvailableQty != 0.0M || dAdjCustomsQty != 0.0M)
                {
                    string strItemNo = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                    string strLotNo = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                    string strRmEhb = myTable.Rows[i]["RM EHB"].ToString().Trim().ToUpper();
                    string strBgdNo = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                    DataRow[] dr = dtRmBal.Select("[Item No]='" + strItemNo + "' AND [Lot No]='" + strLotNo + "' AND [RM EHB]='" + strRmEhb + "' AND [BGD No]='" + strBgdNo + "'");
                    if (dr.Length > 0)
                    {
                        string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dr[0]["Customs Balance"].ToString().Trim()));
                        string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dr[0]["Available RM Balance"].ToString().Trim()));
                        decimal dCustomsBalance = Convert.ToDecimal(strCustomsBal) + dAdjCustomsQty;
                        decimal dAvailableRMBalance = Convert.ToDecimal(strAvailRMBal) + dAdjAvailableQty;

                        oneComm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBalance;
                        oneComm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBalance;
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        oneComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRmEhb;
                        oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBgdNo;
                        oneComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance " +
                                               "WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo AND [RM EHB] = @RMEHB AND [BGD No] = @BGDNo";
                        oneComm.ExecuteNonQuery();
                        oneComm.Parameters.Clear();

                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        oneComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRmEhb;
                        oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBgdNo;
                        oneComm.Parameters.Add("@AdjAvailableQty", SqlDbType.Decimal).Value = dAdjAvailableQty;
                        oneComm.Parameters.Add("@AdjCustomsQty", SqlDbType.Decimal).Value = dAdjCustomsQty;
                        oneComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                        oneComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                        oneComm.CommandText = @"INSERT INTO L_RMBalance_Adjustment([Item No], [Lot No], [RM EHB], [BGD No], [ADJ Available Qty], [ADJ Customs Qty], [Creater], " +
                                               "[Created Date]) VALUES(@ItemNo, @LotNo, @RMEHB, @BGDNo, @AdjAvailableQty, @AdjCustomsQty, @Creater, @CreatedDate)";
                        oneComm.ExecuteNonQuery();
                        oneComm.Parameters.Clear();
                    }
                }
            }
            dtRmBal.Dispose();
            myTable.Dispose();
            oneComm.CommandText = @"DELETE FROM B_MultiUser";
            oneComm.ExecuteNonQuery();
        }

        private void dgvRMBalance_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvRMBalance.RowCount; i++) { this.dgvRMBalance[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvRMBalance.RowCount; i++) { this.dgvRMBalance[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvRMBalance.RowCount; i++)
                    {
                        if (String.Compare(this.dgvRMBalance[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvRMBalance[0, i].Value = true; }
                        else { this.dgvRMBalance[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }
            System.Windows.Forms.SendKeys.Send("{ENTER}");
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
                { this.dgvRMBalance.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvRMBalance.RowCount)
                { this.dgvRMBalance.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvRMBalance.Columns[0].HeaderText = "Select"; }
            }
        }

        private void llblMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When upload RM Balance Adjustment data, please follow below fields name and sequence to list out in Excel. \n{Sheet1 as Excel default name}" +
                            "\n\tItem No, \n\tLot No, \n\tRM EHB, \n\tBGD No, \n\tADJ Available Qty, \n\tADJ Customs Qty", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
