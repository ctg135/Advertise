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

namespace Advertise.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBaseSynchronizer DB;
        public MainWindow(DataBaseSynchronizer dataBaseSynchronizer)
        {
            InitializeComponent();
            DB = dataBaseSynchronizer;
            TableSelector.ItemsSource = DB.GetTableNames().Values;
            TableSelector.SelectionChanged += TableSelector_SelectionChanged;
        }
        private void TableSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetUpTable(e.AddedItems[0].ToString());
        }
        public void SetUpTable(string TableName)
        {
            try
            {
                if (!string.IsNullOrEmpty(TableName))
                {
                    grid.ItemsSource = DB.SelectTable(DB.GetTableNamesReversed()[TableName]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}
