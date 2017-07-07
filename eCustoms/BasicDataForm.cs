using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class BasicDataForm : Form
    {
        protected DataView dvBasicData = new DataView();
        string strFilter = null;
        protected PopUpFilterForm filterFrm = null;

        private DataSet mySet = new DataSet();
        private SqlDataAdapter myAdapter = new SqlDataAdapter();
        private BindingSource myBindSource = new BindingSource();
        private string strSwitch;
        SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);

        private static string[] strList = { "Basic Parameter", "Chinese Description", "Country", "Deposit Scope", "Drools", "HS Code" };

        private static BasicDataForm BasicDataFrm;
        public BasicDataForm(){InitializeComponent();}       
        public static BasicDataForm CreateInstance()
        {
            if (BasicDataFrm == null || BasicDataFrm.IsDisposed) { BasicDataFrm = new BasicDataForm(); }
            return BasicDataFrm;
        }

        private void OrigDataForm_Load(object sender, EventArgs e)
        {
            DataTable myTable = new DataTable();
            myTable.Columns.Add("ObjectName", typeof(string));

            DataRow myRow = myTable.NewRow();
            myRow["ObjectName"] = "";
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[0];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[1];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[2];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[3];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[4];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[5];
            myTable.Rows.Add(myRow);      

            this.bindingNavigatorcmbItem.ComboBox.DataSource = myTable;
            this.bindingNavigatorcmbItem.ComboBox.DisplayMember = this.bindingNavigatorcmbItem.ComboBox.ValueMember = "ObjectName";
            this.bindingNavigatorcmbItem.ComboBox.Focus();
            this.ControlSwitch(false);
        }

        public void SearchData(string strObjectName)
        {
            strFilter = "";
            dvBasicData.RowFilter = "";

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            if (strObjectName == strList[0].Trim()) { SqlComm.CommandText = "SELECT * FROM B_SysInfo ORDER BY [SerialNo]"; }
            else if (strObjectName == strList[1].Trim()) { SqlComm.CommandText = "SELECT * FROM B_ChnDescription ORDER BY [RM EHB]"; }
            else if (strObjectName == strList[2].Trim()) { SqlComm.CommandText = "SELECT * FROM B_Country ORDER BY [ShortName]"; }
            else if (strObjectName == strList[3].Trim()) { SqlComm.CommandText = "SELECT * FROM B_Deposit"; }
            else if (strObjectName == strList[4].Trim()) { SqlComm.CommandText = "SELECT * FROM B_Drools ORDER BY [FG CHN Name], [RM Category]"; }
            else { SqlComm.CommandText = "SELECT * FROM B_HsCode ORDER BY [Grade], [Package Code]"; }

            myAdapter.SelectCommand = SqlComm;
            mySet.Clear();
            myAdapter.Fill(mySet);
            dvBasicData = mySet.Tables[0].DefaultView;
    
            myBindSource.DataSource = dvBasicData;
            this.dgvHandleData.DataSource = myBindSource;

            System.Drawing.Font font = this.dgvHandleData.Font;
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.Font = new System.Drawing.Font(font, FontStyle.Bold);

            if (strObjectName == strList[1].Trim()) { this.dgvHandleData.Columns["RM EHB"].HeaderCell.Style = cellStyle; }
            if (strObjectName == strList[2].Trim()) { this.dgvHandleData.Columns["ShortName"].HeaderCell.Style = cellStyle; }
            if (strObjectName == strList[4].Trim())
            {
                this.dgvHandleData.Columns["FG CHN Name"].HeaderCell.Style = cellStyle;
                this.dgvHandleData.Columns["RM Category"].HeaderCell.Style = cellStyle;
            }           
            if (strObjectName == strList[5].Trim())
            {
                this.dgvHandleData.Columns["Grade"].HeaderCell.Style = cellStyle;
                this.dgvHandleData.Columns["Package Code"].HeaderCell.Style = cellStyle;
            }

            mySet.Dispose();
            myAdapter.Dispose();
            SqlComm.Dispose();
            if (SqlConn.State == ConnectionState.Open)
            { SqlConn.Close(); }    
        }

        public void ControlSwitch(bool bClose)
        {
            this.bindingNavigatorAddItem.Enabled = bClose;
            this.bindingNavigatorDeleteItem.Enabled = bClose;
            this.bindingNavigatorEditItem.Enabled = bClose;
            this.bindingNavigatorUpdateItem.Enabled = bClose;
            this.bindingNavigatorCancelItem.Enabled = bClose;
            this.bindingNavigatorSearchItem.Enabled = !bClose;
        }

        private void bindingNavigatorcmbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            myAdapter = new SqlDataAdapter();
            mySet = new DataSet();
        }

        private void bindingNavigatorSearchItem_Click(object sender, EventArgs e)
        {
            if (this.bindingNavigatorcmbItem.ComboBox.SelectedIndex < 1)
            {
                this.bindingNavigatorcmbItem.ComboBox.Text = null;
                this.bindingNavigatorcmbItem.ComboBox.Focus();
                this.dgvHandleData.Columns.Clear();
                return;
            }

            string strComboBox = this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString().Trim();
            this.SearchData(strComboBox);

            if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[0] || 
                this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[3]) 
            { this.ControlSwitch(false); }
            else
            {
                this.ControlSwitch(true);
                this.bindingNavigatorSearchItem.Enabled = true;
                this.bindingNavigatorUpdateItem.Enabled = false;
                this.bindingNavigatorCancelItem.Enabled = false;
            }
            this.bindingNavigatorcmbItem.Enabled = true;
        }

        private void bindingNavigatorAddItem_Click(object sender, EventArgs e)
        {
            strSwitch = "Add";
            myBindSource.AddNew();
            this.ControlSwitch(false);

            this.bindingNavigatorUpdateItem.Enabled = true;
            this.bindingNavigatorCancelItem.Enabled = true;
            this.bindingNavigatorSearchItem.Enabled = false;
            this.bindingNavigatorcmbItem.Enabled = false;
        }

        private void bindingNavigatorEditItem_Click(object sender, EventArgs e)
        {
            if (this.dgvHandleData.Rows.Count == 0)
            {
                MessageBox.Show("No data exist, reject to edit.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.bindingNavigator1.Focus();
                return;
            }

            strSwitch = "Edit";
            this.ControlSwitch(false);

            this.bindingNavigatorUpdateItem.Enabled = true;
            this.bindingNavigatorCancelItem.Enabled = true;
            this.bindingNavigatorSearchItem.Enabled = false;
            this.bindingNavigatorcmbItem.Enabled = false;

            if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[1])
            {this.dgvHandleData.Columns["RM EHB"].ReadOnly = true; }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[2])
            { this.dgvHandleData.Columns["ShortName"].ReadOnly = true; }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[4])
            {
                this.dgvHandleData.Columns["FG CHN Name"].ReadOnly = true;
                this.dgvHandleData.Columns["RM Category"].ReadOnly = true;
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[5])
            {
                this.dgvHandleData.Columns["Grade"].ReadOnly = true;
                this.dgvHandleData.Columns["Package Code"].ReadOnly = true;
            }
        }

        private void bindingNavigatorUpdateItem_Click(object sender, EventArgs e)
        {
            bool bEmpty = false;
            for (int i = 0; i < this.dgvHandleData.ColumnCount; i++)
            {
                if (!String.IsNullOrEmpty(this.dgvHandleData[i, this.dgvHandleData.CurrentRow.Index].Value.ToString().Trim())) bEmpty = true;
            }
            if (bEmpty == false)
            {
                MessageBox.Show("No data exist, reject to save.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.bindingNavigatorSearchItem_Click(sender, e);
                return;
            }

            SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;
            int iCellIndex = this.dgvHandleData.CurrentCell.RowIndex;
            if (strSwitch == "Add") { iCellIndex = this.dgvHandleData.RowCount - 1; }

            if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[1])
            {
                SqlComm.Parameters.Add("@GoodsType", SqlDbType.NVarChar).Value = this.dgvHandleData["Goods Type", iCellIndex].Value.ToString().Trim().ToUpper();
                string strRmPrice = this.dgvHandleData["RM Price", iCellIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strRmPrice)) { SqlComm.Parameters.Add("@RmPrice", SqlDbType.Decimal).Value = 0.0M; }
                else { SqlComm.Parameters.Add("@RmPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strRmPrice), 2); }
                SqlComm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = this.dgvHandleData["Currency", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@RmChnName", SqlDbType.NVarChar).Value = this.dgvHandleData["RM CHN Name", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@ChnDescription", SqlDbType.NVarChar).Value = this.dgvHandleData["CHN Description", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = this.dgvHandleData["RM EHB", iCellIndex].Value.ToString().Trim().ToUpper();

                if (strSwitch == "Add")
                { SqlComm.CommandText = "INSERT INTO B_ChnDescription([Goods Type], [RM Price], [Currency], [RM CHN Name], [CHN Description], [RM EHB]) " + 
                                        "VALUES(@GoodsType, @RmPrice, @Currency, @RmChnName, @ChnDescription, @RMEHB)"; }

                if (strSwitch == "Edit")
                { SqlComm.CommandText = "UPDATE B_ChnDescription SET [Goods Type] = @GoodsType, [RM Price] = @RmPrice, [Currency] = @Currency, " + 
                                        "[RM CHN Name] = @RmChnName, [CHN Description] = @ChnDescription WHERE [RM EHB] = @RMEHB"; }
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[2])
            {
                SqlComm.Parameters.Add("@EN", SqlDbType.NVarChar).Value = this.dgvHandleData["EN", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@CN", SqlDbType.NVarChar).Value = this.dgvHandleData["CN", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@Code", SqlDbType.NVarChar).Value = this.dgvHandleData["Code", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = this.dgvHandleData["ShortName", iCellIndex].Value.ToString().Trim().ToUpper();

                if (strSwitch == "Add") { SqlComm.CommandText = "INSERT INTO B_Country([EN], [CN], [Code], [ShortName]) VALUES( @EN, @CN, @Code, @ShortName)"; }

                if (strSwitch == "Edit") { SqlComm.CommandText = "UPDATE B_Country SET [EN] = @EN, [CN] = @CN, [Code] = @Code WHERE [ShortName] = @ShortName"; }
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[4])
            {
                SqlComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = this.dgvHandleData["HS Code", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@DroolsChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["Drools CHN Name", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = this.dgvHandleData["Drools EHB", iCellIndex].Value.ToString().Trim();
                string strAvgPrice = this.dgvHandleData["Average Price(RMB)", iCellIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strAvgPrice)) { SqlComm.Parameters.Add("@AveragePrice", SqlDbType.Decimal).Value = 0.0M; }
                else { SqlComm.Parameters.Add("@AveragePrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strAvgPrice), 2); }
                SqlComm.Parameters.Add("@FGChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["FG CHN Name", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = this.dgvHandleData["RM Category", iCellIndex].Value.ToString().Trim().ToUpper();

                if (strSwitch == "Add")
                { SqlComm.CommandText = "INSERT INTO B_Drools([HS Code], [Drools CHN Name], [Drools EHB], [Average Price(RMB)], [FG CHN Name], [RM Category]) " +
                                          "VALUES(@HSCode, @DroolsChineseName, @DroolsEHB, @AveragePrice, @FGChineseName, @RMCategory)"; }

                if (strSwitch == "Edit")
                { SqlComm.CommandText = "UPDATE B_Drools SET [HS Code] = @HSCode, [Drools CHN Name] = @DroolsChineseName, [Drools EHB] = @DroolsEHB, " +
                                          "[Average Price(RMB)] = @AveragePrice WHERE [FG CHN Name] = @FGChineseName AND [RM Category] = @RMCategory"; }
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[5])
            {
                SqlComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = this.dgvHandleData["HS Code", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@ChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["FG CHN Name", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@BOMEHB", SqlDbType.NVarChar).Value = this.dgvHandleData["BOM EHB", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@DutyRate", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Duty Rate", iCellIndex].Value.ToString().Trim());
                string strIsAllocated = this.dgvHandleData["IsAllocated", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@IsAllocated", SqlDbType.Bit).Value = String.IsNullOrEmpty(strIsAllocated) ? false : true;
                SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = this.dgvHandleData["Grade", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@PackageCode", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["Package Code", iCellIndex].Value.ToString().Trim());

                if (strSwitch == "Add")
                { SqlComm.CommandText = "INSERT INTO B_HsCode([HS Code], [FG CHN Name], [BOM EHB], [Duty Rate], [IsAllocated], [Grade], [Package Code]) " +
                                          "VALUES(@HSCode, @ChineseName, @BOMEHB,  @DutyRate, @IsAllocated, @Grade, @PackageCode)"; }

                if (strSwitch == "Edit")
                { SqlComm.CommandText = "UPDATE B_HsCode SET [HS Code] = @HSCode, [FG CHN Name] = @ChineseName, [BOM EHB] = @BOMEHB, [Duty Rate] = @DutyRate, " +
                                          "[IsAllocated] = @IsAllocated WHERE [Grade] = @Grade AND [Package Code] = @PackageCode"; }
            }

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
                if (SqlConn.State == ConnectionState.Open)
                { SqlConn.Close(); }
            }

            myBindSource.EndEdit();
            bindingNavigatorSearchItem_Click(sender, e);
            strSwitch = null;
        }

        private void bindingNavigatorCancelItem_Click(object sender, EventArgs e)
        {
            if (strSwitch == "Add") { this.dgvHandleData.Rows.RemoveAt(this.dgvHandleData.RowCount - 1); }
            mySet.RejectChanges();
            this.ControlSwitch(true);
            this.bindingNavigatorSearchItem.Enabled = true;
            this.bindingNavigatorUpdateItem.Enabled = false;
            this.bindingNavigatorCancelItem.Enabled = false;
            this.bindingNavigatorcmbItem.Enabled = true;
            myBindSource.Position = 0;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (this.dgvHandleData.Rows.Count == 0)
            {
                MessageBox.Show("No data exist, reject to delete.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.bindingNavigator1.Focus();
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this data?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
                if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
                SqlCommand SqlComm = new SqlCommand();
                SqlComm.Connection = SqlConn;

                int iCellIndex = this.dgvHandleData.CurrentCell.RowIndex;
                if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[1])
                {
                    SqlComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = this.dgvHandleData["RM EHB", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = "DELETE FROM B_ChnDescription WHERE [RM EHB] = @RM EHB";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[2])
                {
                    SqlComm.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = this.dgvHandleData["ShortName", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = "DELETE FROM B_Country WHERE [ShortName] = @ShortName";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[4])
                {
                    SqlComm.Parameters.Add("@FGChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["FG CHN Name", iCellIndex].Value.ToString().Trim();
                    SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = this.dgvHandleData["RM Category", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = "DELETE FROM B_Drools WHERE [FG CHN Name] = @FGChineseName AND [RM Category] = @RMCategory";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[5])
                {
                    SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = this.dgvHandleData["Grade", iCellIndex].Value.ToString().Trim();
                    SqlComm.Parameters.Add("@PackageCode", SqlDbType.Decimal).Value = Decimal.Parse(this.dgvHandleData["Package Code", iCellIndex].Value.ToString().Trim());
                    SqlComm.CommandText = "DELETE FROM B_HsCode WHERE [Grade] = @Grade AND [Package Code] = @PackageCode";
                }                         
                
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
                    if (SqlConn.State == ConnectionState.Open)
                    { SqlConn.Close(); }
                }            
            }
            bindingNavigatorSearchItem_Click(sender, e);
        }

        private void tsBtnDownload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.bindingNavigatorcmbItem.Text.ToString().Trim()) || this.dgvHandleData.RowCount == 0)
            {
                MessageBox.Show("There is no data to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            int PageRow = 65536;
            int iPageCount = (int)(this.dgvHandleData.Rows.Count / PageRow);
            if (iPageCount * PageRow < this.dgvHandleData.Rows.Count) { iPageCount += 1; }
            try
            {
                for (int m = 1; m <= iPageCount; m++)
                {
                    string strPath = System.Windows.Forms.Application.StartupPath + "\\" + this.bindingNavigatorcmbItem.Text.Trim() + "_" + m.ToString() + ".xls";
                    if (File.Exists(strPath)) { File.Delete(strPath); }
                    Thread.Sleep(1000);
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); 
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < this.dgvHandleData.Columns.Count; n++)
                    { sb.Append(this.dgvHandleData.Columns[n].Name.Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    for (int i = (m - 1) * PageRow; i < this.dgvHandleData.Rows.Count && i < m * PageRow; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        for (int j = 0; j < this.dgvHandleData.Columns.Count; j++)
                        { sb.Append(this.dgvHandleData[j, i].Value.ToString().Trim() + "\t"); }
                        sb.Append(Environment.NewLine);
                    }                    
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
                MessageBox.Show("Successfully download " + this.bindingNavigatorcmbItem.Text.Trim() + " file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvHandleData.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvHandleData.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvHandleData.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvHandleData[strColumnName, this.dgvHandleData.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] = " + strColumnText; }
                    }
                }
                dvBasicData.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiExcludeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvHandleData.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvHandleData.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvHandleData.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvHandleData[strColumnName, this.dgvHandleData.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvHandleData.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] <> " + strColumnText; }
                    }
                }
                dvBasicData.RowFilter = strFilter;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dvBasicData.RowFilter = "";
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvHandleData.CurrentCell != null)
            {
                string strColumnName = this.dgvHandleData.Columns[this.dgvHandleData.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter);
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dvBasicData.ToTable(true, new string[] { strColumnName });
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
                        if (this.dgvHandleData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvHandleData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvHandleData.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvHandleData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvHandleData.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvHandleData.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                dvBasicData.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}