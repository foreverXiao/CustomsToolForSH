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

namespace SHCustomsSystem
{
    public partial class OrigDataForm : Form
    {
        protected DataView dvFillDGV = new DataView();
        string strFilter = null;
        protected PopUpFilterForm filterFrm = null;

        private DataSet mySet = new DataSet();
        private SqlDataAdapter myAdapter = new SqlDataAdapter();
        private BindingSource myBindSource = new BindingSource();
        private string strSwitch;
        SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);

        private static string[] strList = {"Chinese Description", "Drools", "HS Code", "PC Item", "Allocation", "8A Reference", "Exchange Rate", "Address", "IE Type", "Invoice Parameter", "Tax Parameter", "Deposit Scope", "MTI Prefix", "Weight Ratio", "Yield Ratio", "Order Fulfillment", "Finance Price"};

        private static OrigDataForm origFrm;
        public OrigDataForm()
        {
            InitializeComponent();
        }       
        public static OrigDataForm CreateInstance()
        {
            if (origFrm == null || origFrm.IsDisposed)
            {
                origFrm = new OrigDataForm();
            }
            return origFrm;
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
            myRow["ObjectName"] = "------ ------";
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[5];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[6];
            myTable.Rows.Add(myRow);          

            myRow = myTable.NewRow();
            myRow["ObjectName"] = "------ ------";
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[7];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[8];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[9];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[10];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[11];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[12];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[13];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[14];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = "------ ------";
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[15];
            myTable.Rows.Add(myRow);

            myRow = myTable.NewRow();
            myRow["ObjectName"] = strList[16];
            myTable.Rows.Add(myRow);

            this.bindingNavigatorcmbItem.ComboBox.DataSource = myTable;
            this.bindingNavigatorcmbItem.ComboBox.DisplayMember = this.bindingNavigatorcmbItem.ComboBox.ValueMember = "ObjectName";
            this.bindingNavigatorcmbItem.ComboBox.Focus();

            this.ControlSwitch(false);
        }

        public void SearchData(string strObjectName)
        {
            strFilter = "";
            dvFillDGV.RowFilter = "";

            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }
            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            if (strObjectName == strList[0].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_ChnDescription ORDER BY [Goods Type], [RM Customs Code]"; }

            else if (strObjectName == strList[1].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_Drools ORDER BY [FG CHN Name], [RM Category]"; }

            else if (strObjectName == strList[2].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_HS ORDER BY [Grade]"; }

            else if (strObjectName == strList[3].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_PCItem ORDER BY [Grade]"; }

            else if (strObjectName == strList[4].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_Allocation ORDER BY [Grade]"; }

            else if (strObjectName == strList[5].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_8A_Reference ORDER BY [Item No], [RM Currency]"; }  

            else if (strObjectName == strList[6].Trim())
            { SqlComm.CommandText = @"SELECT [Object], [Rate], [Created Date] AS [Effective Date] FROM B_ExchangeRate ORDER BY [Created Date] DESC, [Object] ASC"; }

            else if (strObjectName == strList[7].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_Address ORDER BY [Short Name]"; }

            else if (strObjectName == strList[8].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_IEType ORDER BY [IE Type]"; }

            else if (strObjectName == strList[9].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_InvoiceParameter"; }

            else if (strObjectName == strList[10].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_TaxParameter"; }

            else if (strObjectName == strList[11].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_DepositScope"; }

            else if (strObjectName == strList[12].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_MTI_Prefix ORDER BY [Type], [Remark]"; }

            else if (strObjectName == strList[13].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_WeightRatio"; }

            else if (strObjectName == strList[14].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_YieldRatio"; }

            else if (strObjectName == strList[15].Trim())
            { SqlComm.CommandText = @"SELECT * FROM B_OrderFulfillment ORDER BY [Batch No], [ESS/LINE] DESC"; }

            else { SqlComm.CommandText = @"SELECT * FROM B_FinancePrice ORDER BY [Grade]"; }

            myAdapter.SelectCommand = SqlComm;
            mySet.Clear();
            myAdapter.Fill(mySet);
            dvFillDGV = mySet.Tables[0].DefaultView;
           
            myBindSource.DataSource = dvFillDGV;
            this.dgvHandleData.DataSource = myBindSource;

            System.Drawing.Font font = this.dgvHandleData.Font;
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.Font = new System.Drawing.Font(font, FontStyle.Bold);

            if (strObjectName == strList[0].Trim())
            { this.dgvHandleData.Columns["RM Customs Code"].HeaderCell.Style = cellStyle; }

            if (strObjectName == strList[1].Trim())
            {
                this.dgvHandleData.Columns["FG CHN Name"].HeaderCell.Style = cellStyle;
                this.dgvHandleData.Columns["RM Category"].HeaderCell.Style = cellStyle;
            }

            if (strObjectName == strList[2].Trim() || strObjectName == strList[3].Trim() || strObjectName == strList[4].Trim())
            { this.dgvHandleData.Columns["Grade"].HeaderCell.Style = cellStyle; }

            if (strObjectName == strList[5].Trim())
            {
                this.dgvHandleData.Columns["Item No"].HeaderCell.Style = cellStyle;
                this.dgvHandleData.Columns["RM Currency"].HeaderCell.Style = cellStyle;
            }

            if (strObjectName == strList[6].Trim())
            { 
                this.dgvHandleData.Columns["Object"].HeaderCell.Style = cellStyle;
                this.dgvHandleData.Columns["Effective Date"].HeaderCell.Style = cellStyle; 
            }

            if (strObjectName == strList[7].Trim())
            { this.dgvHandleData.Columns["Short Name"].HeaderCell.Style = cellStyle; }

            if (strObjectName == strList[8].Trim())
            { this.dgvHandleData.Columns["IE Type"].HeaderCell.Style = cellStyle; }

            if (strObjectName == strList[9].Trim() || strObjectName == strList[10].Trim())
            { this.dgvHandleData.Columns["MaxLine"].HeaderCell.Style = cellStyle; }

            if (strObjectName == strList[13].Trim())
            { this.dgvHandleData.Columns["Ratio"].HeaderCell.Style = cellStyle; }

            if (strObjectName == strList[14].Trim())
            {
                this.dgvHandleData.Columns["Ratio Begin"].HeaderCell.Style = cellStyle;
                this.dgvHandleData.Columns["Ratio End"].HeaderCell.Style = cellStyle;
            }

            if (strObjectName == strList[15].Trim())
            {
                this.dgvHandleData.Columns["Batch No"].HeaderCell.Style = cellStyle;
                this.dgvHandleData.Columns["ESS/LINE"].HeaderCell.Style = cellStyle;
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

            if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == "------ ------")
            {
                MessageBox.Show("Please select value from combobox first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.bindingNavigatorcmbItem.ComboBox.Text = null;
                this.bindingNavigatorcmbItem.ComboBox.Focus();
                this.dgvHandleData.Columns.Clear();
                return;
            }

            string strComboBox = this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString().Trim();
            this.SearchData(strComboBox);

            if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[0] ||
                this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[3] ||
                this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[16]) { this.ControlSwitch(false); }
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
            {
                this.dgvHandleData.Columns["FG CHN Name"].ReadOnly = true;
                this.dgvHandleData.Columns["RM Category"].ReadOnly = true;
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[2] || 
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[4])
            { this.dgvHandleData.Columns["Grade"].ReadOnly = true; }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[5])
            {
                this.dgvHandleData.Columns["Item No"].ReadOnly = true;
                this.dgvHandleData.Columns["RM Currency"].ReadOnly = true;
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[6] ||
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[7] || 
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[8] || 
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[9] ||
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[10] ||
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[11] ||
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[12] ||
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[13] ||
                     this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[14])
            {
                this.bindingNavigatorUpdateItem.Enabled = false;
                MessageBox.Show("Please kindly delete this data to recreate the new data directly, no need to modify.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[15])
            {
                this.dgvHandleData.Columns["Batch No"].ReadOnly = true;
                this.dgvHandleData.Columns["ESS/LINE"].ReadOnly = true;
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
            if (strSwitch == "Add")
            { iCellIndex = this.dgvHandleData.RowCount - 1; }

            if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[1])
            {
                SqlComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = this.dgvHandleData["HS Code", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@DroolsChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["Drools CHN Name", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = this.dgvHandleData["Drools EHB", iCellIndex].Value.ToString().Trim();               
                if (String.IsNullOrEmpty(this.dgvHandleData["Average Price(RMB)", iCellIndex].Value.ToString().Trim()))
                { SqlComm.Parameters.Add("@AveragePrice", SqlDbType.Decimal).Value = 0.0M; }
                else
                { SqlComm.Parameters.Add("@AveragePrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.dgvHandleData["Average Price(RMB)", iCellIndex].Value.ToString().Trim()), 2); }
                SqlComm.Parameters.Add("@FGChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["FG CHN Name", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = this.dgvHandleData["RM Category", iCellIndex].Value.ToString().Trim().ToUpper();

                if (strSwitch == "Add")
                {
                    SqlComm.CommandText = @"INSERT INTO B_Drools([HS Code], [Drools CHN Name], [Drools EHB], [Average Price(RMB)], [FG CHN Name], [RM Category]) " +
                                           "VALUES(@HSCode, @DroolsChineseName, @DroolsEHB, @AveragePrice, @FGChineseName, @RMCategory)";
                }

                if (strSwitch == "Edit")
                {
                    SqlComm.CommandText = @"UPDATE B_Drools SET [HS Code] = @HSCode, [Drools CHN Name] = @DroolsChineseName, [Drools EHB] = @DroolsEHB, " +
                                           "[Average Price(RMB)] = @AveragePrice WHERE [FG CHN Name] = @FGChineseName AND [RM Category] = @RMCategory";
                }
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[2])
            {
                SqlComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = this.dgvHandleData["HS Code", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@ChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["CHN Name", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@BOMEHB", SqlDbType.NVarChar).Value = this.dgvHandleData["BOM EHB", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@PackageCode", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["Package Code", iCellIndex].Value.ToString().Trim());
                SqlComm.Parameters.Add("@DutyRate", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Duty Rate", iCellIndex].Value.ToString().Trim());
                SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = this.dgvHandleData["Grade", iCellIndex].Value.ToString().Trim();

                if (strSwitch == "Add")
                { SqlComm.CommandText = @"INSERT INTO B_HS([HS Code], [CHN Name], [BOM EHB], [Package Code], [Duty Rate], [Grade]) VALUES(@HSCode, @ChineseName, @BOMEHB, @PackageCode, @DutyRate, @Grade)"; }

                if (strSwitch == "Edit")
                { SqlComm.CommandText = @"UPDATE B_HS SET [HS Code] = @HSCode, [CHN Name] = @ChineseName, [BOM EHB] = @BOMEHB, [Package Code] = @PackageCode, [Duty Rate] = @DutyRate WHERE [Grade] = @Grade"; }
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[4])
            {
                SqlComm.Parameters.Add("@HSCode", SqlDbType.NVarChar).Value = this.dgvHandleData["HS Code", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@ChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["CHN Name", iCellIndex].Value.ToString().Trim();
                SqlComm.Parameters.Add("@IsAllocated", SqlDbType.Bit).Value = Convert.ToBoolean(this.dgvHandleData["Is Allocated", iCellIndex].Value.ToString().Trim());
                SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = this.dgvHandleData["Grade", iCellIndex].Value.ToString().Trim();

                if (strSwitch == "Add")
                { SqlComm.CommandText = @"INSERT INTO B_Allocation([HS Code], [CHN Name], [Is Allocated], [Grade]) VALUES(@HSCode, @ChineseName, @IsAllocated, @Grade)"; }

                if (strSwitch == "Edit")
                { SqlComm.CommandText = @"UPDATE B_Allocation SET [HS Code] = @HSCode, [CHN Name] = @ChineseName, [Is Allocated] = @IsAllocated WHERE [Grade] = @Grade"; }
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[5])
            {
                SqlComm.Parameters.Add("@RMCustomsCode", SqlDbType.NVarChar).Value = this.dgvHandleData["RM Customs Code", iCellIndex].Value.ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(this.dgvHandleData["RM Price", iCellIndex].Value.ToString().Trim())) { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = 0.0M; }
                else { SqlComm.Parameters.Add("@RMPrice", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.dgvHandleData["RM Price", iCellIndex].Value.ToString().Trim()), 6); }
                SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.dgvHandleData["Item No", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = this.dgvHandleData["RM Currency", iCellIndex].Value.ToString().Trim().ToUpper();

                if (strSwitch == "Add")
                { SqlComm.CommandText = @"INSERT INTO B_8A_Reference([RM Customs Code], [RM Price], [Item No], [RM Currency]) " + 
                                         "VALUES(@RMCustomsCode, @RMPrice, @ItemNo, @RMCurrency)"; }

                if (strSwitch == "Edit")
                { SqlComm.CommandText = @"UPDATE B_8A_Reference SET [RM Customs Code] = @RMCustomsCode, [RM Price] = @RMPrice " + 
                                         "WHERE [Item No] = @ItemNo AND [RM Currency] = @RMCurrency"; }
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[6])
            {
                if (String.IsNullOrEmpty(this.dgvHandleData["Rate", iCellIndex].Value.ToString().Trim()))
                { SqlComm.Parameters.Add("@Rate", SqlDbType.Decimal).Value = 0.0M; }
                else
                { SqlComm.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.dgvHandleData["Rate", iCellIndex].Value.ToString().Trim()), 6); }
                if (String.IsNullOrEmpty(this.dgvHandleData["Effective Date", iCellIndex].Value.ToString().Trim()))
                { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy")); }
                else
                { SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dgvHandleData["Effective Date", iCellIndex].Value.ToString().Trim()); }
                SqlComm.Parameters.Add("@Object", SqlDbType.NVarChar).Value = this.dgvHandleData["Object", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.CommandText = @"INSERT INTO B_ExchangeRate([Rate], [Created Date], [Object]) VALUES(@Rate, @CreatedDate, @Object)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[7])
            {
                SqlComm.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = this.dgvHandleData["Short Name", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@Address", SqlDbType.NVarChar).Value = this.dgvHandleData["Address", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@Code", SqlDbType.NVarChar).Value = this.dgvHandleData["Code", iCellIndex].Value.ToString().Trim();
                SqlComm.CommandText = @"INSERT INTO B_Address([Short Name], [Address], [Code]) VALUES(@ShortName, @Address, @Code)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[8])
            {
                SqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = this.dgvHandleData["IE Type", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.CommandText = @"INSERT INTO B_IEType([IE Type]) VALUES(@IEType)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[9])
            {
                SqlComm.Parameters.Add("@MaxLine", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["MaxLine", iCellIndex].Value.ToString().Trim());
                SqlComm.CommandText = @"INSERT INTO B_InvoiceParameter([MaxLine]) VALUES(@MaxLine)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[10])
            {
                SqlComm.Parameters.Add("@MaxLine", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["MaxLine", iCellIndex].Value.ToString().Trim());
                SqlComm.CommandText = @"INSERT INTO B_TaxParameter([MaxLine]) VALUES(@MaxLine)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[11])
            {
                SqlComm.Parameters.Add("@MinDeposit", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Min Deposit", iCellIndex].Value.ToString().Trim());
                SqlComm.Parameters.Add("@maxDeposit", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Max Deposit", iCellIndex].Value.ToString().Trim());
                SqlComm.Parameters.Add("@AvailableBal", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Available Balance", iCellIndex].Value.ToString().Trim());
                SqlComm.CommandText = @"INSERT INTO B_DepositScope([Min Deposit], [Max Deposit], [Available Balance]) VALUES(@MinDeposit, @maxDeposit, @AvailableBal)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[12])
            {
                SqlComm.Parameters.Add("@MTIPrefix", SqlDbType.NVarChar).Value = this.dgvHandleData["MTI Prefix", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@Type", SqlDbType.NVarChar).Value = this.dgvHandleData["Type", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = this.dgvHandleData["Remark", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@Note", SqlDbType.NVarChar).Value = this.dgvHandleData["Note", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.CommandText = @"INSERT INTO B_MTI_Prefix([MTI Prefix], [Type], [Remark], [Note]) VALUES(@MTIPrefix, @Type, @Remark, @Note)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[13])
            {
                if (String.IsNullOrEmpty(this.dgvHandleData["Ratio", iCellIndex].Value.ToString().Trim())) { SqlComm.Parameters.Add("@Ratio", SqlDbType.Decimal).Value = 0.0M; }
                else { SqlComm.Parameters.Add("@Ratio", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.dgvHandleData["Ratio", iCellIndex].Value.ToString().Trim()), 6); }
                SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dgvHandleData["Created Date", iCellIndex].Value.ToString().Trim());
                SqlComm.CommandText = @"INSERT INTO B_WeightRatio([Ratio], [Created Date]) VALUES(@Ratio, @CreatedDate)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[14])
            {
                if (String.IsNullOrEmpty(this.dgvHandleData["Ratio Begin", iCellIndex].Value.ToString().Trim())) { SqlComm.Parameters.Add("@RatioBegin", SqlDbType.Decimal).Value = 0.0M; }
                else { SqlComm.Parameters.Add("@RatioBegin", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.dgvHandleData["Ratio Begin", iCellIndex].Value.ToString().Trim()), 2); }
                if (String.IsNullOrEmpty(this.dgvHandleData["Ratio End", iCellIndex].Value.ToString().Trim())) { SqlComm.Parameters.Add("@RatioEnd", SqlDbType.Decimal).Value = 1.0M; }
                else { SqlComm.Parameters.Add("@RatioEnd", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(this.dgvHandleData["Ratio End", iCellIndex].Value.ToString().Trim()), 2); }
                SqlComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dgvHandleData["Created Date", iCellIndex].Value.ToString().Trim());
                SqlComm.CommandText = @"INSERT INTO B_YieldRatio([Ratio Begin], [Ratio End], [Created Date]) VALUES(@RatioBegin, @RatioEnd, @CreatedDate)";
            }

            else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[15])
            {
                if (String.IsNullOrEmpty(this.dgvHandleData["OF Instruction Date", iCellIndex].Value.ToString().Trim())) { SqlComm.Parameters.Add("@OFInstructionDate", SqlDbType.DateTime).Value = DBNull.Value; }
                else { SqlComm.Parameters.Add("@OFInstructionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(this.dgvHandleData["OF Instruction Date", iCellIndex].Value.ToString().Trim()); }
                SqlComm.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = this.dgvHandleData["Order No", iCellIndex].Value.ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(this.dgvHandleData["Order Qty", iCellIndex].Value.ToString().Trim())) { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = 0; }
                else { SqlComm.Parameters.Add("@OrderQty", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["Order Qty", iCellIndex].Value.ToString().Trim()); }
                if (String.IsNullOrEmpty(this.dgvHandleData["GongDan Qty", iCellIndex].Value.ToString().Trim())) { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = 0; }
                else { SqlComm.Parameters.Add("@GongDanQty", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["GongDan Qty", iCellIndex].Value.ToString().Trim()); }
                SqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = this.dgvHandleData["IE Type", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvHandleData["Batch No", iCellIndex].Value.ToString().Trim().ToUpper();
                SqlComm.Parameters.Add("@EssLine", SqlDbType.NVarChar).Value = this.dgvHandleData["ESS/LINE", iCellIndex].Value.ToString().Trim().ToUpper();

                if (strSwitch == "Add")
                {
                    SqlComm.CommandText = @"INSERT INTO B_OrderFulfillment([OF Instruction Date], [Order No], [Order Qty], [GongDan Qty], [IE Type], [Batch No], [ESS/LINE]) " +
                                           "VALUES(@OFInstructionDate, @OrderNo, @OrderQty, @GongDanQty, @IEType, @BatchNo, @EssLine)";
                }

                if (strSwitch == "Edit")
                {
                    SqlComm.CommandText = @"UPDATE B_OrderFulfillment SET [OF Instruction Date] = @OFInstructionDate, [Order No] = @OrderNo, [Order Qty] = @OrderQty, " +
                                           "[GongDan Qty] = @GongDanQty, [IE Type] = @IEType WHERE [Batch No] = @BatchNo AND [ESS/LINE] = @EssLine";
                }
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
            if (strSwitch == "Add")
            { this.dgvHandleData.Rows.RemoveAt(this.dgvHandleData.RowCount - 1); }
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
                    SqlComm.Parameters.Add("@FGChineseName", SqlDbType.NVarChar).Value = this.dgvHandleData["FG CHN Name", iCellIndex].Value.ToString().Trim();
                    SqlComm.Parameters.Add("@RMCategory", SqlDbType.NVarChar).Value = this.dgvHandleData["RM Category", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = @"DELETE FROM B_Drools WHERE [FG CHN Name] = @FGChineseName AND [RM Category] = @RMCategory";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[2])
                {
                    SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = this.dgvHandleData["Grade", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = @"DELETE FROM B_HS WHERE [Grade] = @Grade";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[4])
                {
                    SqlComm.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = this.dgvHandleData["Grade", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = @"DELETE FROM B_Allocation WHERE [Grade] = @Grade";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[5])
                {
                    SqlComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.dgvHandleData["Item No", iCellIndex].Value.ToString().Trim();
                    SqlComm.Parameters.Add("@RMCurrency", SqlDbType.NVarChar).Value = this.dgvHandleData["RM Currency", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = @"DELETE FROM B_8A_Reference WHERE [Item No] = @ItemNo AND [RM Currency] = @RMCurrency";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[6])
                {
                    SqlComm.Parameters.Add("@Object", SqlDbType.NVarChar).Value = this.dgvHandleData["Object", iCellIndex].Value.ToString().Trim();
                    SqlComm.Parameters.Add("@CreatedDate", SqlDbType.Date).Value = Convert.ToDateTime(this.dgvHandleData["Effective Date", iCellIndex].Value.ToString().Trim());
                    SqlComm.CommandText = @"DELETE FROM B_ExchangeRate WHERE [Object] = @Object AND [Created Date] = @CreatedDate";
                }                            

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[7])
                {
                    SqlComm.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = this.dgvHandleData["Short Name", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = @"DELETE FROM B_Address WHERE [Short Name] = @ShortName";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[8])
                {
                    SqlComm.Parameters.Add("@IEType", SqlDbType.NVarChar).Value = this.dgvHandleData["IE Type", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = @"DELETE FROM B_IEType WHERE [IE Type] = @IEType";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[9])
                {
                    SqlComm.Parameters.Add("@MaxLine", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["MaxLine", iCellIndex].Value.ToString().Trim());
                    SqlComm.CommandText = @"DELETE FROM B_InvoiceParameter WHERE [MaxLine] = @MaxLine";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[10])
                {
                    SqlComm.Parameters.Add("@MaxLine", SqlDbType.Int).Value = Convert.ToInt32(this.dgvHandleData["MaxLine", iCellIndex].Value.ToString().Trim());
                    SqlComm.CommandText = @"DELETE FROM B_TaxParameter WHERE [MaxLine] = @MaxLine";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[11])
                {
                    SqlComm.Parameters.Add("@MinDeposit", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Min Deposit", iCellIndex].Value.ToString().Trim());
                    SqlComm.Parameters.Add("@MaxDeposit", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Max Deposit", iCellIndex].Value.ToString().Trim());
                    SqlComm.Parameters.Add("@AvailableBal", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Available Balance", iCellIndex].Value.ToString().Trim());
                    SqlComm.CommandText = @"DELETE FROM B_DepositScope WHERE [Min Deposit] = @MinDeposit AND [Max Deposit] = @MaxDeposit AND [Available Balance] = @AvailableBal";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[12])
                {
                    string strMTI = this.dgvHandleData["MTI Prefix", iCellIndex].Value.ToString().Trim();
                    if (String.IsNullOrEmpty(strMTI))
                    {
                        SqlComm.Parameters.Add("@Type", SqlDbType.NVarChar).Value = this.dgvHandleData["Type", iCellIndex].Value.ToString().Trim();
                        SqlComm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = this.dgvHandleData["Remark", iCellIndex].Value.ToString().Trim();
                        SqlComm.CommandText = @"DELETE FROM B_MTI_Prefix WHERE [MTI Prefix] IS NULL AND [Type] = @Type AND [Remark] = @Remark";
                    }
                    else
                    {
                        SqlComm.Parameters.Add("@MTIPrefix", SqlDbType.NVarChar).Value = this.dgvHandleData["MTI Prefix", iCellIndex].Value.ToString().Trim();
                        SqlComm.Parameters.Add("@Type", SqlDbType.NVarChar).Value = this.dgvHandleData["Type", iCellIndex].Value.ToString().Trim();
                        SqlComm.CommandText = @"DELETE FROM B_MTI_Prefix WHERE [MTI Prefix] = @MTIPrefix AND [Type] = @Type";
                    }
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[13])
                {
                    SqlComm.Parameters.Add("@Ratio", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Ratio", iCellIndex].Value.ToString().Trim());
                    SqlComm.CommandText = @"DELETE FROM B_WeightRatio WHERE [Ratio] = @Ratio";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[14])
                {
                    SqlComm.Parameters.Add("@RatioBegin", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Ratio Begin", iCellIndex].Value.ToString().Trim());
                    SqlComm.Parameters.Add("@RatioEnd", SqlDbType.Decimal).Value = Convert.ToDecimal(this.dgvHandleData["Ratio End", iCellIndex].Value.ToString().Trim());
                    SqlComm.CommandText = @"DELETE FROM B_YieldRatio WHERE [Ratio Begin] = @RatioBegin AND [Ratio End] = @RatioEnd";
                }

                else if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == strList[15])
                {
                    SqlComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = this.dgvHandleData["Batch No", iCellIndex].Value.ToString().Trim();
                    SqlComm.Parameters.Add("@EssLine", SqlDbType.NVarChar).Value = this.dgvHandleData["ESS/LINE", iCellIndex].Value.ToString().Trim();
                    SqlComm.CommandText = @"DELETE FROM B_OrderFulfillment WHERE [Batch No] = @BatchNo AND [ESS/LINE] = @EssLine";
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
                MessageBox.Show("No data exist to download.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

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
                    StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode); //There may be messy code
                    StringBuilder sb = new StringBuilder();
                    for (int n = 0; n < this.dgvHandleData.Columns.Count; n++)
                    { sb.Append(this.dgvHandleData.Columns[n].Name.Trim() + "\t"); }
                    sb.Append(Environment.NewLine);

                    if (this.bindingNavigatorcmbItem.ComboBox.SelectedValue.ToString() == "Order Fulfillment")
                    {
                        for (int i = (m - 1) * PageRow; i < this.dgvHandleData.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < this.dgvHandleData.Columns.Count; j++)
                            {
                                if (j == 0) { sb.Append("'" + this.dgvHandleData[j, i].Value.ToString().Trim() + "\t"); }
                                else { sb.Append(this.dgvHandleData[j, i].Value.ToString().Trim() + "\t"); }
                            }
                            sb.Append(Environment.NewLine);
                        }
                    }
                    else
                    {
                        for (int i = (m - 1) * PageRow; i < this.dgvHandleData.Rows.Count && i < m * PageRow; i++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            for (int j = 0; j < this.dgvHandleData.Columns.Count; j++)
                            { sb.Append(this.dgvHandleData[j, i].Value.ToString().Trim() + "\t"); }
                            sb.Append(Environment.NewLine);
                        }
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
                dvFillDGV.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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
            if (this.dgvHandleData.CurrentCell != null)
            {
                string strColumnName = this.dgvHandleData.Columns[this.dgvHandleData.CurrentCell.ColumnIndex].Name;
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
                dvFillDGV.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}