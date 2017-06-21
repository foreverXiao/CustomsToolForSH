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
    public partial class QueryBOMData : Form
    {      
        private string strgetBatchInfo;
        public string getBatchInfo
        {
            get { return strgetBatchInfo; }
            set { strgetBatchInfo = value; }
        }

        private int igetRowInfo;
        public int getRowInfo
        {
            get { return igetRowInfo; }
            set { igetRowInfo = value; }
        }

        private GetGongDanListForm getGDListFrm = null;
        public QueryBOMData()
        {
            InitializeComponent();
        }

        private void QueryBOMData_Load(object sender, EventArgs e)
        {
            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strgetBatchInfo + "%";
            SqlComm.CommandText = @"SELECT COUNT(*) FROM C_BOM WHERE [Batch No] LIKE @BatchNo AND [Freeze] = 'False'";
            int iJudgeCount = Convert.ToInt32(SqlComm.ExecuteScalar());
            if (iJudgeCount == 0)
            {
                this.lblInfo.Visible = true;
                SqlComm.Parameters.Clear();
                SqlComm.Dispose();
                SqlConn.Close();
                SqlConn.Dispose();
                return;
            }
            else
            {
                string strCount = null;           
                SqlComm.CommandText = @"SELECT [Batch No] FROM C_BOM WHERE [Batch No] LIKE @BatchNo AND [Freeze] = 'False'";
                SqlDataReader SqlReader = SqlComm.ExecuteReader();
                while (SqlReader.Read())
                {
                    if (SqlReader.HasRows)
                    { strCount += SqlReader.GetValue(0).ToString().Trim() + ", "; }
                }
                SqlReader.Close();
                SqlReader.Dispose();

                strCount = strCount.Remove(strCount.Trim().Length - 1) + ".";
                this.lblInfo.Text = "Exist " + iJudgeCount.ToString() + " related BOM data: " + strCount;
                this.lblInfo.Visible = true;
            }

            SqlComm.CommandText = @"SELECT T1.[Batch No], T1.[FG No], T2.[Line No], T2.[Item No], T2.[Lot No], T2.[RM Category], T2.[RM Customs Code], " +
                                   "T2.[BGD No], T1.[FG Qty], T1.[Order Price], T1.[Order Currency], T1.[GongDan Used Qty], T1.[BOM In Customs] " +
                                   "FROM C_BOMDetail T2 RIGHT JOIN C_BOM T1 ON T2.[Batch No] = T1.[Batch No] WHERE T1.[Batch No] LIKE @BatchNo AND " + 
                                   "T1.[Freeze] = 'False' ORDER BY T1.[Batch No], T1.[FG No], T2.[Line No]";

            SqlDataAdapter SqlAdapter = new SqlDataAdapter();
            SqlAdapter.SelectCommand = SqlComm;
            DataTable myTable = new DataTable();
            SqlAdapter.Fill(myTable);
            SqlAdapter.Dispose();

            this.dgvQueryBOM.DataSource = myTable;
            this.dgvQueryBOM.Focus();

            SqlComm.Parameters.Clear();
            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void dgvQueryBOM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string strsetBatchInfo = this.dgvQueryBOM["Batch No", this.dgvQueryBOM.CurrentRow.Index].Value.ToString().Trim();
                string strsetBOMinCustoms = this.dgvQueryBOM["BOM In Customs", this.dgvQueryBOM.CurrentRow.Index].Value.ToString().Trim().ToUpper();
                getGDListFrm = GetGongDanListForm.CreateInstance();
                getGDListFrm.QueryBOM = this;
                getGDListFrm.BatchNoInfo = strsetBatchInfo;
                getGDListFrm.BOMinCustoms = strsetBOMinCustoms;
                getGDListFrm.RowIndexInfo = igetRowInfo;
                getGDListFrm.dgvGongDanList_CellClick(sender, e);
            }
        }
    }
}
