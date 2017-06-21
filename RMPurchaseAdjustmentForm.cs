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
    public partial class RMPurchaseAdjustmentForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        protected DataView dvFillDGV = new DataView();
        protected DataTable dtFillDGV = new DataTable();
        string strFilter = null;
        protected PopUpFilterForm filterFrm = null;

        private DataTable dataTable1 = new DataTable();
        private DataTable dataTable2 = new DataTable();
        private DataTable dataTable3 = new DataTable();
        SqlLib sqlLib = new SqlLib();
        private DataTable dTable = new DataTable();
        int iRowIndex = 0;

        private static RMPurchaseAdjustmentForm RMPurchaseAdjustmentFrm;
        public RMPurchaseAdjustmentForm()
        {
            InitializeComponent();
        }
        public static RMPurchaseAdjustmentForm CreateInstance()
        {
            if (RMPurchaseAdjustmentFrm == null || RMPurchaseAdjustmentFrm.IsDisposed)
            {
                RMPurchaseAdjustmentFrm = new RMPurchaseAdjustmentForm();
            }
            return RMPurchaseAdjustmentFrm;
        }

        private void RMAdjustmentForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";        
        }

        private void RMAdjustmentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtFillDGV.Dispose();
            this.dataTable1.Dispose();
            this.dataTable2.Dispose();
            this.dataTable3.Dispose();
            this.dTable.Dispose();
            sqlLib.Dispose(0);
        }

        private void GetcmbLotNoData()
        {
            this.dataTable2 = sqlLib.GetDataTable(@"SELECT DISTINCT [Lot No] FROM C_RMPurchase", "C_RMPurchase").Copy();
            DataRow LotRow = this.dataTable2.NewRow();
            LotRow["Lot No"] = String.Empty;
            this.dataTable2.Rows.InsertAt(LotRow, 0);
            this.cmbLotNo.DisplayMember = this.cmbLotNo.ValueMember = "Lot No";
            this.cmbLotNo.DataSource = this.dataTable2;
        }

        private void GetcmbBGDNoData()
        {
            this.dataTable3 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMPurchase", "C_RMPurchase").Copy();
            DataRow BGDRow = this.dataTable3.NewRow();
            BGDRow["BGD No"] = String.Empty;
            this.dataTable3.Rows.InsertAt(BGDRow, 0);
            this.cmbBGDNo.DisplayMember = this.cmbBGDNo.ValueMember = "BGD No";
            this.cmbBGDNo.DataSource = this.dataTable3;
        }

        private void cmbItemNo_Enter(object sender, EventArgs e)
        {
            if (this.dataTable1.Rows.Count == 0)
            {
                this.dataTable1 = sqlLib.GetDataTable(@"SELECT DISTINCT [Item No] FROM C_RMPurchase", "C_RMPurchase").Copy();
                DataRow ItemRow = this.dataTable1.NewRow();
                ItemRow["Item No"] = String.Empty;
                this.dataTable1.Rows.InsertAt(ItemRow, 0);
                this.cmbItemNo.DisplayMember = this.cmbItemNo.ValueMember = "Item No";
                this.cmbItemNo.DataSource = this.dataTable1;
            }
        }

        private void cmbItemNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbItemNo.SelectedIndex > 0)
            {
                this.dataTable2.Reset();
                this.dataTable2 = sqlLib.GetDataTable(@"SELECT DISTINCT [Lot No] FROM C_RMPurchase WHERE [Item No] = '" + this.cmbItemNo.Text.ToString().Trim() + "'", "C_RMPurchase").Copy();
                DataRow LotRow = this.dataTable2.NewRow();
                LotRow["Lot No"] = String.Empty;
                this.dataTable2.Rows.InsertAt(LotRow, 0);
                this.cmbLotNo.DisplayMember = this.cmbLotNo.ValueMember = "Lot No";
                this.cmbLotNo.DataSource = this.dataTable2;

                this.dataTable3.Reset();
                this.dataTable3 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMPurchase WHERE [Item No] = '" + this.cmbItemNo.Text.ToString().Trim() + "'", "C_RMPurchase").Copy();
                DataRow BGDRow = this.dataTable3.NewRow();
                BGDRow["BGD No"] = String.Empty;
                this.dataTable3.Rows.InsertAt(BGDRow, 0);
                this.cmbBGDNo.DisplayMember = this.cmbBGDNo.ValueMember = "BGD No";
                this.cmbBGDNo.DataSource = this.dataTable3;
            }
            else 
            { 
                this.GetcmbLotNoData();
                this.GetcmbBGDNoData(); 
            }          
        }

        private void cmbLotNo_Enter(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbItemNo.Text.Trim()))
            { this.GetcmbLotNoData(); }
        }

        private void cmbLotNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataTable3.Reset();
            if (!String.IsNullOrEmpty(this.cmbItemNo.Text.ToString().Trim()))
            {
                if (!String.IsNullOrEmpty(this.cmbLotNo.Text.ToString().Trim()))
                { this.dataTable3 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMPurchase WHERE [Item No] = '" + this.cmbItemNo.Text.ToString().Trim() + "' AND [Lot No] = '" + this.cmbLotNo.Text.ToString().Trim() + "'", "C_RMPurchase").Copy(); }
                else
                { this.dataTable3 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMPurchase WHERE [Item No] = '" + this.cmbItemNo.Text.ToString().Trim() + "'", "C_RMPurchase").Copy(); }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.cmbLotNo.Text.ToString().Trim()))
                { this.dataTable3 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMPurchase WHERE [Lot No] = '" + this.cmbLotNo.Text.ToString().Trim() + "'", "C_RMPurchase").Copy(); }
                else
                { this.dataTable3 = sqlLib.GetDataTable(@"SELECT DISTINCT [BGD No] FROM C_RMPurchase", "C_RMPurchase").Copy(); }
            }
            DataRow BGDRow = this.dataTable3.NewRow();
            BGDRow["BGD No"] = String.Empty;
            this.dataTable3.Rows.InsertAt(BGDRow, 0);
            this.cmbBGDNo.DisplayMember = this.cmbBGDNo.ValueMember = "BGD No";
            this.cmbBGDNo.DataSource = this.dataTable3;
        }

        private void cmbBGDNo_Enter(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbItemNo.Text.Trim()) && String.IsNullOrEmpty(this.cmbLotNo.Text.Trim()))
            { this.GetcmbBGDNoData(); }
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            this.dgvRMAdjustment.DataSource = DBNull.Value; 

            string strBrowse = @"SELECT * FROM C_RMPurchase WHERE";
            if (!String.IsNullOrEmpty(this.cmbItemNo.Text.Trim()))
            { strBrowse += " [Item No] = '" + this.cmbItemNo.Text.ToString().Trim() + "' AND"; }
            if (!String.IsNullOrEmpty(this.cmbLotNo.Text.Trim()))
            { strBrowse += " [Lot No] = '" + this.cmbLotNo.Text.ToString().Trim() + "' AND"; }
            if (!String.IsNullOrEmpty(this.cmbBGDNo.Text.Trim()))
            { strBrowse += " [BGD No] = '" + this.cmbBGDNo.Text.ToString().Trim() + "' AND"; }
            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1)
                    {
                        MessageBox.Show("Begin date is not greater than end date for created date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom.Focus();
                        return;
                    }
                    else
                    { strBrowse += " [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [Created Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse += " [Created Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strBrowse += " [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 4, 4), " AND") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 4); }
            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 6, 6), " WHERE") == 0)
            { strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 6); }
            strBrowse += " ORDER BY [Created Date] DESC, [Item No], [Lot No]";

            SqlConnection browseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (browseConn.State == ConnectionState.Closed) { browseConn.Open(); }
            SqlDataAdapter browseAdapterR = new SqlDataAdapter(strBrowse, browseConn);
            dtFillDGV.Clear();
            browseAdapterR.Fill(dtFillDGV);
            dvFillDGV = dtFillDGV.DefaultView;
            browseAdapterR.Dispose();

            if (dtFillDGV.Rows.Count == 0)
            {
                dtFillDGV.Clear();                
                this.dgvRMPurchase.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvRMPurchase.DataSource = dvFillDGV;
                this.dgvRMPurchase.Rows[0].HeaderCell.Value = 1;
                for (int i = 2; i < this.dgvRMPurchase.ColumnCount; i++)
                { this.dgvRMPurchase.Columns[i].ReadOnly = true; }
                this.dgvRMPurchase.Columns[3].Frozen = true;
            }

            if (browseConn.State == ConnectionState.Open)
            {
                browseConn.Close();
                browseConn.Dispose();
            }            
        }

        private void dgvRMPurchase_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvRMPurchase.RowCount; i++) { this.dgvRMPurchase[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvRMPurchase.RowCount; i++) { this.dgvRMPurchase[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvRMPurchase.RowCount; i++)
                    {
                        if (String.Compare(this.dgvRMPurchase[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvRMPurchase[0, i].Value = true; }
                        else { this.dgvRMPurchase[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvRMPurchase_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvRMPurchase.RowCount == 0) { return; }
            if (this.dgvRMPurchase.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvRMPurchase.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMPurchase[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvRMPurchase.RowCount && iCount > 0)
                { this.dgvRMPurchase.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvRMPurchase.RowCount)
                { this.dgvRMPurchase.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvRMPurchase.Columns[0].HeaderText = "全选"; }
            }
        }

        private void dgvRMPurchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataTable myTable = new DataTable();
                for (int i = 2; i < this.dgvRMPurchase.ColumnCount; i++)
                { myTable.Columns.Add(this.dgvRMPurchase.Columns[i].Name, this.dgvRMPurchase.Columns[i].ValueType); }
                myTable.Columns[3].SetOrdinal(0); // Item No
                myTable.Columns[4].SetOrdinal(1); // Lot No
                myTable.Columns[5].SetOrdinal(2); // RM Customs Code
                myTable.Columns[3].SetOrdinal(14); // Transaction Type
                myTable.Columns[4].SetOrdinal(14); // Customs Entry No
                myTable.Columns[4].SetOrdinal(14); // RM CHN Name
                myTable.Columns.Add("Note", typeof(String));
                myTable.Columns["Note"].DefaultValue = String.Empty;

                iRowIndex = this.dgvRMPurchase.CurrentRow.Index;
                string strPOInvoiceQty = this.dgvRMPurchase["PO Invoice Qty", iRowIndex].Value.ToString().Trim();
                string strAmount = this.dgvRMPurchase["Amount", iRowIndex].Value.ToString().Trim();
                string strRMPrice = this.dgvRMPurchase["RM Price(CIF)", iRowIndex].Value.ToString().Trim();
                string strOPMRcvdDate = this.dgvRMPurchase["OPM Rcvd Date", iRowIndex].Value.ToString().Trim();
                string strCustomsRcvdDate = this.dgvRMPurchase["Customs Rcvd Date", iRowIndex].Value.ToString().Trim();
                string strGatePassThroughDate = this.dgvRMPurchase["Gate PassThrough Date", iRowIndex].Value.ToString().Trim();

                DataRow dr = myTable.NewRow();
                dr["Item No"] = this.dgvRMPurchase["Item No", iRowIndex].Value.ToString().Trim();
                dr["Lot No"] = this.dgvRMPurchase["Lot No", iRowIndex].Value.ToString().Trim();
                dr["RM Customs Code"] = this.dgvRMPurchase["RM Customs Code", iRowIndex].Value.ToString().Trim();
                dr["BGD No"] = this.dgvRMPurchase["BGD No", iRowIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strPOInvoiceQty)) { dr["PO Invoice Qty"] = 0.0M; }
                else { dr["PO Invoice Qty"] = Convert.ToDecimal(strPOInvoiceQty); }
                if (String.IsNullOrEmpty(strAmount)) { dr["Amount"] = 0.0M; }
                else { dr["Amount"] = Convert.ToDecimal(strAmount); }
                if (String.IsNullOrEmpty(strRMPrice)) { dr["RM Price(CIF)"] = 0.0M; }
                else { dr["RM Price(CIF)"] = Convert.ToDecimal(strRMPrice); }
                dr["RM Currency"] = this.dgvRMPurchase["RM Currency", iRowIndex].Value.ToString().Trim();
                dr["Original Country"] = this.dgvRMPurchase["Original Country", iRowIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strOPMRcvdDate)) { dr["OPM Rcvd Date"] = DBNull.Value; }
                else { dr["OPM Rcvd Date"] = Convert.ToDateTime(strOPMRcvdDate); }
                if (String.IsNullOrEmpty(strCustomsRcvdDate)) { dr["Customs Rcvd Date"] = DBNull.Value; }
                else { dr["Customs Rcvd Date"] = strCustomsRcvdDate; }
                dr["PO No"] = this.dgvRMPurchase["PO No", iRowIndex].Value.ToString().Trim();
                dr["Transaction Type"] = this.dgvRMPurchase["Transaction Type", iRowIndex].Value.ToString().Trim();
                dr["Customs Entry No"] = this.dgvRMPurchase["Customs Entry No", iRowIndex].Value.ToString().Trim();
                dr["RM CHN Name"] = this.dgvRMPurchase["RM CHN Name", iRowIndex].Value.ToString().Trim();
                dr["Voucher ID"] = this.dgvRMPurchase["Voucher ID", iRowIndex].Value.ToString().Trim();
                dr["Voucher Status"] = this.dgvRMPurchase["Voucher Status", iRowIndex].Value.ToString().Trim();
                dr["Remark"] = this.dgvRMPurchase["Remark", iRowIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strGatePassThroughDate)) { dr["Gate PassThrough Date"] = DBNull.Value; }
                else { dr["Gate PassThrough Date"] = strGatePassThroughDate; }
                dr["Created Date"] = Convert.ToDateTime(this.dgvRMPurchase["Created Date", iRowIndex].Value.ToString().Trim());
                myTable.Rows.Add(dr);

                dTable = myTable.Copy();
                myTable.Columns.Remove("Remark");
                myTable.Columns.Remove("Created Date");
                this.dgvRMAdjustment.DataSource = DBNull.Value;
                this.dgvRMAdjustment.DataSource = myTable;               
                this.dgvRMAdjustment.ReadOnly = true;
                this.dgvRMAdjustment.Columns[3].Frozen = true;
                if (this.cmbAdjustment.SelectedIndex > 0) { this.cmbAdjustment.Text = String.Empty; }
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvRMPurchase.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMPurchase.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMPurchase.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMPurchase[strColumnName, this.dgvRMPurchase.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
                if (this.dgvRMPurchase.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvRMPurchase.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvRMPurchase.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvRMPurchase[strColumnName, this.dgvRMPurchase.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvRMPurchase.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
            if (this.dgvRMAdjustment.RowCount > 0) { this.dgvRMAdjustment.DataSource = DBNull.Value; }
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvRMPurchase.CurrentCell != null)
            {
                string strColumnName = this.dgvRMPurchase.Columns[this.dgvRMPurchase.CurrentCell.ColumnIndex].Name;
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
                        if (this.dgvRMPurchase.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvRMPurchase.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvRMPurchase.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvRMPurchase.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvRMPurchase.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvRMPurchase.Columns[strColumnName].ValueType == typeof(DateTime))
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

        private void tsmiCustomsRcvdDate_Click(object sender, EventArgs e)
        {
            if (this.dgvRMPurchase.Rows.Count > 0)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        strFilter += @" AND ([Customs Rcvd Date] = '' OR [Customs Rcvd Date] IS NULL)";
                    }
                    else
                    {
                        strFilter = @"([Customs Rcvd Date] = '' OR [Customs Rcvd Date] IS NULL)";
                    }
                    dvFillDGV.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void tsmiVoucherID_Click(object sender, EventArgs e)
        {
            if (this.dgvRMPurchase.Rows.Count > 0)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        strFilter += @" AND ([Voucher ID] = '' OR [Voucher ID] IS NULL OR [Voucher Status] = '' OR [Voucher Status] IS NULL OR [Gate PassThrough Date] IS NULL)";
                    }
                    else
                    {
                        strFilter = @"([Voucher ID] = '' OR [Voucher ID] IS NULL OR [Voucher Status] = '' OR [Voucher Status] IS NULL OR [Gate PassThrough Date] IS NULL)";
                    }
                    dvFillDGV.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void cmbAdjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvRMAdjustment.Rows.Count == 0)
            { 
                MessageBox.Show("There is no data in below grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return; 
            }

            this.dgvRMAdjustment.ReadOnly = true;
            for (int k = 0; k < this.dgvRMAdjustment.ColumnCount; k++)
            { this.dgvRMAdjustment.Columns[k].HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.HighlightText); }

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = Color.FromArgb(178, 235, 140);
            this.dgvRMAdjustment.EnableHeadersVisualStyles = false;
            this.dgvRMAdjustment.ReadOnly = false;

            if (this.cmbAdjustment.SelectedIndex == 0)
            { this.dgvRMAdjustment.ReadOnly = true; }

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Edit-Other") == 0)
            {
                this.dgvRMAdjustment.Columns["Item No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["Lot No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["RM Customs Code"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["BGD No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["Customs Rcvd Date"].ReadOnly = true;

                this.dgvRMAdjustment.Columns["Item No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["Lot No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["RM Customs Code"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["BGD No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["Customs Rcvd Date"].HeaderCell.Style = cellStyle;
            }

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Edit-BGD") == 0)
            {
                for (int i = 0; i < this.dgvRMAdjustment.ColumnCount; i++)
                {
                    if (this.dgvRMAdjustment.Columns[i].Name == "BGD No")
                    {
                        this.dgvRMAdjustment.Columns["BGD No"].ReadOnly = false;
                        continue;
                    }
                    this.dgvRMAdjustment.Columns[i].ReadOnly = true;
                    this.dgvRMAdjustment.Columns[i].HeaderCell.Style = cellStyle;
                }
            }

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Cancel") == 0)
            {
                for (int j = 0; j < this.dgvRMAdjustment.ColumnCount - 1; j++)
                {
                    this.dgvRMAdjustment.Columns[j].ReadOnly = true;
                    this.dgvRMAdjustment.Columns[j].HeaderCell.Style = cellStyle;
                }
            }
        }

        private void btnAdjust_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbAdjustment.Text.ToString().Trim()))
            {
                MessageBox.Show("Please select 'Adjust Type' first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbAdjustment.Focus();
                return;
            }

            if (this.dgvRMAdjustment.RowCount == 0)
            {
                MessageBox.Show("No data need to adjust.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlCommand Comm = new SqlCommand();
            Comm.Connection = Conn;

            /*------ Monitor And Control Multiple Users ------*/
            Comm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
            string strUserName = Convert.ToString(Comm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Comm.Dispose();
                    Comm.Dispose();
                    return;
                }
            }
            else
            {
                Comm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                Comm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                Comm.ExecuteNonQuery();
                Comm.Parameters.RemoveAt("@UserName");
            }

            bool bIsBOM = false, bIsGongDan = false;
            Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.dgvRMPurchase["Item No", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.dgvRMPurchase["Lot No", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = this.dgvRMPurchase["RM Customs Code", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = this.dgvRMPurchase["BGD No", iRowIndex].Value.ToString().Trim();
            Comm.CommandText = @"SELECT COUNT(*) FROM C_BOMDetail WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo AND [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
            int iRecord1 = Convert.ToInt32(Comm.ExecuteScalar());
            if (iRecord1 > 0) 
            {
                bIsBOM = true;
                Comm.CommandText = @"SELECT COUNT(*) FROM C_BOM AS T1 RIGHT JOIN (SELECT DISTINCT [Batch No] FROM C_BOMDetail WHERE [Item No] = @ItemNo AND " +
                                    "[Lot No] = @LotNo AND [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo) AS T2 ON T1.[Batch No] = T2.[Batch No] " +
                                    "WHERE T1.[GongDan Used Qty] > 0.0";
                int iRecord2 = Convert.ToInt32(Comm.ExecuteScalar());
                if (iRecord2 > 0) { bIsGongDan = true; }
            }
            Comm.Parameters.Clear();

            decimal dCustomsBal = 0.0M, dAvailableRMBal = 0.0M, dPOQty = 0.0M;                    
            string strTransactionType = this.dgvRMAdjustment["Transaction Type", 0].Value.ToString().Trim().ToUpper();
            string strBGDNo = this.dgvRMAdjustment["BGD No", 0].Value.ToString().Trim().ToUpper();
            string strCustomsEntryNo = this.dgvRMAdjustment["Customs Entry No", 0].Value.ToString().Trim().ToUpper();
            string strItemNo = this.dgvRMAdjustment["Item No", 0].Value.ToString().Trim().ToUpper();
            string strLotNo = this.dgvRMAdjustment["Lot No", 0].Value.ToString().Trim().ToUpper();
            string strRMCustomsCode = this.dgvRMAdjustment["RM Customs Code", 0].Value.ToString().Trim().ToUpper();
            string strRMCHNName = this.dgvRMAdjustment["RM CHN Name", 0].Value.ToString().Trim().ToUpper();
            decimal dPOInvoiceQty = 0.0M;
            string strPOInvoiceQty = this.dgvRMAdjustment["PO Invoice Qty", 0].Value.ToString().Trim();
            if (!String.IsNullOrEmpty(strPOInvoiceQty)) { dPOInvoiceQty = Math.Round(Convert.ToDecimal(strPOInvoiceQty), 6); }
            decimal dAmount = 0.0M;
            string strAmount = this.dgvRMAdjustment["Amount", 0].Value.ToString().Trim();
            if (!String.IsNullOrEmpty(strAmount)) { dAmount = Math.Round(Convert.ToDecimal(strAmount), 2); }
            decimal dRMPrice = 0.0M;
            string strRMPrice = this.dgvRMAdjustment["RM Price(CIF)", 0].Value.ToString().Trim();
            if (!String.IsNullOrEmpty(strRMPrice)) { dRMPrice = Math.Round(Convert.ToDecimal(strRMPrice), 6); }          
            string strRMCurrency = this.dgvRMAdjustment["RM Currency", 0].Value.ToString().Trim().ToUpper();
            string strOriginalCountry = this.dgvRMAdjustment["Original Country", 0].Value.ToString().Trim().ToUpper();
            string strOPMRcvdDate = this.dgvRMAdjustment["OPM Rcvd Date", 0].Value.ToString().Trim();
            string strCustomsRcvdDate = this.dgvRMAdjustment["Customs Rcvd Date", 0].Value.ToString().Trim();
            string strPONo = this.dgvRMAdjustment["PO No", 0].Value.ToString().Trim().ToUpper();
            string strNote = this.dgvRMAdjustment["Note", 0].Value.ToString().Trim().ToUpper();           

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Edit-Other") == 0)
            {
                #region //Update C_RMPurchase table
                Comm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = strTransactionType;
                Comm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = strCustomsEntryNo;
                Comm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = strRMCHNName;
                Comm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOInvoiceQty;
                Comm.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dAmount;
                Comm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = dRMPrice;
                Comm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strRMCurrency;
                Comm.Parameters.Add("@OriginalCountry", SqlDbType.NVarChar).Value = strOriginalCountry;
                if (String.IsNullOrEmpty(strOPMRcvdDate)) { Comm.Parameters.Add("@OPMRcvdDate", SqlDbType.DateTime).Value = DBNull.Value; }
                else { Comm.Parameters.Add("@OPMRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strOPMRcvdDate); }
                Comm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = strPONo;
                Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                Comm.CommandText = @"UPDATE C_RMPurchase SET [Transaction Type] = @TransactionType, [Customs Entry No] = @CustomsEntryNo, [RM CHN Name] = @RMCHNName, " +
                                    "[PO Invoice Qty] = @POInvoiceQty, [Amount] = @Amount, [RM Price(CIF)] = @RMPrice, [RM Currency] = @RMCurrency, " +
                                    "[Original Country] = @OriginalCountry, [OPM Rcvd Date] = @OPMRcvdDate, [PO No] = @PONo WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                SqlTransaction Trans1 = Conn.BeginTransaction();
                Comm.Transaction = Trans1;
                try
                {
                    Comm.ExecuteNonQuery();
                    Trans1.Commit();
                }
                catch (Exception)
                {
                    Trans1.Rollback();
                    Trans1.Dispose();
                    throw;
                }
                finally
                { Comm.Parameters.Clear(); }
                #endregion

                #region //Update C_RMBalance table
                if (String.Compare(dTable.Rows[0]["PO Invoice Qty"].ToString().Trim(), strPOInvoiceQty) != 0)
                {
                    decimal dFinalQty = dPOInvoiceQty - Convert.ToDecimal(dTable.Rows[0]["PO Invoice Qty"].ToString().Trim());
                    Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                    Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                    Comm.CommandText = @"SELECT [Customs Balance], [Available RM Balance], [PO Invoice Qty] FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                    SqlDataReader dReader = Comm.ExecuteReader();
                    while (dReader.Read())
                    {
                        if (dReader.HasRows)
                        {
                            string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dReader.GetValue(0).ToString().Trim()));
                            string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dReader.GetValue(1).ToString().Trim()));
                            string strPOInvQty = sqlLib.doubleFormat(Double.Parse(dReader.GetValue(2).ToString().Trim()));
                            dCustomsBal = Convert.ToDecimal(strCustomsBal) + dFinalQty;
                            dAvailableRMBal = Convert.ToDecimal(strAvailRMBal) + dFinalQty;
                            dPOQty = Convert.ToDecimal(strPOInvQty) + dFinalQty;
                        }
                    }
                    dReader.Close();
                    dReader.Dispose();
                    Comm.Parameters.Clear();

                    Comm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBal;
                    Comm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBal;
                    Comm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOQty;
                    Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                    Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                    Comm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance, " +
                                        "[PO Invoice Qty] = @POInvoiceQty WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                    SqlTransaction Trans2 = Conn.BeginTransaction();
                    Comm.Transaction = Trans2;
                    try
                    {
                        Comm.ExecuteNonQuery();
                        Trans2.Commit();
                    }
                    catch (Exception)
                    {
                        Trans2.Rollback();
                        Trans2.Dispose();
                        throw;
                    }
                    finally
                    { Comm.Parameters.Clear(); }
                }
                #endregion

                #region //Update RM Currency or RM Price(CIF) in C_BOMDetail table if revised, and indirectly update RM Total Cost(USD) in C_BOM table
                if (bIsBOM == true)
                {                    
                    if (String.Compare(strRMCurrency, dTable.Rows[0]["RM Currency"].ToString().Trim()) != 0 ||
                        Decimal.Compare(dRMPrice, Convert.ToDecimal(dTable.Rows[0]["RM Price(CIF)"].ToString().Trim())) != 0)
                    {
                        string strBatchNo = null, strFGNo = null, strSameBatch = null, strSameFG = null; int iLineNo = 0;
                        Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        Comm.CommandText = @"SELECT T2.[Batch No], T1.[FG No], T2.[Line No] FROM C_BOMDetail T2 LEFT JOIN C_BOM T1 ON T1.[Batch No] = T2.[Batch No] " +
                                            "WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo ORDER BY T2.[Batch No], T2.[Line No]";
                        SqlDataAdapter AdapterPrice = new SqlDataAdapter();
                        AdapterPrice.SelectCommand = Comm;
                        DataTable dtPrice = new DataTable();
                        AdapterPrice.Fill(dtPrice);
                        AdapterPrice.Dispose();
                        Comm.Parameters.Clear();

                        for (int i = 0; i < dtPrice.Rows.Count; i++)
                        {
                            strBatchNo = dtPrice.Rows[i]["Batch No"].ToString().Trim();
                            strFGNo = dtPrice.Rows[i]["FG No"].ToString().Trim();
                            iLineNo = Convert.ToInt32(dtPrice.Rows[i]["Line No"].ToString().Trim());

                            Comm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strRMCurrency;
                            Comm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = dRMPrice;
                            Comm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                            Comm.Parameters.Add("@LineNo", SqlDbType.Int).Value = iLineNo;
                            Comm.CommandText = @"UPDATE C_BOMDetail SET [RM Currency] = @RMCurrency, [RM Price] = @RMPrice WHERE [Batch No] = @BatchNo AND [Line No] = @LineNo";
                            SqlTransaction TransDetail = Conn.BeginTransaction();
                            Comm.Transaction = TransDetail;
                            try
                            {
                                Comm.ExecuteNonQuery();
                                TransDetail.Commit();
                            }
                            catch (Exception)
                            {
                                TransDetail.Rollback();
                                TransDetail.Dispose();
                                throw;
                            }
                            finally
                            { Comm.Parameters.Clear(); }

                            if (String.Compare(strBatchNo, strSameBatch) != 0 && String.Compare(strFGNo, strSameFG) != 0)
                            {
                                strSameBatch = strBatchNo;
                                strSameFG = strFGNo;

                                Comm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                                Comm.CommandText = @"SELECT [RM Currency], [RM Price], [RM Qty] FROM C_BOMDetail WHERE [Batch No] = @BatchNo";
                                SqlDataAdapter Adapter = new SqlDataAdapter();
                                Adapter.SelectCommand = Comm;
                                DataTable dt = new DataTable();
                                Adapter.Fill(dt);
                                Adapter.Dispose();

                                decimal dTotalRMCost = 0.0M;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    decimal dRMCost = 0.0M;
                                    if (String.Compare(dt.Rows[j]["RM Currency"].ToString().Trim(), "USD") != 0)
                                    { dRMCost = Math.Round(Convert.ToDecimal(dt.Rows[j]["RM Price"].ToString().Trim()) * this.GetExchangeRate(dt.Rows[j]["RM Currency"].ToString().Trim()) * Convert.ToDecimal(dt.Rows[j]["RM Qty"].ToString().Trim()), 3); }
                                    else
                                    { dRMCost = Math.Round(Convert.ToDecimal(dt.Rows[j]["RM Price"].ToString().Trim()) * Convert.ToDecimal(dt.Rows[j]["RM Qty"].ToString().Trim()), 3); }
                                    dTotalRMCost += dRMCost;
                                }
                                dt.Clear();
                                dt.Dispose();

                                Comm.Parameters.Clear();
                                Comm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(dTotalRMCost, 2);
                                Comm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                                Comm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                                Comm.CommandText = @"UPDATE C_BOM SET [Total RM Cost(USD)] = @TotalRMCost WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                                SqlTransaction TransMaster = Conn.BeginTransaction();
                                Comm.Transaction = TransMaster;
                                try
                                {
                                    Comm.ExecuteNonQuery();
                                    TransMaster.Commit();
                                }
                                catch (Exception)
                                {
                                    TransMaster.Rollback();
                                    TransMaster.Dispose();
                                    throw;
                                }
                                finally
                                { Comm.Parameters.Clear(); }
                            }
                        }
                        dtPrice.Clear();
                        dtPrice.Dispose();
                    }
                }
                #endregion

                #region //Insert the original data into C_RMPurchaseAdjustment table
                Comm.Parameters.Add("@TransactionType1", SqlDbType.NVarChar).Value = dTable.Rows[0]["Transaction Type"].ToString().Trim();
                Comm.Parameters.Add("@BGDNo1", SqlDbType.NVarChar).Value = dTable.Rows[0]["BGD No"].ToString().Trim();
                Comm.Parameters.Add("@CustomsEntryNo1", SqlDbType.NVarChar).Value = dTable.Rows[0]["Customs Entry No"].ToString().Trim();
                Comm.Parameters.Add("@ItemNo1", SqlDbType.NVarChar).Value = dTable.Rows[0]["Item No"].ToString().Trim();
                Comm.Parameters.Add("@LotNo1", SqlDbType.NVarChar).Value = dTable.Rows[0]["Lot No"].ToString().Trim();
                Comm.Parameters.Add("@RMCustomsCode1", SqlDbType.NVarChar).Value = dTable.Rows[0]["RM Customs Code"].ToString().Trim();
                Comm.Parameters.Add("@RMCHNName1", SqlDbType.NVarChar).Value = dTable.Rows[0]["RM CHN Name"].ToString().Trim();
                Comm.Parameters.Add("@POInvoiceQty1", SqlDbType.Decimal).Value = Convert.ToDecimal(dTable.Rows[0]["PO Invoice Qty"].ToString().Trim());
                Comm.Parameters.Add("@Amount1", SqlDbType.Decimal).Value = Convert.ToDecimal(dTable.Rows[0]["Amount"].ToString().Trim());
                Comm.Parameters.Add("@RMPrice1", SqlDbType.Decimal).Value = Convert.ToDecimal(dTable.Rows[0]["RM Price(CIF)"].ToString().Trim());
                Comm.Parameters.Add("@RMCurrency1", SqlDbType.NVarChar).Value = dTable.Rows[0]["RM Currency"].ToString().Trim();
                Comm.Parameters.Add("@OriginalCountry1", SqlDbType.NVarChar).Value = dTable.Rows[0]["Original Country"].ToString().Trim();
                if (String.IsNullOrEmpty(dTable.Rows[0]["OPM Rcvd Date"].ToString().Trim())) { Comm.Parameters.Add("@OPMRcvdDate1", SqlDbType.Date).Value = DBNull.Value; }
                else { Comm.Parameters.Add("@OPMRcvdDate1", SqlDbType.Date).Value = Convert.ToDateTime(dTable.Rows[0]["OPM Rcvd Date"].ToString().Trim()); }
                Comm.Parameters.Add("@CustomsRcvdDate1", SqlDbType.NVarChar).Value = dTable.Rows[0]["Customs Rcvd Date"].ToString().Trim();
                Comm.Parameters.Add("@PONo1", SqlDbType.NVarChar).Value = dTable.Rows[0]["PO No"].ToString().Trim();
                Comm.Parameters.Add("@CreatedDate1", SqlDbType.Date).Value = Convert.ToDateTime(dTable.Rows[0]["Created Date"].ToString().Trim());
                Comm.Parameters.Add("@Status1", SqlDbType.NVarChar).Value = "Edit-Other1";
                Comm.CommandText = @"INSERT INTO C_RMPurchaseAdjustment([Transaction Type], [BGD No], [Customs Entry No], [Item No], [Lot No], [RM Customs Code], " +
                                    "[RM CHN Name], [PO Invoice Qty], [Amount], [RM Price(CIF)], [RM Currency], [Original Country], [OPM Rcvd Date], " +
                                    "[Customs Rcvd Date], [PO No], [Created Date], [Status]) VALUES(@TransactionType1, @BGDNo1, @CustomsEntryNo1, " +
                                    "@ItemNo1, @LotNo1, @RMCustomsCode1, @RMCHNName1, @POInvoiceQty1, @Amount1, @RMPrice1, @RMCurrency1, @OriginalCountry1, " + 
                                    "@OPMRcvdDate1, @CustomsRcvdDate1, @PONo1, @CreatedDate1, @Status1)";
                SqlTransaction Trans3 = Conn.BeginTransaction();
                Comm.Transaction = Trans3;
                try
                {
                    Comm.ExecuteNonQuery();
                    Trans3.Commit();
                }
                catch (Exception)
                {
                    Trans3.Rollback();
                    Trans3.Dispose();
                    throw;
                }
                finally
                { Comm.Parameters.Clear(); }
                #endregion

                #region //Insert the final data into C_RMPurchaseAdjustment table
                Comm.Parameters.Add("@TransactionType2", SqlDbType.NVarChar).Value = strTransactionType;
                Comm.Parameters.Add("@BGDNo2", SqlDbType.NVarChar).Value = strBGDNo;
                Comm.Parameters.Add("@CustomsEntryNo2", SqlDbType.NVarChar).Value = strCustomsEntryNo;
                Comm.Parameters.Add("@ItemNo2", SqlDbType.NVarChar).Value = strItemNo;
                Comm.Parameters.Add("@LotNo2", SqlDbType.NVarChar).Value = strLotNo;
                Comm.Parameters.Add("@RMCustomsCode2", SqlDbType.NVarChar).Value = strRMCustomsCode;
                Comm.Parameters.Add("@RMCHNName2", SqlDbType.NVarChar).Value = strRMCHNName;
                Comm.Parameters.Add("@POInvoiceQty2", SqlDbType.Decimal).Value = dPOInvoiceQty;
                Comm.Parameters.Add("@Amount2", SqlDbType.Decimal).Value = dAmount;
                Comm.Parameters.Add("@RMPrice2", SqlDbType.Decimal).Value = dRMPrice;
                Comm.Parameters.Add("@RMCurrency2", SqlDbType.NVarChar).Value = strRMCurrency;
                Comm.Parameters.Add("@OriginalCountry2", SqlDbType.NVarChar).Value = strOriginalCountry;
                if (String.IsNullOrEmpty(strOPMRcvdDate)) { Comm.Parameters.Add("@OPMRcvdDate2", SqlDbType.Date).Value = DBNull.Value; }
                else { Comm.Parameters.Add("@OPMRcvdDate2", SqlDbType.Date).Value = Convert.ToDateTime(strOPMRcvdDate); }
                Comm.Parameters.Add("@CustomsRcvdDate2", SqlDbType.NVarChar).Value = strCustomsRcvdDate;
                Comm.Parameters.Add("@PONo2", SqlDbType.NVarChar).Value = strPONo;
                Comm.Parameters.Add("@CreatedDate2", SqlDbType.Date).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                Comm.Parameters.Add("@Status2", SqlDbType.NVarChar).Value = "Edit-Other2";
                Comm.Parameters.Add("@Note2", SqlDbType.NVarChar).Value = strNote;
                Comm.CommandText = @"INSERT INTO C_RMPurchaseAdjustment([Transaction Type], [BGD No], [Customs Entry No], [Item No], [Lot No], [RM Customs Code], " +
                                    "[RM CHN Name], [PO Invoice Qty], [Amount], [RM Price(CIF)], [RM Currency], [Original Country], [OPM Rcvd Date], " +
                                    "[Customs Rcvd Date], [PO No], [Created Date], [Status], [Note]) VALUES(@TransactionType2, @BGDNo2, @CustomsEntryNo2, " +
                                    "@ItemNo2, @LotNo2, @RMCustomsCode2, @RMCHNName2, @POInvoiceQty2, @Amount2, @RMPrice2, @RMCurrency2, @OriginalCountry2, " +
                                    "@OPMRcvdDate2, @CustomsRcvdDate2, @PONo2, @CreatedDate2, @Status2, @Note2)";
                SqlTransaction Trans4 = Conn.BeginTransaction();
                Comm.Transaction = Trans4;
                try
                {
                    Comm.ExecuteNonQuery();
                    Trans4.Commit();
                }
                catch (Exception)
                {
                    Trans4.Rollback();
                    Trans4.Dispose();
                    throw;
                }
                finally
                { Comm.Parameters.Clear(); }
                #endregion

                this.dgvRMPurchase["Transaction Type", iRowIndex].Value = strTransactionType;
                this.dgvRMPurchase["Customs Entry No", iRowIndex].Value = strCustomsEntryNo;
                this.dgvRMPurchase["RM CHN Name", iRowIndex].Value = strRMCHNName;
                this.dgvRMPurchase["PO Invoice Qty", iRowIndex].Value = dPOInvoiceQty;
                this.dgvRMPurchase["Amount", iRowIndex].Value = dAmount;
                this.dgvRMPurchase["RM Price(CIF)", iRowIndex].Value = dRMPrice;
                this.dgvRMPurchase["RM Currency", iRowIndex].Value = strRMCurrency;
                this.dgvRMPurchase["Original Country", iRowIndex].Value = strOriginalCountry;
                if (String.IsNullOrEmpty(strOPMRcvdDate)) { this.dgvRMPurchase["OPM Rcvd Date", iRowIndex].Value = DBNull.Value; }
                else { this.dgvRMPurchase["OPM Rcvd Date", iRowIndex].Value = Convert.ToDateTime(strOPMRcvdDate); }
                this.dgvRMPurchase["PO No", iRowIndex].Value = strPONo;
            }
            else if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Cancel") == 0)
            {
                if (bIsGongDan == false)
                {
                    #region //Delete data in C_RMPurchase table
                    Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                    Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                    Comm.CommandText = @"DELETE FROM C_RMPurchase WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                    SqlTransaction Trans1 = Conn.BeginTransaction();
                    Comm.Transaction = Trans1;
                    try
                    {
                        Comm.ExecuteNonQuery();
                        Trans1.Commit();
                    }
                    catch (Exception)
                    {
                        Trans1.Rollback();
                        Trans1.Dispose();
                        throw;
                    }
                    finally
                    { Comm.Parameters.Clear(); }
                    #endregion

                    #region //Update C_RMBalance table
                    if (strCustomsRcvdDate.Contains("/")) // If not date, no need to update RM Balance table's data
                    {
                        Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        Comm.CommandText = @"SELECT [Customs Balance], [Available RM Balance], [PO Invoice Qty] FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                        SqlDataReader dReader = Comm.ExecuteReader();
                        while (dReader.Read())
                        {
                            if (dReader.HasRows)
                            {
                                string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dReader.GetValue(0).ToString().Trim()));
                                string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dReader.GetValue(1).ToString().Trim()));
                                string strPOInvQty = sqlLib.doubleFormat(Double.Parse(dReader.GetValue(2).ToString().Trim()));
                                dCustomsBal = Convert.ToDecimal(strCustomsBal) - dPOInvoiceQty;
                                dAvailableRMBal = Convert.ToDecimal(strAvailRMBal) - dPOInvoiceQty;
                                dPOQty = Convert.ToDecimal(strPOInvQty) - dPOInvoiceQty;
                            }
                        }
                        dReader.Close();
                        dReader.Dispose();
                        Comm.Parameters.Clear();

                        Comm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBal;
                        Comm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBal;
                        Comm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOQty;
                        Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        Comm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance, " +
                                            "[PO Invoice Qty] = @POInvoiceQty WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                        SqlTransaction Trans2 = Conn.BeginTransaction();
                        Comm.Transaction = Trans2;
                        try
                        {
                            Comm.ExecuteNonQuery();
                            Trans2.Commit();
                        }
                        catch (Exception)
                        {
                            Trans2.Rollback();
                            Trans2.Dispose();
                            throw;
                        }
                        finally
                        { Comm.Parameters.Clear(); }
                    }
                    #endregion

                    #region //Insert into C_RMPurchaseAdjustment table
                    Comm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = strTransactionType;
                    Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                    Comm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = strCustomsEntryNo;
                    Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                    Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                    Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                    Comm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = strRMCHNName;
                    Comm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOInvoiceQty;
                    Comm.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dAmount;
                    Comm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = dRMPrice;
                    Comm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strRMCurrency;
                    Comm.Parameters.Add("@OriginalCountry", SqlDbType.NVarChar).Value = strOriginalCountry;
                    if (String.IsNullOrEmpty(strOPMRcvdDate)) { Comm.Parameters.Add("@OPMRcvdDate", SqlDbType.Date).Value = DBNull.Value; }
                    else { Comm.Parameters.Add("@OPMRcvdDate", SqlDbType.Date).Value = Convert.ToDateTime(strOPMRcvdDate); }
                    Comm.Parameters.Add("@CustomsRcvdDate", SqlDbType.NVarChar).Value = strCustomsRcvdDate;
                    Comm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = strPONo;
                    Comm.Parameters.Add("@CreatedDate", SqlDbType.Date).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                    Comm.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "Cancel";
                    Comm.Parameters.Add("@Note", SqlDbType.NVarChar).Value = strNote;
                    Comm.CommandText = @"INSERT INTO C_RMPurchaseAdjustment([Transaction Type], [BGD No], [Customs Entry No], [Item No], [Lot No], [RM Customs Code], " +
                                        "[RM CHN Name], [PO Invoice Qty], [Amount], [RM Price(CIF)], [RM Currency], [Original Country], [OPM Rcvd Date], " +
                                        "[Customs Rcvd Date], [PO No], [Created Date], [Status], [Note]) VALUES(@TransactionType, @BGDNo, @CustomsEntryNo, " +
                                        "@ItemNo, @LotNo, @RMCustomsCode, @RMCHNName, @POInvoiceQty, @Amount, @RMPrice, @RMCurrency, @OriginalCountry, " +
                                        "@OPMRcvdDate, @CustomsRcvdDate, @PONo, @CreatedDate, @Status, @Note)";

                    SqlTransaction Trans3 = Conn.BeginTransaction();
                    Comm.Transaction = Trans3;
                    try
                    {
                        Comm.ExecuteNonQuery();
                        Trans3.Commit();
                    }
                    catch (Exception)
                    {
                        Trans3.Rollback();
                        Trans3.Dispose();
                        throw;
                    }
                    finally
                    {
                        Comm.Parameters.Clear();
                        this.dgvRMPurchase.Rows.RemoveAt(iRowIndex);
                    }
                    #endregion
                }
                else
                { 
                    MessageBox.Show("Reject to cancel the data, because the related BOM has generated GongDan.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Comm.Dispose();
                    Conn.Close();
                    Conn.Dispose();
                    return;
                }
            }
            else // Edit-BGD
            {
                #region //Update BGD No in C_RMPurchase table
                Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                Comm.CommandText = @"UPDATE C_RMPurchase SET [BGD No] = @BGDNo WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                SqlTransaction Trans = Conn.BeginTransaction();
                Comm.Transaction = Trans;
                try
                {
                    Comm.ExecuteNonQuery();
                    Trans.Commit();
                }
                catch (Exception)
                {
                    Trans.Rollback();
                    Trans.Dispose();
                    throw;
                }
                finally
                { Comm.Parameters.Clear(); }
                #endregion

                if (strCustomsRcvdDate.Contains("/"))
                {
                    #region //Deduct the incorrect BGD No's RM Receipt Quantity in C_RMBalance table
                    Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                    Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = dTable.Rows[0]["BGD No"].ToString().Trim();
                    Comm.CommandText = @"SELECT [Customs Balance], [Available RM Balance], [PO Invoice Qty] FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                    SqlDataReader dReader1 = Comm.ExecuteReader();
                    while (dReader1.Read())
                    {
                        if (dReader1.HasRows)
                        {
                            string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dReader1.GetValue(0).ToString().Trim()));
                            string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dReader1.GetValue(1).ToString().Trim()));
                            string strPOInvQty = sqlLib.doubleFormat(Double.Parse(dReader1.GetValue(2).ToString().Trim()));
                            dCustomsBal = Convert.ToDecimal(strCustomsBal) - dPOInvoiceQty;
                            dAvailableRMBal = Convert.ToDecimal(strAvailRMBal) - dPOInvoiceQty;
                            dPOQty = Convert.ToDecimal(strPOInvQty) - dPOInvoiceQty;
                        }
                    }
                    dReader1.Close();
                    dReader1.Dispose();
                    Comm.Parameters.Clear();

                    Comm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBal;
                    Comm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBal;
                    Comm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOQty;
                    Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                    Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = dTable.Rows[0]["BGD No"].ToString().Trim();
                    Comm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance, " +
                                        "[PO Invoice Qty] = @POInvoiceQty WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                    SqlTransaction Trans1 = Conn.BeginTransaction();
                    Comm.Transaction = Trans1;
                    try
                    {
                        Comm.ExecuteNonQuery();
                        Trans1.Commit();
                    }
                    catch (Exception)
                    {
                        Trans1.Rollback();
                        Trans1.Dispose();
                        throw;
                    }
                    finally
                    { Comm.Parameters.Clear(); }
                    #endregion

                    #region //Add the correct BGD No's RM Receipt Quantity in C_RMBalance table
                    Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                    Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                    Comm.CommandText = @"SELECT COUNT(*) FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                    int iCount = Convert.ToInt32(Comm.ExecuteScalar());
                    if (iCount > 0)
                    {
                        Comm.CommandText = @"SELECT [Customs Balance], [Available RM Balance], [PO Invoice Qty] FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                        SqlDataReader dReader2 = Comm.ExecuteReader();
                        while (dReader2.Read())
                        {
                            if (dReader2.HasRows)
                            {
                                string strCustomsBal = sqlLib.doubleFormat(Double.Parse(dReader2.GetValue(0).ToString().Trim()));
                                string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(dReader2.GetValue(1).ToString().Trim()));
                                string strPOInvQty = sqlLib.doubleFormat(Double.Parse(dReader2.GetValue(2).ToString().Trim()));
                                dCustomsBal = Convert.ToDecimal(strCustomsBal) + dPOInvoiceQty;
                                dAvailableRMBal = Convert.ToDecimal(strAvailRMBal) + dPOInvoiceQty;
                                dPOQty = Convert.ToDecimal(strPOInvQty) + dPOInvoiceQty;
                            }
                        }
                        dReader2.Close();
                        dReader2.Dispose();
                        Comm.Parameters.Clear();

                        Comm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBal;
                        Comm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBal;
                        Comm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOQty;
                        Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                        Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        Comm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance, " +
                                            "[PO Invoice Qty] = @POInvoiceQty WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                    }
                    else
                    {
                        Comm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dPOInvoiceQty;
                        Comm.CommandText = @"INSERT INTO C_RMBalance([RM Customs Code], [BGD No], [Customs Balance], [Available RM Balance], [PO Invoice Qty]) " +
                                            "VALUES(@RMCustomsCode, @BGDNo, @CustomsBalance, @CustomsBalance, @CustomsBalance)";
                    }
                    SqlTransaction Trans2 = Conn.BeginTransaction();
                    Comm.Transaction = Trans2;
                    try
                    {
                        Comm.ExecuteNonQuery();
                        Trans2.Commit();
                    }
                    catch (Exception)
                    {
                        Trans2.Rollback();
                        Trans2.Dispose();
                        throw;
                    }
                    finally
                    { Comm.Parameters.Clear(); }
                    #endregion                  
                }

                #region //Update BGD No in C_BOMDetail table
                if (bIsBOM == true)
                {
                    Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                    Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                    Comm.CommandText = @"SELECT DISTINCT [Batch No], [Line No] FROM C_BOMDetail WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                    SqlDataAdapter AdapterBGD = new SqlDataAdapter();
                    AdapterBGD.SelectCommand = Comm;
                    DataTable dtBGD = new DataTable();
                    AdapterBGD.Fill(dtBGD);
                    AdapterBGD.Dispose();

                    for (int i = 0; i < dtBGD.Rows.Count; i++)
                    {
                        //one Item No & Lot No mapping multi-Batch No
                        Comm.Parameters.Clear();
                        Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        Comm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dtBGD.Rows[i]["Batch No"].ToString().Trim();
                        Comm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dtBGD.Rows[i]["Line No"].ToString().Trim());
                        Comm.CommandText = @"Update C_BOMDetail SET [BGD No] = @BGDNo WHERE [Batch No] = @BatchNo AND [Line No] = @LineNo";
                        SqlTransaction Trans3 = Conn.BeginTransaction();
                        Comm.Transaction = Trans3;
                        try
                        {
                            Comm.ExecuteNonQuery();
                            Trans3.Commit();
                        }
                        catch (Exception)
                        {
                            Trans3.Rollback();
                            Trans3.Dispose();
                            throw;
                        }
                        finally
                        { Comm.Parameters.Clear(); }
                    }
                    dtBGD.Clear();
                    dtBGD.Dispose();
                }
                #endregion

                #region //Insert into C_RMPurchaseAdjustment table
                Comm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = strTransactionType;
                Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                Comm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = strCustomsEntryNo;
                Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                Comm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                Comm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = strRMCHNName;
                Comm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOInvoiceQty;
                Comm.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dAmount;
                Comm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = dRMPrice;
                Comm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = strRMCurrency;
                Comm.Parameters.Add("@OriginalCountry", SqlDbType.NVarChar).Value = strOriginalCountry;
                if (String.IsNullOrEmpty(strOPMRcvdDate)) { Comm.Parameters.Add("@OPMRcvdDate", SqlDbType.Date).Value = DBNull.Value; }
                else { Comm.Parameters.Add("@OPMRcvdDate", SqlDbType.Date).Value = Convert.ToDateTime(strOPMRcvdDate); }
                Comm.Parameters.Add("@CustomsRcvdDate", SqlDbType.NVarChar).Value = strCustomsRcvdDate;
                Comm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = strPONo;
                Comm.Parameters.Add("@CreatedDate", SqlDbType.Date).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                Comm.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "Edit-BGD";
                Comm.Parameters.Add("@Note", SqlDbType.NVarChar).Value = "The initial BGD No: " + dTable.Rows[0]["BGD No"].ToString().Trim();
                Comm.CommandText = @"INSERT INTO C_RMPurchaseAdjustment([Transaction Type], [BGD No], [Customs Entry No], [Item No], [Lot No], [RM Customs Code], " +
                                    "[RM CHN Name], [PO Invoice Qty], [Amount], [RM Price(CIF)], [RM Currency], [Original Country], [OPM Rcvd Date], " +
                                    "[Customs Rcvd Date], [PO No], [Created Date], [Status], [Note]) VALUES(@TransactionType, @BGDNo, @CustomsEntryNo, " +
                                    "@ItemNo, @LotNo, @RMCustomsCode, @RMCHNName, @POInvoiceQty, @Amount, @RMPrice, @RMCurrency, @OriginalCountry, " +
                                    "@OPMRcvdDate, @CustomsRcvdDate, @PONo, @CreatedDate, @Status, @Note)";
                SqlTransaction Trans4 = Conn.BeginTransaction(IsolationLevel.ReadCommitted);
                Comm.Transaction = Trans4;
                try
                {
                    Comm.ExecuteNonQuery();
                    Trans4.Commit();
                }
                catch (Exception)
                {
                    Trans4.Rollback();
                    Trans4.Dispose();
                    throw;
                }
                finally
                { Comm.Parameters.Clear(); }
                #endregion

                this.dgvRMPurchase["BGD No", iRowIndex].Value = strBGDNo;
            }

            Comm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
            Comm.ExecuteNonQuery();
            Comm.Dispose();
            if (MessageBox.Show("Successfully adjust.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            { this.dgvRMAdjustment.DataSource = DBNull.Value; }         
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        private decimal GetExchangeRate(string strExchange)
        {
            decimal dExchangeRate = 0.0M;
            SqlConnection getConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (getConn.State == ConnectionState.Closed) { getConn.Open(); }

            SqlCommand getComm = new SqlCommand();
            getComm.Connection = getConn;

            getComm.Parameters.Add("@Object", SqlDbType.NVarChar).Value = strExchange.Trim() + ":USD";
            getComm.CommandText = @"SELECT [Rate] FROM B_ExchangeRate WHERE [Object] = @Object";

            SqlDataReader getReader = getComm.ExecuteReader();
            while (getReader.Read())
            {
                if (getReader.HasRows)
                {
                    if (String.IsNullOrEmpty(getReader.GetValue(0).ToString().Trim())) { dExchangeRate = 0.0M; }
                    else { dExchangeRate = Convert.ToDecimal(getReader.GetValue(0).ToString().Trim()); }
                }
            }
            getReader.Close();
            getReader.Dispose();
            getComm.Parameters.Clear();
            getComm.Dispose();
            if (getConn.State == ConnectionState.Open)
            {
                getConn.Close();
                getConn.Dispose();
            }
            return dExchangeRate;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvRMPurchase.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No){ return; }
            if (this.dgvRMPurchase.Columns[0].HeaderText != "全选")
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Application.Workbooks.Add(true);

                bool bJudge = false;
                int iRow = 2;
                for (int i = 0; i < this.dgvRMPurchase.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMPurchase[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        bJudge = true;
                        excel.get_Range(excel.Cells[iRow, 1], excel.Cells[iRow, 8]).NumberFormatLocal = "@";
                        excel.Cells[iRow, 1] = this.dgvRMPurchase["Item No", i].Value.ToString().Trim();
                        excel.Cells[iRow, 2] = this.dgvRMPurchase["Lot No", i].Value.ToString().Trim();
                        excel.Cells[iRow, 3] = this.dgvRMPurchase["RM Customs Code", i].Value.ToString().Trim();
                        excel.Cells[iRow, 4] = this.dgvRMPurchase["BGD No", i].Value.ToString().Trim();
                        excel.Cells[iRow, 5] = this.dgvRMPurchase["PO Invoice Qty", i].Value.ToString().Trim();
                        string strOPMRcvdDate = this.dgvRMPurchase["OPM Rcvd Date", i].Value.ToString().Trim();
                        if (String.IsNullOrEmpty(strOPMRcvdDate)) { excel.Cells[iRow, 6] = DBNull.Value; }
                        else { excel.Cells[iRow, 6] = Convert.ToDateTime(strOPMRcvdDate).ToString("M/d/yyyy"); }
                        excel.Cells[iRow, 7] = this.dgvRMPurchase["Customs Rcvd Date", i].Value.ToString().Trim();
                        excel.Cells[iRow, 8] = this.dgvRMPurchase["Remark", i].Value.ToString().Trim();
                        excel.Cells[iRow, 9] = this.dgvRMPurchase["Voucher ID", i].Value.ToString().Trim();
                        excel.Cells[iRow, 10] = this.dgvRMPurchase["Voucher Status", i].Value.ToString().Trim();
                        string strPassGateDate = this.dgvRMPurchase["Gate PassThrough Date", i].Value.ToString().Trim();
                        if (String.IsNullOrEmpty(strPassGateDate)) { excel.Cells[iRow, 11] = DBNull.Value; }
                        else { excel.Cells[iRow, 11] = Convert.ToDateTime(strPassGateDate).ToString("M/d/yyyy"); }
                        string strCreatedDate = this.dgvRMPurchase["Created Date", i].Value.ToString().Trim();
                        if (String.IsNullOrEmpty(strCreatedDate)) { excel.Cells[iRow, 12] = DBNull.Value; }
                        else { excel.Cells[iRow, 12] = Convert.ToDateTime(strCreatedDate).ToString("M/d/yyyy"); }
                        iRow++;
                    }
                }

                if (bJudge)
                {
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 11]).NumberFormatLocal = "@";
                    excel.Cells[1, 1] = this.dgvRMPurchase.Columns["Item No"].HeaderText.ToString().Trim();
                    excel.Cells[1, 2] = this.dgvRMPurchase.Columns["Lot No"].HeaderText.ToString().Trim();
                    excel.Cells[1, 3] = this.dgvRMPurchase.Columns["RM Customs Code"].HeaderText.ToString().Trim();
                    excel.Cells[1, 4] = this.dgvRMPurchase.Columns["BGD No"].HeaderText.ToString().Trim();
                    excel.Cells[1, 5] = this.dgvRMPurchase.Columns["PO Invoice Qty"].HeaderText.ToString().Trim();
                    excel.Cells[1, 6] = this.dgvRMPurchase.Columns["OPM Rcvd Date"].HeaderText.ToString().Trim();
                    excel.Cells[1, 7] = this.dgvRMPurchase.Columns["Customs Rcvd Date"].HeaderText.ToString().Trim();
                    excel.Cells[1, 8] = this.dgvRMPurchase.Columns["Remark"].HeaderText.ToString().Trim();
                    excel.Cells[1, 9] = this.dgvRMPurchase.Columns["Voucher ID"].HeaderText.ToString().Trim();
                    excel.Cells[1, 10] = this.dgvRMPurchase.Columns["Voucher Status"].HeaderText.ToString().Trim();
                    excel.Cells[1, 11] = this.dgvRMPurchase.Columns["Gate PassThrough Date"].HeaderText.ToString().Trim();
                    excel.Cells[1, 12] = this.dgvRMPurchase.Columns["Created Date"].HeaderText.ToString().Trim();

                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 12]).Font.Bold = true;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 12]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, 12]).Font.Name = "Verdana";
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, 12]).Font.Size = 9;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, 12]).Borders.LineStyle = 1;
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Visible = true;
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
            }
            else
            {
                int PageRow = 65536;
                DataTable dtCopy = dvFillDGV.ToTable().Copy();
                int iPageCount = (int)(dtCopy.Rows.Count / PageRow);
                if (iPageCount * PageRow < dtCopy.Rows.Count) { iPageCount += 1; }
                try
                {
                    for (int m = 1; m <= iPageCount; m++)
                    {
                        string strPath = System.Windows.Forms.Application.StartupPath + "\\RM Receiving Data " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
                        if (File.Exists(strPath)) { File.Delete(strPath); }
                        Thread.Sleep(1000);
                        StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < dtCopy.Columns.Count; n++)
                        { sb.Append(dtCopy.Columns[n].ColumnName.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);

                        for (int i = (m - 1) * PageRow; i < dtCopy.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < dtCopy.Columns.Count; j++)
                            {
                                if (j == 1) { sb.Append("'" + dtCopy.Rows[i][j].ToString().Trim() + "\t"); }
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
            try
            {
                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge);
            }
            catch (Exception)
            {
                MessageBox.Show("Upload error, please try again.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void ImportExcelData(string strFilePath, bool bJudge)
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
                myTable.Clear();
                myTable.Dispose();
                return;
            }

            SqlConnection oneConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (oneConn.State == ConnectionState.Closed) { oneConn.Open(); }
            SqlCommand oneComm = new SqlCommand();
            oneComm.Connection = oneConn;

            DialogResult dlgR = MessageBox.Show("Please choose the upload condition:\n[Yes] : Mass update Customs Rcvd Date;\n[No] : Mass update Voucher ID, Status and Gate PassThrough Date;\n[Cancel] : Reject to update.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dlgR == DialogResult.Yes)
            {
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

                for (int i = 0; i < myTable.Rows.Count; i++)
                {
                    string strCustomsRcvdDate = myTable.Rows[i]["Customs Rcvd Date"].ToString().Trim();
                    string strPOInvoiceQty = myTable.Rows[i]["PO Invoice Qty"].ToString().Trim();
                    decimal dPOQuantity = 0.0M;
                    if (!String.IsNullOrEmpty(strPOInvoiceQty)) { dPOQuantity = Math.Round(Convert.ToDecimal(strPOInvoiceQty), 6); }
                    if (strCustomsRcvdDate.Contains("/"))
                    {
                        oneComm.Parameters.Clear();
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                        oneComm.CommandText = @"SELECT [Customs Rcvd Date] FROM C_RMPurchase WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                        string strCusRcvdDate = Convert.ToString(oneComm.ExecuteScalar());

                        oneComm.Parameters.Clear();
                        oneComm.Parameters.Add("@CustomsRcvdDate", SqlDbType.NVarChar).Value = strCustomsRcvdDate;
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                        oneComm.CommandText = @"UPDATE C_RMPurchase SET [Customs Rcvd Date] = @CustomsRcvdDate WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                        SqlTransaction oneTrans1 = oneConn.BeginTransaction();
                        oneComm.Transaction = oneTrans1;
                        try
                        {
                            oneComm.ExecuteNonQuery();
                            oneTrans1.Commit();
                        }
                        catch (Exception)
                        {
                            oneComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                            oneComm.ExecuteNonQuery();

                            oneTrans1.Rollback();
                            oneTrans1.Dispose();
                            throw;
                        }
                        finally
                        { oneComm.Parameters.Clear(); }

                        if (!strCusRcvdDate.Contains("/")) // Avoid to re-update RM Balance
                        {
                            oneComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Customs Code"].ToString().Trim().ToUpper();
                            oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                            oneComm.CommandText = @"SELECT COUNT(*) FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                            int iCount = Convert.ToInt32(oneComm.ExecuteScalar());
                            if (iCount == 0)
                            {
                                oneComm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dPOQuantity;
                                oneComm.CommandText = @"INSERT INTO C_RMBalance([RM Customs Code], [BGD No], [Customs Balance], [Available RM Balance], [PO Invoice Qty]) " +
                                                       "VALUES(@RMCustomsCode, @BGDNo, @CustomsBalance, @CustomsBalance, @CustomsBalance)";
                            }
                            else
                            {
                                decimal dCustomsBalance = 0.0M, dAvailableRMBalance = 0.0M, dPOInvoiceQty = 0.0M;
                                oneComm.CommandText = @"SELECT [Customs Balance], [Available RM Balance], [PO Invoice Qty] FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                                SqlDataReader SqlReader = oneComm.ExecuteReader();
                                while (SqlReader.Read())
                                {
                                    if (SqlReader.HasRows)
                                    {
                                        string strCustomsBal = sqlLib.doubleFormat(Double.Parse(SqlReader.GetValue(0).ToString().Trim()));
                                        string strAvailRMBal = sqlLib.doubleFormat(Double.Parse(SqlReader.GetValue(1).ToString().Trim()));
                                        string strPOInvQty = sqlLib.doubleFormat(Double.Parse(SqlReader.GetValue(2).ToString().Trim()));
                                        dCustomsBalance = Convert.ToDecimal(strCustomsBal);
                                        dAvailableRMBalance = Convert.ToDecimal(strAvailRMBal);
                                        dPOInvoiceQty = Convert.ToDecimal(strPOInvQty);
                                    }
                                }
                                SqlReader.Close();
                                SqlReader.Dispose();

                                oneComm.Parameters.Clear();
                                oneComm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBalance + dPOQuantity;
                                oneComm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBalance + dPOQuantity;
                                oneComm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOInvoiceQty + dPOQuantity;
                                oneComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Customs Code"].ToString().Trim().ToUpper();
                                oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                                oneComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance, " +
                                                       "[PO Invoice Qty] = @POInvoiceQty WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                            }
                            SqlTransaction oneTrans2 = oneConn.BeginTransaction();
                            oneComm.Transaction = oneTrans2;
                            try
                            {
                                oneComm.ExecuteNonQuery();
                                oneTrans2.Commit();
                            }
                            catch (Exception)
                            {
                                oneComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                                oneComm.ExecuteNonQuery();

                                oneTrans2.Rollback();
                                oneTrans2.Dispose();
                                throw;
                            }
                            finally
                            { oneComm.Parameters.Clear(); }
                        }
                    }
                    else
                    {
                        oneComm.Parameters.Clear();
                        oneComm.Parameters.Add("@CustomsRcvdDate", SqlDbType.NVarChar).Value = strCustomsRcvdDate;
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                        oneComm.CommandText = @"UPDATE C_RMPurchase SET [Customs Rcvd Date] = @CustomsRcvdDate WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                        oneComm.ExecuteNonQuery();
                        oneComm.Parameters.Clear();
                    }
                }
                oneComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                oneComm.ExecuteNonQuery();
                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);    
            }
            else if (dlgR == DialogResult.No)
            {
                for (int j = 0; j < myTable.Rows.Count; j++)
                {
                    oneComm.Parameters.Add("@VoucherID", SqlDbType.NVarChar).Value = myTable.Rows[j]["Voucher ID"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@VoucherStatus", SqlDbType.NVarChar).Value = myTable.Rows[j]["Voucher Status"].ToString().Trim().ToUpper();
                    string strGatePassThroughDate = myTable.Rows[j]["Gate PassThrough Date"].ToString().Trim();
                    if (String.IsNullOrEmpty(strGatePassThroughDate)) { oneComm.Parameters.Add("@GatePassThroughDate", SqlDbType.Date).Value = DBNull.Value; }
                    else { oneComm.Parameters.Add("@GatePassThroughDate", SqlDbType.Date).Value = Convert.ToDateTime(strGatePassThroughDate); }
                    oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Item No"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Lot No"].ToString().Trim().ToUpper();

                    oneComm.CommandText = @"UPDATE C_RMPurchase SET [Voucher ID] = @VoucherID, [Voucher Status] = @VoucherStatus, " +
                                           "[Gate PassThrough Date] = @GatePassThroughDate WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                    oneComm.ExecuteNonQuery();
                    oneComm.Parameters.Clear();
                }
                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);    
            }

            oneComm.Dispose();
            myTable.Clear();
            myTable.Dispose();
            if (oneConn.State == ConnectionState.Open)
            {
                oneConn.Close();
                oneConn.Dispose();
            }                       
        }
    }
}
