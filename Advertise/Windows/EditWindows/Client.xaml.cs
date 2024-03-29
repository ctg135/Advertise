﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Advertise.Windows.EditWindows
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        /// <summary>
        /// Экземпляр для работы с базой данных
        /// </summary>
        private DataBaseWorker DB;
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        private int Id;
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public string TableName
        {
            get
            {
                return "clients";
            }
        }
        /// <summary>
        /// Список источников типа "Название" - "Идентификатор"
        /// </summary>
        Dictionary<string, string> Sources;
        /// <summary>
        /// Конструктор окна для изменения записи в таблице "Клиенты"
        /// </summary>
        /// <param name="DataBaseWorker">Экземпляр для работы с базой данных</param>
        /// <param name="Id">Идентификатор записи в таблице "Клиенты"</param>
        public Client(DataBaseWorker DataBaseWorker, int Id)
        {
            InitializeComponent();
            DB = DataBaseWorker;
            this.Id = Id;
            Sources = new Dictionary<string, string>();
            DataTable item = DB.MakeQuery($"SELECT * FROM `{TableName}` WHERE `Id` = '{Id}'");
            foreach (DataRow row in DB.SelectTable("source").Rows)
            {
                Sources.Add(row["Title"].ToString(), row["Id"].ToString());
                ComboBoxItem item1 = item.Rows[0]["Source"].ToString() == row["Id"].ToString() ? new ComboBoxItem() { IsSelected = true, Content = row["Title"].ToString() } : new ComboBoxItem() { Content = row["Title"].ToString() };
                ComboBoxSource.Items.Add(item1);
            }
            // Установка полей
            TextBoxId.Text = item.Rows[0]["Id"].ToString();
            TextBoxName.Text = item.Rows[0]["Name"].ToString();
            DatePickerDate.SelectedDate = DateTime.Parse(item.Rows[0]["Date"].ToString());
            TextBoxTime.Text = item.Rows[0]["Time"].ToString();
            TextBoxProfit.Text = item.Rows[0]["Profit"].ToString();
        }
        /// <summary>
        /// Функция обработчик события нажатия кнопки измененния
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.UpdateTable(TableName, Id.ToString(), new Dictionary<string, string>()
                {
                    { "Id", TextBoxId.Text },
                    { "Name", TextBoxName.Text },
                    { "Source", Sources[ComboBoxSource.Text] },
                    { "Date", ((DateTime)DatePickerDate.SelectedDate).ToString("yyyy-MM-dd") },
                    { "Time", TextBoxTime.Text },
                    { "Profit", TextBoxProfit.Text }
                }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка редактирования записи", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
        }
    }
}
