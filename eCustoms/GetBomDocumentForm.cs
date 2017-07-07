using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class GetBomDocumentForm : Form
    {
        private LoginForm loginFrm = new LoginForm(); 
        private DataTable publicTable;
        private DataTable middleTable;
        private static SaveFileDialog saveFileDialog = new SaveFileDialog();
             
        public GetBomDocumentForm()
        {
            InitializeComponent();
        }

        private static GetBomDocumentForm getCustomsBomFrm;
        public static GetBomDocumentForm CreateInstance()
        {
            if (getCustomsBomFrm == null || getCustomsBomFrm.IsDisposed)
            {
                getCustomsBomFrm = new GetBomDocumentForm();
            }
            return getCustomsBomFrm;
        }

        private void btnConsumption_Click(object sender, EventArgs e)
        {
            SqlConnection consumptionConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (consumptionConn.State == ConnectionState.Closed) { consumptionConn.Open(); }

            string strConsumption = @"SELECT [FG No] + '/' + [Batch No] AS [成品备件号], 1 AS [项号], [RM Customs Code] AS [原料备件号], " +
                                     "CAST(SUM(Consumption) AS decimal(18,6)) AS [净耗], CAST([Qty Loss Rate] AS decimal(18,6)) AS [数量损耗率(%)], " +
                                     "CAST([Qty Loss Rate] AS decimal(18,6)) AS [重量损耗率(%)], [Note] AS [注释], [Drools EHB] AS [废料备件号], " +
                                     "[BOM In Customs] AS [重复备件号], 'C014' AS [手册号] FROM M_DailyBOM GROUP BY [FG No] + '/' + [Batch No], " + 
                                     "[RM Customs Code], [Qty Loss Rate], [Note], [Drools EHB], [BOM In Customs]";
            SqlDataAdapter consumptionAdapter = new SqlDataAdapter(strConsumption, consumptionConn);
            publicTable = new DataTable();
            consumptionAdapter.Fill(publicTable);
            consumptionAdapter.Dispose();

            DataRow[] datarow = publicTable.Select("[净耗] = 0.0");
            if (datarow.Length > 0) 
            {
                foreach (DataRow dr in datarow)
                { dr.Delete(); }
                publicTable.AcceptChanges();
            }

            int iLineNo = 0;
            string strBOM = null;
            for (int i = 0; i < publicTable.Rows.Count; i++)
            {
                if (String.Compare(publicTable.Rows[i]["成品备件号"].ToString().Trim(), strBOM) == 0) { iLineNo++; }
                else
                {
                    strBOM = publicTable.Rows[i]["成品备件号"].ToString().Trim();
                    iLineNo = 1;
                }
                publicTable.Rows[i]["项号"] = iLineNo;
            }

            middleTable = publicTable.Copy();
            this.dgvConsumption.DataSource = publicTable;
            this.dgvConsumption.Columns["手册号"].Visible = false;
            if (consumptionConn.State == ConnectionState.Open)
            {
                consumptionConn.Close();
                consumptionConn.Dispose();
            }
        }

        private void btnUpdateBOMEHB_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to update the BOM EHB?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }
            if (String.IsNullOrEmpty(this.txtBatchNo.Text.Trim())) 
            { 
                MessageBox.Show("Please input Batch No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtBatchNo.Focus();
                return;
            }
            if (String.IsNullOrEmpty(this.txtBOMEHB.Text.Trim()))
            {
                MessageBox.Show("Please input BOM In Customs.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.txtBOMEHB.Focus();
                return;
            }

            SqlConnection sqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = @"UPDATE M_DailyBOM SET [BOM In Customs] = '" + this.txtBOMEHB.Text.Trim().ToUpper() + "' WHERE [Batch No] = '" + this.txtBatchNo.Text.Trim().ToUpper() + "'";
            sqlComm.ExecuteNonQuery();
            sqlComm.Dispose();
            sqlConn.Close();
            sqlConn.Dispose();
            MessageBox.Show("Update data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void llblCheck_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.dgvConsumption.RowCount == 0) 
            {
                MessageBox.Show("Please click 'Consumption' button to generate related data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnConsumption.Focus();
                return;
            }

            SqlConnection checkConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (checkConn.State == ConnectionState.Closed) { checkConn.Open(); }
            SqlCommand checkComm = new SqlCommand();
            checkComm.Connection = checkConn;

            string strBOMRecord = null;
            for (int i = 0; i < this.dgvConsumption.RowCount; i++)
            {
                string strBOM = this.dgvConsumption["成品备件号", i].Value.ToString().Trim();
                checkComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = strBOM;
                checkComm.CommandText = @"SELECT [FG No] + '/' + [Batch No] AS [成品备件号], 1 AS [项号], [RM Customs Code] AS [原料备件号], SUM(Consumption) AS [净耗], " +
                                         "[Qty Loss Rate] AS [数量损耗率(%)], [Qty Loss Rate] AS [重量损耗率(%)], [Note] AS [注释], [Drools EHB] AS [废料备件号], " +
                                         "[BOM In Customs] AS [重复备件号] FROM M_DailyBOM WHERE Consumption > 0 GROUP BY [FG No] + '/' + [Batch No], " +
                                         "[RM Customs Code], [Qty Loss Rate], [Note], [Drools EHB], [BOM In Customs] HAVING [FG No] + '/' + [Batch No] = @BOM";

                SqlDataAdapter checkAdapter = new SqlDataAdapter();
                checkAdapter.SelectCommand = checkComm;
                DataTable checkTable = new DataTable();
                checkAdapter.Fill(checkTable);
                checkAdapter.Dispose();

                int iMAXLine = checkTable.Rows.Count;
                checkTable.Clear();
                checkTable.Dispose();

                int j = 0;
                string strBOMExist = null;
                for (; j < iMAXLine; j++)
                {
                    checkComm.Parameters.Clear();
                    checkComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = this.dgvConsumption["原料备件号", i + j].Value.ToString().Trim();
                    checkComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvConsumption["净耗", i + j].Value.ToString().Trim());
                    checkComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvConsumption["数量损耗率(%)", i + j].Value.ToString().Trim());
                    checkComm.Parameters.Add("@Note", SqlDbType.NVarChar).Value = this.dgvConsumption["注释", i + j].Value.ToString().Trim();
                    checkComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = this.dgvConsumption["废料备件号", i + j].Value.ToString().Trim();
                    checkComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = strBOM;

                    checkComm.CommandText = @"SELECT [成品备件号] FROM E_Consumption WHERE [原料备件号] = @RMCustomsCode AND [净耗] = @Consumption AND " +
                                             "[数量损耗率(%)] = @QtyLossRate AND [注释] = @Note AND [废料备件号] = @DroolsEHB AND [成品备件号] <> @BOM";
                    strBOMExist = Convert.ToString(checkComm.ExecuteScalar());
                    if (String.IsNullOrEmpty(strBOMExist)){ break; }
                }

                if (j == iMAXLine)
                {
                    //There is the same BOM data in customs                 
                    strBOMRecord += "\n" + strBOM + " mapping " + strBOMExist + "\n";
                    checkComm.Parameters.Clear();
                    checkComm.Parameters.Add("@BOMinCustoms", SqlDbType.NVarChar).Value = strBOMExist;
                    checkComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = strBOM;
                    
                    checkComm.CommandText = @"UPDATE M_DailyBOM SET [BOM In Customs] = @BOMinCustoms WHERE [FG No] + '/' + [Batch No] = @BOM";
                    SqlTransaction checkTrans = checkConn.BeginTransaction();
                    checkComm.Transaction = checkTrans;
                    try
                    {
                        checkComm.ExecuteNonQuery();
                        checkTrans.Commit();
                    }
                    catch (Exception)
                    {
                        checkTrans.Rollback();
                        checkTrans.Dispose();
                        throw;
                    }
                }

                i = i + iMAXLine - 1;
                checkComm.Parameters.Clear();
            }

            if (String.IsNullOrEmpty(strBOMRecord))
            {  MessageBox.Show("Does not exist duplicate BOM.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                if (MessageBox.Show("Have duplicate BOM exist:" + strBOMRecord, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    this.dgvConsumption.Refresh();
                    this.btnConsumption_Click(sender, e);
                }
            }
        }

        private void btnOriginalGoods_Click(object sender, EventArgs e)
        {
            SqlConnection origgoodsConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (origgoodsConn.State == ConnectionState.Closed) { origgoodsConn.Open(); }

            string strOrigGoods = @"SELECT DISTINCT NULL AS [序号], M.[FG No] + '/' + M.[Batch No] AS [物料备件号], M.[HS Code] AS [HS编码], M.[CHN Name] AS [中文品名], " +
                                   "M.[FG No] AS [规格], M.[Batch No] AS [型号], N'千克' AS [单位], M.[Order Price] AS [单价(美元)], N'美元' AS [币制], 1 AS [净重], " +
                                   "N'中国' AS [国别], N'成品' AS [货物类型], 1 AS [小数标识], M.[BOM In Customs] AS [重复备件号], M.[Order Currency], " +
                                   "'3122442019' AS [货主十位数编码], H.[BOM EHB] AS [合并货物备件号], 'C014' AS [手册号] FROM M_DailyBOM AS M, B_HS AS H WHERE " +
                                   "SUBSTRING(M.[FG No], 0, CHARINDEX('-', M.[FG No])) = H.[Grade]";
            SqlDataAdapter origgoodsAdapter = new SqlDataAdapter(strOrigGoods, origgoodsConn);
            publicTable = new DataTable();
            publicTable.Reset();
            origgoodsAdapter.Fill(publicTable);
            origgoodsAdapter = new SqlDataAdapter(@"SELECT [Object], [Rate] FROM B_ExchangeRate ORDER BY [Object]", origgoodsConn);
            DataTable pt = new DataTable();
            origgoodsAdapter.Fill(pt);
            origgoodsAdapter.Dispose();

            for (int i = 0; i < publicTable.Rows.Count; i++)
            {
                publicTable.Rows[i]["序号"] = i + 1;
                string strOrderCurrency = publicTable.Rows[i]["Order Currency"].ToString() + ":USD";
                if (String.Compare(strOrderCurrency, "USD:USD") != 0)
                {
                    decimal dPrice = Convert.ToDecimal(publicTable.Rows[i]["单价(美元)"].ToString().Trim());
                    DataRow[] dr = pt.Select("[Object] = '" + strOrderCurrency + "'");
                    if (dr.Length == 0) { publicTable.Rows[i]["单价(美元)"] = 0.0M; }
                    else { publicTable.Rows[i]["单价(美元)"] = Math.Round(dPrice * Convert.ToDecimal(dr[0][1].ToString().Trim()), 2); }
                }
            }
            pt.Clear();
            pt.Dispose();
            publicTable.Columns.Remove("Order Currency");

            this.dgvOriginalGoods.DataSource = publicTable;
            this.dgvOriginalGoods.Columns["货主十位数编码"].Visible = false;
            this.dgvOriginalGoods.Columns["手册号"].Visible = false;
            if (origgoodsConn.State == ConnectionState.Open)
            {
                origgoodsConn.Close();
                origgoodsConn.Dispose();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!this.rbtnConsumption.Checked && !this.rbtnOriginalGoods.Checked)
            {
                MessageBox.Show("Please select consumption or original goods to download first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.dgvConsumption.RowCount == 0 && this.rbtnConsumption.Checked)
            {
                MessageBox.Show("The consumption data not exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnDownload.Focus();
                return;
            }

            if (this.dgvOriginalGoods.RowCount == 0 && this.rbtnOriginalGoods.Checked)
            {
                MessageBox.Show("The original goods data not exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnDownload.Focus();
                return;
            }

            DialogResult dlgR = MessageBox.Show("Please select a document format:\n[Yes] Generate Excel as new version;\n[No] Generate Excel as old version;\n[Cancel] Reject to handle.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dlgR == DialogResult.Yes)
            {
                #region //new version
                if (this.rbtnConsumption.Checked && this.dgvConsumption.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                    string[] strFiledName = { "成品备件号" };
                    SqlLib lib = new SqlLib();
                    DataTable dt = lib.SelectDistinct(middleTable, strFiledName);
                    lib.Dispose(0);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string strEHB = dt.Rows[i][0].ToString().Trim();
                        DataRow[] datarow = middleTable.Select("[成品备件号] = '" + strEHB + "' AND ([重复备件号] IS NULL OR [重复备件号] = '')");
                        if (datarow.Length > 0)
                        {
                            worksheet.Name = strEHB.Replace("/", "X");
                            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[datarow.Length + 1, 10]).NumberFormatLocal = "@";
                            for (int j = 0; j < datarow.Length; j++)
                            {
                                worksheet.Cells[j + 2, 1] = datarow[j][9].ToString().Trim();
                                worksheet.Cells[j + 2, 2] = datarow[j][0].ToString().Trim();
                                worksheet.Cells[j + 2, 3] = datarow[j][3].ToString().Trim();
                                worksheet.Cells[j + 2, 4] = datarow[j][4].ToString().Trim();
                                worksheet.Cells[j + 2, 5] = datarow[j][5].ToString().Trim();
                                worksheet.Cells[j + 2, 6] = "0";
                                worksheet.Cells[j + 2, 7] = datarow[j][2].ToString().Trim();
                                worksheet.Cells[j + 2, 8] = datarow[j][7].ToString().Trim();
                                worksheet.Cells[j + 2, 9] = string.Empty;
                            }
                            worksheet.Cells[1, 1] = "手册号";
                            worksheet.Cells[1, 2] = "成品货物备件号";
                            worksheet.Cells[1, 3] = "净耗数量";
                            worksheet.Cells[1, 4] = "数量损耗率";
                            worksheet.Cells[1, 5] = "重量损耗率";
                            worksheet.Cells[1, 6] = "总耗重量";
                            worksheet.Cells[1, 7] = "原料货物备件号";
                            worksheet.Cells[1, 8] = "废料货物备件号";
                            worksheet.Cells[1, 9] = "备注";

                            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, 9]).Font.Bold = true;
                            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[datarow.Length + 1, 9]).Font.Name = "Verdana";
                            worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[datarow.Length + 1, 9]).Font.Size = 9;
                            worksheet.Cells.EntireColumn.AutoFit();

                            if (i < dt.Rows.Count - 1)
                            {
                                object missing = System.Reflection.Missing.Value;
                                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, missing, missing, missing);
                            }
                        }
                    }
                    dt.Clear();
                    dt.Dispose();
                    excel.Visible = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                }

                if (this.rbtnOriginalGoods.Checked && this.dgvOriginalGoods.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                    worksheet.Name = "OriginalGoods";
                    worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvOriginalGoods.Rows.Count + 1, 6]).NumberFormatLocal = "@";
                    int iActualRow = 0;
                    for (int k = 0; k < this.dgvOriginalGoods.Rows.Count; k++)
                    {
                        if (String.IsNullOrEmpty(this.dgvOriginalGoods[14, k].Value.ToString().Trim()))
                        {
                            iActualRow++;
                            worksheet.Cells[iActualRow + 1, 1] = this.dgvOriginalGoods[15, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 2] = this.dgvOriginalGoods[2, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 3] = this.dgvOriginalGoods[16, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 4] = "//";
                            worksheet.Cells[iActualRow + 1, 5] = this.dgvOriginalGoods[17, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 6] = "1";
                        }
                    }
                    worksheet.Cells[1, 1] = "货主十位数编码";
                    worksheet.Cells[1, 2] = "原始货物备件号";
                    worksheet.Cells[1, 3] = "合并货物备件号";
                    worksheet.Cells[1, 4] = "规格型号";
                    worksheet.Cells[1, 5] = "仓库号";
                    worksheet.Cells[1, 6] = "单位净重";

                    worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, 6]).Font.Bold = true;
                    worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvOriginalGoods.Rows.Count + 1, 6]).Font.Name = "Verdana";
                    worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvOriginalGoods.Rows.Count + 1, 6]).Font.Size = 9;
                    worksheet.Cells.EntireColumn.AutoFit();
                    excel.Visible = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                }
                #endregion
            }
            else if (dlgR == DialogResult.No)
            {
                #region //old version
                if (this.rbtnConsumption.Checked && this.dgvConsumption.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    excel.Application.Workbooks.Add(true);
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvConsumption.RowCount + 1, this.dgvConsumption.ColumnCount - 2]).NumberFormatLocal = "@";
                    int iActualRow = 0, iLineNo = 0;
                    string strBatchFg = String.Empty;
                    for (int i = 0; i < this.dgvConsumption.RowCount; i++)
                    {
                        string strBOM1 = this.dgvConsumption[0, i].Value.ToString().Trim();
                        if (String.Compare(strBOM1, strBatchFg) == 0) { iLineNo++; }
                        else { iLineNo = 1; strBatchFg = strBOM1; }
                        string strBOM2 = this.dgvConsumption[8, i].Value.ToString().Trim(); //Mapping 'BOM In Customs'
                        decimal dConsump = Convert.ToDecimal(this.dgvConsumption[3, i].Value.ToString().Trim()); //Mapping 'Consumption'
                        if (String.IsNullOrEmpty(strBOM2) && dConsump > 0.0M)
                        {
                            iActualRow++;
                            for (int j = 0; j < this.dgvConsumption.ColumnCount - 2; j++)
                            {
                                if (j == 1) { excel.Cells[iActualRow + 1, j + 1] = iLineNo; }
                                else { excel.Cells[iActualRow + 1, j + 1] = this.dgvConsumption[j, i].Value.ToString().Trim(); }
                            }
                        }
                    }
                    for (int k = 0; k < this.dgvConsumption.ColumnCount - 2; k++)
                    { excel.Cells[1, k + 1] = this.dgvConsumption.Columns[k].HeaderText.ToString().Trim(); }

                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvConsumption.ColumnCount - 2]).Font.Bold = true;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvConsumption.ColumnCount - 2]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvConsumption.ColumnCount - 2]).Font.Name = "Verdana";
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvConsumption.ColumnCount - 2]).Font.Size = 9;
                    excel.Cells.EntireColumn.AutoFit();                 
                    excel.Visible = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                }

                if (this.rbtnOriginalGoods.Checked && this.dgvOriginalGoods.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    excel.Application.Workbooks.Add(true);
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvOriginalGoods.RowCount + 1, this.dgvOriginalGoods.ColumnCount - 5]).NumberFormatLocal = "@"; //Set the excel cells format as text
                    int iActualRow = 0;
                    for (int m = 0; m < this.dgvOriginalGoods.RowCount; m++)
                    {
                        if (String.IsNullOrEmpty(this.dgvOriginalGoods[14, m].Value.ToString().Trim())) //The same mapping 'BOM In Customs'
                        {
                            iActualRow++;
                            for (int n = 1; n < this.dgvOriginalGoods.ColumnCount - 4; n++)
                            { excel.Cells[iActualRow + 1, n + 1] = this.dgvOriginalGoods[n + 1, m].Value.ToString().Trim(); }
                        }
                    }
                    for (int y = 1; y <= iActualRow; y++) { excel.Cells[y + 1, 1] = y; } //Regenerate the first column value, since it is the serial number

                    for (int x = 1; x < this.dgvOriginalGoods.ColumnCount - 4; x++)
                    { excel.Cells[1, x] = this.dgvOriginalGoods.Columns[x].HeaderText.ToString().Trim(); }

                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvOriginalGoods.ColumnCount - 5]).Font.Bold = true;
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvOriginalGoods.ColumnCount - 5]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvOriginalGoods.ColumnCount - 5]).Font.Name = "Verdana";
                    excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvOriginalGoods.ColumnCount - 5]).Font.Size = 9;
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Visible = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                }
                #endregion
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (this.dgvConsumption.RowCount == 0 || this.dgvOriginalGoods.RowCount == 0) 
            {
                MessageBox.Show("Please click 'Consumption' button and 'Original Goods' button to generate data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnConsumption.Focus();
                return; 
            }

            SqlConnection approvedConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (approvedConn.State == ConnectionState.Closed) { approvedConn.Open(); }
            SqlCommand approvedComm = new SqlCommand();
            approvedComm.Connection = approvedConn;

            DateTime dt = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm"));
            for (int x = 0; x < this.dgvOriginalGoods.RowCount; x++)
            {
                if (String.Compare(this.dgvOriginalGoods[0, x].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strBatchNo = this.dgvOriginalGoods["型号", x].Value.ToString().Trim();
                    string strFGNo = this.dgvOriginalGoods["规格", x].Value.ToString().Trim();

                    approvedComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    approvedComm.CommandText = @"SELECT MAX([Actual Start Date]) AS [Actual Start Date], MAX([Actual Close Date]) AS [Actual Close Date], [Batch No], " +
                                                "[FG No], [BOM In Customs], [FG Qty], [Order Price], [Order Currency], [Total Input Qty], [Total RM Cost(USD)], " +
                                                "[HS Code], [CHN Name], [Qty Loss Rate], [Drools Qty], MAX([Created Date]) AS [Created Date], '" + loginFrm.PublicUserName +
                                                "' AS [Creater], '" + dt + "' AS [Approved Date] FROM M_DailyBOM GROUP BY [Batch No], [FG No], " +
                                                "[BOM In Customs], [FG Qty], [Order Price], [Order Currency], [Total Input Qty], [Total RM Cost(USD)], [HS Code], " +
                                                "[CHN Name], [Qty Loss Rate], [Drools Qty] HAVING [Batch No] = @BatchNo";

                    SqlDataAdapter approvedAdapter = new SqlDataAdapter();
                    approvedAdapter.SelectCommand = approvedComm;
                    DataTable approvedTable1 = new DataTable();
                    approvedAdapter.Fill(approvedTable1);

                    approvedComm.CommandText = @"SELECT [Batch Path], [Batch No], ROW_NUMBER() OVER(ORDER BY [Batch No]) AS [Line No], [Item No], [Lot No], " + 
                                                "[Inventory Type], [RM Category], [RM Customs Code], [BGD No], [RM Qty], [RM Currency], [RM Price], [Consumption], " + 
                                                "[Drools EHB], [Note] FROM M_DailyBOM WHERE [Consumption] > 0.0 AND [Batch No] = @BatchNo";
                    approvedAdapter.SelectCommand = approvedComm;
                    DataTable approvedTable2 = new DataTable();
                    approvedAdapter.Fill(approvedTable2);

                    approvedComm.Parameters.RemoveAt("@BatchNo");
                    approvedComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = strFGNo + '/' + strBatchNo;
                    approvedComm.CommandText = @"SELECT [FG No] + '/' + [Batch No] AS [成品备件号], ROW_NUMBER() OVER (ORDER BY [RM Customs Code], [Note], [Drools EHB]) AS [项号], " +
                                                "[RM Customs Code] AS [原料备件号], SUM(Consumption) AS [净耗], CAST([Qty Loss Rate] AS decimal(18, 6)) AS [数量损耗率(%)], " +
                                                "CAST([Qty Loss Rate] AS decimal(18, 6)) AS [重量损耗率(%)], [Note] AS [注释], [Drools EHB] AS [废料备件号], '" +
                                                dt + "' AS [Created Date], [BOM In Customs] FROM M_DailyBOM WHERE [Consumption] > 0.0 GROUP BY " + 
                                                "[FG No] + '/' + [Batch No], [RM Customs Code], [Qty Loss Rate], [Note], [Drools EHB], [BOM In Customs] " + 
                                                "HAVING [FG No] + '/' + [Batch No] = @BOM";
                    approvedAdapter.SelectCommand = approvedComm;
                    DataTable approvedTable3 = new DataTable();
                    approvedAdapter.Fill(approvedTable3);
                    approvedAdapter.Dispose();

                    #region //Handle C_BOM, C_BOMDetail, E_Consumption table's data
                    approvedComm.Parameters.Clear();
                    approvedComm.CommandType = CommandType.StoredProcedure;
                    approvedComm.CommandText = @"usp_InsertBOM";
                    approvedComm.Parameters.AddWithValue("@TVP_Master", approvedTable1);
                    approvedComm.ExecuteNonQuery();
                    approvedComm.Parameters.Clear();

                    approvedComm.CommandText = @"usp_InsertBOMDetail";
                    approvedComm.Parameters.AddWithValue("@TVP_Detail", approvedTable2);
                    approvedComm.ExecuteNonQuery();
                    approvedComm.Parameters.Clear();

                    approvedComm.CommandText = @"usp_InsertBOMDoc";
                    approvedComm.Parameters.AddWithValue("@TVP_Doc", approvedTable3);
                    approvedComm.ExecuteNonQuery();
                    approvedComm.Parameters.Clear();
                    #endregion                
                    approvedTable1.Dispose();
                    approvedTable2.Dispose();
                    approvedTable3.Dispose();

                    #region //Handle E_OriginalGoods table's data
                    approvedComm.CommandType = CommandType.Text;
                    approvedComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["HS编码", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["中文品名", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["规格", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["型号", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@Units", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["单位", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvOriginalGoods["单价(美元)", x].Value.ToString().Trim());
                    approvedComm.Parameters.Add("@MoneyStyle", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["币制", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@NetWeight", SqlDbType.Int).Value = Convert.ToInt32(this.dgvOriginalGoods["净重", x].Value.ToString().Trim());
                    approvedComm.Parameters.Add("@Country", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["国别", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@GoodsType", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["货物类型", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@DecimalMark", SqlDbType.Int).Value = Convert.ToInt32(this.dgvOriginalGoods["小数标识", x].Value.ToString().Trim());
                    approvedComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                    approvedComm.Parameters.Add("@BOMinCustoms", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["重复备件号", x].Value.ToString().Trim();
                    approvedComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = this.dgvOriginalGoods["物料备件号", x].Value.ToString().Trim();

                    approvedComm.CommandText = @"INSERT INTO E_OriginalGoods([HS编码], [中文品名], [规格], [型号], [单位], [单价(美元)], [币制], [净重], [国别], " +
                                                "[货物类型], [小数标识], [Created Date], [重复备件号], [物料备件号]) VALUES(@HSCode, @CHNName, @FGNo, @BatchNo, " +
                                                "@Units, @UnitPrice, @MoneyStyle, @NetWeight, @Country, @GoodsType, @DecimalMark, @CreatedDate, @BOMinCustoms, @BOM)";
                    SqlTransaction origgoodsTrans = approvedConn.BeginTransaction();
                    approvedComm.Transaction = origgoodsTrans;
                    try
                    {
                        approvedComm.ExecuteNonQuery();
                        origgoodsTrans.Commit();
                    }
                    catch (Exception)
                    {
                        origgoodsTrans.Rollback();
                        origgoodsTrans.Dispose();
                        try
                        {
                            approvedComm.CommandText = @"UPDATE E_OriginalGoods SET [HS编码] = @HSCode, [中文品名] = @CHNName, [规格] = @FGNo, [型号] = @BatchNo, " +
                                                        "[单位] = @Units, [单价(美元)] = @UnitPrice, [币制] = @MoneyStyle, [净重] = @NetWeight, [国别] = @Country, " +
                                                        "[货物类型] = @GoodsType, [小数标识] = @DecimalMark, [Created Date] = @CreatedDate, [BOM In Customs] = @BOMinCustoms" +
                                                        " WHERE [物料备件号] = @BOM";
                            approvedComm.ExecuteNonQuery();
                        }
                        catch (Exception) { throw; }
                    }
                    finally
                    { approvedComm.Parameters.Clear(); }
                    #endregion

                    approvedComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    approvedComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo";
                    SqlTransaction approvedTran = approvedConn.BeginTransaction();
                    approvedComm.Transaction = approvedTran;
                    try
                    {
                        approvedComm.ExecuteNonQuery();
                        approvedTran.Commit();
                    }
                    catch (Exception)
                    {
                        approvedTran.Rollback();
                        approvedTran.Dispose();
                        try
                        {
                            approvedComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo";
                            approvedComm.ExecuteNonQuery();
                        }
                        catch (Exception) { throw; }
                    }
                    finally { approvedComm.Parameters.Clear(); }
                }
            }

            approvedComm.Dispose();
            if (approvedConn.State == ConnectionState.Open)
            {
                approvedConn.Close();
                approvedConn.Dispose();
            }

            if (MessageBox.Show("Successfully approve.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                this.btnConsumption_Click(sender, e);
                this.btnOriginalGoods_Click(sender, e);
            }
        }

        private void dgvOriginalGoods_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "全选")
                {
                    for (int i = 0; i < this.dgvOriginalGoods.RowCount; i++) { this.dgvOriginalGoods[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "取消全选";
                }
                else if (objHeader == "取消全选")
                {
                    for (int i = 0; i < this.dgvOriginalGoods.RowCount; i++) { this.dgvOriginalGoods[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "全选";
                }
                else if (objHeader == "反选")
                {
                    for (int i = 0; i < this.dgvOriginalGoods.RowCount; i++)
                    {
                        if (String.Compare(this.dgvOriginalGoods[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvOriginalGoods[0, i].Value = true; }

                        else { this.dgvOriginalGoods[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "全选";
                }
            }
        }

        private void dgvOriginalGoods_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvOriginalGoods.RowCount == 0) { return; }
            if (this.dgvOriginalGoods.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvOriginalGoods.RowCount; i++)
                {
                    if (String.Compare(this.dgvOriginalGoods[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvOriginalGoods.RowCount && iCount > 0)
                { this.dgvOriginalGoods.Columns[0].HeaderText = "反选"; }

                else if (iCount == this.dgvOriginalGoods.RowCount)
                { this.dgvOriginalGoods.Columns[0].HeaderText = "取消全选"; }

                else if (iCount == 0)
                { this.dgvOriginalGoods.Columns[0].HeaderText = "全选"; }
            }
        }
    }
}