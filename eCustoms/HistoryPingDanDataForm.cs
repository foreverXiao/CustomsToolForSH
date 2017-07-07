using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class HistoryPingDanDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataView dvFillPD = new DataView();
        private DataTable dtFillPD = new DataTable();
        private DataTable dtIE = new DataTable();
        private DataTable dtGroupID = new DataTable();
        string strFilter = null;
        private DataGridView dgvDetailsPD = new DataGridView();
        protected PopUpFilterForm filterFrm = null;
        private static HistoryPingDanDataForm HistoryPingDanDataFrm;
        public HistoryPingDanDataForm() { InitializeComponent(); }
        public static HistoryPingDanDataForm CreateInstance()
        {
            if (HistoryPingDanDataFrm == null || HistoryPingDanDataFrm.IsDisposed) { HistoryPingDanDataFrm = new HistoryPingDanDataForm(); }
            return HistoryPingDanDataFrm;
        }

        private void HistoryPingDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";
            this.dtpGateOutTime.CustomFormat = " ";

            this.btnUpdate.Enabled = false;
            this.btnDelete.Enabled = false;
        }

        private void HistoryPingDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtFillPD.Dispose();
            dtIE.Dispose();
            dtGroupID.Dispose();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e) { this.dtpFrom.CustomFormat = null; }
        private void dtpFrom_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; } }
        private void dtpTo_ValueChanged(object sender, EventArgs e) { this.dtpTo.CustomFormat = null; }
        private void dtpTo_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; } }
        private void dtpGateOutTime_ValueChanged(object sender, EventArgs e) { this.dtpGateOutTime.CustomFormat = null; }
        private void dtpGateOutTime_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpGateOutTime.CustomFormat = " "; } }

        private void cmbIEType_Enter(object sender, EventArgs e)
        {
            if (dtIE.Rows.Count == 0)
            {
                SqlLib sqlLib = new SqlLib();
                dtIE = sqlLib.GetDataTable(@"SELECT [ObjectValue] AS [IE Type] FROM B_SysInfo WHERE [ObjectName] = 'IE Type' AND [ObjectValue] <> 'RM-D'").Copy();
                DataRow dr = dtIE.NewRow();
                dr["IE Type"] = String.Empty;
                dtIE.Rows.InsertAt(dr, 0);
                sqlLib.Dispose();
            }
            this.cmbIEType.DisplayMember = this.cmbIEType.ValueMember = "IE Type";
            this.cmbIEType.DataSource = dtIE;
        }

        private void cmbGroupID_Enter(object sender, EventArgs e)
        {
            string strIEType = this.cmbIEType.Text.Trim().ToUpper();
            if (!String.IsNullOrEmpty(strIEType))
            {
                string strSQL = @"SELECT DISTINCT [Group ID] FROM C_PingDan WHERE [IE Type] = '" + strIEType + "' ";
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim())) { strSQL += " AND [PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
                if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim())) { strSQL += " AND [PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1).ToString("M/d/yyyy")) + "'"; }
                if (this.cbPingDanID.Checked == true) { strSQL += " AND ([PingDan ID] IS NULL OR [PingDan ID] = '')"; }
                if (this.cbPingDanNo.Checked == true) { strSQL += " AND ([PingDan No] IS NULL OR [PingDan No] = '')"; }
                if (this.cbGateOutTime.Checked == true) { strSQL += " AND [Gate Out Time] IS NULL"; }

                SqlLib SqlLib = new SqlLib();
                dtGroupID.Rows.Clear();
                dtGroupID.Columns.Clear();
                dtGroupID = SqlLib.GetDataTable(strSQL, "C_PingDan").Copy();
                DataRow dr = dtGroupID.NewRow();
                dr["Group ID"] = String.Empty;
                dtGroupID.Rows.InsertAt(dr, 0);
                this.cmbGroupID.DisplayMember = this.cmbGroupID.ValueMember = "Group ID";
                this.cmbGroupID.DataSource = dtGroupID;
                SqlLib.Dispose();
            }
        }

        private void cmbGroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbGroupID.Text.Trim()))
            {
                this.btnUpdate.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else
            {
                this.btnUpdate.Enabled = true;
                this.btnDelete.Enabled = true;
            }
        }

        private void btnGeneratePingDan_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEType.Text.Trim()))
            { MessageBox.Show("Please select I/E Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            strFilter = "";
            dvFillPD.RowFilter = "";
            dtFillPD.Rows.Clear();
            dtFillPD.Columns.Clear();
            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;

            string strIEType = this.cmbIEType.Text.Trim().ToUpper();
            DateTime datetime = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

            sqlComm.Parameters.AddWithValue("@IEType", strIEType);
            sqlComm.Parameters.AddWithValue("@Creater", loginFrm.PublicUserName);
            sqlComm.Parameters.AddWithValue("@PingDanDate", datetime);
            sqlComm.CommandText = @"usp_GetPingDan";
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            DataTable mytable = new DataTable();
            sqlAdapter.Fill(mytable);
            sqlComm.Parameters.Clear();

            if (mytable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mytable.Dispose();
                sqlAdapter.Dispose();
                sqlComm.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
                this.dgvPingDan.DataSource = DBNull.Value;
                return;
            }

            string strGroupID = this.GetPingDanGroupID(strIEType, sqlConn, sqlComm);
            mytable.Columns.Add("Group ID", typeof(string));
            mytable.Columns["Group ID"].SetOrdinal(12);

            int iSerial = 0, iCount = 1;
            string strBeiAnDanID = null;
            foreach (DataRow dr in mytable.Rows)
            {
                string strBADID = dr["BeiAnDan ID"].ToString().Trim();
                if (String.Compare(strBeiAnDanID, strBADID) != 0) { strBeiAnDanID = strBADID; iSerial++; iCount = 1; }
                else
                {
                    if (String.Compare(strIEType, "1418") == 0 || String.Compare(strIEType, "RMB-1418") == 0 || String.Compare(strIEType, "RMB-D") == 0)
                    {
                        if (iCount == 12) { iCount = 1; iSerial++; } //According to Customs requirement to set the parameter
                        else { iCount++; }
                    }
                }
                dr["Group ID"] = this.GetASCGroupID(iSerial, strGroupID, strIEType);
            }
            mytable.AcceptChanges();
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_InsertPingDan";
            sqlComm.Parameters.AddWithValue("@IEType", strIEType);
            sqlComm.Parameters.AddWithValue("@PingDanDate", datetime);
            sqlComm.Parameters.AddWithValue("@tvp_PD", mytable);

            sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            dtFillPD.Rows.Clear();
            dtFillPD.Columns.Clear();
            sqlAdapter.Fill(dtFillPD);
            sqlAdapter.Dispose();
            sqlComm.Parameters.Clear();
            sqlComm.Dispose();
            mytable.Dispose();                       
            if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); sqlConn.Dispose(); }

            dvFillPD = dtFillPD.DefaultView;
            this.dgvPingDan.DataSource = dvFillPD;
            this.dgvPingDan.Columns["IE Type"].Visible = false;
            this.dgvPingDan.Columns["PingDan Amount"].Visible = false;
            for (int i = 0; i < this.dgvPingDan.ColumnCount; i++)
            { this.dgvPingDan.Columns[i].ReadOnly = true; }
            this.dgvPingDan.Columns["GongDan No"].Frozen = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillPD.RowFilter = "";
            if (String.IsNullOrEmpty(this.cmbIEType.Text.Trim()))
            { MessageBox.Show("Please select I/E Type before preview.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string strIEType = this.cmbIEType.Text.Trim().ToUpper();
            string strBrowse = " [IE Type] = '" + strIEType + "'";
            if (this.cbPingDanID.Checked == true) { strBrowse += " AND ([PingDan ID] IS NULL OR [PingDan ID] = '')"; }
            if (this.cbPingDanNo.Checked == true) { strBrowse += " AND ([PingDan No] IS NULL OR [PingDan No] = '')"; }
            if (this.cbGateOutTime.Checked == true) { strBrowse += " AND ([Gate Out Time] IS NULL OR [Gate Out Time] = '')"; }
            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1)
                    { MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    else { strBrowse += " AND [PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else { strBrowse += " AND [PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strBrowse += " AND [PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }
            if (!String.IsNullOrEmpty(this.cmbGroupID.Text.Trim())) { strBrowse += " AND [Group ID] = '" + this.cmbGroupID.Text.Trim().ToUpper() + "'"; }         
            string strSQL = @"SELECT * FROM C_PingDan WHERE" + strBrowse + " ORDER BY [BeiAnDan ID], [GongDan No]"; 

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
            dtFillPD.Columns.Clear();
            dtFillPD.Rows.Clear();
            sqlAdapter.Fill(dtFillPD);
            sqlAdapter.Dispose();          
            if (dtFillPD.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvPingDan.DataSource = DBNull.Value;
            }
            else
            {               
                dvFillPD = dtFillPD.DefaultView;
                this.dgvPingDan.DataSource = dvFillPD;
                this.dgvPingDan.Columns["GongDan No"].Frozen = true;
                this.dgvPingDan.Columns["IE Type"].Visible = false;
                this.dgvPingDan.Columns["PingDan Amount"].Visible = false;                
                for (int i = 0; i < this.dgvPingDan.ColumnCount; i++)
                { this.dgvPingDan.Columns[i].ReadOnly = true; }              
            }
            if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); sqlConn.Dispose(); }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            { MessageBox.Show("There is no data in below data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            decimal dRatio = 0.0M;
            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = "SELECT [ObjectValue] AS [Ratio] FROM B_SysInfo WHERE [ObjectName] = 'WeightRatio'";
            dRatio = Convert.ToDecimal(sqlComm.ExecuteScalar().ToString().Trim());
            sqlComm.Dispose();          

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
             
           // worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 7]).NumberFormatLocal = "@";
            worksheet.Cells[1, 1] = "Line No";
            worksheet.Cells[1, 2] = "BeiAnDan ID";
            worksheet.Cells[1, 3] = "GongDan No";
            worksheet.Cells[1, 4] = "Group ID";
            worksheet.Cells[1, 5] = "PingDan ID";
            worksheet.Cells[1, 6] = "PingDan No";
            worksheet.Cells[1, 7] = "Gate Out Time";

            for (int y = 0; y < this.dgvPingDan.RowCount; y++)
            {
                worksheet.Cells[y + 2, 1] = y + 1;
                worksheet.Cells[y + 2, 2] = this.dgvPingDan["BeiAnDan ID", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 3] = this.dgvPingDan["GongDan No", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 4] = this.dgvPingDan["Group ID", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 5] = this.dgvPingDan["PingDan ID", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 6] = this.dgvPingDan["PingDan No", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 7] = this.dgvPingDan["Gate Out Time", y].Value.ToString().Trim();
            }
            //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 7]).Font.Bold = true;
            //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 7]).Font.Name = "Verdana";
            //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 7]).Font.Size = 9;
            worksheet.Cells.EntireColumn.AutoFit();
            excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            if (String.Compare(this.cmbIEType.Text.Trim(), "RMB") == 0)
            {
                object missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, 1, missing);

               // worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 15]).NumberFormatLocal = "@";
                worksheet.Cells[1, 1] = "出区凭单ID";
                worksheet.Cells[1, 2] = "出区类型";
                worksheet.Cells[1, 3] = "运抵国（地区）";
                worksheet.Cells[1, 4] = "是否加封";
                worksheet.Cells[1, 5] = "项号";
                worksheet.Cells[1, 6] = "物料备件号";
                worksheet.Cells[1, 7] = "货主/客户名称";
                worksheet.Cells[1, 8] = "物料数量";
                worksheet.Cells[1, 9] = "金额";
                worksheet.Cells[1, 10] = "币制";
                worksheet.Cells[1, 11] = "净重";
                worksheet.Cells[1, 12] = "毛重";
                worksheet.Cells[1, 13] = "原产地/目的地";
                worksheet.Cells[1, 14] = "批次/工单号";
                //worksheet.Cells[1, 15] = "备案单号";

                string strPingDanID = null;
                int iLineNo = 0;
                for (int x = 0; x < this.dgvPingDan.RowCount; x++)
                {
                    string strPDID = this.dgvPingDan["PingDan ID", x].Value.ToString().Trim();
                    if (String.Compare(strPingDanID, strPDID) == 0) { ++iLineNo; }
                    else { strPingDanID = strPDID; iLineNo = 1; }

                    worksheet.Cells[x + 2, 1] = String.Empty;
                    worksheet.Cells[x + 2, 2] = "保税";
                    worksheet.Cells[x + 2, 3] = "中国";
                    worksheet.Cells[x + 2, 4] = "否";
                    worksheet.Cells[x + 2, 5] = 1; 
                    worksheet.Cells[x + 2, 6] = this.dgvPingDan["FG EHB", x].Value.ToString().Trim();
                    worksheet.Cells[x + 2, 7] = "沙伯基础创新塑料（上海）有限公司 ";
                    worksheet.Cells[x + 2, 8] = this.dgvPingDan["PingDan Qty", x].Value.ToString().Trim();
                    worksheet.Cells[x + 2, 9] = this.dgvPingDan["PingDan Amount", x].Value.ToString().Trim();
                    worksheet.Cells[x + 2, 10] = "美元";
                    worksheet.Cells[x + 2, 11] = this.dgvPingDan["PingDan Qty", x].Value.ToString().Trim();
                    decimal dGrossWeight = Math.Round(Convert.ToDecimal(this.dgvPingDan["PingDan Qty", x].Value.ToString().Trim()) * dRatio, 2);
                    worksheet.Cells[x + 2, 12] = dGrossWeight.ToString().Trim();
                    worksheet.Cells[x + 2, 13] = "中国";
                    worksheet.Cells[x + 2, 14] = this.dgvPingDan["GongDan No", x].Value.ToString().Trim();
                    //worksheet.Cells[x + 2, 15] = this.dgvPingDan["BeiAnDan No", x].Value.ToString().Trim();
                }
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 15]).Font.Bold = true;
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Name = "Verdana";
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Size = 9;
                worksheet.Cells.EntireColumn.AutoFit();
                excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

                missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, 1, missing);

                //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 9]).NumberFormatLocal = "@";
                worksheet.Cells[1, 1] = "企业内部编号";
                worksheet.Cells[1, 2] = "出库单号";
                worksheet.Cells[1, 3] = "原始货物备件号";
                worksheet.Cells[1, 4] = "数量";
                worksheet.Cells[1, 5] = "净重";
                worksheet.Cells[1, 6] = "毛重";
                worksheet.Cells[1, 7] = "金额";
                worksheet.Cells[1, 8] = "币制";
                worksheet.Cells[1, 9] = "原产国";

                for (int y = 0; y < this.dgvPingDan.RowCount; y++)
                {
                    worksheet.Cells[y + 2, 1] = this.dgvPingDan["Group ID", y].Value.ToString().Trim();
                    worksheet.Cells[y + 2, 2] = this.dgvPingDan["BeiAnDan ID", y].Value.ToString().Trim();
                    worksheet.Cells[y + 2, 3] = this.dgvPingDan["FG EHB", y].Value.ToString().Trim();
                    worksheet.Cells[y + 2, 4] = this.dgvPingDan["PingDan Qty", y].Value.ToString().Trim();
                    worksheet.Cells[y + 2, 5] = this.dgvPingDan["PingDan Qty", y].Value.ToString().Trim();
                    decimal dGrossWeight = Math.Round(Convert.ToDecimal(this.dgvPingDan["PingDan Qty", y].Value.ToString().Trim()) * dRatio, 2);
                    worksheet.Cells[y + 2, 6] = dGrossWeight.ToString().Trim();
                    worksheet.Cells[y + 2, 7] = this.dgvPingDan["PingDan Amount", y].Value.ToString().Trim();
                    worksheet.Cells[y + 2, 8] = "502";
                    worksheet.Cells[y + 2, 9] = "142";
                }
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 9]).Font.Bold = true;
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Name = "Verdana";
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Size = 9;
                worksheet.Cells.EntireColumn.AutoFit();
                excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            }

            else if (String.Compare(this.cmbIEType.Text.Trim(), "BLP") == 0 || String.Compare(this.cmbIEType.Text.Trim(), "EXPORT") == 0)
            {
                SqlDataAdapter sqlAdp = new SqlDataAdapter("SELECT [EN] AS Destination, Code FROM B_Country", sqlConn);
                DataTable dtDestination = new DataTable();
                sqlAdp.Fill(dtDestination);
                sqlAdp.Dispose();
                SqlLib sqlLib = new SqlLib();
                DataTable dtMergeData = sqlLib.MergeDataTable(dvFillPD.ToTable(), dtDestination, "Destination");
                sqlLib.Dispose(0);
                dtDestination.Dispose();

                object missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, 1, missing);
                //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 9]).NumberFormatLocal = "@";
                worksheet.Cells[1, 1] = "企业内部编号";
                worksheet.Cells[1, 2] = "出库单号";
                worksheet.Cells[1, 3] = "原始货物备件号";
                worksheet.Cells[1, 4] = "数量";
                worksheet.Cells[1, 5] = "净重";
                worksheet.Cells[1, 6] = "毛重";
                worksheet.Cells[1, 7] = "金额";
                worksheet.Cells[1, 8] = "币制";
                worksheet.Cells[1, 9] = "原产国";

                for (int z = 0; z < dtMergeData.Rows.Count; z++)
                {
                    worksheet.Cells[z + 2, 1] = dtMergeData.Rows[z]["Group ID"].ToString().Trim();
                    worksheet.Cells[z + 2, 2] = dtMergeData.Rows[z]["BeiAnDan ID"].ToString().Trim();
                    worksheet.Cells[z + 2, 3] = dtMergeData.Rows[z]["FG EHB"].ToString().Trim();
                    worksheet.Cells[z + 2, 4] = dtMergeData.Rows[z]["PingDan Qty"].ToString().Trim();
                    worksheet.Cells[z + 2, 5] = dtMergeData.Rows[z]["PingDan Qty"].ToString().Trim();
                    decimal dGrossWeight = Math.Round(Convert.ToDecimal(dtMergeData.Rows[z]["PingDan Qty"].ToString().Trim()) * dRatio, 2);
                    worksheet.Cells[z + 2, 6] = dGrossWeight.ToString().Trim();
                    worksheet.Cells[z + 2, 7] = dtMergeData.Rows[z]["PingDan Amount"].ToString().Trim();
                    worksheet.Cells[z + 2, 8] = "502";
                    worksheet.Cells[z + 2, 9] = dtMergeData.Rows[z]["Code"].ToString().Trim();
                }
                dtMergeData.Dispose();
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 9]).Font.Bold = true;
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Name = "Verdana";
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Size = 9;
                worksheet.Cells.EntireColumn.AutoFit();
                excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            }
           
            excel.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            worksheet = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            workbook = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
            GC.Collect();
            if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); sqlConn.Dispose(); }
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
            { MessageBox.Show("Please find the uploading path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge);
                //this.btnBrowse_Click(sender, e);
                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", myConn);
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

            SqlConnection uploadConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (uploadConn.State == ConnectionState.Closed) { uploadConn.Open(); }
            SqlCommand uploadComm = new SqlCommand();
            uploadComm.Connection = uploadConn;
            uploadComm.CommandType = CommandType.StoredProcedure;
            uploadComm.CommandText = @"usp_UpdatePingDan";
            uploadComm.Parameters.AddWithValue("@tvp_Mass", myTable);
            uploadComm.ExecuteNonQuery();
            uploadComm.Parameters.Clear();
            uploadComm.Dispose();
            if (uploadConn.State == ConnectionState.Open) { uploadConn.Close(); uploadConn.Dispose(); }       
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            { MessageBox.Show("There is no data in below data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            int iCount = 0;
            string strPingDanID = null, strPingDanNo = null, strGateOutTime = null, strGroupID = this.cmbGroupID.Text.Trim().ToUpper();
            DataRow[] drow = dtFillPD.Select("[Group ID] = '" + strGroupID + "'");
            if (!String.IsNullOrEmpty(this.txtPingDanID.Text.Trim()))
            {
                strPingDanID = this.txtPingDanID.Text.Trim().ToUpper();
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim())) 
                { 
                    iCount = 1;
                    foreach (DataRow dr in drow) { dr["PingDan ID"] = strPingDanID; }
                }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim()))
                {
                    iCount = 2;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan ID"] = strPingDanID;
                        dr["PingDan No"] = strPingDanNo;
                    }
                }
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim())) 
                { 
                    iCount = 3;
                    strGateOutTime = this.dtpGateOutTime.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan ID"] = strPingDanID;
                        dr["Gate Out Time"] = Convert.ToDateTime(strGateOutTime);
                    }
                }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim()))
                {
                    iCount = 4;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    strGateOutTime = this.dtpGateOutTime.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan ID"] = strPingDanID;
                        dr["PingDan No"] = strPingDanNo;
                        dr["Gate Out Time"] = Convert.ToDateTime(strGateOutTime);
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim()))
                { MessageBox.Show("Please input PingDan ID, PingDan No and/or Gate Out Time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim()))
                {
                    iCount = 5;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    foreach (DataRow dr in drow) { dr["PingDan No"] = strPingDanNo; }
                }
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim()))
                {
                    iCount = 6;
                    strGateOutTime = this.dtpGateOutTime.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Gate Out Time"] = Convert.ToDateTime(strGateOutTime); }
                }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpGateOutTime.Text.Trim()))
                {
                    iCount = 7;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    strGateOutTime = this.dtpGateOutTime.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan No"] = strPingDanNo; 
                        dr["Gate Out Time"] = Convert.ToDateTime(strGateOutTime);
                    }
                }
            }
            dtFillPD.AcceptChanges();

            SqlConnection updateConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (updateConn.State == ConnectionState.Closed) { updateConn.Open(); }
            SqlCommand updateComm = new SqlCommand();
            updateComm.Connection = updateConn;
            updateComm.CommandType = CommandType.StoredProcedure;

            if (iCount == 1)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_1";
                updateComm.Parameters.AddWithValue("@PingDanID", strPingDanID);
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 2)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_2";
                updateComm.Parameters.AddWithValue("@PingDanID", strPingDanID);
                updateComm.Parameters.AddWithValue("@PingDanNo", strPingDanNo);
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 3)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_3";
                updateComm.Parameters.AddWithValue("@PingDanID", strPingDanID);
                updateComm.Parameters.AddWithValue("@GateOutTime", Convert.ToDateTime(strGateOutTime));
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 4)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_4";
                updateComm.Parameters.AddWithValue("@PingDanID", strPingDanID);
                updateComm.Parameters.AddWithValue("@PingDanNo", strPingDanNo);
                updateComm.Parameters.AddWithValue("@GateOutTime", Convert.ToDateTime(strGateOutTime));
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 5)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_5";
                updateComm.Parameters.AddWithValue("@PingDanNo", strPingDanNo);
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 6)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_6";
                updateComm.Parameters.AddWithValue("@GateOutTime", Convert.ToDateTime(strGateOutTime));
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 7)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_7";
                updateComm.Parameters.AddWithValue("@PingDanNo", strPingDanNo);
                updateComm.Parameters.AddWithValue("@GateOutTime", Convert.ToDateTime(strGateOutTime));
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }          
            updateComm.Dispose();
            if (updateConn.State == ConnectionState.Open) { updateConn.Close(); updateConn.Dispose(); }
            this.txtPingDanID.Text = string.Empty;
            this.txtPingDanNo.Text = string.Empty;
            MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            { MessageBox.Show("There is no data in below data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection deleteConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (deleteConn.State == ConnectionState.Closed) { deleteConn.Open(); }
            SqlCommand deleteComm = new SqlCommand();
            deleteComm.Connection = deleteConn;
            deleteComm.CommandType = CommandType.StoredProcedure;
            deleteComm.CommandText = @"usp_DeleteHistoryPingDan";
            deleteComm.Parameters.AddWithValue("@GroupID", this.cmbGroupID.Text.Trim().ToUpper());
            deleteComm.Parameters.AddWithValue("@IEtype", this.cmbIEType.Text.Trim().ToUpper());
            deleteComm.Parameters.AddWithValue("@TotAmt", 0.0M);
            deleteComm.ExecuteNonQuery();
            deleteComm.Parameters.Clear();
            deleteComm.Dispose();
            if (deleteConn.State == ConnectionState.Open) { deleteConn.Close(); deleteConn.Dispose(); }
            
            string strGroupID = this.cmbGroupID.Text.Trim();
            DataRow[] drow = dtFillPD.Select("[Group ID] = '" + strGroupID + "'");
            foreach (DataRow dr in drow) { dr.Delete(); }
            dtFillPD.AcceptChanges();
            this.cmbGroupID.Text = string.Empty;
            MessageBox.Show("Delete data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvPingDan.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvPingDan.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvPingDan.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvPingDan[strColumnName, this.dgvPingDan.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] = " + strColumnText; }
                    }
                }
                dvFillPD.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiExcludeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvPingDan.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvPingDan.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvPingDan.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvPingDan[strColumnName, this.dgvPingDan.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvPingDan.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] <> " + strColumnText; }
                    }
                }
                dvFillPD.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillPD.RowFilter = "";
            this.dgvPingDan.Columns[0].HeaderText = "Select";
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.CurrentCell != null)
            {
                string strColumnName = this.dgvPingDan.Columns[this.dgvPingDan.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter);
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dvFillPD.ToTable(true, new string[] { strColumnName });
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
                        if (this.dgvPingDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvPingDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvPingDan.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvPingDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvPingDan.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvPingDan.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                dvFillPD.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private string GetPingDanGroupID(string strIEtype, SqlConnection sqlConn, SqlCommand sqlComm)
        {
            string strHead = null;
            switch (strIEtype)
            {
                case "1418": strHead = "PA"; break;
                case "BLP": strHead = "PG"; break;
                case "EXPORT": strHead = "PB"; break;
                case "RMB": strHead = "PC"; break;
                case "RMB-1418": strHead = "PD"; break;
                case "RMB-D": strHead = "PE"; break;
                default: strHead = "PF"; break;
            }

            string strDate = strHead + "-" + System.DateTime.Now.ToString("yyyyMMdd").Trim();
            sqlComm.CommandType = CommandType.Text;
            sqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = strIEtype;
            sqlComm.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = strDate;
            sqlComm.CommandText = "SELECT MAX(SUBSTRING([Group ID], 13, LEN([Group ID]))) AS MaxID, [IE Type], SUBSTRING([Group ID], 1, 11) AS [GroupID] FROM " + 
                                  "C_PingDan GROUP BY [IE Type], SUBSTRING([Group ID], 1, 11) HAVING [IE Type] = @IEType AND SUBSTRING([Group ID], 1, 11) = @GroupID";
            string strSuffix = Convert.ToString(sqlComm.ExecuteScalar());
            if (String.IsNullOrEmpty(strSuffix)) { strSuffix = "0001"; }
            else
            {
                int iNumber = Convert.ToInt32(strSuffix) + 1;
                if (iNumber.ToString().Trim().Length == 1) { strSuffix = "000" + iNumber.ToString().Trim(); }
                else if (iNumber.ToString().Trim().Length == 2) { strSuffix = "00" + iNumber.ToString().Trim(); }
                else if (iNumber.ToString().Trim().Length == 3) { strSuffix = "0" + iNumber.ToString().Trim(); }
                else { strSuffix = iNumber.ToString().Trim(); }
            }
            sqlComm.Parameters.Clear();
            return strDate + "-" + strSuffix;
        }

        private string GetASCGroupID(int iSerail, string strGroupID, string strIEType)
        {
            iSerail = strIEType == "RMB" ? iSerail : --iSerail;
            int iSuffix = Convert.ToInt32(strGroupID.Split('-')[2].Trim()) + iSerail;
            string strSuffix = null;
            if (iSuffix.ToString().Trim().Length == 1) { strSuffix = "000" + iSuffix.ToString().Trim(); }
            else if (iSuffix.ToString().Trim().Length == 2) { strSuffix = "00" + iSuffix.ToString().Trim(); }
            else if (iSuffix.ToString().Trim().Length == 3) { strSuffix = "0" + iSuffix.ToString().Trim(); }
            else { strSuffix = iSuffix.ToString().Trim(); }

            if (iSerail > 9999) { return String.Empty; }
            else { return strGroupID.Substring(0, 12).Trim() + strSuffix; }
        }
    }
}
