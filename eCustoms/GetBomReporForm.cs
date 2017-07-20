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
    public partial class GetBomReporForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable dtBomPublic;
        private DataTable dtBomDoc = new DataTable();
        private static SaveFileDialog saveFileDialog = new SaveFileDialog();
        private String strBatchListWithoutUSDcomponent = string.Empty; 

        public GetBomReporForm() 
        { InitializeComponent(); }
        private static GetBomReporForm getBomRptFrm;
        public static GetBomReporForm CreateInstance()
        {
            if (getBomRptFrm == null || getBomRptFrm.IsDisposed) { getBomRptFrm = new GetBomReporForm(); }
            return getBomRptFrm;
        }

        private void GetBomReporForm_Load(object sender, EventArgs e) { }
        private void GetBomReporForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            strBatchListWithoutUSDcomponent = string.Empty;
            dtBomPublic = new DataTable();
            dtBomPublic.Dispose(); 
            dtBomDoc.Dispose();
        }

        private void btnConsumption_Click(object sender, EventArgs e)
        {
            SqlConnection consmptConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (consmptConn.State == ConnectionState.Closed) { consmptConn.Open(); }
            string strConsmpt = "SELECT [FG No] + '/' + [Batch No] AS [成品备件号], 1 AS [项号], [RM EHB] AS [原料备件号], [Consumption] AS [净耗], " + 
                                "[Qty Loss Rate] AS [数量损耗率(%)], [Qty Loss Rate] AS [重量损耗率(%)], [RM Category] AS [注释], [Drools EHB] AS [废料备件号], " + 
                                "[BOM In Customs], 'C014' AS [手册号] FROM (" +
                                "SELECT SUBSTRING([FG Description], 0, CHARINDEX('-', [FG Description], CHARINDEX('-', [FG Description], 0) + 1)) AS [FG No], " +
                                "[Batch No], [RM EHB], CAST(SUM(Consumption) AS decimal(18,6)) AS [Consumption], CAST([Qty Loss Rate] AS decimal(18,6)) AS [Qty Loss Rate], " +
                                "CASE WHEN [RM Category] = 'RMB' THEN N'非保料件' ELSE N'保税料件' END AS [RM Category], [Drools EHB], [BOM In Customs] FROM M_DailyBOM " +
	                            "GROUP BY [FG Description], [Batch No], [RM EHB], [Qty Loss Rate], [RM Category], [Drools EHB], [BOM In Customs]) AS tbConsmpt";
            SqlDataAdapter consmptAdapter = new SqlDataAdapter(strConsmpt, consmptConn);
            dtBomPublic = new DataTable();
            consmptAdapter.Fill(dtBomPublic);
            consmptAdapter.Dispose();

            DataRow[] datarow = dtBomPublic.Select("[净耗] = 0.0");
            if (datarow.Length > 0) 
            {
                foreach (DataRow dr in datarow)
                { dr.Delete(); }
                dtBomPublic.AcceptChanges();
            }

            int iLineNo = 0;
            string strBOM = null;
            for (int i = 0; i < dtBomPublic.Rows.Count; i++)
            {
                if (String.Compare(dtBomPublic.Rows[i]["成品备件号"].ToString().Trim(), strBOM) == 0) { iLineNo++; }
                else
                {
                    strBOM = dtBomPublic.Rows[i]["成品备件号"].ToString().Trim();
                    iLineNo = 1;
                }
                dtBomPublic.Rows[i]["项号"] = iLineNo;
            }

            dtBomDoc = dtBomPublic.Copy();
            this.dgvConsmpt.DataSource = dtBomPublic;
            this.dgvConsmpt.Columns["手册号"].Visible = false;
            if (consmptConn.State == ConnectionState.Open)
            {
                consmptConn.Close();
                consmptConn.Dispose();
            }

            //Pop up a message box to list out those Batch No. of finished Goods do not have one single USD component raw material, June.28.2017
            #region
            DataRow[] drsBatchesWithoutUSDcompoent = dtBomPublic.Select(String.Empty, "[成品备件号] ASC");
            strBatchListWithoutUSDcomponent = string.Empty;
            String strBatchName = string.Empty;
            int intCounter = 0;
            for (int i = 0; i < drsBatchesWithoutUSDcompoent.Length; i++)
            {
                if (string.Compare(strBatchName, drsBatchesWithoutUSDcompoent[i]["成品备件号"].ToString().Substring(drsBatchesWithoutUSDcompoent[i]["成品备件号"].ToString().IndexOf("/")+1)) == 0)
                {
                    if (string.Compare(drsBatchesWithoutUSDcompoent[i]["注释"].ToString(), "非保料件") == 0) 
                    {
                        intCounter += 0;
                    }
                    else
                    {
                        intCounter += 1;
                    }

                    if ((intCounter == 0) && i == drsBatchesWithoutUSDcompoent.Length-1)
                    {
                        strBatchListWithoutUSDcomponent += "'" + strBatchName + "',";
                    }
                }
                else
                {
                    if ((intCounter == 0) && !string.IsNullOrEmpty(strBatchName))
                    {
                        strBatchListWithoutUSDcomponent += "'" + strBatchName + "',";
                    }
                    intCounter = 0;
                    strBatchName = drsBatchesWithoutUSDcompoent[i]["成品备件号"].ToString().Substring(drsBatchesWithoutUSDcompoent[i]["成品备件号"].ToString().IndexOf("/")+1);
                    i--;
                }
            }

            if (!string.IsNullOrEmpty(strBatchListWithoutUSDcomponent))
            {
                MessageBox.Show("We do not have bonded component material for the following batch No. " + strBatchListWithoutUSDcomponent.Remove(strBatchListWithoutUSDcomponent.Length -1), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

            #endregion


        }

        private void llblCheck_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.dgvConsmpt.RowCount == 0)
            { MessageBox.Show("Please click 'Consumption' button to generate related data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            SqlConnection duplicBomConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (duplicBomConn.State == ConnectionState.Closed) { duplicBomConn.Open(); }
            SqlCommand duplicBomComm = new SqlCommand();
            duplicBomComm.Connection = duplicBomConn;

            string strBOMRecord = null;
            for (int i = 0; i < this.dgvConsmpt.RowCount; i++)
            {
                string strBOM = this.dgvConsmpt["成品备件号", i].Value.ToString().Trim();
                duplicBomComm.Parameters.Clear();
                duplicBomComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = this.dgvConsmpt["成品备件号", i].Value.ToString().Trim();
                duplicBomComm.CommandText = "SELECT SUBSTRING([FG Description],0,CHARINDEX('-',[FG Description],CHARINDEX('-',[FG Description],0)+1))+'/'+[Batch No] " + 
                                            "AS [成品备件号], 1 AS [项号], [RM EHB] AS [原料备件号], SUM(Consumption) AS [净耗], [Qty Loss Rate] AS [数量损耗率(%)], " + 
                                            "[Qty Loss Rate] AS [重量损耗率(%)], [RM Category] AS [注释], [Drools EHB] AS [废料备件号], [BOM In Customs] AS [重复备件号] " + 
                                            "FROM M_DailyBOM WHERE Consumption > 0 GROUP BY [FG Description], [Batch No], [RM EHB], [Qty Loss Rate], [RM Category], " +
                                            "[Drools EHB], [BOM In Customs] HAVING SUBSTRING([FG Description],0,CHARINDEX('-',[FG Description],CHARINDEX('-',[FG Description],0)+1))+'/'+[Batch No] = @BOM";
                SqlDataAdapter duplicBomAdapter = new SqlDataAdapter();
                duplicBomAdapter.SelectCommand = duplicBomComm;
                DataTable dtDuplicBom = new DataTable();
                duplicBomAdapter.Fill(dtDuplicBom);
                duplicBomAdapter.Dispose();
                int iMaxLine = dtDuplicBom.Rows.Count;
                dtDuplicBom.Dispose();

                string strBomExist = null;
                int j = 0;
                for (; j < iMaxLine; j++)
                {
                    duplicBomComm.Parameters.Clear();
                    duplicBomComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = this.dgvConsmpt["原料备件号", i + j].Value.ToString().Trim();
                    duplicBomComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvConsmpt["净耗", i + j].Value.ToString().Trim());
                    duplicBomComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvConsmpt["数量损耗率(%)", i + j].Value.ToString().Trim());
                    duplicBomComm.Parameters.Add("@RmCategory", SqlDbType.NVarChar).Value = this.dgvConsmpt["注释", i + j].Value.ToString().Trim();
                    duplicBomComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = this.dgvConsmpt["废料备件号", i + j].Value.ToString().Trim();
                    duplicBomComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = strBOM;
                    duplicBomComm.CommandText = "SELECT [成品备件号] FROM E_Consumption WHERE [原料备件号] = @RMEHB AND [净耗] = @Consumption AND " +
                                                "[数量损耗率(%)] = @QtyLossRate AND [注释] = @RmCategory AND [废料备件号] = @DroolsEHB AND [成品备件号] <> @BOM";
                    strBomExist = Convert.ToString(duplicBomComm.ExecuteScalar());
                    if (String.IsNullOrEmpty(strBomExist)) { break; }
                }

                if (j == iMaxLine)
                {
                    strBOMRecord += "\n" + strBOM + " mapping " + strBomExist + "\n";
                    duplicBomComm.Parameters.Clear();
                    duplicBomComm.Parameters.Add("@BomInCustoms", SqlDbType.NVarChar).Value = strBomExist;
                    duplicBomComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = strBOM;
                    duplicBomComm.CommandText = "UPDATE M_DailyBOM SET [BOM In Customs] = @BomInCustoms WHERE " + 
                                                "SUBSTRING([FG Description],0,CHARINDEX('-',[FG Description],CHARINDEX('-',[FG Description],0)+1))+'/'+[Batch No] = @BOM";
                    duplicBomComm.ExecuteNonQuery();
                }
                i = i + iMaxLine - 1;
            }

            if (String.IsNullOrEmpty(strBOMRecord))
            { MessageBox.Show("There is not duplicate BOM.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                if (MessageBox.Show("Existing the duplicate BOM:" + strBOMRecord, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    this.dgvConsmpt.Refresh();
                    this.btnConsumption_Click(sender, e);
                }
            }
        }

        private void btnUpdateBOMEHB_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtBatchNo.Text.Trim()))
            { MessageBox.Show("Please input the field of Batch No.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (String.IsNullOrEmpty(this.txtBOMEHB.Text.Trim()))
            { MessageBox.Show("Please input the field of BOM In Customs.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show("Do you want to update the BOM EHB?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) { return; }

            SqlConnection ehbConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (ehbConn.State == ConnectionState.Closed) { ehbConn.Open(); }
            SqlCommand ehbComm = new SqlCommand();
            ehbComm.Connection = ehbConn;
            ehbComm.CommandText = "UPDATE M_DailyBOM SET [BOM In Customs] = '" + this.txtBOMEHB.Text.Trim().ToUpper() + "' WHERE [Batch No] = '" + this.txtBatchNo.Text.Trim().ToUpper() + "'";
            ehbComm.ExecuteNonQuery();
            ehbComm.Dispose();
            if (ehbConn.State == ConnectionState.Open) { ehbConn.Close(); ehbConn.Dispose(); }
            MessageBox.Show("Update data successfully.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnOriginalGoods_Click(object sender, EventArgs e)
        {
            SqlConnection orgGoodsConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (orgGoodsConn.State == ConnectionState.Closed) { orgGoodsConn.Open(); }
            string strOrigGoods = "SELECT ROW_NUMBER() OVER(ORDER BY [FG No], [Batch No]) AS [序号], [FG No] + '/' + [Batch No] AS [物料备件号], [HS Code] AS [HS编码], " + 
                                  "[CHN Name] AS [中文品名], [FG No] AS [规格], [Batch No] AS [型号], N'千克' AS [单位], [Order Price(USD)] AS [单价(美元)], " + 
                                  "N'美元' AS [币制], 1 AS [净重], N'中国' AS [国别], N'成品' AS [货物类型], 1 AS [小数标识], [BOM In Customs], " + 
                                  "N'3122442019' AS [货主十位数编码], [BOM EHB] AS [合并货物备件号], N'C014' AS [手册号] FROM (" +
                                  "SELECT DISTINCT SUBSTRING(M.[FG Description], 0, CHARINDEX('-', M.[FG Description], CHARINDEX('-', M.[FG Description], 0) + 1)) AS [FG No], " + 
                                  "M.[Batch No], M.[HS Code], M.[CHN Name], M.[Order Price(USD)], M.[BOM In Customs], B.[BOM EHB] FROM M_DailyBOM AS M LEFT JOIN (SELECT " + 
                                  "DISTINCT [Grade], [BOM EHB] FROM B_HsCode) AS B ON SUBSTRING(M.[FG Description], 0, CHARINDEX('-', M.[FG Description], 0)) = B.[Grade]) AS tbog";
            SqlDataAdapter orgGoodsAdapter = new SqlDataAdapter(strOrigGoods, orgGoodsConn);
            dtBomPublic = new DataTable();
            orgGoodsAdapter.Fill(dtBomPublic);
            orgGoodsAdapter.Dispose();

            this.dgvOrgGoods.DataSource = dtBomPublic;
            this.dgvOrgGoods.Columns["货主十位数编码"].Visible = false;
            this.dgvOrgGoods.Columns["手册号"].Visible = false;
            if (orgGoodsConn.State == ConnectionState.Open)
            { orgGoodsConn.Close(); orgGoodsConn.Dispose(); }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!this.rbtnConsumption.Checked && !this.rbtnOriginalGoods.Checked)
            { MessageBox.Show("Please select consumption or original goods to download first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (this.dgvConsmpt.RowCount == 0 && this.rbtnConsumption.Checked)
            { MessageBox.Show("There is no data for consumption.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            if (this.dgvOrgGoods.RowCount == 0 && this.rbtnOriginalGoods.Checked)
            { MessageBox.Show("There is no data for Original Goods.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }

            DialogResult dlgR = MessageBox.Show("Please select a document format:\n[Yes] Generate Excel as old version to upload to Customs system;\n[No] Generate Excel as new version to keep record and print out paper for Customs declaration;\n[Cancel] Reject to handle.", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dlgR == DialogResult.Yes)
            {
                #region //Generate Excel as old version to upload to Customs system
                if (this.rbtnConsumption.Checked && this.dgvConsmpt.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                    string[] strFiledName = { "成品备件号" };
                    SqlLib lib = new SqlLib();
                    DataTable dt = lib.SelectDistinct(dtBomDoc, strFiledName);
                    lib.Dispose(0);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string strEHB = dt.Rows[i][0].ToString().Trim();
                        //if this batch no has no single bonded material, it has to be stopped to submit data for customs declaration. June.29.2017
                        if (strBatchListWithoutUSDcomponent.IndexOf(strEHB.Substring(strEHB.IndexOf("/") + 1)) > 0) { strEHB = ""; };

                        DataRow[] datarow = dtBomDoc.Select("[成品备件号] = '" + strEHB + "' AND ([BOM In Customs] IS NULL OR [BOM In Customs] = '')");
                        if (datarow.Length > 0)
                        {
                            worksheet.Name = strEHB.Replace("/", "|");
                            //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[datarow.Length + 1, 10]).NumberFormatLocal = "@";
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

                            //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, 9]).Font.Bold = true;
                            //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[datarow.Length + 1, 9]).Font.Name = "Verdana";
                            //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[datarow.Length + 1, 9]).Font.Size = 9;
                            worksheet.Cells.EntireColumn.AutoFit();
                            excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
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

                if (this.rbtnOriginalGoods.Checked && this.dgvOrgGoods.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                    worksheet.Name = "OriginalGoods";
                    //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvOrgGoods.Rows.Count + 1, 6]).NumberFormatLocal = "@";
                    int iActualRow = 0;
                    for (int k = 0; k < this.dgvOrgGoods.Rows.Count; k++)
                    {
                        //if this batch no has no single bonded material, it has to be stopped to submit data for customs declaration. June.29.2017
                        if (strBatchListWithoutUSDcomponent.IndexOf(this.dgvOrgGoods[2, k].Value.ToString().Substring(this.dgvOrgGoods[2, k].Value.ToString().IndexOf("/") + 1)) >= 0)
                        { continue; }; // Just skip for this iteration

                        if (String.IsNullOrEmpty(this.dgvOrgGoods[14, k].Value.ToString().Trim()))
                        {
                            iActualRow++;
                            worksheet.Cells[iActualRow + 1, 1] = this.dgvOrgGoods[15, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 2] = this.dgvOrgGoods[2, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 3] = this.dgvOrgGoods[16, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 4] = "//";
                            worksheet.Cells[iActualRow + 1, 5] = this.dgvOrgGoods[17, k].Value.ToString().Trim();
                            worksheet.Cells[iActualRow + 1, 6] = "1";
                        }
                    }
                    worksheet.Cells[1, 1] = "货主十位数编码";
                    worksheet.Cells[1, 2] = "原始货物备件号";
                    worksheet.Cells[1, 3] = "合并货物备件号";
                    worksheet.Cells[1, 4] = "规格型号";
                    worksheet.Cells[1, 5] = "仓库号";
                    worksheet.Cells[1, 6] = "单位净重";

                    //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, 6]).Font.Bold = true;
                    //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvOrgGoods.Rows.Count + 1, 6]).Font.Name = "Verdana";
                    //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[this.dgvOrgGoods.Rows.Count + 1, 6]).Font.Size = 9;
                    worksheet.Cells.EntireColumn.AutoFit();
                    excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                    excel.Visible = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                }
                #endregion
            }
            else if (dlgR == DialogResult.No)
            {
                #region //Generate new version of Spreadsheet files for filing purpose
                if (this.rbtnConsumption.Checked && this.dgvConsmpt.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    excel.Application.Workbooks.Add(true);
                   // excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvConsmpt.RowCount + 1, this.dgvConsmpt.ColumnCount - 2]).NumberFormatLocal = "@";
                    int iActualRow = 0, iLineNo = 0;
                    string strBatchFg = String.Empty;
                    for (int i = 0; i < this.dgvConsmpt.RowCount; i++)
                    {
                        //if this batch no has no single bonded material, it does not need to be saved for filing purpose. July.20.2017
                        // Get the value of the third column in the grid, the value looks like 1000-GY7G350T/0006337069
                        if (strBatchListWithoutUSDcomponent.IndexOf(this.dgvConsmpt[3, i].Value.ToString().Trim().Substring(this.dgvConsmpt[3, i].Value.ToString().Trim().IndexOf("/") + 1)) > 0)
                        { continue; }; // Just skip for this iteration;

                        string strBOM1 = this.dgvConsmpt[0, i].Value.ToString().Trim();
                        if (String.Compare(strBOM1, strBatchFg) == 0) { iLineNo++; }
                        else { iLineNo = 1; strBatchFg = strBOM1; }
                        string strBOM2 = this.dgvConsmpt[8, i].Value.ToString().Trim(); //Mapping 'BOM In Customs'
                        decimal dConsump = Convert.ToDecimal(this.dgvConsmpt[3, i].Value.ToString().Trim()); //Mapping 'Consumption'
                        if (String.IsNullOrEmpty(strBOM2) && dConsump > 0.0M)
                        {
                            iActualRow++;
                            for (int j = 0; j < this.dgvConsmpt.ColumnCount - 2; j++)
                            {
                                if (j == 1) { excel.Cells[iActualRow + 1, j + 1] = iLineNo; }
                                else { excel.Cells[iActualRow + 1, j + 1] = this.dgvConsmpt[j, i].Value.ToString().Trim(); }
                            }
                        }
                    }
                    for (int k = 0; k < this.dgvConsmpt.ColumnCount - 2; k++)
                    { excel.Cells[1, k + 1] = this.dgvConsmpt.Columns[k].HeaderText.ToString().Trim(); }

                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvConsmpt.ColumnCount - 2]).Font.Bold = true;
                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvConsmpt.ColumnCount - 2]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvConsmpt.ColumnCount - 2]).Font.Name = "Verdana";
                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvConsmpt.ColumnCount - 2]).Font.Size = 9;
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                    excel.Visible = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                }

                if (this.rbtnOriginalGoods.Checked && this.dgvOrgGoods.RowCount > 0)
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    excel.Application.Workbooks.Add(true);
                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[this.dgvOrgGoods.RowCount + 1, this.dgvOrgGoods.ColumnCount - 5]).NumberFormatLocal = "@"; //Set the excel cells format as text
                    int iActualRow = 0;
                    for (int m = 0; m < this.dgvOrgGoods.RowCount; m++)
                    {
                        //if this batch no has no single bonded material, it does not need to be saved for filing purpose. July.20.2017
                        // Get the value of the second column in the grid, the value looks like 1000-GY7G350T/0006337069
                        if (strBatchListWithoutUSDcomponent.IndexOf(this.dgvConsmpt[2, m].Value.ToString().Trim().Substring(this.dgvConsmpt[2, m].Value.ToString().Trim().IndexOf("/") + 1)) > 0)
                        { continue; }; // Just skip for this iteration;
                        if (String.IsNullOrEmpty(this.dgvOrgGoods[14, m].Value.ToString().Trim())) //The same mapping 'BOM In Customs'
                        {
                            iActualRow++;
                            for (int n = 1; n < this.dgvOrgGoods.ColumnCount - 4; n++)
                            { excel.Cells[iActualRow + 1, n + 1] = this.dgvOrgGoods[n + 1, m].Value.ToString().Trim(); }
                        }
                    }
                    for (int y = 1; y <= iActualRow; y++) { excel.Cells[y + 1, 1] = y; } //Regenerate the first column value, since it is the serial number

                    for (int x = 1; x < this.dgvOrgGoods.ColumnCount - 4; x++)
                    { excel.Cells[1, x] = this.dgvOrgGoods.Columns[x].HeaderText.ToString().Trim(); }

                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvOrgGoods.ColumnCount - 5]).Font.Bold = true;
                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, this.dgvOrgGoods.ColumnCount - 5]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvOrgGoods.ColumnCount - 5]).Font.Name = "Verdana";
                    //excel.get_Range(excel.Cells[1, 1], excel.Cells[iActualRow + 1, this.dgvOrgGoods.ColumnCount - 5]).Font.Size = 9;
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                    excel.Visible = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                }
                #endregion
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (this.dgvConsmpt.RowCount == 0 && this.dgvOrgGoods.RowCount == 0)
            { MessageBox.Show("Please click 'Consumption' button and 'Original Goods' button to generate data first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            SqlConnection apprBomConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (apprBomConn.State == ConnectionState.Closed) { apprBomConn.Open(); }
            SqlCommand apprBomComm = new SqlCommand();
            apprBomComm.Connection = apprBomConn;
            DateTime dtBomNow = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm"));
            for (int x = 0; x < this.dgvOrgGoods.RowCount; x++)
            {
                if (String.Compare(this.dgvOrgGoods[0, x].EditedFormattedValue.ToString(), "True") == 0)
                {
                    string strBatchNo = this.dgvOrgGoods["型号", x].Value.ToString().Trim();
                    string strFGNo = this.dgvOrgGoods["规格", x].Value.ToString().Trim();

                    apprBomComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    apprBomComm.CommandText = "SELECT DISTINCT [Process Order No], [Actual Start Date], [Actual End Date], [Batch No], [FG No], [FG Description], [FG Qty], " +
                                              "[Total Input Qty], [Drools Qty], [Order Price(USD)], [Total RM Cost(USD)], [Qty Loss Rate], [HS Code], [CHN Name], '" +
                                              loginFrm.PublicUserName + "' AS [Creater], [Created Date], [BOM In Customs] FROM M_DailyBOM WHERE [Batch No] = @BatchNo";
                    SqlDataAdapter apprBomAdapter = new SqlDataAdapter();
                    apprBomAdapter.SelectCommand = apprBomComm;
                    DataTable dtApprBomM = new DataTable();
                    apprBomAdapter.Fill(dtApprBomM);

                    apprBomComm.CommandText = "SELECT [Batch Path], [Batch No], ROW_NUMBER() OVER(ORDER BY [Batch No]) AS [Line No], [Item No], [Item Description], " +
                                              "[Lot No], [Inventory Type], [RM Category], [RM EHB], [BGD No], [RM Qty], [RM Currency], [RM Price], [Consumption], " + 
                                              "[Drools EHB] FROM M_DailyBOM WHERE [Consumption] > 0.0 AND [Batch No] = @BatchNo";
                    apprBomAdapter.SelectCommand = apprBomComm;
                    DataTable dtApprBomD = new DataTable();
                    apprBomAdapter.Fill(dtApprBomD);

                    apprBomComm.Parameters.RemoveAt("@BatchNo");
                    apprBomComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = strFGNo + '/' + strBatchNo;
                    apprBomComm.CommandText = "SELECT [FG No]+'/'+[Batch No] AS [成品备件号], ROW_NUMBER() OVER (ORDER BY [RM EHB], [RM Category], [Drools EHB]) AS [项号], " + 
                                              "[RM EHB] AS [原料备件号], [Consumption] AS [净耗], [Qty Loss Rate] AS [数量损耗率(%)], [Qty Loss Rate] AS [重量损耗率(%)], " + 
                                              "[RM Category] AS [注释], [Drools EHB] AS [废料备件号], '" + dtBomNow + "' AS [Created Date], [BOM In Customs] FROM (" +
                                              "SELECT [Batch No], SUBSTRING([FG Description], 0, CHARINDEX('-', [FG Description], CHARINDEX('-', [FG Description], 0) + 1)) AS [FG No], " +
                                              "[RM EHB], SUM(Consumption) AS [Consumption], CAST([Qty Loss Rate] AS decimal(18, 6)) AS [Qty Loss Rate], " +
                                              "CASE WHEN [RM Category] = 'RMB' THEN N'非保料件' ELSE N'保税料件' END AS [RM Category], [Drools EHB], [BOM In Customs] " +
                                              "FROM M_DailyBOM WHERE [Consumption] > 0.0 GROUP BY [Batch No], [FG Description], [RM EHB], [Qty Loss Rate], [RM Category], " + 
                                              "[Drools EHB], [BOM In Customs]) AS tbcd WHERE [FG No] + '/' + [Batch No] = @BOM";
                    apprBomAdapter.SelectCommand = apprBomComm;
                    DataTable dtApprBomDoc = new DataTable();
                    apprBomAdapter.Fill(dtApprBomDoc);
                    apprBomAdapter.Dispose();

                    #region //Handle C_BOM, C_BOMDetail, E_Consumption table's data
                    apprBomComm.Parameters.Clear();
                    apprBomComm.CommandType = CommandType.StoredProcedure;
                    apprBomComm.CommandText = @"usp_InsertBom";
                    apprBomComm.Parameters.AddWithValue("@tvp_BomMaster", dtApprBomM);
                    apprBomComm.ExecuteNonQuery();
                    apprBomComm.Parameters.Clear();
                    dtApprBomM.Dispose();

                    apprBomComm.CommandText = @"usp_InsertBomDetail";
                    apprBomComm.Parameters.AddWithValue("@tvp_BomDetail", dtApprBomD);
                    apprBomComm.ExecuteNonQuery();
                    apprBomComm.Parameters.Clear();
                    dtApprBomD.Dispose();

                    apprBomComm.CommandText = @"usp_InsertBomDoc";
                    apprBomComm.Parameters.AddWithValue("@tvp_BomDoc", dtApprBomDoc);
                    apprBomComm.ExecuteNonQuery();
                    apprBomComm.Parameters.Clear();
                    dtApprBomDoc.Dispose();

                    if (strBatchNo.Substring(strBatchNo.Trim().Length - 1) == "R")  //update current BOM's gongdan used qty to keep the same with the frozen BOM
                    {
                        apprBomComm.CommandType = CommandType.Text;
                        apprBomComm.CommandText = "SELECT [GongDan Used Qty] FROM C_BOM WHERE [Batch No] = '" + strBatchNo.Remove(strBatchNo.Trim().Length - 1) + "'";
                        string strObj = Convert.ToString(apprBomComm.ExecuteScalar());
                        if (!String.IsNullOrEmpty(strObj))
                        {
                            apprBomComm.Parameters.Clear();
                            apprBomComm.Parameters.Add("@GongDanUsedQty", SqlDbType.Int).Value = Convert.ToInt32(strObj);
                            apprBomComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                            apprBomComm.CommandText = @"UPDATE C_BOM SET [GongDan Used Qty] = @GongDanUsedQty WHERE [Batch No] = @BatchNo";
                            apprBomComm.ExecuteNonQuery();
                            apprBomComm.Parameters.Clear();
                        }
                    }

                    //Freeze the BOM if those Batch No. of finished Goods do not have one single USD component raw material, June.29.2017
                    if (strBatchListWithoutUSDcomponent.IndexOf(strBatchNo) >= 0)  //update current BOM's gongdan used qty to keep the same with the frozen BOM
                    {
                        apprBomComm.CommandType = CommandType.Text;
                        apprBomComm.CommandText = "UPDATE C_BOM SET [Freeze] = 1, [Remark] = 'Frozen by " + loginFrm.PublicUserName + " on " + System.DateTime.Now.ToString("M/d/yyyy HH:mm") + "' WHERE [Batch No] = '" + strBatchNo + "'";
                        apprBomComm.ExecuteNonQuery();
                    }

                    #endregion       
                    
                    
                            
                    #region //Handle E_OriginalGoods table's data
                    apprBomComm.CommandType = CommandType.Text;
                    apprBomComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = this.dgvOrgGoods["HS编码", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@CHNName", SqlDbType.NVarChar).Value = this.dgvOrgGoods["中文品名", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = this.dgvOrgGoods["规格", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvOrgGoods["型号", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@Units", SqlDbType.NVarChar).Value = this.dgvOrgGoods["单位", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvOrgGoods["单价(美元)", x].Value.ToString().Trim());
                    apprBomComm.Parameters.Add("@MoneyStyle", SqlDbType.NVarChar).Value = this.dgvOrgGoods["币制", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@NetWeight", SqlDbType.Int).Value = Convert.ToInt32(this.dgvOrgGoods["净重", x].Value.ToString().Trim());
                    apprBomComm.Parameters.Add("@Country", SqlDbType.NVarChar).Value = this.dgvOrgGoods["国别", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@GoodsType", SqlDbType.NVarChar).Value = this.dgvOrgGoods["货物类型", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@DecimalMark", SqlDbType.Int).Value = Convert.ToInt32(this.dgvOrgGoods["小数标识", x].Value.ToString().Trim());
                    apprBomComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));
                    apprBomComm.Parameters.Add("@BomInCustoms", SqlDbType.NVarChar).Value = this.dgvOrgGoods["BOM In Customs", x].Value.ToString().Trim();
                    apprBomComm.Parameters.Add("@BOM", SqlDbType.NVarChar).Value = this.dgvOrgGoods["物料备件号", x].Value.ToString().Trim();

                    apprBomComm.CommandText = "INSERT INTO E_OriginalGoods([HS编码], [中文品名], [规格], [型号], [单位], [单价(美元)], [币制], [净重], [国别], " +
                                              "[货物类型], [小数标识], [Created Date], [BOM In Customs], [物料备件号]) VALUES(@HSCode, @CHNName, @FGNo, @BatchNo, " +
                                              "@Units, @UnitPrice, @MoneyStyle, @NetWeight, @Country, @GoodsType, @DecimalMark, @CreatedDate, @BomInCustoms, @BOM)";
                    if (strBatchListWithoutUSDcomponent.IndexOf(strBatchNo) < 0) //if this Batch No has at one USD component material on June.29.2017
                    {apprBomComm.ExecuteNonQuery();};
                    
                    apprBomComm.Parameters.Clear();
                    #endregion

                    apprBomComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    apprBomComm.CommandText = "DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo";
                    apprBomComm.ExecuteNonQuery();
                    apprBomComm.Parameters.Clear();
                }
            }
            apprBomComm.Dispose();
            if (apprBomConn.State == ConnectionState.Open)
            {
                apprBomConn.Close();
                apprBomConn.Dispose();
            }
            if (MessageBox.Show("Successfully approve.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                this.btnConsumption_Click(sender, e);
                this.btnOriginalGoods_Click(sender, e);
            }
        }

        private void dgvOrgGoods_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvOrgGoods.RowCount; i++) { this.dgvOrgGoods[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvOrgGoods.RowCount; i++) { this.dgvOrgGoods[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvOrgGoods.RowCount; i++)
                    {
                        if (String.Compare(this.dgvOrgGoods[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvOrgGoods[0, i].Value = true; }

                        else { this.dgvOrgGoods[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }

            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvOrgGoods_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvOrgGoods.RowCount == 0) { return; }
            if (this.dgvOrgGoods.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvOrgGoods.RowCount; i++)
                {
                    if (String.Compare(this.dgvOrgGoods[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvOrgGoods.RowCount && iCount > 0)
                { this.dgvOrgGoods.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvOrgGoods.RowCount)
                { this.dgvOrgGoods.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvOrgGoods.Columns[0].HeaderText = "Select"; }
            }
        }
    }
}