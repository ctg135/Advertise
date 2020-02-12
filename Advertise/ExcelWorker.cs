using System;
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
            ex.Visible = false;
            Excel.Workbook book = ex.Workbooks.Add();
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            sheet.Name = table.TableName;

            int maxRow = table.Rows.Count;
            int maxCol = table.Columns.Count;
            // Вывод заголовка таблицы
            for (int col = 1; col < maxCol + 1; col++)
            {
                sheet.Cells[1, col] = table.Columns[col - 1].ColumnName;
            }
            // Форматирование заголовка
            Excel.Range range1 = sheet.Range[ sheet.Cells[1, 1], sheet.Cells[1, maxCol ] ];
            range1.Font.Bold = true;
            range1.EntireColumn.AutoFit();
            // Вывод содержимого таблицы
            for (int row = 2; row < maxRow + 2; row++)
            {
                for(int col = 1; col < maxCol + 1; col++)
                {
                    sheet.Cells[row, col] = table.Rows[row - 2].ItemArray[col - 1].ToString();
                }
            }
            //Форматирование содержимого
            // ....
            range1 = sheet.Range[sheet.Cells[2, 1], sheet.Cells[maxRow, maxCol]];
            range1.EntireColumn.AutoFit();
            ex.Application.ActiveWorkbook.SaveAs(path);
            ex.Quit();
        }
    }
}
