﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;


namespace Advertise
{
    public class ExcelWorker
    {
        public void ExportTable(DataTable table, string path)
        {
            Excel.Application ex = new Excel.Application();
            ex.SheetsInNewWorkbook = 1;
            ex.Visible = true;
            Excel.Workbook book = ex.Workbooks.Add();
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            sheet.Name = "LOL";
            MessageBox.Show(table.TableName);

        }
    }
}
