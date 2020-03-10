using System.Collections.Generic;
using System.Data;

namespace Advertise.ReportGenerators
{
    /// <summary>
    /// Абстрактный класс для создания отчётов по таблицам
    /// </summary>
    public abstract class IReportGenerator
    {
        /// <summary>
        /// Словарь значений - { Месяц, Номер месяца }
        /// </summary>
        protected Dictionary<string, string> Months { get; } = new Dictionary<string, string>()
        {
            { "Январь" ,   "1" },
            { "Февраль" ,  "2" },
            { "Март" ,     "3" },
            { "Апрель" ,   "4" },
            { "Май" ,      "5" },
            { "Июнь" ,     "6" },
            { "Июль" ,     "7" },
            { "Август" ,   "8" },
            { "Сентябрь" , "9" },
            { "Октябрь" ,  "10" },
            { "Ноябрь" ,   "11" },
            { "Декабрь" ,  "12" }
        };
        /// <summary>
        /// Названия колонок отчета
        /// </summary>
        public abstract List<string> Names { get; }
        protected abstract List<string> names { get; }
        /// <summary>
        /// Месяц составления отчёта
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// Экземпляр для взаимодействия с базой данных
        /// </summary>
        protected DataBaseSynchronizer DB { get; private set; }
        /// <summary>
        /// Конструктор создания экземпляра
        /// </summary>
        /// <param name="dataBaseSync">Интерфейс взаимодействия с базой данных</param>
        /// <param name="month">Месяц построения отчёта</param>
        public IReportGenerator(DataBaseSynchronizer dataBaseSync, string month)
        {
            DB = dataBaseSync;
            Month = month;
        }
        /// <summary>
        /// Конструктор создания экземпляра
        /// </summary>
        /// <param name="dataBaseSync">Интерфейс взаимодействия с базой данных</param>
        public IReportGenerator(DataBaseSynchronizer dataBaseSync)
        {
            DB = dataBaseSync;
        }
        /// <summary>
        /// Создание отчета в виде таблицы
        /// </summary>
        /// <returns>Таблица с отчётом</returns>
        public abstract DataTable GenerateReoport();
        /// <summary>
        /// Генерация отчета в виде списка
        /// </summary>
        /// <returns>Список с отчётом</returns>
        public abstract List<object> GenerateReportAsList();
        
    }
}
