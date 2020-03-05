using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using Advertise.ReportGenerators;
using Advertise.ReportGenerators.Models;
using System.Windows.Forms;
using System.Data;

namespace Advertise.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ExcelWorker EW { get; private set; }
        public DataBaseSynchronizer DB { get; private set; }
        public ReportGenerator SelectedGenerator { get; set; }
        public ReportWindow(DataBaseSynchronizer dataBaseSync, ExcelWorker excelWorker)
        {
            InitializeComponent();
            DB = dataBaseSync;
            ListBoxMonthSelector.SelectionChanged += ListBoxMonthSelector_Selected;
            this.EW = excelWorker;
        }
        public void SetUpReport()
        {
            if(ListBoxMonthSelector.SelectedItems.Count == 1 && ComboBoxReportType.Text != string.Empty)
            {
                string selectedMonth = ((ListBoxItem)ListBoxMonthSelector.SelectedItem).Content.ToString();
                SelectedGenerator.Month = selectedMonth;
                grid.ItemsSource = SelectedGenerator.GenerateReportAsList();
                var cols = SelectedGenerator.Names;
                for(int i = 0; i < cols.Count; i++)
                {
                    grid.Columns[i].Header = cols[i];
                }

                ButtonExportSelected.IsEnabled = true;
            }
        }
        private void ComboBoxItem_Selected_CAC(object sender, RoutedEventArgs e)
        {
            SelectedGenerator = new CACReport(DB);
        }

        private void ComboBoxItem_Selected_ROMI(object sender, RoutedEventArgs e)
        {
            SelectedGenerator = new ROMIReport(DB);
        }

        private void ComboBoxItem_Selected_AOV(object sender, RoutedEventArgs e)
        {
            SelectedGenerator = new AOVReport(DB);
        }

        private void ListBoxMonthSelector_Selected(object sender, RoutedEventArgs e)
        {
            SetUpReport();
        }

        private void ComboBoxReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxMonthSelector.SelectedIndex = -1;
            ButtonExportSelected.IsEnabled = false;
            SetUpReport();
        }

        private void ButtonExportSelected_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string selectedMonth = ((ListBoxItem)ListBoxMonthSelector.SelectedItem).Content.ToString();
                SelectedGenerator.Month = selectedMonth;
                DataTable exp = SelectedGenerator.GenerateReoport();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = $"{exp.TableName}.xlsx";
                saveFileDialog.Title = $"Сохранение отчёта \"{exp.TableName}\"";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        EW.ExportTable(exp, saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Ошибка экспортирования");
                    }
                }
            }
        }
    }
}
