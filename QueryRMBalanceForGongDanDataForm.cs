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
    public partial class QueryRMBalance : Form
    {
        private DataGridView dgvDetails = new DataGridView();
        private string strgetRMCustomsCode;
        public string getRMCustomsCode
        {
            get { return strgetRMCustomsCode; }
            set { strgetRMCustomsCode = value; }
        }

        private string strgetBGDNo;
        public string getBGDNo
        {
            get { return strgetBGDNo; }
            set { strgetBGDNo = value; }
        }

        private static QueryRMBalance queryRMBalance;
        public static QueryRMBalance CreateInstance()
        {
            if (queryRMBalance == null || queryRMBalance.IsDisposed)
            {
                queryRMBalance = new QueryRMBalance();
            }
            return queryRMBalance;
        }

        public QueryRMBalance()
        {
            InitializeComponent();
        }

        private void QueryRMBalance_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.OwnerDraw = true;
            toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.ToolTipTitle = "O(∩_∩)O~~";
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(this.btnDownload, "Prompt: download data to excel");

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strgetRMCustomsCode;
            SqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strgetBGDNo;
            SqlComm.CommandText = @"SELECT T1.[Batch No], T1.[GongDan No], T1.[FG No], T1.[GongDan Qty], T1.[IE Type], T2.[Line No], T2.[Item No], T2.[Lot No], " +
                                   "T2.[RM Customs Code], T2.[BGD No], T2.[Consumption], T2.[RM Used Qty], T2.[Drools Quota], T1.[ESS/LINE], T1.[Order No], " +
                                   "T1.[Destination], T1.[Total Ship Qty], T1.[Order Price], T1.[Order Currency], T2.[RM Category], T2.[Inventory Type], " +
                                   "T2.[RM Currency], T2.[Drools EHB], T2.[RM Price], T2.[Drools EHB], T1.[BOM In Customs] FROM C_GongDan AS T1, C_GongDanDetail " +
                                   "AS T2 WHERE T1.[GongDan No] = T2.[GongDan No] AND T2.[RM Customs Code] = @RMCustomsCode AND T2.[BGD No] = @BGDNo";

            SqlDataAdapter SqlAdapter = new SqlDataAdapter();
            SqlAdapter.SelectCommand = SqlComm;
            DataTable dataTable = new DataTable();
            SqlAdapter.Fill(dataTable);
            SqlAdapter.Dispose();

            if (dataTable.Rows.Count == 0) 
            {
                dataTable.Dispose();
                this.dgvQueryRMBalance.DataSource = DBNull.Value; 
            }
            else { this.dgvQueryRMBalance.DataSource = dataTable; }
            this.dgvQueryRMBalance.Focus();

            SqlComm.Parameters.Clear();
            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void dgvQueryRMBalance_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dgvQueryRMBalance.RowHeadersWidth = 50;
            using (SolidBrush b = new SolidBrush(this.dgvQueryRMBalance.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString(e.RowIndex.ToString(System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvQueryRMBalance.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application RmBalance = new Microsoft.Office.Interop.Excel.Application();
            RmBalance.Application.Workbooks.Add(true);

            if (this.dgvQueryRMBalance.RowCount > 0)
            {
                RmBalance.get_Range(RmBalance.Cells[1, 1], RmBalance.Cells[this.dgvQueryRMBalance.RowCount + 1, this.dgvQueryRMBalance.ColumnCount]).NumberFormatLocal = "@";
                for (int x = 0; x < this.dgvQueryRMBalance.RowCount; x++)
                {
                    for (int y = 0; y < this.dgvQueryRMBalance.ColumnCount; y++)
                    { RmBalance.Cells[x + 2, y + 1] = this.dgvQueryRMBalance[y, x].Value.ToString().Trim(); }
                }

                for (int z = 0; z < this.dgvQueryRMBalance.ColumnCount; z++)
                { RmBalance.Cells[1, z + 1] = this.dgvQueryRMBalance.Columns[z].HeaderText.ToString().Trim(); }

                RmBalance.get_Range(RmBalance.Cells[1, 1], RmBalance.Cells[1, this.dgvQueryRMBalance.ColumnCount]).Font.Bold = true;
                RmBalance.get_Range(RmBalance.Cells[1, 1], RmBalance.Cells[1, this.dgvQueryRMBalance.ColumnCount]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                RmBalance.get_Range(RmBalance.Cells[1, 1], RmBalance.Cells[this.dgvQueryRMBalance.RowCount + 1, this.dgvQueryRMBalance.ColumnCount]).Font.Name = "Verdana";
                RmBalance.get_Range(RmBalance.Cells[1, 1], RmBalance.Cells[this.dgvQueryRMBalance.RowCount + 1, this.dgvQueryRMBalance.ColumnCount]).Font.Size = 9;
                RmBalance.get_Range(RmBalance.Cells[1, 1], RmBalance.Cells[this.dgvQueryRMBalance.RowCount + 1, this.dgvQueryRMBalance.ColumnCount]).Borders.LineStyle = 1;
                RmBalance.Cells.EntireColumn.AutoFit();
                RmBalance.Visible = true;
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(RmBalance);
            RmBalance = null;
        }
    }
}
