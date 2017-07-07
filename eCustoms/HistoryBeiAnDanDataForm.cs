using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class HistoryBeiAnDanDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        DataTable dtIE = new DataTable();       
        DataTable dtMasterBAD = new DataTable();
        DataTable dtDetailBAD = new DataTable();
        private static HistoryBeiAnDanDataForm HistoryBeiAnDanDataFrm;
        public HistoryBeiAnDanDataForm() { InitializeComponent(); }
        public static HistoryBeiAnDanDataForm CreateInstance()
        {
            if (HistoryBeiAnDanDataFrm == null || HistoryBeiAnDanDataFrm.IsDisposed) { HistoryBeiAnDanDataFrm = new HistoryBeiAnDanDataForm(); }
            return HistoryBeiAnDanDataFrm;
        }

        private void HistoryBeiAnDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";
            this.dtpTaxPaidDate.CustomFormat = " ";
            this.dtpCustomsReleaseDate.CustomFormat = " ";
            this.btnUpdate.Enabled = false;
            this.btnDelete.Enabled = false;

            SqlLib sqlLib = new SqlLib();
            dtIE.Rows.Clear();
            dtIE.Columns.Clear();
            dtIE = sqlLib.GetDataTable(@"SELECT [ObjectValue] AS [IE Type] FROM B_SysInfo WHERE [ObjectName] = 'IE Type' AND [ObjectValue] <> 'RM-D'").Copy();
            DataRow dr = dtIE.NewRow();
            dr["IE Type"] = String.Empty;
            dtIE.Rows.InsertAt(dr, 0);
            this.cmbIEtype.DisplayMember = this.cmbIEtype.ValueMember = "IE Type";
            this.cmbIEtype.DataSource = dtIE;
            sqlLib.Dispose();
        }

        private void HistoryBeiAnDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtIE.Dispose();
            dtMasterBAD.Dispose();
            dtDetailBAD.Dispose();
        }

        private void cmbIEtype_SelectedIndexChanged(object sender, EventArgs e) { this.cmbBeiAnDanID.Text = String.Empty; }

        private void cbBeiAnDanNo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbBeiAnDanNo.Checked == true)
            {
                this.cbTaxPaidDate.Checked = false;
                this.cbCustomsReleaseDate.Checked = false;
                this.cbTaxPaidDate.Enabled = false;
                this.cbCustomsReleaseDate.Enabled = false;
            }
            else
            {
                this.cbTaxPaidDate.Checked = false;
                this.cbCustomsReleaseDate.Checked = false;
                this.cbTaxPaidDate.Enabled = true;
                this.cbCustomsReleaseDate.Enabled = true;
            }
        }

        private void cbTaxPaidDate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbTaxPaidDate.Checked == true)
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbCustomsReleaseDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = false;
                this.cbCustomsReleaseDate.Enabled = false;
            }
            else
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbCustomsReleaseDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = true;
                this.cbCustomsReleaseDate.Enabled = true;
            }
        }

        private void cbCustomsReleaseDate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCustomsReleaseDate.Checked == true)
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbTaxPaidDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = false;
                this.cbTaxPaidDate.Enabled = false;
            }
            else
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbTaxPaidDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = true;
                this.cbTaxPaidDate.Enabled = true;
            }
        }      

        private void cmbBeiAnDanID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
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

        private void cmbBeiAnDanID_Enter(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEtype.Text.Trim()))
            { MessageBox.Show("Please select IE type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            string strJudge = " [IE Type] = '" + this.cmbIEtype.Text.Trim().ToUpper() + "'";
            if (this.cbBeiAnDanNo.Checked == true) { strJudge += " AND ([BeiAnDan No] = '' OR [BeiAnDan No] IS NULL)"; }
            if (this.cbTaxPaidDate.Checked == true) { strJudge += " AND ([Tax & Duty Paid Date] IS NULL OR [Tax & Duty Paid Date] = '')"; }
            if (this.cbCustomsReleaseDate.Checked == true) { strJudge += " AND ([Customs Release Date] IS NULL OR [Customs Release Date] = '')"; }
            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1)
                    { MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    else { strJudge += " AND [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else { strJudge += " AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strJudge += " AND [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            sqlConn.Open();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT DISTINCT [BeiAnDan ID] FROM C_BeiAnDan WHERE " + strJudge, sqlConn);
            DataTable dtName = new DataTable();
            sqlAdapter.Fill(dtName);
            sqlAdapter.Dispose();
            DataRow dr = dtName.NewRow();
            dr["BeiAnDan ID"] = String.Empty;
            dtName.Rows.InsertAt(dr, 0);

            this.cmbBeiAnDanID.DisplayMember = this.cmbBeiAnDanID.ValueMember = "BeiAnDan ID";
            this.cmbBeiAnDanID.DataSource = dtName;
            sqlConn.Close();
            sqlConn.Dispose();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e) { this.dtpFrom.CustomFormat = null; }
        private void dtpFrom_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; } }
        private void dtpTo_ValueChanged(object sender, EventArgs e) { this.dtpTo.CustomFormat = null; }
        private void dtpTo_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; } }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEtype.Text.Trim()))
            { MessageBox.Show("Please select I/E type before preview.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            string strBrowse = " [IE Type] = '" + this.cmbIEtype.Text.Trim().ToUpper() + "'";
            if (this.cbBeiAnDanNo.Checked == true) { strBrowse += " AND ([BeiAnDan No] = '' OR [BeiAnDan No] IS NULL)"; }
            if (this.cbTaxPaidDate.Checked == true) { strBrowse += " AND ([Tax & Duty Paid Date] IS NULL OR [Tax & Duty Paid Date] = '')"; }
            if (this.cbCustomsReleaseDate.Checked == true) { strBrowse += " AND ([Customs Release Date] IS NULL OR [Customs Release Date] = '')"; }
            if (!String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim())) { strBrowse += " AND [BeiAnDan ID] = '" + this.cmbBeiAnDanID.Text.Trim().ToUpper() + "'"; }
            if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim()))
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                {
                    if (DateTime.Compare(Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpTo.Value.ToString("M/d/yyyy"))) == 1)
                    { MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    else { strBrowse += " AND [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else { strBrowse += " AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strBrowse += " AND [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            SqlConnection BrowseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BrowseConn.State == ConnectionState.Closed) { BrowseConn.Open(); }
            SqlCommand BrowseComm = new SqlCommand();
            BrowseComm.Connection = BrowseConn;
            BrowseComm.CommandType = CommandType.StoredProcedure;
            BrowseComm.CommandText = @"usp_BeiAnDan_BrowseHistoryData";
            BrowseComm.Parameters.AddWithValue("@Browse", strBrowse);
            BrowseComm.Parameters.AddWithValue("@Judge", "MASTER");
            SqlDataAdapter BrowseAdapter = new SqlDataAdapter();
            BrowseAdapter.SelectCommand = BrowseComm;
            dtMasterBAD.Rows.Clear();
            dtMasterBAD.Columns.Clear();
            BrowseAdapter.Fill(dtMasterBAD);
            BrowseComm.Parameters.Clear();

            if (dtMasterBAD.Rows.Count == 0)
            {
                BrowseAdapter.Dispose();
                BrowseComm.Dispose();
                this.dgvBeiAnDanM.DataSource = DBNull.Value;
                this.dgvBeiAnDanD.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);               
            }
            else 
            {
                BrowseComm.CommandText = @"usp_BeiAnDan_BrowseHistoryData";
                BrowseComm.Parameters.AddWithValue("@Browse", strBrowse);
                BrowseComm.Parameters.AddWithValue("@Judge", "DETAIL");
                BrowseAdapter = new SqlDataAdapter();
                BrowseAdapter.SelectCommand = BrowseComm;
                dtDetailBAD.Rows.Clear();
                dtDetailBAD.Columns.Clear();
                BrowseAdapter.Fill(dtDetailBAD);
                BrowseAdapter.Dispose();
                BrowseComm.Parameters.Clear();

                this.dgvBeiAnDanM.DataSource = dtMasterBAD;              
                this.dgvBeiAnDanM.Columns["IE Type"].Visible = false;
                this.dgvBeiAnDanM.Columns["Group ID"].Frozen = true;
                for (int i = 1; i < this.dgvBeiAnDanM.Columns.Count - 1; i++)
                { 
                    this.dgvBeiAnDanM.Columns[i].ReadOnly = true;
                    this.dgvBeiAnDanM.Columns["BeiAnDan ID"].ReadOnly = false;
                }

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvBeiAnDanM.EnableHeadersVisualStyles = false;
                this.dgvBeiAnDanM.Columns["BeiAnDAn ID"].HeaderCell.Style = cellStyle;

                this.dgvBeiAnDanD.DataSource = dtDetailBAD;
                this.dgvBeiAnDanD.Columns["Group ID"].Frozen = true;
            }
            BrowseComm.Dispose();
            if (BrowseConn.State == ConnectionState.Open) { BrowseConn.Close(); BrowseConn.Dispose(); }
        }

        private void dtpTaxPaidDate_ValueChanged(object sender, EventArgs e) { this.dtpTaxPaidDate.CustomFormat = "M/dd/yyyy HH:mm"; }
        private void dtpTaxPaidDate_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpTaxPaidDate.CustomFormat = " "; } }
        private void dtpCustomsReleaseDate_ValueChanged(object sender, EventArgs e) { this.dtpCustomsReleaseDate.CustomFormat = "M/dd/yyyy HH:mm"; }
        private void dtpCustomsReleaseDate_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpCustomsReleaseDate.CustomFormat = " "; } }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Only BeiAnDan of IE Type "RMB-1418" and "RMB" will have "Tax & Duty Paid Date"
            //For "RMB-1418", "Tax & Duty" are Paid After "2nd Release", i.e. "Tax & Duty Paid Date" can be blank if not paid yet
            //For "RMB",  "Tax & Duty" are Paid Before "2nd Release", i.e. if "2nd Release Date" is available, "Tax & Duty Paid Date" must be provided

            if (this.dgvBeiAnDanM.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
            { MessageBox.Show("Please select BeiAnDan ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to update the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            string strBeiAnDanID = this.cmbBeiAnDanID.Text.Trim().ToUpper();
            DataRow[] drow = dtMasterBAD.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'");
            string strCustomsReleaseDate = dtMasterBAD.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0]["Customs Release Date"].ToString().Trim();
            bool bExist = false;
            if (!String.IsNullOrEmpty(strCustomsReleaseDate)) { bExist = true; }

            int iCount = 0;
            string strBeiAnDanNo = null, strTax = null, strRelease = null;
            if (!String.IsNullOrEmpty(this.txtBeiAnDanNo.Text.Trim()))
            {
                strBeiAnDanNo = this.txtBeiAnDanNo.Text.Trim().ToUpper();
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 1;                  
                    foreach (DataRow dr in drow) { dr["BeiAnDan No"] = strBeiAnDanNo; }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 2;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["BeiAnDan No"] = strBeiAnDanNo;
                        dr["Tax & Duty Paid Date"] = strTax;
                    }
                }
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim())) 
                { 
                    iCount = 3;
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["BeiAnDan No"] = strBeiAnDanNo;
                        dr["Customs Release Date"] = strRelease;
                    }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 4;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["BeiAnDan No"] = strBeiAnDanNo;
                        dr["Tax & Duty Paid Date"] = strTax;
                        dr["Customs Release Date"] = strRelease;
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                { MessageBox.Show("Please input BeiAnDan No and/or date time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 5;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Tax & Duty Paid Date"] = strTax; }
                }
                if (String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 6;
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Customs Release Date"] = strRelease; }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxPaidDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpCustomsReleaseDate.Text.Trim()))
                {
                    iCount = 7;
                    strTax = this.dtpTaxPaidDate.Text.Trim();
                    strRelease = this.dtpCustomsReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["Tax & Duty Paid Date"] = strTax;
                        dr["Customs Release Date"] = strRelease;
                    }
                }
            }
            dtMasterBAD.AcceptChanges();

            SqlConnection updateConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (updateConn.State == ConnectionState.Closed) { updateConn.Open(); }
            SqlCommand updateComm = new SqlCommand();
            updateComm.Connection = updateConn;
           
            /*------ Monitor And Control Multiple Users ------*/
            updateComm.CommandText = "SELECT [UserName] FROM B_MultiUser";
            string strUserName = Convert.ToString(updateComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait for him/her to finish the process.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    updateConn.Dispose();
                    updateComm.Dispose();
                    this.btnUpdate.Focus();
                    return;
                }
            }
            else
            {
                updateComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                updateComm.CommandText = @"INSERT INTO B_MultiUser([UserName]) VALUES(@UserName)";
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.RemoveAt("@UserName");
            }

            updateComm.CommandType = CommandType.StoredProcedure;
            string strGroupID = dtMasterBAD.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0][0].ToString().Trim();
            if (iCount == 1)
            {
                updateComm.CommandText = @"usp_UpdateBeiAnDan_1";
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                updateComm.Parameters.AddWithValue("@BeiAnDanNo", strBeiAnDanNo);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 2)
            {
                updateComm.CommandText = @"usp_UpdateBeiAnDan_2";
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                updateComm.Parameters.AddWithValue("@BeiAnDanNo", strBeiAnDanNo);
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", strTax);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 3)
            {
                updateComm.CommandText = @"usp_UpdateBeiAnDan_3";
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                updateComm.Parameters.AddWithValue("@BeiAnDanNo", strBeiAnDanNo);
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", strRelease);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 4)
            {
                updateComm.CommandText = @"usp_UpdateBeiAnDan_4";
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                updateComm.Parameters.AddWithValue("@BeiAnDanNo", strBeiAnDanNo);
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", strTax);
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", strRelease);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 5)
            {
                updateComm.CommandText = @"usp_UpdateBeiAnDan_5";
                updateComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", strTax);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 6)
            {
                updateComm.CommandText = @"usp_UpdateBeiAnDan_6";
                updateComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", strRelease);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 7)
            {
                updateComm.CommandText = @"usp_UpdateBeiAnDan_7";
                updateComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", strTax);
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", strRelease);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (bExist == false)
            {
                if (iCount == 3 || iCount == 4 || iCount == 6 || iCount == 7)
                {
                    string strGdList = this.GatherGongDanList(strBeiAnDanID, updateConn);
                    if (!String.IsNullOrEmpty(strGdList)) { this.UpdateRMDroolsBalance(strGdList, updateComm, this.cmbIEtype.Text.Trim(), "ADD"); }                    
                }
            }
            updateComm.CommandType = CommandType.Text;
            updateComm.CommandText = "DELETE FROM B_MultiUser";
            updateComm.ExecuteNonQuery();
            updateComm.Dispose();
            if (updateConn.State == ConnectionState.Open)
            {
                updateConn.Close();
                updateConn.Dispose();
            }
            this.txtBeiAnDanNo.Text = string.Empty;
            MessageBox.Show("Update data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GatherGongDanList(string strBeiAnDanID, SqlConnection updateConn)
        {
            SqlDataAdapter updateAdapter = new SqlDataAdapter(@"SELECT DISTINCT [GongDan No] FROM C_BeiAnDan WHERE [BeiAnDan ID] = '" + strBeiAnDanID + "'", updateConn);
            DataTable dtGdList = new DataTable();
            updateAdapter.Fill(dtGdList);
            string strGdList = null;
            for (int i = 0; i < dtGdList.Rows.Count; i++) { strGdList += "'" + dtGdList.Rows[i][0].ToString().Trim() + "',"; }
            strGdList = strGdList.Remove(strGdList.Length - 1);
            dtGdList.Dispose();

            updateAdapter = new SqlDataAdapter("SELECT [Batch No], [FG No], [GongDan No] FROM C_GongDan WHERE [GongDan No] IN (" + strGdList + ")", updateConn);
            DataTable dtGD = new DataTable();
            updateAdapter.Fill(dtGD);
            updateAdapter.Dispose();
            strGdList = string.Empty;
            for (int i = 0; i < dtGD.Rows.Count; i++) { strGdList += "'" + dtGD.Rows[i][2].ToString().Trim() + "',"; }
            if (!String.IsNullOrEmpty(strGdList)) { strGdList = strGdList.Remove(strGdList.Length - 1); }
            dtGD.Dispose();
            return strGdList;
        }

        private void UpdateRMDroolsBalance(string strGdList, SqlCommand updateComm, string strIEType, string strAction)
        {
            updateComm.CommandText = @"usp_UpdateRMBalanceByBeiAnDan";
            updateComm.Parameters.AddWithValue("@GongDanList", strGdList);
            updateComm.Parameters.AddWithValue("@Action", strAction);
            updateComm.ExecuteNonQuery();
            updateComm.Parameters.Clear();
            if (String.Compare(strIEType, "RMB") != 0)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBalanceByBeiAnDan";
                updateComm.Parameters.AddWithValue("@GongDanList", strGdList);
                updateComm.Parameters.AddWithValue("@Action", strAction);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDanM.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
            { MessageBox.Show("Please select BeiAnDan ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            string strBeiAnDanID = this.cmbBeiAnDanID.Text.Trim().ToUpper();
            string strCustomsReleaseDate = dtMasterBAD.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0]["Customs Release Date"].ToString().Trim();
            SqlConnection deleteConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (deleteConn.State == ConnectionState.Closed) { deleteConn.Open(); }
            SqlCommand deleteComm = new SqlCommand();
            deleteComm.Connection = deleteConn;

            if (!String.IsNullOrEmpty(strCustomsReleaseDate))
            {
                MessageBox.Show("The BeiAnDan already 2nd released, system rejects to remove the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                deleteComm.Dispose();
                deleteConn.Dispose();
                return;
            }

            deleteComm.CommandType = CommandType.StoredProcedure;
            deleteComm.CommandText = "usp_DeleteHistoryBeiAnDan";         
            string strGroupID = dtMasterBAD.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0]["Group ID"].ToString().Trim();
            deleteComm.Parameters.AddWithValue("@GroupID", strGroupID);
            deleteComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
            deleteComm.Parameters.AddWithValue("@JudgeObj", "Normal");
            deleteComm.ExecuteNonQuery();
            deleteComm.Parameters.Clear();
            deleteComm.Dispose();
            if (deleteConn.State == ConnectionState.Open) { deleteConn.Close(); deleteConn.Dispose(); }
            
            this.cmbBeiAnDanID.Text = String.Empty;
            this.dgvBeiAnDanM.DataSource = DBNull.Value;
            this.dgvBeiAnDanD.DataSource = DBNull.Value;
            MessageBox.Show("Delete data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvBeiAnDanM_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (MessageBox.Show("Are you sure to update the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }
                SqlConnection ConnBADID = new SqlConnection(SqlLib.StrSqlConnection);
                if (ConnBADID.State == ConnectionState.Closed) { ConnBADID.Open(); }
                SqlCommand CommBADID = new SqlCommand();
                CommBADID.Connection = ConnBADID;
                CommBADID.CommandType = CommandType.StoredProcedure;
                CommBADID.CommandText = @"usp_UpdateBeiAnDan_8";
                string strBeiAnDanID = this.dgvBeiAnDanM["BeiAnDan ID", this.dgvBeiAnDanM.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                string strGroupID = this.dgvBeiAnDanM["Group ID", this.dgvBeiAnDanM.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                CommBADID.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                CommBADID.Parameters.AddWithValue("@GroupID", strGroupID);
                CommBADID.ExecuteNonQuery();
                CommBADID.Parameters.Clear();
                CommBADID.Dispose();
                if (ConnBADID.State == ConnectionState.Open)
                {
                    ConnBADID.Close();
                    ConnBADID.Dispose();
                }
                this.cmbBeiAnDanID.Text = "";
                MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
