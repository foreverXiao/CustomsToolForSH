using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class GetGongDanListForm : Form
    {
        DataTable gongdanTable = new DataTable();
        DataTable gongdanNoTable = new DataTable();
        DataTable gongdanNoTbl = new DataTable();
        protected DataView dvFillDGV = new DataView();
        string strFilter = null;

        private DataGridView dgvDetails = new DataGridView();
        protected PopUpFilterForm filterFrm = null;

        private QueryBOMData queryBOMData = null;
        public QueryBOMData QueryBOM
        {
            get { return queryBOMData; }
            set { queryBOMData = value; }
        }

        private string strBatchNoInfo = null;
        public string BatchNoInfo
        {
            get { return strBatchNoInfo; }
            set { strBatchNoInfo = value; }
        }

        private string strBOMinCustomsInfo = null;
        public string BOMinCustoms
        {
            get { return strBOMinCustomsInfo; }
            set { strBOMinCustomsInfo = value; }
        }

        private int iRowIndexInfo = 0;
        public int RowIndexInfo
        {
            get { return iRowIndexInfo; }
            set { iRowIndexInfo = value; }
        }
       
        private static GetGongDanListForm getGongDanListFrm;
        public GetGongDanListForm()
        {
            InitializeComponent();
        }
        public static GetGongDanListForm CreateInstance()
        {
            if (getGongDanListFrm == null || getGongDanListFrm.IsDisposed)
            {
                getGongDanListFrm = new GetGongDanListForm();
            }
            return getGongDanListFrm;
        }

        private void GetGongDanListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            gongdanTable.Dispose();
            gongdanNoTable.Dispose();
            gongdanNoTbl.Dispose();
        }

        private void btnGatherData_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount > 0)
            {
                gongdanTable.Columns.Clear();
                gongdanTable.Rows.Clear();
                this.dgvGongDanList.DataSource = DBNull.Value;
            }

            strFilter = "";
            dvFillDGV.RowFilter = "";
            gongdanTable.Rows.Clear();
            gongdanTable.Columns.Clear();

            SqlConnection gongdanConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (gongdanConn.State == ConnectionState.Closed) { gongdanConn.Open(); }
            SqlCommand gongdanComm = new SqlCommand();
            gongdanComm.Connection = gongdanConn;
            SqlDataAdapter gongdanAdapter = new SqlDataAdapter();

            gongdanComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
            gongdanComm.CommandText = @"SELECT * FROM A_eShipment WHERE [Created Date] = @CreatedDate";
            gongdanAdapter.SelectCommand = gongdanComm;
            DataTable dtShipment = new DataTable();
            gongdanAdapter.Fill(dtShipment);
            gongdanComm.CommandText = @"SELECT [MTI Prefix], [Remark] FROM B_MTI_Prefix WHERE [Type] = 'OF' ORDER BY [MTI Prefix]";
            gongdanAdapter.SelectCommand = gongdanComm;
            DataTable dtPrefix1 = new DataTable();
            gongdanAdapter.Fill(dtPrefix1);
            gongdanComm.CommandText = @"SELECT [MTI Prefix] FROM B_MTI_Prefix WHERE [Type] = 'ESS'";
            DataTable dtPrefix2 = new DataTable();
            gongdanAdapter.Fill(dtPrefix2);
            gongdanComm.CommandText = "SELECT * FROM V_QuerySUMGongDanQtyByEssLine";
            DataTable dtCheckQty = new DataTable();
            gongdanAdapter.Fill(dtCheckQty);

            int ieShipment = dtShipment.Rows.Count;
            if (ieShipment > 0)
            {
                gongdanComm.CommandText = @"SELECT * FROM V_GatherGongDanList_MTO";
                gongdanAdapter.SelectCommand = gongdanComm;
                gongdanAdapter.Fill(gongdanTable);

                //remove the data have been in B_MTI_Prefix table, according to MTI Prefix & Remark to sift
                DataRow[] drow = gongdanTable.Select("[ESS/LINE] IS NULL OR [ESS/LINE] = ''");
                foreach (DataRow dr in drow) 
                {                  
                    string strOrderNo = dr["Order No"].ToString().Trim().ToUpper();                   
                    string strIEType = dr["IE Type"].ToString().Trim().ToUpper();
                    if (!String.IsNullOrEmpty(strOrderNo))
                    {
                        string strPrefix = strOrderNo.Substring(0, 3);
                        DataRow[] rows = dtPrefix1.Select("[MTI Prefix] = '" + strPrefix + "'");
                        if (rows.Length > 0) 
                        {
                            foreach (DataRow row in rows)
                            {
                                if (strIEType.Contains(row[1].ToString().Trim().ToUpper()))
                                { dr.Delete(); }
                            }
                        }
                    }
                    else
                    {
                        DataRow[] rows = dtPrefix1.Select("[MTI Prefix] IS NULL OR [MTI Prefix] = ''");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow row in rows)
                            {
                                if (strIEType.Contains(row[1].ToString().Trim().ToUpper()))
                                { dr.Delete(); }
                            }
                        }
                    }
                }
                dtPrefix1.Dispose();
                gongdanTable.AcceptChanges();               

                //use txt_order_key instead of blank ESS/LINE
                DataRow[] dataRow = gongdanTable.Select("[ESS/LINE] IS NULL OR [ESS/LINE] = ''");
                foreach (DataRow dr in dataRow)
                {
                    string strOrderNo = dr["Order No"].ToString().Trim().ToUpper();
                    string strOrderKey = dr["txt_order_key"].ToString().Trim().ToUpper();
                    if (!String.IsNullOrEmpty(strOrderNo))
                    {
                        string strPrefix = strOrderNo.Substring(0, 3);
                        DataRow[] rows = dtPrefix2.Select("[MTI Prefix] = '" + strPrefix + "'");
                        if (rows.Length > 0) { dr["ESS/LINE"] = strOrderKey; }
                    }
                    else
                    {
                        DataRow[] rows = dtPrefix1.Select("[MTI Prefix] IS NULL");
                        if (rows.Length > 0) { dr["ESS/LINE"] = strOrderKey; }
                    }
                }
                dtPrefix2.Dispose();
                gongdanTable.Columns.Remove("txt_order_key");
                gongdanTable.AcceptChanges();
            }
            dtShipment.Dispose();          

            DataTable dtMTI = new DataTable();
            gongdanComm.CommandText = @"SELECT COUNT(*) FROM A_eShipment_MTI WHERE [Created Date] = @CreatedDate";
            int ieShipmentMTI = Convert.ToInt32(gongdanComm.ExecuteScalar());
            if (ieShipmentMTI > 0)
            {
                gongdanComm.CommandText = @"SELECT * FROM V_GatherGongDanList_MTI";
                gongdanAdapter.SelectCommand = gongdanComm;
                gongdanAdapter.Fill(dtMTI);
            }
            
            gongdanComm.Parameters.RemoveAt("@CreatedDate");                     
            gongdanComm.CommandText = @"SELECT [Batch No], [ESS/LINE] FROM B_OrderFulfillment";
            gongdanAdapter.SelectCommand = gongdanComm;
            DataTable dtOF = new DataTable();
            gongdanAdapter.Fill(dtOF);         
            gongdanComm.CommandText = @"SELECT [MTI Prefix], [Remark] FROM B_MTI_Prefix WHERE [Type] = 'IE' ORDER BY [MTI Prefix]";
            gongdanAdapter.SelectCommand = gongdanComm;
            DataTable dtPrefix3 = new DataTable();
            gongdanAdapter.Fill(dtPrefix3);
            gongdanAdapter.Dispose();
            gongdanComm.Dispose();

            if (gongdanTable.Rows.Count > 0 && dtMTI.Rows.Count > 0) { gongdanTable.Merge(dtMTI); }
            else if (gongdanTable.Rows.Count == 0 && dtMTI.Rows.Count > 0) { gongdanTable = dtMTI.Copy(); }
            dtMTI.Dispose();

            if (gongdanTable.Rows.Count == 0)
            {
                dtPrefix3.Dispose();
                dtOF.Dispose();
                dtCheckQty.Dispose();
                gongdanTable.Clear();
                this.dgvGongDanList.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {              
                //remove the data have been generated in B_OrderFulfillment table, according to Batch No & ESS/LINE to sift
                if (dtOF.Rows.Count > 0)
                {
                    int iRow = gongdanTable.Rows.Count;
                    for (int x = 0; x < iRow; x++)
                    {
                        string strBatch = gongdanTable.Rows[x]["Batch No"].ToString().Trim();
                        string strEssLine = gongdanTable.Rows[x]["ESS/LINE"].ToString().Trim();
                        DataRow[] dRow = dtOF.Select("[Batch No] = '" + strBatch + "' AND [ESS/LINE] = '" + strEssLine + "'");
                        if (dRow.Length > 0) 
                        {
                            gongdanTable.Rows.RemoveAt(x);
                            x--;
                            iRow--;
                        }
                    }
                }
                gongdanTable.AcceptChanges();
                dtOF.Dispose();

                if (gongdanTable.Rows.Count == 0)
                {
                    dtPrefix3.Dispose();
                    dtCheckQty.Dispose();
                    gongdanTable.Clear();
                    this.dgvGongDanList.DataSource = DBNull.Value;
                    gongdanComm.Dispose();
                    gongdanConn.Dispose();
                    MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }                

                string strBatchNo = null;
                for (int y = 0; y < gongdanTable.Rows.Count; y++)
                { strBatchNo += "'" + gongdanTable.Rows[y]["Batch No"].ToString().Trim().ToUpper() + "',"; }
                strBatchNo = strBatchNo.Remove(strBatchNo.Length - 1);
                string strOrl = @"SELECT DISTINCT gbh.batch_no FROM gme.GME_BATCH_HEADER gbh WHERE gbh.plant_code = 'PGSH' AND gbh.batch_status = 4 AND gbh.batch_no IN ( " + strBatchNo + " )";

                OracleConnection OrlConn = new OracleConnection(SqlLib.StrOracleConnection);
                OrlConn.Open();
                OracleDataAdapter OrlAdapter = new OracleDataAdapter(strOrl, OrlConn);
                DataTable dtOrl = new DataTable();
                OrlAdapter.Fill(dtOrl);
                OrlAdapter.Dispose();

                //remove the BatchNo which is not closed or not created yet
                int iRowCount = gongdanTable.Rows.Count;
                for (int z = 0; z < iRowCount; z++)
                {
                    DataRow[] rows = dtOrl.Select("batch_no = '" + gongdanTable.Rows[z]["Batch No"].ToString().Trim().ToUpper() + "'");
                    if (rows.Length == 0) 
                    {
                        gongdanTable.Rows.RemoveAt(z);
                        z--;
                        iRowCount--;
                    }
                }
                gongdanTable.AcceptChanges();
                dtOrl.Dispose();
                OrlConn.Close();
                OrlConn.Dispose();

                if (gongdanTable.Rows.Count == 0)
                {
                    dtPrefix3.Dispose();
                    dtCheckQty.Dispose();
                    gongdanTable.Clear();
                    this.dgvGongDanList.DataSource = DBNull.Value;
                    gongdanComm.Dispose();
                    gongdanConn.Dispose();
                    MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                //remove the data that total GongDan Qty in C_GongDan table is greater than Order Qty, as per the same ESS/LINE
                //note: every one ESS/LINE is mapped only one Order Qty
                iRowCount = gongdanTable.Rows.Count;
                for (int k = 0; k < iRowCount; k++)
                {                   
                    string strOrderQty = gongdanTable.Rows[k]["Order Qty"].ToString().Trim();
                    if (!String.IsNullOrEmpty(strOrderQty))
                    {
                        string strEssLine = gongdanTable.Rows[k]["ESS/LINE"].ToString().Trim();
                        int iOrderQty = Convert.ToInt32(strOrderQty);
                        DataRow[] drow = dtCheckQty.Select("[ESS/LINE] = '" + strEssLine + "'");
                        if (drow.Length > 0)
                        {
                            int iGDQty = Convert.ToInt32(drow[0][1].ToString().Trim());
                            if (iGDQty >= iOrderQty)
                            {
                                gongdanTable.Rows.RemoveAt(k);
                                k--;
                                iRowCount--;
                            }
                        }
                    }
                }
                dtCheckQty.Dispose();
                gongdanTable.AcceptChanges();

                if (gongdanTable.Rows.Count == 0)
                {
                    dtPrefix3.Dispose();
                    gongdanTable.Clear();
                    this.dgvGongDanList.DataSource = DBNull.Value;
                    gongdanComm.Dispose();
                    gongdanConn.Dispose();
                    MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                //insert 'GongDan No' & 'Order Price' columns, and update 'IE Type' & 'Destination' columns
                //then remove 'External Price' & 'Transfer Price' & 'CustPO' columns
                gongdanTable.Columns.Add("GongDan No", typeof(string));
                gongdanTable.Columns.Add("Order Price", typeof(decimal));
                gongdanTable.Columns["GongDan No"].SetOrdinal(1);
                gongdanTable.Columns["GongDan No"].DefaultValue = String.Empty;
                gongdanTable.Columns["Order Price"].SetOrdinal(7);
                gongdanTable.Columns["Order Price"].DefaultValue = 0.0M;
                DataView dv = gongdanTable.DefaultView;
                dv.Sort = "Batch No ASC";
                for (int i = 0; i < gongdanTable.Rows.Count; i++)
                {
                    string strNewBatchNo = gongdanTable.Rows[i]["Batch No"].ToString().Trim(), strNewGongDanNo = null;
                    int iLength = gongdanTable.Select("[Batch No] = '" + strNewBatchNo + "'").Length;
                    if (iLength == 1) { strNewGongDanNo = this.GetGongDanNo(strNewBatchNo); }
                    else
                    {
                        int iLen = gongdanTable.Select("[Batch No] = '" + strNewBatchNo + "' AND [GongDan No] IS NOT NULL").Length;
                        if (iLen == 0) { strNewGongDanNo = this.GetGongDanNo(strNewBatchNo); }
                        else
                        {
                            DataRow dr = gongdanTable.Select("[Batch No] = '" + strNewBatchNo + "'", "[GongDan No] DESC")[0];
                            string strGDNo = dr["GongDan No"].ToString().Trim().ToUpper();
                            int iSuffix = Convert.ToInt32(strGDNo.Split('-')[1].ToString()) + 1;
                            strNewGongDanNo = strGDNo.Split('-')[0].ToString() + "-" + iSuffix.ToString();
                        }
                    }
                    gongdanTable.Rows[i]["GongDan No"] = strNewGongDanNo;

                    string strOrderCategory = gongdanTable.Rows[i]["Order Category"].ToString().Trim().ToUpper();
                    string strCustPO = gongdanTable.Rows[i]["CustPO"].ToString().Trim();
                    string strIEType = null;
                    if (String.Compare(strOrderCategory, "MTO") == 0)
                    {
                        foreach (DataRow dr in dtPrefix3.Rows)
                        {
                            string str = gongdanTable.Rows[i]["IE Type"].ToString().Trim();
                            bool b = str.ToUpper().Contains(dr["MTI Prefix"].ToString().Trim().ToUpper());
                            if (b) 
                            { 
                                strIEType = dr["Remark"].ToString().Trim().ToUpper();
                                if (String.Compare(strIEType, "BLP") == 0)
                                {
                                    if (str.Contains("RMB"))
                                    { strIEType = "RMB"; }
                                }
                                break; 
                            }
                            else { strIEType = String.Empty; }
                        }
                        gongdanTable.Rows[i]["IE Type"] = strIEType;
                    }
                    else { strIEType = gongdanTable.Rows[i]["IE Type"].ToString().Trim(); }

                    string strTransferPrice = gongdanTable.Rows[i]["Transfer Price"].ToString().Trim();
                    string strExternalPrice = gongdanTable.Rows[i]["External Price"].ToString().Trim();
                    if (String.IsNullOrEmpty(strTransferPrice) || decimal.Parse(strTransferPrice) == 0.0M)
                    {
                        if (String.IsNullOrEmpty(strExternalPrice)) { gongdanTable.Rows[i]["Order Price"] = 0.0M; }
                        else { gongdanTable.Rows[i]["Order Price"] = Convert.ToDecimal(strExternalPrice); }
                    }
                    else
                    {
                        gongdanTable.Rows[i]["Order Price"] = String.IsNullOrEmpty(strTransferPrice) == true ? 0.0M : Convert.ToDecimal(strTransferPrice);
                        gongdanTable.Rows[i]["Order Currency"] = "USD";
                    }

                    if (gongdanTable.Rows[i]["Destination"].ToString().Trim().ToUpper() == "NONE") { gongdanTable.Rows[i]["Destination"] = String.Empty; }
                }
                dtPrefix3.Dispose();
                gongdanTable.Columns.Remove("External Price");
                gongdanTable.Columns.Remove("Transfer Price");
                gongdanTable.Columns.Remove("CustPO");

                //since BatchNo exist the possibility of duplication, need to check GongDanNo's suffix
                gongdanTable.Columns.Add("JudgeDuplicateGD", typeof(bool));
                gongdanTable.Columns["JudgeDuplicateGD"].DefaultValue = false;
                for (int j = 0; j < gongdanTable.Rows.Count; j++)
                {
                    if (String.Compare(gongdanTable.Rows[j]["JudgeDuplicateGD"].ToString().Trim().ToUpper(), "TRUE") != 0)
                    {
                        DataRow[] dRow = gongdanTable.Select("[Batch No] = '" + gongdanTable.Rows[j]["Batch No"].ToString().Trim().ToUpper() + "'");
                        if (dRow.Length == 1) { dRow[0]["JudgeDuplicateGD"] = true; }
                        else
                        {
                            int iCount = 0;
                            foreach (DataRow dr in dRow)
                            {
                                dr["JudgeDuplicateGD"] = true;
                                int iSuffix = Convert.ToInt32(dr["GongDan No"].ToString().Split('-')[1]) + iCount;
                                dr["GongDan No"] = dr["GongDan No"].ToString().Split('-')[0] + "-" + iSuffix.ToString();
                                iCount++;
                            }
                        }
                    }
                }
                gongdanTable.Columns.Remove("JudgeDuplicateGD");
                gongdanTable.Columns.Add("PC Item", typeof(bool));
                gongdanTable.Columns.Add("GD Pending", typeof(bool));
                gongdanTable.Columns.Add("BOM In Customs", typeof(string));
                gongdanTable.Columns.Add("Judge Created BOM", typeof(bool));
                gongdanTable.Columns.Add("Judge Multi-BOM", typeof(bool));
                gongdanTable.Columns.Add("Judge GD Qty", typeof(bool));

                dvFillDGV = gongdanTable.DefaultView;
                this.dgvGongDanList.DataSource = dvFillDGV;
                this.dgvGongDanList.Columns["BOM In Customs"].Visible = false;
                this.dgvGongDanList.Columns["Judge Created BOM"].Visible = false;
                this.dgvGongDanList.Columns["Judge Multi-BOM"].Visible = false;
                this.dgvGongDanList.Columns["Judge GD Qty"].Visible = false;
                this.dgvGongDanList.Columns["GongDan No"].Frozen = true;
            }
            if (gongdanConn.State == ConnectionState.Open)
            {
                gongdanConn.Close();
                gongdanConn.Dispose();
            }
        }

        private string GetGongDanNo(string strBatchNo)
        {
            string strGongDanNo = null;
            if (gongdanNoTbl.Rows.Count == 0)
            {
                SqlConnection conn1 = new SqlConnection(SqlLib.StrSqlConnection);
                if (conn1.State == ConnectionState.Closed) { conn1.Open(); }
                string strSQL = @"SELECT [Batch No], MAX(MaxID) AS MaxLine FROM (SELECT [Batch No], [GongDan No], 
                                  CASE WHEN CHARINDEX('-', [GongDan No]) > 0 THEN SUBSTRING([GongDan No], CHARINDEX('-', [GongDan No]) + 1, LEN([GongDan No]))
                                  ELSE 0 END AS MaxID FROM M_DailyGongDanList) AS X GROUP BY [Batch No]
                                  UNION
                                  SELECT [Batch No], MAX(MaxID) AS MaxLine FROM (SELECT [Batch No], [GongDan No], 
                                  CASE WHEN CHARINDEX('-', [GongDan No]) > 0 THEN SUBSTRING([GongDan No], CHARINDEX('-', [GongDan No]) + 1, LEN([GongDan No]))
                                  ELSE 0 END AS MaxID FROM M_DailyGongDan) AS X GROUP BY [Batch No]";
                SqlDataAdapter adapter1 = new SqlDataAdapter(strSQL, conn1);

                adapter1.Fill(gongdanNoTbl); //Search max Gongdan No from table M_DailyGongDanList and M_DailyGongDan
                adapter1.Dispose();
                if (conn1.State == ConnectionState.Open)
                {
                    conn1.Close();
                    conn1.Dispose();
                }
            }
            if (gongdanNoTable.Rows.Count == 0)
            {
                SqlConnection conn2 = new SqlConnection(SqlLib.StrSqlConnection);
                if (conn2.State == ConnectionState.Closed) { conn2.Open(); }
                string strSQL = @"SELECT [Batch No], MAX(MaxID) AS MaxLine FROM (SELECT [Batch No], [GongDan No], 
                                  CASE WHEN CHARINDEX('-', [GongDan No]) > 0 THEN SUBSTRING([GongDan No], CHARINDEX('-', [GongDan No]) + 1, LEN([GongDan No]))
                                  ELSE 0 END AS MaxID FROM C_GongDan) AS X GROUP BY [Batch No]";
                SqlDataAdapter adapter2 = new SqlDataAdapter(strSQL, conn2);

                adapter2.Fill(gongdanNoTable); //Search max Gongdan No from table C_GongDan
                adapter2.Dispose();
                if (conn2.State == ConnectionState.Open)
                {
                    conn2.Close();
                    conn2.Dispose();
                }
            }

            DataRow[] dr1 = gongdanNoTbl.Select("[Batch No] = '" + strBatchNo + "'");
            if (dr1.Length == 0)
            {
                DataRow[] dr2 = gongdanNoTable.Select("[Batch No] = '" + strBatchNo + "'");
                if (dr2.Length == 0) { strGongDanNo = strBatchNo + "-1"; }
                else
                {
                    int iSuffix = Convert.ToInt32(dr2[0][1].ToString()) + 1;
                    strGongDanNo = strBatchNo + "-" + iSuffix.ToString().Trim();
                }
            }
            else
            {
                DataRow dr = gongdanNoTbl.Select("[Batch No] = '" + strBatchNo + "'", "[MaxLine] DESC")[0];
                int iSuffix = Convert.ToInt32(dr[1].ToString()) + 1;
                strGongDanNo = strBatchNo + "-" + iSuffix.ToString();
            }
            return strGongDanNo;
        }

        private void dgvGongDanList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvGongDanList.RowCount; i++) { this.dgvGongDanList[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvGongDanList.RowCount; i++) { this.dgvGongDanList[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
                    {
                        if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvGongDanList[0, i].Value = true; }

                        else { this.dgvGongDanList[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvGongDanList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvGongDanList.RowCount == 0) { return; }
            if (this.dgvGongDanList.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvGongDanList.RowCount && iCount > 0)
                { this.dgvGongDanList.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvGongDanList.RowCount)
                { this.dgvGongDanList.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvGongDanList.Columns[0].HeaderText = "全选"; }
            }
        }

        public void dgvGongDanList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (queryBOMData != null)
            {
                queryBOMData.Close();
                queryBOMData = null;

                string strNewGongDan = null;
                this.dgvGongDanList["Batch No", iRowIndexInfo].Value = strBatchNoInfo;
                int iLength = gongdanTable.Select("[Batch No] = '" + strBatchNoInfo + "'").Length;              
                if (iLength == 1) { strNewGongDan = GetGongDanNo(strBatchNoInfo); }
                else 
                {
                    int iLen = gongdanTable.Select("[Batch No] = '" + strBatchNoInfo + "' AND [GongDan No] IS NOT NULL").Length;
                    if (iLen == 0) { strNewGongDan = GetGongDanNo(strBatchNoInfo); }
                    else
                    {
                        DataRow dr = gongdanTable.Select("[Batch No] = '" + strBatchNoInfo + "'", "[GongDan No] DESC")[0];
                        string strGDNo = dr["GongDan No"].ToString().Trim().ToUpper();
                        int iSuffix = Convert.ToInt32(strGDNo.Split('-')[1].ToString()) + 1;
                        strNewGongDan = strGDNo.Split('-')[0].ToString() + "-" + iSuffix.ToString();
                    }
                }
                this.dgvGongDanList["GongDan No", iRowIndexInfo].Value = strNewGongDan;            
                this.dgvGongDanList["BOM In Customs", iRowIndexInfo].Value = strBOMinCustomsInfo;

                this.Activate();
                this.Refresh();
                return;
            }

            if (e.ColumnIndex == 1) //Delete
            {
                if (MessageBox.Show("Are you sure to delete the data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (this.dgvGongDanList.Columns[0].HeaderText.Trim() == "È«Ñ¡") 
                    {
                        MessageBox.Show("Please choose to delete the data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (this.dgvGongDanList.Columns.Contains("Created Date"))
                    {
                        SqlConnection delConn = new SqlConnection(SqlLib.StrSqlConnection);
                        delConn.Open();
                        SqlCommand delComm = new SqlCommand();
                        delComm.Connection = delConn;

                        string strGongDanList = null, strBatchEssList = null;
                        int iRow = this.dgvGongDanList.Rows.Count;
                        for (int i = 0; i < iRow; i++)
                        {
                            if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                            {
                                strGongDanList += "'" + this.dgvGongDanList["GongDan No", i].Value.ToString().Trim() + "',";
                                strBatchEssList += "'" + this.dgvGongDanList["Batch No", i].Value.ToString().Trim() + this.dgvGongDanList["ESS/LINE", i].Value.ToString().Trim() + "',";                               
                            }                           
                        }
                        if (!String.IsNullOrEmpty(strGongDanList))
                        {
                            strGongDanList = strGongDanList.Remove(strGongDanList.Length - 1);
                            delComm.CommandText = @"DELETE FROM M_DailyGongDanList WHERE [GongDan No] IN (" + strGongDanList + ")";
                            delComm.ExecuteNonQuery();
                        }
                        if (!String.IsNullOrEmpty(strBatchEssList))
                        {
                            strBatchEssList = strBatchEssList.Remove(strBatchEssList.Length - 1);
                            delComm.CommandText = @"DELETE FROM B_OrderFulfillment WHERE [Batch No] + [ESS/LINE] IN (" + strBatchEssList + ")";
                            delComm.ExecuteNonQuery();
                        }
                        delConn.Close();
                        delComm.Dispose();
                    }

                    int iRows = this.dgvGongDanList.Rows.Count;
                    for (int j = 0; j < iRows; j++)
                    {
                        if (String.Compare(this.dgvGongDanList[0, j].EditedFormattedValue.ToString(), "True") == 0)
                        {
                            this.dgvGongDanList.Rows.RemoveAt(j);
                            j--;
                            iRows--;
                        }
                    }
                }
            }

            if (e.ColumnIndex == 2) //Split
            {
                int iCurrentRow = this.dgvGongDanList.CurrentRow.Index;
                string strGongDan = this.dgvGongDanList["GongDan No", iCurrentRow].Value.ToString().Trim();
                string strPCItem = this.dgvGongDanList["PC Item", iCurrentRow].Value.ToString().Trim().ToUpper();
                string strGDPending = this.dgvGongDanList["GD Pending", iCurrentRow].Value.ToString().Trim().ToUpper();                

                DataRow dRow = gongdanTable.NewRow();
                dRow["Batch No"] = this.dgvGongDanList["Batch No", iCurrentRow].Value.ToString().Trim();
                dRow["GongDan No"] = strGongDan.Split('-')[0] + "-" + (Convert.ToInt32(strGongDan.Split('-')[1]) + 1).ToString().Trim();
                dRow["Order No"] = this.dgvGongDanList["Order No", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["ESS/LINE"] = this.dgvGongDanList["ESS/LINE", iCurrentRow].Value.ToString().Trim();              
                if (String.IsNullOrEmpty(this.dgvGongDanList["Order Qty", iCurrentRow].Value.ToString().Trim())) { dRow["Order Qty"] = 0; }
                else { dRow["Order Qty"] = Convert.ToInt32(this.dgvGongDanList["Order Qty", iCurrentRow].Value.ToString().Trim()); }  
                if (String.IsNullOrEmpty(this.dgvGongDanList["GongDan Qty", iCurrentRow].Value.ToString().Trim())) { dRow["GongDan Qty"] = 0; }
                else { dRow["GongDan Qty"] = Convert.ToInt32(this.dgvGongDanList["GongDan Qty", iCurrentRow].Value.ToString().Trim()); }
                dRow["IE Type"] = this.dgvGongDanList["IE Type", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["Destination"] = this.dgvGongDanList["Destination", iCurrentRow].Value.ToString().Trim().ToUpper();             
                if (String.IsNullOrEmpty(this.dgvGongDanList["Order Price", iCurrentRow].Value.ToString().Trim())) { dRow["Order Price"] = 0.0M; }
                else { dRow["Order Price"] = Convert.ToDecimal(this.dgvGongDanList["Order Price", iCurrentRow].Value.ToString().Trim()); }
                dRow["Order Currency"] = this.dgvGongDanList["Order Currency", iCurrentRow].Value.ToString().Trim().ToUpper();               
                dRow["Order Category"] = this.dgvGongDanList["Order Category", iCurrentRow].Value.ToString().Trim().ToUpper();
                dRow["PartNo"] = this.dgvGongDanList["PartNo", iCurrentRow].Value.ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(strPCItem) || String.Compare(strPCItem, "FALSE") == 0) { dRow["PC Item"] = false; }
                else { dRow["PC Item"] = true; }
                if (String.IsNullOrEmpty(strGDPending) || String.Compare(strGDPending, "FALSE") == 0) { dRow["GD Pending"] = false; }
                else { dRow["GD Pending"] = true; }
                dRow["BOM In Customs"] = this.dgvGongDanList["BOM In Customs", iCurrentRow].Value.ToString().Trim().ToUpper();
                gongdanTable.Rows.InsertAt(dRow, iCurrentRow + 1);

                DataView dv = gongdanTable.DefaultView;
                dv.Sort = "GongDan No";
                this.dgvGongDanList.DataSource = gongdanTable;
            }

            if (e.ColumnIndex == 3) //View BOM data
            {
                string strBatchNo = this.dgvGongDanList["Batch No", this.dgvGongDanList.CurrentRow.Index].Value.ToString().Trim().Substring(0, 8);
                QueryBOMData queryBOM = new QueryBOMData();
                queryBOM.getBatchInfo = strBatchNo;
                queryBOM.getRowInfo = this.dgvGongDanList.CurrentRow.Index;
                queryBOM.Show();
            }

            if (e.ColumnIndex == 10) //IE Type comboBox value
            {
                int iIEType = this.dgvGongDanList.Columns["IE Type"].Index;
                if (this.dgvGongDanList.CurrentCell.ColumnIndex == iIEType)
                {
                    FunctionDGV_IETYPE();
                    dgvDetails.Width = 119;
                    dgvDetails.Height = 180;

                    Rectangle rec = this.dgvGongDanList.GetCellDisplayRectangle(9, this.dgvGongDanList.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvGongDanList.Columns[9].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvGongDanList.Location.Y > this.dgvGongDanList.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else
                    { dgvDetails.Top = rec.Top + this.dgvGongDanList.Location.Y; }

                    if (dgvDetails.RowCount > 0) { dgvDetails.Visible = true; }
                }
            }

            if (e.ColumnIndex == 11) //Destination comboBox value
            {
                int iDestination = this.dgvGongDanList.Columns["Destination"].Index;
                if (this.dgvGongDanList.CurrentCell.ColumnIndex == iDestination)
                {
                    FunctionDGV_DESTINATION();
                    dgvDetails.Width = 239;
                    dgvDetails.Height = 114;

                    Rectangle rec = this.dgvGongDanList.GetCellDisplayRectangle(10, this.dgvGongDanList.CurrentRow.Index, false);
                    dgvDetails.Left = rec.Left + this.dgvGongDanList.Columns[10].Width;
                    if (rec.Top + dgvDetails.Height + this.dgvGongDanList.Location.Y > this.dgvGongDanList.Height)
                    { dgvDetails.Top = rec.Top - dgvDetails.Height; }
                    else
                    { dgvDetails.Top = rec.Top + this.dgvGongDanList.Location.Y; }

                    if (dgvDetails.RowCount > 0) { dgvDetails.Visible = true; }
                }
            }

            if (e.ColumnIndex != 6 && e.ColumnIndex != 10 && e.ColumnIndex != 11)
            { dgvDetails.Visible = false; }
        }

        private void DGV_Details_CellClick(object sender, EventArgs e)
        {
            int iIEType = this.dgvGongDanList.Columns["IE Type"].Index;
            int iDestination = this.dgvGongDanList.Columns["Destination"].Index;

            if (this.dgvGongDanList.CurrentCell != null && this.dgvGongDanList.CurrentCell.ColumnIndex == iIEType)
            {
                string strIEType = dgvDetails["IE Type", dgvDetails.CurrentCell.RowIndex].Value.ToString().Trim();
                this.dgvGongDanList[iIEType, this.dgvGongDanList.CurrentCell.RowIndex].Value = strIEType;
            }

            if (this.dgvGongDanList.CurrentCell != null && this.dgvGongDanList.CurrentCell.ColumnIndex == iDestination)
            {
                string strDestination = dgvDetails["Address", dgvDetails.CurrentCell.RowIndex].Value.ToString().Trim();
                this.dgvGongDanList[iDestination, this.dgvGongDanList.CurrentCell.RowIndex].Value = strDestination;
            }
            dgvDetails.Visible = false;     
        }

        private void FunctionDGV_IETYPE()
        {
            SqlLib sqllib = new SqlLib();
            DataTable ietypeTable = sqllib.GetDataTable(@"SELECT * FROM B_IEType", "B_IEType").Copy();
            sqllib.Dispose();  

            dgvDetails.DataSource = ietypeTable;
            this.dgvGongDanList.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);         
        }

        private void FunctionDGV_DESTINATION()
        {
            SqlLib sqlLib = new SqlLib();
            DataTable destinationTable = sqlLib.GetDataTable(@"SELECT [Short Name] AS Address, [Address] AS Destination FROM B_Address", "B_Address").Copy();
            sqlLib.Dispose();               

            dgvDetails.DataSource = destinationTable;
            this.dgvGongDanList.Controls.Add(dgvDetails);
            dgvDetails.Visible = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.CellClick += new DataGridViewCellEventHandler(DGV_Details_CellClick);          
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnCheck.Focus();
                return;
            }

            int iExistBOM = 0, iExistSuffix = 0;
            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvGongDanList["Batch No", i].Value.ToString().Trim().Substring(0, 8) + "%";
                SqlComm.CommandText = @"SELECT COUNT(*) FROM C_BOM WHERE [Batch No] LIKE @BatchNo AND [Freeze] = 'False'";
                int iJudgeCount = Convert.ToInt32(SqlComm.ExecuteScalar()); //Batch No is froze and no-generate Batch + Suffix. This case belongs to no-created BOM.

                if (iJudgeCount == 0)
                {
                    this.dgvGongDanList["Judge Created BOM", i].Value = true;
                    DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                    dgvRow.DefaultCellStyle.BackColor = Color.LightPink;
                    iExistBOM++;
                }
                else
                {
                    if (iJudgeCount == 1)
                    {
                        SqlComm.Parameters.Clear();
                        SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvGongDanList["Batch No", i].Value.ToString().Trim();
                        SqlComm.CommandText = @"SELECT COUNT(*) FROM C_BOM WHERE [Batch No] = @BatchNo AND [Freeze] = 'False'";
                        int iCount = Convert.ToInt32(SqlComm.ExecuteScalar());
                        if (iCount == 0) //Batch No is froze but Batch + Suffix is ok. This case belongs to Multi-BOM.
                        {
                            this.dgvGongDanList["Judge Multi-BOM", i].Value = true;
                            DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                            dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
                            iExistSuffix++;
                        }
                        else
                        {
                            SqlComm.Parameters.Clear();
                            SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvGongDanList["Batch No", i].Value.ToString().Trim().Substring(0, 8);
                            SqlComm.CommandText = @"SELECT [BOM In Customs] FROM C_BOM WHERE SUBSTRING([Batch No], 1, 8) = @BatchNo AND [Freeze] = 'False'";
                            string strBOMinCustoms = Convert.ToString(SqlComm.ExecuteScalar());
                            if (!String.IsNullOrEmpty(strBOMinCustoms)) { this.dgvGongDanList["BOM In Customs", i].Value = strBOMinCustoms; }

                            if (this.dgvGongDanList["Judge Multi-BOM", i].Value.ToString().Trim() == "True")
                            {
                                this.dgvGongDanList["Judge Multi-BOM", i].Value = false;
                                this.dgvGongDanList.Rows[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                            }
                        }
                    }

                    if (iJudgeCount > 1)
                    {
                        this.dgvGongDanList["Judge Multi-BOM", i].Value = true;
                        DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                        dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
                        iExistSuffix++;
                    }

                    if (this.dgvGongDanList["Judge Created BOM", i].Value.ToString().Trim() == "True")
                    {
                        this.dgvGongDanList["Judge Created BOM", i].Value = false;
                        this.dgvGongDanList.Rows[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                    }                 
                }
                SqlComm.Parameters.Clear();
            }

            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }

            if (iExistBOM == 0 && iExistSuffix == 0)
            { MessageBox.Show("No abnormal data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                if (iExistBOM == 0 && iExistSuffix > 0)
                { MessageBox.Show(iExistSuffix.ToString() + " Batch No has multi-BOM.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                else if (iExistBOM > 0 && iExistSuffix == 0)
                { MessageBox.Show(iExistBOM.ToString() + " BOM has not created yet.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                else
                {
                    string strInfo = iExistBOM.ToString() + " BOM has not created yet.\n" + iExistSuffix.ToString() + " Batch No has multi-BOM.";
                    MessageBox.Show(strInfo, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnCheckQty_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnCheckQty.Focus();
                return;
            }

            for (int x = 0; x < this.dgvGongDanList.RowCount; x++)
            {
                string strJudge = this.dgvGongDanList["Judge GD Qty", x].Value.ToString().Trim().ToUpper();
                if (String.Compare(strJudge, "TRUE") == 0)
                {
                    this.dgvGongDanList["Judge GD Qty", x].Value = false;
                    this.dgvGongDanList.Rows[x].DefaultCellStyle.BackColor = Color.Gainsboro;
                    gongdanTable.Rows[x].AcceptChanges();
                }
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;

            DataTable mytable = new DataTable();
            mytable.Columns.Add("Batch No", typeof(string));
            mytable.Columns.Add("GongDan Qty", typeof(Int32));
            string strBatchNo = this.dgvGongDanList["Batch No", 0].Value.ToString().Trim();
            int iGongDanQty = Convert.ToInt32(this.dgvGongDanList["GongDan Qty", 0].Value.ToString().Trim());
            if (this.dgvGongDanList.RowCount == 1)
            {
                DataRow dRow = mytable.NewRow();
                dRow[0] = strBatchNo;
                dRow[1] = iGongDanQty;
                mytable.Rows.Add(dRow);
            }
            else
            {
                for (int i = 1; i < this.dgvGongDanList.RowCount; i++)
                {
                    if (String.Compare(this.dgvGongDanList["Batch No", i].Value.ToString().Trim(), strBatchNo) != 0)
                    {
                        DataRow dRow = mytable.NewRow();
                        dRow[0] = strBatchNo;
                        dRow[1] = iGongDanQty;
                        mytable.Rows.Add(dRow);

                        strBatchNo = this.dgvGongDanList["Batch No", i].Value.ToString().Trim();
                        iGongDanQty = Convert.ToInt32(this.dgvGongDanList["GongDan Qty", i].Value.ToString().Trim());
                    }
                    else { iGongDanQty += Convert.ToInt32(this.dgvGongDanList["GongDan Qty", i].Value.ToString().Trim()); }

                    if (i == this.dgvGongDanList.RowCount - 1)
                    {
                        DataRow dRow = mytable.NewRow();
                        dRow[0] = strBatchNo;
                        dRow[1] = iGongDanQty;
                        mytable.Rows.Add(dRow);
                    }
                }
            }

            mytable.Columns.Add("JudgeQty", typeof(bool));
            mytable.Columns.Add("FG Qty", typeof(Int32));
            mytable.Columns.Add("Total GD Used Qty", typeof(Int32));
            mytable.Columns[2].DefaultValue = false;
            mytable.Columns[3].DefaultValue = 0;
            mytable.Columns[4].DefaultValue = 0;
            int iCount = 0;
            int iTableRow = mytable.Rows.Count;
            for (int j = 0; j < iTableRow; j++)
            {
                sqlComm.Parameters.Clear();
                sqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = mytable.Rows[j][0].ToString().Trim();
                sqlComm.CommandText = @"SELECT [FG Qty] FROM C_BOM WHERE [Batch No] = @BatchNo";
                int iFGQty = Convert.ToInt32(sqlComm.ExecuteScalar());
                if (iFGQty == 0)
                { 
                    //Delete the data that the Batch has not created
                    mytable.Rows.RemoveAt(j);
                    j--;
                    iTableRow--;
                    continue;
                }

                sqlComm.CommandText = @"SELECT COUNT(*) FROM C_GongDan WHERE [Batch No] = @BatchNo";
                int iJudgeGD = Convert.ToInt32(sqlComm.ExecuteScalar());
                int iTotalGDQty = 0;
                if (iJudgeGD == 0) { iTotalGDQty = 0; }
                else
                {
                    sqlComm.CommandText = @"SELECT SUM([GongDan Qty]) AS TotalGDQty FROM C_GongDan WHERE [Batch No] = @BatchNo";
                    iTotalGDQty = Convert.ToInt32(sqlComm.ExecuteScalar().ToString().Trim());
                }

                int iResult = iTotalGDQty + Convert.ToInt32(mytable.Rows[j][1].ToString().Trim()) - iFGQty;
                if (iResult > 0)
                {
                    mytable.Rows[j][2] = true;
                    mytable.Rows[j][3] = iFGQty;
                    mytable.Rows[j][4] = iTotalGDQty;                   
                    iCount++;
                }         
            }

            if (iCount == 0)
            {
                MessageBox.Show("No abnormal data exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvGongDanList.Focus();
            }
            else
            {
                int iRow = mytable.Rows.Count;
                for (int k = 0; k < iRow; k++)
                {
                    if (String.Compare(mytable.Rows[k][2].ToString().Trim(), "True") != 0)
                    { 
                        mytable.Rows.RemoveAt(k);
                        k--;
                        iRow--;
                    }
                }

                for (int m = 0; m < mytable.Rows.Count; m++)
                {
                    string strBatch = mytable.Rows[m][0].ToString().Trim();
                    for (int n = 0; n < this.dgvGongDanList.RowCount; n++)
                    {
                        if (String.Compare(this.dgvGongDanList["Batch No", n].Value.ToString().Trim(), strBatch) == 0)
                        {
                            this.dgvGongDanList["Judge GD Qty", n].Value = true;
                            DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(n);
                            dgvRow.DefaultCellStyle.BackColor = Color.Khaki;
                        }
                    }
                }

                mytable.Columns.RemoveAt(2);
                mytable.Columns[1].SetOrdinal(3);
                PopUpInfoForm PopUpInfoFrm = new PopUpInfoForm();
                PopUpInfoFrm.DataTableRecord = mytable;
                PopUpInfoFrm.Show();
                //MessageBox.Show("There are " + iCount.ToString() + " Batch No that Total GongDan Qty is greater than FG Qty", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }

            mytable.Dispose();
            sqlComm.Parameters.Clear();
            sqlComm.Dispose();
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        private void btnPCItem_Click(object sender, EventArgs e)
        {
            SqlConnection sqlconn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlconn.State == ConnectionState.Closed) { sqlconn.Open(); }
            SqlDataAdapter sqladapter = new SqlDataAdapter(@"SELECT * FROM B_PCItem", sqlconn);
            DataTable dtPCItem = new DataTable();
            sqladapter.Fill(dtPCItem);
            sqladapter.Dispose();

            int iCount = 0;
            for (int i = 0; i < this.dgvGongDanList.Rows.Count; i++)
            {
                string strPartNo = this.dgvGongDanList["PartNo", i].Value.ToString().Trim();
                if (!String.IsNullOrEmpty(strPartNo) && strPartNo.Contains("-"))
                {
                    string strOrderCurr = this.dgvGongDanList["Order Currency", i].Value.ToString().Trim();
                    if (String.Compare(strOrderCurr.ToUpper(), "RMB") == 0)
                    {
                        string strGrade = strPartNo.Split('-')[0].Trim();
                        DataRow[] drow = dtPCItem.Select("[Grade] = '" + strGrade + "'");
                        if (drow.Length > 0)
                        {
                            this.dgvGongDanList["IE Type", i].Value = "RMB-D";
                            this.dgvGongDanList["PC Item", i].Value = true;
                            iCount++;
                        }
                    }
                }
            }
            gongdanTable.AcceptChanges();

            if (iCount > 0) { MessageBox.Show("There are " + iCount.ToString() + " PC Item.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("There is no PC Item.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlconn.Close();
                sqlconn.Dispose();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount == 0) 
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnGenerate.Focus();
                return;
            }

            if (this.dgvGongDanList.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select the data to generate gongdan list.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvGongDanList.Focus();
                return;
            }

            SqlConnection SqlDbConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlDbConn.State == ConnectionState.Closed) { SqlDbConn.Open(); }
            SqlCommand SqlDbComm = new SqlCommand();
            SqlDbComm.Connection = SqlDbConn;

            int iRowCount = this.dgvGongDanList.RowCount;
            for (int i = 0; i < iRowCount; i++)
            {
                if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strBatch = this.dgvGongDanList["Batch No", i].Value.ToString().Trim().ToUpper();
                    string strEss = this.dgvGongDanList["ESS/LINE", i].Value.ToString().Trim().ToUpper();                
                    string strIEType = this.dgvGongDanList["IE Type", i].Value.ToString().Trim().ToUpper();
                    string strGDPending = this.dgvGongDanList["GD Pending", i].Value.ToString().Trim().ToUpper();
                    string strOrderNo = this.dgvGongDanList["Order No", i].Value.ToString().Trim().ToUpper();
                    string strOrderQty = this.dgvGongDanList["Order Qty", i].Value.ToString().Trim();
                    string strGDQty = this.dgvGongDanList["GongDan Qty", i].Value.ToString().Trim();
                    string strOrderPrice = this.dgvGongDanList["Order Price", i].Value.ToString().Trim();
                    string strOrderCurrency = this.dgvGongDanList["Order Currency", i].Value.ToString().Trim().ToUpper();
                    string strOrderCategory = this.dgvGongDanList["Order Category", i].Value.ToString().Trim().ToUpper();
                    string strDestination = this.dgvGongDanList["Destination", i].Value.ToString().Trim().ToUpper();
                    string strPartNo = this.dgvGongDanList["PartNo", i].Value.ToString().Trim().ToUpper();
                    string strPCItem = this.dgvGongDanList["PC Item", i].Value.ToString().Trim().ToUpper();

                    SqlDbComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatch;
                    SqlDbComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = strOrderNo;
                    if (String.IsNullOrEmpty(strOrderQty)) { SqlDbComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = 0; }
                    else { SqlDbComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = Convert.ToInt32(strOrderQty); }
                    if (String.IsNullOrEmpty(strGDQty)) { SqlDbComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                    else { SqlDbComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGDQty); }
                    SqlDbComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = strIEType;
                    SqlDbComm.Parameters.Add("@OrderCategory", SqlDbType.NVarChar).Value = strOrderCategory;
                    SqlDbComm.Parameters.Add("@OrderCurrency", SqlDbType.NVarChar).Value = strOrderCurrency;
                    if (String.IsNullOrEmpty(strOrderPrice)) { SqlDbComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = 0.0M; }
                    else { SqlDbComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strOrderPrice), 4); }
                    SqlDbComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = strDestination;
                    SqlDbComm.Parameters.Add("@ESSLINE", SqlDbType.NVarChar).Value = strEss;
                    SqlDbComm.Parameters.Add("@PartNo", SqlDbType.NVarChar).Value = strPartNo;
                    if (String.IsNullOrEmpty(strPCItem) || String.Compare(strPCItem, "FALSE") == 0) { SqlDbComm.Parameters.Add("@PCItem", SqlDbType.Bit).Value = false; }
                    else { SqlDbComm.Parameters.Add("@PCItem", SqlDbType.Bit).Value = true; }

                    if (String.IsNullOrEmpty(strEss) || 
                        String.IsNullOrEmpty(strIEType) || 
                        String.IsNullOrEmpty(strDestination) || 
                        String.IsNullOrEmpty(strOrderCurrency) || 
                        String.IsNullOrEmpty(strOrderCategory))
                    { SqlDbComm.Parameters.Add("@GDPending", SqlDbType.Bit).Value = true; }
                    else if (String.IsNullOrEmpty(strGDPending) || 
                             String.Compare(strGDPending, "FALSE") == 0)
                    { SqlDbComm.Parameters.Add("@GDPending", SqlDbType.Bit).Value = false; }
                    else { SqlDbComm.Parameters.Add("@GDPending", SqlDbType.Bit).Value = true; }

                    SqlDbComm.Parameters.Add("@BOMInCustoms", SqlDbType.NVarChar).Value = this.dgvGongDanList["BOM In Customs", i].Value.ToString().Trim().ToUpper();
                    SqlDbComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm"));
                    SqlDbComm.Parameters.Add("@GongDanNo", SqlDbType.NVarChar).Value = this.dgvGongDanList["GongDan No", i].Value.ToString().Trim();

                    SqlDbComm.CommandText = @"INSERT INTO M_DailyGongDanList([Batch No], [Order No], [Order Qty], [GongDan Qty], [IE Type], [Order Category], " +
                                             "[Order Currency], [Order Price], [Destination], [ESS/LINE], [PartNo], [PC Item], [GD Pending], [BOM In Customs], " +
                                             "[Created Date], [GongDan No]) VALUES(@BatchNo, @OrderNo, @OrderQty, @GongDanQty, @IEType, @OrderCategory, @OrderCurrency, " +
                                             "@OrderPrice, @Destination, @ESSLINE, @PartNo, @PCItem, @GDPending, @BOMinCustoms, @CreatedDate, @GongDanNo)";
                    SqlTransaction OleDbTrans1 = SqlDbConn.BeginTransaction();
                    SqlDbComm.Transaction = OleDbTrans1;
                    try
                    {
                        SqlDbComm.ExecuteNonQuery();
                        OleDbTrans1.Commit();
                    }
                    catch (Exception)
                    {
                        OleDbTrans1.Rollback();
                        OleDbTrans1.Dispose();
                        try
                        {
                            SqlDbComm.CommandText = @"UPDATE M_DailyGongDanList SET [Batch No] = @BatchNo, [Order No] = @OrderNo, [Order Qty] = @OrderQty, " +
                                                     "[GongDan Qty] = @GongDanQty, [IE Type] = @IEType, [Order Category] = @OrderCategory, [Order Currency] = @OrderCurrency, " +
                                                     "[Order Price] = @OrderPrice, [Destination] = @Destination, [ESS/LINE] = @ESSLINE, [PartNo] = @PartNo, " +
                                                     "[PC Item] = @PCItem, [GD Pending] = @GDPending, [BOM In Customs] = @BOMinCustoms, [Created Date] = @CreatedDate " +
                                                     "WHERE [GongDan No] = @GongDanNo";
                            SqlDbComm.ExecuteNonQuery();
                        }
                        catch (Exception) { throw; }
                    }
                    finally { SqlDbComm.Parameters.Clear(); }

                    if (!String.IsNullOrEmpty(strBatch) && !String.IsNullOrEmpty(strEss))
                    {
                        SqlDbComm.Parameters.Add("@OFInstructionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm"));
                        SqlDbComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = strOrderNo;
                        if (String.IsNullOrEmpty(strOrderQty)) { SqlDbComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = 0; }
                        else { SqlDbComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = Convert.ToInt32(strOrderQty); }
                        if (String.IsNullOrEmpty(strGDQty)) { SqlDbComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { SqlDbComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(strGDQty); }
                        SqlDbComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = strIEType;
                        SqlDbComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatch;
                        SqlDbComm.Parameters.Add("@ESSLINE", SqlDbType.NVarChar).Value = strEss;

                        SqlDbComm.CommandText = @"INSERT INTO B_OrderFulfillment([OF Instruction Date], [Order No], [Order Qty], [GongDan Qty], [IE Type], [Batch No], " +
                                                 "[ESS/LINE]) VALUES(@OFInstructionDate, @OrderNo, @OrderQty, @GongDanQty, @IEType, @BatchNo, @ESSLINE)";
                        SqlTransaction OleDbTrans2 = SqlDbConn.BeginTransaction(IsolationLevel.ReadCommitted);
                        SqlDbComm.Transaction = OleDbTrans2;
                        try
                        {
                            SqlDbComm.ExecuteNonQuery();
                            OleDbTrans2.Commit();
                        }
                        catch (Exception)
                        {
                            OleDbTrans2.Rollback();
                            OleDbTrans2.Dispose();
                            try
                            {
                                SqlDbComm.CommandText = @"UPDATE B_OrderFulfillment SET [OF Instruction Date] = @OFInstructionDate, [Order No] = @OrderNo, " +
                                                         "[Order Qty] = @OrderQty, [GongDan Qty] = @GongDanQty, [IE Type] = @IEType WHERE [Batch No] = @BatchNo " +
                                                         "AND [ESS/LINE] = @ESSLINE";
                                SqlDbComm.ExecuteNonQuery();
                            }
                            catch (Exception) { throw; }
                        }
                        finally { SqlDbComm.Parameters.Clear(); }
                    }
                    this.dgvGongDanList.Rows.RemoveAt(i);
                    i--;
                    iRowCount--;
                }            
            }
            SqlDbComm.Dispose();
            if (SqlDbConn.State == ConnectionState.Open)
            {
                SqlDbConn.Close();
                SqlDbConn.Dispose();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }

            string strSQL = @"SELECT [Batch No], [GongDan No], [Order No], [ESS/LINE], [Order Qty], [GongDan Qty], [IE Type], [Destination], [Order Price], [Order Currency], " +
                             "[Order Category], [PartNo], [PC Item], [GD Pending], [BOM In Customs], [Created Date] FROM M_DailyGongDanList ORDER BY [GongDan No]";
            SqlDataAdapter Adapter = new SqlDataAdapter(strSQL, Conn);
            DataTable myTable = new DataTable();
            Adapter.Fill(myTable);
            Adapter.Dispose();

            if (myTable.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                myTable.Clear();
                myTable.Dispose();
                Conn.Close();
                Conn.Dispose();
                this.btnBrowse.Focus();
                return;
            }

            gongdanTable.Rows.Clear();
            gongdanTable.Columns.Clear();
            gongdanTable = myTable.Copy();
            myTable.Dispose();
            gongdanTable.Columns.Add("Judge Created BOM", typeof(bool));
            gongdanTable.Columns.Add("Judge Multi-BOM", typeof(bool));
            gongdanTable.Columns.Add("Judge GD Qty", typeof(bool));

            dvFillDGV = gongdanTable.DefaultView;
            this.dgvGongDanList.DataSource = dvFillDGV;
            this.dgvGongDanList.Columns[0].HeaderText = "全选"; 
            this.dgvGongDanList.Columns["BOM In Customs"].Visible = false;
            this.dgvGongDanList.Columns["Created Date"].Visible = false;
            this.dgvGongDanList.Columns["Judge Created BOM"].Visible = false;
            this.dgvGongDanList.Columns["Judge Multi-BOM"].Visible = false;
            this.dgvGongDanList.Columns["Judge GD Qty"].Visible = false;
            this.dgvGongDanList.Columns["GongDan No"].Frozen = true;

            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }           
        }

        private void tsmiCheck1Filter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim()))
                { strFilter += " AND [Judge Created BOM] = True"; }
                else
                { strFilter = "[Judge Created BOM] = True"; }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.LightPink;
            }
        }

        private void tsmiCheck2Filter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim()))
                { strFilter += " AND [Judge Multi-BOM] = True"; }
                else
                { strFilter = "[Judge Multi-BOM] = True"; }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.Aquamarine;
            }
        }

        private void tsmiCheck3Filter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(strFilter.Trim()))
                { strFilter += " AND [Judge GD Qty] = True"; }
                else
                { strFilter = "[Judge GD Qty] = True"; }
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                DataGridViewRow dgvRow = this.dgvGongDanList.Rows.SharedRow(i);
                dgvRow.DefaultCellStyle.BackColor = Color.Khaki;
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvGongDanList.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvGongDanList.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvGongDanList.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvGongDanList[strColumnName, this.dgvGongDanList.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
                if (this.dgvGongDanList.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvGongDanList.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvGongDanList.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvGongDanList[strColumnName, this.dgvGongDanList.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvGongDanList.Columns[iColumnIndex].ValueType == typeof(DateTime))
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
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.CurrentCell != null)
            {
                string strColumnName = this.dgvGongDanList.Columns[this.dgvGongDanList.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter); 
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dvFillDGV.ToTable(true, new string[]{strColumnName});
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
                        // Just data types are characters to use "LIKE" or "NOT LIKE" to filter
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string)) 
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }                                                                 
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; } 
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }                
                    }
                    else
                    {
                        if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvGongDanList.Columns[strColumnName].ValueType == typeof(DateTime))
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvGongDanList.RowCount == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnDownload.Focus();
                return;
            }

            if (this.dgvGongDanList.Columns[0].HeaderText == "全选")
            {
                MessageBox.Show("Please select the data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.dgvGongDanList.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            int iRow = 2;
            for (int i = 0; i < this.dgvGongDanList.RowCount; i++)
            {
                if (String.Compare(this.dgvGongDanList[0, i].EditedFormattedValue.ToString(), "True") == 0)
                {
                    excel.get_Range(excel.Cells[iRow, 1], excel.Cells[iRow, this.dgvGongDanList.ColumnCount - 4]).NumberFormatLocal = "@";
                    for (int j = 4; j < this.dgvGongDanList.ColumnCount; j++)
                    { excel.Cells[iRow, j - 3] = this.dgvGongDanList[j, i].Value.ToString().Trim(); }
                    iRow++;
                }
            }

            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanList.ColumnCount - 4]).NumberFormatLocal = "@";
            for (int k = 4; k < this.dgvGongDanList.ColumnCount; k++)
            { excel.Cells[1, k - 3] = this.dgvGongDanList.Columns[k].HeaderText.ToString(); }

            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanList.ColumnCount - 4]).Font.Bold = true;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvGongDanList.ColumnCount - 4]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvGongDanList.ColumnCount - 4]).Font.Name = "Verdana";
            excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvGongDanList.ColumnCount - 4]).Font.Size = 9;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[iRow - 1, this.dgvGongDanList.ColumnCount - 4]).Borders.LineStyle = 1;
            excel.Cells.EntireColumn.AutoFit();
            excel.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }

        private void lblDownload_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 9]).NumberFormatLocal = "@";
            excel.Cells[1, 1] = "Batch No";
            excel.Cells[1, 2] = "Order No";
            excel.Cells[1, 3] = "ESS/LINE";
            excel.Cells[1, 4] = "Order Qty";
            excel.Cells[1, 5] = "GongDan Qty";
            excel.Cells[1, 6] = "IE Type";            
            excel.Cells[1, 7] = "Destination";
            excel.Cells[1, 8] = "Order Price";    
            excel.Cells[1, 9] = "Order Currency";
            excel.Cells[1, 10] = "Order Category";
            excel.Cells[1, 11] = "PartNo";
            excel.Cells[1, 12] = "PC Item";

            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 12]).Font.Bold = true;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 12]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 12]).Font.Name = "Verdana";
            excel.get_Range(excel.Cells[1, 1], excel.Cells[1, 12]).Font.Size = 9;
            excel.Cells.EntireColumn.AutoFit();
            excel.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
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

            DialogResult dlgR = MessageBox.Show("Please choose the upload condition:\n[Yes] : Insert the new data;\n[No] : Update the old data;\n[Cancel] : Reject to upload.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dlgR == DialogResult.No)
            {
                if (this.dgvGongDanList.RowCount == 0)
                {
                    MessageBox.Show("Current interface no data exist, so reject to update.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                for (int i = 0; i < myTable.Rows.Count; i++)
                {
                    string strGongDanNo = myTable.Rows[i]["GongDan No"].ToString().Trim().ToUpper();
                    string strOrderQty = myTable.Rows[i]["Order Qty"].ToString().Trim();
                    string strGongDanQty = myTable.Rows[i]["GongDan Qty"].ToString().Trim();
                    string strIEType = myTable.Rows[i]["IE Type"].ToString().Trim().ToUpper();
                    string strOrderPrice = myTable.Rows[i]["Order Price"].ToString().Trim();
                    string strPCItem = myTable.Rows[i]["PC Item"].ToString().Trim();
                    string strGDPending = myTable.Rows[i]["GD Pending"].ToString().Trim().ToUpper();

                    DataRow dr = gongdanTable.Select("[GongDan No] = '" + strGongDanNo + "'")[0];
                    dr["Batch No"] = myTable.Rows[i]["Batch No"].ToString().Trim().ToUpper();
                    dr["GongDan No"] = strGongDanNo;
                    dr["Order No"] = myTable.Rows[i]["Order No"].ToString().Trim().ToUpper();
                    dr["ESS/LINE"] = myTable.Rows[i]["ESS/LINE"].ToString().Trim();
                    if (String.IsNullOrEmpty(strOrderQty)) { dr["Order Qty"] = 0; }
                    else { dr["Order Qty"] = Convert.ToInt32(strOrderQty); }
                    if (String.IsNullOrEmpty(strGongDanQty)) { dr["GongDan Qty"] = 0; }
                    else { dr["GongDan Qty"] = Convert.ToInt32(strGongDanQty); }
                    dr["IE Type"] = strIEType;
                    dr["Destination"] = myTable.Rows[i]["Destination"].ToString().Trim().ToUpper();
                    if (String.IsNullOrEmpty(strOrderPrice)) { dr["Order Price"] = 0.0M; }
                    else { dr["Order Price"] = Math.Round(Convert.ToDecimal(strOrderPrice), 4); }
                    dr["Order Currency"] = myTable.Rows[i]["Order Currency"].ToString().Trim().ToUpper();
                    dr["Order Category"] = myTable.Rows[i]["Order Category"].ToString().Trim().ToUpper();
                    dr["PartNo"] = myTable.Rows[i]["PartNo"].ToString().Trim().ToUpper();
                    if (String.IsNullOrEmpty(strPCItem) || String.Compare(strPCItem.ToUpper(), "FALSE") == 0) { dr["PC Item"] = false; }
                    else { dr["PC Item"] = true; }
                    if (String.IsNullOrEmpty(strIEType)) { dr["GD Pending"] = true; }
                    else if (String.IsNullOrEmpty(strGDPending) || String.Compare(strGDPending, "FALSE") == 0) { dr["GD Pending"] = false; }
                    else { dr["GD Pending"] = true; }
                    dr["BOM In Customs"] = myTable.Rows[i]["BOM In Customs"].ToString().Trim().ToUpper();
                }

                MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                myTable.Dispose(); 
            }
            else if (dlgR == DialogResult.Yes)
            {
                SqlConnection ulConn = new SqlConnection(SqlLib.StrSqlConnection);
                ulConn.Open();
                SqlDataAdapter SqlAdapter = new SqlDataAdapter(@"SELECT [Batch No], [ESS/LINE] FROM B_OrderFulfillment", ulConn);
                DataTable dtableOF = new DataTable();
                SqlAdapter.Fill(dtableOF);
                SqlAdapter.Dispose();
                ulConn.Close();
                ulConn.Dispose();

                if (this.dgvGongDanList.RowCount > 0)
                { MessageBox.Show("Make sure the data grid view is empty.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
                else
                {
                    DataTable dtRecord = new DataTable();
                    dtRecord.Columns.Add("Batch No", typeof(string));
                    dtRecord.Columns.Add("ESS/LINE", typeof(string));

                    strFilter = "";
                    dvFillDGV.RowFilter = "";
                    if (myTable.Columns.Contains("GongDan No")) { myTable.Columns.Remove("GongDan No"); }
                    myTable.Columns.Add("GongDan No", typeof(string));
                    myTable.Columns["GongDan No"].SetOrdinal(1);
                    int iRowCount = myTable.Rows.Count;
                    for (int j = 0; j < iRowCount; j++)
                    {
                        string strBatchNo = myTable.Rows[j]["Batch No"].ToString().Trim().ToUpper();
                        string strEssLine = myTable.Rows[j]["ESS/LINE"].ToString().Trim().ToUpper();
                        DataRow[] dataRow = dtableOF.Select("[Batch No] = '" + strBatchNo + "' AND [ESS/LINE] = '" + strEssLine + "'");
                        if (dataRow.Length == 0)
                        {
                            string strGongDanNo = null;
                            int iLength = myTable.Select("[Batch No] = '" + strBatchNo + "'").Length;
                            if (iLength == 1) { strGongDanNo = this.GetGongDanNo(strBatchNo); }
                            else
                            {
                                int iLen = myTable.Select("[Batch No] = '" + strBatchNo + "' AND [GongDan No] IS NOT NULL").Length;
                                if (iLen == 0) { strGongDanNo = this.GetGongDanNo(strBatchNo); }
                                else
                                {
                                    DataRow dr = myTable.Select("[Batch No] = '" + strBatchNo + "'", "[GongDan No] DESC")[0];
                                    string strGDNo = dr["GongDan No"].ToString().Trim().ToUpper();
                                    int iSuffix = Convert.ToInt32(strGDNo.Split('-')[1].ToString()) + 1;
                                    strGongDanNo = strGDNo.Split('-')[0].ToString() + "-" + iSuffix.ToString();
                                }
                            }
                            myTable.Rows[j]["GongDan No"] = strGongDanNo;

                            string strOrderQty = myTable.Rows[j]["Order Qty"].ToString().Trim();
                            string strGongDanQty = myTable.Rows[j]["GongDan Qty"].ToString().Trim();
                            string strOrderPrice = myTable.Rows[j]["Order Price"].ToString().Trim();
                            if (String.IsNullOrEmpty(strOrderQty)) { myTable.Rows[j]["Order Qty"] = 0; }
                            else { myTable.Rows[j]["Order Qty"] = Convert.ToInt32(strOrderQty); }
                            if (String.IsNullOrEmpty(strGongDanQty)) { myTable.Rows[j]["GongDan Qty"] = 0; }
                            else { myTable.Rows[j]["GongDan Qty"] = Convert.ToInt32(strGongDanQty); }
                            if (String.IsNullOrEmpty(strOrderPrice)) { myTable.Rows[j]["Order Price"] = 0.0M; }
                            else { myTable.Rows[j]["Order Price"] = Math.Round(Convert.ToDecimal(strOrderPrice), 4); }
                            myTable.Rows[j]["Order No"] = myTable.Rows[j]["Order No"].ToString().Trim().ToUpper();
                        }
                        else
                        {
                            DataRow dr = dtRecord.NewRow();
                            dr[0] = strBatchNo;
                            dr[1] = strEssLine;
                            dtRecord.Rows.Add(dr);
                            myTable.Rows.RemoveAt(j);
                            j--;
                            iRowCount--;
                        }
                    }

                    gongdanTable = myTable.Copy();
                    if (gongdanTable.Columns.Contains("Created Date")) { gongdanTable.Columns.Remove("Created Date"); }
                    if (gongdanTable.Columns.Contains("GD Pending")) { gongdanTable.Columns.Remove("GD Pending"); }                  
                    if (gongdanTable.Columns.Contains("Judge Created BOM")) { gongdanTable.Columns.Remove("Judge Created BOM"); }
                    if (gongdanTable.Columns.Contains("Judge Multi-BOM")) { gongdanTable.Columns.Remove("Judge Multi-BOM"); }
                    if (gongdanTable.Columns.Contains("Judge GD Qty")) { gongdanTable.Columns.Remove("Judge GD Qty"); }

                    gongdanTable.Columns.Add("GD Pending", typeof(bool));
                    if (!gongdanTable.Columns.Contains("BOM In Customs")) { gongdanTable.Columns.Add("BOM In Customs", typeof(string)); }
                    gongdanTable.Columns.Add("Judge Created BOM", typeof(bool));
                    gongdanTable.Columns.Add("Judge Multi-BOM", typeof(bool));
                    gongdanTable.Columns.Add("Judge GD Qty", typeof(bool));

                    dvFillDGV = gongdanTable.DefaultView;
                    this.dgvGongDanList.DataSource = dvFillDGV;
                    this.dgvGongDanList.Columns["BOM In Customs"].Visible = false;
                    this.dgvGongDanList.Columns["Judge Created BOM"].Visible = false;
                    this.dgvGongDanList.Columns["Judge Multi-BOM"].Visible = false;
                    this.dgvGongDanList.Columns["Judge GD Qty"].Visible = false;
                    this.dgvGongDanList.Columns["GongDan No"].Frozen = true;

                    if (dtRecord.Rows.Count > 0)
                    {
                        PopUpInfoForm PopUpInfoFrm = new PopUpInfoForm();
                        PopUpInfoFrm.DataTableRecord = dtRecord.Copy();
                        PopUpInfoFrm.Show();
                    }
                    else { MessageBox.Show("Upload successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    dtRecord.Dispose();
                }
                dtableOF.Dispose();
                myTable.Dispose();
            }
            else
            {
                myTable.Clear();
                myTable.Dispose();
                return;
            }       
        }
    }
}