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

namespace Advertise.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Экземпляр для работы с базой данных "Advertise"
        /// </summary>
        private DataBaseSynchronizer DB;
        private Dictionary<string, string> TableNames;
        public MainWindow(DataBaseSynchronizer dataBaseSynchronizer)
        {
            InitializeComponent();
            DB = dataBaseSynchronizer;
            TableNames = DB.GetTableNamesReversed();
            TableSelector.ItemsSource = DB.GetTableNames().Values;
            TableSelector.SelectionChanged += TableSelector_SelectionChanged;
        }
        /// <summary>
        /// Обработчик события выбор названия таблицы
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы события</param>
        private void TableSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetUpTable(e.AddedItems[0].ToString());
        }

        /// <summary>
        /// Функция для устаовки таблицы данных
        /// </summary>
        /// <param name="TableName">Имя таблицы в базе данных</param>
        public void SetUpTable(string TableName)
        {
            try
            {
                if (!string.IsNullOrEmpty(TableName))
                {
                    grid.ItemsSource = DB.SelectTable(TableNames[TableName]);
                }
                SetUpTableNames(TableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary> #####
        /// Функция для установки русских столбцов
        /// </summary>
        /// <param name="TableName">Имя таблицы в базе данных</param>
        public void SetUpTableNames(string TableName)
        {
            var names = DB.ColumnNames(TableNames[TableName]);
            foreach(var col in grid.Columns)
            {
                col.Header = names[col.Header.ToString()];
            }
        }
    }

}
