using Advertise.ReportGenerators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using Controls = System.Windows.Controls;

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

            foreach (Controls.ListBoxItem item in ListBoxMonths.Items) /// Добавление обработчиков для нажатых месяцов
            {
                ((Controls.CheckBox)item.Content).Click += CheckButtonExportState;
            }

            foreach(Controls.ListBoxItem item in ListBoxReports.Items) /// Добавление обработчиков для нажатых отчетов
            {
                ((Controls.CheckBox)item.Content).Click += CheckButtonExportState;
            }

            CheckButtonExportState(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Выбрать все месяцы
        /// </summary>
        private void ButtonCheckMonths_Click(object sender, RoutedEventArgs e)
        {
            SetAllListBoxItemsToState(ListBoxMonths, true);
            CheckButtonExportState(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Убрать выделения на месяцах
        /// </summary>
        private void ButtonNonCheckMonths_Click(object sender, RoutedEventArgs e)
        {
            SetAllListBoxItemsToState(ListBoxMonths, false);
            CheckButtonExportState(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Выбрать все типы отчета
        /// </summary>
        private void ButtonCheckReports_Click(object sender, RoutedEventArgs e)
        {
            SetAllListBoxItemsToState(ListBoxReports, true);
            CheckButtonExportState(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Убрать выделения на отчётах
        /// </summary>
        private void ButtonNonCheckReports_Click(object sender, RoutedEventArgs e)
        {
            SetAllListBoxItemsToState(ListBoxReports, false);
            CheckButtonExportState(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Устанавливает все элементы списка к соостоянию
        /// </summary>
        /// <param name="ListBox">Список</param>
        /// <param name="State">Состояние</param>
        private void SetAllListBoxItemsToState(Controls.ListBox ListBox, bool State)
        {
            foreach (Controls.ListBoxItem item in ListBox.Items)
            {
                Controls.CheckBox box = (Controls.CheckBox)item.Content;
                box.IsChecked = State;
            }
            ListBox.SelectedIndex = -1;
        }
        /// <summary>
        /// Проверка и установка включенности кнопки создания отчета
        /// </summary>
        private void CheckButtonExportState(object sender, RoutedEventArgs e)
        {
            bool AnyMonthSelected = false;  /// Выбран ли один месяц
            bool AnyReportSelected = false; /// Выбран ли один отчет

            foreach (Controls.ListBoxItem item in ListBoxMonths.Items) /// Просмотр всех месяцев
            {
                if ( (bool)((Controls.CheckBox)item.Content).IsChecked ) /// Если хоть один выбран, выход из цикла
                {
                    AnyMonthSelected = true;
                    break;
                }
            }

            foreach (Controls.ListBoxItem item in ListBoxReports.Items) /// Просмотр всех отчётов
            {
                if ( (bool)((Controls.CheckBox)item.Content).IsChecked ) /// Если хоть один выбран, выход из цикла
                {
                    AnyReportSelected = true;
                    break;
                }
            }

            ButtonExport.IsEnabled = AnyMonthSelected && AnyReportSelected; /// Если выбран и месяц и отчет
        }


        
        /// <summary>
        /// Нажатие на кнопку сохранения отчета
        /// </summary>
        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) /// Создание файлового диалога
            {
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = $"Отчёт на {DateTime.Now.ToString("d")}.xlsx";
                saveFileDialog.Title = $"Сохранение настроенного отчёта";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) /// Если директория задана, создаем отчеты
                {
                    try 
                    {
                        Cursor = System.Windows.Input.Cursors.Wait;

                        List<IReportGenerator> Reports = new List<IReportGenerator>();  /// Список отчетов
                        List<string> ReportTypes = new List<string>();                  /// Список типов отчетов

                        foreach (Controls.ListBoxItem listBoxItem in ListBoxReports.Items)  /// Сбор типов отчетов
                        {
                            Controls.CheckBox checkBox = (Controls.CheckBox)listBoxItem.Content;
                            if ((bool)checkBox.IsChecked)
                            {
                                ReportTypes.Add(checkBox.Content.ToString());
                            }
                        }

                        List<string> Months = new List<string>(); /// Спсиок месяцев

                        foreach (Controls.ListBoxItem listBoxItem in ListBoxMonths.Items)   /// Сбор месяцев
                        {
                            Controls.CheckBox checkBox = (Controls.CheckBox)listBoxItem.Content;
                            if ((bool)checkBox.IsChecked)
                            {
                                Months.Add(checkBox.Content.ToString());
                            }
                        }

                        if (Months.Count == 0 || ReportTypes.Count == 0) throw new Exception("Отчёт не выбран!");

                        foreach (string month in Months) /// Для каждого выбранного месяца
                        {
                            foreach (string reportType in ReportTypes) /// Создать каждый тип отчета
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
                        List<DataTable> tables = new List<DataTable>(); /// Таблицы для отчета

                        foreach (IReportGenerator report in Reports)
                        {
                            tables.Add(report.GenerateReoport()); /// Генерация отчетов
                        }

                        EW.ExportTables(tables.ToArray(), saveFileDialog.FileName); /// Экспортирование отчетов
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
