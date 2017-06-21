using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class TotalPingDanDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataView dvFillDGV = new DataView();
        private DataTable dtFillDGV = new DataTable();
        private DataTable dtIE = new DataTable();
        private DataTable dtGroupID = new DataTable();
        string strFilter = null;
        private DataGridView dgvDetails = new DataGridView();
        protected PopUpFilterForm filterFrm = null;
        private static TotalPingDanDataForm totalPingDanDataFrm;
        public TotalPingDanDataForm()
        {
            InitializeComponent();
        }
        public static TotalPingDanDataForm CreateInstance()
        {
            if (totalPingDanDataFrm == null || totalPingDanDataFrm.IsDisposed)
            {
                totalPingDanDataFrm = new TotalPingDanDataForm();
            }
            return totalPingDanDataFrm;
        }

        private void TotalPingDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpFrom.CustomFormat = " ";
            this.dtpTo.CustomFormat = " ";
            this.dtpPassGateTime.CustomFormat = " ";

            this.btnUpdate.Enabled = false;
            this.btnDelete.Enabled = false;
            this.dgvPingDan.Columns[0].Visible = false;
        }

        private void TotalPingDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtFillDGV.Dispose();
            dtIE.Dispose();
            dtGroupID.Dispose();
        }

        private void dgvPingDan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvPingDan.RowCount; i++) { this.dgvPingDan[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvPingDan.RowCount; i++) { this.dgvPingDan[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvPingDan.RowCount; i++)
                    {
                        if (String.Compare(this.dgvPingDan[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvPingDan[0, i].Value = true; }

                        else { this.dgvPingDan[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvPingDan_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvPingDan.RowCount == 0) { return; }
            if (this.dgvPingDan.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvPingDan.RowCount; i++)
                {
                    if (String.Compare(this.dgvPingDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvPingDan.RowCount && iCount > 0)
                { this.dgvPingDan.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvPingDan.RowCount)
                { this.dgvPingDan.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvPingDan.Columns[0].HeaderText = "全选"; }
            }
        }

        private void dgvPingDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int iCurrentRow = this.dgvPingDan.CurrentRow.Index;
                string strGongDan = this.dgvPingDan["GongDan No", iCurrentRow].Value.ToString().Trim();

                DataRow dRow = dtFillDGV.NewRow();
                dRow["IE Type"] = this.dgvPingDan["IE Type", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["BeiAnDan ID"] = this.dgvPingDan["BeiAnDan ID", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["GongDan No"] = this.dgvPingDan["GongDan No", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["Order Category"] = this.dgvPingDan["Order Category", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["ESS/LINE"] = this.dgvPingDan["ESS/LINE", iCurrentRow].Value.ToString().Trim();
                dRow["Order No"] = this.dgvPingDan["Order No", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["FG EHB"] = this.dgvPingDan["FG EHB", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["FG CHN Name"] = this.dgvPingDan["FG CHN Name", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["Destination"] = this.dgvPingDan["Destination", iCurrentRow].Value.ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(this.dgvPingDan["GongDan Qty", iCurrentRow].Value.ToString().Trim())) { dRow["GongDan Qty"] = 0; }
                else { dRow["GongDan Qty"] = Convert.ToInt32(this.dgvPingDan["GongDan Qty", iCurrentRow].Value.ToString().Trim()); }
                if (String.IsNullOrEmpty(this.dgvPingDan["GD Balance", iCurrentRow].Value.ToString().Trim())) { dRow["GD Balance"] = 0; }
                else { dRow["GD Balance"] = Convert.ToInt32(this.dgvPingDan["GD Balance", iCurrentRow].Value.ToString().Trim()); }
                if (String.IsNullOrEmpty(this.dgvPingDan["GongDan Amount", iCurrentRow].Value.ToString().Trim())) { dRow["GongDan Amount"] = 0.0M; }
                else { dRow["GongDan Amount"] = Convert.ToDecimal(this.dgvPingDan["GongDan Amount", iCurrentRow].Value.ToString().Trim()); }
                dRow["Group ID"] = string.Empty;
                dRow["PingDan Qty"] = 0; 
                dRow["PingDan Amount"] = 0.0M; 
                dRow["PingDan ID"] = string.Empty;
                dRow["PingDan No"] = string.Empty;
                dRow["Pass Gate Time"] = DBNull.Value; 
                dRow["PingDan Date"] = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                dRow["Creater"] = loginFrm.PublicUserName;
                dRow["Remark"] = string.Empty;
                dRow["BeiAnDan No"] = this.dgvPingDan["BeiAnDan No", iCurrentRow].Value.ToString().Trim().ToUpper();             
                dtFillDGV.Rows.InsertAt(dRow, iCurrentRow + 1);
                dtFillDGV.AcceptChanges();

                DataView dv = dtFillDGV.DefaultView;
                dv.Sort = "[BeiAnDan ID], [GongDan No]";
                this.dgvPingDan.DataSource = dtFillDGV;
            }

            if (e.ColumnIndex == 2)
            {
                if (this.dgvPingDan.Columns[0].HeaderText == "全选")
                {
                    MessageBox.Show("Please choose to update the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.dgvPingDan.Focus();
                    return;
                }

                SqlConnection saveConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (saveConn.State == ConnectionState.Closed) { saveConn.Open(); }
                SqlCommand saveComm = new SqlCommand();
                saveComm.Connection = saveConn;
                saveComm.CommandType = CommandType.StoredProcedure;

                DataTable dtbl = new DataTable();
                dtbl = dvFillDGV.ToTable().Clone();
                for (int i = 0; i < this.dgvPingDan.RowCount; i++)
                {
                    if (this.dgvPingDan[0, i].EditedFormattedValue.ToString().Trim() == "True")
                    {
                        int iGongDanQty = Convert.ToInt32(this.dgvPingDan["GongDan Qty", i].Value.ToString().Trim());
                        int iGDBalance = Convert.ToInt32(this.dgvPingDan["GD Balance", i].Value.ToString().Trim());
                        int iPingDanQty = Convert.ToInt32(this.dgvPingDan["PingDan Qty", i].Value.ToString().Trim());
                        decimal dGongDanAmount = Convert.ToDecimal(this.dgvPingDan["GongDan Amount", i].Value.ToString().Trim());
                        decimal dPingDanAmount = iGongDanQty > 0 ? Math.Round(dGongDanAmount * iPingDanQty / iGongDanQty, 2) : 0.0M;                       

                        decimal dPingDanAmt = Convert.ToDecimal(this.dgvPingDan["PingDan Amount", i].Value.ToString().Trim());
                        string strGroupID = this.dgvPingDan["Group ID", i].Value.ToString().Trim().ToUpper();
                        if (dPingDanAmt == 0.0M && !String.IsNullOrEmpty(strGroupID))
                        {
                            this.dgvPingDan["GD Balance", i].Value = iGDBalance - iPingDanQty;
                            this.dgvPingDan["PingDan Amount", i].Value = dPingDanAmount;
                            dtbl.ImportRow(dvFillDGV.ToTable().Rows[i]);  
                        }                                         
                    }
                }
                dtFillDGV.AcceptChanges();
                dtbl.Columns.Remove("GD Balance");
                dtbl.Columns.Remove("PingDan ID");
                dtbl.Columns.Remove("PingDan No");
                dtbl.Columns.Remove("Pass Gate Time");
                dtbl.Columns.Remove("BeiAnDan No");

                string[] strColumn = { "GongDan No" };
                SqlLib saveLib = new SqlLib();
                DataTable dataTbl1 = saveLib.SelectDistinct(dtbl, strColumn);
                string strGongDanNo = null;
                dataTbl1.Columns.Add("PingDan Qty", typeof(Int32));               
                foreach (DataRow dr in dataTbl1.Rows)
                {
                    strGongDanNo += "'" + dr[0].ToString().Trim() + "',";
                    dr[1] = Convert.ToInt32(dtbl.Compute("SUM([PingDan Qty])", "[GongDan No] = '" + dr[0].ToString().Trim() + "'").ToString().Trim());
                }
                strGongDanNo = strGongDanNo.Remove(strGongDanNo.Length - 1);

                saveComm.CommandText = @"usp_GatherDailyPingDanForMTI";
                saveComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                saveComm.Parameters.AddWithValue("@OrderCategory", "'MTI'");
                saveComm.Parameters.AddWithValue("@IEType", "'RMB'");
                SqlDataAdapter saveAdapter = new SqlDataAdapter();
                saveAdapter.SelectCommand = saveComm;
                DataTable dataTbl2 = new DataTable();
                saveAdapter.Fill(dataTbl2);
                saveAdapter.Dispose();
                DataTable dataTbl3 = saveLib.UniteDataTable(dataTbl1, dataTbl2);
                saveLib.Dispose(0);
                dataTbl1.Dispose();
                dataTbl2.Dispose();
                dataTbl3.Columns["Line No"].SetOrdinal(0);

                saveComm.Parameters.Clear();                
                saveComm.CommandText = @"usp_InsertPingDanForMTI";
                saveComm.Parameters.AddWithValue("@TVP_MTI", dtbl);
                saveComm.ExecuteNonQuery();
                saveComm.Parameters.Clear();
                dtbl.Dispose();
                saveComm.CommandText = @"usp_UpdateBeiAnDanUsedQtyForMTI";
                saveComm.Parameters.AddWithValue("@TVP_MTI", dataTbl3);
                saveComm.ExecuteNonQuery();
                saveComm.Parameters.Clear();
                dataTbl3.Dispose();
                saveComm.Dispose();
                if (saveConn.State == ConnectionState.Open)
                {
                    saveConn.Close();
                    saveConn.Dispose();
                }
                MessageBox.Show("Save successfully", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dtpPassGateTime_ValueChanged(object sender, EventArgs e)
        {
            this.dtpPassGateTime.CustomFormat = null;
        }

        private void dtpPassGateTime_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpPassGateTime.CustomFormat = " "; }
        }

        private void cmbIEType_Enter(object sender, EventArgs e)
        {
            if (dtIE.Rows.Count == 0)
            {
                SqlLib sqlLib = new SqlLib();
                dtIE = sqlLib.GetDataTable(@"SELECT * FROM B_IEType WHERE [IE Type] <> 'RM-D'", "B_IEType").Copy();
                DataRow dr = dtIE.NewRow();
                dr["IE Type"] = String.Empty;
                dtIE.Rows.InsertAt(dr, 0);
                sqlLib.Dispose();
            }
            this.cmbIEType.DisplayMember = this.cmbIEType.ValueMember = "IE Type";
            this.cmbIEType.DataSource = dtIE;         
        }

        private void cmbIEType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rbtnMTI.Checked = false;
            this.rbtnMTO.Checked = false;

            string strIEType = this.cmbIEType.Text.Trim().ToUpper();
            if (String.Compare(strIEType, "RMB") == 0)
            {
                this.rbtnMTI.Enabled = true;
                this.rbtnMTO.Enabled = true;                             
            }
            else
            {
                this.rbtnMTI.Enabled = false;
                this.rbtnMTO.Enabled = false;
            }
        }

        private void cbPingDanID_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.cbPingDanID.Checked == true)
            //{
            //    this.cbPingDanNo.Checked = false;
            //    this.cbPassGateTime.Checked = false;
            //    this.cbPingDanNo.Enabled = false;
            //    this.cbPassGateTime.Enabled = false;
            //}
            //else
            //{
            //    this.cbPingDanNo.Checked = false;
            //    this.cbPassGateTime.Checked = false;
            //    this.cbPingDanNo.Enabled = true;
            //    this.cbPassGateTime.Enabled = true;
            //}
        }

        private void cbPingDanNo_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.cbPingDanNo.Checked == true)
            //{
            //    this.cbPassGateTime.Checked = false;
            //    this.cbPassGateTime.Enabled = false;
            //}
            //else
            //{
            //    this.cbPassGateTime.Checked = false;
            //    this.cbPassGateTime.Enabled = true;
            //}
        }

        private void cmbGroupID_Enter(object sender, EventArgs e)
        {
            string strIEType = this.cmbIEType.Text.Trim().ToUpper();
            if (!String.IsNullOrEmpty(strIEType))
            {
                string strSQL = @"SELECT DISTINCT [Group ID] FROM C_PingDan WHERE [IE Type] = '" + strIEType + "' ";
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim())) { strSQL += " AND [PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
                if (!String.IsNullOrEmpty(this.dtpTo.Text.Trim())) { strSQL += " AND [PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1).ToString("M/d/yyyy")) + "'"; }
                /*if (this.cbPingDanID.Checked == false)
                {
                    if (this.cbPingDanNo.Checked == true) { strSQL += " AND ([PingDan ID] IS NOT NULL AND [PingDan ID] <> '') AND ([PingDan No] IS NULL OR [PingDan No] = '')"; }
                    else
                    { if (this.cbPassGateTime.Checked == true) { strSQL += " AND ([PingDan No] IS NOT NULL AND [PingDan No] <> '') AND [Pass Gate Time] IS NULL"; } }
                }
                else { strSQL += " AND ([PingDan ID] IS NULL OR [PingDan ID] = '')"; }*/

                if (this.cbPingDanID.Checked == true) { strSQL += " AND ([PingDan ID] IS NULL OR [PingDan ID] = '')"; }
                if (this.cbPingDanNo.Checked == true) { strSQL += " AND ([PingDan No] IS NULL OR [PingDan No] = '')"; }
                if (this.cbPassGateTime.Checked == true) { strSQL += " AND [Pass Gate Time] IS NULL"; }

                if (String.Compare(strIEType.ToUpper(), "RMB") == 0)
                {
                    if (this.rbtnMTI.Checked == true) { strSQL += " AND [Order Category] = 'MTI'"; }
                    else { strSQL += " AND [Order Category] = 'MTO'"; }
                }

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
            {
                MessageBox.Show("Please select IE Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbIEType.Focus();
                return;
            }

            if (String.Compare(this.cmbIEType.Text.Trim().ToUpper(), "RMB") == 0 && 
                this.rbtnMTI.Checked == false && this.rbtnMTO.Checked == false)
            {
                MessageBox.Show("Please select order category(MTO/MTI).", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            strFilter = "";
            dvFillDGV.RowFilter = "";
            dtFillDGV.Rows.Clear();
            dtFillDGV.Columns.Clear();
            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;

            string strIEType = this.cmbIEType.Text.Trim().ToUpper();
            string strCheck = null;
            if (this.rbtnMTI.Checked == true) { strCheck = "MTI"; }
            if (this.rbtnMTO.Checked == true) { strCheck = "MTO"; }
            DateTime datetime = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

            sqlComm.Parameters.AddWithValue("@IEType", strIEType);
            if (String.Compare(strCheck, "MTI") != 0) { sqlComm.Parameters.AddWithValue("@OrderCategory", "MTO"); }
            else { sqlComm.Parameters.AddWithValue("@OrderCategory", "MTI"); }
            sqlComm.Parameters.AddWithValue("@Creater", loginFrm.PublicUserName);
            sqlComm.Parameters.AddWithValue("@PingDanDate", datetime);
            sqlComm.CommandText = @"usp_GatherDailyPingDanByIEType";
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
                this.dgvPingDan.Columns[0].HeaderText = "全选";
                return;
            }

            string strGroupID = this.GetPingDanGroupID(strIEType, sqlConn, sqlComm); 
            if (String.Compare(strCheck, "MTI") != 0)
            {
                mytable.Columns.Add("Group ID", typeof(string));
                mytable.Columns["Group ID"].SetOrdinal(13);
    
                int iSerial = 0, iCount = 1;
                if (String.Compare(strCheck, "MTO") != 0)
                {
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
                }
                else
                {
                    foreach (DataRow dr in mytable.Rows)
                    {
                        dr["Group ID"] = this.GetASCGroupID(iSerial, strGroupID, strIEType);
                        iSerial++;
                    }
                }               
                mytable.AcceptChanges();
                DataTable dtCopy = mytable.Copy();
                dtCopy.Columns.Remove("BeiAnDan No");

                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = @"usp_InsertPingDanByIEType";
                sqlComm.Parameters.AddWithValue("@IEType", strIEType);
                if (String.Compare(strCheck, "MTI") != 0) { sqlComm.Parameters.AddWithValue("@OrderCategory", "MTO"); }
                else { sqlComm.Parameters.AddWithValue("@OrderCategory", "MTI"); }
                sqlComm.Parameters.AddWithValue("@PingDanDate", datetime);
                sqlComm.Parameters.AddWithValue("@TVP_PD", dtCopy);
                dtCopy.Dispose();

                sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlComm;
                DataTable dtCopy1 = new DataTable();
                sqlAdapter.Fill(dtCopy1);
                sqlComm.Parameters.Clear();
         
                SqlLib sqlLibCopy = new SqlLib();
                string[] strCopy = { "GongDan No", "Group ID", "BeiAnDan No" };
                DataTable dtCopy2 = sqlLibCopy.SelectDistinct(mytable, strCopy);
                dtFillDGV = sqlLibCopy.MergeDataTable1(dtCopy1, dtCopy2, "GongDan No", "Group ID");
                dtCopy1.Dispose();
                dtCopy2.Dispose();
                sqlLibCopy.Dispose(0);
            }
            else
            {
                mytable.Columns.Remove("Line No");
                mytable.Columns.Add("Group ID", typeof(string));
                mytable.Columns.Add("PingDan Qty", typeof(Int32));
                mytable.Columns.Add("PingDan Amount", typeof(decimal));
                mytable.Columns.Add("PingDan ID", typeof(string));
                mytable.Columns.Add("PingDan No", typeof(string));
                mytable.Columns.Add("Pass Gate Time", typeof(DateTime));
                mytable.Columns.Add("Remark", typeof(string));
                mytable.Columns["BeiAnDan No"].SetOrdinal(21);
                mytable.Columns["Creater"].SetOrdinal(19);
                mytable.Columns["PingDan Date"].SetOrdinal(18);
                
                foreach (DataRow dr in mytable.Rows)
                {
                    dr["PingDan Qty"] = Convert.ToInt32(dr["GD Balance"].ToString().Trim());
                    dr["PingDan Amount"] = 0.0M;
                    dr["Pass Gate Time"] = DBNull.Value;
                }
                mytable.Rows[0]["Group ID"] = strGroupID;
                dtFillDGV = mytable.Copy();
            }

            mytable.Dispose();
            sqlAdapter.Dispose();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }

            if (String.Compare(strIEType, "RMB") != 0) { dtFillDGV.Columns.Remove("BeiAnDan No"); }
            dvFillDGV = dtFillDGV.DefaultView;
            this.dgvPingDan.DataSource = dvFillDGV;
            this.dgvPingDan.Columns[0].Visible = false;
            this.dgvPingDan.Columns[1].Visible = false;
            this.dgvPingDan.Columns[2].Visible = false;    
            this.dgvPingDan.Columns["IE Type"].Visible = false;
            this.dgvPingDan.Columns["Order Category"].Visible = false;
            this.dgvPingDan.Columns["GongDan Amount"].Visible = false;
            this.dgvPingDan.Columns["PingDan Amount"].Visible = false;
            for (int i = 3; i < this.dgvPingDan.ColumnCount; i++)
            { this.dgvPingDan.Columns[i].ReadOnly = true; }

            if (this.rbtnMTI.Checked == true)
            {
                this.dgvPingDan.Columns[0].Visible = true;
                this.dgvPingDan.Columns[1].Visible = true;
                this.dgvPingDan.Columns[2].Visible = true;          
                this.dgvPingDan.Columns["Group ID"].ReadOnly = false;
                this.dgvPingDan.Columns["PingDan Qty"].ReadOnly = false;
                this.dgvPingDan.Columns["Remark"].ReadOnly = false;

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvPingDan.EnableHeadersVisualStyles = false;
                this.dgvPingDan.Columns["Group ID"].HeaderCell.Style = cellStyle;
                this.dgvPingDan.Columns["PingDan Qty"].HeaderCell.Style = cellStyle;
                this.dgvPingDan.Columns["Remark"].HeaderCell.Style = cellStyle;
            }
            this.dgvPingDan.Columns["GongDan No"].Frozen = true; 
        } 

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEType.Text.Trim()))
            {
                MessageBox.Show("Please select IE Type before preview.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbIEType.Focus();
                return;
            }

            if (String.Compare(this.cmbIEType.Text.Trim().ToUpper(), "RMB") == 0 &&
                this.rbtnMTI.Checked == false && this.rbtnMTO.Checked == false)
            {
                MessageBox.Show("Please select order category(MTO/MTI).", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string strIEType = this.cmbIEType.Text.Trim().ToUpper();
            string strCheck = null;
            if (rbtnMTI.Checked == true) { strCheck = "MTI"; }
            if (rbtnMTO.Checked == true) { strCheck = "MTO"; }

            string strBrowse = " P.[IE Type] = '" + strIEType + "'";
            if (String.Compare(strIEType, "RMB") == 0)
            {
                if (String.Compare(strCheck, "MTI") != 0) { strBrowse += " AND P.[Order Category] = 'MTO'"; }
                else { strBrowse += " AND P.[Order Category] = 'MTI'"; }
            }
            if (this.cbPingDanID.Checked == true) { strBrowse += " AND (P.[PingDan ID] IS NULL OR P.[PingDan ID] = '')"; }
            if (this.cbPingDanNo.Checked == true) { strBrowse += " AND (P.[PingDan No] IS NULL OR p.[PingDan No] = '')"; }
            if (this.cbPassGateTime.Checked == true) { strBrowse += " AND (p.[Pass Gate Time] IS NULL OR p.[Pass Gate Time] = '')"; }
            //if (this.cbPingDanID.Checked == false)
            //{
            //    if (this.cbPingDanNo.Checked == true) { strBrowse += " AND (P.[PingDan ID] IS NOT NULL AND P.[PingDan ID] <> '') AND (P.[PingDan No] IS NULL OR P.[PingDan No] = '')"; }
            //    else
            //    { if (this.cbPassGateTime.Checked == true) { strBrowse += " AND (P.[PingDan No] IS NOT NULL AND P.[PingDan No] <> '') AND P.[Pass Gate Time] IS NULL"; } }
            //}
            //else { strBrowse += " AND (P.[PingDan ID] IS NULL OR P.[PingDan ID] = '')"; }

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
                    { strBrowse += " AND P.[PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "' AND P.[PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                { strBrowse += " AND P.[PingDan Date] < '" + Convert.ToDateTime(this.dtpTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.dtpFrom.Text.Trim()))
                { strBrowse += " AND P.[PingDan Date] >= '" + Convert.ToDateTime(this.dtpFrom.Value.ToString("M/d/yyyy")) + "'"; }
            }

            if (!String.IsNullOrEmpty(this.cmbGroupID.Text.Trim())) { strBrowse += " AND P.[Group ID] = '" + this.cmbGroupID.Text.Trim().ToUpper() + "'"; }

            strFilter = "";
            dvFillDGV.RowFilter = "";
            string strSQL;
            if (String.Compare(strIEType, "RMB") == 0)
            { strSQL = @"SELECT P.*, B.[BeiAnDan No] FROM C_PingDan AS P LEFT OUTER JOIN C_BeiAnDan AS B ON P.[GongDan No] = B.[GongDan No] WHERE" + strBrowse + " ORDER BY P.[BeiAnDan ID], P.[GongDan No]"; }
            else { strSQL = @"SELECT P.* FROM C_PingDan AS P WHERE" + strBrowse + " ORDER BY P.[BeiAnDan ID], P.[GongDan No]"; }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSQL, sqlConn);
            dtFillDGV.Columns.Clear();
            dtFillDGV.Rows.Clear();
            sqlAdapter.Fill(dtFillDGV);
            sqlAdapter.Dispose();
          
            if (dtFillDGV.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvPingDan.DataSource = DBNull.Value;             
            }
            else
            {               
                dvFillDGV = dtFillDGV.DefaultView;
                this.dgvPingDan.DataSource = dvFillDGV;
                this.dgvPingDan.Columns[0].Visible = false;
                this.dgvPingDan.Columns[1].Visible = false;
                this.dgvPingDan.Columns[2].Visible = false;
                this.dgvPingDan.Columns["GongDan No"].Frozen = true;
                this.dgvPingDan.Columns["IE Type"].Visible = false;
                this.dgvPingDan.Columns["Order Category"].Visible = false;
                this.dgvPingDan.Columns["GongDan Amount"].Visible = false;
                this.dgvPingDan.Columns["PingDan Amount"].Visible = false;                
                for (int i = 3; i < this.dgvPingDan.ColumnCount; i++)
                { this.dgvPingDan.Columns[i].ReadOnly = true; }              
            }
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data in below data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvPingDan.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            decimal dRatio = 0.0M;
            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = @"SELECT [Ratio] FROM B_WeightRatio";
            dRatio = Convert.ToDecimal(sqlComm.ExecuteScalar().ToString().Trim());
            sqlComm.Dispose();          

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
             
            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 7]).NumberFormatLocal = "@";
            worksheet.Cells[1, 1] = "Line No";
            worksheet.Cells[1, 2] = "BeiAnDan ID";
            worksheet.Cells[1, 3] = "GongDan No";
            worksheet.Cells[1, 4] = "Group ID";
            worksheet.Cells[1, 5] = "PingDan ID";
            worksheet.Cells[1, 6] = "PingDan No";
            worksheet.Cells[1, 7] = "Pass Gate Time";

            for (int y = 0; y < this.dgvPingDan.RowCount; y++)
            {
                worksheet.Cells[y + 2, 1] = y + 1;
                worksheet.Cells[y + 2, 2] = this.dgvPingDan["BeiAnDan ID", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 3] = this.dgvPingDan["GongDan No", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 4] = this.dgvPingDan["Group ID", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 5] = this.dgvPingDan["PingDan ID", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 6] = this.dgvPingDan["PingDan No", y].Value.ToString().Trim();
                worksheet.Cells[y + 2, 7] = this.dgvPingDan["Pass Gate Time", y].Value.ToString().Trim();
            }
            worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 7]).Font.Bold = true;
            worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 7]).Font.Name = "Verdana";
            worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 7]).Font.Size = 9;
            worksheet.Cells.EntireColumn.AutoFit();

            if (String.Compare(this.cmbIEType.Text.Trim(), "RMB") == 0)
            {
                object missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, 1, missing);

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 15]).NumberFormatLocal = "@";
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
                worksheet.Cells[1, 15] = "备案单号";

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
                    if (this.rbtnMTO.Checked == false) { worksheet.Cells[x + 2, 5] = iLineNo.ToString().Trim(); }
                    else { worksheet.Cells[x + 2, 5] = 1; }
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
                    worksheet.Cells[x + 2, 15] = this.dgvPingDan["BeiAnDan No", x].Value.ToString().Trim();
                }
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 15]).Font.Bold = true;
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Name = "Verdana";
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Size = 9;
                worksheet.Cells.EntireColumn.AutoFit();

                missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, 1, missing);

                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 9]).NumberFormatLocal = "@";
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
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 9]).Font.Bold = true;
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Name = "Verdana";
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Size = 9;
                worksheet.Cells.EntireColumn.AutoFit();
            }

            else if (String.Compare(this.cmbIEType.Text.Trim(), "BLP") == 0 || String.Compare(this.cmbIEType.Text.Trim(), "EXPORT") == 0)
            {
                SqlDataAdapter sqlAdp = new SqlDataAdapter("SELECT [Short Name] AS Destination, Code FROM B_Address", sqlConn);
                DataTable dtDestination = new DataTable();
                sqlAdp.Fill(dtDestination);
                sqlAdp.Dispose();

                SqlLib sqlLib = new SqlLib();
                DataTable dtMergeData = sqlLib.MergeDataTable(dvFillDGV.ToTable(), dtDestination, "Destination");
                sqlLib.Dispose(0);
                dtDestination.Clear();
                dtDestination.Dispose();

                object missing = System.Reflection.Missing.Value;
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, 1, missing);
                worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvPingDan.RowCount + 1, 9]).NumberFormatLocal = "@";
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
                dtMergeData.Clear();
                dtMergeData.Dispose();
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, 9]).Font.Bold = true;
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Name = "Verdana";
                worksheet.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Size = 9;
                worksheet.Cells.EntireColumn.AutoFit();
            }
           
            excel.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            worksheet = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            workbook = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
            GC.Collect();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
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
                //this.btnBrowse_Click(sender, e);
                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            SqlConnection uploadConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (uploadConn.State == ConnectionState.Closed) { uploadConn.Open(); }
            SqlCommand uploadComm = new SqlCommand();
            uploadComm.Connection = uploadConn;
            uploadComm.CommandType = CommandType.StoredProcedure;
            uploadComm.CommandText = @"usp_UpdatePingDan_Mass";
            uploadComm.Parameters.AddWithValue("@TVP_Mass", myTable);
            uploadComm.ExecuteNonQuery();
            uploadComm.Parameters.Clear();
            uploadComm.Dispose();
            if (uploadConn.State == ConnectionState.Open)
            {
                uploadConn.Close();
                uploadConn.Dispose();
            }           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data in below data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvPingDan.Focus();
                return;
            }

            int iCount = 0;
            string strPingDanID = null, strPingDanNo = null, strPassGateTime = null, strGroupID = this.cmbGroupID.Text.Trim().ToUpper();
            DataRow[] drow = dtFillDGV.Select("[Group ID] = '" + strGroupID + "'");
            if (!String.IsNullOrEmpty(this.txtPingDanID.Text.Trim()))
            {
                strPingDanID = this.txtPingDanID.Text.Trim().ToUpper();
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim())) 
                { 
                    iCount = 1;
                    foreach (DataRow dr in drow) { dr["PingDan ID"] = strPingDanID; }
                }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim()))
                {
                    iCount = 2;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan ID"] = strPingDanID;
                        dr["PingDan No"] = strPingDanNo;
                    }
                }
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim())) 
                { 
                    iCount = 3;
                    strPassGateTime = this.dtpPassGateTime.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan ID"] = strPingDanID;
                        dr["Pass Gate Time"] = Convert.ToDateTime(strPassGateTime);
                    }
                }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim()))
                {
                    iCount = 4;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    strPassGateTime = this.dtpPassGateTime.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan ID"] = strPingDanID;
                        dr["PingDan No"] = strPingDanNo;
                        dr["Pass Gate Time"] = Convert.ToDateTime(strPassGateTime);
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim()))
                {
                    MessageBox.Show("Please input PingDan ID, PingDan No and/or Pass Gate Time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim()))
                {
                    iCount = 5;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    foreach (DataRow dr in drow) { dr["PingDan No"] = strPingDanNo; }
                }
                if (String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim()))
                {
                    iCount = 6;
                    strPassGateTime = this.dtpPassGateTime.Text.Trim();
                    foreach (DataRow dr in drow) { dr["Pass Gate Time"] = Convert.ToDateTime(strPassGateTime); }
                }
                if (!String.IsNullOrEmpty(this.txtPingDanNo.Text.Trim()) && !String.IsNullOrEmpty(this.dtpPassGateTime.Text.Trim()))
                {
                    iCount = 7;
                    strPingDanNo = this.txtPingDanNo.Text.Trim();
                    strPassGateTime = this.dtpPassGateTime.Text.Trim();
                    foreach (DataRow dr in drow)
                    {
                        dr["PingDan No"] = strPingDanNo; 
                        dr["Pass Gate Time"] = Convert.ToDateTime(strPassGateTime);
                    }
                }
            }
            dtFillDGV.AcceptChanges();

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
                updateComm.Parameters.AddWithValue("@PassGateTime", Convert.ToDateTime(strPassGateTime));
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 4)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_4";
                updateComm.Parameters.AddWithValue("@PingDanID", strPingDanID);
                updateComm.Parameters.AddWithValue("@PingDanNo", strPingDanNo);
                updateComm.Parameters.AddWithValue("@PassGateTime", Convert.ToDateTime(strPassGateTime));
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
                updateComm.Parameters.AddWithValue("@PassGateTime", Convert.ToDateTime(strPassGateTime));
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            if (iCount == 7)
            {
                updateComm.CommandText = @"usp_UpdatePingDan_7";
                updateComm.Parameters.AddWithValue("@PingDanNo", strPingDanNo);
                updateComm.Parameters.AddWithValue("@PassGateTime", Convert.ToDateTime(strPassGateTime));
                updateComm.Parameters.AddWithValue("@GroupID", strGroupID);
                updateComm.ExecuteNonQuery();
                updateComm.Parameters.Clear();
            }
            
            updateComm.Dispose();
            if (updateConn.State == ConnectionState.Open)
            {
                updateConn.Close();
                updateConn.Dispose();
            }
            this.txtPingDanID.Text = string.Empty;
            this.txtPingDanNo.Text = string.Empty;
            MessageBox.Show("Update successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data in below data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvPingDan.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to delete the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection deleteConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (deleteConn.State == ConnectionState.Closed) { deleteConn.Open(); }
            SqlCommand deleteComm = new SqlCommand();
            deleteComm.Connection = deleteConn;
            deleteComm.CommandType = CommandType.StoredProcedure;
            deleteComm.CommandText = @"usp_DeleteDataForHistoricalPingDan";
            deleteComm.Parameters.AddWithValue("@GroupID", this.cmbGroupID.Text.Trim().ToUpper());
            deleteComm.Parameters.AddWithValue("@JudgeMTI", this.rbtnMTI.Checked.ToString().Trim().ToUpper());
            deleteComm.Parameters.AddWithValue("@TotAmt", 0.0M);
            deleteComm.ExecuteNonQuery();
            deleteComm.Parameters.Clear();
            deleteComm.Dispose();
            if (deleteConn.State == ConnectionState.Open)
            {
                deleteConn.Close();
                deleteConn.Dispose();
            }
            
            string strGroupID = this.cmbGroupID.Text.Trim();
            DataRow[] drow = dtFillDGV.Select("[Group ID] = '" + strGroupID + "'");
            foreach (DataRow dr in drow) { dr.Delete(); }
            dtFillDGV.AcceptChanges();
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
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            this.dgvPingDan.Columns[0].HeaderText = "全选";
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.CurrentCell != null)
            {
                string strColumnName = this.dgvPingDan.Columns[this.dgvPingDan.CurrentCell.ColumnIndex].Name;
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
                dvFillDGV.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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
            sqlComm.CommandText = @"SELECT MAX(SUBSTRING([Group ID], 13, LEN([Group ID]))) AS MaxID, [IE Type], SUBSTRING([Group ID], 1, 11) AS [GroupID] FROM " + 
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
