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
    public partial class RMUnionQuery : Form
    {
        protected DataView dvFillDGV = new DataView();
        protected DataTable dtFillDGV = new DataTable();
        string strFilter = null;
        protected PopUpFilterForm filterFrm = null;

        private DataTable dtITEMNO = new DataTable();
        private DataTable dtRMEHB = new DataTable();
        SqlLib sqlLib = new SqlLib();

        private static RMUnionQuery RMUnionQueryFrm;
        public static RMUnionQuery CreateInstance()
        {
            if (RMUnionQueryFrm == null || RMUnionQueryFrm.IsDisposed)
            {
                RMUnionQueryFrm = new RMUnionQuery();
            }
            return RMUnionQueryFrm;
        }
        public RMUnionQuery()
        {
            InitializeComponent();
        }

        private void RMUnionQuery_Load(object sender, EventArgs e)
        {
            this.cbRMPurchase.Checked = true;
            this.cbRMBalance.Enabled = false;
        }

        private void RMUnionQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtFillDGV.Dispose();
            dtITEMNO.Dispose();
            dtRMEHB.Dispose();
            sqlLib.Dispose(0);
        }

        private void cbRMPurchase_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbRMPurchase.Checked == true) { this.cbRMBalance.Enabled = false; }
            else { this.cbRMBalance.Enabled = true; }
            this.cbRMBalance.Checked = false;
        }

        private void cbRMBalance_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbRMBalance.Checked == true) { this.cbRMPurchase.Enabled = false; }
            else { this.cbRMPurchase.Enabled = true; }
            this.cbRMPurchase.Checked = false;
        }

        private void GetcmbRMCustomsCode()
        {
            this.dtRMEHB = sqlLib.GetDataTable(@"SELECT DISTINCT [RM Customs Code] FROM C_RMPurchase", "C_RMPurchase").Copy(); 
            DataRow drRMEHB = this.dtRMEHB.NewRow();
            drRMEHB["RM Customs Code"] = String.Empty;
            this.dtRMEHB.Rows.InsertAt(drRMEHB, 0);
            this.cmbRMCustomsCode.DisplayMember = this.cmbRMCustomsCode.ValueMember = "RM Customs Code";
            this.cmbRMCustomsCode.DataSource = this.dtRMEHB;
        }

        private void cmbItemNo_Enter(object sender, EventArgs e)
        {
            if (this.dtITEMNO.Rows.Count == 0)
            {               
                this.dtITEMNO = sqlLib.GetDataTable(@"SELECT DISTINCT [Item No] FROM C_RMPurchase", "C_RMPurchase").Copy();
                DataRow ItemRow = this.dtITEMNO.NewRow();
                ItemRow["Item No"] = String.Empty;
                this.dtITEMNO.Rows.InsertAt(ItemRow, 0);
                this.cmbItemNo.DisplayMember = this.cmbItemNo.ValueMember = "Item No";
                this.cmbItemNo.DataSource = this.dtITEMNO;               
            }
        }

        private void cmbItemNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbItemNo.SelectedIndex > 0)
            {
                this.dtRMEHB.Reset();
                this.dtRMEHB = sqlLib.GetDataTable(@"SELECT DISTINCT [RM Customs Code] FROM C_RMPurchase WHERE [Item No] = '" + this.cmbItemNo.SelectedValue.ToString().Trim() + "'", "C_RMPurchase").Copy();
                DataRow LotRow = this.dtRMEHB.NewRow();
                LotRow["RM Customs Code"] = String.Empty;
                this.dtRMEHB.Rows.InsertAt(LotRow, 0);
                this.cmbRMCustomsCode.DisplayMember = this.cmbRMCustomsCode.ValueMember = "RM Customs Code";
                this.cmbRMCustomsCode.DataSource = this.dtRMEHB;
            }
            else { this.GetcmbRMCustomsCode(); }
        }

        private void cmbRMCustomsCode_Enter(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(this.cmbItemNo.Text.Trim()))
            {
                this.GetcmbRMCustomsCode();
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (this.cbRMPurchase.Checked == false && this.cbRMBalance.Checked == false)
            {
                MessageBox.Show("Please select one checkbox before preview.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cbRMPurchase.Focus();
                return;
            }

            strFilter = "";
            dvFillDGV.RowFilter = "";

            string strBrowse = null;
            if (this.cbRMPurchase.Checked == true)
            {
                strBrowse = @"SELECT P.[Item No], P.[Lot No], P.[RM Customs Code], P.[BGD No], B.[Customs Balance], B.[Available RM Balance], B.[GongDan Pending], " +
                             "B.[SharingRM Qty], P.[PO Invoice Qty], B.[PO Invoice Qty] AS [Total PO Qty], P.[RM CHN Name], P.[Amount], P.[RM Price(CIF)], P.[RM Currency], " +
                             "P.[Original Country], P.[OPM Rcvd Date], P.[Customs Rcvd Date], P.[Gate PassThrough Date], P.[Created Date] FROM C_RMBalance AS B RIGHT JOIN " +
                             "C_RMPurchase AS P ON (B.[RM Customs Code] = P.[RM Customs Code]) AND (B.[BGD No] = P.[BGD No]) WHERE";
                if (!String.IsNullOrEmpty(this.cmbItemNo.Text.Trim()))
                { strBrowse += " P.[Item No] = '" + this.cmbItemNo.Text.ToString().Trim() + "' AND"; }
                if (!String.IsNullOrEmpty(this.cmbRMCustomsCode.Text.Trim()))
                { strBrowse += " P.[RM Customs Code] = '" + this.cmbRMCustomsCode.Text.ToString().Trim() + "'"; }
            }
            else 
            {
                strBrowse = @"SELECT B.[RM Customs Code], B.[BGD No], P.[Item No], P.[Lot No], B.[Customs Balance], B.[Available RM Balance], B.[GongDan Pending], " +
                             "B.[SharingRM Qty], P.[PO Invoice Qty], B.[PO Invoice Qty] AS [Total PO Qty], P.[RM CHN Name], P.[Amount], P.[RM Price(CIF)], P.[RM Currency], " +
                             "P.[Original Country], P.[OPM Rcvd Date], P.[Customs Rcvd Date], P.[Gate PassThrough Date], P.[Created Date] FROM C_RMBalance AS B LEFT JOIN " +
                             "C_RMPurchase AS P ON (B.[BGD No] = P.[BGD No]) AND (B.[RM Customs Code] = P.[RM Customs Code]) WHERE";
                if (!String.IsNullOrEmpty(this.cmbItemNo.Text.Trim()))
                { strBrowse += " P.[Item No] = '" + this.cmbItemNo.Text.ToString().Trim() + "' AND"; }
                if (!String.IsNullOrEmpty(this.cmbRMCustomsCode.Text.Trim()))
                { strBrowse += " B.[RM Customs Code] = '" + this.cmbRMCustomsCode.Text.ToString().Trim() + "'"; }
            }           
            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 4, 4), " AND") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 4); }
            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 6, 6), " WHERE") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 6); }

            SqlConnection browseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (browseConn.State == ConnectionState.Closed) { browseConn.Open(); }
            SqlDataAdapter browseAdapter = new SqlDataAdapter(strBrowse, browseConn);
            dtFillDGV.Rows.Clear();
            dtFillDGV.Columns.Clear();
            browseAdapter.Fill(dtFillDGV);
            dvFillDGV = dtFillDGV.DefaultView;
            browseAdapter.Dispose();

            this.dgvRMUnionQuery.DataSource = DBNull.Value;
            if (dtFillDGV.Rows.Count == 0)
            {
                dtFillDGV.Clear();
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else 
            { 
                this.dgvRMUnionQuery.DataSource = dvFillDGV;
                if (this.cbRMPurchase.Checked == true) { this.dgvRMUnionQuery.Columns["Lot No"].Frozen = true; }
                else { this.dgvRMUnionQuery.Columns["BGD No"].Frozen = true; }
            }

            if (browseConn.State == ConnectionState.Open)
            {
                browseConn.Close();
                browseConn.Dispose();
            }            
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvRMUnionQuery.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            int PageRow = 65536;
            DataTable dtCopy = dvFillDGV.ToTable().Copy();
            int iPageCount = (int)(dtCopy.Rows.Count / PageRow);
            if (iPageCount * PageRow < dtCopy.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\RMUnionQuery " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < dtCopy.Columns.Count; n++)
                    { sb.Append(dtCopy.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    int x = 0;
                    if (cbRMPurchase.Checked == true) { x = 3; }
                    else { x = 1; }
                    for (int i = (m - 1) * PageRow; i < dtCopy.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < dtCopy.Columns.Count; j++)
                        {
                            if (j == x) { sb.Append("'" + dtCopy.Rows[i][j].ToString().Trim() + "\t"); }
                            else if (j == 3 && cbRMPurchase.Checked == true) { sb.Append("'" + dtCopy.Rows[i][j].ToString().Trim() + "\t"); }
                            else { sb.Append(dtCopy.Rows[i][j].ToString().Trim() + "\t"); }
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
            dtCopy.Dispose();

            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //excel.Application.Workbooks.Add(true);

            //excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvRMUnionQuery.Rows.Count + 1, this.dgvRMUnionQuery.Columns.Count]).NumberFormatLocal = "@";
            //for (int i = 0; i < this.dgvRMUnionQuery.RowCount; i++)
            //{
            //    for (int j = 0; j < this.dgvRMUnionQuery.ColumnCount; j++)
            //    { excel.Cells[i + 2, j + 1] = this.dgvRMUnionQuery[j, i].Value.ToString().Trim(); }
            //}

            //for (int k = 0; k < this.dgvRMUnionQuery.ColumnCount; k++)
            //{ excel.Cells[1, k + 1] = this.dgvRMUnionQuery.Columns[k].HeaderText.ToString(); }

            //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvRMUnionQuery.ColumnCount]).Font.Bold = true;
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvRMUnionQuery.ColumnCount]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvRMUnionQuery.Rows.Count + 1, this.dgvRMUnionQuery.ColumnCount]).Font.Name = "Verdana";
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvRMUnionQuery.Rows.Count + 1, this.dgvRMUnionQuery.ColumnCount]).Font.Size = 9;
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvRMUnionQuery.Rows.Count + 1, this.dgvRMUnionQuery.ColumnCount]).Borders.LineStyle = 1;
            //excel.Cells.EntireColumn.AutoFit();
            //excel.Visible = true;

            //System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            //excel = null;
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvRMUnionQuery.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMUnionQuery.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMUnionQuery.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMUnionQuery[strColumnName, this.dgvRMUnionQuery.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
                if (this.dgvRMUnionQuery.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMUnionQuery.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMUnionQuery.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMUnionQuery[strColumnName, this.dgvRMUnionQuery.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMUnionQuery.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
            if (this.dgvRMUnionQuery.CurrentCell != null)
            {
                string strColumnName = this.dgvRMUnionQuery.Columns[this.dgvRMUnionQuery.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter);
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dvFillDGV.ToTable(true, new string[] { strColumnName });
                filterFrm.cmbFilterContent.DisplayMember = strColumnName;
                filterFrm.ShowDialog();
                fundeleFilter delefilter_DBAD = new fundeleFilter(funfilter);
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
                        if (this.dgvRMUnionQuery.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvRMUnionQuery.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvRMUnionQuery.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvRMUnionQuery.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvRMUnionQuery.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvRMUnionQuery.Columns[strColumnName].ValueType == typeof(DateTime))
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
    }
}
