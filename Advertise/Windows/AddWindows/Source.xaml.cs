using System;
using System.Collections.Generic;
using System.Windows;

namespace Advertise.Windows.AddWindows
{
    /// <summary>
    /// Логика взаимодействия для Source.xaml
    /// </summary>
    public partial class Source : Window
    {
        /// <summary>
        /// Возвращает имя таблицы в базе данных
        /// </summary>
        private string TableName
        {
            get
            {
                return "source";
            }
        }
        /// <summary>
        /// Экземпляр для работы с базой данных
        /// /// </summary>
        private DataBaseWorker DB;
        public Source(DataBaseWorker DataBaseWorker)
        {
            DB = DataBaseWorker;
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик нажатия на кнопку добавления записи
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.Insert(TableName, new Dictionary<string, string>() 
                {
                    { "Id", TextBoxId.Text.ToLower() == "по умолчанию" ? "" : TextBoxId.Text },
                    { "Title", TextBoxTitle.Text }
                }
                );
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления записи", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
