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
    public partial class AssisDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private static AssisDataForm assisFrm;
        private static string[] strList = { "RM_Receiving", "eShipment", "eShipment_MTI", "eSchedule", "QA_BO", "XFZ_Inv", "Finance_Price", "Chinese_Description", "PC_Item" };

        public AssisDataForm()
        {
            InitializeComponent();
        }       

        public static AssisDataForm CreateInstance()
        {
            if (assisFrm == null || assisFrm.IsDisposed)
            {
                assisFrm = new AssisDataForm();
            }
            return assisFrm;
        }

        private void AssisDataForm_Load(object sender, EventArgs e)
        {
            this.lblPrompt.Visible = false;

            DataTable myTable = new DataTable();
            myTable.Columns.Add("UploadObj");

            DataRow myRow = myTable.NewRow();
            myRow["UploadObj"] = "";
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[0];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[1];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[2];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[3];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[4];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[5];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[6];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[7];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["UploadObj"] = strList[8];
            myTable.Rows.Add(myRow);

            this.cmbUploadObj.DataSource = myTable;
            this.cmbUploadObj.DisplayMember = this.cmbUploadObj.ValueMember = "UploadObj";
        }

        private void cmbUploadObj_Enter(object sender, EventArgs e)
        {
            this.txtFilePath.Text = String.Empty;
            this.lblPrompt.Text = String.Empty;
            this.lblPrompt.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Database File(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            this.txtFilePath.Text = openDlg.FileName;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.cmbUploadObj.SelectedIndex < 1)
            {
                MessageBox.Show("Please select the upload object.", "Prompt", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                this.cmbUploadObj.Text = null;
                this.cmbUploadObj.Focus();
                this.dgvQueryData.Columns.Clear();
                return;
            }

            if (String.IsNullOrEmpty(this.txtFilePath.Text.Trim()))
            {
                MessageBox.Show("Please select the upload path.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtFilePath.Focus();
                return;
            }
            try
            {
                this.lblPrompt.Visible = true;
                this.lblPrompt.Text = "Currently uploading data......";             

                bool bJudge = this.txtFilePath.Text.Contains(".xlsx");
                this.ImportExcelData(this.txtFilePath.Text.Trim(), bJudge);
            }
            catch (Exception)
            {
                this.lblPrompt.Text = "Upload error, please try again.";
                throw;
            }
        }

        public void ImportExcelData(string strFilePath, bool bJudge)
        {
            this.dgvQueryData.Columns.Clear();

            string strValue = this.cmbUploadObj.SelectedValue.ToString().Trim();
            string strConn;
            if (bJudge)
            { strConn = @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + strFilePath + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }  // Excel2007--2010 Version
            else
            { strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }  // Excel1997--2003 Version

            OleDbConnection myConn = new OleDbConnection(strConn);
            myConn.Open();
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", myConn);
            DataTable myTable = new DataTable();
            myDataAdapter.Fill(myTable);
            myDataAdapter.Dispose();
            myConn.Close();
            myConn.Dispose();
        
            if (myTable.Rows.Count > 0)  
            {
                this.DeleteTableData(strValue);  //Clear related table before upload

                SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
                SqlCommand SqlComm = new SqlCommand();
                SqlComm.Connection = SqlConn;

                if (strValue == strList[0])
                {
                    /*------ Monitor And Control Multiple Users ------*/
                    SqlComm.CommandText = @"SELECT [UserName] FROM B_MonitorMultipleUsers";
                    string strUserName = Convert.ToString(SqlComm.ExecuteScalar());
                    if (!String.IsNullOrEmpty(strUserName))
                    {
                        if (String.Compare(strUserName.Trim().ToUpper(), loginFrm.PublicUserName.Trim().ToUpper()) != 0)
                        {
                            MessageBox.Show(strUserName + " is handling RM Balance/Drools Balance data. Please wait a moment.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            SqlComm.Dispose();
                            SqlComm.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        SqlComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = loginFrm.PublicUserName.ToUpper();
                        SqlComm.CommandText = @"INSERT INTO B_MonitorMultipleUsers([UserName]) VALUES(@UserName)";
                        SqlComm.ExecuteNonQuery();
                        SqlComm.Parameters.RemoveAt("@UserName");
                    }

                    SqlDataAdapter SqlAdapter = new SqlDataAdapter(@"SELECT [Item No], [Lot No] FROM C_RMPurchase", SqlConn);
                    DataTable dtRMPurchase = new DataTable();
                    SqlAdapter.Fill(dtRMPurchase);
                    SqlAdapter.Dispose();

                    DataTable dtRecord = new DataTable();
                    dtRecord.Columns.Add("Item No", typeof(string));
                    dtRecord.Columns.Add("Lot No", typeof(string));
                    int iRecord = 0;
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        string strRMCustomsCode = myTable.Rows[i]["RM Customs Code"].ToString().Trim().ToUpper();
                        string strBGDNo = myTable.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                        if (!String.IsNullOrEmpty(strRMCustomsCode) && !String.IsNullOrEmpty(strBGDNo))
                        {
                            string strItemNo = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                            string strLotNo = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                            DataRow[] dRow = dtRMPurchase.Select("[Item No] = '" + strItemNo + "' AND [Lot No] = '" + strLotNo + "'");
                            if (dRow.Length == 1)
                            {
                                DataRow dr = dtRecord.NewRow();
                                dr[0] = strItemNo;
                                dr[1] = strLotNo;
                                dtRecord.Rows.Add(dr);
                                iRecord++;
                            }
                            else
                            {
                                string strCustomsRcvdDate = myTable.Rows[i]["Customs Rcvd Date"].ToString().Trim();
                                SqlComm.Parameters.Clear();
                                SqlComm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Transaction Type"].ToString().Trim().ToUpper();
                                SqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                SqlComm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Customs Entry No"].ToString().Trim().ToUpper();
                                SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                                SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                                SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                                SqlComm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM CHN Name"].ToString().Trim().ToUpper();
                                if (String.IsNullOrEmpty(myTable.Rows[i]["PO Invoice Qty"].ToString().Trim()))
                                { SqlComm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = 0.0M; }
                                else
                                { SqlComm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["PO Invoice Qty"].ToString().Trim()), 6); }
                                if (String.IsNullOrEmpty(myTable.Rows[i]["Amount"].ToString().Trim()))
                                { SqlComm.Parameters.Add("@Amount", SqlDbType.Decimal).Value = 0.0M; }
                                else
                                { SqlComm.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["Amount"].ToString().Trim()), 2); }
                                if (String.IsNullOrEmpty(myTable.Rows[i]["RM Price(CIF)"].ToString().Trim()))
                                { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                                else
                                { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["RM Price(CIF)"].ToString().Trim()), 6); }
                                SqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Currency"].ToString().Trim();
                                SqlComm.Parameters.Add("@OriginalCountry", SqlDbType.NVarChar).Value = myTable.Rows[i]["Original Country"].ToString().Trim().ToUpper();
                                if (String.IsNullOrEmpty(myTable.Rows[i]["OPM Rcvd Date"].ToString().Trim()))
                                { SqlComm.Parameters.Add("@OPMRcvdDate", SqlDbType.DateTime).Value = DBNull.Value; }
                                else
                                { SqlComm.Parameters.Add("@OPMRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[i]["OPM Rcvd Date"].ToString().Trim()); }
                                SqlComm.Parameters.Add("@CustomsRcvdDate", SqlDbType.NVarChar).Value = strCustomsRcvdDate;
                                SqlComm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = myTable.Rows[i]["PO No"].ToString().Trim().ToUpper();
                                SqlComm.Parameters.Add("@VoucherID", SqlDbType.NVarChar).Value = myTable.Rows[i]["Voucher ID"].ToString().Trim().ToUpper();
                                SqlComm.Parameters.Add("@VoucherStatus", SqlDbType.NVarChar).Value = myTable.Rows[i]["Voucher Status"].ToString().Trim().ToUpper();
                                if (String.IsNullOrEmpty(myTable.Rows[i]["Gate PassThrough Date"].ToString().Trim()))
                                { SqlComm.Parameters.Add("@GatePassThroughDate", SqlDbType.DateTime).Value = DBNull.Value; }
                                else
                                { SqlComm.Parameters.Add("@GatePassThroughDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[i]["Gate PassThrough Date"].ToString().Trim()); }
                                SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[i]["Remark"].ToString().Trim().ToUpper();
                                if (String.IsNullOrEmpty(myTable.Rows[i]["Created Date"].ToString().Trim()))
                                { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy")); }
                                else { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[i]["Created Date"].ToString().Trim()); }

                                SqlComm.CommandText = @"INSERT INTO C_RMPurchase([Transaction Type], [BGD No], [Customs Entry No], [Item No], [Lot No], [RM Customs Code], " +
                                                       "[RM CHN Name], [PO Invoice Qty], [Amount], [RM Price(CIF)], [RM Currency], [Original Country], [OPM Rcvd Date], " +
                                                       "[Customs Rcvd Date], [PO No], [Voucher ID], [Voucher Status], [Gate PassThrough Date], [Remark], [Created Date]) " +
                                                       "VALUES(@TransactionType, @BGDNo, @CustomsEntryNo, @ItemNo, @LotNo, @RMCustomsCode, @RMCHNName, @POInvoiceQty, " +
                                                       "@Amount, @RMPrice, @RMCurrency, @OriginalCountry, @OPMRcvdDate, @CustomsRcvdDate, @PONo, @VoucherID, @VoucherStatus, " + 
                                                       "@GatePassThroughDate, @Remark, @CreatedDate)";

                                SqlTransaction SqlTran = SqlConn.BeginTransaction();
                                SqlComm.Transaction = SqlTran;

                                try
                                {
                                    SqlComm.ExecuteNonQuery();
                                    SqlTran.Commit();
                                }
                                catch (Exception)
                                {
                                    SqlComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                                    SqlComm.ExecuteNonQuery();

                                    SqlTran.Rollback();
                                    SqlTran.Dispose();
                                    throw;
                                }
                                finally { SqlComm.Parameters.Clear(); }

                                if (strCustomsRcvdDate.Contains("/")) //Make sure "Customs Rcvd Date" has date; If not, nothing to do in RM Balance table
                                {
                                    //Insert or Update C_RMBalance table's columns
                                    decimal dPOQuantity = Math.Round(Convert.ToDecimal(myTable.Rows[i]["PO Invoice Qty"].ToString()), 6);
                                    SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                                    SqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                                    SqlComm.CommandText = @"SELECT COUNT(*) FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                                    int iBalanceCount = Convert.ToInt32(SqlComm.ExecuteScalar());

                                    if (iBalanceCount == 0)
                                    {
                                        SqlComm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dPOQuantity;
                                        SqlComm.CommandText = @"INSERT INTO C_RMBalance([RM Customs Code], [BGD No], [Customs Balance], [Available RM Balance], " +
                                                               "[PO Invoice Qty]) VALUES(@RMCustomsCode, @BGDNo, @CustomsBalance, @CustomsBalance, @CustomsBalance)";

                                        SqlTransaction SqlTrans1 = SqlConn.BeginTransaction();
                                        SqlComm.Transaction = SqlTrans1;

                                        try
                                        {
                                            SqlComm.ExecuteNonQuery();
                                            SqlTrans1.Commit();
                                        }
                                        catch (Exception)
                                        {
                                            SqlComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                                            SqlComm.ExecuteNonQuery();

                                            SqlTrans1.Rollback();
                                            SqlTrans1.Dispose();                                         
                                            throw;
                                        }
                                        finally { SqlComm.Parameters.Clear(); }
                                    }
                                    else
                                    {
                                        decimal dCustomsBalance = 0.0M, dAvailableRMBalance = 0.0M, dPOInvoiceQty = 0.0M;
                                        SqlComm.CommandText = @"SELECT [Customs Balance], [Available RM Balance], [PO Invoice Qty] FROM C_RMBalance WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";
                                        SqlDataReader SqlReader = SqlComm.ExecuteReader();
                                        while (SqlReader.Read())
                                        {
                                            if (SqlReader.HasRows)
                                            {
                                                dCustomsBalance = Convert.ToDecimal(SqlReader.GetValue(0).ToString().Trim());
                                                dAvailableRMBalance = Convert.ToDecimal(SqlReader.GetValue(1).ToString().Trim());
                                                dPOInvoiceQty = Convert.ToDecimal(SqlReader.GetValue(2).ToString().Trim());
                                            }
                                        }
                                        SqlReader.Close();
                                        SqlReader.Dispose();

                                        SqlComm.Parameters.Clear();
                                        SqlComm.Parameters.Add("@CustomsBalance", SqlDbType.Decimal).Value = dCustomsBalance + dPOQuantity;
                                        SqlComm.Parameters.Add("@AvailableRMBalance", SqlDbType.Decimal).Value = dAvailableRMBalance + dPOQuantity;
                                        SqlComm.Parameters.Add("@POInvoiceQty", SqlDbType.Decimal).Value = dPOInvoiceQty + dPOQuantity;
                                        SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = strRMCustomsCode;
                                        SqlComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;

                                        SqlComm.CommandText = @"UPDATE C_RMBalance SET [Customs Balance] = @CustomsBalance, [Available RM Balance] = @AvailableRMBalance, " + 
                                                               "[PO Invoice Qty] = @POInvoiceQty WHERE [RM Customs Code] = @RMCustomsCode AND [BGD No] = @BGDNo";

                                        SqlTransaction SqlTrans2 = SqlConn.BeginTransaction();
                                        SqlComm.Transaction = SqlTrans2;

                                        try
                                        {
                                            SqlComm.ExecuteNonQuery();
                                            SqlTrans2.Commit();
                                        }
                                        catch (Exception)
                                        {
                                            SqlComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                                            SqlComm.ExecuteNonQuery();

                                            SqlTrans2.Rollback();
                                            SqlTrans2.Dispose();
                                            throw;
                                        }
                                        finally { SqlComm.Parameters.Clear(); }
                                    }
                                }
                            }
                        }
                    }
                    dtRMPurchase.Clear();
                    dtRMPurchase.Dispose();

                    if(iRecord > 0)
                    {
                        PopUpInfoForm PopUpInfoFrm = new PopUpInfoForm();
                        PopUpInfoFrm.DataTableRecord = dtRecord.Copy();
                        PopUpInfoFrm.Show();
                    }
                    dtRecord.Dispose();
                    SqlComm.CommandText = @"DELETE FROM B_MonitorMultipleUsers";
                    SqlComm.ExecuteNonQuery();
                }

                else if (strValue == strList[1])
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        SqlComm.Parameters.Add("@OPMSO", SqlDbType.NVarChar).Value = myTable.Rows[i]["OPM SO"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@SHASO", SqlDbType.NVarChar).Value = myTable.Rows[i]["SHA SO"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@CustPO", SqlDbType.NVarChar).Value = myTable.Rows[i]["CustPO"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@PartNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["PartNo"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["LotNo"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = myTable.Rows[i]["Curr"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Order Qty"].ToString().Trim())) { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Order Qty"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["External Price"].ToString().Trim())) { SqlComm.Parameters.Add("@ExternalPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@ExternalPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["External Price"].ToString().Trim()), 4); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Transfer Price"].ToString().Trim())) { SqlComm.Parameters.Add("@TransferPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@TransferPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["Transfer Price"].ToString().Trim()), 4); }
                        SqlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = myTable.Rows[i]["Destination"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        SqlComm.CommandText = @"INSERT INTO A_eShipment([OPM SO], [SHA SO], [CustPO], [PartNo], [LotNo], [Curr], [Order Qty], [External Price], " +
                                               "[Transfer Price], [Destination], [Created Date]) VALUES(@OPMSO, @SHASO, @CustPO, @PartNo, @LotNo, @Currency, " +
                                               "@OrderQty, @ExternalPrice, @TransferPrice, @Destination, @CreatedDate)";
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
                }

                else if (strValue == strList[2])
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        SqlComm.Parameters.Add("@OPMSO", SqlDbType.NVarChar).Value = myTable.Rows[i]["OPM SO"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@SHASO", SqlDbType.NVarChar).Value = myTable.Rows[i]["SHA SO"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@CustPO", SqlDbType.NVarChar).Value = myTable.Rows[i]["CustPO"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@PartNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["PartNo"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["LotNo"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = myTable.Rows[i]["Curr"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@GongDanType", SqlDbType.NVarChar).Value = myTable.Rows[i]["GongDan Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = myTable.Rows[i]["Destination"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Order Qty"].ToString().Trim())) { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Order Qty"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["GongDan Qty"].ToString().Trim())) { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                        else { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["GongDan Qty"].ToString().Trim()); }
                        SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        SqlComm.CommandText = @"INSERT INTO A_eShipment_MTI([OPM SO], [SHA SO], [CustPO], [PartNo], [LotNo], [Curr], [GongDan Type], " +
                                               "[Destination], [Order Qty], [GongDan Qty], [Created Date]) VALUES(@OPMSO, @SHASO, @CustPO, @PartNo, " +
                                               "@LotNo, @Currency, @GongDanType, @Destination, @OrderQty, @GongDanQty, @CreatedDate)";
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
                }

                else if (strValue == strList[3])
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["txt_item_no"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["txt_lot_no"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@OrderKey", SqlDbType.NVarChar).Value = myTable.Rows[i]["txt_order_key"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LocalSO", SqlDbType.NVarChar).Value = myTable.Rows[i]["txt_local_so"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = myTable.Rows[i]["txt_currency"].ToString().Trim().ToUpper();
                        string strPlannedProQty = myTable.Rows[i]["planned_production_qty"].ToString().Trim();
                        if (String.IsNullOrEmpty(strPlannedProQty)) { SqlComm.Parameters.Add("@PlannedProductQty", SqlDbType.Int).Value = 0; }
                        else
                        {
                            string strFltOrderQty = myTable.Rows[i]["flt_order_qty"].ToString().Trim();
                            if (String.IsNullOrEmpty(strFltOrderQty) || Convert.ToInt32(strFltOrderQty) > Convert.ToInt32(strPlannedProQty))
                            { SqlComm.Parameters.Add("@PlannedProductQty", SqlDbType.Int).Value = Convert.ToInt32(strPlannedProQty); }
                            else { SqlComm.Parameters.Add("@PlannedProductQty", SqlDbType.Int).Value = Convert.ToInt32(strFltOrderQty); }
                        }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["dat_finish_date"].ToString().Trim())) { SqlComm.Parameters.Add("@FinishDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else { SqlComm.Parameters.Add("@FinishDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[i]["dat_finish_date"].ToString().Trim()); }
                        SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[i]["txt_remark"].ToString().Trim();
                        SqlComm.Parameters.Add("@IntStatusKey", SqlDbType.NVarChar).Value = myTable.Rows[i]["int_status_key"].ToString().Trim();
                        SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        SqlComm.CommandText = @"INSERT INTO A_eSchedule([txt_item_no], [txt_lot_no], [txt_order_key], [txt_local_so], [txt_currency], " +
                                               "[planned_production_qty], [dat_finish_date], [txt_remark], [int_status_key], [Created Date]) VALUES(@ItemNo, " +
                                               "@LotNo, @OrderKey, @LocalSO, @Currency, @PlannedProductQty, @FinishDate, @Remark, @IntStatusKey, @CreatedDate)";
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
                }

                else if (strValue == strList[4])
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        SqlComm.Parameters.Add("@WhseCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["Whse Code"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@GIClass", SqlDbType.NVarChar).Value = myTable.Rows[i]["GI Class"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@InvType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Inv Type"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Item No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot No"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@SublotNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["Sublot No"].ToString().Trim();
                        SqlComm.Parameters.Add("@Location", SqlDbType.NVarChar).Value = myTable.Rows[i]["Location"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@LotStatus", SqlDbType.NVarChar).Value = myTable.Rows[i]["Lot Status"].ToString().Trim().ToUpper();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Onhand Qty"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@OnhandQty", SqlDbType.Int).Value = 0; }
                        else
                        { SqlComm.Parameters.Add("@OnhandQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Onhand Qty"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Trans Qty"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@TransQty", SqlDbType.Int).Value = 0; }
                        else
                        { SqlComm.Parameters.Add("@TransQty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["Trans Qty"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Move In Date"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@MoveInDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else
                        { SqlComm.Parameters.Add("@MoveInDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[i]["Move In Date"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Lot Created"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@LotCreated", SqlDbType.DateTime).Value = DBNull.Value; }
                        else
                        { SqlComm.Parameters.Add("@LotCreated", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[i]["Lot Created"].ToString().Trim()); }
                        SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        SqlComm.CommandText = @"INSERT INTO A_QA_BO([Whse Code], [GI Class], [Inv Type], [Item No], [Lot No], [Sublot No], [Location], [Lot Status], " +
                                               "[Onhand Qty], [Trans Qty], [Move In Date], [Lot Created], [Created Date]) VALUES(@WhseCode, @GIClass, @InvType, " +
                                               "@ItemNo, @LotNo, @SublotNo, @Location, @LotStatus, @OnhandQty, @TransQty, @MoveInDate, @LotCreated, @CreatedDate)";
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
                }

                else if (strValue == strList[5])
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        if (String.IsNullOrEmpty(myTable.Rows[i]["上架日期"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@GroundingDate", SqlDbType.DateTime).Value = DBNull.Value; }
                        else
                        { SqlComm.Parameters.Add("@GroundingDate", SqlDbType.DateTime).Value = Convert.ToDateTime(myTable.Rows[i]["上架日期"].ToString().Trim()); }
                        SqlComm.Parameters.Add("@StorageInstruction", SqlDbType.NVarChar).Value = myTable.Rows[i]["入库指令号"].ToString().Trim();
                        SqlComm.Parameters.Add("@GoodsNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["货号"].ToString().Trim();
                        SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["批号"].ToString().Trim();
                        SqlComm.Parameters.Add("@SecondBatchNo", SqlDbType.NVarChar).Value = myTable.Rows[i]["次批号"].ToString().Trim();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["数量"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@Qty", SqlDbType.Int).Value = 0; }
                        else
                        { SqlComm.Parameters.Add("@Qty", SqlDbType.Int).Value = Convert.ToInt32(myTable.Rows[i]["数量"].ToString().Trim()); }
                        if (String.IsNullOrEmpty(myTable.Rows[i]["净重"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@NetWeight", SqlDbType.Decimal).Value = 0.0M; }
                        else
                        { SqlComm.Parameters.Add("@NetWeight", SqlDbType.Decimal).Value = Convert.ToDecimal(myTable.Rows[i]["净重"].ToString().Trim()); }
                        SqlComm.Parameters.Add("@StorageLocation", SqlDbType.NVarChar).Value = myTable.Rows[i]["储位代码"].ToString().Trim();
                        SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[i]["备注"].ToString().Trim();
                        SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        SqlComm.CommandText = @"INSERT INTO A_XFZ_Inv([上架日期], [入库指令号], [货号], [批号], [次批号], [数量], [净重], [储位代码], [备注], [Created Date]) " +
                                               "VALUES(@GroundingDate, @StorageInstruction, @GoodsNo, @BatchNo, @SecondBatchNo, @Qty, @NetWeight, @StorageLocation, @Remark, @CreatedDate)";
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
                }

                else if (strValue == strList[6])
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = myTable.Rows[i]["Grade"].ToString().Trim();
                        SqlComm.Parameters.Add("@NewProductGroup", SqlDbType.NVarChar).Value = myTable.Rows[i]["New Product Group"].ToString().Trim();
                        SqlComm.Parameters.Add("@NewProductFamily", SqlDbType.NVarChar).Value = myTable.Rows[i]["New Product Family"].ToString().Trim();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["Price"].ToString().Trim()))
                        { SqlComm.Parameters.Add("@Price", SqlDbType.Decimal).Value = 0.0M; }
                        else
                        { SqlComm.Parameters.Add("@Price", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["Price"].ToString().Trim()), 2); }
                        SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                        SqlComm.CommandText = @"INSERT INTO B_FinancePrice([Grade], [New Product Group], [New Product Family], [Price], [Created Date]) " +
                                               "VALUES(@Grade, @NewProductGroup, @NewProductFamily, @Price, @CreatedDate)";
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
                }

                else if (strValue == strList[7])
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM Customs Code"].ToString().Trim();
                        SqlComm.Parameters.Add("@GoodsType", SqlDbType.NVarChar).Value = myTable.Rows[i]["Goods Type"].ToString().Trim();
                        if (String.IsNullOrEmpty(myTable.Rows[i]["RM Price"].ToString().Trim())) { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                        else { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(myTable.Rows[i]["RM Price"].ToString().Trim()), 6); }
                        SqlComm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = myTable.Rows[i]["Currency"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = myTable.Rows[i]["RM CHN Name"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@CHNDescription", SqlDbType.NVarChar).Value = myTable.Rows[i]["CHN Description"].ToString().Trim().ToUpper();

                        SqlComm.CommandText = @"INSERT INTO B_ChnDescription([RM Customs Code], [Goods Type], [RM Price], [Currency], [RM CHN Name], [CHN Description]) " +
                                               "VALUES(@RMCustomsCode, @GoodsType, @RMPrice, @Currency, @RMCHNName, @CHNDescription)";
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
                }

                else
                {
                    for (int i = 0; i < myTable.Rows.Count; i++)
                    {
                        SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = myTable.Rows[i]["Grade"].ToString().Trim().ToUpper();
                        SqlComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = myTable.Rows[i]["CHN Name"].ToString().Trim();
                        SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = myTable.Rows[i]["Remark"].ToString().Trim().ToUpper();

                        SqlComm.CommandText = @"INSERT INTO B_PCItem([Grade], [CHN Name], [Remark]) VALUES(@Grade, @CHNName, @Remark)";
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
                }

                SqlComm.Dispose();
                if (SqlConn.State == ConnectionState.Open) 
                {
                    SqlConn.Close();
                    SqlConn.Dispose();
                }

                this.lblPrompt.Text = "Data successfully uploaded.";
            }
            else
            { this.lblPrompt.Text = "There is no data."; }

            myTable.Clear();
            myTable.Dispose();
        }

        public void DeleteTableData(string strObj)
        {
            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            if (strObj == strList[1])
            { SqlComm.CommandText = @"DELETE FROM A_eShipment"; }

            else if (strObj == strList[2])
            { SqlComm.CommandText = @"DELETE FROM A_eShipment_MTI"; }

            else if (strObj == strList[3])
            { SqlComm.CommandText = @"DELETE FROM A_eSchedule"; }

            else if (strObj == strList[4])
            { SqlComm.CommandText = @"DELETE FROM A_QA_BO"; }

            else if (strObj == strList[5])
            { SqlComm.CommandText = @"DELETE FROM A_XFZ_Inv"; }

            else if (strObj == strList[6])
            { SqlComm.CommandText = @"DELETE FROM B_FinancePrice"; }

            else if (strObj == strList[7])
            { SqlComm.CommandText = @"DELETE FROM B_ChnDescription"; }

            else { SqlComm.CommandText = @"DELETE FROM B_PCItem"; }

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
            {
                SqlComm.Parameters.Clear();
                SqlComm.Dispose();
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (this.cmbUploadObj.SelectedIndex < 1)
            {
                MessageBox.Show("Please select the upload object first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cmbUploadObj.Focus();
                return;
            }

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }

            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            string strValue = this.cmbUploadObj.SelectedValue.ToString().Trim();
            if (strValue == strList[0])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM C_RMPurchase"; }

            else if (strValue == strList[1])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM A_eShipment"; }

            else if (strValue == strList[2])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM A_eShipment_MTI"; }

            else if (strValue == strList[3])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM A_eSchedule"; }

            else if (strValue == strList[4])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM A_QA_BO"; }

            else if (strValue == strList[5])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM A_XFZ_Inv"; }

            else if (strValue == strList[6])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM B_FinancePrice"; }

            else if (strValue == strList[7])
            { SqlComm.CommandText = @"SELECT COUNT(*) FROM B_ChnDescription"; }

            else { SqlComm.CommandText = @"SELECT COUNT(*) FROM B_PCItem"; }

            int iCount = Convert.ToInt32(SqlComm.ExecuteScalar());
            if (iCount > 0)
            {
                if (this.lblPrompt.Visible == false) { this.lblPrompt.Visible = true; }
                this.lblPrompt.Text = @"Today successfully uploaded " + iCount.ToString() + " rows of data.";

                if (strValue == strList[0])
                { SqlComm.CommandText = @"SELECT * FROM C_RMPurchase"; }

                else if (strValue == strList[1])
                { SqlComm.CommandText = @"SELECT * FROM A_eShipment"; }

                else if (strValue == strList[2])
                { SqlComm.CommandText = @"SELECT * FROM A_eShipment_MTI"; }

                else if (strValue == strList[3])
                { SqlComm.CommandText = @"SELECT * FROM A_eSchedule"; }

                else if (strValue == strList[4])
                { SqlComm.CommandText = @"SELECT * FROM A_QA_BO"; }

                else if (strValue == strList[5])
                { SqlComm.CommandText = @"SELECT * FROM A_XFZ_Inv"; }

                else if (strValue == strList[6])
                { SqlComm.CommandText = @"SELECT * FROM B_FinancePrice"; }

                else if (strValue == strList[7])
                { SqlComm.CommandText = @"SELECT * FROM B_ChnDescription"; }

                else { SqlComm.CommandText = @"SELECT * FROM B_PCItem"; }

                SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                SqlAdapter.SelectCommand = SqlComm;
                DataSet dataSet = new DataSet();
                SqlAdapter.Fill(dataSet);
                SqlAdapter.Dispose();
                this.dgvQueryData.DataSource = dataSet.Tables[0];
            }
            else
            {
                if (this.lblPrompt.Visible == false) { this.lblPrompt.Visible = true; } 
                this.lblPrompt.Text = @"Today there are no upload data.";
            }

            SqlComm.Dispose();           
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void llinkPrompt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {           
            if (this.cmbUploadObj.SelectedIndex < 1)
            {
                MessageBox.Show("Please select the upload object.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbUploadObj.Text = null;
                this.cmbUploadObj.Focus();
                return;
            }

            string strObject = this.cmbUploadObj.SelectedValue.ToString().Trim();
            if (strObject == strList[0])
            {
                MessageBox.Show("When upload RM_Receiving data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tTransaction Type, \n\tBGD No, \n\tCustoms Entry No, \n\tRM CHN Name, \n\tItem No, \n\tLot No, \n\tRM Customs Code, \n\tPO Invoice Qty, " +
                                "\n\tAmount, \n\tRM Price(CIF), \n\tRM Currency, \n\tOriginal Country, \n\tOPM Rcvd Date, \n\tCustoms Rcvd Date, \n\tPO No, \n\tVoucher ID, " + 
                                "\n\tVoucher Status, \n\tGate PassThrough Date, \n\tCreated Date, \n\tRemark", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (strObject == strList[1])
            {
                MessageBox.Show("When upload eShipment data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tOPM SO, \n\tSHA SO, \n\tCustPO, \n\tPartNo, \n\tLotNo, \n\tCurr, \n\tOrder Qty, \n\tExternal Price, \n\tTransfer Price, \n\tDestination", 
                                "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (strObject == strList[2])
            {
                MessageBox.Show("When upload eShipment_MTI data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tOPM SO, \n\tSHA SO, \n\tCustPO, \n\tPartNo, \n\tLotNo, \n\tCurr, \n\tGongDan Type, \n\tDestination, \n\tOrder Qty, \n\tGongDan Qty", 
                                "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (strObject == strList[3])
            {
                MessageBox.Show("When upload eSchedule data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\ttxt_item_no, \n\ttxt_lot_no, \n\ttxt_order_key, \n\ttxt_local_so, \n\ttxt_currency, \n\tflt_order_qty, " +
                                "\n\tplanned_production_qty, \n\tdat_finish_date, \n\ttxt_remark, \n\tint_status_key", "Prompt", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (strObject == strList[4])
            {
                MessageBox.Show("When upload QA_BO data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tWhse Code, \n\tGI Class, \n\tInv Type, \n\tItem No, \n\tLot No, \n\tSublot No, \n\tLocation, \n\tLot Status, " +
                                "\n\tOnhand Qty, \n\tTrans Qty, \n\tMove In Date, \n\tLot Created", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (strObject == strList[5])
            {
                MessageBox.Show("When upload XFZ_Inv data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\t上架日期, \n\t入库指令号, \n\t货号, \n\t批号, \n\t次批号, \n\t数量, \n\t净重, \n\t储位代码, \n\t备注", "Prompt", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (strObject == strList[6])
            {
                MessageBox.Show("When upload Finance_Price data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tGrade, \n\tNew Product Group, \n\tNew Product Family, \n\tPrice", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (strObject == strList[7])
            {
                MessageBox.Show("When upload Chinese_Description data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tRM Customs Code, \n\tGoods Type, \n\tRM Price, \n\tCurrency ,\n\tRM CHN Name, \n\tCHN Description", "Prompt",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                MessageBox.Show("When upload PC_Item data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tGrade, \n\tCHN Name, \n\tRemark", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return;
        }
    }
}