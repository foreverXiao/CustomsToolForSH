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
    public partial class GetDistributionPingDanDataForm : Form
    {
        DataTable dtIE = new DataTable();
        DataTable dtExRate = new DataTable();
        DataTable dtPublicPD = new DataTable();
        string strFilter = null;
        decimal dTotAmt = 0.0M;
        private LoginForm loginFrm = new LoginForm();              
        private DataGridView dgvDetails = new DataGridView();
        protected DataView dvPublicPD = new DataView();
        protected PopUpFilterForm filterFrm = null;

        private static GetDistributionPingDanDataForm GetDistributionPingDanDataFrm;
        public GetDistributionPingDanDataForm() { InitializeComponent(); }
        public static GetDistributionPingDanDataForm CreateInstance()
        {
            if (GetDistributionPingDanDataFrm == null || GetDistributionPingDanDataFrm.IsDisposed)
            { GetDistributionPingDanDataFrm = new GetDistributionPingDanDataForm(); }
            return GetDistributionPingDanDataFrm;
        }

        private void GetDistributionPingDanDataForm_Load(object sender, EventArgs e)
        {
            this.dtpPDFrom.CustomFormat = " ";
            this.dtpPDTo.CustomFormat = " ";

            this.btnAdjustIEtype.Enabled = false;
            this.btnCheckDeposit.Enabled = false;
            this.btnDownloadDoc.Enabled = false;
            this.btnSaveData.Enabled = false;

            this.dgvPingDan.Columns[0].Visible = false;
            this.dgvPingDan.Columns[1].Visible = false;
            this.dgvPingDan.Columns[2].Visible = false;

            SqlLib sqlLib = new SqlLib();
            dtIE = sqlLib.GetDataTable(@"SELECT [ObjectValue] AS [IE Type] FROM B_SysInfo WHERE [ObjectName] = 'IE Type'").Copy();
            dtExRate = sqlLib.GetDataTable(@"SELECT [ObjectName] AS [Object], [ObjectValue] AS [Rate] FROM B_SysInfo WHERE [ObjectName] LIKE 'ExchangeRate%'").Copy();
            sqlLib.Dispose(0);
        }
        private void GetDistributionPingDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtIE.Dispose();
            dtExRate.Dispose();
            dtPublicPD.Dispose();
        }
        private void dtpPDFrom_ValueChanged(object sender, EventArgs e) { this.dtpPDFrom.CustomFormat = null; }
        private void dtpPDFrom_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpPDFrom.CustomFormat = " "; } }
        private void dtpPDTo_ValueChanged(object sender, EventArgs e) { this.dtpPDTo.CustomFormat = null; }
        private void dtpPDTo_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) { this.dtpPDTo.CustomFormat = " "; } }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvPublicPD.RowFilter = "";
            SqlConnection ConnPD = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnPD.State == ConnectionState.Closed) { ConnPD.Open(); }
            SqlCommand CommPD = new SqlCommand();
            CommPD.Connection = ConnPD;
            //CommPD.CommandText = "SELECT [Available Balance] FROM B_Deposit";
            //decimal dAvalBal = Convert.ToDecimal(CommPD.ExecuteScalar());
            //if (dAvalBal < 0.0M)
            //{
            //    MessageBox.Show("The deposit is negative now, system reject to do.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    CommPD.Dispose();
            //    ConnPD.Dispose();
            //    return;
            //}

            CommPD.Parameters.Clear();
            CommPD.CommandType = CommandType.StoredProcedure;
            CommPD.CommandText = @"usp_GetPingDanForRMD";
            CommPD.Parameters.AddWithValue("@Obj", "A");
            SqlDataAdapter AdapterPD = new SqlDataAdapter();
            AdapterPD.SelectCommand = CommPD;
            dtPublicPD.Clear();
            AdapterPD.Fill(dtPublicPD);
            AdapterPD.Dispose();
            if (dtPublicPD.Rows.Count == 0)
            {
                this.dgvPingDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                CommPD.Dispose();
                ConnPD.Dispose();
                return;
            }

            foreach (DataRow dr in dtPublicPD.Rows)
            {
                decimal dAmount = 0.0M;
                decimal dGongDanQty = Convert.ToDecimal(dr["GongDan Qty"].ToString().Trim());
                if (String.Compare(dr["Order Currency"].ToString().Trim(), "USD") != 0)
                {
                    string strObject = dr["Order Currency"].ToString().Trim();
                    DataRow[] drRate = dtExRate.Select("[Object] = 'ExchangeRate:" + strObject + "'");
                    if (drRate.Length == 0) { dAmount = 0.0M; }
                    else
                    {
                        decimal dPrice = Math.Round(Convert.ToDecimal(dr["Order Price"].ToString()) * Convert.ToDecimal(drRate[0][1].ToString().Trim()), 4);
                        dAmount = Math.Round(dPrice * dGongDanQty, 2);
                    }
                }
                else { dAmount = Math.Round(Convert.ToDecimal(dr["Order Price"].ToString().Trim()) * dGongDanQty, 2); }
                dr["GongDan Amount(USD)"] = dAmount;
            }
            dtPublicPD.AcceptChanges();

            CommPD.Parameters.Clear();
            CommPD.CommandText = @"usp_GetPingDanForRMD";
            CommPD.Parameters.AddWithValue("@Obj", "B");
            SqlDataAdapter AdpPD = new SqlDataAdapter();
            AdpPD.SelectCommand = CommPD;
            DataTable dtMiddle = new DataTable();
            AdpPD.Fill(dtMiddle);

            CommPD.Parameters.Clear();
            CommPD.CommandText = @"usp_GetPingDanForRMD";
            CommPD.Parameters.AddWithValue("@Obj", "C");
            AdpPD.SelectCommand = CommPD;
            DataTable dtMidDetail = new DataTable();
            AdpPD.Fill(dtMidDetail);
            dtMidDetail.Columns.Remove("Item No");
            dtMidDetail.Columns.Remove("Lot No");

            CommPD.Parameters.Clear();
            CommPD.CommandType = CommandType.Text;
            CommPD.CommandText = "SELECT [ObjectValue] FROM B_SysInfo WHERE [ObjectName] = 'TaxParameter'";
            int iPara = Convert.ToInt32(Convert.ToString(CommPD.ExecuteScalar()));
            string strGroupId = this.GetGroupID(ConnPD, AdpPD), strMessage = null;
            int iSerial = 0;
            foreach (DataRow drPingDan in dtPublicPD.Rows)
            {
                DataRow[] drow = dtMiddle.Select("[GongDan No] = '" + drPingDan["GongDan No"].ToString().Trim() + "'");
                if (drow.Length > iPara)
                {
                    drPingDan["Group ID"] = this.GetASCGroupID(strGroupId, iSerial);
                    iSerial++;
                    foreach (DataRow dr in drow) { dtMiddle.Rows.Remove(dr); }
                    dtMiddle.AcceptChanges();
                    strMessage += drPingDan["GongDan No"].ToString().Trim() + "\n";
                }
            }
            dtPublicPD.AcceptChanges();

            if (dtMiddle.Rows.Count > 0)
            {
                DataTable dtScore = new DataTable();
                this.GetGroupByGongDanScore(dtMiddle, out dtScore, iPara, ConnPD, AdpPD);
                DataTable dtChName = new DataTable();
                this.GetGroupByChineseName(dtMiddle, out dtChName, dtMidDetail, iPara, ConnPD, AdpPD);
                DataTable dtThirdSort = new DataTable();
                this.GetGroupByThirdSort(dtMiddle, out dtThirdSort, dtChName, iPara, ConnPD, AdpPD);
                dtChName.Columns.Remove("Total Score");
                dtThirdSort.Columns.Remove("Total Score");

                string[] str = { "Group ID" };
                SqlLib SQLLib = new SqlLib();
                DataTable dt1 = SQLLib.SelectDistinct(dtScore, str);
                int iScore = dt1.Rows.Count;
                dt1.Dispose();
                DataTable dt2 = SQLLib.SelectDistinct(dtChName, str);
                int iChName = dt2.Rows.Count;
                dt2.Dispose();
                DataTable dt3 = SQLLib.SelectDistinct(dtThirdSort, str);
                int iThirdSort = dt3.Rows.Count;
                dt3.Dispose();
                SQLLib.Dispose(0);                
                dtPublicPD.Reset();
                if (iScore < iChName)
                {
                    if (iScore < iThirdSort) { dtPublicPD = dtScore.Copy(); }
                    else { dtPublicPD = dtThirdSort.Copy(); }
                }
                else
                {
                    if (iChName > iThirdSort) { dtPublicPD = dtThirdSort.Copy(); }
                    else { dtPublicPD = dtChName.Copy(); }
                }
                dtScore.Dispose();
                dtChName.Dispose();
                dtThirdSort.Dispose();
            }
            dtMiddle.Dispose();
            dtMidDetail.Dispose();
            AdpPD.Dispose();
            CommPD.Dispose();
            if (ConnPD.State == ConnectionState.Open) { ConnPD.Close(); ConnPD.Dispose(); }
            dtPublicPD.AcceptChanges();
            DataView dview = dtPublicPD.DefaultView;
            dview.Sort = "Group ID ASC, GongDan No ASC";
            dtPublicPD = dview.ToTable();
            dvPublicPD = dtPublicPD.DefaultView;
            this.dgvPingDan.DataSource = dvPublicPD;
            this.dgvPingDan.Columns[0].HeaderText = "Select";
            for (int i = 3; i < this.dgvPingDan.ColumnCount; i++) { this.dgvPingDan.Columns[i].ReadOnly = true; }
            this.dgvPingDan.Columns["IE Type"].ReadOnly = false;            
            this.dgvPingDan.Columns["GongDan Amount(USD)"].ReadOnly = false;
            //this.dgvPingDan.Columns["Remark"].ReadOnly = false;
            this.dgvPingDan.Columns[0].Visible = true;
            this.dgvPingDan.Columns[1].Visible = true;
            this.dgvPingDan.Columns["FG No"].Visible = false;
            this.dgvPingDan.Columns["BeiAnDan ID"].Visible = false;
            this.dgvPingDan.Columns["Duty Rate"].Visible = false;
            this.dgvPingDan.Columns["GongDan No"].Frozen = true;

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = Color.FromArgb(178, 235, 140);
            this.dgvPingDan.EnableHeadersVisualStyles = false;
            this.dgvPingDan.Columns["IE Type"].HeaderCell.Style = cellStyle;
            this.dgvPingDan.Columns["GongDan Amount(USD)"].HeaderCell.Style = cellStyle;
            //this.dgvPingDan.Columns["Remark"].HeaderCell.Style = cellStyle;

            if (!String.IsNullOrEmpty(strMessage))
            { MessageBox.Show("The number of bonded material more than " + iPara.ToString().Trim() + " for below GongDan:\n" + strMessage, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            this.btnAdjustIEtype.Enabled = true;
            this.btnCheckDeposit.Enabled = true;
            this.btnDownloadDoc.Enabled = true;
            this.btnSaveData.Enabled = true;
        }

        private string GetGroupID(SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            string strDate = System.DateTime.Now.ToString("yyyyMMdd").Trim();
            string strGroupID = @"SELECT SUBSTRING([Group ID], 3, 8) AS [GroupID], MAX(CAST(SUBSTRING([Group ID], 12, LEN([Group ID])) AS Int)) AS [MaxID], [IE Type] FROM " +
                                 "C_PingDan WHERE [IE Type] = 'RM-D' AND [Group ID] LIKE '%" + strDate + "%' GROUP BY SUBSTRING([Group ID], 3, 8), [IE Type]";
            sqlAdp = new SqlDataAdapter(strGroupID, sqlConn);
            DataTable dtGroupID = new DataTable();
            sqlAdp.Fill(dtGroupID);

            string strSuffix = null;
            if (dtGroupID.Rows.Count == 0) { strSuffix = "01"; }
            else
            {
                strSuffix = dtGroupID.Rows[0]["MaxID"].ToString().Trim();
                int iNumber1 = Convert.ToInt32(strSuffix) + 1;
                if (iNumber1.ToString().Trim().Length == 1) { strSuffix = "0" + iNumber1.ToString().Trim(); }
                else { strSuffix = iNumber1.ToString().Trim(); }
            }
            dtGroupID.Dispose();
            return "H-" + strDate + "-" + strSuffix;
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

        private void GetGroupByGongDanScore(DataTable dtMiddle, out DataTable dtScore, int iPara, SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            dtScore = dtPublicPD.Copy();
            DataView dv = dtScore.DefaultView;
            dv.Sort = "FG CHN Name DESC, FG EHB ASC";
            dtScore = dv.ToTable();

            DataTable dtCombineRM = new DataTable();
            dtCombineRM = dtMiddle.Clone();
            string strGD = null;
            int iRow = dtScore.Rows.Count, iCount = 0;
            for (int j = 0; j < iRow; j++)
            {
                string strGongDan = dtScore.Rows[j]["GongDan No"].ToString().Trim();
                string strGroup = dtScore.Rows[j]["Group ID"].ToString().Trim();
                if (String.IsNullOrEmpty(strGroup))
                {
                    DataRow[] drow = dtMiddle.Select("[GongDan No] = '" + strGongDan + "'");
                    foreach (DataRow row in drow)
                    {
                        string strRMEHB = row["RM EHB"].ToString().Trim();
                        string strCountry = row["Country of Origin"].ToString().Trim();
                        DataRow[] rows = dtCombineRM.Select("[RM EHB] = '" + strRMEHB + "' AND [Country of Origin] = '" + strCountry + "'");
                        if (rows.Length == 0)
                        {
                            DataRow dr = dtCombineRM.NewRow();
                            dr["RM EHB"] = strRMEHB;
                            dr["Country of Origin"] = strCountry;
                            dtCombineRM.Rows.Add(dr);
                        }
                        dtCombineRM.AcceptChanges();
                    }
                    if (dtCombineRM.Rows.Count < iPara)
                    {
                        strGD += strGongDan + ";";
                        if (j == iRow - 1)
                        {
                            string strID = this.GetGroupID(sqlConn, sqlAdp);
                            strGD = strGD.Remove(strGD.Trim().Length - 1);
                            int iLen = strGD.Split(';').Length;
                            for (int k = 0; k < iLen; k++)
                            {
                                string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                                DataRow rows = dtScore.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                                rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                            }
                            iCount++;
                            strGD = String.Empty;
                            dtCombineRM.Clear();
                        }
                    }
                    else if (dtCombineRM.Rows.Count == iPara)
                    {
                        string strID = this.GetGroupID(sqlConn, sqlAdp);
                        strGD += strGongDan;
                        int iLen = strGD.Split(';').Length;
                        for (int k = 0; k < iLen; k++)
                        {
                            string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                            DataRow rows = dtScore.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                            rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                        }
                        iCount++;
                        strGD = String.Empty;
                        dtCombineRM.Clear();
                    }
                    else
                    {
                        string strID = this.GetGroupID(sqlConn, sqlAdp);
                        strGD = strGD.Remove(strGD.Trim().Length - 1);
                        int iLen = strGD.Split(';').Length;
                        for (int k = 0; k < iLen; k++)
                        {
                            string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                            DataRow rows = dtScore.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                            rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                        }
                        iCount++;
                        j--;
                        strGD = String.Empty;
                        dtCombineRM.Clear();
                    }
                }
            }
            dtCombineRM.Dispose();
            dtScore.AcceptChanges();
        }

        private void GetGroupByChineseName(DataTable dtMiddle, out DataTable dtChName, DataTable dtMidDetail, int iPara, SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            dtChName = dtPublicPD.Copy();
            SqlLib sqllib = new SqlLib();
            string[] str1 = { "GongDan No", "RM EHB", "Country of Origin" };
            DataTable dtDtl1 = sqllib.SelectDistinct(dtMidDetail, str1);
            dtDtl1.Columns.Add("Count", typeof(Int32));
            foreach (DataRow dr in dtDtl1.Rows)
            {
                string strRMEHB = dr["RM EHB"].ToString().Trim();
                string strCountry = dr["Country of Origin"].ToString().Trim();
                int iNumber = dtDtl1.Select("[RM EHB] = '" + strRMEHB + "' AND [Country of Origin] = '" + strCountry + "'").Length;
                if (iNumber > (Int32)(iPara * 3 / 4)) { dr["Count"] = 5; }
                else if (iNumber > (Int32)(iPara / 2)) { dr["Count"] = 3; }
                else { dr["Count"] = 1; }
            }
            string[] str2 = { "RM EHB", "Country of Origin", "Count" };
            DataTable dtDtl2 = sqllib.SelectDistinct(dtDtl1, str2);
            sqllib.Dispose(0);
            dtDtl1.Dispose();

            dtMidDetail.Columns.Add("RM Score", typeof(Int32));
            foreach (DataRow dr in dtMidDetail.Rows)
            {
                string strRMEHB = dr["RM EHB"].ToString().Trim();
                string strCountry = dr["Country of Origin"].ToString().Trim();
                DataRow[] row = dtDtl2.Select("[RM EHB] = '" + strRMEHB + "' AND [Country of Origin] = '" + strCountry + "'");
                if (row.Length > 0) { dr["RM Score"] = Convert.ToInt32(row[0]["Count"].ToString().Trim()); }
                else { dr["RM Score"] = 0; }
            }
            dtDtl2.Dispose();

            dtChName.Columns.Add("Total Score", typeof(Int32));
            foreach (DataRow drow in dtChName.Rows)
            {
                string strGongDanNo = drow["GongDan No"].ToString().Trim();
                drow["Total Score"] = Convert.ToInt32(dtMidDetail.Compute("SUM([RM Score])", "[GongDan No] = '" + strGongDanNo + "'"));
            }
            DataView dv = dtChName.DefaultView;
            dv.Sort = "Total Score DESC";
            dtChName = dv.ToTable();

            DataTable dtCombineRM = new DataTable();
            dtCombineRM = dtMiddle.Clone();
            string strGD = null;
            int iRow = dtChName.Rows.Count, iCount = 0;
            for (int j = 0; j < iRow; j++)
            {
                string strGongDan = dtChName.Rows[j]["GongDan No"].ToString().Trim();
                string strGroup = dtChName.Rows[j]["Group ID"].ToString().Trim();
                if (String.IsNullOrEmpty(strGroup))
                {
                    DataRow[] drow = dtMiddle.Select("[GongDan No] = '" + strGongDan + "'");
                    foreach (DataRow row in drow)
                    {
                        string strRMEHB = row["RM EHB"].ToString().Trim();
                        string strCountry = row["Country of Origin"].ToString().Trim();
                        DataRow[] rows = dtCombineRM.Select("[RM EHB] = '" + strRMEHB + "' AND [Country of Origin] = '" + strCountry + "'");
                        if (rows.Length == 0)
                        {
                            DataRow dr = dtCombineRM.NewRow();
                            dr["RM EHB"] = strRMEHB;
                            dr["Country of Origin"] = strCountry;
                            dtCombineRM.Rows.Add(dr);
                        }
                        dtCombineRM.AcceptChanges();
                    }

                    if (dtCombineRM.Rows.Count < iPara)
                    {
                        strGD += strGongDan + ";";
                        if (j == iRow - 1)
                        {
                            string strID = this.GetGroupID(sqlConn, sqlAdp);
                            strGD = strGD.Remove(strGD.Trim().Length - 1);
                            int iLen = strGD.Split(';').Length;
                            for (int k = 0; k < iLen; k++)
                            {
                                string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                                DataRow rows = dtChName.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                                rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                            }
                            iCount++;
                            strGD = String.Empty;
                            dtCombineRM.Clear();
                        }
                    }
                    else if (dtCombineRM.Rows.Count == iPara)
                    {
                        string strID = this.GetGroupID(sqlConn, sqlAdp);
                        strGD += strGongDan;
                        int iLen = strGD.Split(';').Length;
                        for (int k = 0; k < iLen; k++)
                        {
                            string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                            DataRow rows = dtChName.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                            rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                        }
                        iCount++;
                        strGD = String.Empty;
                        dtCombineRM.Clear();
                    }
                    else
                    {
                        string strID = this.GetGroupID(sqlConn, sqlAdp);
                        strGD = strGD.Remove(strGD.Trim().Length - 1);
                        int iLen = strGD.Split(';').Length;
                        for (int k = 0; k < iLen; k++)
                        {
                            string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                            DataRow rows = dtChName.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                            rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                        }
                        iCount++;
                        j--;
                        strGD = String.Empty;
                        dtCombineRM.Clear();
                    }
                }
            }
            dtCombineRM.Dispose();
        }

        private void GetGroupByThirdSort(DataTable dtMiddle, out DataTable dtThirdSort, DataTable dtChName, int iPara, SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            dtThirdSort = dtChName.Copy();
            dtThirdSort.Columns.Remove("Group ID");
            dtThirdSort.Columns.Add("Group ID", typeof(string));
            dtThirdSort.Columns["Group ID"].SetOrdinal(0);

            DataView dv = dtThirdSort.DefaultView;
            dv.Sort = "FG EHB DESC, Total Score DESC";
            dtThirdSort = dv.ToTable();

            DataTable dtCombineRM = new DataTable();
            dtCombineRM = dtMiddle.Clone();
            string strGD = null;
            int iRow = dtThirdSort.Rows.Count, iCount = 0;
            for (int j = 0; j < iRow; j++)
            {
                string strGongDan = dtThirdSort.Rows[j]["GongDan No"].ToString().Trim();
                string strGroup = dtThirdSort.Rows[j]["Group ID"].ToString().Trim();
                if (String.IsNullOrEmpty(strGroup))
                {
                    DataRow[] drow = dtMiddle.Select("[GongDan No] = '" + strGongDan + "'");
                    foreach (DataRow row in drow)
                    {
                        string strRMEHB = row["RM EHB"].ToString().Trim();
                        string strCountry = row["Country of Origin"].ToString().Trim();
                        DataRow[] rows = dtCombineRM.Select("[RM EHB] = '" + strRMEHB + "' AND [Country of Origin] = '" + strCountry + "'");
                        if (rows.Length == 0)
                        {
                            DataRow dr = dtCombineRM.NewRow();
                            dr["RM EHB"] = strRMEHB;
                            dr["Country of Origin"] = strCountry;
                            dtCombineRM.Rows.Add(dr);
                        }
                        dtCombineRM.AcceptChanges();
                    }

                    if (dtCombineRM.Rows.Count < iPara)
                    {
                        strGD += strGongDan + ";";
                        if (j == iRow - 1)
                        {
                            string strID = this.GetGroupID(sqlConn, sqlAdp);
                            strGD = strGD.Remove(strGD.Trim().Length - 1);
                            int iLen = strGD.Split(';').Length;
                            for (int k = 0; k < iLen; k++)
                            {
                                string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                                DataRow rows = dtThirdSort.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                                rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                            }
                            iCount++;
                            strGD = String.Empty;
                            dtCombineRM.Clear();
                        }
                    }
                    else if (dtCombineRM.Rows.Count == iPara)
                    {
                        string strID = this.GetGroupID(sqlConn, sqlAdp);
                        strGD += strGongDan;
                        int iLen = strGD.Split(';').Length;
                        for (int k = 0; k < iLen; k++)
                        {
                            string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                            DataRow rows = dtThirdSort.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                            rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                        }
                        iCount++;
                        strGD = String.Empty;
                        dtCombineRM.Clear();
                    }
                    else
                    {
                        string strID = this.GetGroupID(sqlConn, sqlAdp);
                        strGD = strGD.Remove(strGD.Trim().Length - 1);
                        int iLen = strGD.Split(';').Length;
                        for (int k = 0; k < iLen; k++)
                        {
                            string strGongDanNo = strGD.Split(';')[k].ToString().Trim();
                            DataRow rows = dtThirdSort.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                            rows["Group ID"] = this.GetASCGroupID(strID, iCount);
                        }
                        iCount++;
                        j--;
                        strGD = String.Empty;
                        dtCombineRM.Clear();
                    }
                }
            }
            dtCombineRM.Dispose();
        }      

        private void dgvPingDan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvPingDan.RowCount; i++) { this.dgvPingDan[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvPingDan.RowCount; i++) { this.dgvPingDan[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvPingDan.RowCount; i++)
                    {
                        if (String.Compare(this.dgvPingDan[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvPingDan[0, i].Value = true; }

                        else { this.dgvPingDan[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }
            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvPingDan_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.btnCheckDeposit.Enabled == true && this.btnDownloadDoc.Enabled == true && this.btnSaveData.Enabled == true)
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
                    { this.dgvPingDan.Columns[0].HeaderText = "Reverse"; }

                    else if (iCount == this.dgvPingDan.RowCount)
                    { this.dgvPingDan.Columns[0].HeaderText = "Cancel"; }

                    else if (iCount == 0)
                    { this.dgvPingDan.Columns[0].HeaderText = "Select"; }
                }
            }
        }

        private void dgvPingDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (MessageBox.Show("Are you sure to remove current row's data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
                int iRow = this.dgvPingDan.CurrentRow.Index;
                this.dgvPingDan.Rows.RemoveAt(iRow);
                dtPublicPD.AcceptChanges();
            }
            if (e.ColumnIndex == 2)
            {
                if (MessageBox.Show("Are you sure to update current row's data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

                int iRowIndex = this.dgvPingDan.CurrentRow.Index;
                string strGongDan = this.dgvPingDan["GongDan No", iRowIndex].Value.ToString().Trim();
                string strRemark = this.dgvPingDan["Remark", iRowIndex].Value.ToString().Trim().ToUpper();
                decimal dFgAmt = Math.Round(Convert.ToDecimal(this.dgvPingDan["GongDan Amount(USD)", iRowIndex].Value.ToString().Trim()), 2);
                decimal dFgQty = Convert.ToDecimal(this.dgvPingDan["GongDan Qty", iRowIndex].Value.ToString().Trim());
                decimal dUnitPrice = Math.Round(dFgAmt / dFgQty, 2);

                this.dgvPingDan["GongDan Amount(USD)", iRowIndex].Value = dFgAmt;
                this.dgvPingDan["Order Price", iRowIndex].Value = dUnitPrice;
                this.dgvPingDan["Remark", iRowIndex].Value = String.IsNullOrEmpty(strRemark) ? String.Empty : strRemark;
                dtPublicPD.AcceptChanges();
            }
            if (e.ColumnIndex == 4)
            {
                int iIEType = this.dgvPingDan.Columns["IE Type"].Index;
                if (this.dgvPingDan.CurrentCell.ColumnIndex == iIEType)
                {
                    FunctionDGV_IETYPE();
                    dgvDetails.Width = 119;
                    dgvDetails.Height = 158;

                    Rectangle rec = this.dgvPingDan.GetCellDisplayRectangle(3, this.dgvPingDan.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvPingDan.Columns[3].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvPingDan.Location.Y > this.dgvPingDan.Height) { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else { dgvDetails.Top = rec.Top + this.dgvPingDan.Location.Y; }
                    dgvDetails.Visible = true;
                }
            }
            if (e.ColumnIndex != 4) { dgvDetails.Visible = false; }
        }

        private void FunctionDGV_IETYPE()
        {
            dgvDetails.DataSource = dtIE;
            this.dgvPingDan.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);
        }

        private void DGV_Details_CellClick(object sender, EventArgs e)
        {
            int iIEType = this.dgvPingDan.Columns["IE Type"].Index;

            if (this.dgvPingDan.CurrentCell != null && this.dgvPingDan.CurrentCell.ColumnIndex == iIEType)
            {
                string strIEType = dgvDetails["IE Type", dgvDetails.CurrentCell.RowIndex].Value.ToString().Trim();
                this.dgvPingDan[iIEType, this.dgvPingDan.CurrentCell.RowIndex].Value = strIEType;
            }
            dgvDetails.Visible = false;
        }

        private void btnAdjustIEtype_Click(object sender, EventArgs e)
        {           
            if (this.dgvPingDan.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (this.dgvPingDan.Columns[0].HeaderText == "Select")
            { MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to adjust I/E type?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection ConnIE = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnIE.State == ConnectionState.Closed) { ConnIE.Open(); }
            SqlCommand CommIE = new SqlCommand();
            CommIE.Connection = ConnIE;
            int iRowCount = this.dgvPingDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvPingDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strIE = this.dgvPingDan["IE Type", i].Value.ToString().Trim().ToUpper();
                    if (String.Compare(strIE, "RM-D") != 0)
                    {                      
                        CommIE.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = strIE;
                        CommIE.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.dgvPingDan["GongDan No", i].Value.ToString().Trim().ToUpper();  
                        CommIE.CommandText = "UPDATE C_GongDan SET [IE Type] = @IEType, [BeiAnDan Used Qty] = 0 WHERE [GongDan No] = @GongDanNo";
                        CommIE.ExecuteNonQuery();
                        CommIE.Parameters.Clear();
                        this.dgvPingDan.Rows.RemoveAt(i);
                        iRowCount--;
                        i--;
                    }
                }
            }
            dtPublicPD.AcceptChanges();
            if (dtPublicPD.Rows.Count == 0)
            {
                this.dgvPingDan.DataSource = DBNull.Value;
                this.dgvPingDan.Columns[0].HeaderText = String.Empty;
            }
            CommIE.Dispose();
            if (ConnIE.State == ConnectionState.Open) { ConnIE.Close(); ConnIE.Dispose(); }
            MessageBox.Show("Successfully revise the related I/E type.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCheckDeposit_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            decimal dRate = Convert.ToDecimal(dtExRate.Select("[Object]='ExchangeRate:CNY'")[0][1].ToString().Trim());

            SqlConnection ConnDeposit = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnDeposit.State == ConnectionState.Closed) { ConnDeposit.Open(); }
            SqlDataAdapter AdapterDeposit = new SqlDataAdapter(@"SELECT * FROM B_Deposit", ConnDeposit);
            DataTable dtDeposite = new DataTable();
            AdapterDeposit.Fill(dtDeposite);
            AdapterDeposit.Dispose();
            ConnDeposit.Dispose();
            decimal dMinDeposit = Convert.ToDecimal(dtDeposite.Rows[0]["Min Deposit"].ToString().Trim());
            decimal dAvaiBal = Convert.ToDecimal(dtDeposite.Rows[0]["Available Balance"].ToString().Trim());
            dtDeposite.Dispose();

            foreach (DataRow dr in dtPublicPD.Rows)
            { dTotAmt += Convert.ToDecimal(dr["GongDan Amount(USD)"].ToString().Trim()) * (0.17M + 1.17M * Convert.ToDecimal(dr["Duty Rate"].ToString().Trim())); }
            dTotAmt = Math.Round(dTotAmt / dRate, 2);
            decimal dSubtract = dAvaiBal - dTotAmt;
            if (dSubtract >= dMinDeposit) { MessageBox.Show("The deposite is sufficient.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                MessageBox.Show("Don't have enough deposit.\n\nAvailable Balance is: " + dAvaiBal.ToString() + "\nCurrent FG Total Cost is: " + dTotAmt.ToString()
                + "\nThe remaining amount(" + dSubtract.ToString() + ") is less than the min deposite(" + dMinDeposit.ToString() + ").", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }       

        private void btnDownloadDoc_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            decimal dRatio = 0.0M;
            SqlConnection ConnDoc = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnDoc.State == ConnectionState.Closed) { ConnDoc.Open(); }
            SqlCommand CommDoc = new SqlCommand();
            CommDoc.Connection = ConnDoc;
            CommDoc.CommandText = @"SELECT [ObjectValue] FROM B_SysInfo WHERE [ObjectName] = 'WeightRatio'";
            dRatio = Convert.ToDecimal(CommDoc.ExecuteScalar().ToString().Trim());
            CommDoc.Dispose();
            ConnDoc.Dispose();

            Microsoft.Office.Interop.Excel.Application excel_doc = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_doc = excel_doc.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_doc = workbooks_doc.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_doc.Worksheets[1];

            //worksheet_doc.get_Range(worksheet_doc.Cells[1, 1], worksheet_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).NumberFormatLocal = "@";
            worksheet_doc.Cells[1, 1] = "出区凭单ID";
            worksheet_doc.Cells[1, 2] = "出区类型";
            worksheet_doc.Cells[1, 3] = "运抵国（地区）";
            worksheet_doc.Cells[1, 4] = "是否加封";
            worksheet_doc.Cells[1, 5] = "项号";
            worksheet_doc.Cells[1, 6] = "物料备件号";
            worksheet_doc.Cells[1, 7] = "货主/客户名称";
            worksheet_doc.Cells[1, 8] = "物料数量";
            worksheet_doc.Cells[1, 9] = "金额";
            worksheet_doc.Cells[1, 10] = "币制";
            worksheet_doc.Cells[1, 11] = "净重";
            worksheet_doc.Cells[1, 12] = "毛重";
            worksheet_doc.Cells[1, 13] = "原产地/目的地";
            worksheet_doc.Cells[1, 14] = "批次/工单号";
            worksheet_doc.Cells[1, 15] = "备案单号";

            string strGroupID = null;
            int iLineNo = 0;
            for (int x = 0; x < this.dgvPingDan.RowCount; x++)
            {
                string strGPID = this.dgvPingDan["Group ID", x].Value.ToString().Trim();
                if (String.Compare(strGroupID, strGPID) == 0) { ++iLineNo; }
                else { strGroupID = strGPID; iLineNo = 1; }

                worksheet_doc.Cells[x + 2, 1] = String.Empty;
                worksheet_doc.Cells[x + 2, 2] = "保税";
                worksheet_doc.Cells[x + 2, 3] = "中国";
                worksheet_doc.Cells[x + 2, 4] = "否";
                worksheet_doc.Cells[x + 2, 5] = iLineNo.ToString().Trim(); 
                worksheet_doc.Cells[x + 2, 6] = this.dgvPingDan["FG EHB", x].Value.ToString().Trim();
                worksheet_doc.Cells[x + 2, 7] = "沙伯基础创新塑料（上海）有限公司 ";
                worksheet_doc.Cells[x + 2, 8] = this.dgvPingDan["GongDan Qty", x].Value.ToString().Trim();
                worksheet_doc.Cells[x + 2, 9] = this.dgvPingDan["GongDan Amount(USD)", x].Value.ToString().Trim();
                worksheet_doc.Cells[x + 2, 10] = "美元";
                worksheet_doc.Cells[x + 2, 11] = this.dgvPingDan["GongDan Qty", x].Value.ToString().Trim();
                decimal dGrossWeight = Math.Round(Convert.ToDecimal(this.dgvPingDan["GongDan Qty", x].Value.ToString().Trim()) * dRatio, 2);
                worksheet_doc.Cells[x + 2, 12] = dGrossWeight.ToString().Trim();
                worksheet_doc.Cells[x + 2, 13] = "中国";
                worksheet_doc.Cells[x + 2, 14] = this.dgvPingDan["GongDan No", x].Value.ToString().Trim();
                worksheet_doc.Cells[x + 2, 15] = String.Empty;
            }
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[1, 15]).Font.Bold = true;
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Name = "Verdana";
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Size = 9;
            worksheet_doc.Cells.EntireColumn.AutoFit();
            excel_doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            object missing = System.Reflection.Missing.Value;
            worksheet_doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_doc.Worksheets.Add(missing, missing, 1, missing);
            //worksheet_doc.get_Range(worksheet_doc.Cells[1, 1], worksheet_doc.Cells[this.dgvPingDan.RowCount + 1, 9]).NumberFormatLocal = "@";
            worksheet_doc.Cells[1, 1] = "企业内部编号";
            worksheet_doc.Cells[1, 2] = "出库单号";
            worksheet_doc.Cells[1, 3] = "原始货物备件号";
            worksheet_doc.Cells[1, 4] = "数量";
            worksheet_doc.Cells[1, 5] = "净重";
            worksheet_doc.Cells[1, 6] = "毛重";
            worksheet_doc.Cells[1, 7] = "金额";
            worksheet_doc.Cells[1, 8] = "币制";
            worksheet_doc.Cells[1, 9] = "原产国";

            for (int y = 0; y < this.dgvPingDan.RowCount; y++)
            {
                worksheet_doc.Cells[y + 2, 1] = this.dgvPingDan["Group ID", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 2] = "'" + this.dgvPingDan["BeiAnDan ID", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 3] = "'" + this.dgvPingDan["FG EHB", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 4] = this.dgvPingDan["GongDan Qty", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 5] = this.dgvPingDan["GongDan Qty", y].Value.ToString().Trim();
                decimal dGrossWeight = Math.Round(Convert.ToDecimal(this.dgvPingDan["GongDan Qty", y].Value.ToString().Trim()) * dRatio, 2);
                worksheet_doc.Cells[y + 2, 6] = dGrossWeight.ToString().Trim();
                worksheet_doc.Cells[y + 2, 7] = this.dgvPingDan["GongDan Amount(USD)", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 8] = "502";
                worksheet_doc.Cells[y + 2, 9] = "142";
            }
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[1, 9]).Font.Bold = true;
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Name = "Verdana";
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Size = 9;
            worksheet_doc.Cells.EntireColumn.AutoFit();
            excel_doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            excel_doc.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_doc);
            worksheet_doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook_doc);
            workbook_doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_doc);
            excel_doc = null;
            GC.Collect();
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (dTotAmt == 0.0M) { MessageBox.Show("Please check the deposit first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            DataTable dtCopy = dtPublicPD.Copy();
            DataRow[] drow = dtCopy.Select("[Remark] = 'FG On Hand Inventory'");
            if (drow.Length > 0)
            {
                foreach (DataRow dr in drow) { dr["Group ID"] = dr["Group ID"].ToString().Trim() + "-O"; }
                dtCopy.AcceptChanges();
            }
            SqlConnection SaveConnPD = new SqlConnection(SqlLib.StrSqlConnection);
            if (SaveConnPD.State == ConnectionState.Closed) { SaveConnPD.Open(); }
            SqlCommand SaveCommPD = new SqlCommand();
            SaveCommPD.Connection = SaveConnPD;
            SaveCommPD.CommandType = CommandType.StoredProcedure;
            SaveCommPD.CommandText = @"usp_InsertPingDanForRMD";
            SaveCommPD.Parameters.AddWithValue("@Creater", loginFrm.PublicUserName.ToUpper());
            SaveCommPD.Parameters.AddWithValue("@PingDanDate", System.DateTime.Now);
            SaveCommPD.Parameters.AddWithValue("@tvp_PingDanRMD", dtCopy);
            SaveCommPD.Parameters.AddWithValue("@TotAmt", dTotAmt);
            SaveCommPD.ExecuteNonQuery();
            SaveCommPD.Parameters.Clear();
            SaveCommPD.Dispose();
            SaveConnPD.Dispose();
            dtCopy.Dispose();
            MessageBox.Show("Successfully save the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }        

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strBrowse = null;
            if (this.cbGateOutTime.Checked == true) { strBrowse += " AND (C.[Gate Out Time] IS NULL OR C.[Gate Out Time] = '')"; }
            if (String.IsNullOrEmpty(this.txtGongDanNo.Text.Trim()))
            {              
                if (!String.IsNullOrEmpty(this.dtpPDTo.Text.Trim()))
                {
                    if (!String.IsNullOrEmpty(this.dtpPDFrom.Text.Trim()))
                    {
                        if (DateTime.Compare(Convert.ToDateTime(this.dtpPDFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpPDTo.Value.ToString("M/d/yyyy"))) == 1)
                        {
                            MessageBox.Show("Start date should be not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.dtpPDFrom.Focus();
                            return;
                        }
                        else
                        { strBrowse += " AND C.[PingDan Date] >= '" + Convert.ToDateTime(this.dtpPDFrom.Value.ToString("M/d/yyyy")) + "' AND C.[PingDan Date] < '" + Convert.ToDateTime(this.dtpPDTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                    }
                    else
                    { strBrowse += " AND C.[PingDan Date] < '" + Convert.ToDateTime(this.dtpPDTo.Value.AddDays(1.0).ToString("M/d/yyyy")) + "'"; }
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.dtpPDFrom.Text.Trim()))
                    { strBrowse += " AND C.[PingDan Date] >= '" + Convert.ToDateTime(this.dtpPDFrom.Value.ToString("M/d/yyyy")) + "'"; }
                }
            }
            else { strBrowse += " AND C.[GongDan No] = '" + this.txtGongDanNo.Text.Trim() + "'"; }

            strFilter = "";
            dvPublicPD.RowFilter = "";
            string strSQL = "SELECT C.*, B.[Duty Rate] FROM C_PingDan AS C LEFT JOIN (SELECT DISTINCT [FG CHN Name], [Duty Rate] FROM B_HsCode) AS B ON " + 
                            "C.[FG CHN Name] = B.[FG CHN Name] WHERE C.[IE Type] = 'RM-D'" + strBrowse + " ORDER BY C.[Group ID], C.[GongDan No]";

            SqlConnection ConnRMD = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnRMD.State == ConnectionState.Closed) { ConnRMD.Open(); }
            SqlDataAdapter AdapterRMD = new SqlDataAdapter(strSQL, ConnRMD);
            dtPublicPD.Columns.Clear();
            dtPublicPD.Rows.Clear();
            AdapterRMD.Fill(dtPublicPD);
            AdapterRMD.Dispose();
            ConnRMD.Dispose();

            if (dtPublicPD.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvPingDan.DataSource = DBNull.Value;
            }
            else
            {
                dvPublicPD = dtPublicPD.DefaultView;
                this.dgvPingDan.DataSource = dvPublicPD;
                for (int i = 3; i < this.dgvPingDan.ColumnCount; i++)
                { this.dgvPingDan.Columns[i].ReadOnly = true; }
                this.dgvPingDan.Columns[0].Visible = false;
                this.dgvPingDan.Columns[1].Visible = false;
                this.dgvPingDan.Columns[2].Visible = false;
                this.dgvPingDan.Columns["IE Type"].Visible = false;
                this.dgvPingDan.Columns["FG No"].Visible = false;
                this.dgvPingDan.Columns["BeiAnDan ID"].Visible = false;              
                this.dgvPingDan.Columns["Destination"].Visible = false;
                this.dgvPingDan.Columns["Duty Rate"].Visible = false;
                this.dgvPingDan.Columns["GongDan No"].Frozen = true;
                this.btnAdjustIEtype.Enabled = false;
                this.btnCheckDeposit.Enabled = false;
                this.btnDownloadDoc.Enabled = false;
                this.btnSaveData.Enabled = false;
            }
        }

        private void llblMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("When upload to batch update the data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                            "\n\tGongDan No, \n\tFG No, \n\tPingDan ID, \n\tPingDan No, \n\tGate Out Time.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtSearchPath.Text = openDlg.FileName;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string strFilePath = this.txtSearchPath.Text.Trim();
            if (String.IsNullOrEmpty(strFilePath))
            { MessageBox.Show("Please select the uploading path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("Are you sure to batch update the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No) { return; }

            bool bJudge = strFilePath.Contains(".xlsx");
            string strConn;
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + "; Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection eConn = new OleDbConnection(strConn);
            eConn.Open();
            OleDbDataAdapter eAdapter = new OleDbDataAdapter("SELECT [GongDan No], [FG No], [PingDan ID], [PingDan No], [Gate Out Time] FROM [Sheet1$] WHERE [GongDan No] IS NOT NULL AND [GongDan No] <> ''", eConn);
            DataTable dtUploadPD = new DataTable();
            dtUploadPD.Columns.Add("GongDan No", typeof(string));
            dtUploadPD.Columns.Add("FG No", typeof(string));
            dtUploadPD.Columns.Add("PingDan ID", typeof(string));
            dtUploadPD.Columns.Add("PingDan No", typeof(string));
            dtUploadPD.Columns.Add("Gate Out Time", typeof(string));
            eAdapter.Fill(dtUploadPD);
            eAdapter.Dispose();
            eConn.Close();
            eConn.Dispose();
            if (dtUploadPD.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtUploadPD.Dispose();
                return;
            }

            SqlConnection pdConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (pdConn.State == ConnectionState.Closed) { pdConn.Open(); }
            SqlCommand pdComm = new SqlCommand();
            pdComm.Connection = pdConn;
            pdComm.CommandText = "SELECT * FROM B_MultiUser";
            string strUserName = Convert.ToString(pdComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait for him/her to finish the process.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pdConn.Dispose();
                    pdComm.Dispose();
                    return;
                }
            }
            else
            {
                pdComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                pdComm.CommandText = "INSERT INTO B_MultiUser([UserName]) VALUES(@UserName)";
                pdComm.ExecuteNonQuery();
                pdComm.Parameters.RemoveAt("@UserName");
            }

            DataTable dtCopyPD = dtUploadPD.Copy();
            dtUploadPD.Columns.Remove("FG No");
            pdComm.CommandType = CommandType.StoredProcedure;
            pdComm.CommandText = @"usp_UpdatePingDanForRMD";
            pdComm.Parameters.AddWithValue("@tvp_PD", dtUploadPD);
            SqlDataAdapter pdAdp = new SqlDataAdapter();
            pdAdp.SelectCommand = pdComm;
            DataTable dtPDRMD = new DataTable();
            pdAdp.Fill(dtPDRMD);
            pdAdp.Dispose();
            pdComm.Parameters.Clear();
            dtUploadPD.Dispose();
            if (dtPDRMD.Rows.Count > 0)
            {
                dtPDRMD.Columns.Add("FG No", typeof(string));
                foreach (DataRow drow in dtPDRMD.Rows)
                {
                    DataRow dr = dtCopyPD.Select("[GongDan No]='" + drow[0].ToString().Trim() + "'")[0];
                    drow[1] = dr["FG No"].ToString().Trim();
                }
                dtPDRMD.AcceptChanges();
            }
            dtCopyPD.Dispose();

            if (dtPDRMD.Rows.Count > 0)
            {
                string strGdList = null;
                foreach (DataRow dr in dtPDRMD.Rows) { strGdList += "'" + dr[0].ToString().Trim() + "',"; }
                strGdList = strGdList.Remove(strGdList.Length - 1);
                this.UpdateRMDroolsBalance(strGdList, pdComm);
            }
            dtPDRMD.Dispose();
            pdComm.CommandType = CommandType.Text;
            pdComm.CommandText = "DELETE FROM B_MultiUser";
            pdComm.ExecuteNonQuery();
            pdComm.Dispose();
            if (pdConn.State == ConnectionState.Open) { pdConn.Close(); pdConn.Dispose(); }
            MessageBox.Show("Update data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateRMDroolsBalance(string strGdList, SqlCommand pdComm)
        {
            pdComm.CommandText = @"usp_UpdateRMBalanceByPingDan";
            pdComm.Parameters.AddWithValue("@GongDanList", strGdList);
            pdComm.ExecuteNonQuery();
            pdComm.Parameters.Clear();
            pdComm.CommandText = @"usp_UpdateDroolsBalanceByPingDan";
            pdComm.Parameters.AddWithValue("@GongDanList", strGdList);
            pdComm.ExecuteNonQuery();
            pdComm.Parameters.Clear();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel_DL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_DL = excel_DL.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_DL = workbooks_DL.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_DL = (Microsoft.Office.Interop.Excel.Worksheet)workbook_DL.Worksheets[1];

            int iRowNo = dvPublicPD.ToTable().Rows.Count;
            int iColNo = dvPublicPD.ToTable().Columns.Count;
            //worksheet_DL.get_Range(worksheet_DL.Cells[1, 1], worksheet_DL.Cells[iRowNo + 1, iColNo - 1]).NumberFormatLocal = "@";
            for (int i = 0; i < iColNo - 1; i++) { worksheet_DL.Cells[1, i + 1] = dvPublicPD.ToTable().Columns[i].ColumnName.ToString(); }
            for (int j = 0; j < iRowNo; j++)
            {
                for (int k = 0; k < iColNo - 1; k++)
                { worksheet_DL.Cells[j + 2, k + 1] = "'" + dvPublicPD.ToTable().Rows[j][k].ToString().Trim(); }
            }

            //worksheet_DL.get_Range(excel_DL.Cells[1, 1], excel_DL.Cells[1, iColNo - 1]).Font.Bold = true;
            //worksheet_DL.get_Range(excel_DL.Cells[1, 1], excel_DL.Cells[iRowNo + 1, iColNo - 1]).Font.Name = "Verdana";
            //worksheet_DL.get_Range(excel_DL.Cells[1, 1], excel_DL.Cells[iRowNo + 1, iColNo - 1]).Font.Size = 9;
            worksheet_DL.Cells.EntireColumn.AutoFit();
            excel_DL.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            excel_DL.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_DL);
            worksheet_DL = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook_DL);
            workbook_DL = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_DL);
            excel_DL = null;
            GC.Collect();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.Rows.Count == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            string strGongDanNo = this.txtGongDanNo.Text.Trim().ToUpper();
            if (String.IsNullOrEmpty(strGongDanNo)) { MessageBox.Show("Please input the GongDan No first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            DialogResult dlgR = MessageBox.Show("Are you sure to delete the data?\n[Yes] Save PingDan ID and PingDan No before delete data;\n[No] DO NOT save history data to delete directly;\n[Cancel] Reject to handle.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dlgR == DialogResult.Cancel) { return; }

            string strGateOutTime = dtPublicPD.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["Gate Out Time"].ToString().Trim();
            SqlConnection pdDelConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (pdDelConn.State == ConnectionState.Closed) { pdDelConn.Open(); }
            SqlCommand pdDelComm = new SqlCommand();
            pdDelComm.Connection = pdDelConn;
            if (!String.IsNullOrEmpty(strGateOutTime))
            {
                MessageBox.Show("The PingDan already 2nd released, system rejects to delete the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                pdDelComm.Dispose();
                pdDelConn.Dispose();
                return;
            }
           
            decimal dGdAmt = Convert.ToDecimal(dtPublicPD.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["PingDan Amount"].ToString().Trim());
            decimal dDrate = Convert.ToDecimal(dtPublicPD.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["Duty Rate"].ToString().Trim());
            decimal dRate = Convert.ToDecimal(dtExRate.Select("[Object] = 'ExchangeRate:CNY'")[0][1].ToString().Trim());
            decimal dTotAmt = Math.Round(dGdAmt * (0.17M + 1.17M * dDrate) / dRate, 2);
            if (dlgR == DialogResult.Yes)
            {
                string strPingDanID = dtPublicPD.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["PingDan ID"].ToString().Trim();
                string strPingDanNo = dtPublicPD.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["PingDan No"].ToString().Trim();
                pdDelComm.CommandText = @"DELETE FROM M_PendingPingDan_RMD WHERE [GongDan No] = '" + strGongDanNo + "'";
                pdDelComm.ExecuteNonQuery();
                pdDelComm.CommandText = @"INSERT INTO M_PendingPingDan_RMD([GongDan No], [PingDan ID], [PingDan No]) VALUES('" + strGongDanNo + "', '" + strPingDanID + "', '" + strPingDanNo + "')";
                pdDelComm.ExecuteNonQuery();
            }
            pdDelComm.CommandType = CommandType.StoredProcedure;
            pdDelComm.CommandText = @"usp_DeleteHistoryPingDan";
            pdDelComm.Parameters.AddWithValue("@GroupID", strGongDanNo);
            pdDelComm.Parameters.AddWithValue("@IEtype", "RM-D");
            pdDelComm.Parameters.AddWithValue("@TotAmt", dTotAmt);
            pdDelComm.ExecuteNonQuery();
            pdDelComm.Parameters.Clear();
            pdDelComm.Dispose();
            if (pdDelConn.State == ConnectionState.Open) { pdDelConn.Close(); pdDelConn.Dispose(); }

            DataRow[] drow = dtPublicPD.Select("[GongDan No] = '" + strGongDanNo + "'");
            foreach (DataRow dr in drow) { dr.Delete(); }
            dtPublicPD.AcceptChanges();
            this.txtGongDanNo.Text = string.Empty;
            MessageBox.Show("Successfully delete the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                dvPublicPD.RowFilter = strFilter;
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
                dvPublicPD.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvPublicPD.RowFilter = "";
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
                filterFrm.cmbFilterContent.DataSource = dvPublicPD.ToTable(true, new string[] { strColumnName });
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
                dvPublicPD.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxShow.Visible == false) { this.gBoxShow.Visible = true; }
            else { this.gBoxShow.Visible = false; }
        }

        private void btnSearchFGT_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtFGT.Text = openDlg.FileName;
        }

        private void btnUploadFGT_Click(object sender, EventArgs e)
        {
            string strFilePath = this.txtFGT.Text.Trim();
            if (String.IsNullOrEmpty(strFilePath)) { MessageBox.Show("Please find out the uploading file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("Are you sure to batch upload GongDan No?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No) { return; }

            bool bJudge = strFilePath.Contains(".xlsx");
            string strConn;
            if (bJudge) { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + "; Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection eCon = new OleDbConnection(strConn);
            eCon.Open();
            OleDbDataAdapter eAdp = new OleDbDataAdapter("SELECT DISTINCT [GongDan No] FROM [Sheet1$] WHERE [GongDan No] IS NOT NULL AND [GongDan No] <> ''", eCon);
            DataTable eTbl = new DataTable();
            eAdp.Fill(eTbl);
            eAdp.Dispose();
            eCon.Dispose();
            if (eTbl.Rows.Count == 0)
            {
                MessageBox.Show("There is no GongDan to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                eTbl.Dispose();
                return;
            }

            SqlConnection fgtConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (fgtConn.State == ConnectionState.Closed) { fgtConn.Open(); }
            SqlCommand fgtComm = new SqlCommand();
            fgtComm.Connection = fgtConn;
            fgtComm.CommandType = CommandType.StoredProcedure;
            fgtComm.CommandText = @"usp_InsertGongDan_FGT";
            fgtComm.Parameters.AddWithValue("@tvp_FGT", eTbl);
            fgtComm.ExecuteNonQuery();
            fgtComm.Parameters.Clear();
            fgtComm.Dispose();
            fgtConn.Close();
            fgtConn.Dispose();
            eTbl.Dispose();
            MessageBox.Show("Upload GongDan successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDownloadFGT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to download GongDan information?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection ConnFGT = new SqlConnection(SqlLib.StrSqlConnection);
            if (ConnFGT.State == ConnectionState.Closed) { ConnFGT.Open(); }
            SqlDataAdapter AdapterFGT = new SqlDataAdapter("SELECT DISTINCT [GongDan No], [GongDan Qty] FROM C_GongDan WHERE [IE Type] = 'RM-D' AND ([BeiAnDan Used Qty] = 0.0)", ConnFGT);
            DataTable dtFGT = new DataTable();
            AdapterFGT.Fill(dtFGT);
            AdapterFGT.Dispose();
            ConnFGT.Dispose();
            if (dtFGT.Rows.Count == 0)
            {
                MessageBox.Show("There is no GongDan information.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtFGT.Dispose();
                return;
            }

            Microsoft.Office.Interop.Excel.Application excel_doc = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_doc = excel_doc.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_doc = workbooks_doc.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_doc.Worksheets[1];

            //worksheet_doc.get_Range(worksheet_doc.Cells[1, 1], worksheet_doc.Cells[dtFGT.Rows.Count + 1, 2]).NumberFormatLocal = "@";
            worksheet_doc.Cells[1, 1] = "GongDan No";
            worksheet_doc.Cells[1, 2] = "GongDan Qty";
            for (int i = 0; i < dtFGT.Rows.Count; i++)
            {
                worksheet_doc.Cells[i + 2, 1] = dtFGT.Rows[i][0].ToString().Trim();
                worksheet_doc.Cells[i + 2, 2] = dtFGT.Rows[i][1].ToString().Trim();
            }
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[1, 15]).Font.Bold = true;
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Name = "Verdana";
            //worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Size = 9;
            worksheet_doc.Cells.EntireColumn.AutoFit();
            excel_doc.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;

            excel_doc.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_doc);
            worksheet_doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook_doc);
            workbook_doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_doc);
            excel_doc = null;
            GC.Collect();
        }
    }
}
