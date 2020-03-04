using System.Collections.Generic;
using System.Data;

namespace Advertise.ReportGenerators
{
    public abstract class ReportGenerator
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
        protected DataBaseSynchronizer DB { get; private set; }
        public ReportGenerator(DataBaseSynchronizer dataBaseSync, string month)
        {
            DB = dataBaseSync;
            Month = month;
        }
        public ReportGenerator(DataBaseSynchronizer dataBaseSync)
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
        /// <returns></returns>
        public abstract List<object> GenerateReportAsList();
        
    }
}
