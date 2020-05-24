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
            dbworker = new DataBaseWorker();
            MainWindow = new Windows.MainWindow();
            var res = new Windows.AuthorizationWindow( /// Отображения окна авторизации
                dbworker,
                Advertise.Properties.Settings.Default.Server,
                Advertise.Properties.Settings.Default.Database,
                Advertise.Properties.Settings.Default.DateTimeValue                
            ).ShowDialog();
            if (res) /// Если авторизация пройдена
            {
                synchronizer = new DataBaseSynchronizer(dbworker);
                excelWorker = new ExcelWorker(synchronizer);
                var main = new Windows.MainWindow(synchronizer, excelWorker);
                MainWindow = main;
                MainWindow.Show();
            }
            else
            {
                Current.Shutdown();
            }
        }
    }
}
