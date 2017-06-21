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
    public partial class GetBeiAnDanDataForm : Form
    {
        DataTable dt = new DataTable();
        DataTable dtable = new DataTable();
        DataTable dtGroupID1 = new DataTable();
        DataTable dtGroupID2 = new DataTable();
        protected DataView dvFillDGV = new DataView();
        string strFilter = null;
        bool bPending = false;
        protected PopUpFilterForm filterFrm = null;
        private DataGridView dgvDetails = new DataGridView();  

        private static GetBeiAnDanDataForm getBeiAnDanDataFrm;
        public GetBeiAnDanDataForm()
        {
            InitializeComponent();
        }
        public static GetBeiAnDanDataForm CreateInstance()
        {
            if (getBeiAnDanDataFrm == null || getBeiAnDanDataFrm.IsDisposed)
            {
                getBeiAnDanDataFrm = new GetBeiAnDanDataForm();
            }
            return getBeiAnDanDataFrm;
        }

        private void GetBeiAnDanDataForm_Load(object sender, EventArgs e)
        {
            SqlLib sqlLib = new SqlLib();
            dt = sqlLib.GetDataTable(@"SELECT * FROM B_IEType WHERE [IE Type] <> 'RM-D'", "B_IEType").Copy();
            DataRow dr = dt.NewRow();
            dr["IE Type"] = String.Empty;
            dt.Rows.InsertAt(dr, 0);
            this.cmbIEtype.DisplayMember = this.cmbIEtype.ValueMember = "IE Type";
            this.cmbIEtype.DataSource = dt;
            sqlLib.Dispose();
        }

        private void GetBeiAnDanDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dt.Dispose();
            dtable.Dispose();
            dtGroupID1.Dispose();
            dtGroupID2.Dispose();
        }

        private void dgvBeiAnDan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++) { this.dgvBeiAnDan[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++) { this.dgvBeiAnDan[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
                    {
                        if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvBeiAnDan[0, i].Value = true; }

                        else { this.dgvBeiAnDan[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
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

                if (iCount < this.dgvBeiAnDan.RowCount && iCount > 0)
                { this.dgvBeiAnDan.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvBeiAnDan.RowCount)
                { this.dgvBeiAnDan.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvBeiAnDan.Columns[0].HeaderText = "全选"; }
            }
        }

        private void dgvBeiAnDan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2) //IE Type comboBox value
            {
                int iIEType = this.dgvBeiAnDan.Columns["IE Type"].Index;
                if (this.dgvBeiAnDan.CurrentCell.ColumnIndex == iIEType)
                {
                    FunctionDGV_IETYPE();
                    dgvDetails.Width = 119;
                    dgvDetails.Height = 180;

                    Rectangle rec = this.dgvBeiAnDan.GetCellDisplayRectangle(1, this.dgvBeiAnDan.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvBeiAnDan.Columns[1].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvBeiAnDan.Location.Y > this.dgvBeiAnDan.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else
                    { dgvDetails.Top = rec.Top + this.dgvBeiAnDan.Location.Y; }

                    dgvDetails.Visible = true; 
                }
            }

            if (e.ColumnIndex != 2) { dgvDetails.Visible = false; }
        }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbIEtype.Text.Trim()))
            {
                MessageBox.Show("Please select IE Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbIEtype.Focus();
                return;
            }

            bool bSwitch = false;
            string strIEtype = this.cmbIEtype.Text.Trim().ToUpper();
            if (String.Compare(strIEtype, "RMB") == 0)
            {
                DialogResult dlgR = MessageBox.Show("Please choose the condition to gather data:\n[Yes] : Gather data(normal order no) for RMB;\n[No] : Gather data(scrap sales order no) for RMB;", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dlgR == DialogResult.Yes) { bSwitch = true; }
            }

            bPending = false;
            strFilter = "";
            dvFillDGV.RowFilter = "";
            if (this.dtGroupID1.Rows.Count > 0) { this.dtGroupID1.Reset(); }
            if (this.dtGroupID2.Rows.Count > 0) { this.dtGroupID2.Reset(); }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            
            sqlComm.Parameters.Clear();
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_GatherDailyBeiAnDanByIEType";
            sqlComm.Parameters.AddWithValue("@IEType", strIEtype);
            sqlComm.Parameters.AddWithValue("@bSwitch", bSwitch); //bSwith (if bSwith is 0) means  it is for scrap sell with IE type RMB
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            dtable.Rows.Clear();
            dtable.Columns.Clear();
            sqlAdapter.Fill(dtable);          
            sqlAdapter.Dispose();          

            if (dtable.Rows.Count == 0)
            {
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlComm.Dispose();
                sqlConn.Close();
                sqlConn.Dispose();
            }
            else
            {
                SqlDataAdapter SqlAdp = new SqlDataAdapter(@"SELECT [Object], [Rate] FROM B_ExchangeRate", sqlConn);
                DataTable dtbl = new DataTable();
                SqlAdp.Fill(dtbl);
                SqlAdp = new SqlDataAdapter(@"SELECT * FROM B_FinancePrice", sqlConn);
                DataTable dtGrade = new DataTable();
                SqlAdp.Fill(dtGrade);                            
           
                dtable.Columns.Add("Group ID", typeof(string));
                dtable.Columns["Group ID"].SetOrdinal(0);
                dtable.Columns.Add("Selling Amount", typeof(decimal));
                dtable.Columns["Selling Amount"].SetOrdinal(10);
                dtable.Columns.Add("IE Rev Amt", typeof(decimal));

                string strGroupID = this.GetGroupID(strIEtype, sqlConn, SqlAdp);
                SqlAdp.Dispose(); 
                foreach (DataRow dr in dtable.Rows)
                {
                    decimal dAmount = 0.0M;
                    decimal dGDQty = Convert.ToDecimal(dr["GongDan Qty"].ToString().Trim());
                    if (String.Compare(dr["Currency"].ToString().Trim(), "USD") != 0)
                    {
                        string strObject = dr["Currency"].ToString().Trim() + ":" + "USD";
                        DataRow[] drRate = dtbl.Select("[Object] = '" + strObject + "'");
                        if (drRate.Length == 0) { dAmount = 0.0M; }
                        else
                        {
                            decimal dPrice = Math.Round(Convert.ToDecimal(dr["Selling Price"].ToString()) * Convert.ToDecimal(drRate[0][1].ToString().Trim()), 4);
                            dAmount = Math.Round(dPrice * dGDQty, 2);
                        }
                    }
                    else { dAmount = Math.Round(Convert.ToDecimal(dr["Selling Price"].ToString().Trim()) * dGDQty, 2); }
                    dr["Selling Amount"] = dAmount;

                    if (String.Compare(strIEtype, "BLP") != 0 && 
                        String.Compare(strIEtype, "EXPORT") != 0 && 
                        String.Compare(strIEtype, "RMB") != 0) 
                    { dr["Group ID"] = strGroupID; }                  

                    if (String.Compare(strIEtype, "RMB") == 0 && String.Compare(dr["Order Category"].ToString().Trim().ToUpper(), "MTI") == 0)
                    {// MTI order is sold to domestic, IE Rev Amt is referred to the selling price by product's grade according to finance guideline
                        DataRow[] drGrade = dtGrade.Select("[Grade] = '" + dr["FG EHB"].ToString().Trim().Split('-')[0] + "'");
                        if (drGrade.Length > 0) { dr["IE Rev Amt"] = Math.Round(Convert.ToDecimal(drGrade[0][3].ToString().Trim()) * dGDQty, 2); }
                    }
                }
                dtbl.Dispose();
                dtGrade.Dispose();

                if (String.Compare(strIEtype, "BLP") == 0 || 
                    String.Compare(strIEtype, "EXPORT") == 0)
                {
                    for (int i = 0; i < dtable.Rows.Count; i++)
                    { dtable.Rows[i]["Group ID"] = this.GetASCGroupID(strGroupID, i); }
                }

                if (String.Compare(strIEtype, "RMB") == 0)
                {
                    string strGDFRG, strGDFRGD;
                    if (bSwitch == true) //for non-scrap sales RMB
                    { 
                        strGDFRG = @"SELECT * FROM V_GongDanForRMBGroup1"; 
                        strGDFRGD = @"SELECT * FROM V_GongDanForRMBGroupDetail1";
                    }
                    else //For Scrap sales RMB
                    { 
                        strGDFRG = @"SELECT * FROM V_GongDanForRMBGroup2";
                        strGDFRGD = @"SELECT * FROM V_GongDanForRMBGroupDetail2";
                    }

                    SqlDataAdapter sqlAdp = new SqlDataAdapter(strGDFRG, sqlConn);
                    DataTable dtMiddle = new DataTable();
                    sqlAdp.Fill(dtMiddle);
                    sqlAdp = new SqlDataAdapter(strGDFRGD, sqlConn);
                    DataTable dtMidDetail = new DataTable();
                    sqlAdp.Fill(dtMidDetail);
                    dtMidDetail.Columns.Remove("Item No");
                    dtMidDetail.Columns.Remove("Lot No");

                    sqlComm.Parameters.Clear();
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.CommandText = @"SELECT * FROM B_InvoiceParameter";
                    int iPara = Convert.ToInt32(sqlComm.ExecuteScalar());
                    string strGroupId = this.GetGroupID(String.Empty, sqlConn, sqlAdp);
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
                        }                    
                    }

                    if (dtMiddle.Rows.Count > 0)
                    {
                        DataTable dtScore = new DataTable();
                        this.GetGroupByGongDanScore(dtMiddle, out dtScore, iPara, sqlConn, sqlAdp);
                        DataTable dtChName = new DataTable();
                        this.GetGroupByChineseName(dtMiddle, out dtChName, dtMidDetail, iPara, sqlConn, sqlAdp);
                        DataTable dtThirdSort = new DataTable();
                        this.GetGroupByThirdSort(dtMiddle, out dtThirdSort, dtChName, iPara, sqlConn, sqlAdp);
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
                    sqlAdp.Dispose(); 

                    DataView dview = dtable.DefaultView;
                    dview.Sort = "Group ID ASC, GongDan No ASC";
                    dtable = dview.ToTable();
                }
              
                sqlComm.Parameters.Clear();
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = @"usp_InsertDailyBeiAnDan_UpdateBeiAnDanUsedQty";
                sqlComm.Parameters.AddWithValue("@TVP_Insert_Update", dtable);
                sqlComm.ExecuteNonQuery();
                sqlComm.Parameters.Clear();

                sqlComm.CommandType = CommandType.Text;
                sqlComm.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = strIEtype;
                if (String.Compare(strIEtype, "RMB") == 0)
                {
                    if (bSwitch == true)
                    {
                        sqlComm.CommandText = @"SELECT [Group ID], [IE Type], [GongDan No], [Total Ship Qty], [GongDan Qty], [FG EHB], [ESS/LINE], [Selling Price], " +
                                               "[Currency], [Local Total RM Cost] AS [Local Total RM Cost(USD)], [Selling Amount] AS [Selling Amount(USD)], [Order No], " +
                                               "[Order Category], [CHN Name], [Destination], [Pass to IE Date], [GongDan Approved Date], [IE Rev Amt], [OF Rev Amt], " +
                                               "[Customs Total RM Cost], [IE Remark], [OF Remark] FROM M_DailyBeiAnDan WHERE [IE Type] = @IEtype AND " +
                                               "[Order No] <> 'SCRAP SALES' AND [Is Generated Doc] = 'False' ORDER BY [Group ID], [GongDan No]";
                    }
                    else
                    {
                        sqlComm.CommandText = @"SELECT [Group ID], [IE Type], [GongDan No], [Total Ship Qty], [GongDan Qty], [FG EHB], [ESS/LINE], [Selling Price], " +
                                               "[Currency], [Local Total RM Cost] AS [Local Total RM Cost(USD)], [Selling Amount] AS [Selling Amount(USD)], [Order No], " +
                                               "[Order Category], [CHN Name], [Destination], [Pass to IE Date], [GongDan Approved Date], [IE Rev Amt], [OF Rev Amt], " +
                                               "[Customs Total RM Cost], [IE Remark], [OF Remark] FROM M_DailyBeiAnDan WHERE [IE Type] = @IEtype AND " + 
                                               "[Order No] = 'SCRAP SALES' AND [Is Generated Doc] = 'False' ORDER BY [Group ID], [GongDan No]";
                    }
                }
                else
                {
                    sqlComm.CommandText = @"SELECT [Group ID], [IE Type], [GongDan No], [Total Ship Qty], [GongDan Qty], [FG EHB], [ESS/LINE], [Selling Price], " +
                                           "[Currency], [Local Total RM Cost] AS [Local Total RM Cost(USD)], [Selling Amount] AS [Selling Amount(USD)], [Order No], " +
                                           "[Order Category], [CHN Name], [Destination], [Pass to IE Date], [GongDan Approved Date], [IE Rev Amt], [OF Rev Amt], " +
                                           "[Customs Total RM Cost], [IE Remark], [OF Remark] FROM M_DailyBeiAnDan WHERE [IE Type] = @IEtype AND [Is Generated Doc] = 'False' " +
                                           "ORDER BY [Group ID], [GongDan No]";
                }
                SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                SqlAdapter.SelectCommand = sqlComm;
                dtable.Rows.Clear();
                dtable.Columns.Clear();
                SqlAdapter.Fill(dtable);
                SqlAdapter.Dispose();

                sqlComm.Parameters.Clear();
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }

                dvFillDGV = dtable.DefaultView;
                this.dgvBeiAnDan.DataSource = dvFillDGV;
                this.dgvBeiAnDan.Columns[0].HeaderText = "全选";
                for (int j = 3; j < this.dgvBeiAnDan.ColumnCount; j++) { this.dgvBeiAnDan.Columns[j].ReadOnly = true; }
                this.dgvBeiAnDan.Columns["IE Rev Amt"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["Customs Total RM Cost"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["IE Remark"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["GongDan No"].Frozen = true;
                this.dgvBeiAnDan.Columns["IE Rev Amt"].DefaultCellStyle.ForeColor = Color.Red;
                this.dgvBeiAnDan.Columns["OF Rev Amt"].DefaultCellStyle.ForeColor = Color.Red;
                this.dgvBeiAnDan.Columns["Customs Total RM Cost"].DefaultCellStyle.ForeColor = Color.Red;

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvBeiAnDan.EnableHeadersVisualStyles = false;
                this.dgvBeiAnDan.Columns["Group ID"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["IE Type"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["IE Rev Amt"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["Customs Total RM Cost"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["IE Remark"].HeaderCell.Style = cellStyle;
            }
        }

        private void GetGroupByGongDanScore(DataTable dtMiddle, out DataTable dtScore, int iPara, SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            dtScore = dtable.Copy();
            DataView dv = dtScore.DefaultView;
            dv.Sort = "CHN Name DESC, FG EHB ASC";
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
                            string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                        string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                        string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                            string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                        string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                        string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                            string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                        string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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
                        string strID = this.GetGroupID("RMB", sqlConn, sqlAdp);
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

        private void btnRemoveData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to remove the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (this.dgvBeiAnDan.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDan.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_EditDailyBeiAnDan";
            sqlComm.Parameters.AddWithValue("@Pending", bPending.ToString().Trim().ToUpper());
            sqlComm.Parameters.AddWithValue("@Action", "DELETE");
            sqlComm.Parameters.AddWithValue("@IEType", string.Empty);

            int iRowCount = this.dgvBeiAnDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strGongDanNo = this.dgvBeiAnDan["GongDan No", i].Value.ToString().Trim().ToUpper();                                      
                    sqlComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                    sqlComm.ExecuteNonQuery();
                    sqlComm.Parameters.RemoveAt("@GongDanNo");                 

                    this.dgvBeiAnDan.Rows.RemoveAt(i);
                    iRowCount--;
                    i--;                 
                }
            }
            dtable.AcceptChanges();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }

            if (dtable.Rows.Count == 0) 
            { 
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                this.dgvBeiAnDan.Columns[0].HeaderText = String.Empty;
                this.cmbIEtype.Text = String.Empty;
            }        
            MessageBox.Show("Successfully remove.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAdjustIEtype_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to adjust IE type?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (this.dgvBeiAnDan.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDan.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_EditDailyBeiAnDan";
            sqlComm.Parameters.AddWithValue("@Pending", bPending.ToString().Trim().ToUpper());
            sqlComm.Parameters.AddWithValue("@Action", "ADJUST");

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
                        sqlComm.Parameters.AddWithValue("@GongDanNo", strGongDanNo);
                        sqlComm.Parameters.AddWithValue("@IEType", strIEtype);
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.RemoveAt("@GongDanNo");
                        sqlComm.Parameters.RemoveAt("@IEType");

                        this.dgvBeiAnDan.Rows.RemoveAt(i);
                        iRowCount--;
                        i--;
                    }
                }
            }
            dtable.AcceptChanges();

            if (dtable.Rows.Count == 0) 
            { 
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                this.dgvBeiAnDan.Columns[0].HeaderText = String.Empty;
                this.cmbIEtype.Text = String.Empty;
            }
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            MessageBox.Show("Successfully Done.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCheckAmount_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            int iCount = 0;
            for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
            {
                decimal dLocalTotalRMCost = Convert.ToDecimal(this.dgvBeiAnDan["Local Total RM Cost(USD)", i].Value.ToString().Trim());
                decimal dSellingAmount = Convert.ToDecimal(this.dgvBeiAnDan["Selling Amount(USD)", i].Value.ToString().Trim());
                decimal dIERevAmt = Convert.ToDecimal(this.dgvBeiAnDan["IE Rev Amt", i].Value.ToString().Trim());
                decimal dOFRevAmt = Convert.ToDecimal(this.dgvBeiAnDan["IE Rev Amt", i].Value.ToString().Trim());
                int iJudge = 0;
                if (dOFRevAmt > 0.0M) { iJudge = Decimal.Compare(dLocalTotalRMCost, dOFRevAmt); }
                else
                {
                    if (dIERevAmt > 0.0M) { iJudge = Decimal.Compare(dLocalTotalRMCost, dIERevAmt); }
                    else { iJudge = Decimal.Compare(dLocalTotalRMCost, dSellingAmount); }
                }

                if (iJudge == 1)
                {
                    iCount++;
                    DataGridViewRow dgvRow = this.dgvBeiAnDan.Rows.SharedRow(i);
                    dgvRow.DefaultCellStyle.BackColor = Color.LightPink;
                    this.dgvBeiAnDan[0, i].Value = true;
                }
            }

            if (iCount == 0) { MessageBox.Show("All the data is normal.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("There are " + iCount.ToString().Trim() + " abnormal data that Local Total RM Cost is greater than Selling Amount.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnUpdateData_Click(object sender, EventArgs e)
        {
            if (this.dgvBeiAnDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (this.dgvBeiAnDan.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDan.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;

            if (bPending == true)
            {
                for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
                {
                    if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        string strGroupID = this.dgvBeiAnDan["Group ID", i].Value.ToString().Trim().ToUpper();
                        decimal dIERevAmt = 0.0M, dCustomsTotalRMCost = 0.0M;
                        if (!String.IsNullOrEmpty(this.dgvBeiAnDan["IE Rev Amt", i].Value.ToString().Trim()))
                        { dIERevAmt = Math.Round(Convert.ToDecimal(this.dgvBeiAnDan["IE Rev Amt", i].Value.ToString().Trim()), 2); }
                        if (!String.IsNullOrEmpty(this.dgvBeiAnDan["Customs Total RM Cost", i].Value.ToString().Trim()))
                        { dCustomsTotalRMCost = Math.Round(Convert.ToDecimal(this.dgvBeiAnDan["Customs Total RM Cost", i].Value.ToString().Trim()), 2); }
                        string strRemark = this.dgvBeiAnDan["IE Remark", i].Value.ToString().Trim().ToUpper();
                        string strGongDanNo = this.dgvBeiAnDan["GongDan No", i].Value.ToString().Trim();

                        string strHold = this.dgvBeiAnDan["Pending", i].Value.ToString().Trim();
                        bool bHold = false;
                        if (String.IsNullOrEmpty(strHold) || String.Compare(strHold.ToUpper(), "FALSE") == 0) { bHold = false; }
                        else { bHold = true; }

                        sqlComm.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = strGroupID;
                        sqlComm.Parameters.Add("@IERevAmt", SqlDbType.Decimal).Value = dIERevAmt;
                        sqlComm.Parameters.Add("@CustomsTotalRMCost", SqlDbType.Decimal).Value = dCustomsTotalRMCost;
                        sqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                        sqlComm.Parameters.Add("@Pending", SqlDbType.Bit).Value = bHold;
                        sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                        sqlComm.CommandText = @"UPDATE M_PendingBeiAnDan_Export SET [Group ID] = @GroupID, [IE Rev Amt] = @IERevAmt, [Customs Total RM Cost] = " +
                                               "@CustomsTotalRMCost, [IE Remark] = @Remark, [Pending] = @Pending WHERE [GongDan No] = @GongDanNo";
                        SqlTransaction sqlTrans = sqlConn.BeginTransaction();
                        sqlComm.Transaction = sqlTrans;
                        try
                        {
                            sqlComm.ExecuteNonQuery();
                            sqlTrans.Commit();
                        }
                        catch (Exception)
                        {
                            sqlTrans.Rollback();
                            sqlTrans.Dispose();
                            throw;
                        }
                        finally { sqlComm.Parameters.Clear(); }

                        dtable.Rows[i]["Group ID"] = strGroupID;
                        dtable.Rows[i]["IE Rev Amt"] = dIERevAmt;
                        dtable.Rows[i]["Customs Total RM Cost"] = dCustomsTotalRMCost;
                        dtable.Rows[i]["IE Remark"] = strRemark;
                        dtable.Rows[i]["Pending"] = bHold;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
                {
                    if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        string strGroupID = this.dgvBeiAnDan["Group ID", i].Value.ToString().Trim().ToUpper();
                        decimal dIERevAmt = 0.0M, dCustomsTotalRMCost = 0.0M;
                        if (!String.IsNullOrEmpty(this.dgvBeiAnDan["IE Rev Amt", i].Value.ToString().Trim()))
                        { dIERevAmt = Math.Round(Convert.ToDecimal(this.dgvBeiAnDan["IE Rev Amt", i].Value.ToString().Trim()), 2); }
                        if (!String.IsNullOrEmpty(this.dgvBeiAnDan["Customs Total RM Cost", i].Value.ToString().Trim()))
                        { dCustomsTotalRMCost = Math.Round(Convert.ToDecimal(this.dgvBeiAnDan["Customs Total RM Cost", i].Value.ToString().Trim()), 2); }
                        string strRemark = this.dgvBeiAnDan["IE Remark", i].Value.ToString().Trim().ToUpper();
                        string strGongDanNo = this.dgvBeiAnDan["GongDan No", i].Value.ToString().Trim();

                        sqlComm.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = strGroupID;
                        sqlComm.Parameters.Add("@IERevAmt", SqlDbType.Decimal).Value = dIERevAmt;
                        sqlComm.Parameters.Add("@CustomsTotalRMCost", SqlDbType.Decimal).Value = dCustomsTotalRMCost;
                        sqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
                        sqlComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = strGongDanNo;
                        sqlComm.CommandText = @"UPDATE M_DailyBeiAnDan SET [Group ID] = @GroupID, [IE Rev Amt] = @IERevAmt, [Customs Total RM Cost] = @CustomsTotalRMCost, " +
                                               "[IE Remark] = @Remark WHERE [GongDan No] = @GongDanNo";
                        SqlTransaction sqlTran = sqlConn.BeginTransaction();
                        sqlComm.Transaction = sqlTran;
                        try
                        {
                            sqlComm.ExecuteNonQuery();
                            sqlTran.Commit();
                        }
                        catch (Exception)
                        {
                            sqlTran.Rollback();
                            sqlTran.Dispose();
                            throw;
                        }
                        finally { sqlComm.Parameters.Clear(); }

                        dtable.Rows[i]["Group ID"] = strGroupID;
                        dtable.Rows[i]["IE Rev Amt"] = dIERevAmt;
                        dtable.Rows[i]["Customs Total RM Cost"] = dCustomsTotalRMCost;
                        dtable.Rows[i]["IE Remark"] = strRemark;
                    }
                }
            }
            dtable.AcceptChanges();

            this.dgvBeiAnDan.Columns[0].HeaderText = "全选";
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }            
            MessageBox.Show("Successfully update.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);           
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            string strIEType = this.cmbIEtype.Text.Trim();
            if (String.IsNullOrEmpty(strIEType))
            {
                MessageBox.Show("Please select IE Type first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.cmbIEtype.Focus();
                return;
            }

            bPending = false;
            if (String.Compare(strIEType.ToUpper(), "EXPORT") == 0)
            {
                DialogResult dlgR = MessageBox.Show("Please choose the condition to preview:\n[Yes] : Browse the data generated by gongdan;\n[No] : Browse the data from export pending data;\n[Cancel] : Reject to browse.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dlgR == DialogResult.Yes) { bPending = false; }
                else { bPending = true; }
            }

            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;

            if (bPending == true)
            {
                sqlComm.CommandText = @"SELECT [Group ID], [IE Type], [GongDan No], [Total Ship Qty], [GongDan Qty], [FG EHB], [ESS/LINE], [Selling Price], " +
                                       "[Currency], [Local Total RM Cost] AS [Local Total RM Cost(USD)], [Selling Amount] AS [Selling Amount(USD)], [Order No], " +
                                       "[Order Category], [CHN Name], [Destination], [Pass to IE Date], [GongDan Approved Date], [IE Rev Amt], [OF Rev Amt], " +
                                       "[Customs Total RM Cost], [IE Remark], [OF Remark], [Pending] FROM M_PendingBeiAnDan_Export WHERE [IE Type] = 'EXPORT' " + 
                                       "ORDER BY [Group ID], [GongDan No]";
            }
            else
            {
                sqlComm.Parameters.Add("@IEtype", SqlDbType.NVarChar).Value = strIEType.ToUpper();
                sqlComm.CommandText = @"SELECT [Group ID], [IE Type], [GongDan No], [Total Ship Qty], [GongDan Qty], [FG EHB], [ESS/LINE], [Selling Price], " +
                                       "[Currency], [Local Total RM Cost] AS [Local Total RM Cost(USD)], [Selling Amount] AS [Selling Amount(USD)], [Order No], " +
                                       "[Order Category], [CHN Name], [Destination], [Pass to IE Date], [GongDan Approved Date], [IE Rev Amt], [OF Rev Amt], " +
                                       "[Customs Total RM Cost], [IE Remark], [OF Remark] FROM M_DailyBeiAnDan WHERE [IE Type] = @IEtype ORDER BY [Group ID], [GongDan No]";
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = sqlComm;
            dtable.Rows.Clear();
            dtable.Columns.Clear();
            sqlAdapter.Fill(dtable);
            sqlAdapter.Dispose();

            sqlComm.Parameters.Clear();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            if (dtable.Rows.Count == 0)
            {
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dvFillDGV = dtable.DefaultView;
                this.dgvBeiAnDan.DataSource = dvFillDGV;
                this.dgvBeiAnDan.Columns[0].HeaderText = "全选";
                for (int j = 3; j < this.dgvBeiAnDan.ColumnCount; j++) { this.dgvBeiAnDan.Columns[j].ReadOnly = true; }
                this.dgvBeiAnDan.Columns["IE Rev Amt"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["Customs Total RM Cost"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["IE Remark"].ReadOnly = false;
                this.dgvBeiAnDan.Columns["GongDan No"].Frozen = true;
                this.dgvBeiAnDan.Columns["IE Rev Amt"].DefaultCellStyle.ForeColor = Color.Red;
                this.dgvBeiAnDan.Columns["OF Rev Amt"].DefaultCellStyle.ForeColor = Color.Red;
                this.dgvBeiAnDan.Columns["Customs Total RM Cost"].DefaultCellStyle.ForeColor = Color.Red;

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.BackColor = Color.FromArgb(178, 235, 140);
                this.dgvBeiAnDan.EnableHeadersVisualStyles = false;
                this.dgvBeiAnDan.Columns["Group ID"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["IE Type"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["IE Rev Amt"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["Customs Total RM Cost"].HeaderCell.Style = cellStyle;
                this.dgvBeiAnDan.Columns["IE Remark"].HeaderCell.Style = cellStyle;

                if (bPending == true)
                {
                    this.dgvBeiAnDan.Columns["Pending"].ReadOnly = false;
                    this.dgvBeiAnDan.Columns["Pending"].HeaderCell.Style = cellStyle;
                }
            }
        }

        private string GetGroupID(string strIEtype, SqlConnection sqlConn, SqlDataAdapter sqlAdp)
        {
            string strHead = null;
            switch(strIEtype)
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
            string strSQL1 = @"SELECT SUBSTRING([Group ID], 3, 8) AS [GroupID], MAX(CAST(SUBSTRING([Group ID], 12, LEN([Group ID])) AS Int)) AS [MaxID], [IE Type] FROM " +
                              "C_BeiAnDan WHERE [IE Type] = '" + strIEtype + "' AND [Group ID] LIKE '%" + strDate + "%' GROUP BY SUBSTRING([Group ID], 3, 8), [IE Type]";
            sqlAdp = new SqlDataAdapter(strSQL1, sqlConn);
            sqlAdp.Fill(dtGroupID1);

            string strSQL2 = @"SELECT SUBSTRING([Group ID], 3, 8) AS [GroupID], MAX(CAST(SUBSTRING([Group ID], 12, LEN([Group ID])) AS Int)) AS [MaxID], [IE Type] FROM " +
                              "M_DailyBeiAnDan WHERE [IE Type] = '" + strIEtype + "' AND [Group ID] LIKE '%" + strDate + "%' GROUP BY SUBSTRING([Group ID], 3, 8), [IE Type]";
            sqlAdp = new SqlDataAdapter(strSQL2, sqlConn);
            dtGroupID2.Clear();
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

            string strSuffix2 = null;
            if (dtGroupID2.Rows.Count == 0) { strSuffix2 = "01"; }
            else
            {
                strSuffix2 = dtGroupID2.Rows[0]["MaxID"].ToString().Trim();
                int iNumber2 = Convert.ToInt32(strSuffix2) + 1;
                if (iNumber2.ToString().Trim().Length == 1) { strSuffix2 = "0" + iNumber2.ToString().Trim(); }
                else { strSuffix2 = iNumber2.ToString().Trim(); }
            }

            if (String.Compare(strIEtype, "EXPORT") == 0)
            {
                string strSQL3 = @"SELECT SUBSTRING([Group ID], 3, 8) AS [GroupID], MAX(CAST(SUBSTRING([Group ID], 12, LEN([Group ID])) AS Int)) AS [MaxID], " +
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

            string strReturn = strPrefix + strSuffix;
            return strReturn;
        }

        private void FunctionDGV_IETYPE()
        {
            DataTable dataTbl = dt.Copy();
            dataTbl.Rows.RemoveAt(0);
            DataRow dr = dataTbl.NewRow();
            dr[0] = "RM-D";
            dataTbl.Rows.Add(dr);
            dgvDetails.DataSource = dataTbl;
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
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";
            this.dgvBeiAnDan.Columns[0].HeaderText = "全选";
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
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void cmbIEtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Compare(this.cmbIEtype.Text.Trim(), "EXPORT") == 0) 
            {
                this.btnShowRMB.Visible = true; 
                this.btnDownload.Enabled = true;
                this.btnTransfer.Enabled = true;
            }
            else 
            {
                this.btnShowRMB.Visible = false;
                this.btnDownload.Enabled = false;
                this.btnTransfer.Enabled = false;
            }

            //if (String.Compare(this.cmbIEtype.Text.Trim(), "RMB") == 0 ||
            //    String.Compare(this.cmbIEtype.Text.Trim(), "EXPORT") == 0) { this.btnShowRMB.Visible = true; }
            //else { this.btnShowRMB.Visible = false; }
        }

        private void btnShowRMB_Click(object sender, EventArgs e)
        {
            //if (String.Compare(this.cmbIEtype.Text.Trim(), "RMB") == 0)
            //{
            //    SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            //    if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            //    SqlDataAdapter sqlAdapter = new SqlDataAdapter(@"SELECT * FROM V_GongDanForRMBGroupDetail", sqlConn);
            //    DataTable dtShowRMB = new DataTable();
            //    sqlAdapter.Fill(dtShowRMB);
            //    sqlAdapter.Dispose();

            //    if (dtShowRMB.Rows.Count == 0)
            //    { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            //    else
            //    {
            //        PopUpInfoForm PopUpInfoFrm = new PopUpInfoForm();
            //        PopUpInfoFrm.DataTableRecord = dtShowRMB.Copy();
            //        PopUpInfoFrm.Width = 704;
            //        PopUpInfoFrm.Height = 500;
            //        PopUpInfoFrm.Show();
            //    }

            //    dtShowRMB.Dispose();
            //    if (sqlConn.State == ConnectionState.Open)
            //    {
            //        sqlConn.Close();
            //        sqlConn.Dispose();
            //    }
            //}

            if (String.Compare(this.cmbIEtype.Text.Trim(), "EXPORT") == 0)
            {
                if (this.gBoxShow.Visible == false)
                {
                    this.gBoxShow.Visible = true;
                    this.txtPath.Text = string.Empty;
                }
                else
                { this.gBoxShow.Visible = false; }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.Rows.Count == 0)
            {
                MessageBox.Show("There is no data in data grid view.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnDownload.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand(@"SELECT * FROM B_WeightRatio", sqlConn);
            decimal dRatio = Convert.ToDecimal(sqlComm.ExecuteScalar());
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }

            Microsoft.Office.Interop.Excel.Application excel_Doc = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks_Doc = excel_Doc.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook_Doc = workbooks_Doc.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets[1];

            int iRow1 = 2;
            for (int i = 0; i < this.dgvBeiAnDan.RowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    worksheet_Doc.get_Range(worksheet_Doc.Cells[iRow1, 1], worksheet_Doc.Cells[iRow1, this.dgvBeiAnDan.ColumnCount - 1]).NumberFormatLocal = "@";
                    for (int j = 1; j < this.dgvBeiAnDan.ColumnCount; j++)
                    { worksheet_Doc.Cells[iRow1, j] = this.dgvBeiAnDan[j, i].Value.ToString().Trim(); }
                    iRow1++;
                }
            }

            if (iRow1 > 2)
            {
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).NumberFormatLocal = "@";
                for (int k = 1; k < this.dgvBeiAnDan.ColumnCount; k++)
                { worksheet_Doc.Cells[1, k] = this.dgvBeiAnDan.Columns[k].HeaderText.ToString(); }

                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Bold = true;
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, this.dgvBeiAnDan.ColumnCount - 1]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow1 - 1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Name = "Verdana";
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow1 - 1, this.dgvBeiAnDan.ColumnCount - 1]).Font.Size = 9;
                worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow1 - 1, this.dgvBeiAnDan.ColumnCount - 1]).Borders.LineStyle = 1;
                worksheet_Doc.Cells.EntireColumn.AutoFit();
            }

            object missing = System.Reflection.Missing.Value;
            worksheet_Doc = (Microsoft.Office.Interop.Excel.Worksheet)workbook_Doc.Worksheets.Add(missing, missing, missing, missing);

            worksheet_Doc.get_Range(excel_Doc.Cells[1, 1], excel_Doc.Cells[1, 15]).NumberFormatLocal = "@";
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
                    decimal dSellingAmt = Convert.ToDecimal(this.dgvBeiAnDan["Selling Amount(USD)", i].Value.ToString().Trim());
                    decimal dIERevAmt = Convert.ToDecimal(this.dgvBeiAnDan["IE Rev Amt", i].Value.ToString().Trim());
                    decimal dOFRevAmt = Convert.ToDecimal(this.dgvBeiAnDan["OF Rev Amt", i].Value.ToString().Trim());
                    decimal dAmount = 0.0M;
                    if (dOFRevAmt > 0.0M) { dAmount = dOFRevAmt; }
                    else
                    {
                        if (dIERevAmt > 0.0M) { dAmount = dIERevAmt; }
                        else { dAmount = dSellingAmt; }
                    }

                    worksheet_Doc.get_Range(excel_Doc.Cells[iRow2, 1], excel_Doc.Cells[iRow2, 15]).NumberFormatLocal = "@";
                    worksheet_Doc.Cells[iRow2, 1] = this.dgvBeiAnDan["Group ID", i].Value.ToString().Trim();
                    worksheet_Doc.Cells[iRow2, 2] = string.Empty;
                    worksheet_Doc.Cells[iRow2, 3] = "成品报关出区";
                    worksheet_Doc.Cells[iRow2, 4] = 0.0M;
                    worksheet_Doc.Cells[iRow2, 5] = "进料对口";
                    worksheet_Doc.Cells[iRow2, 6] = string.Empty;
                    worksheet_Doc.Cells[iRow2, 7] = 0;
                    worksheet_Doc.Cells[iRow2, 8] = this.dgvBeiAnDan["FG EHB", i].Value.ToString().Trim();
                    worksheet_Doc.Cells[iRow2, 9] = strGongDanQty;
                    worksheet_Doc.Cells[iRow2, 10] = strGongDanQty;
                    worksheet_Doc.Cells[iRow2, 11] = Convert.ToInt32(strGongDanQty) * dRatio;
                    worksheet_Doc.Cells[iRow2, 12] = dAmount;
                    worksheet_Doc.Cells[iRow2, 13] = "美元";
                    worksheet_Doc.Cells[iRow2, 14] = this.dgvBeiAnDan["Destination", i].Value.ToString().Trim();
                    worksheet_Doc.Cells[iRow2, 15] = this.dgvBeiAnDan["GongDan No", i].Value.ToString().Trim(); 
                    iRow2++;
                }
            }

            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 15]).Font.Bold = true;
            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[1, 15]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow2 - 1, 15]).Font.Name = "Verdana";
            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow2 - 1, 15]).Font.Size = 9;
            worksheet_Doc.get_Range(worksheet_Doc.Cells[1, 1], worksheet_Doc.Cells[iRow2 - 1, 15]).Borders.LineStyle = 1;
            worksheet_Doc.Cells.EntireColumn.AutoFit();
            excel_Doc.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Doc);
            excel_Doc = null;
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to transfer the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
            if (this.dgvBeiAnDan.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (this.dgvBeiAnDan.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvBeiAnDan.Focus();
                return;
            }

            DataTable transferTbl = dtable.Clone();
            int iRowCount = this.dgvBeiAnDan.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvBeiAnDan[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    transferTbl.ImportRow(dvFillDGV.ToTable().Rows[i]);
                    this.dgvBeiAnDan.Rows.RemoveAt(i);
                    i--;
                    iRowCount--;
                }
            }
            dtable.AcceptChanges();

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = @"usp_InsertDailyBeiAnDan_EXPORT";
            sqlComm.Parameters.AddWithValue("@TVP_Insert_EXPORT", transferTbl);
            sqlComm.ExecuteNonQuery();
            sqlComm.Parameters.Clear();
            sqlComm.Dispose();
            transferTbl.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }

            if (dtable.Rows.Count == 0)
            {
                this.dgvBeiAnDan.DataSource = DBNull.Value;
                this.dgvBeiAnDan.Columns[0].HeaderText = String.Empty;
                this.cmbIEtype.Text = String.Empty;
            }
            else { this.dgvBeiAnDan.Columns[0].HeaderText = "全选"; }
            MessageBox.Show("Successfully transfer.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);  
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
            OleDbDataAdapter myAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$] WHERE [IE Type] = 'EXPORT'", myConn);
            DataTable myTable = new DataTable();
            myAdapter.Fill(myTable);
            myAdapter.Dispose();
            myConn.Close();
            myConn.Dispose();

            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to upload.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myTable.Dispose();
                return;
            }
            if (myTable.Columns.Count < 23)
            {
                MessageBox.Show("The uploading data not belongs to the 'EXPORT PENDING' data.");
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
            uploadComm.CommandText = @"usp_UpdatePendingBeiAnDan_EXPORT";
            uploadComm.Parameters.AddWithValue("@TVP_EXPORT", myTable);
            uploadComm.ExecuteNonQuery();
            uploadComm.Parameters.Clear();
            myTable.Dispose();
            uploadComm.Dispose();
            if (uploadConn.State == ConnectionState.Open)
            {
                uploadConn.Close();
                uploadConn.Dispose();
            }
            MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
