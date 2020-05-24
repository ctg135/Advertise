using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Advertise.Windows.EditWindows
{
    /// <summary>
    /// Логика взаимодействия для Investment.xaml
    /// </summary>
    public partial class Investment : Window
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
                return "investments";
            }
        }
        /// <summary>
        /// Словарь источников типа {"Название", "Номер"}
        /// </summary>
        Dictionary<string, string> Sources;
        /// <summary>
        /// Конструктор окна для изменения записи в таблице "Клиенты"
        /// </summary>
        /// <param name="DataBaseWorker">Экземпляр для работы с базой данных</param>
        /// <param name="Id">Идентификатор записи в таблице "Клиенты"</param>
        public Investment(DataBaseWorker DataBaseWorker, int Id)
        {
            InitializeComponent();
            DB = DataBaseWorker;
            this.Id = Id;
            Sources = new Dictionary<string, string>(); 
            DataTable item = DB.MakeQuery($"SELECT * FROM `{TableName}` WHERE `Id` = '{Id}'"); /// Вывод данных в элементы управления
            foreach (DataRow row in DB.SelectTable("source").Rows)
            {
                Sources.Add(row["Title"].ToString(), row["Id"].ToString());
                ComboBoxItem item1 = item.Rows[0]["Source"].ToString() == row["Id"].ToString() ? new ComboBoxItem() { IsSelected = true, Content = row["Title"].ToString() } : new ComboBoxItem() { Content = row["Title"].ToString() };
                ComboBoxSource.Items.Add(item1);
            }
            TextBoxId.Text = item.Rows[0]["Id"].ToString();
            foreach(ComboBoxItem comboBoxItem in ComboBoxMonth.Items)
                if(comboBoxItem.Content.ToString() == item.Rows[0]["Month"].ToString())                
                    comboBoxItem.IsSelected = true;
            TextBoxAmount.Text = item.Rows[0]["Amount"].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.UpdateTable(TableName, Id.ToString(), new Dictionary<string, string>()
                {
                    { "Id", TextBoxId.Text },
                    { "Source", Sources[ComboBoxSource.Text] },
                    { "Amount", TextBoxAmount.Text },
                    { "Month", ComboBoxMonth.Text }
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
