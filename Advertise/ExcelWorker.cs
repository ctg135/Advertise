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
        public DataBaseSynchronizer DB { get; private set; }
        public ExcelWorker(DataBaseSynchronizer dataBaseSync)
        {
            DB = dataBaseSync;
        }
        public void ExportTable(string TableName, string path)
        {
            // Выбор данных из базы данных
            DataTable table = new DataTable();
            try
            {
                table = DB.SelectTable(TableName);
            }
            catch(Exception e)
            {
                throw e;
            }
            // Вывод данных
            Excel.Application ex = new Excel.Application();
            try
            {
                ex.SheetsInNewWorkbook = 1;
                ex.Visible = false;
                Excel.Workbook book = ex.Workbooks.Add();
                Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
                ExportTableToSheet(table, sheet);
                ex.Application.ActiveWorkbook.SaveAs(path);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ex.Quit();
            }
        }
        private void ExportTableToSheet(DataTable Table, Excel.Worksheet Sheet)
        {
            Sheet.Name = Table.TableName;
            int maxRow = Table.Rows.Count;
            int maxCol = Table.Columns.Count;
            for (int col = 1; col < maxCol + 1; col++)
            {
                Sheet.Cells[1, col] = Table.Columns[col - 1].ColumnName;
            }
            Excel.Range range1 = Sheet.Range[Sheet.Cells[1, 1], Sheet.Cells[1, maxCol]];
            range1.Font.Bold = true;
            range1.EntireColumn.AutoFit();
            for (int row = 2; row < maxRow + 2; row++)
            {
                for (int col = 1; col < maxCol + 1; col++)
                {
                    Sheet.Cells[row, col] = Table.Rows[row - 2].ItemArray[col - 1].ToString();
                }
            }
            range1 = Sheet.Range[Sheet.Cells[2, 1], Sheet.Cells[maxRow, maxCol]];
            range1.EntireColumn.AutoFit();
        }
    }
}
