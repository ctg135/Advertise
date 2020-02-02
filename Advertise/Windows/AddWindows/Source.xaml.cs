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

namespace Advertise.Windows.AddWindows
{
    /// <summary>
    /// Логика взаимодействия для Source.xaml
    /// </summary>
    public partial class Source : Window
    {
        private string TableName
        {
            get
            {
                return "source";
            }
        }
        private DataBaseWorker DB;
        public Source(DataBaseWorker DataBaseWorker)
        {
            DB = DataBaseWorker;
            InitializeComponent();
        }

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
