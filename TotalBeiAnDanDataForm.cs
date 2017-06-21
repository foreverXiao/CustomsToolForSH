using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class TotalBeiAnDanDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        DataTable dt = new DataTable();       
        DataTable dtBeiAnDanM = new DataTable();
        DataTable dtBeiAnDanD = new DataTable();
        private static TotalBeiAnDanDataForm totalBeiAnDanDataFrm;
        public TotalBeiAnDanDataForm()
        {
            InitializeComponent();
        }
        public static TotalBeiAnDanDataForm CreateInstance()
        {
            if (totalBeiAnDanDataFrm == null || totalBeiAnDanDataFrm.IsDisposed)
            {
                totalBeiAnDanDataFrm = new TotalBeiAnDanDataForm();
            }
            return totalBeiAnDanDataFrm;
        }

        private void TotalBeiAnDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";
            this.dtpTaxPaidDate.CustomFormat = " ";
            this.dtpCustomsReleaseDate.CustomFormat = " ";
            this.btnUpdate.Enabled = false;
            this.btnDelete.Enabled = false;

            SqlLib sqlLib = new SqlLib();
            dt.Rows.Clear();
            dt.Columns.Clear();
            dt = sqlLib.GetDataTable(@"SELECT * FROM B_IEType WHERE [IE Type] <> 'RM-D'", "B_IEType").Copy();
            DataRow dr = dt.NewRow();
            dr["IE Type"] = String.Empty;
            dt.Rows.InsertAt(dr, 0);
            this.cmbIEtype.DisplayMember = this.cmbIEtype.ValueMember = "IE Type";
            this.cmbIEtype.DataSource = dt;
            sqlLib.Dispose();
        }

        private void TotalBeiAnDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dt.Dispose();
            dtBeiAnDanM.Dispose();
            dtBeiAnDanD.Dispose();
        }

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

        private void cmbIEtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbBeiAnDanID.Text = String.Empty;
        }

        private void cmbBeiAnDanID_Enter(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEtype.Text.Trim()))
            {
                MessageBox.Show("Please select IE type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbIEtype.Focus();
                return;
            }

            string strJudge = " [IE Type] = '" + this.cmbIEtype.Text.Trim().ToUpper() + "'";
            if (this.cbBeiAnDanNo.Checked == true) { strJudge += " AND ([BeiAnDan No] = '' OR [BeiAnDan No] IS NULL)"; }
            if (this.cbTaxPaidDate.Checked == true) { strJudge += " AND ([Tax & Duty Paid Date] IS NULL OR [Tax & Duty Paid Date] = '')"; }
            if (this.cbCustomsReleaseDate.Checked == true) { strJudge += " AND ([Customs Release Date] IS NULL OR [Customs Release Date] = '')"; }
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
                    { strJudge += " AND [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strJudge += " AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEtype.Text.Trim()))
            {
                MessageBox.Show("Please select IE type before preview.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbIEtype.Focus();
                return;
            }

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
                    {
                        MessageBox.Show("Begin date is not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.dtpFrom.Focus();
                        return;
                    }
                    else
                    { strBrowse += " AND [BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse += " AND [BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
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
            BrowseComm.CommandText = @"usp_BeiAnDan_BrowseHistoricalData";
            BrowseComm.Parameters.AddWithValue("@Browse", strBrowse);
            BrowseComm.Parameters.AddWithValue("@Judge", "MASTER");
            SqlDataAdapter BrowseAdapter = new SqlDataAdapter();
            BrowseAdapter.SelectCommand = BrowseComm;
            dtBeiAnDanM.Rows.Clear();
            dtBeiAnDanM.Columns.Clear();
            BrowseAdapter.Fill(dtBeiAnDanM);
            BrowseComm.Parameters.Clear();

            if (dtBeiAnDanM.Rows.Count == 0)
            {
                BrowseAdapter.Dispose();
                BrowseComm.Dispose();
                this.dgvBeiAnDanM.DataSource = DBNull.Value;
                this.dgvBeiAnDanD.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);               
            }
            else 
            {
                BrowseComm.CommandText = @"usp_BeiAnDan_BrowseHistoricalData";
                BrowseComm.Parameters.AddWithValue("@Browse", strBrowse);
                BrowseComm.Parameters.AddWithValue("@Judge", "DETAIL");
                BrowseAdapter = new SqlDataAdapter();
                BrowseAdapter.SelectCommand = BrowseComm;
                dtBeiAnDanD.Rows.Clear();
                dtBeiAnDanD.Columns.Clear();
                BrowseAdapter.Fill(dtBeiAnDanD);
                BrowseAdapter.Dispose();
                BrowseComm.Parameters.Clear();

                this.dgvBeiAnDanM.DataSource = dtBeiAnDanM;              
                this.dgvBeiAnDanM.Columns["IE Type"].Visible = false;
                this.dgvBeiAnDanM.Columns["Group ID"].Frozen = true;
                for (int i = 1; i < this.dgvBeiAnDanM.Columns.Count - 1; i++)
                { 
                    this.dgvBeiAnDanM.Columns[i].ReadOnly = true;
                    this.dgvBeiAnDanM.Columns["BeiAnDan ID"].ReadOnly = false;
                    this.dgvBeiAnDanM.Columns["IE Remark_2"].ReadOnly = false;
                }

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvBeiAnDanM.EnableHeadersVisualStyles = false;
                this.dgvBeiAnDanM.Columns["BeiAnDAn ID"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDanM.Columns["IE Remark_2"].HeaderCell.Style = cellStyle;

                this.dgvBeiAnDanD.DataSource = dtBeiAnDanD;
                this.dgvBeiAnDanD.Columns["Group ID"].Frozen = true;
            }

            BrowseComm.Dispose();
            if (BrowseConn.State == ConnectionState.Open)
            {
                BrowseConn.Close();
                BrowseConn.Dispose();
            }
        }

        private void dtpTaxPaidDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTaxPaidDate.CustomFormat = "M/dd/yyyy HH:mm";
        }

        private void dtpTaxPaidDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTaxPaidDate.CustomFormat = " "; }
        }

        private void dtpCustomsReleaseDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpCustomsReleaseDate.CustomFormat = "M/dd/yyyy HH:mm";
        }

        private void dtpCustomsReleaseDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpCustomsReleaseDate.CustomFormat = " "; }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Only BeiAnDan of IE Type "RMB-1418" and "RMB" will have "Tax & Duty Paid Date"
            //For "RMB-1418", "Tax & Duty" are Paid After "2nd Release", i.e. "Tax & Duty Paid Date" can be blank if not paid yet
            //For "RMB",  "Tax & Duty" are Paid Before "2nd Release", i.e. if "2nd Release Date" is available, "Tax & Duty Paid Date" must be provided

            if (this.dgvBeiAnDanM.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
            {
                MessageBox.Show("Please select BeiAnDan ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbBeiAnDanID.Focus();
                return;
            }
            if (MessageBox.Show("Are you sure to update the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            string strBeiAnDanID = this.cmbBeiAnDanID.Text.Trim().ToUpper();
            DataRow[] drow = dtBeiAnDanM.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'");
            string strCustomsReleaseDate = dtBeiAnDanM.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0]["Customs Release Date"].ToString().Trim();
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
                {
                    MessageBox.Show("Please input BeiAnDan No and/or date time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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
            dtBeiAnDanM.AcceptChanges();

            SqlConnection updateConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (updateConn.State == ConnectionState.Closed) { updateConn.Open(); }
            SqlCommand updateComm = new SqlCommand();
            updateComm.Connection = updateConn;
           
            /*------ Monitor And Control Multiple Users ------*/
            updateComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
            string strUserName = Convert.ToString(updateComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait this user to log out.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    updateConn.Dispose();
                    updateComm.Dispose();
                    this.btnUpdate.Focus();
                    return;
                }
            }
            else
            {
                updateComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                updateComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.RemoveAt("@UserName");
            }

            updateComm.CommandType = CommandType.StoredProcedure;
            string strGroupID = dtBeiAnDanM.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0][0].ToString().Trim();
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
                    string strGDList = this.GatherGongDanList(strBeiAnDanID, updateConn);
                    this.UpdateRMDroolsBalance(strGDList, updateComm, this.cmbIEtype.Text.Trim(), "ADD"); 
                }
            }
            updateComm.CommandType = CommandType.Text;
            updateComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
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
            DataTable dtGDList = new DataTable();
            updateAdapter.Fill(dtGDList);
            updateAdapter.Dispose();
            string strGDList = null;
            for (int i = 0; i < dtGDList.Rows.Count; i++)
            { strGDList += "'" + dtGDList.Rows[i][0].ToString().Trim() + "',"; }
            strGDList = strGDList.Remove(strGDList.Length - 1);
            dtGDList.Dispose();
            return strGDList;
        }

        private void UpdateRMDroolsBalance(string strGDList, SqlCommand updateComm, string strIEType, string strAction)
        {
            updateComm.CommandText = @"usp_UpdateRMBalanceByBeiAnDan";
            updateComm.Parameters.AddWithValue("@GongDanList", strGDList);
            updateComm.Parameters.AddWithValue("@Action", strAction);
            updateComm.ExecuteNonQuery();
            updateComm.Parameters.Clear();
            if (String.Compare(strIEType, "RMB") != 0)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBalanceByBeiAnDan";
                updateComm.Parameters.AddWithValue("@GongDanList", strGDList);
                updateComm.Parameters.AddWithValue("@Action", strAction);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDanM.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
            {
                MessageBox.Show("Please select BeiAnDan ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbBeiAnDanID.Focus();
                return;
            }
            if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            string strBeiAnDanID = this.cmbBeiAnDanID.Text.Trim().ToUpper();
            string strCustomsReleaseDate = dtBeiAnDanM.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0]["Customs Release Date"].ToString().Trim();
            SqlConnection deleteConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (deleteConn.State == ConnectionState.Closed) { deleteConn.Open(); }
            SqlCommand deleteComm = new SqlCommand();
            deleteComm.Connection = deleteConn;

            if (!String.IsNullOrEmpty(strCustomsReleaseDate))
            {
                MessageBox.Show("The BeiAnDan already 2nd released, system rejects to remove the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                deleteComm.Dispose();
                deleteConn.Close();
                deleteConn.Dispose();
                return;
            }

            /*deleteComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
            string strUserName = Convert.ToString(deleteComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    deleteComm.Dispose();
                    deleteComm.Dispose();
                    this.btnUpdate.Focus();
                    return;
                }
            }
            else
            {
                deleteComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                deleteComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                deleteComm.ExecuteNonQuery();
                deleteComm.Parameters.RemoveAt("@UserName");
            }
          
            deleteComm.CommandType = CommandType.StoredProcedure;
            if (!String.IsNullOrEmpty(strCustomsReleaseDate))
            {
                string strGDList = this.GatherGongDanList(strBeiAnDanID, deleteConn);
                this.UpdateRMDroolsBalance(strGDList, deleteComm, this.cmbIEtype.Text.Trim(), "DEL"); 
            }*/

            deleteComm.CommandType = CommandType.StoredProcedure;
            deleteComm.CommandText = @"usp_DeleteDataForHistoricalBeiAnDan";         
            string strGroupID = dtBeiAnDanM.Select("[BeiAnDan ID] = '" + strBeiAnDanID + "'")[0]["Group ID"].ToString().Trim();
            deleteComm.Parameters.AddWithValue("@GroupID", strGroupID);
            deleteComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
            deleteComm.Parameters.AddWithValue("@JudgeObj", "Normal");
            deleteComm.ExecuteNonQuery();
            deleteComm.Parameters.Clear();
            //deleteComm.CommandType = CommandType.Text;
            //deleteComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
            //deleteComm.ExecuteNonQuery();
            deleteComm.Dispose();
            if (deleteConn.State == ConnectionState.Open)
            {
                deleteConn.Close();
                deleteConn.Dispose();
            }
            
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
                SqlConnection remarkConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (remarkConn.State == ConnectionState.Closed) { remarkConn.Open(); }               
                SqlCommand remarkComm = new SqlCommand();
                remarkComm.Connection = remarkConn;
                remarkComm.CommandType = CommandType.StoredProcedure;
                remarkComm.CommandText = @"usp_UpdateBeiAnDan_8";

                string strBeiAnDanID = this.dgvBeiAnDanM["BeiAnDan ID", this.dgvBeiAnDanM.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                string strIERemark = this.dgvBeiAnDanM["IE Remark_2", this.dgvBeiAnDanM.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                string strGroupID = this.dgvBeiAnDanM["Group ID", this.dgvBeiAnDanM.CurrentRow.Index].Value.ToString().Trim().ToUpper();              
                remarkComm.Parameters.AddWithValue("@BeiAnDanID", strBeiAnDanID);
                remarkComm.Parameters.AddWithValue("@IERemark2", strIERemark);
                remarkComm.Parameters.AddWithValue("@GroupID", strGroupID);
                remarkComm.ExecuteNonQuery();
                remarkComm.Parameters.Clear();              
                remarkComm.Dispose();
                if (remarkConn.State == ConnectionState.Open)
                {
                    remarkConn.Close();
                    remarkConn.Dispose();
                }
                this.cmbBeiAnDanID.Text = "";
                MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
