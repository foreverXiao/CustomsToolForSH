using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class PopUpInfoForm : Form
    {
        public PopUpInfoForm()
        {
            InitializeComponent();
        }

        private DataTable dtRecord = null;
        public DataTable DataTableRecord
        {
            get { return dtRecord; }
            set { dtRecord = value; }
        }

        private void PopUpInfoForm_Load(object sender, EventArgs e)
        {
            this.dgvPopupInfo.DataSource = dtRecord;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            //excel.get_Range(excel.Cells[1, 1], excel.Cells[dtRecord.Rows.Count + 1, dtRecord.Columns.Count]).NumberFormatLocal = "@";
            for (int i = 0; i < dtRecord.Rows.Count; i++)
            {
                for (int j = 0; j < dtRecord.Columns.Count; j++)
                { excel.Cells[i + 2, j + 1] = dtRecord.Rows[i][j].ToString().Trim(); }
            }

            for (int k = 0; k < dtRecord.Columns.Count; k++)
            { excel.Cells[1, k + 1] = dtRecord.Columns[k].ColumnName.Trim(); }

            //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, dtRecord.Columns.Count]).Font.Bold = true;
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[1, dtRecord.Columns.Count]).AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[dtRecord.Rows.Count + 1, dtRecord.Columns.Count]).Font.Name = "Verdana";
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[dtRecord.Rows.Count + 1, dtRecord.Columns.Count]).Font.Size = 9;
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[dtRecord.Rows.Count + 1, dtRecord.Columns.Count]).Borders.LineStyle = 1;
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
            excel.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }
    }
}
