using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Advertise
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        DataBaseWorker dbworker;
        DataBaseSynchronizer synchronizer;
        ExcelWorker excelWorker;
        public App()
        {
            dbworker = new DataBaseWorker(Advertise.Properties.Settings.Default.connectionString);
            if (CheckConnectionWithDB())
            {
                synchronizer = new DataBaseSynchronizer(dbworker);
                excelWorker = new ExcelWorker(synchronizer);
                new Windows.MainWindow(synchronizer, excelWorker).Show();
            }
            else
            {
                Current.Shutdown();
            }

        }
        /// <summary>
        /// Функция для проверки подключения к базе данных<br></br>
        /// При ошибке подключеният требует выводит MessageBox до тех пор, пока не подключится или закрывает приложение
        /// </summary>
        private bool CheckConnectionWithDB()
        {
            bool retry;
            bool success = false;
            do
            {
                retry = false;
                try
                {
                    success = dbworker.CheckConnection();
                }
                catch (Exception ex)
                {
                    switch (MessageBox.Show(ex.Message + "\nНажмите \"Да\" для потворного подключения", "Ошибка подключения", MessageBoxButton.YesNo, MessageBoxImage.Error))
                    {
                        case MessageBoxResult.Yes:
                            retry = true;
                            break;
                        case MessageBoxResult.No:
                            retry = false;
                            break;
                    }
                }
            } while (retry);
            return success;
        }
    }
}
