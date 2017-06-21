using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class RMShareOutForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable rmTable = new DataTable();
        private DataView rmView = new DataView();
        private DataTable dtRMEHB = new DataTable();
        private DataTable dtItemNo = new DataTable();
        SqlLib sqlLib = new SqlLib();
        string strSwitch = null;
        string strFilter = null;
        int iIndexRow = 0;
        protected PopUpFilterForm filterFrm = null;

        private static RMShareOutForm RMShareOutFrm;
        public static RMShareOutForm CreateInstance()
        {
            if (RMShareOutFrm == null || RMShareOutFrm.IsDisposed)
            {
                RMShareOutFrm = new RMShareOutForm();
            }
            return RMShareOutFrm;
        }
        public RMShareOutForm()
        {
            InitializeComponent();
        }

        private void SetBlank()
        {
            this.cmbType.Text = String.Empty;
            this.txtRMCustomsCode.Text = String.Empty;
            this.txtBeiAnDanNoIN.Text = String.Empty;
            this.txtItemNo.Text = String.Empty;
            this.txtLotNo.Text = String.Empty;
            this.dtpCreatedDate.CustomFormat = " ";

            this.txtRMCHNName.Text = String.Empty;
            this.txtNetWeight.Text = String.Empty;
            this.txtBeiAnDanNoOut.Text = String.Empty;
            this.txtBeiAnDanID.Text = String.Empty;
            this.dtpBeiAnDanDate.CustomFormat = " ";
            this.dtpCustomsReleaseDate.CustomFormat = " ";
            this.txtPingDanID.Text = String.Empty;
            this.txtPingDanNo.Text = String.Empty;
            this.txtPingDanStatus.Text = String.Empty;
            this.dtpPassGateDate.CustomFormat = " ";
            this.txtRemark.Text = String.Empty;
        }

        private void SetReadOnly(string strBehavior)
        {
            switch (strBehavior)
            {
                case "readonly":
                    this.cmbType.Enabled = false;
                    this.txtRMCustomsCode.ReadOnly = true;
                    this.txtBeiAnDanNoIN.ReadOnly = true;
                    this.txtItemNo.ReadOnly = true;
                    this.txtLotNo.ReadOnly = true;
                    this.dtpCreatedDate.Enabled= false;

                    this.txtRMCHNName.ReadOnly = true;
                    this.txtNetWeight.ReadOnly = true;
                    this.txtBeiAnDanNoOut.ReadOnly = true;
                    this.txtBeiAnDanID.ReadOnly = true;
                    this.dtpBeiAnDanDate.Enabled = false;
                    this.dtpCustomsReleaseDate.Enabled = false;
                    this.txtPingDanID.ReadOnly = true;
                    this.txtPingDanNo.ReadOnly = true;
                    this.txtPingDanStatus.ReadOnly = true;
                    this.dtpPassGateDate.Enabled = false;
                    this.txtRemark.ReadOnly = true;
                    break;
                case "edition":
                    this.cmbType.Enabled = false;
                    this.txtRMCustomsCode.ReadOnly = true;
                    this.txtBeiAnDanNoIN.ReadOnly = true;
                    this.txtItemNo.ReadOnly = true;
                    this.txtLotNo.ReadOnly = true;
                    this.dtpCreatedDate.Enabled= false;

                    this.txtRMCHNName.ReadOnly = false;
                    this.txtNetWeight.ReadOnly = false;
                    this.txtBeiAnDanNoOut.ReadOnly = false;
                    this.txtBeiAnDanID.ReadOnly = false;
                    this.dtpBeiAnDanDate.Enabled = true;
                    this.dtpCustomsReleaseDate.Enabled = true;
                    this.txtPingDanID.ReadOnly = false;
                    this.txtPingDanNo.ReadOnly = false;
                    this.txtPingDanStatus.ReadOnly = false;
                    this.dtpPassGateDate.Enabled = true;
                    this.txtRemark.ReadOnly = false;
                    break;
                case "addition":
                     this.cmbType.Enabled = true;
                    this.txtRMCustomsCode.ReadOnly = false;
                    this.txtBeiAnDanNoIN.ReadOnly = false;
                    this.txtItemNo.ReadOnly = false;
                    this.txtLotNo.ReadOnly = false;
                    this.dtpCreatedDate.Enabled= true;

                    this.txtRMCHNName.ReadOnly = false;
                    this.txtNetWeight.ReadOnly = false;
                    this.txtBeiAnDanNoOut.ReadOnly = false;
                    this.txtBeiAnDanID.ReadOnly = false;
                    this.dtpBeiAnDanDate.Enabled = true;
                    this.dtpCustomsReleaseDate.Enabled = true;
                    this.txtPingDanID.ReadOnly = false;
                    this.txtPingDanNo.ReadOnly = false;
                    this.txtPingDanStatus.ReadOnly = false;
                    this.dtpPassGateDate.Enabled = true;
                    this.txtRemark.ReadOnly = false;
                    break;
                default: break;
            }
        }

        private void RMShareOutForm_Load(object sender, EventArgs e)
        {
            this.SetBlank();
            this.SetReadOnly("readonly");
        }

        private void RMShareOutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            rmView.Dispose();
            rmTable.Dispose();
            dtRMEHB.Dispose();
            dtItemNo.Dispose();
            sqlLib.Dispose(0);
        }

        private void dtpCreatedDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpCreatedDate.CustomFormat = null;
        }

        private void dtpCreatedDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpCreatedDate.CustomFormat = " "; }
        }

        private void dtpBeiAnDanDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpBeiAnDanDate.CustomFormat = null;
        }

        private void dtpBeiAnDanDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpBeiAnDanDate.CustomFormat = " "; }
        }

        private void dtpCustomsReleaseDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpCustomsReleaseDate.CustomFormat = null;
        }

        private void dtpCustomsReleaseDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpCustomsReleaseDate.CustomFormat = " "; }
        }

        private void dtpPassGateDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpPassGateDate.CustomFormat = null;
        }

        private void dtpPassGateDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpPassGateDate.CustomFormat = " "; }
        }

        private void GetItemNo()
        {
            dtItemNo = sqlLib.GetDataTable(@"SELECT DISTINCT [Item No] FROM C_RMShareOut", "B_RMShareOut").Copy();
            DataRow ItemNoRow = dtItemNo.NewRow();
            ItemNoRow["Item No"] = String.Empty;
            dtItemNo.Rows.InsertAt(ItemNoRow, 0);
            this.cmbItemNo.DisplayMember = this.cmbItemNo.ValueMember = "Item No";
            this.cmbItemNo.DataSource = dtItemNo;
        }

        private void cmbRMCustomsCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbRMCustomsCode.SelectedIndex > 0)
            {
                dtItemNo.Reset();
                dtItemNo = sqlLib.GetDataTable(@"SELECT DISTINCT [Item No] FROM C_RMShareOut WHERE [原料备件号] = '" + this.cmbRMCustomsCode.SelectedValue.ToString().Trim() + "'", "B_RMShareOut").Copy();
                DataRow ItemNoRow = dtItemNo.NewRow();
                ItemNoRow["Item No"] = String.Empty;
                dtItemNo.Rows.InsertAt(ItemNoRow, 0);
                this.cmbItemNo.DisplayMember = this.cmbItemNo.ValueMember = "Item No";
                this.cmbItemNo.DataSource = dtItemNo;
            }
            else { this.GetItemNo(); }
        }

        private void cmbRMCustomsCode_Enter(object sender, EventArgs e)
        {
            if (dtRMEHB.Rows.Count == 0)
            {
                dtRMEHB = sqlLib.GetDataTable(@"SELECT DISTINCT [原料备件号] FROM C_RMShareOut", "B_RMShareOut").Copy();
                DataRow RMEHBRow = dtRMEHB.NewRow();
                RMEHBRow["原料备件号"] = String.Empty;
                dtRMEHB.Rows.InsertAt(RMEHBRow, 0);
                this.cmbRMCustomsCode.DisplayMember = this.cmbRMCustomsCode.ValueMember = "原料备件号";
                this.cmbRMCustomsCode.DataSource = dtRMEHB;
            }
        }

        private void cmbItemNo_Enter(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbRMCustomsCode.Text.ToString().Trim())) { this.GetItemNo(); }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            strFilter = "";
            rmView.RowFilter = "";
            string strBrowse = @"SELECT * FROM C_RMShareOut";
            string strRMEHB = this.cmbRMCustomsCode.Text.ToString().Trim();
            string strItemNo = this.cmbItemNo.Text.ToString().Trim();
            if (!String.IsNullOrEmpty(strRMEHB) && String.IsNullOrEmpty(strItemNo))
            { strBrowse += " WHERE [原料备件号] = '" + strRMEHB.ToUpper() + "'"; }
            else if (String.IsNullOrEmpty(strRMEHB) && !String.IsNullOrEmpty(strItemNo))
            { strBrowse += " WHERE [Item No] = '" + strItemNo.ToUpper() + "'"; }
            else if (!String.IsNullOrEmpty(strRMEHB) && !String.IsNullOrEmpty(strItemNo))
            { strBrowse += " WHERE [原料备件号] = '" + strRMEHB.ToUpper() + "' AND [Item No] = '" + strItemNo.ToUpper() + "'"; }
            strBrowse += " ORDER BY [Created Date] DESC";

            SqlConnection rmConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (rmConn.State == ConnectionState.Closed) { rmConn.Open(); }
            SqlDataAdapter rmAdapter = new SqlDataAdapter(strBrowse, rmConn);
            rmTable.Rows.Clear();
            rmTable.Columns.Clear();
            rmAdapter.Fill(rmTable);
            rmAdapter.Dispose();
            rmView = rmTable.DefaultView;

            if (rmTable.Rows.Count == 0)
            {
                rmTable.Clear();
                this.dgvRMShareOut.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvRMShareOut.DataSource = rmView;
                this.dgvRMShareOut.Columns[0].HeaderText = "全选";
                for (int i = 2; i < this.dgvRMShareOut.ColumnCount; i++)
                { this.dgvRMShareOut.Columns[i].ReadOnly = true; }
                this.dgvRMShareOut.Columns[4].Frozen = true;
            }
            if (rmConn.State == ConnectionState.Open)
            {
                rmConn.Close();
                rmConn.Dispose();
            }
            this.SetBlank();
            this.SetReadOnly("readonly");
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvRMShareOut.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvRMShareOut.Columns[0].HeaderText != "全选")
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Application.Workbooks.Add(true);

                int iRow = 2;
                for (int i = 0; i < this.dgvRMShareOut.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMShareOut[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        excel.get_Range(excel.Cells[iRow, 1], excel.Cells[iRow, this.dgvRMShareOut.ColumnCount - 2]).NumberFormatLocal = "@";
                        for (int j = 2; j < this.dgvRMShareOut.ColumnCount; j++)
                        { excel.Cells[iRow, j - 1] = this.dgvRMShareOut[j, i].Value.ToString().Trim(); }
                        iRow++;
                    }
                }

                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvRMShareOut.ColumnCount - 2]).NumberFormatLocal = "@";
                for (int k = 2; k < this.dgvRMShareOut.ColumnCount; k++)
                { excel.Cells[1, k - 1] = this.dgvRMShareOut.Columns[k].HeaderText.ToString(); }

                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvRMShareOut.ColumnCount - 2]).Font.Bold = true;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvRMShareOut.ColumnCount - 2]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvRMShareOut.ColumnCount - 2]).Font.Name = "Verdana";
                excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvRMShareOut.ColumnCount - 2]).Font.Size = 9;
                excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvRMShareOut.ColumnCount - 2]).Borders.LineStyle = 1;
                excel.Cells.EntireColumn.AutoFit();
                excel.Visible = true;

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
            }
            else
            {
                int PageRow = 65536;
                //int iPageCount = (Int32)(rmTable.Rows.Count / PageRow);
                //if (iPageCount * PageRow < rmTable.Rows.Count) { iPageCount += 1; }
                int iPageCount = (Int32)(rmView.Table.Rows.Count / PageRow);              
                if (iPageCount * PageRow < rmView.Table.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\RM Receiving Data " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        //for (int n = 0; n < rmTable.Columns.Count; n++)
                        //{ sb.Append(rmTable.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        for (int n = 0; n < rmView.Table.Columns.Count; n++)
                        { sb.Append(rmView.Table.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        //for (int i = (m - 1) * PageRow; i < rmTable.Rows.Count && i < m * PageRow; i++)
                        //{
                        //    System.Windows.Forms.Application.DoEvents();
                        //    for (int j = 0; j < rmTable.Columns.Count; j++)
                        //    {
                        //        if (j == 2 || j == 7 || j == 11) { sb.Append("'" + rmTable.Rows[i][j].ToString().Trim() + "\t"); }
                        //        else { sb.Append(rmTable.Rows[i][j].ToString().Trim() + "\t"); }
                        //    }
                        //    sb.Append(Environment.NewLine);
                        //}
                        for (int i = (m - 1) * PageRow; i < rmView.Table.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < rmView.Table.Columns.Count; j++)
                            {
                                if (j == 2 || j == 7 || j == 11) { sb.Append("'" + rmView.Table.Rows[i][j].ToString().Trim() + "\t"); }
                                else { sb.Append(rmView.Table.Rows[i][j].ToString().Trim() + "\t"); }
                            }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                    MessageBox.Show("Successfully generated Related RM Receving data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void GetRowData()
        {
            int iRowIndex = this.dgvRMShareOut.CurrentRow.Index;
            this.cmbType.Text = this.dgvRMShareOut["类型", iRowIndex].Value.ToString().Trim();
            this.txtRMCustomsCode.Text = this.dgvRMShareOut["原料备件号", iRowIndex].Value.ToString().Trim();
            this.txtBeiAnDanNoIN.Text = this.dgvRMShareOut["进境备案单号", iRowIndex].Value.ToString().Trim();
            this.txtItemNo.Text = this.dgvRMShareOut["Item No", iRowIndex].Value.ToString().Trim();
            this.txtLotNo.Text = this.dgvRMShareOut["Lot No", iRowIndex].Value.ToString().Trim();
            this.dtpCreatedDate.CustomFormat = Convert.ToDateTime(this.dgvRMShareOut["Created Date", iRowIndex].Value.ToString().Trim()).ToString("M/d/yyyy"); 

            this.txtRMCHNName.Text = this.dgvRMShareOut["中文品名", iRowIndex].Value.ToString().Trim();
            this.txtNetWeight.Text = this.dgvRMShareOut["净重", iRowIndex].Value.ToString().Trim();
            this.txtBeiAnDanNoOut.Text = this.dgvRMShareOut["出境备案单号", iRowIndex].Value.ToString().Trim();
            this.txtBeiAnDanID.Text = this.dgvRMShareOut["备案ID", iRowIndex].Value.ToString().Trim();
            if (!String.IsNullOrEmpty(this.dgvRMShareOut["备案时间", iRowIndex].Value.ToString().Trim()))
            { this.dtpBeiAnDanDate.CustomFormat = Convert.ToDateTime(this.dgvRMShareOut["备案时间", iRowIndex].Value.ToString().Trim()).ToString("M/d/yyyy"); }
            else { this.dtpBeiAnDanDate.CustomFormat = " "; }
            if (!String.IsNullOrEmpty(this.dgvRMShareOut["放行日期审核时间", iRowIndex].Value.ToString().Trim()))
            { this.dtpCustomsReleaseDate.CustomFormat = Convert.ToDateTime(this.dgvRMShareOut["放行日期审核时间", iRowIndex].Value.ToString().Trim()).ToString("M/d/yyyy"); }
            else { this.dtpCustomsReleaseDate.CustomFormat = " "; }
            this.txtPingDanID.Text = this.dgvRMShareOut["凭单ID", iRowIndex].Value.ToString().Trim();
            this.txtPingDanNo.Text = this.dgvRMShareOut["出区凭单", iRowIndex].Value.ToString().Trim();
            this.txtPingDanStatus.Text = this.dgvRMShareOut["凭单状态", iRowIndex].Value.ToString().Trim();
            if (!String.IsNullOrEmpty(this.dgvRMShareOut["卡口过机日期", iRowIndex].Value.ToString().Trim()))
            { this.dtpPassGateDate.CustomFormat = Convert.ToDateTime(this.dgvRMShareOut["卡口过机日期", iRowIndex].Value.ToString().Trim()).ToString("M/d/yyyy"); }
            else { this.dtpPassGateDate.CustomFormat = " "; }
            this.txtRemark.Text = this.dgvRMShareOut["Remark", iRowIndex].Value.ToString().Trim();
            iIndexRow = iRowIndex;
        }

        private void dgvRMShareOut_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1) { this.GetRowData(); }
        }

        private void dgvRMShareOut_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvRMShareOut.RowCount; i++) { this.dgvRMShareOut[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvRMShareOut.RowCount; i++) { this.dgvRMShareOut[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvRMShareOut.RowCount; i++)
                    {
                        if (String.Compare(this.dgvRMShareOut[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvRMShareOut[0, i].Value = true; }
                        else { this.dgvRMShareOut[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvRMShareOut_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvRMShareOut.RowCount == 0) { return; }
            if (this.dgvRMShareOut.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvRMShareOut.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMShareOut[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvRMShareOut.RowCount && iCount > 0)
                { this.dgvRMShareOut.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvRMShareOut.RowCount)
                { this.dgvRMShareOut.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvRMShareOut.Columns[0].HeaderText = "全选"; }
            }
        }

        private void toolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {           
            if (e.Button.ToolTipText == "Add")
            {
                this.SetBlank();
                this.SetReadOnly("addition");
                strSwitch = "Add";
            }

            if (e.Button.ToolTipText == "Edit")
            {
                if (String.IsNullOrEmpty(this.txtRMCustomsCode.Text.Trim()))
                {
                    MessageBox.Show("Please select to edit the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.SetReadOnly("edition");
                strSwitch = "Edit";
            }

            if (e.Button.ToolTipText == "Update" || e.Button.ToolTipText == "Delete")
            {
                SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;

                /*------ Monitor And Control Multiple Users ------*/
                sqlComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
                string strUserName = Convert.ToString(sqlComm.ExecuteScalar());
                if (!String.IsNullOrEmpty(strUserName))
                {
                    if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                    {
                        MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        sqlComm.Dispose();
                        sqlConn.Dispose();
                        return;
                    }
                }
                else
                {
                    sqlComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                    sqlComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                    sqlComm.ExecuteNonQuery();
                    sqlComm.Parameters.RemoveAt("@UserName");
                }

                if (e.Button.ToolTipText == "Update")
                {
                    if (String.Compare(strSwitch, "Add") == 0)
                    {
                        if (String.IsNullOrEmpty(this.cmbType.Text.Trim()) ||
                            String.IsNullOrEmpty(this.txtRMCustomsCode.Text.Trim()) ||
                            String.IsNullOrEmpty(this.txtBeiAnDanNoIN.Text.Trim()) ||
                            String.IsNullOrEmpty(this.txtItemNo.Text.Trim()) ||
                            String.IsNullOrEmpty(this.txtLotNo.Text.Trim()) ||
                            String.IsNullOrEmpty(this.dtpCreatedDate.Text.Trim()) ||
                            String.IsNullOrEmpty(this.dtpBeiAnDanDate.Text.Trim()) || 
                            String.IsNullOrEmpty(this.txtNetWeight.Text.Trim()))
                        {
                            MessageBox.Show("Please input data for all columns containing *.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            goto delObj;
                        }

                        sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.txtItemNo.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.txtLotNo.Text.Trim().ToUpper();
                        sqlComm.CommandText = @"SELECT T2.[Item No], T2.[Lot No], T1.[RM Customs Code], T1.[BGD No], T1.[Customs Balance], T1.[Available RM Balance], " +
                                               "T1.[SharingRM Qty] FROM C_RMBalance AS T1 RIGHT JOIN C_RMPurchase AS T2 ON T1.[BGD No] = T2.[BGD No] AND " +
                                               "T1.[RM Customs Code] = T2.[RM Customs Code] WHERE T2.[Item No] = @ItemNo AND T2.[Lot No] = @LotNo";
                        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                        sqlAdapter.SelectCommand = sqlComm;
                        DataTable dt1 = new DataTable();
                        sqlAdapter.Fill(dt1);
                        sqlAdapter.Dispose();
                        sqlComm.Parameters.Clear();

                        if (dt1.Rows.Count == 0)
                        {
                            MessageBox.Show("There is no data in RM Receiving table. Please check the data correctness.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dt1.Dispose();
                            goto delObj;
                        }
                        else
                        {
                            string strRMEHB = dt1.Rows[0]["RM Customs Code"].ToString().Trim();
                            string strBGDNo = dt1.Rows[0]["BGD No"].ToString().Trim();
                            string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dt1.Rows[0]["Customs Balance"].ToString().Trim()));
                            string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dt1.Rows[0]["Available RM Balance"].ToString().Trim()));
                            string strPOInvQty = sqlLib.doubleFormat(Double.Parse(dt1.Rows[0]["SharingRM Qty"].ToString().Trim()));
                            decimal dCustomsBal = Convert.ToDecimal(strCustomsBal);
                            decimal dAvailRMBal = Convert.ToDecimal(strAvailRMBal);
                            decimal dShareOut = Convert.ToDecimal(strPOInvQty);

                            decimal dNetWeight = Math.Round(Convert.ToDecimal(this.txtNetWeight.Text.Trim()), 6);
                            string strBeiAnDanDate = this.dtpBeiAnDanDate.Text.Trim();
                            string strCustomsReleaseDate = this.dtpCustomsReleaseDate.Text.Trim();
                            if (String.IsNullOrEmpty(strBeiAnDanDate) && String.IsNullOrEmpty(strCustomsReleaseDate))
                            {
                                sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut + dNetWeight;
                                sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                sqlComm.CommandText = @"UPDATE C_RMBalance SET [SharingRM Qty] = @ShareOut WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                sqlComm.ExecuteNonQuery();
                                sqlComm.Parameters.Clear();
                            }
                            else if (!String.IsNullOrEmpty(strBeiAnDanDate) && String.IsNullOrEmpty(strCustomsReleaseDate))
                            {
                                if (dAvailRMBal - dNetWeight < 0.0M)
                                {
                                    if (MessageBox.Show("The Share-out Qty is greater than Available RM Balance, do you want to continue?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        dt1.Dispose();
                                        goto delObj;
                                    }
                                }

                                sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = dAvailRMBal - dNetWeight;
                                sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut + dNetWeight;
                                sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                sqlComm.CommandText = @"UPDATE C_RMBalance SET [Available RM Balance] = @AvailRMBal, [SharingRM Qty] = @ShareOut " +
                                                       "WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                sqlComm.ExecuteNonQuery();
                                sqlComm.Parameters.Clear();
                            }
                            else if (String.IsNullOrEmpty(strBeiAnDanDate) && !String.IsNullOrEmpty(strCustomsReleaseDate)) { /*this case can not happen*/ }
                            else
                            {
                                sqlComm.Parameters.Add("@CustomsBal", SqlDbType.Decimal).Value = dCustomsBal - dNetWeight;
                                sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = dAvailRMBal - dNetWeight;
                                sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut + dNetWeight;
                                sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                sqlComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBal, [Available RM Balance] = @AvailRMBal, " +
                                                       "[SharingRM Qty] = @ShareOut WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                sqlComm.ExecuteNonQuery();
                                sqlComm.Parameters.Clear();
                            }
                        }
                        dt1.Dispose();

                        sqlComm.Parameters.Add("@Type", SqlDbType.NVarChar).Value = this.cmbType.Text.Trim();
                        sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = this.txtRMCustomsCode.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@BeiAnDanNoIN", SqlDbType.NVarChar).Value = this.txtBeiAnDanNoIN.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.txtItemNo.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.txtLotNo.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = this.txtRMCHNName.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@NetWeight", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.txtNetWeight.Text.Trim()), 6);
                        sqlComm.Parameters.Add("@BeiAnDanNoOUT", SqlDbType.NVarChar).Value = this.txtBeiAnDanNoOut.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@BeiAnDanID", SqlDbType.NVarChar).Value = this.txtBeiAnDanID.Text.Trim().ToUpper();
                        if (String.IsNullOrEmpty(this.dtpBeiAnDanDate.Text.Trim())) { sqlComm.Parameters.Add("@BeiAnDanDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { sqlComm.Parameters.Add("@BeiAnDanDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dtpBeiAnDanDate.Text.Trim()); }
                        if (String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim())) { sqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { sqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dtpCustomsReleaseDate.Text.Trim()); }
                        sqlComm.Parameters.Add("@PingDanID", SqlDbType.NVarChar).Value = this.txtPingDanID.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@PingDanNo", SqlDbType.NVarChar).Value = this.txtPingDanNo.Text.Trim().ToUpper();
                        sqlComm.Parameters.Add("@PingDanStatus", SqlDbType.NVarChar).Value = this.txtPingDanStatus.Text.Trim().ToUpper();
                        if (String.IsNullOrEmpty(this.dtpPassGateDate.Text.Trim())) { sqlComm.Parameters.Add("@PassGateDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { sqlComm.Parameters.Add("@PassGateDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dtpPassGateDate.Text.Trim()); }
                        if (String.IsNullOrEmpty(this.dtpCreatedDate.Text.Trim())) { sqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy")); }
                        else { sqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dtpCreatedDate.Text.Trim()); }
                        sqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = this.txtRemark.Text.Trim().ToUpper();

                        sqlComm.CommandText = @"INSERT INTO C_RMShareOut([类型], [原料备件号], [进境备案单号], [Item No], [Lot No], [中文品名], [净重], [出境备案单号], [备案ID], " +
                                               "[备案时间], [放行日期审核时间], [凭单ID], [出区凭单], [凭单状态], [卡口过机日期], [Created Date], [Remark]) VALUES(@Type, " +
                                               "@RMCustomsCode, @BeiAnDanNoIN, @ItemNo, @LotNo, @RMCHNName, @NetWeight, @BeiAnDanNoOUT, @BeiAnDanID, @BeiAnDanDate, " +
                                               "@CustomsReleaseDate, @PingDanID, @PingDanNo, @PingDanStatus, @PassGateDate, @CreatedDate, @Remark)";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();

                        this.SetReadOnly("readonly");
                        MessageBox.Show("Successfully add new data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        strSwitch = String.Empty;
                    }

                    if (String.Compare(strSwitch, "Edit") == 0)
                    {
                        int iRowIndex = iIndexRow; /*int iRowIndex = this.dgvRMShareOut.CurrentRow.Index*/
                        string strType = this.dgvRMShareOut["类型", iRowIndex].Value.ToString().Trim();
                        string strRMCustomsCode = this.dgvRMShareOut["原料备件号", iRowIndex].Value.ToString().Trim();
                        string strBeiAnDanNoIN = this.dgvRMShareOut["进境备案单号", iRowIndex].Value.ToString().Trim();
                        string strItemNo = this.dgvRMShareOut["Item No", iRowIndex].Value.ToString().Trim();
                        string strLotNo = this.dgvRMShareOut["Lot No", iRowIndex].Value.ToString().Trim();
                        DateTime dtCreatedDate = Convert.ToDateTime(this.dgvRMShareOut["Created Date", iRowIndex].Value.ToString().Trim());
                        decimal dNetWeight = Convert.ToDecimal(this.dgvRMShareOut["净重", iRowIndex].Value.ToString().Trim());
                        string strBeiAnDanDate = this.dgvRMShareOut["备案时间", iRowIndex].Value.ToString().Trim();
                        string strCustomsReleaseDate = this.dgvRMShareOut["放行日期审核时间", iRowIndex].Value.ToString().Trim();

                        string strCHNName = this.txtRMCHNName.Text.ToString().Trim().ToUpper();
                        decimal dNW = Math.Round(Convert.ToDecimal(this.txtNetWeight.Text.ToString().Trim()), 6);
                        string strBeiAnDanNoOUT = this.txtBeiAnDanNoOut.Text.ToString().Trim().ToUpper();
                        string strBeiAnDanID = this.txtBeiAnDanID.Text.ToString().Trim().ToUpper();
                        string strBADDate = this.dtpBeiAnDanDate.Text.ToString().Trim();
                        string strCRDate = this.dtpCustomsReleaseDate.Text.ToString().Trim();
                        string strPingDanID = this.txtPingDanID.Text.ToString().Trim().ToUpper();
                        string strPingDanNo = this.txtPingDanNo.Text.ToString().Trim().ToUpper();
                        string strPingDanStatus = this.txtPingDanStatus.Text.ToString().Trim().ToUpper();
                        string strPassGateDate = this.dtpPassGateDate.Text.ToString().Trim();
                        string strRemark = this.txtRemark.Text.ToString().Trim().ToUpper();

                        sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        sqlComm.CommandText = @"SELECT T2.[Item No], T2.[Lot No], T1.[RM Customs Code], T1.[BGD No], T1.[Customs Balance], T1.[Available RM Balance], " +
                                               "T1.[SharingRM Qty] FROM C_RMBalance AS T1 RIGHT JOIN C_RMPurchase AS T2 ON T1.[BGD No] = T2.[BGD No] AND " +
                                               "T1.[RM Customs Code] = T2.[RM Customs Code] WHERE T2.[Item No] = @ItemNo AND T2.[Lot No] = @LotNo";
                        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                        sqlAdapter.SelectCommand = sqlComm;
                        DataTable dt2 = new DataTable();
                        sqlAdapter.Fill(dt2);
                        sqlAdapter.Dispose();
                        sqlComm.Parameters.Clear();

                        string strRMEHB = dt2.Rows[0]["RM Customs Code"].ToString().Trim();
                        string strBGDNo = dt2.Rows[0]["BGD No"].ToString().Trim();
                        string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dt2.Rows[0]["Customs Balance"].ToString().Trim()));
                        string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dt2.Rows[0]["Available RM Balance"].ToString().Trim()));
                        string strShareRMQty = sqlLib.doubleFormat(Double.Parse(dt2.Rows[0]["SharingRM Qty"].ToString().Trim()));
                        decimal dCustomsBal = Convert.ToDecimal(strCustomsBal);
                        decimal dAvailRMBal = Convert.ToDecimal(strAvailRMBal);
                        decimal dShareOut = Convert.ToDecimal(strShareRMQty);
                        dt2.Dispose();

                        if (String.IsNullOrEmpty(strBeiAnDanDate) && String.IsNullOrEmpty(strCustomsReleaseDate))
                        {
                            if (String.IsNullOrEmpty(strBADDate) && String.IsNullOrEmpty(strCRDate))
                            {
                                if (Decimal.Compare(dNetWeight, dNW) != 0)
                                {
                                    sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut - dNetWeight + dNW;
                                    sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                    sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                    sqlComm.CommandText = @"UPDATE C_RMBalance SET [SharingRM Qty] = @ShareOut WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                    sqlComm.ExecuteNonQuery();
                                    sqlComm.Parameters.Clear();
                                }
                            }
                            else if (!String.IsNullOrEmpty(strBADDate) && String.IsNullOrEmpty(strCRDate))
                            {
                                if (dAvailRMBal - dNW < 0.0M)
                                {
                                    if (MessageBox.Show("The Share-out Qty is greater than Available RM Balance, do you want to continue?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    { goto delObj; }
                                }

                                sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = dAvailRMBal - dNW;
                                sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut - dNetWeight + dNW;
                                sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                sqlComm.CommandText = @"UPDATE C_RMBalance SET [Available RM Balance] = @AvailRMBal, [SharingRM Qty] = @ShareOut " +
                                                       "WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                sqlComm.ExecuteNonQuery();
                                sqlComm.Parameters.Clear();
                            }
                            else if (String.IsNullOrEmpty(strBADDate) && !String.IsNullOrEmpty(strCRDate)) { /*this case can not happen*/ }
                            else
                            {
                                sqlComm.Parameters.Add("@CustomsBal", SqlDbType.Decimal).Value = dCustomsBal - dNW;
                                sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = dAvailRMBal - dNW;
                                sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut - dNetWeight + dNW;
                                sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                sqlComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBal, [Available RM Balance] = @AvailRMBal, " +
                                                       "[SharingRM Qty] = @ShareOut WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                sqlComm.ExecuteNonQuery();
                                sqlComm.Parameters.Clear();
                            }
                        }
                        else if (!String.IsNullOrEmpty(strBeiAnDanDate) && String.IsNullOrEmpty(strCustomsReleaseDate))
                        {
                            if (String.IsNullOrEmpty(strBADDate) && String.IsNullOrEmpty(strCRDate)) { /*this case can not happen*/ }
                            else if (!String.IsNullOrEmpty(strBADDate) && String.IsNullOrEmpty(strCRDate))
                            {
                                if (Decimal.Compare(dNetWeight, dNW) != 0)
                                {
                                    if (dAvailRMBal + dNetWeight - dNW < 0.0M)
                                    {
                                        if (MessageBox.Show("The Share-out Qty is greater than Available RM Balance, do you want to continue?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                        { goto delObj; }
                                    }

                                    sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = dAvailRMBal + dNetWeight - dNW;
                                    sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut - dNetWeight + dNW;
                                    sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                    sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                    sqlComm.CommandText = @"UPDATE C_RMBalance SET [Available RM Balance] = @AvailRMBal, [SharingRM Qty] = @ShareOut " +
                                                           "WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                    sqlComm.ExecuteNonQuery();
                                    sqlComm.Parameters.Clear();
                                }
                            }
                            else if (String.IsNullOrEmpty(strBADDate) && !String.IsNullOrEmpty(strCRDate)) { /*this case can not happen*/ }
                            else
                            {
                                sqlComm.Parameters.Add("@CustomsBal", SqlDbType.Decimal).Value = dCustomsBal - dNW;
                                sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = dAvailRMBal + dNetWeight - dNW;
                                sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut - dNetWeight + dNW;
                                sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                                sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                sqlComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBal, [Available RM Balance] = @AvailRMBal, " +
                                                       "[SharingRM Qty] = @ShareOut WHERE [RM Customs Code] = @RMEHB AND [BGD No] = @BGDNo";
                                sqlComm.ExecuteNonQuery();
                                sqlComm.Parameters.Clear();
                            }
                        }
                        else if (String.IsNullOrEmpty(strBeiAnDanDate) && !String.IsNullOrEmpty(strCustomsReleaseDate)) { /*this case can not happen*/ }
                        else
                        {
                            if (Decimal.Compare(dNetWeight, dNW) != 0)
                            {
                                MessageBox.Show("The data already 2nd released, Net weight must remain the same.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                goto delObj;
                            }
                        }

                        sqlComm.Parameters.Clear();
                        sqlComm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = strCHNName;
                        sqlComm.Parameters.Add("@NetWeight", SqlDbType.Decimal).Value = dNW;
                        sqlComm.Parameters.Add("@BeiAnDanNoOUT", SqlDbType.NVarChar).Value = strBeiAnDanNoOUT;
                        sqlComm.Parameters.Add("@BeiAnDanID", SqlDbType.NVarChar).Value = strBeiAnDanID;
                        if (!String.IsNullOrEmpty(strBADDate)) { sqlComm.Parameters.Add("@BeiAnDanDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strBADDate); }
                        else
                        {
                            if (!String.IsNullOrEmpty(strBeiAnDanDate)) { sqlComm.Parameters.Add("@BeiAnDanDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strBeiAnDanDate); }
                            else { sqlComm.Parameters.Add("@BeiAnDanDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        }
                        if (!String.IsNullOrEmpty(strCRDate)) { sqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCRDate); }
                        else
                        {
                            if (!String.IsNullOrEmpty(strCustomsReleaseDate)) { sqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCustomsReleaseDate); }
                            else { sqlComm.Parameters.Add("@CustomsReleaseDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        }
                        sqlComm.Parameters.Add("@PingDanID", SqlDbType.NVarChar).Value = strPingDanID;
                        sqlComm.Parameters.Add("@PingDanNo", SqlDbType.NVarChar).Value = strPingDanNo;
                        sqlComm.Parameters.Add("@PingDanStatus", SqlDbType.NVarChar).Value = strPingDanStatus;
                        if (!String.IsNullOrEmpty(strPassGateDate)) { sqlComm.Parameters.Add("@PassGateDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strPassGateDate); }
                        else { sqlComm.Parameters.Add("@PassGateDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        sqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                        sqlComm.Parameters.Add("@Type", SqlDbType.NVarChar).Value = strType;
                        sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        sqlComm.Parameters.Add("@BeiAnDanNoIN", SqlDbType.NVarChar).Value = strBeiAnDanNoIN;
                        sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        sqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = dtCreatedDate;

                        sqlComm.CommandText = @"UPDATE C_RMShareOut SET [中文品名] = @RMCHNName, [净重] = @NetWeight, [出境备案单号] = @BeiAnDanNoOUT, [备案ID] = @BeiAnDanID, " +
                                               "[备案时间] = @BeiAnDanDate, [放行日期审核时间] = @CustomsReleaseDate, [凭单ID] = @PingDanID, [出区凭单] = @PingDanNo, " +
                                               "[凭单状态] = @PingDanStatus, [卡口过机日期] = @PassGateDate, [Remark] = @Remark WHERE [类型] = @Type AND " +
                                               "[原料备件号] = @RMCustomsCode AND [进境备案单号] = @BeiAnDanNoIN AND [Item No] = @ItemNo AND [Lot No] = @LotNo AND " +
                                               "[Created Date] = @CreatedDate";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();

                        this.SetReadOnly("readonly");
                        this.dgvRMShareOut["中文品名", iRowIndex].Value = strCHNName;
                        this.dgvRMShareOut["净重", iRowIndex].Value = dNW;
                        this.dgvRMShareOut["出境备案单号", iRowIndex].Value = strBeiAnDanNoOUT;
                        this.dgvRMShareOut["备案ID", iRowIndex].Value = strBeiAnDanID;
                        if (!String.IsNullOrEmpty(strBADDate)) { this.dgvRMShareOut["备案时间", iRowIndex].Value = Convert.ToDateTime(strBADDate); }
                        if (!String.IsNullOrEmpty(strCRDate)) { this.dgvRMShareOut["放行日期审核时间", iRowIndex].Value = Convert.ToDateTime(strCRDate); }
                        this.dgvRMShareOut["凭单ID", iRowIndex].Value = strPingDanID;
                        this.dgvRMShareOut["出区凭单", iRowIndex].Value = strPingDanNo;
                        this.dgvRMShareOut["凭单状态", iRowIndex].Value = strPingDanStatus;
                        if (!String.IsNullOrEmpty(strPassGateDate)) { this.dgvRMShareOut["卡口过机日期", iRowIndex].Value = Convert.ToDateTime(strPassGateDate); }
                        this.dgvRMShareOut["Remark", iRowIndex].Value = strRemark;

                        MessageBox.Show("Successfully edit the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        strSwitch = String.Empty;
                    }
                }

                else /*if (e.Button.ToolTipText == "Delete")*/
                {
                    if (String.IsNullOrEmpty(this.txtRMCustomsCode.Text.Trim()))
                    {
                        MessageBox.Show("Please select to delete the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

                    string strCustomsReleaseDate = this.dtpCustomsReleaseDate.Text.Trim();
                    if (!String.IsNullOrEmpty(strCustomsReleaseDate))
                    {
                        MessageBox.Show("The data already 2nd released, system rejects to delete it.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.txtItemNo.Text.Trim();
                    sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.txtLotNo.Text.Trim();
                    sqlComm.CommandText = @"SELECT T2.[Item No], T2.[Lot No], T1.[RM Customs Code], T1.[BGD No], T1.[Available RM Balance], " +
                                           "T1.[SharingRM Qty] FROM C_RMBalance AS T1 RIGHT JOIN C_RMPurchase AS T2 ON T1.[BGD No] = T2.[BGD No] AND " +
                                           "T1.[RM Customs Code] = T2.[RM Customs Code] WHERE T2.[Item No] = @ItemNo AND T2.[Lot No] = @LotNo";
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                    sqlAdapter.SelectCommand = sqlComm;
                    DataTable dt3 = new DataTable();
                    sqlAdapter.Fill(dt3);
                    sqlAdapter.Dispose();
                    sqlComm.Parameters.Clear();

                    string strRMCustomsCode = dt3.Rows[0]["RM Customs Code"].ToString().Trim();
                    string strBGDNo = dt3.Rows[0]["BGD No"].ToString().Trim();
                    string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dt3.Rows[0]["Available RM Balance"].ToString().Trim()));
                    string strShareRMQty = sqlLib.doubleFormat(Double.Parse(dt3.Rows[0]["SharingRM Qty"].ToString().Trim()));
                    decimal dAvailRMBal = Convert.ToDecimal(strAvailRMBal);
                    decimal dShareOut = Convert.ToDecimal(strShareRMQty);
                    decimal dNetWeight = Math.Round(Convert.ToDecimal(this.txtNetWeight.Text.Trim()), 6);
                    if (String.IsNullOrEmpty(this.dtpBeiAnDanDate.Text.Trim()))
                    {
                        sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut - dNetWeight;
                        sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        sqlComm.CommandText = @"UPDATE C_RMBalance SET [SharingRM Qty] = @ShareOut WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }
                    else
                    {
                        sqlComm.Parameters.Add("@AvailRMBal", SqlDbType.Decimal).Value = dAvailRMBal + dNetWeight;
                        sqlComm.Parameters.Add("@ShareOut", SqlDbType.Decimal).Value = dShareOut - dNetWeight;
                        sqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        sqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        sqlComm.CommandText = @"UPDATE C_RMBalance SET [Available RM Balance] = @AvailRMBal, [SharingRM Qty] = @ShareOut " +
                                               "WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }

                    sqlComm.Parameters.Clear();
                    sqlComm.Parameters.Add("@Type", SqlDbType.NVarChar).Value = this.cmbType.Text.Trim();
                    sqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = this.txtRMCustomsCode.Text.Trim();
                    sqlComm.Parameters.Add("@BeiAnDanNoIN", SqlDbType.NVarChar).Value = this.txtBeiAnDanNoIN.Text.Trim();
                    sqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.txtItemNo.Text.Trim();
                    sqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.txtLotNo.Text.Trim();
                    sqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dtpCreatedDate.Text.ToString().Trim());
                    sqlComm.CommandText = @"DELETE FROM C_RMShareOut WHERE [类型] = @Type AND [原料备件号] = @RMEHB AND [进境备案单号] = @BeiAnDanNoIN " +
                                           "AND [Item No] = @ItemNo AND [Lot No] = @LotNo AND [Created Date] = @CreatedDate";
                    sqlComm.ExecuteNonQuery();
                    sqlComm.Parameters.Clear();

                    this.dgvRMShareOut.Rows.RemoveAt(iIndexRow); /*this.dgvRMShareOut.CurrentRow.Index*/
                    this.SetBlank();
                    this.SetReadOnly("readonly");
                    MessageBox.Show("Successfully delete the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt3.Dispose();                  
                }
            delObj:
                sqlComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                sqlComm.ExecuteNonQuery();
                sqlComm.Dispose();
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }
            }

            if (e.Button.ToolTipText == "Cancel")
            {
                if (String.Compare(strSwitch, "Add") == 0)
                {
                    this.SetBlank();
                    this.SetReadOnly("readonly");
                }
                if (String.Compare(strSwitch, "Edit") == 0)
                {
                    this.GetRowData();
                    this.SetReadOnly("readonly");
                }
                strSwitch = String.Empty;
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvRMShareOut.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMShareOut.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMShareOut.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMShareOut[strColumnName, this.dgvRMShareOut.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] = " + strColumnText; }
                    }
                }
                rmView.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiExcludeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvRMShareOut.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMShareOut.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMShareOut.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMShareOut[strColumnName, this.dgvRMShareOut.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMShareOut.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] <> " + strColumnText; }
                    }
                }
                rmView.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            rmView.RowFilter = "";
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvRMShareOut.CurrentCell != null)
            {
                string strColumnName = this.dgvRMShareOut.Columns[this.dgvRMShareOut.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter);
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = rmView.ToTable(true, new string[] { strColumnName });
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
                        if (this.dgvRMShareOut.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvRMShareOut.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvRMShareOut.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvRMShareOut.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvRMShareOut.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvRMShareOut.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                rmView.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
