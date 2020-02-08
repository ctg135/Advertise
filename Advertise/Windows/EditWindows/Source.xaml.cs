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

namespace Advertise.Windows.EditWindows
{
    /// <summary>
    /// Логика взаимодействия для Source.xaml
    /// </summary>
    public partial class Source : Window
    {
        DataBaseWorker DB;
        Models.Source Item;
        int Id;
        string TableName
        {
            get
            {
                return "source";
            }
        }
        public Source(DataBaseWorker DataBaseWorker,  int Id)
        {
            InitializeComponent();
            DB = DataBaseWorker;
            this.Id = Id;
            DataTable item = DB.MakeQuery($"SELECT * FROM `{TableName}` WHERE `Id` = '{Id}'");
            Item = new Models.Source()
            {
                Id = int.Parse(item.Rows[0]["Id"].ToString()),
                Title = item.Rows[0]["Title"].ToString()
            };
            TextBoxId.Text = Item.Id.ToString();
            TextBoxTitle.Text = Item.Title;
        }
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
