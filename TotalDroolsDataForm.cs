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
    public partial class TotalDroolsDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable dt = new DataTable();
        private static TotalDroolsDataForm totalDroolsDataFrm;
        public TotalDroolsDataForm()
        {
            InitializeComponent();
        }
        public static TotalDroolsDataForm CreateInstance()
        {
            if (totalDroolsDataFrm == null || totalDroolsDataFrm.IsDisposed)
            {
                totalDroolsDataFrm = new TotalDroolsDataForm();
            }
            return totalDroolsDataFrm;
        }

        private void TotalDroolsDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";
            this.dtpTaxDate.CustomFormat = " ";
            this.dtpReleaseDate.CustomFormat = " ";

            this.groupBoxEdit.Enabled = false;
            this.groupBoxCancel.Enabled = false;
        }

        private void TotalDroolsDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dt.Dispose();
        }

        private void cbBeiAnDanNo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbBeiAnDanNo.Checked == true)
            {
                this.cbTaxDate.Checked = false;
                this.cbReleaseDate.Checked = false;
                this.cbTaxDate.Enabled = false;
                this.cbReleaseDate.Enabled = false;
            }
            else
            {
                this.cbTaxDate.Checked = false;
                this.cbReleaseDate.Checked = false;
                this.cbTaxDate.Enabled = true;
                this.cbReleaseDate.Enabled = true;
            }
        }

        private void cbTaxDate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbTaxDate.Checked == true)
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbReleaseDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = false;
                this.cbReleaseDate.Enabled = false;
            }
            else
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbReleaseDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = true;
                this.cbReleaseDate.Enabled = true;
            }
        }

        private void cbReleaseDate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbReleaseDate.Checked == true)
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbTaxDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = false;
                this.cbTaxDate.Enabled = false;
            }
            else
            {
                this.cbBeiAnDanNo.Checked = false;
                this.cbTaxDate.Checked = false;
                this.cbBeiAnDanNo.Enabled = true;
                this.cbTaxDate.Enabled = true;
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

        private void cmbBeiAnDanID_Enter(object sender, EventArgs e)
        {
            SqlLib sqlLib = new SqlLib();
            DataTable dtable = new DataTable();
            dtable.Clear();
            string strSQL = "SELECT DISTINCT [Drools BeiAnDan ID] FROM C_BeiAnDan_Drools WHERE";
            if (this.cbBeiAnDanNo.Checked == true) { strSQL += " ([Drools BeiAnDan No] = '' OR [Drools BeiAnDan No] IS NULL) AND"; }
            if (this.cbTaxDate.Checked == true) { strSQL += " [Tax & Duty Paid Date] IS NULL AND"; }
            if (this.cbReleaseDate.Checked == true) { strSQL += " [Customs Release Date] IS NULL AND"; }
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
                    { strSQL += " [Drools BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [Drools BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else
                { strSQL += " [Drools BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strSQL += " [Drools BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND"; }
            }
            if (String.Compare(strSQL.Substring(strSQL.Trim().Length - 4), " AND") == 0) { strSQL = strSQL.Remove(strSQL.Trim().Length - 4); }
            if (String.Compare(strSQL.Substring(strSQL.Trim().Length - 6), " WHERE") == 0) { strSQL = strSQL.Remove(strSQL.Trim().Length - 6); }
            dtable = sqlLib.GetDataTable(strSQL, "C_BeiAnDan_Drools").Copy();

            DataRow drow = dtable.NewRow();
            drow["Drools BeiAnDan ID"] = String.Empty;
            dtable.Rows.InsertAt(drow, 0);
            this.cmbBeiAnDanID.DisplayMember = this.cmbBeiAnDanID.ValueMember = "Drools BeiAnDan ID";
            this.cmbBeiAnDanID.DataSource = dtable;
            sqlLib.Dispose();
        }

        private void cmbBeiAnDanID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
            {
                this.groupBoxEdit.Enabled = false;
                this.groupBoxCancel.Enabled = false;
            }
            else
            {
                this.groupBoxEdit.Enabled = true;
                this.groupBoxCancel.Enabled = true;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strbrowse = @"SELECT * FROM C_BeiAnDan_Drools WHERE";
            if (this.cbBeiAnDanNo.Checked == true) { strbrowse += " ([Drools BeiAnDan No] = '' OR [Drools BeiAnDan No] IS NULL) AND"; }
            if (this.cbTaxDate.Checked == true) { strbrowse += " [Tax & Duty Paid Date] IS NULL AND"; }
            if (this.cbReleaseDate.Checked == true) { strbrowse += " [Customs Release Date] IS NULL AND"; }
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
                    { strbrowse += " [Drools BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND [Drools BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
                }
                else
                { strbrowse += " [Drools BeiAnDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "' AND"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strbrowse += " [Drools BeiAnDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND"; }
            }
            if (!String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.ToString().Trim())) { strbrowse += " [Drools BeiAnDan ID] = '" + this.cmbBeiAnDanID.Text.ToString().Trim().ToUpper() + "' AND"; }
            if (String.Compare(strbrowse.Substring(strbrowse.Trim().Length - 4), " AND") == 0) { strbrowse = strbrowse.Remove(strbrowse.Trim().Length - 4); }
            if (String.Compare(strbrowse.Substring(strbrowse.Trim().Length - 6, 6), " WHERE") == 0) { strbrowse = strbrowse.Remove(strbrowse.Trim().Length - 6); }
            strbrowse += " ORDER BY [Drools BeiAnDan Date] DESC, [Drools BeiAnDan ID], [Drools EHB]";

            SqlConnection previewConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (previewConn.State == ConnectionState.Closed) { previewConn.Open(); }
            SqlDataAdapter previewAdapter = new SqlDataAdapter(strbrowse, previewConn);
            dt.Clear();
            previewAdapter.Fill(dt);
            previewAdapter.Dispose();

            if (dt.Rows.Count == 0)
            {
                dt.Clear();
                this.dgvDrools.DataSource = DBNull.Value;
            }
            else
            {
                this.dgvDrools.DataSource = dt;
                for (int i = 1; i < this.dgvDrools.ColumnCount; i++)
                { this.dgvDrools.Columns[i].ReadOnly = true; }
                this.dgvDrools.Columns[0].Frozen = true;
            }

            if (previewConn.State == ConnectionState.Open)
            {
                previewConn.Close();
                previewConn.Dispose();
            }
        }

        private void dtpTaxDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpTaxDate.CustomFormat = null;
        }

        private void dtpTaxDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpTaxDate.CustomFormat = " "; }
        }

        private void dtpReleaseDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpReleaseDate.CustomFormat = null;
        }

        private void dtpReleaseDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpReleaseDate.CustomFormat = " "; }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.dgvDrools.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            int iCount = 0;
            string strBeiAnDanNo = null, strTax = null, strRelease = null, strBADID = this.cmbBeiAnDanID.Text.Trim().ToUpper();
            DataRow[] drow = dt.Select("[Drools BeiAnDan ID] = '" + strBADID + "'");
            string strCustomsReleaseDate = dt.Select("[Drools BeiAnDan ID] = '" + strBADID + "'")[0]["Customs Release Date"].ToString().Trim();
            bool bExist = false;
            if (!String.IsNullOrEmpty(strCustomsReleaseDate)) { bExist = true; }

            if (!String.IsNullOrEmpty(this.txtBeiAnDanNo.Text.Trim()))
            {
                strBeiAnDanNo = this.txtBeiAnDanNo.Text.Trim().ToUpper();
                if (String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) && 
                    String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim()))
                {
                    iCount = 1;
                    foreach (DataRow dr in drow) { dr["Drools BeiAnDan No"] = strBeiAnDanNo; }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) && 
                    String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim()))
                {
                    iCount = 2;
                    strTax = this.dtpTaxDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["Drools BeiAnDan No"] = strBeiAnDanNo;
                        dr["Tax & Duty Paid Date"] = Convert.ToDateTime(strTax);
                    }
                }
                if (String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) && !String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim())) 
                { 
                    iCount = 3;
                    strRelease = this.dtpReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow) 
                    {
                        dr["Drools BeiAnDan No"] = strBeiAnDanNo;
                        dr["Customs Release Date"] = Convert.ToDateTime(strRelease); 
                    }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) && 
                    !String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim()))
                {
                    iCount = 4;
                    strTax = this.dtpTaxDate.Text.Trim();
                    strRelease = this.dtpReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["Drools BeiAnDan No"] = strBeiAnDanNo;
                        dr["Tax & Duty Paid Date"] = Convert.ToDateTime(strTax);
                        dr["Customs Release Date"] = Convert.ToDateTime(strRelease);
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) &&
                    String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim()))
                {
                    MessageBox.Show("Please input Drools BeiAnDan No and/or date time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) && 
                    String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim()))
                {
                    iCount = 5;
                    strTax = this.dtpTaxDate.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Tax & Duty Paid Date"] = Convert.ToDateTime(strTax); }
                }
                if (String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) && 
                    !String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim()))
                {
                    iCount = 6;
                    strRelease = this.dtpReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Customs Release Date"] = Convert.ToDateTime(strRelease); }
                }
                if (!String.IsNullOrEmpty(this.dtpTaxDate.Text.Trim()) && 
                    !String.IsNullOrEmpty(this.dtpReleaseDate.Text.Trim()))
                {
                    iCount = 7;
                    strTax = this.dtpTaxDate.Text.Trim();
                    strRelease = this.dtpReleaseDate.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["Tax & Duty Paid Date"] = Convert.ToDateTime(strTax);
                        dr["Customs Release Date"] = Convert.ToDateTime(strRelease);
                    }
                }
            }
            dt.AcceptChanges();

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
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
            if (iCount == 1)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBeiAnDan_1";
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanNo", strBeiAnDanNo);
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBADID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 2)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBeiAnDan_2";
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanNo", strBeiAnDanNo);
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBADID);
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", Convert.ToDateTime(strTax));
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 3) 
            {
                updateComm.CommandText = @"usp_UpdateDroolsBeiAnDan_3";
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanNo", strBeiAnDanNo);
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBADID);
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", Convert.ToDateTime(strRelease));
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 4)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBeiAnDan_4";
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanNo", strBeiAnDanNo);
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBADID);
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", Convert.ToDateTime(strTax));
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", Convert.ToDateTime(strRelease));
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 5)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBeiAnDan_5";
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", Convert.ToDateTime(strTax));
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBADID);              
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 6)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBeiAnDan_6";
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", Convert.ToDateTime(strRelease));
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBADID);                
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 7)
            {
                updateComm.CommandText = @"usp_UpdateDroolsBeiAnDan_7";
                updateComm.Parameters.AddWithValue("@TaxDutyPaidDate", Convert.ToDateTime(strTax));
                updateComm.Parameters.AddWithValue("@CustomsReleaseDate", Convert.ToDateTime(strRelease));
                updateComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBADID);               
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            updateComm.Parameters.Clear();
            if (bExist == false)
            {
                if (iCount == 3 || iCount == 4 || iCount == 6 || iCount == 7)
                {
                    string strGDList = this.GatherGongDanList(strBADID, updateConn);
                    updateComm.CommandText = @"usp_UpdateDroolsBalanceByDroolsBAD";
                    updateComm.Parameters.AddWithValue("@GongDanList", strGDList);
                    updateComm.Parameters.AddWithValue("@Action", "ADD");
                    updateComm.ExecuteNonQuery();
                    updateComm.Parameters.Clear();
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
            this.cbBeiAnDanNo.Checked = false;
            this.cbTaxDate.Checked = false;
            this.cbReleaseDate.Checked = false;
            this.txtBeiAnDanNo.Text = String.Empty;          
            this.dtpTaxDate.CustomFormat = " ";
            this.dtpReleaseDate.CustomFormat = " ";
            MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GatherGongDanList(string strDroolsBeiAnDanID, SqlConnection updateConn)
        {
            SqlDataAdapter updateAdapter = new SqlDataAdapter(@"SELECT DISTINCT [GongDan No] FROM C_BeiAnDan_Drools WHERE [Drools BeiAnDan ID] = '" + strDroolsBeiAnDanID + "'", updateConn);
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

        private void dgvDrools_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (String.IsNullOrEmpty(this.cmbBeiAnDanID.Text.Trim()))
                {
                    MessageBox.Show("Please select Drools BeiAnDan ID first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.cmbBeiAnDanID.Focus();
                    return;
                }
                if (this.dgvDrools.RowCount == 0)
                {
                    MessageBox.Show("There is no data in right grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) { return; }
              
                int iRowIndex = this.dgvDrools.CurrentRow.Index;
                string strCustomsReleaseDate = this.dgvDrools["Customs Release Date", iRowIndex].Value.ToString().Trim();
                if (!String.IsNullOrEmpty(strCustomsReleaseDate))
                {
                    MessageBox.Show("The Drools BeiAnDan already 2nd released, so reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.cbSinglyDelete.Checked = false;
                    this.txtBADID.Text = String.Empty;
                    return;
                }

                SqlConnection deleteConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (deleteConn.State == ConnectionState.Closed) { deleteConn.Open(); }
                SqlCommand deleteComm = new SqlCommand();
                deleteComm.Connection = deleteConn;               

                /*------ Monitor And Control Multiple Users ------*/
                deleteComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
                string strUserName = Convert.ToString(deleteComm.ExecuteScalar());
                if (!String.IsNullOrEmpty(strUserName))
                {
                    if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                    {
                        MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        deleteConn.Dispose();
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
                string strBeiAnDanID = this.dgvDrools["Drools BeiAnDan ID", iRowIndex].Value.ToString().Trim();                           
                if (this.cbSinglyDelete.Checked == false)
                {
                    string strGDList = this.GatherGongDanList(strBeiAnDanID, deleteConn);
                    deleteComm.CommandText = @"usp_UpdateDroolsBalanceByDroolsBAD";
                    deleteComm.Parameters.AddWithValue("@GongDanList", strGDList);
                    deleteComm.Parameters.AddWithValue("@Action", "DEL");
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.Clear();
                    deleteComm.CommandText = @"usp_DeleteDataForHistoricalDroolsBAD_Mass";
                    deleteComm.Parameters.AddWithValue("@DroolsBeiAnDanID", strBeiAnDanID);
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.Clear();

                    this.cmbBeiAnDanID.Text = String.Empty;
                    this.dgvDrools.DataSource = DBNull.Value;
                }
                else
                {
                    if (String.IsNullOrEmpty(this.txtBADID.Text.Trim()))
                    {
                        MessageBox.Show("Please input BeiAnDan ID.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        deleteComm.Dispose();
                        deleteConn.Close();
                        deleteConn.Dispose();
                        this.txtBADID.Focus();
                        return;
                    }

                    string strGongDanNo = this.dgvDrools["GongDan No", iRowIndex].Value.ToString().Trim();
                    string strDroolsEHB = this.dgvDrools["Drools EHB", iRowIndex].Value.ToString().Trim();
                    string strBADID = this.txtBADID.Text.Trim().ToUpper();
                    decimal dDroolsQty = Math.Round(Convert.ToDecimal(this.dgvDrools["Drools Qty", iRowIndex].Value.ToString().Trim()), 4);
                    deleteComm.CommandText = @"usp_DeleteDataForHistoricalDroolsBAD_Single";
                    deleteComm.Parameters.AddWithValue("@DroolsEHB", strDroolsEHB);
                    deleteComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    deleteComm.Parameters.AddWithValue("@DroolsQty", dDroolsQty);
                    deleteComm.Parameters.AddWithValue("@OldBeiAnDanID", strBeiAnDanID);
                    deleteComm.Parameters.AddWithValue("@BeiAnDanNo", string.Empty);
                    deleteComm.Parameters.AddWithValue("@TaxDutyPaidDate", DBNull.Value);
                    deleteComm.Parameters.AddWithValue("@NewBeiAnDanID", strBADID);
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.Clear();

                    deleteComm.CommandType = CommandType.Text;
                    deleteComm.Parameters.Add("@BeiAnDanID", SqlDbType.NVarChar).Value = strBeiAnDanID;
                    deleteComm.CommandText = @"SELECT * FROM E_BeiAnDan_Drools WHERE [Drools BeiAnDan ID] = @BeiAnDanID";
                    SqlDataAdapter deleteAdapter = new SqlDataAdapter();
                    deleteAdapter.SelectCommand = deleteComm;
                    DataTable dtDoc = new DataTable();
                    deleteAdapter.Fill(dtDoc);
                    deleteAdapter.Dispose();
                    deleteComm.Parameters.RemoveAt("@BeiAnDanID");  

                    deleteComm.Parameters.Add("@BADID", SqlDbType.NVarChar).Value = strBeiAnDanID;
                    deleteComm.CommandText = @"DELETE FROM E_BeiAnDan_Drools WHERE [Drools BeiAnDan ID] = @BADID";
                    deleteComm.ExecuteNonQuery();
                    deleteComm.Parameters.RemoveAt("@BADID");

                    decimal dTotalGW = 0.0M, dQty = 0.0M, dAmount = 0.0M;
                    DataRow[] rows = dtDoc.Select("[备件号] = '" + strDroolsEHB + "'");
                    foreach (DataRow row in rows)
                    {
                        dTotalGW = Convert.ToDecimal(row["总毛重"].ToString().Trim());
                        dQty = Convert.ToDecimal(row["数量"].ToString().Trim());
                        dAmount = Convert.ToDecimal(row["金额"].ToString().Trim());
                    }
                    dTotalGW = dTotalGW - dDroolsQty;
                    dAmount = Math.Round(dAmount * (dQty - dDroolsQty) / dQty, 2);
                    dQty = dQty - dDroolsQty;


                    int i = 0;
                    foreach (DataRow dr in dtDoc.Rows)
                    {
                        string strDrools = dr["备件号"].ToString().Trim();
                        deleteComm.Parameters.Add("@BADID", SqlDbType.NVarChar).Value = strBADID;
                        deleteComm.Parameters.Add("@BADNO", SqlDbType.NVarChar).Value = String.Empty;
                        deleteComm.Parameters.Add("@DZLX", SqlDbType.NVarChar).Value = dr["单证类型"].ToString().Trim();
                        deleteComm.Parameters.Add("@TGW", SqlDbType.Decimal).Value = dTotalGW;
                        deleteComm.Parameters.Add("@MYFS", SqlDbType.NVarChar).Value = dr["贸易方式"].ToString().Trim();
                        deleteComm.Parameters.Add("@JZQYDM", SqlDbType.NVarChar).Value = dr["结转企业代码"].ToString().Trim();
                        deleteComm.Parameters.Add("@XH", SqlDbType.Int).Value = ++i;
                        deleteComm.Parameters.Add("@BJH", SqlDbType.NVarChar).Value = strDrools;
                        if (String.Compare(strDrools, strDroolsEHB) == 0)
                        {
                            deleteComm.Parameters.Add("@QTY", SqlDbType.Decimal).Value = dQty;
                            deleteComm.Parameters.Add("@NW", SqlDbType.Decimal).Value = dQty;
                            deleteComm.Parameters.Add("@GW", SqlDbType.Decimal).Value = dQty;
                            deleteComm.Parameters.Add("@AMT", SqlDbType.Decimal).Value = dAmount;
                        }
                        else
                        {
                            deleteComm.Parameters.Add("@QTY", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["数量"].ToString().Trim());
                            deleteComm.Parameters.Add("@NW", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["净重"].ToString().Trim());
                            deleteComm.Parameters.Add("@GW", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["毛重"].ToString().Trim());
                            deleteComm.Parameters.Add("@AMT", SqlDbType.Decimal).Value = Convert.ToDecimal(dr["金额"].ToString().Trim());
                        }
                        deleteComm.Parameters.Add("@BZ", SqlDbType.NVarChar).Value = dr["币制"].ToString().Trim();
                        deleteComm.Parameters.Add("@YCD", SqlDbType.NVarChar).Value = dr["原产地/目的地"].ToString().Trim();
                        deleteComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        deleteComm.CommandText = @"INSERT INTO E_BeiAnDan_Drools([Drools BeiAnDan ID], [备案单号], [单证类型], [总毛重], [贸易方式], [结转企业代码], " +
                                                  "[项号], [备件号], [数量], [净重], [毛重], [金额], [币制], [原产地/目的地], [Created Date]) VALUES(@BADID, @BADNO, " +
                                                  "@DZLX, @TGW, @MYFS, @JZQYDM, @XH, @BJH, @QTY, @NW, @GW, @AMT, @BZ, @YCD, @CreatedDate)";
                        deleteComm.ExecuteNonQuery();
                        deleteComm.Parameters.Clear();
                    }
                    dtDoc.Dispose();
                    this.cbSinglyDelete.Checked = false;
                    this.cmbBeiAnDanID.Text = this.txtBADID.Text.Trim().ToUpper();
                    this.txtBADID.Text = String.Empty;
                    this.btnPreview_Click(sender, e);
                }

                deleteComm.CommandType = CommandType.Text;
                deleteComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                deleteComm.ExecuteNonQuery();
                deleteComm.Dispose();
                if (deleteConn.State == ConnectionState.Open)
                {
                    deleteConn.Close();
                    deleteConn.Dispose();
                }                               
                MessageBox.Show("Done successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    } 
}
