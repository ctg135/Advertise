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
        /// <summary>
        /// Список имен таблиц базы данных на русском
        /// </summary>
        private Dictionary<string, string> TableNames;
        /// <summary>
        /// Список столбцов таблиц на русском
        /// </summary>
        private Dictionary<string, string> CurrentFields;
        public MainWindow(DataBaseSynchronizer dataBaseSynchronizer)
        {
            InitializeComponent();
            DB = dataBaseSynchronizer;
            TableNames = DB.GetTableNamesReversed();
            TableSelector.ItemsSource = DB.GetTableNames().Values;
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
                    SetUpTableNames(TableName);
                    CurrentFields = new Dictionary<string, string>();
                    CurrentFields = DB.ColumnNamesReversed(TableNames[TableName]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
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
        /// <summary>
        /// Функция-обработчик измененения значения ячейки
        /// </summary>
        /// <param name="sender">Отправитель сообщения</param>
        /// <param name="e">Аргументы события</param>
        private void grid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                DB.UpdateTable(
                    TableNames[TableSelector.Text], 
                    ((Models.IModel)grid.Items[e.Row.GetIndex()]).Id.ToString(), 
                    CurrentFields[e.Column.Header.ToString()], 
                    (e.EditingElement as TextBox).Text
                );
            }
            catch(Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show(ex.Message);
            }
        }
        private void grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ButtonDelete.IsEnabled = grid.SelectedCells.Count > 0;
        }
        /// <summary>
        /// Функция-обработчик нажатия на кнопку удаления выбранных записей
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Параметры события</param>
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            List<string> DeletingID = new List<string>();
            foreach (var row in grid.SelectedItems)
            {
                DeletingID.Add(((Models.IModel)row).Id.ToString());
            }
            DB.DeleteSome(TableNames[TableSelector.Text], DeletingID);
            SetUpTable(TableSelector.Text);
        }
    }

}
