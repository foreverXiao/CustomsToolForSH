using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class GetBatchDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private delegate void ShowProgress(int ICurrent, int IToutal);

        public GetBatchDataForm()
        {
            InitializeComponent();
        }

        private static GetBatchDataForm getBatchDataFrm;
        public static GetBatchDataForm CreateInstance()
        {
            if (getBatchDataFrm == null || getBatchDataFrm.IsDisposed)
            {
                getBatchDataFrm = new GetBatchDataForm();
            }
            return getBatchDataFrm;
        }

        private void GetBatchForm_Load(object sender, EventArgs e)
        {
            this.txtDay.Text = null;
            this.txtPath.Text = null;
            this.txtMulBatch.Text = null;

            this.dgvGetBatch.Columns.Clear();
            this.dgvUploadBatch.Columns.Clear();
        }

        private void fakeLabel5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When upload BOM data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                            "\n\tActual Start Date, \n\tActual Close Date, \n\tBatch Path, \n\tBatch No (just 8 digits, without the suffix), \n\tFG No, \n\tLine No, " +
                            "\n\tItem No, \n\tLot No, \n\tInventory Type, \n\tRM Category, \n\tFG Qty (mapping 'TOTAL_OUT'), \n\tRM Qty (mapping 'RM_LOT_QTY').",
                            "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSearchBatch_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtPath.Text = openDlg.FileName;
        }

        private void btnUploadBatch_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtPath.Text.Trim()))
            {
                MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtPath.Focus();
                return;
            }

            try
            {
                this.fakeLabel4.LabelText = "Currently uploading data...";
                bool bJudge = this.txtPath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtPath.Text.Trim(), bJudge);
            }
            catch (Exception)
            {
                this.fakeLabel4.LabelText = "Upload error, please try again.";
                throw;
            }
        }

        public void ImportExcelData(string strFilePath, bool bJudge)
        {
            string strConn;
            if (bJudge)
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else
            { strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", myConn);
            DataTable myTable = new DataTable();
            myDataAdapter.Fill(myTable);                 

            if (myTable.Rows.Count == 0)
            {
                this.fakeLabel4.LabelText = " There is no data.";
                myTable.Clear();
                myTable.Dispose();
                myConn.Close();
                myConn.Dispose();
                myDataAdapter.Dispose();  
                return;
            }

            myDataAdapter = new OleDbDataAdapter("SELECT [Batch No], MAX(CInt([Line No])) AS MaxLine FROM [Sheet1$] GROUP BY [Batch No]", myConn);
            DataTable newTable = new DataTable();
            myDataAdapter.Fill(newTable);
            myDataAdapter.Dispose();   
            myConn.Close();
            myConn.Dispose();             

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            string strStat = null, strBatchNO = null;
            ArrayList tempList = new ArrayList();
            for (int j = 0; j < newTable.Rows.Count; j++)
            {
                //Abstractly query 'Batch No', since maybe exist the same 'Batch No + suffix'
                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = newTable.Rows[j][0].ToString().Trim() + "%"; 
                SqlComm.CommandText = @"SELECT [Batch No] FROM C_BOM WHERE [Batch No] LIKE @BatchNo";
                SqlDataReader SqlReader = SqlComm.ExecuteReader();
                while (SqlReader.Read())
                {
                    if (SqlReader.HasRows)
                    {
                        strBatchNO = SqlReader.GetValue(0).ToString().Trim();
                        strStat += newTable.Rows[j][0].ToString().Trim() + "(new) --> mapping --> " + SqlReader.GetValue(0).ToString().Trim() + "(system)\n"; 
                    }
                }
                SqlReader.Close();
                SqlReader.Dispose();

                if (!String.IsNullOrEmpty(strBatchNO))
                {
                    if (strBatchNO.Length > 8)
                    {
                        int iSuffix = Convert.ToInt32(strBatchNO.Substring(9, 1).ToString().Trim()) + 1;
                        strBatchNO = strBatchNO.Substring(0, 9).ToString().Trim() + iSuffix.ToString().Trim();
                    }
                    else
                    { strBatchNO = strBatchNO + "N1"; }

                    tempList.Add(newTable.Rows[j]["MaxLine"].ToString().Trim() + '/' + strBatchNO);
                    strBatchNO = String.Empty;
                }
                SqlComm.Parameters.Clear();
            }

            if (!String.IsNullOrEmpty(strStat))
            {
                string strInfo = "Below 'Batch No' already exists.\nIf choose 'OK', system will auto-generate a new 'Batch No'.\nIf choose 'Cancel', system will reject to upload and return.\n";
                if (MessageBox.Show(strInfo + strStat, "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    this.fakeLabel4.LabelText = String.Empty;
                    myTable.Clear();
                    myTable.Dispose();
                    newTable.Clear();
                    newTable.Dispose();
                    SqlComm.Dispose();
                    SqlConn.Close();
                    SqlConn.Dispose();
                    return;
                }

                string[] strList = (string[])tempList.ToArray(typeof(string));
                tempList.Clear();
                int iRowCount = 0;
                for (int m = 0; m < strList.Length; m++)
                {
                    int iQuery = Convert.ToInt32(strList[m].Split('/')[0].ToString().Trim());
                    string strQuery = strList[m].Split('/')[1].ToString().Trim();

                    for (int k = 0; k < iQuery; k++)
                    {
                        myTable.Rows[iRowCount]["Batch No"] = strQuery;
                        iRowCount++;
                    }
                }
            }

            string strBatch = null;
            for (int n = 0; n < myTable.Rows.Count; n++)
            {
                string strBatchNo = this.GetBatchFormat(myTable.Rows[n]["Batch No"].ToString().Trim());
                string strFGNo = myTable.Rows[n]["FG No"].ToString().Trim();
                if (String.Compare(strBatch, strBatchNo) != 0)
                {
                    strBatch = strBatchNo;
                    SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                    SqlComm.CommandText = @"SELECT COUNT(*) FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                    int iCount = Convert.ToInt32(SqlComm.ExecuteScalar());
                    if (iCount > 0)
                    {
                        SqlComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                        SqlComm.ExecuteNonQuery();                      
                    }
                }

                SqlComm.Parameters.Clear();
                SqlComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = myTable.Rows[n]["Actual Start Date"].ToString().Trim();
                SqlComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = myTable.Rows[n]["Actual Close Date"].ToString().Trim();
                SqlComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = myTable.Rows[n]["Batch Path"].ToString().Trim();
                SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[n]["Item No"].ToString().Trim();
                SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[n]["Lot No"].ToString().Trim();
                SqlComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = myTable.Rows[n]["Inventory Type"].ToString().Trim();
                SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = myTable.Rows[n]["RM Category"].ToString().Trim();
                SqlComm.Parameters.Add("@FGQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[n]["FG Qty"].ToString().Trim());
                SqlComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[n]["RM Qty"].ToString().Trim()), 6);
                SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                SqlComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[n]["Line No"].ToString().Trim());

                SqlComm.CommandText = @"INSERT INTO M_DailyBOM([Actual Start Date], [Actual Close Date], [Batch Path], [Item No], [Lot No], [Inventory Type], " + 
                                       "[RM Category], [FG Qty], [RM Qty], [Created Date], [Creater], [Batch No], [FG No], [Line No]) VALUES(@ActualStartDate, " +
                                       "@ActualCloseDate, @BatchPath, @ItemNo, @LotNo, @InventoryType, @RMCategory, @FGQty, @RMQty, @CreatedDate, @Creater, " +
                                       "@BatchNo, @FGNo, @LineNo)";
                SqlTransaction SqlTrans = SqlConn.BeginTransaction();
                SqlComm.Transaction = SqlTrans;
                try
                {
                    SqlComm.ExecuteNonQuery();
                    SqlTrans.Commit();
                }
                catch (Exception)
                {
                    SqlTrans.Rollback();
                    SqlTrans.Dispose();
                    throw;
                }
                finally
                { SqlComm.Parameters.Clear(); }
            }

            SqlComm.Dispose();
            newTable.Dispose();
            this.fakeLabel4.LabelText = "Data successfully uploaded.";
            this.dgvUploadBatch.DataSource = myTable;
 
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void btnGatherBatch_Click(object sender, EventArgs e)
        {
            this.btnGatherBatch.Enabled = false;
            string strSQL1 = null, strSQL2 = null, strSQL3 = null, strSQL4 = null, strSQL5 = null, strSQL6 = null, strRpck1 = null, strRpck2 = null;
            if (String.IsNullOrEmpty(this.txtMulBatch.Text.ToString())) 
            { 
                if (String.IsNullOrEmpty(this.txtDay.ToString())) 
                {
                    this.btnGatherBatch.Enabled = true;
                    return; 
                } 
            }
            else
            {
                strSQL1 = " WHERE gbh.batch_no IN ( ";
                for (int i = 0; i < this.txtMulBatch.Lines.Length; i++)
                { strSQL1 += "'" + this.txtMulBatch.Lines[i].ToString().Trim() + "',"; }
                strSQL1 = strSQL1.Remove(strSQL1.Length - 1) + ") ";
            }

            strSQL2 = " SELECT '1' lvl, '/' || gbh.batch_no Path, '' Total_RECY_RPCK, ";

            strSQL3 = " gbh.actual_start_date Start_Dt, gbh.batch_close_date Cls_Dt, " +
                      " gbh.batch_no, b2.item_no FG_No, TO_Number(gmd2.actual_qty) Total_OUT, TRUNC(TO_Number(c.Total_IN),11) Total_IN, " +
                      " b.item_no, b.inv_type, TRUNC(TO_Number(itp.RM_lot_qty),11) RM_lot_qty, '\\' || ilm.lot_no lot_no, ilm.sublot_no, " +
                      " DECODE(SUBSTR(ilm.Lot_no, 7, 1), 'B','USD', 'X','USD', 'U','USD', 'R','RMB') RM_Category, gbh.attribute3 || '/' || gbh.attribute4 OPMSO, " +
                      " gbh.batch_id, gmd.item_id, itp.lot_id FROM gme.GME_BATCH_HEADER gbh, gme.GME_MATERIAL_DETAILS gmd, gme.GME_MATERIAL_DETAILS gmd2, " +
                      " gmi.ic_item_mst_b b, gmi.ic_item_mst_b b2, gmi.ic_lots_mst ilm, (SELECT doc_id, item_id, line_id, lot_id, SUM(trans_qty * -1) as RM_lot_qty " + 
                      " FROM apps.ic_tran_vw1 itp WHERE itp.whse_code = 'SH01' AND itp.doc_type = 'PROD' AND itp.completed_ind = 1 AND itp.line_type = -1 " + 
                      " GROUP BY doc_id, item_id, line_id, lot_id HAVING SUM(trans_qty) !=0) itp, (SELECT doc_id, SUM(trans_qty * -1) Total_IN FROM apps.ic_tran_vw1 itp " + 
                      " WHERE itp.whse_code = 'SH01' AND itp.doc_type = 'PROD' AND itp.completed_ind = 1 AND itp.line_type = -1 GROUP BY doc_id) c ";

            strSQL4 = " WHERE gbh.plant_code = 'PGSH' AND gbh.batch_status = 4 AND gbh.batch_close_date > TRUNC(SYSDATE) - " + this.txtDay.Text.Trim();

            strSQL5 = " AND gbh.batch_id = c.doc_id AND gbh.batch_id = gmd.batch_id AND gmd.line_type = -1 AND gmd.actual_qty != 0 " +
                      " AND gmd.item_id = b.item_id AND gmd.batch_id = itp.doc_id(+) AND gmd.item_id = itp.item_id(+) AND gmd.material_detail_id = itp.line_id(+) " +
                      " AND itp.item_id = ilm.item_id(+) AND itp.lot_id = ilm.lot_id(+) AND gbh.batch_id = gmd2.batch_id AND gmd2.item_id = b2.item_id AND gmd2.line_type = 1 " +
                      " ORDER BY gbh.batch_no, b.inv_type DESC, b.item_no, ilm.lot_no, ilm.sublot_no "; 						


            if (String.IsNullOrEmpty(this.txtMulBatch.Text.ToString()))
            { strSQL6 = strSQL2 + strSQL3 + strSQL4 + strSQL5; }
            else
            { strSQL6 = strSQL2 + strSQL3 + strSQL1 + strSQL5; }

            OracleConnection OrcConn = new OracleConnection(SqlLib.StrOracleConnection);
            if (OrcConn.State == ConnectionState.Closed) { OrcConn.Open(); }
            OracleDataAdapter OrcAdapter = new OracleDataAdapter(strSQL6, OrcConn);
            DataTable Orcable = new DataTable();
            Orcable.Clear();
            OrcAdapter.Fill(Orcable);

            Orcable.Columns.Add("RECY_RPCK", typeof(string));
            Orcable.Columns.Add("Data_Source", typeof(string));
            Orcable.Columns.Add("Drools_Rate", typeof(decimal));

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            SqlDataAdapter SqlAdapter = new SqlDataAdapter(@"SELECT [Batch No] FROM C_BOM", SqlConn);
            DataTable mytable = new DataTable();
            SqlAdapter.Fill(mytable);
            SqlAdapter = new SqlDataAdapter(@"SELECT DISTINCT [Batch No], [FG No] FROM M_DailyBOM", SqlConn);
            DataTable mytbl = new DataTable();
            SqlAdapter.Fill(mytbl);
//            SqlAdapter = new SqlDataAdapter(@"SELECT T1.[Actual Start Date], T1.[Actual Close Date], T1.[Batch No], T1.[FG No], T1.[FG Qty], T2.[Item No], T2.[Lot No], T2.[RM Category], T2.[RM Customs Code], 
//                         T2.Consumption, T1.[Qty Loss Rate] FROM C_BOM AS T1 INNER JOIN C_BOMDetail AS T2 ON T1.[Batch No] = T2.[Batch No] WHERE T1.Freeze = 0", SqlConn);
//            DataTable dtable = new DataTable();
//            SqlAdapter.Fill(dtable);
            SqlAdapter.Dispose();         

            int iRowLine = Orcable.Rows.Count;
            string strBatchJudge = null, strSameJudge = null;
            for (int p = 0; p < iRowLine; p++)
            {
                if (String.Compare(Orcable.Rows[p][5].ToString(), strBatchJudge) != 0 &&
                    String.Compare(Orcable.Rows[p][5].ToString(), strSameJudge) != 0) 
                {
                    DataRow[] dr = mytable.Select("[Batch No] = '" + Orcable.Rows[p][5].ToString().Trim() + "'");
                    if (dr.Length > 0)
                    {
                        strBatchJudge = Orcable.Rows[p][5].ToString();
                        strSameJudge = String.Empty;
                        Orcable.Rows.RemoveAt(p);           
                        p--;
                        iRowLine--;
                    } 
                    else { strSameJudge = Orcable.Rows[p][5].ToString(); }
                }
                else
                {
                    if (String.IsNullOrEmpty(strSameJudge))
                    {
                        Orcable.Rows.RemoveAt(p); 
                        p--;
                        iRowLine--;
                    }
                }
            }
            mytable.Clear();
            mytable.Dispose();

            strRpck1 = " SELECT batch_id, item_id, lot_id, batch_no, '\\' || SUBSTR(From_lot,1,8) From_Lot, SUBSTR(From_lot,10) From_Sublot, " +
                       " ROUND(SUM(qty)) RPCK_IN_QTY FROM (SELECT gbh.batch_no, gmd.line_type, b.item_no FG_no, ilm.lot_no, ilm.sublot_no, iaj.whse_code, " + 
                       " iaj.location, iaj.trans_type, iaj.reason_code, iaj.qty, ijm.journal_no, ijm.journal_comment From_lot, gbh.batch_id, " + 
                       " b.item_id, ilm.lot_id FROM gme.gme_batch_header gbh, gme.GME_MATERIAL_DETAILS gmd, gmi.ic_item_mst_b b, gmi.ic_adjs_jnl iaj, " + 
                       " gmi.ic_jrnl_mst ijm, gmi.ic_lots_mst ilm WHERE gbh.batch_id = ";

            strRpck2 = " AND gbh.batch_id = gmd.batch_id AND gmd.item_id = b.item_id  AND gmd.line_type = 1 AND gbh.batch_no = ilm.lot_no  " +
                       " AND gmd.item_id = ilm.item_id AND ilm.item_id = iaj.item_id AND ilm.lot_id = iaj.lot_id AND iaj.trans_type = 'ADJI' " + 
                       " AND (iaj.reason_code = 'RPCK' or iaj.reason_code = 'RPK2' ) AND iaj.qty > 0 AND iaj.journal_id = ijm.journal_id ) " +
                       " GROUP BY batch_id, item_id, lot_id, batch_no, From_lot ";

            iRowLine = Orcable.Rows.Count;
            string BatchID = null;
            int iRecord = 0;
            string strRecord = null;
            for (int s = 0; s < iRowLine; s++)
            {
                #region //For each Batch, verify if any Lots have been Repacked into its Output Lots
                if (String.Compare(Orcable.Rows[s][0].ToString(), "1") == 0 && 
                    String.Compare(Orcable.Rows[s][16].ToString(), BatchID) != 0)
                {
                    BatchID = Orcable.Rows[s][16].ToString();
                    OrcAdapter = new OracleDataAdapter(strRpck1 + BatchID + strRpck2, OrcConn);
                    DataTable myTable = new DataTable();
                    myTable.Clear();
                    OrcAdapter.Fill(myTable);

                    if (myTable.Rows.Count > 0)
                    {
                        for (int k = 0; k < myTable.Rows.Count; k++)
                        {
                            //Write a Batch Input Line for the Repack Item
                            DataRow dRow = Orcable.NewRow();
                            string strPath = Orcable.Rows[s][1].ToString() + "/";
                            string strBatchNo = Orcable.Rows[s][5].ToString();
                            string strFGNO = Orcable.Rows[s][6].ToString(), strLOTNO = this.GetBatchFormat(myTable.Rows[k]["From_Lot"].ToString().Substring(1));
                            decimal dRpckQty = Convert.ToDecimal(myTable.Rows[k]["Rpck_in_qty"].ToString());                           

                            dRow[0] = 1;
                            dRow[1] = strPath; //Batch Path
                            if (String.IsNullOrEmpty(Orcable.Rows[s][2].ToString())) { dRow[2] = 0; } //Total Recycle & Repack Qty 
                            else{ dRow[2] = Convert.ToInt32(Orcable.Rows[s][2].ToString()); }
                            if (String.IsNullOrEmpty(Orcable.Rows[s][3].ToString())) { dRow[3] = DBNull.Value; }
                            else { dRow[3] = Convert.ToDateTime(Orcable.Rows[s][3].ToString()); } //Start Date
                            if (String.IsNullOrEmpty(Orcable.Rows[s][4].ToString())) { dRow[4] = DBNull.Value; }
                            else { dRow[4] = Convert.ToDateTime(Orcable.Rows[s][4].ToString()); } //Close Date
                            dRow[5] = strBatchNo; //Batch No
                            dRow[6] = strFGNO; //FG No
                            dRow[7] = Convert.ToInt32(Orcable.Rows[s][7].ToString()); //Total Output
                            dRow[8] = 0.0M; //Total Input
                            dRow[9] = dRow[6].ToString(); //Item No
                            dRow[10] = "FG"; //Inventory Type
                            dRow[11] = dRpckQty; //RM Lot Qty
                            dRow[12] = myTable.Rows[k]["From_Lot"].ToString(); //Lot No
                            dRow[13] = myTable.Rows[k]["From_Sublot"].ToString(); //Sublot --> Int32 or String 
                            dRow[14] = Orcable.Rows[s][14].ToString(); //RM Category
                            dRow[15] = Orcable.Rows[s][15].ToString(); //OPMSO
                            dRow[16] = Convert.ToInt32(Orcable.Rows[s][16].ToString()); //Batch ID
                            dRow[17] = Convert.ToInt32(myTable.Rows[k]["item_id"].ToString()); //Item ID
                            dRow[18] = Convert.ToInt32(myTable.Rows[k]["lot_id"].ToString()); //Lot ID
                            dRow[19] = "RPCK";
                            dRow[20] = String.Empty;
                            dRow[21] = 0.0M;
                            Orcable.Rows.Add(dRow);

                            //Find Repack Lot BOM.  If not found in Customs BOM DB, look up from OPM
                            if (!String.IsNullOrEmpty(strLOTNO))
                            {
                                bool bBOM = false;
                                //this.LookupBOM(out bBOM, strLOTNO, strFGNO, String.Empty, strPath, dRpckQty, "RPCK", Orcable, dtable, 0);
                                this.LookupBOM(out bBOM, strLOTNO, strFGNO, String.Empty, strPath, dRpckQty, "RPCK", Orcable, 0, SqlConn);
                                if (bBOM == false)
                                {
                                    strSQL6 = " SELECT '2' lvl, '" + strPath + strLOTNO + "' Path, " + dRpckQty + " Total_RECY_RPCK, " + strSQL3 + 
                                              " WHERE gbh.plant_code = 'PGSH' AND gbh.batch_status = 4 AND gbh.batch_no = '" + strLOTNO + "' " + strSQL5;

                                    OrcAdapter = new OracleDataAdapter(strSQL6, OrcConn);
                                    System.Data.DataTable dataTable = new System.Data.DataTable();
                                    dataTable.Clear();
                                    OrcAdapter.Fill(dataTable);

                                    if (dataTable.Rows.Count > 0)
                                    {
                                        for (int m = 0; m < dataTable.Rows.Count; m++)
                                        {
                                            decimal dRecyRpck = Convert.ToDecimal(dataTable.Rows[m][2].ToString().Trim());
                                            decimal dOutputQty = Convert.ToDecimal(dataTable.Rows[m][7].ToString().Trim());
                                            decimal dRMQty = Convert.ToDecimal(dataTable.Rows[m][11].ToString().Trim());

                                            dRow = Orcable.NewRow();
                                            dRow[0] = Convert.ToInt32(dataTable.Rows[m][0].ToString());
                                            dRow[1] = dataTable.Rows[m][1].ToString() + "&";
                                            dRow[2] = Convert.ToDecimal(dataTable.Rows[m][2].ToString());
                                            dRow[3] = dataTable.Rows[m][3].ToString();
                                            dRow[4] = dataTable.Rows[m][4].ToString();
                                            dRow[5] = strPath.Substring(1, 8);
                                            dRow[6] = strFGNO;
                                            dRow[7] = dRecyRpck;
                                            dRow[8] = Convert.ToDecimal(dataTable.Rows[m][8].ToString());
                                            dRow[9] = dataTable.Rows[m][9].ToString();
                                            dRow[10] = dataTable.Rows[m][10].ToString();
                                            dRow[11] = Math.Round(dRMQty * dRecyRpck / dOutputQty, 12);
                                            dRow[12] = dataTable.Rows[m][12].ToString();
                                            dRow[13] = dataTable.Rows[m][13].ToString();
                                            dRow[14] = dataTable.Rows[m][14].ToString();
                                            dRow[15] = dataTable.Rows[m][15].ToString();
                                            dRow[16] = Convert.ToInt32(dataTable.Rows[m][16].ToString());
                                            dRow[17] = Convert.ToInt32(dataTable.Rows[m][17].ToString());
                                            dRow[18] = Convert.ToInt32(dataTable.Rows[m][18].ToString());
                                            dRow[19] = "RPCK";
                                            dRow[20] = "OPM";
                                            dRow[21] = 0.0M;
                                            Orcable.Rows.Add(dRow);
                                        }
                                    }
                                    else
                                    {
                                        iRecord++;
                                        strRecord += "new Batch No is: " + strBatchNo + " --> contain OPM Repack Batch No: " + strLOTNO + "\n";
                                    }
                                    dataTable.Clear();
                                    dataTable.Dispose();
                                }                            
                            }
                        }
                    }
                    myTable.Clear();
                    myTable.Dispose();
                }
                #endregion

                #region //Get Recycle BOM
                if (String.Compare(Orcable.Rows[s][10].ToString(), "FG") == 0)
                {
                    //Please still use old FG No, do not use Item No to replace it. --> below strFGNO is mapping Item No, strFGNO1 is mapping FG No.
                    string strFGNO = Orcable.Rows[s][9].ToString(), strFGNO1 = Orcable.Rows[s][6].ToString();
                    string strLOTNO = Orcable.Rows[s][12].ToString().Substring(1), strPath = Orcable.Rows[s][1].ToString() + "/";
                    decimal dRecyQty = Convert.ToDecimal(Orcable.Rows[s][11].ToString());
                    int iTotalOutput = Convert.ToInt32(Orcable.Rows[s][7].ToString());

                    bool bBOM = false;
                    //this.LookupBOM(out bBOM, strLOTNO, strFGNO, strFGNO1, strPath, dRecyQty, "RECY", Orcable, dtable, iTotalOutput);
                    this.LookupBOM(out bBOM, strLOTNO, strFGNO, strFGNO1, strPath, dRecyQty, "RECY", Orcable, iTotalOutput, SqlConn);
                    if (bBOM == false) 
                    {
                        strSQL6 =  " SELECT '2' lvl, '" + strPath + strLOTNO + "' Path, " + dRecyQty + " Total_RECY_RPCK, " + strSQL3 + 
                                   " WHERE gbh.plant_code = 'PGSH' AND gbh.batch_status = 4 AND gbh.batch_no = '" + strLOTNO + "' " + strSQL5;

                        OrcAdapter = new OracleDataAdapter(strSQL6, OrcConn);
                        DataTable dTable = new DataTable();
                        dTable.Clear();
                        OrcAdapter.Fill(dTable);

                        if (dTable.Rows.Count > 0)
                        {
                            for (int z = 0; z < dTable.Rows.Count; z++)
                            {
                                DataRow dRow = Orcable.NewRow();
                                dRow[0] = Convert.ToInt32(dTable.Rows[z][0].ToString());
                                dRow[1] = dTable.Rows[z][1].ToString() + "&" + dTable.Rows[z][6].ToString();
                                dRow[2] = Convert.ToDecimal(dTable.Rows[z][2].ToString());
                                if (String.IsNullOrEmpty(dTable.Rows[z][3].ToString())) { dRow[3] = DBNull.Value; }
                                else { dRow[3] = Convert.ToDateTime(dTable.Rows[z][3].ToString()); }
                                if (String.IsNullOrEmpty(dTable.Rows[z][4].ToString())) { dRow[4] = DBNull.Value; }
                                else { dRow[4] = Convert.ToDateTime(dTable.Rows[z][4].ToString()); }
                                dRow[5] = strPath.Substring(1, 8).Trim();
                                dRow[6] = strFGNO1;
                                dRow[7] = iTotalOutput;
                                dRow[8] = Convert.ToDecimal(dTable.Rows[z][8].ToString());
                                dRow[9] = dTable.Rows[z][9].ToString();
                                dRow[10] = dTable.Rows[z][10].ToString();
                                dRow[11] = Math.Round(Convert.ToDecimal(dTable.Rows[z][11].ToString()) * Convert.ToDecimal(dRow[2].ToString()) / Convert.ToInt32(dTable.Rows[z][7].ToString()), 12);
                                dRow[12] = dTable.Rows[z][12].ToString();
                                dRow[13] = dTable.Rows[z][13].ToString();
                                dRow[14] = dTable.Rows[z][14].ToString();
                                dRow[15] = dTable.Rows[z][15].ToString();
                                dRow[16] = Convert.ToInt32(dTable.Rows[z][16].ToString());
                                dRow[17] = Convert.ToInt32(dTable.Rows[z][17].ToString());
                                dRow[18] = Convert.ToInt32(dTable.Rows[z][18].ToString());
                                dRow[19] = "RECY";
                                dRow[20] = "OPM";
                                dRow[21] = 0.0M;
                                Orcable.Rows.Add(dRow);
                            }
                        }
                        else
                        {
                            iRecord++;
                            strRecord += "new Batch No is: " + strPath.Substring(1, 8).Trim() + " --> contain OPM Recycle Batch No: " + strLOTNO + "\n";
                        }
                        dTable.Clear();
                        dTable.Dispose();
                    }
                }
                #endregion
            }
            //dtable.Clear();
            //dtable.Dispose();

            DataRow[] zeroRow = Orcable.Select("[RM_LOT_QTY] = 0");
            foreach (DataRow dr in zeroRow) { dr.Delete(); }
            Orcable.AcceptChanges();

            SqlLib sqlLib = new SqlLib();
            string[] str = { "BATCH_NO", "TOTAL_OUT", "RECY_RPCK" };
            DataTable dtbl = sqlLib.SelectDistinct(Orcable, str);
            sqlLib.Dispose(0);
            int idtbl = dtbl.Rows.Count;
            for (int i = 0; i < idtbl; i++)
            {
                string strRecyRpck = dtbl.Rows[i]["RECY_RPCK"].ToString().Trim();
                if (!String.IsNullOrEmpty(strRecyRpck))
                {
                    dtbl.Rows.RemoveAt(i);
                    i--;
                    idtbl--;
                }
            }
            dtbl.AcceptChanges();
            idtbl = dtbl.Rows.Count;
            for (int j = 0; j < idtbl; j++)
            {
                string strBatchNo = dtbl.Rows[j][0].ToString().Trim();
                DataRow[] drow = Orcable.Select("[BATCH_NO] = '" + strBatchNo + "' AND [INV_TYPE] = 'FG' AND [RECY_RPCK] = 'RPCK'");
                if (drow.Length > 0)
                {
                    int iTotalOut = Convert.ToInt32(Orcable.Compute("SUM([RM_LOT_QTY])", "[BATCH_NO] = '" + strBatchNo + "' AND [TOTAL_IN] = 0.0 AND [INV_TYPE] = 'FG' AND [RECY_RPCK] = 'RPCK'"));
                    dtbl.Rows[j][1] = Convert.ToInt32(dtbl.Rows[j][1].ToString().Trim()) + iTotalOut;
                }
                else
                {
                    dtbl.Rows.RemoveAt(j);
                    j--;
                    idtbl--;
                }
            }
            dtbl.AcceptChanges();

            iRowLine = Orcable.Rows.Count;
            for (int a = 0; a < iRowLine; a++)
            {
                if (String.Compare(Orcable.Rows[a][10].ToString(), "FG") == 0)
                {
                    Orcable.Rows.RemoveAt(a);
                    a--;
                    iRowLine--;
                }
            }

            if (dtbl.Rows.Count > 0)
            {
                foreach (DataRow rows in dtbl.Rows)
                {
                    string strBatchNo = rows[0].ToString();
                    DataRow[] dataRow = Orcable.Select("[BATCH_NO] = '" + strBatchNo + "'");
                    foreach (DataRow row in dataRow)
                    { row["TOTAL_OUT"] = Convert.ToInt32(rows[1].ToString()); }
                }
                Orcable.AcceptChanges();
            }
            dtbl.Dispose();

            DataView dView = Orcable.DefaultView;
            dView.Sort = "batch_no";
            Orcable = dView.ToTable();
            dView.Dispose();

            Orcable.Columns.RemoveAt(21); //Drools_Rate
            Orcable.Columns.RemoveAt(20); //Data_Source
            Orcable.Columns.RemoveAt(19); //RECY_RPCK
            Orcable.Columns.RemoveAt(18); //LOT_ID
            Orcable.Columns.RemoveAt(17); //ITEM_ID
            Orcable.Columns.RemoveAt(16); //BATCH_ID
            Orcable.Columns.RemoveAt(15); //OPMSO
            Orcable.Columns.RemoveAt(13); //SUBLOT_NO
            Orcable.Columns.RemoveAt(8);  //TOTAL_IN
            Orcable.Columns.RemoveAt(2);  //TOTAL_RECY_RPCK
            Orcable.Columns.RemoveAt(0);  //LVL
            Orcable.Columns.Add("Line_No", typeof(Int32));
            Orcable.Columns["Line_No"].SetOrdinal(5);

            int iSerialNo = 0;
            string strBatchNO = null;
            for (int w = 0; w < Orcable.Rows.Count; w++)
            {                
                if (String.Compare(Orcable.Rows[w][3].ToString(), strBatchNO) != 0)
                {
                    iSerialNo = 1;
                    strBatchNO = Orcable.Rows[w][3].ToString();
                }
                else { iSerialNo++; }
                Orcable.Rows[w][5] = iSerialNo;
            }

            Orcable.Columns[0].ColumnName = "Batch Path";
            Orcable.Columns[1].ColumnName = "Actual Start Date";
            Orcable.Columns[2].ColumnName = "Actual Close Date";
            Orcable.Columns[3].ColumnName = "Batch No";
            Orcable.Columns[4].ColumnName = "FG No";
            Orcable.Columns[5].ColumnName = "Line No";
            Orcable.Columns[6].ColumnName = "FG Qty";
            Orcable.Columns[7].ColumnName = "Item No";
            Orcable.Columns[8].ColumnName = "Inventory Type";
            Orcable.Columns[9].ColumnName = "RM Qty";
            Orcable.Columns[10].ColumnName = "Lot No";
            Orcable.Columns[11].ColumnName = "RM Category";
 
            foreach (DataRow dr in mytbl.Rows)
            {
                string strBN = this.GetBatchFormat(dr["Batch No"].ToString().Trim());
                string strFN = dr["FG No"].ToString().Trim();
                DataRow[] drow = Orcable.Select("[Batch No] = '" + strBN + "' AND [FG No] = '" + strFN + "'");
                if (drow.Length > 0)
                {
                    SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBN;
                    SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFN;
                    SqlComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                    SqlComm.ExecuteNonQuery();
                    SqlComm.Parameters.Clear();
                }
            }
            mytbl.Clear();
            mytbl.Dispose();

            for (int y = 0; y < Orcable.Rows.Count; y++)
            {
                SqlComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = Orcable.Rows[y][1].ToString().Trim();
                SqlComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = Orcable.Rows[y][2].ToString().Trim();
                SqlComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = Orcable.Rows[y][0].ToString().Trim();
                SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = Orcable.Rows[y][7].ToString().Trim();
                SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = Orcable.Rows[y][10].ToString().Substring(1).Trim();
                SqlComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = Orcable.Rows[y][8].ToString().Trim();
                SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = Orcable.Rows[y][11].ToString().Trim();
                SqlComm.Parameters.Add("@FGQty", SqlDbType.Int).Value = Convert.ToInt32(Orcable.Rows[y][6].ToString().Trim());
                SqlComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(Orcable.Rows[y][9].ToString().Trim()), 11);
                SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                SqlComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = loginFrm.PublicUserName;
                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Orcable.Rows[y][3].ToString().Trim();
                SqlComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = Orcable.Rows[y][4].ToString().Trim();
                SqlComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(Orcable.Rows[y][5].ToString().Trim());

                SqlComm.CommandText = @"INSERT INTO M_DailyBOM([Actual Start Date], [Actual Close Date], [Batch Path], [Item No], [Lot No], [Inventory Type], [RM Category], " +
                                       "[FG Qty], [RM Qty], [Created Date], [Creater], [Batch No], [FG No], [Line No]) VALUES(@ActualStartDate, @ActualCloseDate, " +
                                       "@BatchPath, @ItemNo, @LotNo, @InventoryType, @RMCategory, @FGQty, @RMQty, @CreatedDate, @Creater, @BatchNo, @FGNo, @LineNo)";
                SqlTransaction SqlTrans = SqlConn.BeginTransaction();
                SqlComm.Transaction = SqlTrans;
                try
                {
                    SqlComm.ExecuteNonQuery();
                    SqlTrans.Commit();
                }
                catch (Exception)
                {
                    SqlTrans.Rollback();
                    SqlTrans.Dispose();
                    throw;
                }
                finally
                { SqlComm.Parameters.Clear(); }
            }

            OrcAdapter.Dispose();
            if (OrcConn.State == ConnectionState.Open)
            {
                OrcConn.Close();
                OrcConn.Dispose();
            }

            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }

            this.btnGatherBatch.Enabled = true;                    
            Orcable.Columns["RM Category"].SetOrdinal(9);
            Orcable.Columns["Lot No"].SetOrdinal(8);
            this.dgvGetBatch.DataSource = Orcable;

            if (iRecord == 0) { MessageBox.Show("There are " + Orcable.Rows.Count.ToString() + " rows data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { MessageBox.Show("There are " + iRecord.ToString() + " abnormal Batch No that cannot generate repack/recycle data as below:\n" + strRecord, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void LookupBOM(out bool bBOM, string strLotNo, string strFGNo, string strFGNO1, string strPath, decimal dRpckQty, string strRpckRecy, DataTable table, DataTable dtable, int iTotalOut)
        {
            bBOM = false;
            if (strLotNo.Contains("-"))
            {
                strLotNo = strLotNo.Split('-')[0].Trim() + strLotNo.Split('-')[1].Trim();
                strLotNo = this.GetBatchFormat(strLotNo);
            }
            DataRow[] drow = dtable.Select("[Batch No] = '" + strLotNo + "' AND [FG No] = '" + strFGNo + "'");
            if (drow.Length > 0)
            {
                bBOM = true;
                foreach (DataRow dr in drow)
                {
                    DataRow dRow = table.NewRow();
                    dRow[0] = 2;
                    if (String.IsNullOrEmpty(strFGNO1)) { dRow[1] = strPath + strLotNo + "#"; }
                    else { dRow[1] = strPath + strLotNo + "#" + dr["FG No"].ToString(); }
                    dRow[2] = dRpckQty;
                    if (String.IsNullOrEmpty(dr["Actual Start Date"].ToString())) { dRow[3] = DBNull.Value; }
                    else { dRow[3] = Convert.ToDateTime(dr["Actual Start Date"].ToString()); }
                    if (String.IsNullOrEmpty(dr["Actual Close Date"].ToString())) { dRow[4] = DBNull.Value; }
                    else { dRow[4] = Convert.ToDateTime(dr["Actual Close Date"].ToString()); }
                    dRow[5] = strPath.Substring(1, 8).Trim();
                    if (String.IsNullOrEmpty(strFGNO1)) { dRow[6] = strFGNo; }
                    else { dRow[6] = strFGNO1; }
                    if (String.Compare(strRpckRecy, "RECY") == 0) { dRow[7] = iTotalOut; }
                    else { dRow[7] = dRpckQty; }
                    dRow[8] = 0.0M;
                    dRow[9] = dr["Item No"].ToString();
                    if (String.IsNullOrEmpty(dRow[9].ToString())) { dRow[9] = dr["RM Customs Code"].ToString(); }
                    dRow[10] = "RM";
                    dRow[11] = Math.Round(Convert.ToDecimal(dRpckQty * Convert.ToDecimal(dr["Consumption"].ToString()) / (1 - Convert.ToDecimal(dr["Qty Loss Rate"].ToString()) / 100)), 12);
                    dRow[12] = "\\" + dr["Lot No"].ToString();
                    if (String.IsNullOrEmpty(dRow[12].ToString())) { dRow[12] = dr["RM Customs Code"].ToString(); }
                    dRow[13] = String.Empty;
                    dRow[14] = dr["RM Category"].ToString();
                    dRow[15] = String.Empty;
                    dRow[16] = 0;
                    dRow[17] = 0;
                    dRow[18] = 0;
                    dRow[19] = strRpckRecy;
                    if (dRow[19].ToString() == "RECY") { dRow[6] = strFGNO1; }
                    dRow[20] = String.Empty;
                    dRow[21] = Convert.ToDecimal(dr["Qty Loss Rate"].ToString());

                    table.Rows.Add(dRow);
                }
            }
        }

        private void LookupBOM(out bool bBOM, string strLotNo, string strFGNo, string strFGNO1, string strPath, decimal dRpckQty, string strRpckRecy, DataTable table, int iTotalOut, SqlConnection sqlConn)
        {
            bBOM = false;
            if (strLotNo.Contains("-"))
            {
                strLotNo = strLotNo.Split('-')[0].Trim() + strLotNo.Split('-')[1].Trim();
                strLotNo = this.GetBatchFormat(strLotNo);
            }

            string strSQL = @"SELECT T1.[Actual Start Date], T1.[Actual Close Date], T1.[Batch No], T1.[FG No], T1.[FG Qty], T2.[Item No], T2.[Lot No], 
                              T2.[RM Category], T2.[RM Customs Code], T2.Consumption, T1.[Qty Loss Rate] FROM C_BOM AS T1 INNER JOIN C_BOMDetail AS T2 
                              ON T1.[Batch No] = T2.[Batch No] WHERE T1.Freeze = 0 AND T1.[Batch No] = '" + strLotNo + "' AND T1.[FG No] = '" + strFGNo + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(strSQL, sqlConn);
            DataTable dtable = new DataTable();
            adapter.Fill(dtable);
            adapter.Dispose();

            if (dtable.Rows.Count > 0)
            {
                bBOM = true;
                foreach (DataRow dr in dtable.Rows)
                {
                    DataRow dRow = table.NewRow();
                    dRow[0] = 2;
                    if (String.IsNullOrEmpty(strFGNO1)) { dRow[1] = strPath + strLotNo + "#"; }
                    else { dRow[1] = strPath + strLotNo + "#" + dr["FG No"].ToString(); }
                    dRow[2] = dRpckQty;
                    if (String.IsNullOrEmpty(dr["Actual Start Date"].ToString())) { dRow[3] = DBNull.Value; }
                    else { dRow[3] = Convert.ToDateTime(dr["Actual Start Date"].ToString()); }
                    if (String.IsNullOrEmpty(dr["Actual Close Date"].ToString())) { dRow[4] = DBNull.Value; }
                    else { dRow[4] = Convert.ToDateTime(dr["Actual Close Date"].ToString()); }
                    dRow[5] = strPath.Substring(1, 8).Trim();
                    if (String.IsNullOrEmpty(strFGNO1)) { dRow[6] = strFGNo; }
                    else { dRow[6] = strFGNO1; }
                    if (String.Compare(strRpckRecy, "RECY") == 0) { dRow[7] = iTotalOut; }
                    else { dRow[7] = dRpckQty; }
                    dRow[8] = 0.0M;
                    dRow[9] = dr["Item No"].ToString();
                    if (String.IsNullOrEmpty(dRow[9].ToString())) { dRow[9] = dr["RM Customs Code"].ToString(); }
                    dRow[10] = "RM";
                    dRow[11] = Math.Round(Convert.ToDecimal(dRpckQty * Convert.ToDecimal(dr["Consumption"].ToString()) / (1 - Convert.ToDecimal(dr["Qty Loss Rate"].ToString()) / 100)), 12);
                    dRow[12] = "\\" + dr["Lot No"].ToString();
                    if (String.IsNullOrEmpty(dRow[12].ToString())) { dRow[12] = dr["RM Customs Code"].ToString(); }
                    dRow[13] = String.Empty;
                    dRow[14] = dr["RM Category"].ToString();
                    dRow[15] = String.Empty;
                    dRow[16] = 0;
                    dRow[17] = 0;
                    dRow[18] = 0;
                    dRow[19] = strRpckRecy;
                    if (dRow[19].ToString() == "RECY") { dRow[6] = strFGNO1; }
                    dRow[20] = String.Empty;
                    dRow[21] = Convert.ToDecimal(dr["Qty Loss Rate"].ToString());

                    table.Rows.Add(dRow);
                }
            }
            dtable.Dispose();
        }

        private string GetBatchFormat(string strBatchNo)
        {
            bool bJudge = strBatchNo.Contains("N");
            int iLength = 0;
            if (bJudge == false) { iLength = strBatchNo.Length; }
            else { iLength = strBatchNo.Split('N')[0].Length; }
            if (iLength == 3) { strBatchNo = "00000" + strBatchNo; }
            else if (iLength == 4) { strBatchNo = "0000" + strBatchNo; }
            else if (iLength == 5) { strBatchNo = "000" + strBatchNo; }
            else if (iLength == 6) { strBatchNo = "00" + strBatchNo; }
            else if (iLength == 7) { strBatchNo = "0" + strBatchNo; }
            return strBatchNo;
        }

        private void btnSaveOPM_Click(object sender, EventArgs e)
        {
            if (this.dgvGetBatch.RowCount == 0) return;

            SqlConnection opmConn = new SqlConnection(SqlLib.StrOPMConnection);
            if (opmConn.State == ConnectionState.Closed) { opmConn.Open(); }
            SqlCommand opmComm = new SqlCommand();
            opmComm.Connection = opmConn;

            string strBatch = null;
            int iCount = 0;
            for (int i = 0; i < this.dgvGetBatch.RowCount; i++)
            {
                if (String.Compare(strBatch, this.dgvGetBatch["Batch No", i].Value.ToString().Trim()) != 0)
                {
                    strBatch = this.dgvGetBatch["Batch No", i].Value.ToString().Trim();
                    iCount = 1;
                }
                else { iCount++; }

                opmComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = this.dgvGetBatch["Actual Start Date", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@ActualCloseDate", SqlDbType.NVarChar).Value = this.dgvGetBatch["Actual Close Date", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = this.dgvGetBatch["Batch Path", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.dgvGetBatch["Item No", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.dgvGetBatch["Lot No", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@InventoryType", SqlDbType.NVarChar).Value = this.dgvGetBatch["Inventory Type", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = this.dgvGetBatch["RM Category", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@FGQty", SqlDbType.Int).Value = Convert.ToInt32(this.dgvGetBatch["FG Qty", i].Value.ToString().Trim());
                opmComm.Parameters.Add("@RMQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.dgvGetBatch["RM Qty", i].Value.ToString().Trim()), 6);
                opmComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                opmComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatch;
                opmComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = this.dgvGetBatch["FG No", i].Value.ToString().Trim();
                opmComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = iCount;

                opmComm.CommandText = @"INSERT INTO C_OPMHistory([Actual Start Date], [Actual Close Date], [Batch Path], [Item No], [Lot No], [Inventory Type], " +
                                       "[RM Category], [FG Qty], [RM Qty], [Created Date], [Batch No], [FG No], [Line No]) VALUES(@ActualStartDate, @ActualCloseDate, " +
                                       "@BatchPath, @ItemNo, @LotNo, @InventoryType, @RMCategory, @FGQty, @RMQty, @CreatedDate, @BatchNo, @FGNo, @LineNo)";
                SqlTransaction opmTrans = opmConn.BeginTransaction();
                opmComm.Transaction = opmTrans;
                try
                {
                    opmComm.ExecuteNonQuery();
                    opmTrans.Commit();
                }
                catch (Exception)
                {
                    opmTrans.Rollback();
                    opmTrans.Dispose();

                    try
                    {
                        opmComm.CommandText = @"UPDATE C_OPMHistory SET [Actual Start Date] = @ActualStartDate, [Actual Close Date] = @ActualCloseDate, " +
                                               "[Batch Path] = @BatchPath, [Item No] = @ItemNo, [Lot No] = @LotNo, [Inventory Type] = @InventoryType, " +
                                               "[RM Category] = @RMCategory, [FG Qty] = @FGQty, [RM Qty] = @RMQty, [Created Date] = @CreatedDate WHERE " +
                                               "[Batch No] = @BatchNo AND [FG No] = @FGNo AND [Line No] = @LineNo";
                        opmComm.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                }
                finally
                { opmComm.Parameters.Clear(); }
            }

            if (MessageBox.Show("Successfully save OPM data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                opmComm.Dispose();
                if (opmConn.State == ConnectionState.Open)
                {
                    opmConn.Close();
                    opmConn.Dispose();
                }
            }
        }
    }
}