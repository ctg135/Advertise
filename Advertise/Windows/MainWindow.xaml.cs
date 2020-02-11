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
using System.Windows.Forms;
using System.Data;

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
        public DataBaseSynchronizer DB;
        /// <summary>
        /// Экземпляр для работы с файлами Excel
        /// </summary>
        public ExcelWorker excelWorker;
        /// <summary>
        /// Список имен таблиц базы данных на русском
        /// </summary>
        public Dictionary<string, string> TableNames;
        /// <summary>
        /// Экземпляр для контроля состояния порграммы
        /// </summary>
        private StateControllerMainWindow stater;
        /// <summary>
        /// Событие выбранной таблицы
        /// </summary>
        public event EventHandler TableSelected;
        /// <summary>
        /// Событие обновления таблицы
        /// </summary>
        public event EventHandler TableRefreshing;
        /// <summary>
        /// Событие выбранной строки
        /// </summary>
        public event EventHandler RowSelected;
        /// <summary>
        /// Событие выбранных строк
        /// </summary>
        public event EventHandler RowsSelected;
        /// <summary>
        /// Событие не выбранных строк
        /// </summary>
        public event EventHandler RowNotSelected;
        public MainWindow(DataBaseSynchronizer dataBaseSynchronizer, ExcelWorker excelWorker)
        {
            InitializeComponent();
            DB = dataBaseSynchronizer;
            this.excelWorker = excelWorker;
            TableNames = DB.GetTableNamesReversed();
            TableSelector.ItemsSource = DB.GetTableNames().Values;
            stater = new StateControllerMainWindow(this);
        }
        /// <summary>
        /// Функция для устаовки таблицы данных
        /// </summary>
        /// <param name="TableName">Имя таблицы на русском</param>
        public void SetUpTable(string TableName)
        {
            try
            {
                if (!string.IsNullOrEmpty(TableName))
                {
                    grid.ItemsSource = DB.SelectTable(TableNames[TableName]);
                    SetUpTableNames(TableName);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка заполнения таблицы", MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }
        /// <summary>
        /// Функция для установки русских столбцов
        /// </summary>
        /// <param name="TableName">Имя таблицы на русском</param>
        public void SetUpTableNames(string TableName)
        {
            var names = DB.ColumnNames(TableNames[TableName]);
            foreach (var col in grid.Columns)
            {
                col.Header = names[col.Header.ToString()];
            }
        }
        /// <summary>
        /// Обработчик события выбора таблицы из селектора
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы события</param>
        private void TableSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TableSelected(this, e);
            TableRefreshing(this, e);
        }
        /// <summary>
        /// Функция-обработчик события выбранной таблицы
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Параметры события</param>
        private void grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (grid.SelectedItems.Count == 1)
            {
                RowSelected(this, e);
            }
            else if(grid.SelectedItems.Count > 1)
            {
                RowsSelected(this, e);
            }
            else if(grid.SelectedItems.Count == 0)
            {
                RowNotSelected(this, e);
            }
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
            TableRefreshing(this, e);
        }
        /// <summary>
        /// Обработчик события нажатия на кнопку доабления записей
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Параметры события</param>
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Выбор токна добавления по имени таблицы
            switch(TableNames[TableSelector.Text])
            {
                case "source":
                    new AddWindows.Source(DB.DataBaseWorker).ShowDialog();
                    break;
                case "investments":
                    new AddWindows.Investment(DB.DataBaseWorker).ShowDialog();
                    break;
                case "clients":
                    new AddWindows.Client(DB.DataBaseWorker).ShowDialog();
                    break;
                default:
                    System.Windows.MessageBox.Show("Форма добавления записи для этой таблицы не найдена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            TableRefreshing(this, e);
        }
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            switch (TableNames[TableSelector.Text])
            {
                case "source":
                    Models.Source source = (Models.Source)grid.SelectedItem;
                    new EditWindows.Source(DB.DataBaseWorker, source.Id).ShowDialog();
                    break;
                case "clients":
                    Models.Client clients = (Models.Client)grid.SelectedItem;
                    new EditWindows.Client(DB.DataBaseWorker, clients.Id).ShowDialog();
                    break;
                case "investments":
                    Models.Investment investment = (Models.Investment)grid.SelectedItem;
                    new EditWindows.Investment(DB.DataBaseWorker, investment.Id).ShowDialog();
                    break;
                default:
                    System.Windows.MessageBox.Show("Форма редактирования записи для этой таблицы не найдена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            TableRefreshing(this, e);
        }
        /// <summary>
        /// Обработка события нажатия на кнопку экспорта
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="e">Аргументы события</param>
        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = $"{TableSelector.Text}.xlsx";
                saveFileDialog.Title = $"Сохранение таблицы \"{TableSelector.Text}\"";
                if(saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dataTable = DB.GetExportTable(TableNames[TableSelector.Text]);
                    dataTable.TableName = TableSelector.Text;
                    excelWorker.ExportTable(dataTable, saveFileDialog.FileName);
                }
            }
        }
    }

}
