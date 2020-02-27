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

namespace Advertise.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public DataBaseSynchronizer DB { get; private set; }
        public ReoportGenerator SelectedGenerator { get; set; }
        public ReportWindow(DataBaseSynchronizer dataBaseSync, ExcelWorker excelWorker)
        {
            InitializeComponent();
            DB = dataBaseSync;
            ListBoxMonthSelector.SelectionChanged += ListBoxMonthSelector_Selected;
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
            }
        }
        private void ComboBoxItem_Selected_CAC(object sender, RoutedEventArgs e)
        {
            SelectedGenerator = new CACReport(DB);
            SetUpReport();
        }

        private void ComboBoxItem_Selected_ROMI(object sender, RoutedEventArgs e)
        {
            //...
            SetUpReport();
        }

        private void ComboBoxItem_Selected_AOV(object sender, RoutedEventArgs e)
        {
            //
            SetUpReport();
        }

        private void ListBoxMonthSelector_Selected(object sender, RoutedEventArgs e)
        {
            SetUpReport();
        }
    }
}
