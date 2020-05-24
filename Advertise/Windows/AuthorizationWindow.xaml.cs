using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Advertise.Windows
{
    /// <summary>
    /// Окно авторизации
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private DataBaseWorker db;
        private bool Result;
        private string serverName;
        private string dataBaseName;
        private string dateTimeValue;
        /// <summary>
        /// Конструктор окна для авторизации
        /// </summary>
        /// <param name="DataBaseWorker"></param>
        /// <param name="Server"></param>
        /// <param name="DataBase"></param>
        /// <param name="DateTimeValue"></param>
        public AuthorizationWindow(DataBaseWorker DataBaseWorker, string Server, string DataBase, string DateTimeValue)
        {
            InitializeComponent();
            db = DataBaseWorker;
            serverName = Server;
            dataBaseName = DataBase;
            dateTimeValue = DateTimeValue;
            Result = false;
        }
        /// <summary>
        /// Переопрделение базового <see cref="ShowDialog"/>
        /// <para/>
        /// Возвращает true при успешной авторизации
        /// </summary>
        /// <returns>Возвращает true при успешной авторизации</returns>
        public new bool ShowDialog()
        {
            base.ShowDialog();
            return Result;
        }
        /// <summary>
        /// Функция для вывода сообщения-ошибки
        /// </summary>
        /// <param name="Message">Сообщение для вывода</param>
        /// <param name="Caption">Заголовок</param>
        private void ErrorMessage(string Message, string Caption) => System.Windows.MessageBox.Show(Message, Caption, MessageBoxButton.OK, MessageBoxImage.Error);
        /// <summary>
        /// Нажатие на кнопку авторизации
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Аргументы события</param>
        private void AuthoClick(object sender, RoutedEventArgs e)
        {
            /// Создание строки подключения
            string ConnString = $"Server={serverName};Database={dataBaseName};User Id={LoginBox.Text};Pwd={PasswordBox.Password};convert zero datetime={dateTimeValue}";
            /// Смена курсора
            Cursor = System.Windows.Input.Cursors.Wait;
            Result = true;
            try
            {
                /// Проверка работы строки подключения
                db.ConnectionString = ConnString;
                db.CheckConnection();
            }
            catch(Exception exc)
            {
                Result = false;
                System.Diagnostics.Debug.WriteLine(exc.Message);
                if (exc.Message == "Unable to connect to any of the specified MySQL hosts")
                {
                    ErrorMessage("Нету подключения к базе данных", "Ошибка подключения");
                }
                else if (exc.Message.StartsWith("Authentication to host"))
                {
                    ErrorMessage("Неверное имя пользователя или пароль", "Ошибка Авторизации");
                }
                else
                {
                    ErrorMessage(exc.Message, "Ошибка");
                }
            }
            finally
            {
                /// Возвращение курсора
                Cursor = System.Windows.Input.Cursors.Arrow;
            }
            /// Если авторизация прошла успешно, закрываем окно
            if (Result) Close();
        }
    }
}
