using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class GetPingDanDataForm_RM_D : Form
    {
        DataTable dt = new DataTable();
        DataTable dtbl = new DataTable();
        DataTable dtable = new DataTable();
        DataTable dtGongDanNo = new DataTable();
        string strFilter = null;
        private LoginForm loginFrm = new LoginForm();              
        private DataGridView dgvDetails = new DataGridView();
        protected DataView dvFillDGV = new DataView();
        protected PopUpFilterForm filterFrm = null;

        private static GetPingDanDataForm_RM_D PingDanDataFrm;
        public GetPingDanDataForm_RM_D()
        {
            InitializeComponent();
        }
        public static GetPingDanDataForm_RM_D CreateInstance()
        {
            if (PingDanDataFrm == null || PingDanDataFrm.IsDisposed)
            {
                PingDanDataFrm = new GetPingDanDataForm_RM_D();
            }
            return PingDanDataFrm;
        }

        private void GetPingDanDataForm_RM_D_Load(object sender, EventArgs e)
        {
            this.dtpPDFrom.CustomFormat = " ";
            this.dtpPDTo.CustomFormat = " ";

            this.btnAmtCheck.Enabled = false;
            this.btnDutyCheck.Enabled = false;
            this.btnDownloadDoc.Enabled = false;
            this.btnSaveData.Enabled = false;

            this.btnDelete.Enabled = false;
            this.dgvPingDan.Columns[0].Visible = false;
            this.dgvPingDan.Columns[1].Visible = false;

            SqlLib sqlLib = new SqlLib();
            dt = sqlLib.GetDataTable(@"SELECT * FROM B_IEType WHERE [IE Type] <> 'RM-D'", "B_IEType").Copy();
            dtbl = sqlLib.GetDataTable(@"SELECT [Object], [Rate] FROM B_ExchangeRate", "B_ExchangeRate").Copy();
            sqlLib.Dispose(0);
        }

        private void GetPingDanDataForm_RM_D_FormClosing(object sender, FormClosingEventArgs e)
        {
            dt.Dispose();
            dtbl.Dispose();
            dtable.Dispose();
            dtGongDanNo.Dispose();
        }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;

            sqlComm.Parameters.Clear();
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_GatherDailyPingDanForRM-D";
            sqlComm.Parameters.AddWithValue("@Obj", "A");
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            dtable.Rows.Clear();
            dtable.Columns.Clear();
            sqlAdapter.Fill(dtable);
            sqlAdapter.Dispose();

            if (dtable.Rows.Count == 0)
            {
                this.dgvPingDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlComm.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }
            else
            {              
                foreach (DataRow dr in dtable.Rows)
                {
                    decimal dAmount = 0.0M;
                    decimal dGDQty = Convert.ToDecimal(dr["GongDan Qty"].ToString().Trim());
                    if (String.Compare(dr["Order Currency"].ToString().Trim(), "USD") != 0)
                    {
                        string strObject = dr["Order Currency"].ToString().Trim() + ":" + "USD";
                        DataRow[] drRate = dtbl.Select("[Object] = '" + strObject + "'");
                        if (drRate.Length == 0) { dAmount = 0.0M; }
                        else
                        {
                            decimal dPrice = Math.Round(Convert.ToDecimal(dr["Order Price"].ToString()) * Convert.ToDecimal(drRate[0][1].ToString().Trim()), 4);
                            dAmount = Math.Round(dPrice * dGDQty, 2);
                        }
                    }
                    else { dAmount = Math.Round(Convert.ToDecimal(dr["Order Price"].ToString().Trim()) * dGDQty, 2); }
                    dr["GongDan Amount(USD)"] = dAmount;
                }
                dtable.AcceptChanges();

                sqlComm.Parameters.Clear();
                sqlComm.CommandText = @"usp_GatherDailyPingDanForRM-D";
                sqlComm.Parameters.AddWithValue("@Obj", "B");
                SqlDataAdapter SqlAdp = new SqlDataAdapter();
                SqlAdp.SelectCommand = sqlComm;
                DataTable dtMiddle = new DataTable();
                SqlAdp.Fill(dtMiddle);

                sqlComm.Parameters.Clear();
                sqlComm.CommandText = @"usp_GatherDailyPingDanForRM-D";
                sqlComm.Parameters.AddWithValue("@Obj", "C");
                SqlAdp.SelectCommand = sqlComm;
                DataTable dtMidDetail = new DataTable();
                SqlAdp.Fill(dtMidDetail);
                dtMidDetail.Columns.Remove("Item No");
                dtMidDetail.Columns.Remove("Lot No");

                sqlComm.Parameters.Clear();
                sqlComm.CommandType = CommandType.Text;
                sqlComm.CommandText = @"SELECT * FROM B_TaxParameter";
                int iPara = Convert.ToInt32(sqlComm.ExecuteScalar());
                string strGroupId = this.GetGroupID(sqlConn, SqlAdp), strMessage = null;
                int iSerial = 0;
                foreach (DataRow dr in dtable.Rows)
                {
                    DataRow[] drow = dtMiddle.Select("[GongDan No] = '" + dr["GongDan No"].ToString().Trim() + "'");
                    if (drow.Length > iPara)
                    {
                        dr["Group ID"] = this.GetASCGroupID(strGroupId, iSerial);
                        iSerial++;
                        foreach (DataRow row in drow) { dtMiddle.Rows.Remove(row); }
                        dtMiddle.AcceptChanges();
                        strMessage += dr["GongDan No"].ToString().Trim() + "\n";
                    }
                }
                dtable.AcceptChanges();

                if (dtMiddle.Rows.Count > 0)
                {
                    DataTable dtScore = new DataTable();
                    this.GetGroupByGongDanScore(dtMiddle, out dtScore, iPara, sqlConn, SqlAdp);
                    DataTable dtChName = new DataTable();
                    this.GetGroupByChineseName(dtMiddle, out dtChName, dtMidDetail, iPara, sqlConn, SqlAdp);
                    DataTable dtThirdSort = new DataTable();
                    this.GetGroupByThirdSort(dtMiddle, out dtThirdSort, dtChName, iPara, sqlConn, SqlAdp);
                    dtChName.Columns.Remove("Total Score");
                    dtThirdSort.Columns.Remove("Total Score");

                    string[] str = { "Group ID" };
                    SqlLib SQLLib = new SqlLib();
                    DataTable dataTbl1 = SQLLib.SelectDistinct(dtScore, str);
                    int iScore = dataTbl1.Rows.Count;
                    dataTbl1.Dispose();
                    DataTable dataTbl2 = SQLLib.SelectDistinct(dtChName, str);
                    int iChName = dataTbl2.Rows.Count;
                    dataTbl2.Dispose();
                    DataTable dataTbl3 = SQLLib.SelectDistinct(dtThirdSort, str);
                    int iThirdSort = dataTbl3.Rows.Count;
                    dataTbl3.Dispose();
                    SQLLib.Dispose(0);

                    dtable.Reset();
                    if (iScore < iChName)
                    {
                        if (iScore < iThirdSort) { dtable = dtScore.Copy(); }
                        else { dtable = dtThirdSort.Copy(); }
                    }
                    else
                    {
                        if (iChName > iThirdSort) { dtable = dtThirdSort.Copy(); }
                        else { dtable = dtChName.Copy(); }
                    }
                    dtScore.Dispose();
                    dtChName.Dispose();
                    dtThirdSort.Dispose();
                }

                dtMiddle.Dispose();
                dtMidDetail.Dispose();
                SqlAdp.Dispose();               
                sqlComm.Parameters.Clear();
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }

                DataView dview = dtable.DefaultView;
                dview.Sort = "Group ID ASC, GongDan No ASC";
                dtable = dview.ToTable();

                dvFillDGV = dtable.DefaultView;
                this.dgvPingDan.DataSource = dvFillDGV;
                this.dgvPingDan.Columns[0].HeaderText = "全选";
                for (int i = 2; i < this.dgvPingDan.ColumnCount; i++) { this.dgvPingDan.Columns[i].ReadOnly = true; }
                this.dgvPingDan.Columns["IE Type"].ReadOnly = false;
                this.dgvPingDan.Columns["GongDan Amount(USD)"].ReadOnly = false;
                this.dgvPingDan.Columns["Remark"].ReadOnly = false;
                this.dgvPingDan.Columns[0].Visible = true;
                this.dgvPingDan.Columns[1].Visible = true;               
                this.dgvPingDan.Columns["BeiAnDan ID"].Visible = false;
                this.dgvPingDan.Columns["Duty Rate"].Visible = false;
                this.dgvPingDan.Columns["GongDan No"].Frozen = true;

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvPingDan.EnableHeadersVisualStyles = false;
                this.dgvPingDan.Columns["IE Type"].HeaderCell.Style = cellStyle;
                this.dgvPingDan.Columns["GongDan Amount(USD)"].HeaderCell.Style = cellStyle;
                this.dgvPingDan.Columns["Remark"].HeaderCell.Style = cellStyle;

                if (!String.IsNullOrEmpty(strMessage))
                { MessageBox.Show("The number of bonded material more than " + iPara.ToString().Trim() + " for below GongDan:\n" + strMessage, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                this.btnAmtCheck.Enabled = true;
                this.btnDutyCheck.Enabled = true;
                this.btnDownloadDoc.Enabled = true;
                this.btnSaveData.Enabled = true;
            }
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

            string strReturn = strPrefix + strSuffix;
            return strReturn;
        }

        private void GetGroupByGongDanScore(DataTable dtMiddle, out DataTable dtScore, int iPara, SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            dtScore = dtable.Copy();
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
                        string strRMCustomsCode = row["RM Customs Code"].ToString().Trim();
                        string strOriginalCountry = row["Original Country"].ToString().Trim();
                        DataRow[] rows = dtCombineRM.Select("[RM Customs Code] = '" + strRMCustomsCode + "' AND [Original Country] = '" + strOriginalCountry + "'");
                        if (rows.Length == 0)
                        {
                            DataRow dr = dtCombineRM.NewRow();
                            dr["RM Customs Code"] = strRMCustomsCode;
                            dr["Original Country"] = strOriginalCountry;
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
            dtChName = dtable.Copy();
            SqlLib sqllib = new SqlLib();
            string[] str1 = { "GongDan No", "RM Customs Code", "Original Country" };
            DataTable dtDetail1 = sqllib.SelectDistinct(dtMidDetail, str1);
            dtDetail1.Columns.Add("Count", typeof(Int32));
            foreach (DataRow dr in dtDetail1.Rows)
            {
                string strRMCustomsCode = dr["RM Customs Code"].ToString().Trim();
                string strOriginalCountry = dr["Original Country"].ToString().Trim();
                int iNumber = dtDetail1.Select("[RM Customs Code] = '" + strRMCustomsCode + "' AND [Original Country] = '" + strOriginalCountry + "'").Length;
                if (iNumber > (Int32)(iPara * 3 / 4)) { dr["Count"] = 5; }
                else if (iNumber > (Int32)(iPara / 2)) { dr["Count"] = 3; }
                else { dr["Count"] = 1; }
            }

            int abc = dtDetail1.Select("[Count] is Null").Length;

            string[] str2 = { "RM Customs Code", "Original Country", "Count" };
            DataTable dtDetail2 = sqllib.SelectDistinct(dtDetail1, str2);
            sqllib.Dispose(0);
            dtDetail1.Dispose();

            

            dtMidDetail.Columns.Add("RM Score", typeof(Int32));
            foreach (DataRow dr in dtMidDetail.Rows)
            {
                string strRMCustomsCode = dr["RM Customs Code"].ToString().Trim();
                string strOriginalCountry = dr["Original Country"].ToString().Trim();
                DataRow[] row = dtDetail2.Select("[RM Customs Code] = '" + strRMCustomsCode + "' AND [Original Country] = '" + strOriginalCountry + "'");
                if (row.Length > 0)
                { dr["RM Score"] = Convert.ToInt32(row[0]["Count"].ToString().Trim()); }
                else { dr["RM Score"] = 0; }

 
            }
            dtDetail2.Dispose();



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
                        string strRMCustomsCode = row["RM Customs Code"].ToString().Trim();
                        string strOriginalCountry = row["Original Country"].ToString().Trim();
                        DataRow[] rows = dtCombineRM.Select("[RM Customs Code] = '" + strRMCustomsCode + "' AND [Original Country] = '" + strOriginalCountry + "'");
                        if (rows.Length == 0)
                        {
                            DataRow dr = dtCombineRM.NewRow();
                            dr["RM Customs Code"] = strRMCustomsCode;
                            dr["Original Country"] = strOriginalCountry;
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
                        string strRMCustomsCode = row["RM Customs Code"].ToString().Trim();
                        string strOriginalCountry = row["Original Country"].ToString().Trim();
                        DataRow[] rows = dtCombineRM.Select("[RM Customs Code] = '" + strRMCustomsCode + "' AND [Original Country] = '" + strOriginalCountry + "'");
                        if (rows.Length == 0)
                        {
                            DataRow dr = dtCombineRM.NewRow();
                            dr["RM Customs Code"] = strRMCustomsCode;
                            dr["Original Country"] = strOriginalCountry;
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

        private void btnAdjustIEtype_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to adjust IE type?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (this.dgvPingDan.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvPingDan.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_EditPingDanIeTypeForRM-D";

            int iRowCount = this.dgvPingDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvPingDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strIEtype = this.dgvPingDan["IE Type", i].Value.ToString().Trim().ToUpper();
                    if (String.Compare(strIEtype, "RM-D") != 0)
                    {
                        string strGongDanNo = this.dgvPingDan["GongDan No", i].Value.ToString().Trim().ToUpper();
                        sqlComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                        sqlComm.Parameters.AddWithValue("@IEType", strIEtype);
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.RemoveAt("@GongDanNo");
                        sqlComm.Parameters.RemoveAt("@IEType");

                        this.dgvPingDan.Rows.RemoveAt(i);
                        iRowCount--;
                        i--;
                    }
                }
            }
            dtable.AcceptChanges();

            if (dtable.Rows.Count == 0)
            {
                this.dgvPingDan.DataSource = DBNull.Value;
                this.dgvPingDan.Columns[0].HeaderText = String.Empty;
            }
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            MessageBox.Show("Successfully changed the related IE type.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRemoveData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to remove the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (this.dgvPingDan.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvPingDan.Focus();
                return;
            }
            /*if (this.dgvPingDan.Columns[0].HeaderText == "取消全选") 
            {
                MessageBox.Show("Don't allow to delete all data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvPingDan.Focus();
                return;
            }*/

            int iRowCount = this.dgvPingDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvPingDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    this.dgvPingDan.Rows.RemoveAt(i);
                    iRowCount--;
                    i--;
                }
            }
            dtable.AcceptChanges();
            MessageBox.Show("Successfully removed the related data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAmtCheck_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0) 
            { 
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
                return; 
            }
            int iCount = 0;
            for (int i = 0; i < this.dgvPingDan.RowCount; i++)
            {
                decimal dFGCost = Convert.ToDecimal(this.dgvPingDan["GongDan Amount(USD)", i].Value.ToString().Trim());
                decimal dRMCost = Convert.ToDecimal(this.dgvPingDan["Total RM Cost(USD)", i].Value.ToString().Trim());
                if (Decimal.Compare(dRMCost, dFGCost) == 1)
                {
                    iCount++;
                    DataGridViewRow dgvRow = this.dgvPingDan.Rows.SharedRow(i);
                    dgvRow.DefaultCellStyle.BackColor = Color.LightPink;
                    this.dgvPingDan[0, i].Value = true;
                }
            }

            if (iCount == 0) { MessageBox.Show("All the data is normal.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("There are " + iCount.ToString().Trim() + " abnormal data that Total RM Cost is greater than Selling Amount.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnDutyCheck_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0) 
            { 
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
                return; 
            }
            DataRow[] drRate = dtbl.Select("[Object] = 'RMB:USD'");
            if (drRate.Length == 0) 
            {
                MessageBox.Show("There is no the exchange rate for RMB vs USD.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
                return;
            }
            else
            {
                decimal dRate = Convert.ToDecimal(drRate[0][1].ToString().Trim());
                if (dRate == 0.0M) { MessageBox.Show("The exchange rate(RMB:USD) is zero, it's wrong.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

                SqlConnection sqlCon = new SqlConnection(SqlLib.StrSqlConnection);
                if (sqlCon.State == ConnectionState.Closed) { sqlCon.Open(); }
                SqlDataAdapter sqlAdp = new SqlDataAdapter(@"SELECT * FROM B_DepositScope", sqlCon);
                DataTable dtDeposite = new DataTable();
                sqlAdp.Fill(dtDeposite);
                sqlAdp.Dispose();
                sqlCon.Dispose();
                decimal dMinDeposit = Convert.ToDecimal(dtDeposite.Rows[0]["Min Deposit"].ToString().Trim());
                decimal dAvaiBal = Convert.ToDecimal(dtDeposite.Rows[0]["Available Balance"].ToString().Trim());
                dtDeposite.Dispose();

                decimal dTotAmt = 0.0M;
                foreach (DataRow dr in dtable.Rows)
                {
                    dTotAmt += Convert.ToDecimal(dr["GongDan Amount(USD)"].ToString().Trim()) * (0.17M + 1.17M * Convert.ToDecimal(dr["Duty Rate"].ToString().Trim()));
                }
                dTotAmt = Math.Round(dTotAmt / dRate, 2);
                decimal dSubtract = dAvaiBal - dTotAmt;
                if (dSubtract >= dMinDeposit) 
                { MessageBox.Show("The deposite is sufficient.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else 
                { MessageBox.Show("Don't have enough deposit.\nAvailable Balance is: " + dAvaiBal.ToString() + "\nCurrent FG Total Cost is: " + dTotAmt.ToString() 
                  + "\nSo the remaining amount(" + dSubtract.ToString() + ") is less than the min deposite scope(" + dMinDeposit.ToString() + ").", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void btnDownloadDoc_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            decimal dRatio = 0.0M;
            SqlConnection SqlCon = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlCon.State == ConnectionState.Closed) { SqlCon.Open(); }
            SqlCommand SqlCom = new SqlCommand();
            SqlCom.Connection = SqlCon;
            SqlCom.CommandText = @"SELECT [Ratio] FROM B_WeightRatio";
            dRatio = Convert.ToDecimal(SqlCom.ExecuteScalar().ToString().Trim());
            SqlCom.Dispose();

            Microsoft.Office.Interop.Excel.Application excel_doc = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_doc = excel_doc.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_doc = workbooks_doc.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_doc.Worksheets[1];

            worksheet_doc.get_Range(worksheet_doc.Cells[1, 1], worksheet_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).NumberFormatLocal = "@";
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
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[1, 15]).Font.Bold = true;
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Name = "Verdana";
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Size = 9;
            worksheet_doc.Cells.EntireColumn.AutoFit();

            object missing = System.Reflection.Missing.Value;
            worksheet_doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_doc.Worksheets.Add(missing, missing, 1, missing);
            worksheet_doc.get_Range(worksheet_doc.Cells[1, 1], worksheet_doc.Cells[this.dgvPingDan.RowCount + 1, 9]).NumberFormatLocal = "@";
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
                worksheet_doc.Cells[y + 2, 2] = this.dgvPingDan["BeiAnDan ID", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 3] = this.dgvPingDan["FG EHB", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 4] = this.dgvPingDan["GongDan Qty", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 5] = this.dgvPingDan["GongDan Qty", y].Value.ToString().Trim();
                decimal dGrossWeight = Math.Round(Convert.ToDecimal(this.dgvPingDan["GongDan Qty", y].Value.ToString().Trim()) * dRatio, 2);
                worksheet_doc.Cells[y + 2, 6] = dGrossWeight.ToString().Trim();
                worksheet_doc.Cells[y + 2, 7] = this.dgvPingDan["GongDan Amount(USD)", y].Value.ToString().Trim();
                worksheet_doc.Cells[y + 2, 8] = "502";
                worksheet_doc.Cells[y + 2, 9] = "142";
            }
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[1, 9]).Font.Bold = true;
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Name = "Verdana";
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 9]).Font.Size = 9;
            worksheet_doc.Cells.EntireColumn.AutoFit();

            excel_doc.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet_doc);
            worksheet_doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook_doc);
            workbook_doc = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_doc);
            excel_doc = null;
            GC.Collect();
            if (SqlCon.State == ConnectionState.Open)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            decimal dTotAmt = 0.0M, dRate = Convert.ToDecimal(dtbl.Select("[Object] = 'RMB:USD'")[0][1].ToString().Trim());
            foreach (DataRow dr in dtable.Rows)
            {
                dTotAmt += Convert.ToDecimal(dr["GongDan Amount(USD)"].ToString().Trim()) * (0.17M + 1.17M * Convert.ToDecimal(dr["Duty Rate"].ToString().Trim()));
            }
            dTotAmt = Math.Round(dTotAmt / dRate, 2);

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlCommand Comm = new SqlCommand();
            Comm.Connection = Conn;
            Comm.CommandType = CommandType.StoredProcedure;
            Comm.CommandText = @"usp_InsertDailyPingDanForRM-D";
            Comm.Parameters.AddWithValue("@Creater", loginFrm.PublicUserName.ToUpper());
            Comm.Parameters.AddWithValue("@PingDanDate", System.DateTime.Now);
            Comm.Parameters.AddWithValue("@TVP_InsertPingDan", dtable);
            Comm.Parameters.AddWithValue("@TotAmt", dTotAmt);
            Comm.ExecuteNonQuery();
            Comm.Parameters.Clear();
            Comm.Dispose();
            Conn.Dispose();

            MessageBox.Show("Successfully save the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dtpPDFrom_ValueChanged(object sender, EventArgs e)
        {
            this.dtpPDFrom.CustomFormat = null;
        }

        private void dtpPDFrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpPDFrom.CustomFormat = " "; }
        }

        private void dtpPDTo_ValueChanged(object sender, EventArgs e)
        {
            this.dtpPDTo.CustomFormat = null;
        }

        private void dtpPDTo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { this.dtpPDTo.CustomFormat = " "; }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strBrowse = null;
            if (this.cbPassGateTime.Checked == true) { strBrowse += " AND (C.[Pass Gate Time] IS NULL OR C.[Pass Gate Time] = '')"; }
            if (String.IsNullOrEmpty(this.cmbGongDanNo.Text.Trim()))
            {              
                if (!String.IsNullOrEmpty(this.dtpPDTo.Text.Trim()))
                {
                    if (!String.IsNullOrEmpty(this.dtpPDFrom.Text.Trim()))
                    {
                        if (DateTime.Compare(Convert.ToDateTime(this.dtpPDFrom.Value.ToString("M/d/yyyy")), Convert.ToDateTime(this.dtpPDTo.Value.ToString("M/d/yyyy"))) == 1)
                        {
                            MessageBox.Show("Begin date is not greater than end date.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            else { strBrowse += " AND C.[GongDan No] = '" + this.cmbGongDanNo.Text.Trim() + "'"; }

            strFilter = "";
            dvFillDGV.RowFilter = "";
            string strSQL = @"SELECT C.*, B.[Duty Rate] FROM C_PingDan AS C LEFT JOIN (SELECT DISTINCT [CHN Name], [Duty Rate] FROM B_HS) AS B ON " + 
                             "C.[FG CHN Name] = B.[CHN Name] WHERE C.[IE Type] = 'RM-D'" + strBrowse + " ORDER BY C.[Group ID], C.[GongDan No]";

            SqlConnection Con = new SqlConnection(SqlLib.StrSqlConnection);
            if (Con.State == ConnectionState.Closed) { Con.Open(); }
            SqlDataAdapter Adp = new SqlDataAdapter(strSQL, Con);
            dtable.Columns.Clear();
            dtable.Rows.Clear();
            Adp.Fill(dtable);
            Adp.Dispose();
            Con.Dispose();

            if (dtable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvPingDan.DataSource = DBNull.Value;
            }
            else
            {
                dvFillDGV = dtable.DefaultView;
                this.dgvPingDan.DataSource = dvFillDGV;
                for (int i = 2; i < this.dgvPingDan.ColumnCount; i++)
                { this.dgvPingDan.Columns[i].ReadOnly = true; }
                this.dgvPingDan.Columns[0].Visible = false;
                this.dgvPingDan.Columns[1].Visible = false;
                this.dgvPingDan.Columns["IE Type"].Visible = false;
                this.dgvPingDan.Columns["BeiAnDan ID"].Visible = false;              
                this.dgvPingDan.Columns["Order Category"].Visible = false;
                this.dgvPingDan.Columns["Destination"].Visible = false;
                this.dgvPingDan.Columns["GongDan Qty"].Visible = false;
                this.dgvPingDan.Columns["GongDan Amount"].Visible = false;
                this.dgvPingDan.Columns["Duty Rate"].Visible = false;
                this.dgvPingDan.Columns["ESS/LINE"].Frozen = true;

                this.btnAmtCheck.Enabled = false;
                this.btnDutyCheck.Enabled = false;
                this.btnDownloadDoc.Enabled = false;
                this.btnSaveData.Enabled = false;
            }
        }

        private void cmbGongDanNo_Enter(object sender, EventArgs e)
        {
            SqlLib SqlLib = new SqlLib();
            string[] strGongDan = { "GongDan No" };
            dtGongDanNo.Clear();
            dtGongDanNo = SqlLib.SelectDistinct(dtable, strGongDan).Copy();
            DataRow dr = dtGongDanNo.NewRow();
            dr["GongDan No"] = String.Empty;
            dtGongDanNo.Rows.InsertAt(dr, 0);
            this.cmbGongDanNo.DisplayMember = this.cmbGongDanNo.ValueMember = "GongDan No";
            this.cmbGongDanNo.DataSource = dtGongDanNo;
            SqlLib.Dispose(0);
        }

        private void cmbGongDanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbGongDanNo.Text.Trim())) { this.btnDelete.Enabled = false; }
            else { this.btnDelete.Enabled = true; }
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
            {
                MessageBox.Show("Please select the uploading path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnSearch.Focus();
                return;
            }
            if (MessageBox.Show("Are you sure to batch update the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No) { return; }

            bool bJudge = strFilePath.Contains(".xlsx");
            string strConn;
            if (bJudge)
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else
            { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + "; Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection eConn = new OleDbConnection(strConn);
            eConn.Open();
            OleDbDataAdapter eAdapter = new OleDbDataAdapter("SELECT [GongDan No], [PingDan ID], [PingDan No], [Pass Gate Time] FROM [Sheet1$] WHERE [GongDan No] IS NOT NULL AND [GongDan No] <> ''", eConn);
            DataTable eTable = new DataTable();
            eAdapter.Fill(eTable);
            eAdapter.Dispose();
            eConn.Close();
            eConn.Dispose();
            if (eTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                eTable.Clear();
                eTable.Dispose();
                return;
            }

            SqlConnection pdConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (pdConn.State == ConnectionState.Closed) { pdConn.Open(); }
            SqlCommand pdComm = new SqlCommand();
            pdComm.Connection = pdConn;

            pdComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
            string strUserName = Convert.ToString(pdComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                {
                    MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait this user to log out.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pdConn.Dispose();
                    pdComm.Dispose();
                    return;
                }
            }
            else
            {
                pdComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                pdComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                pdComm.ExecuteNonQuery();
                pdComm.Parameters.RemoveAt("@UserName");
            }

            pdComm.CommandType = CommandType.StoredProcedure;
            pdComm.CommandText = @"usp_UpdatePingDanForRM-D";
            pdComm.Parameters.AddWithValue("@TVP_PD", eTable);
            SqlDataAdapter pdAdp = new SqlDataAdapter();
            pdAdp.SelectCommand = pdComm;
            DataTable eTbl = new DataTable();
            pdAdp.Fill(eTbl);
            pdAdp.Dispose();
            pdComm.Parameters.Clear();
            eTable.Clear();
            eTable.Dispose();

            if (eTbl.Rows.Count > 0)
            {
                string strGdList = null;
                foreach (DataRow dr in eTbl.Rows) { strGdList += "'" + dr[0].ToString().Trim() + "',"; }
                strGdList = strGdList.Remove(strGdList.Length - 1);
                this.UpdateRMDroolsBalance(strGdList, pdComm);
            }
            eTbl.Clear();
            eTbl.Dispose();
            pdComm.CommandType = CommandType.Text;
            pdComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
            pdComm.ExecuteNonQuery();
            pdComm.Dispose();
            if (pdConn.State == ConnectionState.Open)
            {
                pdConn.Close();
                pdConn.Dispose();
            }
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
            if (this.dgvPingDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel_DL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_DL = excel_DL.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_DL = workbooks_DL.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_DL = (Microsoft.Office.Interop.Excel.Worksheet)workbook_DL.Worksheets[1];

            int iRowNo = dvFillDGV.ToTable().Rows.Count;
            int iColNo = dvFillDGV.ToTable().Columns.Count;
            worksheet_DL.get_Range(worksheet_DL.Cells[1, 1], worksheet_DL.Cells[iRowNo + 1, iColNo - 1]).NumberFormatLocal = "@";
            for (int i = 0; i < iColNo - 1; i++) { worksheet_DL.Cells[1, i + 1] = dvFillDGV.ToTable().Columns[i].ColumnName.ToString(); }
            for (int j = 0; j < iRowNo; j++)
            {
                for (int k = 0; k < iColNo - 1; k++)
                { worksheet_DL.Cells[j + 2, k + 1] = dvFillDGV.ToTable().Rows[j][k].ToString().Trim(); }
            }

            worksheet_DL.get_Range(excel_DL.Cells[1, 1], excel_DL.Cells[1, iColNo - 1]).Font.Bold = true;
            worksheet_DL.get_Range(excel_DL.Cells[1, 1], excel_DL.Cells[iRowNo + 1, iColNo - 1]).Font.Name = "Verdana";
            worksheet_DL.get_Range(excel_DL.Cells[1, 1], excel_DL.Cells[iRowNo + 1, iColNo - 1]).Font.Size = 9;
            worksheet_DL.Cells.EntireColumn.AutoFit();

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
            if (this.dgvPingDan.Rows.Count == 0) { return; }
            DialogResult dlgR = MessageBox.Show("Are you sure to delete the data?\n[Yes] Save PingDan ID and PingDan No before doing it;\n[No] Don't keep history data, delete directly;\n[Cancel] Reject to handle.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dlgR == DialogResult.Cancel) { return; }

            string strGongDanNo = this.cmbGongDanNo.Text.Trim().ToUpper();
            string strPassGateTime = dtable.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["Pass Gate Time"].ToString().Trim();
            SqlConnection pdDelConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (pdDelConn.State == ConnectionState.Closed) { pdDelConn.Open(); }
            SqlCommand pdDelComm = new SqlCommand();
            pdDelComm.Connection = pdDelConn;

            if (!String.IsNullOrEmpty(strPassGateTime))
            {
                MessageBox.Show("The PingDan already 2nd released, system rejects to remove the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                pdDelComm.Dispose();
                pdDelConn.Close();
                pdDelConn.Dispose();
                return;
            }
           
            decimal dGdAmt = Convert.ToDecimal(dtable.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["GongDan Amount"].ToString().Trim());
            decimal dDrate = Convert.ToDecimal(dtable.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["Duty Rate"].ToString().Trim());
            decimal dRate = Convert.ToDecimal(dtbl.Select("[Object] = 'RMB:USD'")[0][1].ToString().Trim());
            decimal dTotAmt = Math.Round(dGdAmt * (0.17M + 1.17M * dDrate) / dRate, 2);

            if (dlgR == DialogResult.Yes)
            {
                string strPingDanID = dtable.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["PingDan ID"].ToString().Trim();
                string strPingDanNo = dtable.Select("[GongDan No] = '" + strGongDanNo + "'")[0]["PingDan No"].ToString().Trim();
                pdDelComm.CommandText = @"DELETE FROM M_PendingPingDan_RMD WHERE [GongDan No] = '" + strGongDanNo + "'";
                pdDelComm.ExecuteNonQuery();
                pdDelComm.CommandText = @"INSERT INTO M_PendingPingDan_RMD([GongDan No], [PingDan ID], [PingDan No]) VALUES('" + strGongDanNo + "', '" + strPingDanID + "', '" + strPingDanNo + "')";
                pdDelComm.ExecuteNonQuery();
            }

            pdDelComm.CommandType = CommandType.StoredProcedure;
            pdDelComm.CommandText = @"usp_DeleteDataForHistoricalPingDan";
            pdDelComm.Parameters.AddWithValue("@GroupID", strGongDanNo);
            pdDelComm.Parameters.AddWithValue("@JudgeMTI", "RM-D");
            pdDelComm.Parameters.AddWithValue("@TotAmt", dTotAmt);
            pdDelComm.ExecuteNonQuery();
            pdDelComm.Parameters.Clear();
            pdDelComm.Dispose();
            if (pdDelConn.State == ConnectionState.Open)
            {
                pdDelConn.Close();
                pdDelConn.Dispose();
            }

            DataRow[] drow = dtable.Select("[GongDan No] = '" + strGongDanNo + "'");
            foreach (DataRow dr in drow) { dr.Delete(); }
            dtable.AcceptChanges();
            this.cmbGongDanNo.Text = string.Empty;
            MessageBox.Show("Delete data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (this.btnAmtCheck.Enabled == true && this.btnDutyCheck.Enabled == true && this.btnDownloadDoc.Enabled == true && this.btnSaveData.Enabled == true)
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
        }

        private void dgvPingDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (MessageBox.Show("Are you sure to update current row's data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
                
                int iRowIndex = this.dgvPingDan.CurrentRow.Index;
                string strGongDan = this.dgvPingDan["GongDan No", iRowIndex].Value.ToString().Trim();
                string strRemark = this.dgvPingDan["Remark", iRowIndex].Value.ToString().Trim().ToUpper();
                decimal dFgAmt = Math.Round(Convert.ToDecimal(this.dgvPingDan["GongDan Amount(USD)", iRowIndex].Value.ToString().Trim()), 2);
                decimal dFgQty = Convert.ToDecimal(this.dgvPingDan["GongDan Qty", iRowIndex].Value.ToString().Trim());
                decimal dUnitPrice = Math.Round(dFgAmt / dFgQty, 4);

                this.dgvPingDan["GongDan Amount(USD)", iRowIndex].Value = dFgAmt;
                this.dgvPingDan["Order Price", iRowIndex].Value = dUnitPrice;
                this.dgvPingDan["Remark", iRowIndex].Value = String.IsNullOrEmpty(strRemark) ? String.Empty : strRemark;
                dtable.AcceptChanges();
                //MessageBox.Show("Seccussfully updated the current row's data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (e.ColumnIndex == 3) 
            {
                int iIEType = this.dgvPingDan.Columns["IE Type"].Index;
                if (this.dgvPingDan.CurrentCell.ColumnIndex == iIEType)
                {
                    FunctionDGV_IETYPE();
                    dgvDetails.Width = 119;
                    dgvDetails.Height = 158;

                    Rectangle rec = this.dgvPingDan.GetCellDisplayRectangle(1, this.dgvPingDan.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvPingDan.Columns[1].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvPingDan.Location.Y > this.dgvPingDan.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else
                    { dgvDetails.Top = rec.Top + this.dgvPingDan.Location.Y; }

                    dgvDetails.Visible = true;
                }
            }

            if (e.ColumnIndex != 3) { dgvDetails.Visible = false; }
        }

        private void FunctionDGV_IETYPE()
        {
            dgvDetails.DataSource = dt;
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
            if (String.IsNullOrEmpty(strFilePath))
            {
                MessageBox.Show("Please select the uploading path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnSearch.Focus();
                return;
            }
            if (MessageBox.Show("Are you sure to batch upload GongDan No?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No) { return; }

            bool bJudge = strFilePath.Contains(".xlsx");
            string strConn;
            if (bJudge)
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else
            { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + "; Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection eCon = new OleDbConnection(strConn);
            eCon.Open();
            OleDbDataAdapter eAdp = new OleDbDataAdapter("SELECT DISTINCT [GongDan No] FROM [Sheet1$] WHERE [GongDan No] IS NOT NULL AND [GongDan No] <> ''", eCon);
            DataTable eTbl = new DataTable();
            eAdp.Fill(eTbl);
            eAdp.Dispose();
            eCon.Close();
            eCon.Dispose();
            if (eTbl.Rows.Count == 0)
            {
                MessageBox.Show("There is no GongDan to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                eTbl.Clear();
                eTbl.Dispose();
                return;
            }

            SqlConnection fgtConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (fgtConn.State == ConnectionState.Closed) { fgtConn.Open(); }
            SqlCommand fgtComm = new SqlCommand();
            fgtComm.Connection = fgtConn;
            fgtComm.CommandType = CommandType.StoredProcedure;
            fgtComm.CommandText = @"usp_InsertGongDan_FGT";
            fgtComm.Parameters.AddWithValue("@TVP_FGT", eTbl);
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
            if (MessageBox.Show("Do you want to download GongDan information?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            SqlConnection SqlCon = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlCon.State == ConnectionState.Closed) { SqlCon.Open(); }
            SqlDataAdapter SqlAdp = new SqlDataAdapter("SELECT DISTINCT [GongDan No], [GongDan Qty] FROM C_GongDan WHERE [IE Type] = 'RM-D' AND ([BeiAnDan Used Qty] = 0.0 OR [BeiAnDan Used Qty] IS NULL)", SqlCon);
            DataTable dtFGT = new DataTable();
            SqlAdp.Fill(dtFGT);
            SqlAdp.Dispose();
            SqlCon.Close();
            SqlCon.Dispose();
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

            worksheet_doc.get_Range(worksheet_doc.Cells[1, 1], worksheet_doc.Cells[dtFGT.Rows.Count + 1, 2]).NumberFormatLocal = "@";
            worksheet_doc.Cells[1, 1] = "GongDan No";
            worksheet_doc.Cells[1, 2] = "GongDan Qty";
            for (int i = 0; i < dtFGT.Rows.Count; i++)
            {
                worksheet_doc.Cells[i + 2, 1] = dtFGT.Rows[i][0].ToString().Trim();
                worksheet_doc.Cells[i + 2, 2] = dtFGT.Rows[i][1].ToString().Trim();
            }
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[1, 15]).Font.Bold = true;
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Name = "Verdana";
            worksheet_doc.get_Range(excel_doc.Cells[1, 1], excel_doc.Cells[this.dgvPingDan.RowCount + 1, 15]).Font.Size = 9;
            worksheet_doc.Cells.EntireColumn.AutoFit();

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
