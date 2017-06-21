using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class GetDroolsDataDocForm : Form
    {
        DataTable sqlTable = new DataTable();
        DataTable sqlTbl = new DataTable();
        protected DataView dvFillDGV = new DataView();
        string strFilter = null;
        protected PopUpFilterForm filterFrm = null;

        private LoginForm loginFrm = new LoginForm();
        private static GetDroolsDataDocForm getDroolsDataDocFrm;
        public GetDroolsDataDocForm()
        {
            InitializeComponent();
        }
        public static GetDroolsDataDocForm CreateInstance()
        {
            if (getDroolsDataDocFrm == null || getDroolsDataDocFrm.IsDisposed)
            {
                getDroolsDataDocFrm = new GetDroolsDataDocForm();
            }
            return getDroolsDataDocFrm;
        }

        private void GetDroolsDataDocForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlTable.Dispose();
            sqlTbl.Dispose();
        }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection gatherConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gatherConn.State == ConnectionState.Closed) { gatherConn.Open(); }
            SqlDataAdapter gatherAdapter = new SqlDataAdapter(@"SELECT * FROM V_GatherDrools", gatherConn);
            sqlTable.Rows.Clear();
            sqlTable.Columns.Clear();
            gatherAdapter.Fill(sqlTable);
            gatherAdapter.Dispose();
            if (sqlTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvDroolsData.DataSource = DBNull.Value;
                gatherConn.Close();
                gatherConn.Dispose();
                return;
            }
            sqlTable.Columns["GongDan No"].SetOrdinal(0);
            sqlTable.Columns["Drools Qty"].SetOrdinal(2);

            SqlCommand gatherComm = new SqlCommand();
            gatherComm.Connection = gatherConn;
            gatherComm.CommandType = CommandType.StoredProcedure;
            gatherComm.CommandText = @"usp_EditDailyDroolsBeiAnDan";
            gatherComm.Parameters.AddWithValue("@TVP_DroolsBAD", sqlTable);
            gatherComm.Parameters.AddWithValue("@Action", "ADD");
            gatherComm.ExecuteNonQuery();
            gatherComm.Parameters.Clear();
            gatherComm.Dispose();
            if (gatherConn.State == ConnectionState.Open)
            {
                gatherConn.Close();
                gatherConn.Dispose();
            }

            dvFillDGV = sqlTable.DefaultView;
            this.dgvDroolsData.DataSource = dvFillDGV;
            this.dgvDroolsData.Columns[0].HeaderText = "全选";
            this.dgvDroolsData.Columns["FG CHN Name"].Visible = false;
            for (int k = 1; k < this.dgvDroolsData.ColumnCount; k++) { this.dgvDroolsData.Columns[k].ReadOnly = true; }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection previewConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (previewConn.State == ConnectionState.Closed) { previewConn.Open(); }

            SqlDataAdapter previewAdapter = new SqlDataAdapter(@"SELECT * FROM M_DailyBeiAnDan_Drools", previewConn);
            sqlTable.Rows.Clear();
            sqlTable.Columns.Clear();
            previewAdapter.Fill(sqlTable);
            previewAdapter.Dispose();
            if (previewConn.State == ConnectionState.Open)
            {
                previewConn.Close();
                previewConn.Dispose();
            }

            if (sqlTable.Rows.Count == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {                
                dvFillDGV = sqlTable.DefaultView;
                this.dgvDroolsData.DataSource = DBNull.Value;
                this.dgvDroolsData.DataSource = dvFillDGV;
                this.dgvDroolsData.Columns[0].HeaderText = "全选";
                this.dgvDroolsData.Columns["FG CHN Name"].Visible = false;
                for (int i = 1; i < this.dgvDroolsData.ColumnCount; i++) { this.dgvDroolsData.Columns[i].ReadOnly = true; }
            }          
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvDroolsData.RowCount == 0)
            {
                MessageBox.Show("No data exist at below data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.dgvDroolsData.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvDroolsData.Focus();
                return;
            }

            if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection delConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (delConn.State == ConnectionState.Closed) { delConn.Open(); }
                SqlCommand delComm = new SqlCommand();
                delComm.Connection = delConn;
                delComm.CommandType = CommandType.StoredProcedure;

                DataTable copyTable = sqlTable.Clone();
                int iRowCount = this.dgvDroolsData.RowCount;
                for (int i = 0; i < iRowCount; i++)
                {
                    if (String.Compare(this.dgvDroolsData[0, i].EditedFormattedValue.ToString().Trim(), "True") == 0)
                    {
                        copyTable.ImportRow(dvFillDGV.ToTable().Rows[i]);
                        this.dgvDroolsData.Rows.RemoveAt(i);                        
                        iRowCount--;
                        i--;
                    }
                }
                sqlTable.AcceptChanges();
              
                delComm.CommandText = @"usp_EditDailyDroolsBeiAnDan";
                delComm.Parameters.AddWithValue("@TVP_DroolsBAD", copyTable);
                delComm.Parameters.AddWithValue("@Action", "DEL");
                delComm.ExecuteNonQuery();
                delComm.Parameters.Clear();                          
                delComm.Dispose();
                if (delConn.State == ConnectionState.Open)
                {
                    delConn.Close();
                    delConn.Dispose();
                }
                copyTable.Dispose();
                if (sqlTable.Rows.Count == 0) { this.dgvDroolsData.DataSource = DBNull.Value; }
                this.dgvDroolsData.Columns[0].HeaderText = "全选";
            }
        }

        private void btnGatherDoc_Click(object sender, EventArgs e)
        {
            SqlConnection gatherdocConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gatherdocConn.State == ConnectionState.Closed) { gatherdocConn.Open(); }

            SqlDataAdapter gatherdocAdapter = new SqlDataAdapter(@"SELECT * FROM V_GatherDroolsDoc", gatherdocConn);
            sqlTbl.Columns.Clear(); 
            sqlTbl.Rows.Clear(); 
            sqlTbl.Columns.Add("毛重", typeof(decimal));
            gatherdocAdapter.Fill(sqlTbl);
            sqlTbl.Columns["毛重"].SetOrdinal(8);
            if (sqlTbl.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvDroolsDoc.DataSource = DBNull.Value;
                gatherdocAdapter.Dispose();
                gatherdocConn.Close();
                gatherdocConn.Dispose();
                return;
            }

            decimal dGrossWeight = (decimal)sqlTbl.Compute("SUM([毛重])", "");
            sqlTbl.Columns.Add("总毛重", typeof(decimal));
            sqlTbl.Columns["总毛重"].SetOrdinal(2);
            sqlTbl.Columns.Add("金额", typeof(decimal));
            sqlTbl.Columns["金额"].SetOrdinal(10);

            gatherdocAdapter = new SqlDataAdapter(@"SELECT [FG CHN Name], [Average Price(RMB)] FROM B_Drools WHERE RIGHT([Drools EHB], 1) = 'W'", gatherdocConn);
            DataTable dtDrools = new DataTable();
            gatherdocAdapter.Fill(dtDrools);
            gatherdocAdapter.Dispose();

            for (int i = 0; i < sqlTbl.Rows.Count; i++)
            {
                decimal dGW = Convert.ToDecimal(sqlTbl.Rows[i]["毛重"].ToString().Trim());
                string strCHNName = sqlTbl.Rows[i]["CHN Name"].ToString().Trim();
                DataRow[] dr = dtDrools.Select("[FG CHN Name] = '" + strCHNName + "'");
                decimal dAvgPrice = 0.0M;
                if (dr.Length > 0) { dAvgPrice = Convert.ToDecimal(dr[0][1].ToString().Trim()); }

                sqlTbl.Rows[i]["金额"] = Math.Round(dGW * dAvgPrice, 2);
                sqlTbl.Rows[i]["总毛重"] = dGrossWeight;
                sqlTbl.Rows[i]["项号"] = i + 1;
            }
            dtDrools.Clear();
            dtDrools.Dispose();
            sqlTbl.Columns.Remove("CHN Name");

            this.dgvDroolsDoc.DataSource = DBNull.Value;
            this.dgvDroolsDoc.DataSource = sqlTbl;
            this.dgvDroolsDoc.Columns["备案单号"].Visible = false;
            this.dgvDroolsDoc.Columns["单证类型"].Visible = false;
            this.dgvDroolsDoc.Columns["贸易方式"].Visible = false;
            this.dgvDroolsDoc.Columns["结转企业代码"].Visible = false;         
            this.dgvDroolsDoc.Columns["币制"].Visible = false;
            this.dgvDroolsDoc.Columns["原产地/目的地"].Visible = false;
            this.dgvDroolsDoc.Columns["工单/批次号"].Visible = false;

            if (gatherdocConn.State == ConnectionState.Open)
            {
                gatherdocConn.Close();
                gatherdocConn.Dispose();
            }
        }

        private void btnCheckBalance_Click(object sender, EventArgs e)
        {
            if (this.dgvDroolsDoc.Rows.Count == 0)
            {
                MessageBox.Show("There is no data in document grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection checkConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (checkConn.State == ConnectionState.Closed) { checkConn.Open(); }
            SqlDataAdapter checkAdapter = new SqlDataAdapter(@"SELECT * FROM C_DroolsBalance", checkConn);
            DataTable checktable = new DataTable();
            checkAdapter.Fill(checktable);
            checkAdapter.Dispose();

            string strRecord = null;
            foreach (DataRow dr in sqlTbl.Rows)
            {  
                //ignore the case: there is the Drools EHB in M_DailyBeiAnDan_Drools table, but not in B_Drools table --> this case isn't allowed to happen
                string strDroolsEHB = dr["备件号"].ToString().Trim();
                decimal dDroolsQty = Convert.ToDecimal(dr["数量"].ToString().Trim());
                DataRow[] rows = checktable.Select("[Drools EHB] = '" + strDroolsEHB + "'");
                if (rows.Length > 0)
                {
                    decimal dPendingQty = Convert.ToDecimal(rows[0]["Pending Qty"].ToString().Trim());
                    decimal dAvailableBal = Convert.ToDecimal(rows[0]["Available Balance"].ToString().Trim());
                    decimal dLeftQty = dAvailableBal - dPendingQty - dDroolsQty;
                    if (dLeftQty < 0.0M)
                    { strRecord += "\n备件号: " + strDroolsEHB + "; 扣负数量: " + dLeftQty.ToString().Trim(); }
                }
            }
            checktable.Dispose();
            if (checkConn.State == ConnectionState.Open)
            {
                checkConn.Close();
                checkConn.Dispose();
            }

            if (String.IsNullOrEmpty(strRecord))
            { MessageBox.Show("It is normal for Drools Balance.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("Below data will break out Drools Balance:" + strRecord, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }              
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvDroolsDoc.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnDownload.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_GatherDroolsDocDetail", sqlConn);
            DataTable mytbl = new DataTable();
            sqlAdapter.Fill(mytbl);
            sqlAdapter.Dispose();
            sqlConn.Close();
            sqlConn.Dispose();

            Microsoft.Office.Interop.Excel.Application excel_Drools = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_Drools = excel_Drools.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_Drools = workbooks_Drools.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_Drools = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Drools.Worksheets[1];

            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[sqlTbl.Rows.Count + 1, sqlTbl.Columns.Count]).NumberFormatLocal = "@";
            for (int i = 0; i < sqlTbl.Rows.Count; i++)
            {
                for (int j = 0; j < sqlTbl.Columns.Count; j++)
                { worksheet_Drools.Cells[i + 2, j + 1] = sqlTbl.Rows[i][j].ToString().Trim(); }
            }
            for (int k = 0; k < sqlTbl.Columns.Count; k++)
            { worksheet_Drools.Cells[1, k + 1] = sqlTbl.Columns[k].ColumnName.ToString().Trim(); }

            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[1, sqlTbl.Columns.Count]).Font.Bold = true;
            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[sqlTbl.Rows.Count + 1, sqlTbl.Columns.Count]).Font.Name = "Verdana";
            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[sqlTbl.Rows.Count + 1, sqlTbl.Columns.Count]).Font.Size = 9;
            worksheet_Drools.Cells.EntireColumn.AutoFit();

            object missing = System.Reflection.Missing.Value;
            worksheet_Drools = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Drools.Worksheets.Add(missing, missing, missing, missing);

            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[mytbl.Rows.Count + 1, mytbl.Columns.Count]).NumberFormatLocal = "@";
            for (int x = 0; x < mytbl.Rows.Count; x++)
            {
                for (int y = 0; y < mytbl.Columns.Count; y++)
                { worksheet_Drools.Cells[x + 2, y + 1] = mytbl.Rows[x][y].ToString().Trim(); }
            }
            for (int z = 0; z < mytbl.Columns.Count; z++)
            { worksheet_Drools.Cells[1, z + 1] = mytbl.Columns[z].ColumnName.ToString().Trim(); }

            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[1, mytbl.Columns.Count]).Font.Bold = true;
            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[mytbl.Rows.Count + 1, mytbl.Columns.Count]).Font.Name = "Verdana";
            worksheet_Drools.get_Range(worksheet_Drools.Cells[1, 1], worksheet_Drools.Cells[mytbl.Rows.Count + 1, mytbl.Columns.Count]).Font.Size = 9;
            worksheet_Drools.Cells.EntireColumn.AutoFit();

            excel_Drools.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_Drools);
            worksheet_Drools = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Drools);
            excel_Drools = null;
            mytbl.Dispose();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (this.dgvDroolsData.RowCount == 0 || this.dgvDroolsDoc.RowCount == 0)
            {
                MessageBox.Show("There is no data in below grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (String.IsNullOrEmpty(this.txtBeiAnDanID.Text.ToString().Trim()))
            {
                MessageBox.Show("Please input the BeiAnDan ID.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtBeiAnDanID.Focus();
                return;
            }
            string strBeiAnDanID = this.txtBeiAnDanID.Text.ToString().Trim().ToUpper();
            DateTime dtime = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
                      
            sqlComm.CommandText = @"usp_InsertDroolsBeiAnDan";
            sqlComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBeiAnDanID);
            sqlComm.Parameters.AddWithValue("@CreatedDate", dtime);
            sqlComm.Parameters.AddWithValue("@TVP_DroolsBAD", sqlTable);
            sqlComm.Parameters.AddWithValue("@TVP_DroolsBADDoc", sqlTbl);
            sqlComm.ExecuteNonQuery();
            sqlComm.Parameters.Clear();

            this.txtBeiAnDanID.Text = string.Empty;
            this.dgvDroolsData.DataSource = DBNull.Value;
            this.dgvDroolsDoc.DataSource = DBNull.Value;
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            MessageBox.Show("Successfully approve.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvDroolsData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvDroolsData.RowCount; i++) { this.dgvDroolsData[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvDroolsData.RowCount; i++) { this.dgvDroolsData[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvDroolsData.RowCount; i++)
                    {
                        if (String.Compare(this.dgvDroolsData[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvDroolsData[0, i].Value = true; }

                        else { this.dgvDroolsData[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvDroolsData_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvDroolsData.RowCount == 0) { return; }
            if (this.dgvDroolsData.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvDroolsData.RowCount; i++)
                {
                    if (String.Compare(this.dgvDroolsData[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvDroolsData.RowCount && iCount > 0)
                { this.dgvDroolsData.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvDroolsData.RowCount)
                { this.dgvDroolsData.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvDroolsData.Columns[0].HeaderText = "全选"; }
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvDroolsData.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvDroolsData.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvDroolsData.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvDroolsData[strColumnName, this.dgvDroolsData.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
                if (this.dgvDroolsData.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvDroolsData.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvDroolsData.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvDroolsData[strColumnName, this.dgvDroolsData.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvDroolsData.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
            this.dgvDroolsData.Columns[0].HeaderText = "全选";
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvDroolsData.CurrentCell != null)
            {
                string strColumnName = this.dgvDroolsData.Columns[this.dgvDroolsData.CurrentCell.ColumnIndex].Name;
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
                        if (this.dgvDroolsData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvDroolsData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvDroolsData.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvDroolsData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvDroolsData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvDroolsData.Columns[strColumnName].ValueType == typeof(DateTime))
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxShow.Visible == false)
            {
                this.gBoxShow.Visible = true;
                this.dtpFrom.CustomFormat = " ";
                this.dtpTo.CustomFormat = " ";
            }
            else { this.gBoxShow.Visible = false; }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = null;
        }

        private void dtpFrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTo.CustomFormat = null;
        }

        private void dtpTo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; }
        }

        private void btnDownload1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            string strGo = null;
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
                    { strGo += " AND CAST(B.[Customs Release Date] AS datetime) >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND CAST(B.[Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strGo += " AND CAST(B.[Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strGo += " AND CAST(B.[Customs Release Date] AS datetime) >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            string strSQL1 = @"SELECT N'非保' AS [出区类型], N'中国' AS [运抵国（地区）], N'否' AS [是否加封], " +
                              "ROW_NUMBER() OVER(ORDER BY B.[GongDan No], T.[Drools EHB]) AS [项号], T.[Drools EHB] AS [物料备件号], " +
                              "N'沙伯基础创新塑料（上海）有限公司' AS [货主/客户名称], T.[Drools Qty] AS [物料数量], " +
                              "CAST((T.[Drools Qty] * T.[RM Price]) AS decimal(18, 2)) AS [金额], N'人民币' AS [币制], T.[Drools Qty] AS [净重], " +
                              "T.[Drools Qty] AS [毛重], ISNULL(T.[Original Country], '') AS [原产地/目的地], B.[GongDan No] AS [批次/工单号] " +
                              "FROM C_BeiAnDan AS B INNER JOIN (SELECT D.[GongDan No], D.[Drools EHB], SUM(CAST(D.[Drools Quota] AS decimal(18, 6))) AS [Drools Qty], " +
                              "D.[RM Price], R.[Original Country] FROM C_GongDanDetail AS D LEFT OUTER JOIN C_RMPurchase AS R ON D.[Item No] = R.[Item No] AND " +
                              "D.[Lot No] = R.[Lot No] WHERE (RIGHT(D.[Drools EHB], 1) = 'R') GROUP BY D.[GongDan No], D.[Drools EHB], D.[RM Price], R.[Original Country]) AS T " +
                              "ON B.[GongDan No] = T.[GongDan No] WHERE B.[IE Type] <> 'RMB' AND B.[Customs Release Date] IS NOT NULL AND B.[Customs Release Date] <> ''" + strGo;

            string strSQL2 = @"SELECT [FG EHB] AS [成品备件号], [RM Customs Code] AS [原料备件号], ISNULL([RM CHN Name], '') AS [原料中文品名], " +
                              "SUM([Consumption]) AS [单耗], [Drools Rate] AS [数量损耗率(%)], N'非保料件' AS [料件性质], [Drools EHB] AS [废品备件号], " +
                              "[CHN Name] AS [成品中文名称], [GongDan Qty] AS [数量], SUM([RM Qty]) AS [耗用量], SUM([Drools Qty]) AS [Drools], [BGD No] AS [报关单号] " +
                              "FROM (SELECT B.[FG EHB], D.[RM Customs Code], D.[Consumption], M.[Drools Rate], D.[Drools EHB], B.[CHN Name], B.[GongDan Qty], " +
                              "CAST(D.[RM Used Qty] AS decimal(18, 6)) AS [RM Qty], CAST(D.[Drools Quota] AS decimal(18, 6)) AS [Drools Qty], D.[BGD No], R.[RM CHN Name] " +
                              "FROM C_GongDan AS M RIGHT OUTER JOIN C_BeiAnDan AS B ON M.[GongDan No] = B.[GongDan No] FULL OUTER JOIN C_GongDanDetail AS D LEFT OUTER JOIN " +
                              "C_RMPurchase AS R ON D.[Item No] = R.[Item No] AND D.[Lot No] = R.[Lot No] ON M.[GongDan No] = D.[GongDan No] " +
                              "WHERE B.[IE Type] <> 'RMB' AND RIGHT(D.[Drools EHB], 1) = 'R' AND B.[Customs Release Date] IS NOT NULL AND B.[Customs Release Date] <> ''" + 
                              strGo + ") AS X GROUP BY [FG EHB], [RM Customs Code], [RM CHN Name], [Drools Rate], [Drools EHB], [CHN Name], [GongDan Qty], [BGD No]";

            SqlConnection dl1Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (dl1Conn.State == ConnectionState.Closed) { dl1Conn.Open(); }
            SqlDataAdapter dl1Adapter = new SqlDataAdapter(strSQL1, dl1Conn);
            DataTable dt1 = new DataTable();
            dl1Adapter.Fill(dt1);
            dl1Adapter = new SqlDataAdapter(strSQL2, dl1Conn);
            DataTable dt2 = new DataTable();
            dl1Adapter.Fill(dt2);
            dl1Adapter.Dispose();

            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dt1.Dispose();
                dt2.Dispose();
                dl1Conn.Close();
                dl1Conn.Dispose();
                return;
            }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(dt1.Rows.Count / PageRow);
                if (iPageCount * PageRow < dt1.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\Drools BeiAnDan(Non-RMB IE & RMB RM)_ " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); 
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dt1.Columns.Count; n++)
                        { sb.Append(dt1.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < dt1.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                if (j == 12) { sb.Append("'" + dt1.Rows[i][j].ToString().Trim() + "\t"); }
                                else { sb.Append(dt1.Rows[i][j].ToString().Trim() + "\t"); }
                            }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }                  
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                finally { dt1.Dispose(); }

                if (dt2.Rows.Count > 0)
                {
                    iPageCount = (int)(dt2.Rows.Count / PageRow);
                    if (iPageCount * PageRow < dt2.Rows.Count) { iPageCount += 1; }
                    try
                    {
                        for (int m = 1; m <= iPageCount; m++)
                        {
                            string strPath = System.Windows.Forms.Application.StartupPath + "\\Drools BeiAnDan(Non-RMB IE & RMB RM) Detail_ " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                            if (File.Exists(strPath)) { File.Delete(strPath); }
                            Thread.Sleep(1000);
                            StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); 
                            StringBuilder sb = new StringBuilder();
                            for (int n = 0; n < dt2.Columns.Count; n++)
                            { sb.Append(dt2.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                            sb.Append(Environment.NewLine);

                            for (int i = (m - 1) * PageRow; i < dt2.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dt2.Columns.Count; j++)
                                {
                                    if (j == 11) { sb.Append("'" + dt2.Rows[i][j].ToString().Trim() + "\t"); }
                                    else { sb.Append(dt2.Rows[i][j].ToString().Trim() + "\t"); }
                                }
                                sb.Append(Environment.NewLine);
                            }

                            sw.Write(sb.ToString());
                            sw.Flush();
                            sw.Close();
                            sw.Dispose();
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    finally { dt2.Dispose(); }
                }
                MessageBox.Show("Successfully generated Drools BeiAnDan(Non-RMB IE & RMB RM) Report.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (dl1Conn.State == ConnectionState.Open)
            {
                dl1Conn.Close();
                dl1Conn.Dispose();
            }
        }

        private void btnDownload2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            string strGo = null;
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
                    { strGo += " AND CAST(B.[Customs Release Date] AS datetime) >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND CAST(B.[Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strGo += " AND CAST(B.[Customs Release Date] AS datetime) < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strGo += " AND CAST(B.[Customs Release Date] AS datetime) >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            string strSQL3 = @"SELECT [GongDan No], [FG EHB], [FG CHN Name], [BeiAnDan ID], [BeiAnDan No], [Drools EHB], CAST(SUM([Drools Quota]) AS decimal(18, 6)) AS " +
                              "[Drools Qty] FROM (SELECT B.[GongDan No], B.[FG EHB], B.[CHN Name] AS [FG CHN Name], B.[BeiAnDan ID], B.[BeiAnDan No], D.[Drools EHB], " +
                              "D.[Drools Quota] FROM C_BeiAnDan AS B LEFT OUTER JOIN C_GongDanDetail AS D ON " +
                              "B.[GongDan No] = D.[GongDan No] WHERE RIGHT(D.[Drools EHB], 1) = 'W' AND B.[IE Type] = 'RMB' AND B.[Customs Release Date] IS NOT NULL " +
                              "AND B.[Customs Release Date] <> ''" + strGo + ") AS X GROUP BY [GongDan No], [FG EHB], [FG CHN Name], [BeiAnDan ID], [BeiAnDan No], [Drools EHB]";

            string strSQL4 = @"SELECT [GongDan No], [FG EHB], [FG CHN Name], [BeiAnDan ID], [BeiAnDan No], [Drools EHB], CAST(SUM([Drools Quota]) AS decimal(18, 6)) AS " +
                              "[Drools Qty] FROM (SELECT B.[GongDan No], B.[FG EHB], B.[CHN Name] AS [FG CHN Name], B.[BeiAnDan ID], B.[BeiAnDan No], D.[Drools EHB], " +
                              "D.[Drools Quota] FROM C_BeiAnDan AS B LEFT OUTER JOIN C_GongDanDetail AS D ON " +
                              "B.[GongDan No] = D.[GongDan No] WHERE RIGHT(D.[Drools EHB], 1) = 'R' AND B.[IE Type] = 'RMB' AND B.[Customs Release Date] IS NOT NULL " +
                              "AND B.[Customs Release Date] <> ''" + strGo + ") AS X GROUP BY [GongDan No], [FG EHB], [FG CHN Name], [BeiAnDan ID], [BeiAnDan No], [Drools EHB]";

            SqlConnection dl2Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (dl2Conn.State == ConnectionState.Closed) { dl2Conn.Open(); }
            SqlDataAdapter dl2Adapter = new SqlDataAdapter(strSQL3, dl2Conn);
            DataTable dt3 = new DataTable();
            dl2Adapter.Fill(dt3);
            dl2Adapter = new SqlDataAdapter(strSQL4, dl2Conn);
            DataTable dt4 = new DataTable();
            dl2Adapter.Fill(dt4);
            dl2Adapter.Dispose();

            if (dt3.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dt3.Dispose();
                dt4.Dispose();
                dl2Conn.Close();
                dl2Conn.Dispose();
                return;
            }
            else
            {
                int PageRow = 65536;
                int iPageCount = (int)(dt3.Rows.Count / PageRow);
                if (iPageCount * PageRow < dt3.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\Drools BeiAnDan(RMB IE & USD RM)_ " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode);
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dt3.Columns.Count; n++)
                        { sb.Append(dt3.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < dt3.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < dt3.Columns.Count; j++)
                            {
                                if (j == 0 || j == 4) { sb.Append("'" + dt3.Rows[i][j].ToString().Trim() + "\t"); }
                                else { sb.Append(dt3.Rows[i][j].ToString().Trim() + "\t"); }
                            }
                            sb.Append(Environment.NewLine);
                        }

                        sw.Write(sb.ToString());
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                finally { dt3.Dispose(); }

                if (dt4.Rows.Count > 0)
                {
                    iPageCount = (int)(dt4.Rows.Count / PageRow);
                    if (iPageCount * PageRow < dt4.Rows.Count) { iPageCount += 1; }
                    try
                    {
                        for (int m = 1; m <= iPageCount; m++)
                        {
                            string strPath = System.Windows.Forms.Application.StartupPath + "\\Drools BeiAnDan(RMB IE & RMB RM)_ " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                            if (File.Exists(strPath)) { File.Delete(strPath); }
                            Thread.Sleep(1000);
                            StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode);
                            StringBuilder sb = new StringBuilder();
                            for (int n = 0; n < dt4.Columns.Count; n++)
                            { sb.Append(dt4.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                            sb.Append(Environment.NewLine);

                            for (int i = (m - 1) * PageRow; i < dt4.Rows.Count && i < m * PageRow; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                for (int j = 0; j < dt4.Columns.Count; j++)
                                {
                                    if (j == 0 || j == 4) { sb.Append("'" + dt4.Rows[i][j].ToString().Trim() + "\t"); }
                                    else { sb.Append(dt4.Rows[i][j].ToString().Trim() + "\t"); }
                                }
                                sb.Append(Environment.NewLine);
                            }

                            sw.Write(sb.ToString());
                            sw.Flush();
                            sw.Close();
                            sw.Dispose();
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    finally { dt4.Dispose(); }
                }
                MessageBox.Show("Successfully generated Drools BeiAnDan(RMB IE & USD/RMB RM) Report.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (dl2Conn.State == ConnectionState.Open)
            {
                dl2Conn.Close();
                dl2Conn.Dispose();
            }
        }

        private void btnSearchPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtSearchPath.Text = openDlg.FileName;
        }

        private void btnUploadDownload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtSearchPath.Text.Trim()))
            {
                MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnSearchPath.Focus();
                return;
            }
            try
            {
                bool bJudge = this.txtSearchPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtSearchPath.Text.Trim(), bJudge);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upload error, please try again.\n" + ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void ImportExcelData(string strFilePath, bool bJudge)
        {
            string strConn;
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

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
                myTable.Clear();
                myTable.Dispose();
                return;
            }
            myTable.Columns.Add("GongDan No", typeof(string));
            myTable.Columns.Add("FG EHB", typeof(string));
            string strNow = "AD" + System.DateTime.Now.ToString("yyMMdd");

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (myConn.State == ConnectionState.Closed) { Conn.Open(); }
            string strSQL = @"SELECT TOP 1 CAST(SUBSTRING([GongDan No], 10, LEN([GongDan No])) AS Int) AS [MaxID] FROM C_BeiAnDan_Drools WHERE [GongDan No] LIKE '"+ strNow + "%' ORDER BY 1 DESC";
            SqlDataAdapter Adpter = new SqlDataAdapter(strSQL, Conn);
            DataTable dtMaxSerial = new DataTable();
            Adpter.Fill(dtMaxSerial);
            Adpter.Dispose();

            int iSerial = 0;
            if (dtMaxSerial.Rows.Count == 0) { iSerial = 1; }
            else { iSerial = Convert.ToInt32(dtMaxSerial.Rows[0][0].ToString().Trim()) + 1; }
            dtMaxSerial.Clear();
            dtMaxSerial.Dispose();
            foreach (DataRow dr in myTable.Rows)
            {
                dr["GongDan No"] = strNow + "-" + iSerial.ToString();
                dr["FG EHB"] = "*/" + strNow;
                iSerial++;
            }
            myTable.AcceptChanges();

            SqlCommand Comm = new SqlCommand();
            Comm.Connection = Conn;
            Comm.CommandType = CommandType.StoredProcedure;
            Comm.CommandText = "usp_AdjustDailyDroolsBeiAnDan";
            Comm.Parameters.AddWithValue("@TVP_Adjust", myTable);
            string strMessage = String.Empty;
            try { Comm.ExecuteNonQuery(); }
            catch (Exception ex) { strMessage = ex.Message; }
            finally
            {
                Comm.Parameters.Clear();
                Comm.Dispose();
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            if (String.IsNullOrEmpty(strMessage)) { MessageBox.Show("Insert the adjustment data successfully!\nPlease click 'Preview' button to query the related data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show(strMessage, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When upload the related data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                            "\n\tDroole EHB, \n\tDrools CHN Name, \n\tDrools Qty, \n\tFG CHN Name", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
    }
}