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
    public partial class RMReceivingForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable dTable = new DataTable();
        SqlLib sqlLib = new SqlLib();
        int iRowIndex = 0;

        private static RMReceivingForm RMReceivingFrm;
        public RMReceivingForm() { InitializeComponent(); }
        public static RMReceivingForm CreateInstance()
        {
            if (RMReceivingFrm == null || RMReceivingFrm.IsDisposed) { RMReceivingFrm = new RMReceivingForm(); }
            return RMReceivingFrm;
        }

        private void RMReceivingForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";        
        }

        private void RMReceivingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dTable.Dispose();
            sqlLib.Dispose(0);
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        { this.dtpFrom.CustomFormat = null; }
        private void dtpFrom_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; } }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        { this.dtpTo.CustomFormat = null; }
        private void dtpTo_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; } }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string strFieldName = this.cmbFieldName.Text.Trim();
            string strTxtField = this.txtFieldName.Text.Trim();
            if (String.IsNullOrEmpty(strFieldName) && !String.IsNullOrEmpty(strTxtField))
            { MessageBox.Show("Please select field name first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!String.IsNullOrEmpty(strFieldName) && String.IsNullOrEmpty(strTxtField)) 
            { MessageBox.Show("Please input the value of " + strFieldName + " field.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            this.dgvRMAdjustment.DataSource = DBNull.Value;
            string strBrowse = @"SELECT * FROM C_RMReceiving WHERE";
            if (!String.IsNullOrEmpty(strFieldName) && !String.IsNullOrEmpty(strTxtField))
            { strBrowse = @"SELECT * FROM C_RMReceiving WHERE [" + strFieldName + "] = '" + strTxtField + "' AND"; }
            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1) {
                        MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    else { strBrowse += " [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [Created Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else { strBrowse += " [Created Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strBrowse += " [Created Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND"; }
            }
            if (String.Compare(strBrowse.Substring(strBrowse.Trim().Length - 5, 5), "WHERE") == 0)
            { MessageBox.Show("Please select the filter condition.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            strBrowse = strBrowse.Remove(strBrowse.Trim().Length - 3) + " ORDER BY [Created Date] DESC, [Item No], [Lot No]";

            SqlConnection browseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (browseConn.State == ConnectionState.Closed) { browseConn.Open(); }
            SqlDataAdapter browseAdapterR = new SqlDataAdapter(strBrowse, browseConn);
            DataTable dtFillRmRcvd = new DataTable();
            browseAdapterR.Fill(dtFillRmRcvd);
            browseAdapterR.Dispose();

            if (dtFillRmRcvd.Rows.Count == 0)
            {
                dtFillRmRcvd.Clear();                
                this.dgvRMReceiving.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvRMReceiving.DataSource = dtFillRmRcvd;
                this.dgvRMReceiving.Rows[0].HeaderCell.Value = 1;
                for (int i = 2; i < this.dgvRMReceiving.ColumnCount; i++)
                { this.dgvRMReceiving.Columns[i].ReadOnly = true; }
                this.dgvRMReceiving.Columns[3].Frozen = true;
            }
            if (browseConn.State == ConnectionState.Open)
            {
                browseConn.Close();
                browseConn.Dispose();
            }            
        }

        private void dgvRMReceiving_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvRMReceiving.RowCount; i++) { this.dgvRMReceiving[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvRMReceiving.RowCount; i++) { this.dgvRMReceiving[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvRMReceiving.RowCount; i++)
                    {
                        if (String.Compare(this.dgvRMReceiving[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvRMReceiving[0, i].Value = true; }
                        else { this.dgvRMReceiving[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }

            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvRMReceiving_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvRMReceiving.RowCount == 0) { return; }
            if (this.dgvRMReceiving.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvRMReceiving.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMReceiving[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvRMReceiving.RowCount && iCount > 0)
                { this.dgvRMReceiving.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvRMReceiving.RowCount)
                { this.dgvRMReceiving.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvRMReceiving.Columns[0].HeaderText = "Select"; }
            }
        }

        private void dgvRMReceiving_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataTable AdjustTbl = new DataTable();
                for (int i = 2; i < this.dgvRMReceiving.ColumnCount; i++)
                { AdjustTbl.Columns.Add(this.dgvRMReceiving.Columns[i].Name, this.dgvRMReceiving.Columns[i].ValueType); }               

                iRowIndex = this.dgvRMReceiving.CurrentRow.Index;
                string strGoodsRcvdDate = this.dgvRMReceiving["Goods Rcvd Date", iRowIndex].Value.ToString().Trim();
                string strCustomsRcvdDate = this.dgvRMReceiving["Customs Rcvd Date", iRowIndex].Value.ToString().Trim();
                string strGateInDate = this.dgvRMReceiving["Gate In Date", iRowIndex].Value.ToString().Trim();

                DataRow dr = AdjustTbl.NewRow();
                dr["Transaction Type"] = this.dgvRMReceiving["Transaction Type", iRowIndex].Value.ToString().Trim();
                dr["Customs Entry No"] = this.dgvRMReceiving["Customs Entry No", iRowIndex].Value.ToString().Trim();
                dr["Inbound Delivery No"] = this.dgvRMReceiving["Inbound Delivery No", iRowIndex].Value.ToString().Trim();
                dr["Item No"] = this.dgvRMReceiving["Item No", iRowIndex].Value.ToString().Trim();
                dr["Item Description"] = this.dgvRMReceiving["Item Description", iRowIndex].Value.ToString().Trim();
                dr["Lot No"] = this.dgvRMReceiving["Lot No", iRowIndex].Value.ToString().Trim();
                dr["BGD No"] = this.dgvRMReceiving["BGD No", iRowIndex].Value.ToString().Trim();
                dr["RM EHB"] = this.dgvRMReceiving["RM EHB", iRowIndex].Value.ToString().Trim(); 
                dr["RM CHN Name"] = this.dgvRMReceiving["RM CHN Name", iRowIndex].Value.ToString().Trim();                
                dr["PO Invoice Qty"] = Convert.ToDecimal(this.dgvRMReceiving["PO Invoice Qty", iRowIndex].Value.ToString().Trim());
                dr["PO Invoice Amount"] = Convert.ToDecimal(this.dgvRMReceiving["PO Invoice Amount", iRowIndex].Value.ToString().Trim()); 
                dr["PO Unit Price"] = Convert.ToDecimal(this.dgvRMReceiving["PO Unit Price", iRowIndex].Value.ToString().Trim()); 
                dr["PO Currency"] = this.dgvRMReceiving["PO Currency", iRowIndex].Value.ToString().Trim();
                dr["SAP PO No"] = this.dgvRMReceiving["SAP PO No", iRowIndex].Value.ToString().Trim();
                dr["Rcvd SLOC"] = this.dgvRMReceiving["Rcvd SLOC", iRowIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strGoodsRcvdDate)) { dr["Goods Rcvd Date"] = DBNull.Value; }
                else { dr["Goods Rcvd Date"] = Convert.ToDateTime(strGoodsRcvdDate); }
                dr["ShipFrom Country"] = this.dgvRMReceiving["ShipFrom Country", iRowIndex].Value.ToString().Trim();
                dr["Country of Origin"] = this.dgvRMReceiving["Country of Origin", iRowIndex].Value.ToString().Trim();             
                if (String.IsNullOrEmpty(strCustomsRcvdDate)) { dr["Customs Rcvd Date"] = DBNull.Value; }
                else { dr["Customs Rcvd Date"] = Convert.ToDateTime(strCustomsRcvdDate); }                                            
                dr["Receipts ID"] = this.dgvRMReceiving["Receipts ID", iRowIndex].Value.ToString().Trim();
                dr["Receipts Status"] = this.dgvRMReceiving["Receipts Status", iRowIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strGateInDate)) { dr["Gate In Date"] = DBNull.Value; }
                else { dr["Gate In Date"] = Convert.ToDateTime(strGateInDate); }
                dr["Creater"] = this.dgvRMReceiving["Creater", iRowIndex].Value.ToString().Trim();
                dr["Created Date"] = this.dgvRMReceiving["Created Date", iRowIndex].Value.ToString().Trim();
                AdjustTbl.Rows.Add(dr);

                dTable = AdjustTbl.Copy();
                this.dgvRMAdjustment.DataSource = DBNull.Value;
                this.dgvRMAdjustment.DataSource = AdjustTbl;
                this.dgvRMAdjustment.ReadOnly = true;
                this.dgvRMAdjustment.Columns[3].Frozen = true;
                if (this.cmbAdjustment.SelectedIndex > 0) { this.cmbAdjustment.Text = String.Empty; }
            }
        }

        private void cmbAdjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvRMAdjustment.Rows.Count == 0) { return; }
            this.dgvRMAdjustment.ReadOnly = true;
            for (int k = 0; k < this.dgvRMAdjustment.ColumnCount; k++)
            { this.dgvRMAdjustment.Columns[k].HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.HighlightText); }

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = Color.FromArgb(178, 235, 140);
            this.dgvRMAdjustment.EnableHeadersVisualStyles = false;
            this.dgvRMAdjustment.ReadOnly = false;

            if (this.cmbAdjustment.SelectedIndex == 0) { this.dgvRMAdjustment.ReadOnly = true; }
            this.dgvRMAdjustment.Columns["Item Description"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Invoice Qty"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Invoice Amount"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Unit Price"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Currency"].Visible = false;
            this.dgvRMAdjustment.Columns["SAP PO No"].Visible = false;
            this.dgvRMAdjustment.Columns["Rcvd SLOC"].Visible = false;
            this.dgvRMAdjustment.Columns["Creater"].Visible = false;
            this.dgvRMAdjustment.Columns["Created Date"].Visible = false;

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Edition") == 0)
            {
                this.dgvRMAdjustment.Columns["Inbound Delivery No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["Item No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["Lot No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["BGD No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["RM EHB"].ReadOnly = true;               

                this.dgvRMAdjustment.Columns["Inbound Delivery No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["Item No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["Lot No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["BGD No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["RM EHB"].HeaderCell.Style = cellStyle;              
            }

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Deletion") == 0)
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
                MessageBox.Show("Please select 'Adjustment Items' first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            bool bIsBOM = false;
            Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.dgvRMReceiving["Item No", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.dgvRMReceiving["Lot No", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = this.dgvRMReceiving["RM EHB", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = this.dgvRMReceiving["BGD No", iRowIndex].Value.ToString().Trim();
            Comm.CommandText = @"SELECT COUNT(*) FROM C_BOMDetail WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo AND [RM EHB] = @RMEHB AND [BGD No] = @BGDNo";
            int iRecord1 = Convert.ToInt32(Comm.ExecuteScalar());
            if (iRecord1 > 0) { bIsBOM = true; }
            Comm.Parameters.Clear();
                 
            string strTransactionType = this.dgvRMAdjustment["Transaction Type", 0].Value.ToString().Trim().ToUpper();
            string strCustomsEntryNo = this.dgvRMAdjustment["Customs Entry No", 0].Value.ToString().Trim().ToUpper();
            string strItemNo = this.dgvRMAdjustment["Item No", 0].Value.ToString().Trim().ToUpper();
            string strLotNo = this.dgvRMAdjustment["Lot No", 0].Value.ToString().Trim().ToUpper();
            string strBGDNo = this.dgvRMAdjustment["BGD No", 0].Value.ToString().Trim().ToUpper();                    
            string strRMEHB = this.dgvRMAdjustment["RM EHB", 0].Value.ToString().Trim().ToUpper();
            string strRMCHNName = this.dgvRMAdjustment["RM CHN Name", 0].Value.ToString().Trim().ToUpper();        
            string strShipFromCountry = this.dgvRMAdjustment["ShipFrom Country", 0].Value.ToString().Trim().ToUpper();
            string strCountryofOrigin = this.dgvRMAdjustment["Country of Origin", 0].Value.ToString().Trim().ToUpper();
            string strGoodsRcvdDate = this.dgvRMAdjustment["Goods Rcvd Date", 0].Value.ToString().Trim();
            string strCustomsRcvdDate = this.dgvRMAdjustment["Customs Rcvd Date", 0].Value.ToString().Trim();
            string strReceiptsID = this.dgvRMAdjustment["Receipts ID", 0].Value.ToString().Trim().Trim();
            string strReceiptsStatus = this.dgvRMAdjustment["Receipts Status", 0].Value.ToString().Trim().ToUpper();
            string strGateInDate = this.dgvRMAdjustment["Gate In Date", 0].Value.ToString().Trim().ToUpper();

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Edition") == 0)
            {
                string strObj = null;
                string strOldReceiptsID = dTable.Rows[0]["Receipts ID"].ToString().Trim().ToUpper();
                string strOldReceiptsStatus = dTable.Rows[0]["Receipts Status"].ToString().Trim().ToUpper();
                string strOldCutomsRcvdDate = dTable.Rows[0]["Customs Rcvd Date"].ToString().Trim();
                string strOldGateInDate = dTable.Rows[0]["Gate In Date"].ToString().Trim();             
                #region //Update C_RMReceiving table
                Comm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = strTransactionType;
                Comm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = strCustomsEntryNo;
                Comm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = strRMCHNName;
                Comm.Parameters.Add("@ShipFromCountry", SqlDbType.NVarChar).Value = strShipFromCountry;
                Comm.Parameters.Add("@CountryofOrigin", SqlDbType.NVarChar).Value = strCountryofOrigin;
                if (String.IsNullOrEmpty(strGoodsRcvdDate)) { Comm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = DBNull.Value; }
                else { Comm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGoodsRcvdDate); }
                if (!String.IsNullOrEmpty(strOldCutomsRcvdDate) && !String.IsNullOrEmpty(strCustomsRcvdDate))
                {
                    Comm.Parameters.Add("@CustomsRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCustomsRcvdDate);
                    strObj += ", [Customs Rcvd Date] = @CustomsRcvdDate";
                }
                if (!String.IsNullOrEmpty(strOldReceiptsID) && !String.IsNullOrEmpty(strReceiptsID))
                {
                    Comm.Parameters.Add("@ReceiptsID", SqlDbType.NVarChar).Value = strReceiptsID;
                    strObj += ", [Receipts ID] = @ReceiptsID";
                }
                if (!String.IsNullOrEmpty(strOldReceiptsStatus) && !String.IsNullOrEmpty(strReceiptsStatus))
                {
                    Comm.Parameters.Add("@ReceiptsStatus", SqlDbType.NVarChar).Value = strReceiptsStatus;
                    strObj += ", [Receipts Status] = @ReceiptsStatus";
                }
                if (!String.IsNullOrEmpty(strOldGateInDate) && !String.IsNullOrEmpty(strGateInDate))
                {
                    Comm.Parameters.Add("@GateInDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGateInDate);
                    strObj += ", [Gate In Date] = @GateInDate ";
                }
                Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;              
                Comm.CommandText = "UPDATE C_RMReceiving SET [Transaction Type] = @TransactionType, [Customs Entry No] = @CustomsEntryNo, [RM CHN Name] = @RMCHNName, " +
                                   "[ShipFrom Country] = @ShipFromCountry, [Country of Origin] = @CountryofOrigin, [Goods Rcvd Date] = @GoodsRcvdDate " + strObj +
                                   " WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
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
                { Comm.Parameters.Clear(); Comm.Dispose(); }
                #endregion

                this.dgvRMReceiving["Transaction Type", iRowIndex].Value = strTransactionType;
                this.dgvRMReceiving["Customs Entry No", iRowIndex].Value = strCustomsEntryNo;
                this.dgvRMReceiving["RM CHN Name", iRowIndex].Value = strRMCHNName;
                this.dgvRMReceiving["ShipFrom Country", iRowIndex].Value = strShipFromCountry;
                this.dgvRMReceiving["Country of Origin", iRowIndex].Value = strCountryofOrigin;
                if (String.IsNullOrEmpty(strGoodsRcvdDate)) { this.dgvRMReceiving["Goods Rcvd Date", iRowIndex].Value = DBNull.Value; }
                else { this.dgvRMReceiving["Goods Rcvd Date", iRowIndex].Value = Convert.ToDateTime(strGoodsRcvdDate); }
                if (!String.IsNullOrEmpty(strCustomsRcvdDate)) { this.dgvRMReceiving["Customs Rcvd Date", iRowIndex].Value = Convert.ToDateTime(strCustomsRcvdDate); }
                this.dgvRMReceiving["Receipts ID", iRowIndex].Value = strReceiptsID;
                this.dgvRMReceiving["Receipts Status", iRowIndex].Value = strReceiptsStatus;
                if (!String.IsNullOrEmpty(strGateInDate)) { this.dgvRMReceiving["Gate In Date", iRowIndex].Value = Convert.ToDateTime(strGateInDate); }

                if (MessageBox.Show("Successfully edit the relevant data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                { this.dgvRMAdjustment.DataSource = DBNull.Value; }
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            else //if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Deletion") == 0)
            {
                if (bIsBOM == false)
                {
                    #region //Delete data in C_RMReceiving table
                    Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                    Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                    Comm.CommandText = @"DELETE FROM C_RMReceiving WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                    Comm.ExecuteNonQuery();
                    Comm.Parameters.Clear();
                    #endregion

                    #region //Delete data C_RMBalance table
                    if (String.IsNullOrEmpty(strCustomsRcvdDate)) 
                    {
                        Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        Comm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                        Comm.CommandText = @"DELETE C_RMBalance WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo AND [BGD No] = @BGDNo AND [RM EHB] = @RMEHB";
                        Comm.ExecuteNonQuery();
                        Comm.Parameters.Clear();
                    }
                    #endregion
                    this.cmbAdjustment.Text = string.Empty;
                    this.dgvRMReceiving.Rows.RemoveAt(iRowIndex);
                    Comm.Dispose();
                    if (MessageBox.Show("Successfully delete the relevant data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                    { this.dgvRMAdjustment.DataSource = DBNull.Value; }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
                else
                { 
                    MessageBox.Show("Already generated BOM, deletion is terminated.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Comm.Dispose();
                    Conn.Close();
                    Conn.Dispose();
                    return;
                }
            }       
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to download all history data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            SqlConnection ConnDL = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnDL.State == ConnectionState.Closed) { ConnDL.Open(); }
            SqlDataAdapter AdapterDL = new SqlDataAdapter("SELECT * FROM C_RMReceiving", ConnDL);
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
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\RM Receiving Data " + System.DateTime.Today.ToString("yyyyMMdd") + "_" + m.ToString() + ".xls";
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
                MessageBox.Show("Completely download all RM Receving data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            finally { ConnDL.Dispose(); dtDL.Dispose(); }
        }

        private void llinkPrompt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strSelectItem = this.cmbSelectItem.Text.Trim();
            if (String.IsNullOrEmpty(strSelectItem)) {
                MessageBox.Show("Please turn on the Selection Items.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (strSelectItem.Contains("1-"))
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tTransaction Type, \n\tCustoms Entry No, \n\tInbound Delivery No, \n\tItem No, \n\tItem Description, \n\tLot No, " +
                                "\n\tBGD No, \n\tRM EHB, \n\tRM CNH Name, \n\tPO Invoice Qty, \n\tPO Invoice Amount, \n\tPO Currency, \n\tSAP PO No, \n\tRcvd SLOC, " +
                                "\n\tGoods Rcvd Date, \n\tShipFrom Country, \n\tCountry of Origin", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (strSelectItem.Contains("2-"))
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tInbound Delivery No, \n\tItem No, \n\tLot No, \n\tBGD No, \n\tRM EHB, \n\tPO Invoice Qty, \n\tCustoms Rcvd Date", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else //if(strSelectItem.Contains("3-"))
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tInbound Delivery No, \n\tItem No, \n\tLot No, \n\tReceipts ID, \n\tReceipts Status, \n\tGate In Date", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string strSelectItem = this.cmbSelectItem.Text.Trim();
            if (String.IsNullOrEmpty(strSelectItem)) {
                MessageBox.Show("Please turn on the Selection Items.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (String.IsNullOrEmpty(this.txtPath.Text.Trim())) {
                MessageBox.Show("Please find out the uploading file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge, strSelectItem);
            }
            catch (Exception)
            {
                MessageBox.Show("Upload error, please try again.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void ImportExcelData(string strFilePath, bool bJudge, string strSelectItem)
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
                myTable.Clear();
                myTable.Dispose();
                return;
            }

            SqlConnection oneConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (oneConn.State == ConnectionState.Closed) { oneConn.Open(); }
            SqlCommand oneComm = new SqlCommand();
            oneComm.Connection = oneConn;
            if (strSelectItem.Contains("1-"))
            {
                SqlDataAdapter oneAdpter = new SqlDataAdapter("SELECT [Inbound Delivery No], [Item No], [Lot No] FROM C_RMReceiving", oneConn); 
                DataTable dtRmRcvd = new DataTable();
                oneAdpter.Fill(dtRmRcvd);
                oneAdpter.Dispose();

                #region
                DataTable dtRecord = new DataTable();
                dtRecord.Columns.Add("Inbound Delivery No", typeof(string));
                dtRecord.Columns.Add("Item No", typeof(string));
                dtRecord.Columns.Add("Lot No", typeof(string));
                for (int i = 0; i < myTable.Rows.Count; i++)
                {
                    string strIDN = myTable.Rows[i]["Inbound Delivery No"].ToString().Trim().ToUpper();
                    string strIN = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                    string strLN = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                    DataRow[] drHistory = dtRmRcvd.Select("[Inbound Delivery No]='" + strIDN + "' AND [Item No]='" + strIN + "' AND [Lot No]='" + strLN + "'");
                    if (drHistory.Length == 0)
                    {
                        oneComm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Transaction Type"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Customs Entry No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@InboundDeliveryNo", SqlDbType.NVarChar).Value = strIDN;
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strIN;
                        oneComm.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item Description"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLN;
                        oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM EHB"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@RMChnName", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM CHN Name"].ToString().Trim().ToUpper();
                        string strPoInvoiceQty = myTable.Rows[i]["PO Invoice Qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strPoInvoiceQty)) { oneComm.Parameters.Add("@PoInvoiceQty", SqlDbType.Decimal).Value = 0.0M; }
                        else { oneComm.Parameters.Add("@PoInvoiceQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strPoInvoiceQty), 6); }
                        string strPoInvoiceAmt = myTable.Rows[i]["PO Invoice Amount"].ToString().Trim();
                        if (String.IsNullOrEmpty(strPoInvoiceAmt)) { oneComm.Parameters.Add("@PoInvoiceAmount", SqlDbType.Decimal).Value = 0.0M; }
                        else { oneComm.Parameters.Add("@PoInvoiceAmount", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strPoInvoiceAmt), 2); }
                        decimal dPoUnitPrice = 0.0M;
                        if (!String.IsNullOrEmpty(strPoInvoiceQty)) { dPoUnitPrice = Math.Round(Convert.ToDecimal(strPoInvoiceAmt) / Convert.ToDecimal(strPoInvoiceQty), 2); }
                        oneComm.Parameters.Add("@PoUnitPrice", SqlDbType.Decimal).Value = dPoUnitPrice;
                        oneComm.Parameters.Add("@PoCurrency", SqlDbType.NVarChar).Value = myTable.Rows[i]["PO Currency"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@SapPoNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["SAP PO No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@RcvdSloc", SqlDbType.NVarChar).Value = myTable.Rows[i]["Rcvd SLOC"].ToString().Trim().ToUpper();
                        string strGoodsRcvdDate = myTable.Rows[i]["Goods Rcvd Date"].ToString().Trim();
                        if (String.IsNullOrEmpty(strGoodsRcvdDate)) { oneComm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { oneComm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGoodsRcvdDate); }
                        oneComm.Parameters.Add("@ShipFromCountry", SqlDbType.NVarChar).Value = myTable.Rows[i]["ShipFrom Country"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@CountryofOrigin", SqlDbType.NVarChar).Value = myTable.Rows[i]["Country of Origin"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                        oneComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        oneComm.CommandText = "INSERT INTO C_RMReceiving([Transaction Type], [Customs Entry No], [Inbound Delivery No], [Item No], [Item Description], " +
                                              "[Lot No], [BGD No], [RM EHB], [RM CHN Name], [PO Invoice Qty], [PO Invoice Amount], [PO Unit Price], [PO Currency], " +
                                              "[SAP PO No], [Rcvd SLOC], [Goods Rcvd Date], [ShipFrom Country], [Country of Origin], [Creater], [Created Date]) " +
                                              "VALUES(@TransactionType, @CustomsEntryNo, @InboundDeliveryNo, @ItemNo, @ItemDescription, @LotNo, @BGDNo, @RMEHB, " +
                                              "@RMChnName, @PoInvoiceQty, @PoInvoiceAmount, @PoUnitPrice, @PoCurrency, @SapPoNo, @RcvdSloc, @GoodsRcvdDate, " +
                                              "@ShipFromCountry, @CountryofOrigin, @Creater, @CreatedDate)";
                        oneComm.ExecuteNonQuery();
                        oneComm.Parameters.Clear();
                    }
                    else
                    {
                        DataRow dr = dtRecord.NewRow();
                        dr[0] = strIDN;
                        dr[1] = strIN;
                        dr[2] = strLN;
                        dtRecord.Rows.Add(dr);
                    }
                }
                dtRmRcvd.Dispose();
                if (dtRecord.Rows.Count == 0) { MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else 
                {
                    PopUpInfoForm PopUpInfoFrm = new PopUpInfoForm();
                    PopUpInfoFrm.DataTableRecord = dtRecord.Copy();
                    PopUpInfoFrm.Show();
                }
                #endregion
            }
            else if (strSelectItem.Contains("2-"))
            {
                #region
                oneComm.CommandText = "SELECT * FROM B_MultiUser";
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
                    oneComm.CommandText = "INSERT INTO B_MultiUser([UserName]) VALUES(@UserName)";
                    oneComm.ExecuteNonQuery();
                    oneComm.Parameters.Clear();
                }

                DataRow[] drow = myTable.Select("[Customs Rcvd Date] IS NOT NULL");
                if (drow.Length > 0)
                {
                    SqlDataAdapter oneAdpter = new SqlDataAdapter("SELECT [Item No], [Lot No], [Customs Rcvd Date] FROM C_RMReceiving", oneConn);
                    DataTable dtRmRcvd = new DataTable();
                    oneAdpter.Fill(dtRmRcvd);
                    oneAdpter.Dispose();
                    foreach (DataRow dr in drow)
                    {
                        string strCustomsRcvdDate = dr["Customs Rcvd Date"].ToString().Trim();
                        string strPOInvoiceQty = dr["PO Invoice Qty"].ToString().Trim();
                        decimal dPOQuantity = 0.0M;
                        if (!String.IsNullOrEmpty(strPOInvoiceQty)) { dPOQuantity = Math.Round(Convert.ToDecimal(strPOInvoiceQty), 6); }
                        if (!String.IsNullOrEmpty(strCustomsRcvdDate))
                        {
                            string strIN = dr["Item No"].ToString().Trim().ToUpper();
                            string strLN = dr["Lot No"].ToString().Trim().ToUpper();
                            string strCusRcvdDate = null;
                            DataRow[] drHistory = dtRmRcvd.Select("[Item No]='" + strIN + "' AND [Lot No]='" + strLN + "'");
                            if (drHistory.Length > 0) { strCusRcvdDate = drHistory[0]["Customs Rcvd Date"].ToString().Trim(); }

                            oneComm.Parameters.Clear();
                            oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim().ToUpper();
                            oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim().ToUpper();
                            oneComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = dr["RM EHB"].ToString().Trim().ToUpper();
                            oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = dr["BGD No"].ToString().Trim().ToUpper();
                            if (String.IsNullOrEmpty(strCusRcvdDate))
                            {
                                oneComm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dPOQuantity;
                                oneComm.CommandText = "INSERT INTO C_RMBalance([Item No], [Lot No], [RM EHB], [BGD No], [Customs Balance], [Available RM Balance], " +
                                                      "[PO Invoice Qty]) VALUES(@ItemNo, @LotNo, @RMEHB, @BGDNo, @CustomsBalance, @CustomsBalance, @CustomsBalance)";
                                oneComm.ExecuteNonQuery();                               
                            }

                            oneComm.Parameters.Clear();
                            oneComm.Parameters.Add("@CustomsRcvdDate", SqlDbType.NVarChar).Value = strCustomsRcvdDate;
                            oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim().ToUpper();
                            oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim().ToUpper();
                            oneComm.CommandText = @"UPDATE C_RMReceiving SET [Customs Rcvd Date] = @CustomsRcvdDate WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                            oneComm.ExecuteNonQuery();
                            oneComm.Parameters.Clear();
                        }
                    }
                    dtRmRcvd.Dispose();
                }
                oneComm.Parameters.Clear();
                oneComm.CommandText = "DELETE FROM B_MultiUser";
                oneComm.ExecuteNonQuery();
                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                #endregion
            }
            else //if (strSelectItem.Contains("3-"))
            {
                #region
                DataRow[] drow = myTable.Select("[Receipts ID] IS NOT NULL AND [Receipts ID] <> ''");
                if (drow.Length > 0)
                {
                    foreach (DataRow dr in drow)
                    {
                        oneComm.Parameters.Clear();
                        oneComm.Parameters.Add("@ReceiptsID", SqlDbType.NVarChar).Value = dr["Receipts ID"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim().ToUpper();
                        oneComm.CommandText = "UPDATE C_RMReceiving SET [Receipts ID] = @ReceiptsID WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                        oneComm.ExecuteNonQuery();
                    }
                }
                drow = myTable.Select("[Receipts Status] IS NOT NULL AND [Receipts Status] <> ''");
                if (drow.Length > 0)
                {
                    foreach (DataRow dr in drow)
                    {
                        oneComm.Parameters.Clear();
                        oneComm.Parameters.Add("@ReceiptsStatus", SqlDbType.NVarChar).Value = dr["Receipts Status"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim().ToUpper();
                        oneComm.CommandText = "UPDATE C_RMReceiving SET [Receipts Status] = @ReceiptsStatus WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                        oneComm.ExecuteNonQuery();
                    }
                }
                drow = myTable.Select("[Gate In Date] IS NOT NULL");
                for (int i = 0; i < drow.Length; i++)
                {
                    string strGateInDate = drow[i]["Gate In Date"].ToString().Trim();
                    if (!String.IsNullOrEmpty(strGateInDate))
                    {
                        oneComm.Parameters.Clear();
                        oneComm.Parameters.Add("@GateInDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGateInDate);
                        oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = drow[i]["Item No"].ToString().Trim().ToUpper();
                        oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = drow[i]["Lot No"].ToString().Trim().ToUpper();
                        oneComm.CommandText = "UPDATE C_RMReceiving SET [Gate In Date] = @GateInDate WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                        oneComm.ExecuteNonQuery();                       
                    }
                }
                //for (int j = 0; j < myTable.Rows.Count; j++)
                //{
                //    oneComm.Parameters.Add("@ReceiptsID", SqlDbType.NVarChar).Value = myTable.Rows[j]["Receipts ID"].ToString().Trim().ToUpper();
                //    oneComm.Parameters.Add("@ReceiptsStatus", SqlDbType.NVarChar).Value = myTable.Rows[j]["Receipts Status"].ToString().Trim().ToUpper();
                //    string strGateInDate = myTable.Rows[j]["Gate In Date"].ToString().Trim();
                //    if (String.IsNullOrEmpty(strGateInDate)) { oneComm.Parameters.Add("@GateInDate", SqlDbType.DateTime).Value = DBNull.Value; }
                //    else { oneComm.Parameters.Add("@GateInDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGateInDate); }
                //    oneComm.Parameters.Add("@InboundDeliveryNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Inbound Delivery No"].ToString().Trim().ToUpper();
                //    oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Item No"].ToString().Trim().ToUpper();
                //    oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[j]["Lot No"].ToString().Trim().ToUpper();
                //    oneComm.CommandText = @"UPDATE C_RMReceiving SET [Receipts ID] = @ReceiptsID, [Receipts Status] = @ReceiptsStatus, [Gate In Date] = @GateInDate " +
                //                           "WHERE [Inbound Delivery No] = @InboundDeliveryNo AND [Item No] = @ItemNo AND [Lot No] = @LotNo";
                //    oneComm.ExecuteNonQuery();
                //    oneComm.Parameters.Clear();
                //}
                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                #endregion
            }
            oneComm.Dispose();
            myTable.Dispose();
            if (oneConn.State == ConnectionState.Open) { oneConn.Close(); oneConn.Dispose(); }
        }   
    }
}
