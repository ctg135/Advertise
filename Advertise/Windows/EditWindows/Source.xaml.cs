using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Advertise.Windows.EditWindows
{
    /// <summary>
    /// Логика взаимодействия для Source.xaml
    /// </summary>
    public partial class Source : Window
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
                return "source";
            }
        }
        /// <summary>
        /// Конструктор окна для изменения записи в таблице "Источники"
        /// </summary>
        /// <param name="DataBaseWorker">Экземпляр для работы с базой данных</param>
        /// <param name="Id">Идентификатор записи в таблице "Источники"</param>
        public Source(DataBaseWorker DataBaseWorker, int Id)
        {
            InitializeComponent();
            DB = DataBaseWorker;
            this.Id = Id;
            // Установка полей
            DataTable item = DB.MakeQuery($"SELECT * FROM `{TableName}` WHERE `Id` = '{Id}'");
            TextBoxId.Text = item.Rows[0]["Id"].ToString();
            TextBoxTitle.Text = item.Rows[0]["Title"].ToString();
        }
        /// <summary>
        /// Обработчик нажатия на кнопку изменения записи
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.UpdateTable(TableName, Id.ToString(), new Dictionary<string, string>()
                {
                    { "Id", TextBoxId.Text },
                    {"Title", TextBoxTitle.Text }
                }
                );
            }
            catch(Exception ex)
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
