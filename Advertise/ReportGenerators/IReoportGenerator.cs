using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Advertise.ReportGenerators
{
    public abstract class ReoportGenerator
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
        public abstract List<string> Names { get; }
        protected abstract List<string> names { get; }
        public string Month { get; set; }
        protected DataBaseSynchronizer DB { get; private set; }
        public ReoportGenerator(DataBaseSynchronizer dataBaseSync, string month)
        {
            DB = dataBaseSync;
            Month = month;
        }
        public ReoportGenerator(DataBaseSynchronizer dataBaseSync)
        {
            DB = dataBaseSync;
        }
        public abstract DataTable GenerateReoport();
        public abstract List<object> GenerateReportAsList();
        
    }
}
