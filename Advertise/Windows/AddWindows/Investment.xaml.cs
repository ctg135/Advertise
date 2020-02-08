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
using System.Data;

namespace Advertise.Windows.AddWindows
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
        /// Словарь источников типа { "Название" - "Идентификатор" }
        /// </summary>
        private Dictionary<string, string> Sources;
        /// <summary>
        /// Возвращает имя таблицы в базе данных
        /// </summary>
        private string TableName
        {
            get
            {
                return "investments";
            }
        }
        /// <summary>
        /// Конструктор окна для добавления записей в таблицу "Вложения"
        /// </summary>
        /// <param name="DataBaseWorker">Экземпляр для работы с базой данных</param>
        public Investment(DataBaseWorker DataBaseWorker)
        {
            InitializeComponent();
            this.DB = DataBaseWorker;
            Sources = new Dictionary<string, string>();
            // Установка значений селектора
            DataTable Table = DB.SelectTable("source");
            foreach (DataRow row in Table.Rows)
            {
                ComboBoxSource.Items.Add(row["Title"].ToString());
                Sources.Add(row["Title"].ToString(), row["Id"].ToString());
            }
            // Установка текущей даты
            ComboBoxMonth.SelectedIndex = int.Parse(DateTime.Now.ToString("MM")) - 1;
        }
        /// <summary>
        /// Обработчик нажатия на кнопку добавления записи
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.Insert(TableName, new Dictionary<string, string> {
                    { "Id", TextBoxId.Text.ToLower() == "по умолчанию" ? "" : TextBoxId.Text  },
                    { "Source", Sources[ComboBoxSource.Text] },
                    { "Amount", TextBoxAmount.Text },
                    { "Month", ComboBoxMonth.Text }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления записи", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
        }
    }
}
