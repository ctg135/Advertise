using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Windows;
using Controls = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Advertise.ReportGenerators;
using System.Data;

using System.Diagnostics;

namespace Advertise.Windows
{
    /// <summary>
    /// Логика взаимодействия для TotalReportWIndow.xaml
    /// </summary>
    public partial class TotalReportWIndow : Window
    {
        public ExcelWorker EW { get; private set; }
        public DataBaseSynchronizer DB { get; private set; }
        public TotalReportWIndow(DataBaseSynchronizer dataBaseSync, ExcelWorker excelWorker)
        {
            InitializeComponent();
            DB = dataBaseSync;
            EW = excelWorker;
        }
        
        private void ButtonCheckMonths_Click     (object sender, RoutedEventArgs e) => SetAllListBoxItemsToState(ListBoxMonths, true);

        private void ButtonNonCheckMonths_Click  (object sender, RoutedEventArgs e) => SetAllListBoxItemsToState(ListBoxMonths, false);

        private void ButtonCheckReports_Click    (object sender, RoutedEventArgs e) => SetAllListBoxItemsToState(ListBoxReports, true);

        private void ButtonNonCheckReports_Click (object sender, RoutedEventArgs e) => SetAllListBoxItemsToState(ListBoxReports, false);

        private void SetAllListBoxItemsToState(Controls.ListBox ListBox, bool State)
        {
            foreach (Controls.ListBoxItem item in ListBox.Items)
            {
                Controls.CheckBox box = (Controls.CheckBox)item.Content;
                box.IsChecked = State;
            }
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = $"Отчёт на {DateTime.Now.ToString("d")}.xlsx";
                saveFileDialog.Title = $"Сохранение настроенного отчёта";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        List<IReportGenerator> Reports = new List<IReportGenerator>();

                        List<string> ReportTypes = new List<string>();

                        foreach (Controls.ListBoxItem listBoxItem in ListBoxReports.Items)
                        {
                            Controls.CheckBox checkBox = (Controls.CheckBox)listBoxItem.Content;
                            if ((bool)checkBox.IsChecked)
                            {
                                ReportTypes.Add(checkBox.Content.ToString());
                            }
                        }

                        List<string> Months = new List<string>();

                        foreach (Controls.ListBoxItem listBoxItem in ListBoxMonths.Items)
                        {
                            Controls.CheckBox checkBox = (Controls.CheckBox)listBoxItem.Content;
                            if ((bool)checkBox.IsChecked)
                            {
                                Months.Add(checkBox.Content.ToString());
                            }
                        }

                        foreach (string month in Months)
                        {
                            foreach (string reportType in ReportTypes)
                            {
                                switch (reportType)
                                {
                                    case "Отчёт AOV":
                                        Reports.Add(new AOVReport(DB, month));
                                        break;
                                    case "Отчёт CAC":
                                        Reports.Add(new CACReport(DB, month));
                                        break;
                                    case "Отчёт ROMI":
                                        Reports.Add(new ROMIReport(DB, month));
                                        break;
                                }
                            }
                        }
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        foreach (IReportGenerator report in Reports)
                        {
                            Debug.WriteLine($"{report} {report.Month}");
                        }

                        List<DataTable> tables = new List<DataTable>();

                        foreach (IReportGenerator report in Reports)
                        {
                            tables.Add(report.GenerateReoport());
                        }


                        EW.ExportTables(tables.ToArray(), saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Ошибка экспортирования");
                    }
                    System.Windows.MessageBox.Show("Экспорт завершен!");
                }
            }
        }
    }
}
