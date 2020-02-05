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
        private DataBaseWorker DB;
        /// <summary>
        /// Словарь источников типа { "Название" - "Идентификатор" }
        /// </summary>
        private Dictionary<string, string> Sources;
        private string TableName
        {
            get
            {
                return "investments";
            }
        }
        public Investment(DataBaseWorker DataBaseWorker)
        {
            InitializeComponent();
            this.DB = DataBaseWorker;
            Sources = new Dictionary<string, string>();
            // 
            DataTable Table = DB.SelectTable("source");
            foreach (DataRow row in Table.Rows)
            {
                ComboBoxSource.Items.Add(row["Title"].ToString());
                Sources.Add(row["Title"].ToString(), row["Id"].ToString());
            }
            TextBoxMonth.Text = DateTime.Now.ToString("MM");
            TextBoxYear.Text = DateTime.Now.ToString("yyyy");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.Insert(TableName, new Dictionary<string, string> {
                    { "Id", TextBoxId.Text.ToLower() == "по умолчанию" ? "" : TextBoxId.Text  },
                    { "Source", Sources[ComboBoxSource.Text] },
                    { "Amount", TextBoxAmount.Text },
                    { "Month", TextBoxMonth.Text },
                    { "Year", TextBoxYear.Text }
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
