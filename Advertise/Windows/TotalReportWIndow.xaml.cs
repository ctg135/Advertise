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

namespace Advertise.Windows
{
    /// <summary>
    /// </summary>
    public partial class TotalReportWIndow : Window
    {
        public ExcelWorker EW { get; private set; }
        public DataBaseSynchronizer DB { get; private set; }
        public TotalReportWIndow(DataBaseSynchronizer dataBaseSync, ExcelWorker excelWorker)
        {
            DB = dataBaseSync;
            EW = excelWorker;

            InitializeComponent();

            foreach (Controls.ListBoxItem item in ListBoxMonths.Items)
            {
                ((Controls.CheckBox)item.Content).Click += CheckButtonExportState;
            }

            foreach(Controls.ListBoxItem item in ListBoxReports.Items)
            {
                ((Controls.CheckBox)item.Content).Click += CheckButtonExportState;
            }

            CheckButtonExportState(this, new RoutedEventArgs());
        }
        
        private void ButtonCheckMonths_Click     (object sender, RoutedEventArgs e) => SetAllListBoxItemsToState(ListBoxMonths, true);

        private void ButtonNonCheckMonths_Click(object sender, RoutedEventArgs e)
        {
            SetAllListBoxItemsToState(ListBoxMonths, false);
            CheckButtonExportState(this, new RoutedEventArgs());
        }

        private void ButtonCheckReports_Click    (object sender, RoutedEventArgs e) => SetAllListBoxItemsToState(ListBoxReports, true);

        private void ButtonNonCheckReports_Click(object sender, RoutedEventArgs e)
        {
            SetAllListBoxItemsToState(ListBoxReports, false);
            CheckButtonExportState(this, new RoutedEventArgs());
        }

        private void SetAllListBoxItemsToState(Controls.ListBox ListBox, bool State)
        {
            foreach (Controls.ListBoxItem item in ListBox.Items)
            {
                Controls.CheckBox box = (Controls.CheckBox)item.Content;
                box.IsChecked = State;
            }
            ListBox.SelectedIndex = -1;
        }

        private void CheckButtonExportState(object sender, RoutedEventArgs e)
        {
            bool AnyMonthSelected = false;
            bool AnyReportSelected = false;

            foreach (Controls.ListBoxItem item in ListBoxMonths.Items)
            {
                if ( (bool)((Controls.CheckBox)item.Content).IsChecked )
                {
                    AnyMonthSelected = true;
                    break;
                }
            }

            foreach (Controls.ListBoxItem item in ListBoxReports.Items)
            {
                if ( (bool)((Controls.CheckBox)item.Content).IsChecked )
                {
                    AnyReportSelected = true;
                    break;
                }
            }

            ButtonExport.IsEnabled = AnyMonthSelected && AnyReportSelected;
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

                        if (Months.Count == 0 || ReportTypes.Count == 0) throw new Exception("Отчёт не выбран!");

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
