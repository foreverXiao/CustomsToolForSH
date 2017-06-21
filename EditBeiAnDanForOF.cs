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
    public partial class EditBeiAnDanForOF : Form
    {
        private LoginForm loginFrm = new LoginForm();
        DataTable dt = new DataTable();
        DataTable dtName = new DataTable();
        protected DataView dvFillDGV = new DataView();
        string strFilter = null;
        protected PopUpFilterForm filterFrm = null;
        int iGongDanStatus = 0;

        private static EditBeiAnDanForOF getEditionBeiAnDanFrm;
        public EditBeiAnDanForOF()
        {
            InitializeComponent();
        }
        public static EditBeiAnDanForOF CreateInstance()
        {
            if (getEditionBeiAnDanFrm == null || getEditionBeiAnDanFrm.IsDisposed)
            {
                getEditionBeiAnDanFrm = new EditBeiAnDanForOF();
            }
            return getEditionBeiAnDanFrm;
        }

        private void EditBeiAnDanForOF_Load(object sender, EventArgs e)
        {
            SqlLib sqlLib = new SqlLib();
            dt = sqlLib.GetDataTable(@"SELECT * FROM B_IEType", "B_IEType").Copy();
            DataRow dr = dt.NewRow();
            dr["IE Type"] = String.Empty;
            dt.Rows.InsertAt(dr, 0);
            this.cmbIEtype.DisplayMember = this.cmbIEtype.ValueMember = "IE Type";
            this.cmbIEtype.DataSource = dt;
            sqlLib.Dispose();

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;
            string strUserName = loginFrm.PublicUserName;
            SqlComm.Parameters.Add("@LoginName", SqlDbType.NVarChar).Value = strUserName;
            SqlComm.CommandText = @"SELECT [Group] FROM B_UserInfo WHERE [LoginName] = @LoginName";
            string strGroup = Convert.ToString(SqlComm.ExecuteScalar()).Trim().ToUpper();
            if (String.Compare(strGroup, "ADMIN") == 0) { this.rbtn3.Enabled = true; }
            SqlComm.Dispose();
            SqlConn.Close();
        }

        private void EditBeiAnDanForOF_FormClosing(object sender, FormClosingEventArgs e)
        {
            dt.Dispose();
            dtName.Dispose();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strIEType = this.cmbIEtype.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strIEType))
            {
                MessageBox.Show("Please select IE Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            strFilter = "";
            dvFillDGV.RowFilter = "";
            SqlConnection browseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (browseConn.State == ConnectionState.Closed) { browseConn.Open(); }

            string strSQL = null;
            if (String.Compare(strIEType, "EXPORT") == 0)
            { strSQL = @"SELECT [Pass to IE Date], [GongDan Approved Date] AS [GD Approved Date], [ESS/LINE], [IE Type], [GongDan No], [FG EHB], [CHN Name], " +
                        "[Destination], [Order No], [Order Category], [Total Ship Qty], [GongDan Qty], [Local Total RM Cost], [Selling Amount], [IE Rev Amt], " +
                        "[Customs Total RM Cost], [OF Rev Amt], '' AS [BeiAnDan ID], '' AS [BeiAnDan No], '' AS [Tax & Duty Paid Date], [OF Remark], [IE Remark] " +
                        "FROM M_PendingBeiAnDan_Export WHERE [IE Type] = 'EXPORT' ORDER BY [GongDan No]"; 
            }
            else if (String.Compare(strIEType, "RM-D") == 0)
            {
                strSQL = @"SELECT [Created Date] AS [Pass to IE Date], [Approved Date] AS [GD Approved Date], [ESS/LINE], [IE Type], [GongDan No], " +
                          "CASE WHEN [BOM In Customs] IS NULL or [BOM In Customs] = '' THEN [FG No] + '/' + [Batch No] ELSE [BOM In Customs] END AS [FG EHB], " + 
                          "[CHN Name], [Destination], [Order No], [Order Category], [Total Ship Qty], [GongDan Qty], [Total RM Cost(USD)] AS [Local Total RM Cost], " +
                          "CAST([GongDan Qty] * [Order Price] as decimal(18, 2)) AS [Selling Amount], 0.0 AS [IE Rev Amt], 0.0 AS [Customs Total RM Cost], " + 
                          "0.0 AS [OF Rev Amt], '' AS [BeiAnDan ID], '' AS [BeiAnDan No], '' AS [Tax & Duty Paid Date], '' AS [OF Remark], '' AS [IE Remark] " + 
                          "FROM C_GongDan WHERE [IE Type] = '" + strIEType + "' AND ([BeiAnDan Used Qty] = 0.0 OR [BeiAnDan Used Qty] IS NULL)";
            }
            else
            {
                strSQL = @"SELECT [Pass to IE Date], [GongDan Approved Date] AS [GD Approved Date], [ESS/LINE], [IE Type], [GongDan No], [FG EHB], [CHN Name], " +
                          "[Destination], [Order No], [Order Category], [Total Ship Qty], [GongDan Qty], [Local Total RM Cost], [Selling Amount], [IE Rev Amt], " +
                          "[Customs Total RM Cost], [OF Rev Amt], '' AS [BeiAnDan ID], '' AS [BeiAnDan No], '' AS [Tax & Duty Paid Date], [OF Remark], [IE Remark] " +
                          "FROM M_DailyBeiAnDan WHERE [IE Type] = '" + strIEType + "' ORDER BY [IE Type], [GongDan No]";
            }

            SqlDataAdapter browseAdapter = new SqlDataAdapter(strSQL, browseConn);
            DataTable myTable = new DataTable();
            browseAdapter.Fill(myTable);
            browseAdapter.Dispose();

            dtName.Clear();
            if (!String.IsNullOrEmpty(this.cmbIEtype.Text.Trim()))
            {
                DataRow[] dRow = myTable.Select("[IE Type] = '" + this.cmbIEtype.Text.Trim().ToUpper() + "'");
                dtName = myTable.Clone();
                foreach (DataRow dr in dRow) { dtName.ImportRow(dr); }
            }
            else { dtName = myTable.Copy(); }
            myTable.Dispose();

            if (browseConn.State == ConnectionState.Open)
            {
                browseConn.Close();
                browseConn.Dispose();
            }
            if (dtName.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDan.DataSource = DBNull.Value;
            }
            else 
            {
                dvFillDGV = dtName.DefaultView;
                this.dgvBeiAnDan.DataSource = dvFillDGV;
                this.dgvBeiAnDan.Columns["GongDan No"].Frozen = true;
                for (int i = 1; i < this.dgvBeiAnDan.ColumnCount; i++) { this.dgvBeiAnDan.Columns[i].ReadOnly = true; }
                this.dgvBeiAnDan.Columns["OF Rev Amt"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["OF Remark"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["IE Rev Amt"].DefaultCellStyle.ForeColor = Color.Red;
                this.dgvBeiAnDan.Columns["Customs Total RM Cost"].DefaultCellStyle.ForeColor = Color.Red;
                this.dgvBeiAnDan.Columns["IE Remark"].DefaultCellStyle.ForeColor = Color.Red;

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvBeiAnDan.EnableHeadersVisualStyles = false;
                this.dgvBeiAnDan.Columns["OF Rev Amt"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["OF Remark"].HeaderCell.Style = cellStyle;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.dgvBeiAnDan.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvBeiAnDan.RowCount + 1, this.dgvBeiAnDan.ColumnCount - 1]).NumberFormatLocal = "@";
            for (int x = 0; x < this.dgvBeiAnDan.RowCount; x++)
            {
                for (int y = 1; y < this.dgvBeiAnDan.ColumnCount; y++)
                { excel.Cells[x + 2, y] = this.dgvBeiAnDan[y, x].Value.ToString().Trim(); }
            }

            for (int z = 1; z < this.dgvBeiAnDan.ColumnCount; z++)
            { excel.Cells[1, z] = this.dgvBeiAnDan.Columns[z].HeaderText.ToString().Trim(); }

            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Bold = true;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvBeiAnDan.RowCount + 1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Name = "Verdana";
            excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvBeiAnDan.RowCount + 1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Size = 9;
            excel.Cells.EntireColumn.AutoFit();
            excel.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }

        private void dgvBeiAnDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (MessageBox.Show("Are you sure to update the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

                int iRowIndex = this.dgvBeiAnDan.CurrentRow.Index;
                string strBeiAnDanID = this.dgvBeiAnDan["BeiAnDan ID", iRowIndex].Value.ToString().Trim();
                if (!String.IsNullOrEmpty(strBeiAnDanID))
                {
                    MessageBox.Show("Reject to do, because already generated BeiAnDan ID.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                string strOFRevAmt = this.dgvBeiAnDan["OF Rev Amt", iRowIndex].Value.ToString().Trim();
                decimal dOFRevAmt = 0.0M;
                if (!String.IsNullOrEmpty(strOFRevAmt)) { dOFRevAmt = Math.Round(Convert.ToDecimal(strOFRevAmt), 2); }
                string strOFRemark = this.dgvBeiAnDan["OF Remark", iRowIndex].Value.ToString().Trim().ToUpper();
                string strGongDanNo = this.dgvBeiAnDan["GongDan No", iRowIndex].Value.ToString().Trim();

                SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;

                string strIEType = this.cmbIEtype.Text.Trim().ToUpper();
                sqlComm.Parameters.Add("@OFRevAmt", SqlDbType.Decimal).Value = dOFRevAmt;
                sqlComm.Parameters.Add("@OFRemark", SqlDbType.NVarChar).Value = strOFRemark;
                sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                if (String.Compare(strIEType, "EXPORT") == 0)
                { sqlComm.CommandText = @"UPDATE M_PendingBeiAnDan_Export SET [OF Rev Amt] = @OFRevAmt, [OF Remark] = @OFRemark WHERE [GongDan No] = @GongDanNo"; }
                else { sqlComm.CommandText = @"UPDATE M_DailyBeiAnDan SET [OF Rev Amt] = @OFRevAmt, [OF Remark] = @OFRemark WHERE [GongDan No] = @GongDanNo"; }
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();

                sqlComm.Dispose();
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }
                this.dgvBeiAnDan["OF Rev Amt", iRowIndex].Value = dOFRevAmt;
                this.dgvBeiAnDan["OF Remark", iRowIndex].Value = strOFRemark;
                dtName.AcceptChanges();
                MessageBox.Show("Successfully update.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBeiAnDan.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBeiAnDan.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBeiAnDan.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBeiAnDan[strColumnName, this.dgvBeiAnDan.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
                if (this.dgvBeiAnDan.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBeiAnDan.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBeiAnDan.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBeiAnDan[strColumnName, this.dgvBeiAnDan.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBeiAnDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
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

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDan.CurrentCell != null)
            {
                string strColumnName = this.dgvBeiAnDan.Columns[this.dgvBeiAnDan.CurrentCell.ColumnIndex].Name;
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
                        if (this.dgvBeiAnDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBeiAnDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBeiAnDan.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvBeiAnDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBeiAnDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBeiAnDan.Columns[strColumnName].ValueType == typeof(DateTime))
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

        private void btnDownloadDoc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

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
            downloadComm.CommandText = @"usp_QueryDataForOF";
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
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\QueryDataStatusforOF" + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < myName.Columns.Count; n++)
                        { sb.Append(myName.Columns[n].ColumnName.Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < myName.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < myName.Columns.Count; j++)
                            {
                                if (j == 21) { sb.Append("'" + myName.Rows[i][j].ToString().Trim() + "\t"); }
                                else { sb.Append(myName.Rows[i][j].ToString().Trim() + "\t"); }
                            }
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxShow.Visible == false)
            { 
                this.gBoxShow.Visible = true;
                this.txtGongDanNo.Text = string.Empty;
                this.txtRemark.Text = string.Empty;

                iGongDanStatus = 0;
                this.txtGDNO.Text = string.Empty;
                this.txtRMK.Text = string.Empty;
                this.txtOldEssLine.Text = string.Empty;
                this.txtNewEssLine.Text = string.Empty;
                this.txtOrderNo.Text = string.Empty;
            }
            else
            { this.gBoxShow.Visible = false; }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtGongDanNo.Text.Trim()))
            {
                MessageBox.Show("Please input GongDan No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtGongDanNo.Focus();
                return;
            }
            if (String.IsNullOrEmpty(this.txtRemark.Text.Trim()))
            {
                MessageBox.Show("Please input Remark information.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtRemark.Focus();
                return;
            }
            if (MessageBox.Show("Are you sure to update the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            SqlConnection updateConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (updateConn.State == ConnectionState.Closed) { updateConn.Open(); }
            SqlCommand updateComm = new SqlCommand();
            updateComm.Connection = updateConn;

            updateComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.txtGongDanNo.Text.Trim().ToUpper();
            updateComm.CommandText = @"SELECT COUNT(*) FROM C_BeiAnDan WHERE [GongDan No] = @GongDanNo";
            int icount = Convert.ToInt32(updateComm.ExecuteScalar());
            if (icount == 0)
            {
                MessageBox.Show("There is no this GongDan in BeiAnDan table, please input it again.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                updateComm.Dispose();
                updateConn.Close();
                updateConn.Dispose();
                this.txtGongDanNo.Focus();
                return;
            }

            updateComm.Parameters.Clear();
            updateComm.Parameters.Add("@OFRemark2", SqlDbType.NVarChar).Value = this.txtRemark.Text.Trim();
            updateComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.txtGongDanNo.Text.Trim().ToUpper();
            updateComm.CommandText = @"UPDATE C_BeiAnDan SET [OF Remark_2] = @OFRemark2 WHERE [GongDan No] = @GongDanNo";
            updateComm.ExecuteNonQuery();
            updateComm.Parameters.Clear();
            updateComm.Dispose();
            if (updateConn.State == ConnectionState.Open)
            {
                updateConn.Close();
                updateConn.Dispose();
            }
            MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnQR_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtGDNO.Text.Trim()))
            {
                MessageBox.Show("Please input GongDan No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtGDNO.Focus();
                return;
            }

            SqlConnection queryConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (queryConn.State == ConnectionState.Closed) { queryConn.Open(); }
            SqlCommand queryComm = new SqlCommand();
            queryComm.Connection = queryConn;

            queryComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.txtGDNO.Text.Trim().ToUpper();
            queryComm.CommandText = @"SELECT COUNT(*) FROM C_GongDan WHERE [GongDan No] = @GongDanNo";
            int icount = Convert.ToInt32(queryComm.ExecuteScalar());
            if (icount > 0)
            {
                iGongDanStatus = 1;
                queryComm.CommandText = @"SELECT [IE Type] FROM C_GongDan WHERE [GongDan No] = @GongDanNo";
                string strIEType = Convert.ToString(queryComm.ExecuteScalar());
                if (String.Compare(strIEType, "EXPORT") != 0) { queryComm.CommandText = @"SELECT COUNT(*) FROM M_DailyBeiAnDan WHERE [GongDan No] = @GongDanNo"; }
                else { queryComm.CommandText = @"SELECT COUNT(*) FROM M_PendingBeiAnDan_Export WHERE [GongDan No] = @GongDanNo"; }
                icount = Convert.ToInt32(queryComm.ExecuteScalar());
                if (icount > 0) { iGongDanStatus = 2; }
                else 
                {
                    queryComm.CommandText = @"SELECT COUNT(*) FROM C_BeiAnDan WHERE [GongDan No] = @GongDanNo";
                    icount = Convert.ToInt32(queryComm.ExecuteScalar());
                    if (icount > 0)
                    {
                        iGongDanStatus = 3;
                        queryComm.CommandText = @"SELECT COUNT(*) FROM C_PingDan WHERE [GongDan No] = @GongDanNo";
                        icount = Convert.ToInt32(queryComm.ExecuteScalar());
                        if (icount > 0) { iGongDanStatus = 4; }
                    }
                }
            }
            else { iGongDanStatus = 0; }

            queryComm.Parameters.Clear();
            queryComm.Dispose();
            if (queryConn.State == ConnectionState.Open)
            {
                queryConn.Close();
                queryConn.Dispose();
            }

            if (iGongDanStatus == 1) { this.txtRMK.Text = "This GongDan No status: just only generated GongDan, so user can update the related data."; }
            else if (iGongDanStatus == 2) { this.txtRMK.Text = "This GongDan No status: already generated BeiAnDan, but donot approve, so user cannot perform the update."; }
            else if (iGongDanStatus == 3) { this.txtRMK.Text = "This GongDan No status: existed in BeiAnDan historical data, so user can update the related data."; }
            else { this.txtRMK.Text = "This GongDan No status: already generated PingDan, so user can update the related data."; }
        }

        private void btnUPD_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtGDNO.Text.Trim()))
            {
                MessageBox.Show("Please input GongDan No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtGDNO.Focus();
                return;
            }

            if (iGongDanStatus == 0)
            {
                MessageBox.Show("Please click 'query' button to check the GongDan status.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnQR.Focus();
                return;
            }

            if (iGongDanStatus == 2)
            {
                MessageBox.Show("System rejects to do the update.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (String.IsNullOrEmpty(this.txtOldEssLine.Text.Trim()) || String.IsNullOrEmpty(this.txtNewEssLine.Text.Trim()) || String.IsNullOrEmpty(this.txtOrderNo.Text.Trim()))
            {
                MessageBox.Show("PLease input old ESS/LINE, new ESS/LINE and Order No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtOldEssLine.Focus();
                return;
            }

            SqlConnection updateConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (updateConn.State == ConnectionState.Closed) { updateConn.Open(); }
            SqlCommand updateComm = new SqlCommand();
            updateComm.Connection = updateConn;
            updateComm.Parameters.Add("@EssLine", SqlDbType.NVarChar).Value = this.txtNewEssLine.Text.Trim().ToUpper();
            updateComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = this.txtOrderNo.Text.Trim().ToUpper();
            updateComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.txtGDNO.Text.Trim().ToUpper();
            if (iGongDanStatus == 1) 
            { 
                updateComm.CommandText = @"UPDATE C_GongDan SET [ESS/LINE] = @EssLine, [Order No] = @OrderNo WHERE [GongDan No] = @GongDanNo";
                updateComm.ExecuteNonQuery();
            }
            else if (iGongDanStatus == 3)
            {
                updateComm.CommandText = @"UPDATE C_GongDan SET [ESS/LINE] = @EssLine, [Order No] = @OrderNo WHERE [GongDan No] = @GongDanNo";
                updateComm.ExecuteNonQuery();
                updateComm.CommandText = @"UPDATE C_BeiAnDan SET [ESS/LINE] = @EssLine, [Order No] = @OrderNo WHERE [GongDan No] = @GongDanNo";
                updateComm.ExecuteNonQuery();
            }
            else
            {
                updateComm.CommandText = @"UPDATE C_GongDan SET [ESS/LINE] = @EssLine, [Order No] = @OrderNo WHERE [GongDan No] = @GongDanNo";
                updateComm.ExecuteNonQuery();
                updateComm.CommandText = @"UPDATE C_BeiAnDan SET [ESS/LINE] = @EssLine, [Order No] = @OrderNo WHERE [GongDan No] = @GongDanNo";
                updateComm.ExecuteNonQuery();
                updateComm.CommandText = @"UPDATE C_PingDan SET [ESS/LINE] = @EssLine, [Order No] = @OrderNo WHERE [GongDan No] = @GongDanNo";
                updateComm.ExecuteNonQuery();
            }
            updateComm.Parameters.Clear();
            updateComm.CommandType = CommandType.StoredProcedure;
            updateComm.CommandText = @"usp_UpdateOrderFulfillment";
            updateComm.Parameters.AddWithValue("@GongDanNo", this.txtGDNO.Text.Trim().ToUpper());
            updateComm.Parameters.AddWithValue("@OldEssLine", this.txtOldEssLine.Text.Trim().ToUpper());
            updateComm.Parameters.AddWithValue("@NewEssLine", this.txtNewEssLine.Text.Trim().ToUpper());
            updateComm.Parameters.AddWithValue("@OrderNo", this.txtOrderNo.Text.Trim().ToUpper());
            updateComm.ExecuteNonQuery();
            updateComm.Parameters.Clear();
            updateComm.Dispose();
            if (updateConn.State == ConnectionState.Open)
            {
                updateConn.Close();
                updateConn.Dispose();
            }
            MessageBox.Show("Update the related data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdateOC_Click(object sender, EventArgs e)
        {
            string strGongDanNo = this.txtGDNO.Text.Trim();
            string strOrderCategory = this.cmbOrderCategory.SelectedItem.ToString().Trim();
            if (String.IsNullOrEmpty(strGongDanNo)) { MessageBox.Show("Please input Order No first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection connSQL = new SqlConnection(SqlLib.StrSqlConnection);
            connSQL.Open();
            SqlCommand commSQL = new SqlCommand();
            commSQL.Connection = connSQL;
            commSQL.CommandText = "UPDATE C_GongDan SET [Order Category] = '" + strOrderCategory + "' WHERE [Order No] = '" + strGongDanNo + "'";
            commSQL.ExecuteNonQuery();
            commSQL.CommandText = "UPDATE C_BeiAnDan SET [Order Category] = '" + strOrderCategory + "' WHERE [Order No] = '" + strGongDanNo + "'";
            commSQL.ExecuteNonQuery();
            commSQL.Dispose();
            connSQL.Close();
            connSQL.Dispose();
            MessageBox.Show("Update the related data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
