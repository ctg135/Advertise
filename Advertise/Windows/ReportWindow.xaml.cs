using Advertise.ReportGenerators;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Advertise.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ExcelWorker EW { get; private set; }
        public DataBaseSynchronizer DB { get; private set; }
        public IReportGenerator SelectedGenerator { get; set; }
        public ReportWindow(DataBaseSynchronizer dataBaseSync, ExcelWorker excelWorker)
        {
            InitializeComponent();
            DB = dataBaseSync;
            ListBoxMonthSelector.SelectionChanged += ListBoxMonthSelector_Selected;
            this.EW = excelWorker;
        }
        /// <summary>
        /// Производит вывод таблицы отчета
        /// </summary>
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
        /// <summary>
        /// Для генерации выбран отчет <c>CAC</c>
        /// </summary>
        private void ComboBoxItem_Selected_CAC(object sender, RoutedEventArgs e)
        {
            SelectedGenerator = new CACReport(DB);
        }
        /// <summary>
        /// Для генерации выбран отчет <c>ROMI</c>
        /// </summary>
        private void ComboBoxItem_Selected_ROMI(object sender, RoutedEventArgs e)
        {
            SelectedGenerator = new ROMIReport(DB);
        }
        /// <summary>
        /// Для генерации выбран отчет <c>AOV</c>
        /// </summary>
        private void ComboBoxItem_Selected_AOV(object sender, RoutedEventArgs e)
        {
            SelectedGenerator = new AOVReport(DB);
        }
        /// <summary>
        /// Вывод таблицы отчёта при выборе элемнта списка
        /// </summary>
        private void ListBoxMonthSelector_Selected(object sender, RoutedEventArgs e)
        {
            SetUpReport();
        }
        /// <summary>
        /// Очистка выбранных элементов при смене типа отчета
        /// </summary>
        private void ComboBoxReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxMonthSelector.SelectedIndex = -1;
            ButtonExportSelected.IsEnabled = false;
            grid.Columns.Clear();
            grid.ItemsSource = null;
            SetUpReport();
        }
        /// <summary>
        /// Нажатие на кнопку создания отчёта
        /// </summary>
        private void ButtonExportSelected_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) /// Создание диалога для сохранения файла
            {
                string selectedMonth = ((ListBoxItem)ListBoxMonthSelector.SelectedItem).Content.ToString();
                SelectedGenerator.Month = selectedMonth;
                DataTable exp = SelectedGenerator.GenerateReoport();  /// Создание отчета
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = $"{exp.TableName}.xlsx";
                saveFileDialog.Title = $"Сохранение отчёта \"{exp.TableName}\"";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        Cursor = System.Windows.Input.Cursors.Wait;
                        EW.ExportTable(exp, saveFileDialog.FileName); /// Вывод отчёта
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Ошибка экспортирования");
                    }
                    finally
                    {
                        Cursor = System.Windows.Input.Cursors.Arrow;
                    }
                    System.Windows.MessageBox.Show("Экспорт завершен!");
                }
            }
        }
    }
}
