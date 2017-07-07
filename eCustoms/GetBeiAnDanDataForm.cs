using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class GetBeiAnDanDataForm : Form
    {
        DataTable dtIE = new DataTable();
        DataTable dtDailyBAD = new DataTable();
        protected DataView dvFillDGV = new DataView();
        string strFilter = null;
        bool bPending = false;
        protected PopUpFilterForm filterFrm = null;
        private DataGridView dgvDetails = new DataGridView();  

        private static GetBeiAnDanDataForm getBeiAnDanDataFrm;
        public GetBeiAnDanDataForm() { InitializeComponent(); }
        public static GetBeiAnDanDataForm CreateInstance()
        {
            if (getBeiAnDanDataFrm == null || getBeiAnDanDataFrm.IsDisposed) { getBeiAnDanDataFrm = new GetBeiAnDanDataForm(); }
            return getBeiAnDanDataFrm;
        }

        private void GetBeiAnDanDataForm_Load(object sender, EventArgs e)
        {
            SqlLib sqlLib = new SqlLib();
            dtIE = sqlLib.GetDataTable(@"SELECT [ObjectValue] AS [IE Type] FROM B_SysInfo WHERE [ObjectName] = 'IE Type' AND [ObjectValue] <> 'RM-D'").Copy();
            DataRow dr = dtIE.NewRow();
            dr["IE Type"] = String.Empty;
            dtIE.Rows.InsertAt(dr, 0);
            this.cmbIEtype.DisplayMember = this.cmbIEtype.ValueMember = "IE Type";
            this.cmbIEtype.DataSource = dtIE;
            sqlLib.Dispose();
        }

        private void GetBeiAnDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtIE.Dispose();
            dtDailyBAD.Dispose();
        }

        private void dgvBeiAnDan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++) { this.dgvBeiAnDan[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++) { this.dgvBeiAnDan[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
                    {
                        if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvBeiAnDan[0, i].Value = true; }

                        else { this.dgvBeiAnDan[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }

            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvBeiAnDan_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvBeiAnDan.RowCount == 0) { return; }
            if (this.dgvBeiAnDan.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
                {
                    if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }
                if (iCount < this.dgvBeiAnDan.RowCount && iCount > 0) { this.dgvBeiAnDan.Columns[0].HeaderText = "Reverse"; }
                else if (iCount == this.dgvBeiAnDan.RowCount) { this.dgvBeiAnDan.Columns[0].HeaderText = "Cancel"; }
                else if (iCount == 0) { this.dgvBeiAnDan.Columns[0].HeaderText = "Select"; }
            }
        }

        private void dgvBeiAnDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2) 
            {
                int iIEType = this.dgvBeiAnDan.Columns["IE Type"].Index;
                if (this.dgvBeiAnDan.CurrentCell.ColumnIndex == iIEType)
                {
                    FunctionDGV_IETYPE();
                    dgvDetails.Width = 119;
                    dgvDetails.Height = 180;

                    Rectangle rec = this.dgvBeiAnDan.GetCellDisplayRectangle(1, this.dgvBeiAnDan.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvBeiAnDan.Columns[1].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvBeiAnDan.Location.Y > this.dgvBeiAnDan.Height) { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else { dgvDetails.Top = rec.Top + this.dgvBeiAnDan.Location.Y; }
                    dgvDetails.Visible = true; 
                }
            }
            if (e.ColumnIndex != 2) { dgvDetails.Visible = false; }
        }

        private void FunctionDGV_IETYPE()
        {
            DataTable dtDataIE = dtIE.Copy();
            dtDataIE.Rows.RemoveAt(0);
            DataRow dr = dtDataIE.NewRow();
            dr[0] = "RM-D";
            dtDataIE.Rows.Add(dr);
            dgvDetails.DataSource = dtDataIE;
            this.dgvBeiAnDan.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);
        }

        private void DGV_Details_CellClick(object sender, EventArgs e)
        {
            int iIEType = this.dgvBeiAnDan.Columns["IE Type"].Index;
            if (this.dgvBeiAnDan.CurrentCell != null && this.dgvBeiAnDan.CurrentCell.ColumnIndex == iIEType)
            {
                string strIEType = dgvDetails["IE Type", dgvDetails.CurrentCell.RowIndex].Value.ToString().Trim();
                this.dgvBeiAnDan[iIEType, this.dgvBeiAnDan.CurrentCell.RowIndex].Value = strIEType;
            }
            dgvDetails.Visible = false;
        }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            string strIEtype = this.cmbIEtype.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strIEtype))
            { MessageBox.Show("Please select I/E Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            bPending = false;
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection ConnBAD = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnBAD.State == ConnectionState.Closed) { ConnBAD.Open(); }
            SqlCommand CommBAD = new SqlCommand();
            CommBAD.Connection = ConnBAD;
            CommBAD.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = strIEtype;
            CommBAD.CommandText = "SELECT [IE Type], [GongDan No], [Batch No], [FG No], CASE WHEN [BOM In Customs] = '' OR  [BOM In Customs] IS NULL THEN " +
                                  "SUBSTRING([FG Description], 0, CHARINDEX('-', [FG Description], CHARINDEX('-', [FG Description], 0) + 1)) + '/' + [Batch No] ELSE " +
                                  "[BOM In Customs] END AS [FG EHB], [CHN Name], [Order No], [GongDan Qty], [Order Price] AS [Selling Price], " +
                                  "[Order Currency] AS [Currency], [Destination], [Approval Date] AS [GongDan Approval Date] FROM C_GongDan WHERE [IE Type] = @IEtype " +
                                  "AND [BeiAnDan Used Qty] = 0";
            SqlDataAdapter AdapterBAD = new SqlDataAdapter();
            AdapterBAD.SelectCommand = CommBAD;
            dtDailyBAD.Rows.Clear();
            dtDailyBAD.Columns.Clear();
            AdapterBAD.Fill(dtDailyBAD);
            AdapterBAD.Dispose();
            if (dtDailyBAD.Rows.Count == 0)
            {
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommBAD.Dispose();
                ConnBAD.Dispose();
                return;
            }

            SqlDataAdapter AdpBAD = new SqlDataAdapter("SELECT [ObjectName] AS [Object], [ObjectValue] AS [Rate] FROM B_SysInfo WHERE [ObjectName] LIKE 'ExchangeRate%'", ConnBAD);
            DataTable dtbl = new DataTable();
            AdpBAD.Fill(dtbl);
            dtDailyBAD.Columns.Add("Group ID", typeof(string));
            dtDailyBAD.Columns["Group ID"].SetOrdinal(0);
            dtDailyBAD.Columns.Add("Selling Amount", typeof(decimal));
            dtDailyBAD.Columns["Selling Amount"].SetOrdinal(11);

            string strGroupID = this.GetGroupID(strIEtype, ConnBAD, AdpBAD);
            AdpBAD.Dispose();
            foreach (DataRow dr in dtDailyBAD.Rows)
            {
                decimal dAmount = 0.0M;
                decimal dGdQty = Convert.ToDecimal(dr["GongDan Qty"].ToString().Trim());
                if (String.Compare(dr["Currency"].ToString().Trim(), "USD") != 0)
                {
                    string strObject = "ExchangeRate:" + dr["Currency"].ToString().Trim();
                    DataRow[] drRate = dtbl.Select("[Object] = '" + strObject + "'");
                    if (drRate.Length == 0) { dAmount = 0.0M; }
                    else
                    {
                        decimal dPrice = Math.Round(Convert.ToDecimal(dr["Selling Price"].ToString()) * Convert.ToDecimal(drRate[0][1].ToString().Trim()), 4);
                        dAmount = Math.Round(dPrice * dGdQty, 2);
                    }
                }
                else { dAmount = Math.Round(Convert.ToDecimal(dr["Selling Price"].ToString().Trim()) * dGdQty, 2); }
                dr["Selling Amount"] = dAmount;

                //if there is RMB, need to consider the group ID based on previous logics or reality business process
                if (String.Compare(strIEtype, "BLP") != 0 && String.Compare(strIEtype, "EXPORT") != 0) { dr["Group ID"] = strGroupID; }
            }
            dtbl.Dispose();
            if (String.Compare(strIEtype, "BLP") == 0 || String.Compare(strIEtype, "EXPORT") == 0)
            {
                for (int i = 0; i < dtDailyBAD.Rows.Count; i++)
                { dtDailyBAD.Rows[i]["Group ID"] = this.GetASCGroupID(strGroupID, i); }
            }

            CommBAD.Parameters.Clear();
            CommBAD.CommandType = CommandType.StoredProcedure;
            CommBAD.CommandText = @"usp_InsertDailyBeiAnDan";
            CommBAD.Parameters.AddWithValue("@tvp_InsertDailyBeiAnDan", dtDailyBAD);
            CommBAD.ExecuteNonQuery();
            CommBAD.Parameters.Clear();

            CommBAD.CommandType = CommandType.Text;
            CommBAD.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = strIEtype;
            CommBAD.CommandText = "SELECT [Group ID], [IE Type], [GongDan No], [Batch No], [FG No], [FG EHB], [CHN Name], [Order No], [GongDan Qty], [Selling Price], " +
                                  "[Currency], [Selling Amount] AS [Selling Amount(USD)], [Destination], [GongDan Approval Date] FROM M_DailyBeiAnDan WHERE " +
                                  "[IE Type] = @IEtype AND [Is Generated Doc] = 'False' ORDER BY [Group ID], [GongDan No]";
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();
            SqlAdapter.SelectCommand = CommBAD;
            dtDailyBAD.Rows.Clear();
            dtDailyBAD.Columns.Clear();
            SqlAdapter.Fill(dtDailyBAD);
            SqlAdapter.Dispose();
            CommBAD.Parameters.Clear();
            CommBAD.Dispose();
            if (ConnBAD.State == ConnectionState.Open) { ConnBAD.Close(); ConnBAD.Dispose(); }

            dvFillDGV = dtDailyBAD.DefaultView;
            this.dgvBeiAnDan.DataSource = dvFillDGV;
            this.dgvBeiAnDan.Columns[0].HeaderText = "Select";
            for (int j = 3; j < this.dgvBeiAnDan.ColumnCount; j++) { this.dgvBeiAnDan.Columns[j].ReadOnly = true; }
            this.dgvBeiAnDan.Columns["Batch No"].Visible = false;
            this.dgvBeiAnDan.Columns["GongDan No"].Frozen = true;
        }

        private string GetGroupID(string strIEtype, SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            string strHead = null;
            switch (strIEtype)
            {
                case "1418": strHead = "A"; break;
                case "BLP": strHead = "G"; break;
                case "EXPORT": strHead = "B"; break;
                case "RMB": strHead = "C"; break;
                case "RMB-1418": strHead = "D"; break;
                case "RMB-D": strHead = "E"; break;
                default: strHead = "F"; break;
            }

            string strDate = System.DateTime.Now.ToString("yyyyMMdd").Trim();
            string strSQL1 = "SELECT SUBSTRING([Group ID], 3, 8) AS [GroupID], MAX(CAST(SUBSTRING([Group ID], 12, LEN([Group ID])) AS Int)) AS [MaxID], [IE Type] FROM " +
                             "C_BeiAnDan WHERE [IE Type] = '" + strIEtype + "' AND [Group ID] LIKE '%" + strDate + "%' GROUP BY SUBSTRING([Group ID], 3, 8), [IE Type]";
            sqlAdp = new SqlDataAdapter(strSQL1, sqlConn);
            DataTable dtGroupID1 = new DataTable();
            sqlAdp.Fill(dtGroupID1);

            string strSQL2 = "SELECT SUBSTRING([Group ID], 3, 8) AS [GroupID], MAX(CAST(SUBSTRING([Group ID], 12, LEN([Group ID])) AS Int)) AS [MaxID], [IE Type] FROM " +
                             "M_DailyBeiAnDan WHERE [IE Type] = '" + strIEtype + "' AND [Group ID] LIKE '%" + strDate + "%' GROUP BY SUBSTRING([Group ID], 3, 8), [IE Type]";
            sqlAdp = new SqlDataAdapter(strSQL2, sqlConn);
            DataTable dtGroupID2 = new DataTable();
            sqlAdp.Fill(dtGroupID2);

            string strSuffix1 = null;
            if (dtGroupID1.Rows.Count == 0) { strSuffix1 = "01"; }
            else
            {
                strSuffix1 = dtGroupID1.Rows[0]["MaxID"].ToString().Trim();
                int iNumber1 = Convert.ToInt32(strSuffix1) + 1;
                if (iNumber1.ToString().Trim().Length == 1) { strSuffix1 = "0" + iNumber1.ToString().Trim(); }
                else { strSuffix1 = iNumber1.ToString().Trim(); }
            }
            dtGroupID1.Dispose();

            string strSuffix2 = null;
            if (dtGroupID2.Rows.Count == 0) { strSuffix2 = "01"; }
            else
            {
                strSuffix2 = dtGroupID2.Rows[0]["MaxID"].ToString().Trim();
                int iNumber2 = Convert.ToInt32(strSuffix2) + 1;
                if (iNumber2.ToString().Trim().Length == 1) { strSuffix2 = "0" + iNumber2.ToString().Trim(); }
                else { strSuffix2 = iNumber2.ToString().Trim(); }
            }
            dtGroupID2.Dispose();

            if (String.Compare(strIEtype, "EXPORT") == 0)
            {
                string strSQL3 = "SELECT SUBSTRING([Group ID], 3, 8) AS [GroupID], MAX(CAST(SUBSTRING([Group ID], 12, LEN([Group ID])) AS Int)) AS [MaxID], " +
                                 "[IE Type] FROM M_PendingBeiAnDan_Export WHERE [Group ID] LIKE '%" + strDate + "%' GROUP BY SUBSTRING([Group ID], 3, 8), [IE Type]";
                sqlAdp = new SqlDataAdapter(strSQL3, sqlConn);
                DataTable dtGroupID3 = new DataTable();
                dtGroupID3.Clear();
                sqlAdp.Fill(dtGroupID3);

                string strSuffix3 = null;
                if (dtGroupID3.Rows.Count == 0) { strSuffix3 = "01"; }
                else
                {
                    strSuffix3 = dtGroupID3.Rows[0]["MaxID"].ToString().Trim();
                    int iNumber3 = Convert.ToInt32(strSuffix3) + 1;
                    if (iNumber3.ToString().Trim().Length == 1) { strSuffix3 = "0" + iNumber3.ToString().Trim(); }
                    else { strSuffix3 = iNumber3.ToString().Trim(); }
                }
                if (String.Compare(strSuffix3, strSuffix2) > 0) { strSuffix2 = strSuffix3; }
                dtGroupID3.Dispose();
            }

            string strGroupID = null;
            if (String.Compare(strSuffix1, strSuffix2) >= 0) { strGroupID = strHead + "-" + strDate + "-" + strSuffix1; }
            else { strGroupID = strHead + "-" + strDate + "-" + strSuffix2; }
            return strGroupID;
        }

        private string GetASCGroupID(string strGroupID, int iCount)
        {
            string[] strArray = strGroupID.Split('-');
            string strPrefix = strArray[0].Trim() + "-" + strArray[1].Trim() + "-";
            string strSuffix = strArray[2].Trim();
            int iNumber = Convert.ToInt32(strSuffix) + iCount;
            if (iNumber > 9) { strSuffix = iNumber.ToString().Trim(); }
            else { strSuffix = "0" + iNumber.ToString().Trim(); }
            return strPrefix + strSuffix;
        }

        private void cmbIEtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Compare(this.cmbIEtype.Text.Trim(), "EXPORT") == 0)
            {
                this.btnDownload.Enabled = true;
                this.btnTransfer.Enabled = true;
                this.btnUpload.Enabled = true;
            }
            else
            {
                this.btnDownload.Enabled = false;
                this.btnTransfer.Enabled = false;
                this.btnUpload.Enabled = false;
            }
        }

        private void btnRemoveData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to remove the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (this.dgvBeiAnDan.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection DelConnBAD = new SqlConnection(SqlLib.StrSqlConnection);
            if (DelConnBAD.State == ConnectionState.Closed) { DelConnBAD.Open(); }
            SqlCommand DelCommBAD = new SqlCommand();
            DelCommBAD.Connection = DelConnBAD;
            DelCommBAD.CommandType = CommandType.StoredProcedure;
            DelCommBAD.CommandText = @"usp_EditDailyBeiAnDan";
            DelCommBAD.Parameters.AddWithValue("@Pending", bPending.ToString().Trim().ToUpper());
            DelCommBAD.Parameters.AddWithValue("@Action", "DELETE");
            DelCommBAD.Parameters.AddWithValue("@IEType", string.Empty);

            int iRowCount = this.dgvBeiAnDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strGongDanNo = this.dgvBeiAnDan["GongDan No", i].Value.ToString().Trim().ToUpper();                                      
                    DelCommBAD.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    DelCommBAD.ExecuteNonQuery();
                    DelCommBAD.Parameters.RemoveAt("@GongDanNo");                 

                    this.dgvBeiAnDan.Rows.RemoveAt(i);
                    iRowCount--;
                    i--;                 
                }
            }
            dtDailyBAD.AcceptChanges();
            DelCommBAD.Dispose();
            if (DelConnBAD.State == ConnectionState.Open) { DelConnBAD.Close(); DelConnBAD.Dispose(); }
            if (dtDailyBAD.Rows.Count == 0) 
            { 
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                this.dgvBeiAnDan.Columns[0].HeaderText = String.Empty;
                this.cmbIEtype.Text = String.Empty;
            }        
            MessageBox.Show("Successfully remove the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAdjustIEtype_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to adjust IE type?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (this.dgvBeiAnDan.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection AdjConnBAD = new SqlConnection(SqlLib.StrSqlConnection);
            if (AdjConnBAD.State == ConnectionState.Closed) { AdjConnBAD.Open(); }
            SqlCommand AdjCommBAD = new SqlCommand();
            AdjCommBAD.Connection = AdjConnBAD;
            AdjCommBAD.CommandType = CommandType.StoredProcedure;
            AdjCommBAD.CommandText = @"usp_EditDailyBeiAnDan";
            AdjCommBAD.Parameters.AddWithValue("@Pending", bPending.ToString().Trim().ToUpper());
            AdjCommBAD.Parameters.AddWithValue("@Action", "ADJUST");

            string strIE = this.cmbIEtype.Text.Trim().ToUpper();
            int iRowCount = this.dgvBeiAnDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strIEtype = this.dgvBeiAnDan["IE Type", i].Value.ToString().Trim().ToUpper();
                    if (String.Compare(strIE, strIEtype) != 0)
                    {
                        string strGongDanNo = this.dgvBeiAnDan["GongDan No", i].Value.ToString().Trim().ToUpper();
                        AdjCommBAD.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                        AdjCommBAD.Parameters.AddWithValue("@IEType", strIEtype);
                        AdjCommBAD.ExecuteNonQuery();
                        AdjCommBAD.Parameters.RemoveAt("@GongDanNo");
                        AdjCommBAD.Parameters.RemoveAt("@IEType");

                        this.dgvBeiAnDan.Rows.RemoveAt(i);
                        iRowCount--;
                        i--;
                    }
                }
            }
            dtDailyBAD.AcceptChanges();
            AdjCommBAD.Dispose();
            if (AdjConnBAD.State == ConnectionState.Open) { AdjConnBAD.Close(); AdjConnBAD.Dispose(); }
            if (dtDailyBAD.Rows.Count == 0) 
            { 
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                this.dgvBeiAnDan.Columns[0].HeaderText = String.Empty;
                this.cmbIEtype.Text = String.Empty;
            }            
            MessageBox.Show("Successfully adjust the I/E type.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strIEType = this.cmbIEtype.Text.Trim();
            if (String.IsNullOrEmpty(strIEType)) { MessageBox.Show("Please select I/E Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            bPending = false;
            if (String.Compare(strIEType.ToUpper(), "EXPORT") == 0)
            {
                DialogResult dlgR = MessageBox.Show("Please choose the condition to preview:\n[Yes] : Browse the data generated by gongdan;\n[No] : Browse the data from export pending data;\n[Cancel] : Reject to browse.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dlgR == DialogResult.Yes) { bPending = false; }
                else { bPending = true; this.btnTransfer.Enabled = false; }
            }
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            if (bPending == true)
            {
                sqlComm.CommandText = "SELECT [Group ID], [IE Type], [GongDan No], [Batch No], [FG No], [FG EHB], [CHN Name], [Order No], [GongDan Qty], " +
                                      "[Selling Price], [Currency], [Selling Amount] AS [Selling Amount(USD)], [Destination], [GongDan Approval Date], [Pending] " +
                                      "FROM M_PendingBeiAnDan_Export ORDER BY [Group ID], [GongDan No]";
            }
            else
            {
                sqlComm.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = strIEType.ToUpper();
                sqlComm.CommandText = "SELECT [Group ID], [IE Type], [GongDan No], [Batch No], [FG No], [FG EHB], [CHN Name], [Order No], [GongDan Qty], " +
                                      "[Selling Price], [Currency], [Selling Amount] AS [Selling Amount(USD)], [Destination], [GongDan Approval Date] FROM " +
                                      "M_DailyBeiAnDan WHERE [IE Type] = @IEtype ORDER BY [Group ID], [GongDan No]";
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            dtDailyBAD.Rows.Clear();
            dtDailyBAD.Columns.Clear();
            sqlAdapter.Fill(dtDailyBAD);
            sqlAdapter.Dispose();
            sqlComm.Parameters.Clear();
            if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); sqlConn.Dispose(); }
            if (dtDailyBAD.Rows.Count == 0)
            {
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dvFillDGV = dtDailyBAD.DefaultView;
            this.dgvBeiAnDan.DataSource = dvFillDGV;
            this.dgvBeiAnDan.Columns[0].HeaderText = "Select";
            for (int j = 3; j < this.dgvBeiAnDan.ColumnCount; j++) { this.dgvBeiAnDan.Columns[j].ReadOnly = true; }
            this.dgvBeiAnDan.Columns["Batch No"].Visible = false;
            this.dgvBeiAnDan.Columns["GongDan No"].Frozen = true;
            if (bPending == true) { this.dgvBeiAnDan.Columns["Pending"].ReadOnly = false; }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.Rows.Count == 0)
            { MessageBox.Show("There is no data in data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection DlConnBAD = new SqlConnection(SqlLib.StrSqlConnection);
            if (DlConnBAD.State == ConnectionState.Closed) { DlConnBAD.Open(); }
            SqlCommand DlCommBAD = new SqlCommand(@"SELECT [ObjectValue] FROM B_SysInfo WHERE [ObjectName] = 'WeightRatio'", DlConnBAD);
            decimal dRatio = Convert.ToDecimal(DlCommBAD.ExecuteScalar());
            DlCommBAD.Dispose();
            if (DlConnBAD.State == ConnectionState.Open) { DlConnBAD.Close(); DlConnBAD.Dispose(); }
            //int iTotalGD = Convert.ToInt32(dtDailyBAD.Compute("SUM([GongDan Qty])", "").ToString());

            Microsoft.Office.Interop.Excel.Application excel_Doc = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_Doc = excel_Doc.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_Doc = workbooks_Doc.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets[1];
            int iRow1 = 2;
            for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    //worksheet_Doc.get_Range(worksheet_Doc.Cells[iRow1, 1], worksheet_Doc.Cells[iRow1, this.dgvBeiAnDan.ColumnCount - 1]).NumberFormatLocal = "@";
                    for (int j = 1; j < this.dgvBeiAnDan.ColumnCount; j++)
                    { worksheet_Doc.Cells[iRow1, j] = this.dgvBeiAnDan[j, i].Value.ToString().Trim(); }
                    iRow1++;
                }
            }
            if (iRow1 > 2)
            {
                //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).NumberFormatLocal = "@";
                for (int k = 1; k < this.dgvBeiAnDan.ColumnCount; k++)
                { worksheet_Doc.Cells[1, k] = this.dgvBeiAnDan.Columns[k].HeaderText.ToString(); }

                //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Bold = true;
                //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow1 - 1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Name = "Verdana";
                //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow1 - 1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Size = 9;
                //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow1 - 1, this.dgvBeiAnDan.ColumnCount - 1]).Borders.LineStyle = 1;
                worksheet_Doc.Cells.EntireColumn.AutoFit();
                excel_Doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            }
            object missing = System.Reflection.Missing.Value;
            worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);

            //worksheet_Doc.get_Range(excel_Doc.Cells[1, 1], excel_Doc.Cells[1, 15]).NumberFormatLocal = "@";
            worksheet_Doc.Cells[1, 1] = "Group ID";
            worksheet_Doc.Cells[1, 2] = "备案单号";
            worksheet_Doc.Cells[1, 3] = "单证类型";
            worksheet_Doc.Cells[1, 4] = "总毛重";
            worksheet_Doc.Cells[1, 5] = "贸易方式";
            worksheet_Doc.Cells[1, 6] = "结转企业代码";
            worksheet_Doc.Cells[1, 7] = "项号";
            worksheet_Doc.Cells[1, 8] = "备件号";
            worksheet_Doc.Cells[1, 9] = "数量";
            worksheet_Doc.Cells[1, 10] = "净重";
            worksheet_Doc.Cells[1, 11] = "毛重";
            worksheet_Doc.Cells[1, 12] = "金额";
            worksheet_Doc.Cells[1, 13] = "币制";
            worksheet_Doc.Cells[1, 14] = "原产地/目的地";
            worksheet_Doc.Cells[1, 15] = "工单/批次号";

            int iRow2 = 2;
            for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strGongDanQty = this.dgvBeiAnDan["GongDan Qty", i].Value.ToString().Trim();
                    //worksheet_Doc.get_Range(excel_Doc.Cells[iRow2, 1], excel_Doc.Cells[iRow2, 15]).NumberFormatLocal = "@";
                    worksheet_Doc.Cells[iRow2, 1] = this.dgvBeiAnDan["Group ID", i].Value.ToString().Trim();
                    worksheet_Doc.Cells[iRow2, 2] = string.Empty;
                    worksheet_Doc.Cells[iRow2, 3] = "成品报关出区";
                    worksheet_Doc.Cells[iRow2, 4] = Convert.ToInt32(strGongDanQty) * dRatio;
                    worksheet_Doc.Cells[iRow2, 5] = "进料对口";
                    worksheet_Doc.Cells[iRow2, 6] = string.Empty;
                    worksheet_Doc.Cells[iRow2, 7] = iRow2 - 1;
                    worksheet_Doc.Cells[iRow2, 8] = this.dgvBeiAnDan["FG EHB", i].Value.ToString().Trim();
                    worksheet_Doc.Cells[iRow2, 9] = strGongDanQty;
                    worksheet_Doc.Cells[iRow2, 10] = strGongDanQty;
                    worksheet_Doc.Cells[iRow2, 11] = Convert.ToInt32(strGongDanQty) * dRatio;
                    worksheet_Doc.Cells[iRow2, 12] = this.dgvBeiAnDan["Selling Amount(USD)", i].Value.ToString().Trim();
                    worksheet_Doc.Cells[iRow2, 13] = "美元";
                    worksheet_Doc.Cells[iRow2, 14] = this.dgvBeiAnDan["Destination", i].Value.ToString().Trim();
                    worksheet_Doc.Cells[iRow2, 15] = this.dgvBeiAnDan["GongDan No", i].Value.ToString().Trim(); 
                    iRow2++;
                }
            }
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 15]).Font.Bold = true;
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 15]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow2 - 1, 15]).Font.Name = "Verdana";
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow2 - 1, 15]).Font.Size = 9;
            //worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow2 - 1, 15]).Borders.LineStyle = 1;
            worksheet_Doc.Cells.EntireColumn.AutoFit();
            excel_Doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            excel_Doc.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Doc);
            excel_Doc = null;
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to transfer the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            if (this.dgvBeiAnDan.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            DataTable dtTransfer = dtDailyBAD.Clone();
            int iRowCount = this.dgvBeiAnDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    dtTransfer.ImportRow(dvFillDGV.ToTable().Rows[i]);
                    this.dgvBeiAnDan.Rows.RemoveAt(i);
                    i--;
                    iRowCount--;
                }
            }
            dtDailyBAD.AcceptChanges();

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_InsertDailyBeiAnDan_EXPORT";
            sqlComm.Parameters.AddWithValue("@tvp_InsertDailyBAD_EXPORT", dtTransfer);
            sqlComm.ExecuteNonQuery();
            sqlComm.Parameters.Clear();
            sqlComm.Dispose();
            dtTransfer.Dispose();
            if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); sqlConn.Dispose(); }
            if (dtDailyBAD.Rows.Count == 0)
            {
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                this.dgvBeiAnDan.Columns[0].HeaderText = String.Empty;
                this.cmbIEtype.Text = String.Empty;
            }
            else { this.dgvBeiAnDan.Columns[0].HeaderText = "Select"; }
            MessageBox.Show("Successfully transfer the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);  
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
            { MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                bool bJudge = this.txtPath.Text.ToLower().Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge);
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
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT [GongDan No], [Pending], [Group ID] FROM [Sheet1$] WHERE [IE Type] = 'EXPORT'", myConn);
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
            myTable.Columns.Add("Line No", typeof(Int32));
            myTable.Columns["Line No"].SetOrdinal(0);
            int iLineNo = 1;
            foreach (DataRow dr in myTable.Rows) { dr["Line No"] = iLineNo++; }
            myTable.AcceptChanges();

            SqlConnection uploadConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (uploadConn.State == ConnectionState.Closed) { uploadConn.Open(); }
            SqlCommand uploadComm = new SqlCommand();
            uploadComm.Connection = uploadConn;
            uploadComm.CommandType = CommandType.StoredProcedure;
            uploadComm.CommandText = @"usp_UpdateBeiAnDan_EXPORT";
            uploadComm.Parameters.AddWithValue("@tvp_BeiAnDan_Export", myTable);
            uploadComm.ExecuteNonQuery();
            uploadComm.Parameters.Clear();
            myTable.Dispose();
            uploadComm.Dispose();
            if (uploadConn.State == ConnectionState.Open) { uploadConn.Close(); uploadConn.Dispose(); }
            MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            this.dgvBeiAnDan.Columns[0].HeaderText = "Select";
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxGID.Visible == false) { this.gBoxGID.Visible = true; }
            else { this.gBoxGID.Visible = false; this.txtGongDanNo.Text = string.Empty; this.txtGroupID.Text = string.Empty; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to update the related Group ID?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            string strGongDanNo = this.txtGongDanNo.Text.ToUpper().Trim();
            string strGroupID = this.txtGroupID.Text.ToUpper().Trim();
            if (String.IsNullOrEmpty(strGongDanNo)) { MessageBox.Show("Please input the GongDan No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (String.IsNullOrEmpty(strGroupID)) { MessageBox.Show("Please input the Group ID.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            SqlConnection ConnGID = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnGID.State == ConnectionState.Closed) { ConnGID.Open(); }
            SqlCommand CommGID = new SqlCommand();
            CommGID.Connection = ConnGID;
            CommGID.CommandText = "Update M_DailyBeiAnDan SET [Group ID] = '" + strGroupID + "' WHERE [GongDan No] = '" + strGongDanNo + "'";
            CommGID.ExecuteNonQuery();
            CommGID.Dispose();
            if (ConnGID.State == ConnectionState.Open)
            {
                ConnGID.Close();
                ConnGID.Dispose();
            }
            MessageBox.Show("Update Group ID successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
