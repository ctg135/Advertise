﻿using System;
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
                    SetUpTableNames(TableNames[TableName]);
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
            var names = DB.ColumnNames(TableName);
            foreach (var col in grid.Columns)
            {
                col.Header = names[col.Header.ToString()];
            }
        }
        /// <summary>
        /// Обработчик события выбора таблицы
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Аргументы события</param>
        private void TableSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetUpTable(e.AddedItems[0].ToString());
            CurrentFields = new Dictionary<string, string>();
            CurrentFields = DB.ColumnNamesReversed(TableNames[e.AddedItems[0].ToString()]);
            ButtonAdd.IsEnabled = e.AddedItems.Count > 0;
        }
        /// <summary>
        /// Функция-обработчик события выбранной таблицы
        /// </summary>
        /// <param name="sender">Объект-отправитель</param>
        /// <param name="e">Параметры события</param>
        private void grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ButtonDelete.IsEnabled = grid.SelectedItems.Count > 0;
            ButtonEdit.IsEnabled = grid.SelectedItems.Count == 1;
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
                    MessageBox.Show("Форма добавления записи для этой таблицы не найдена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            // Обновление таблицы
            SetUpTable(TableSelector.Text);
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
                    MessageBox.Show("Форма редактирования записи для этой таблицы не найдена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            // Обновление таблицы
            SetUpTable(TableSelector.Text);
        }
    }

}
