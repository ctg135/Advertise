using System.Collections.Generic;
using System.Data;
using Advertise.ReportGenerators.Models;

namespace Advertise.ReportGenerators
{
    /// <summary>
    /// Класс создания AOV отчёта
    /// </summary>
    public class AOVReport : ReportGenerator
    {
        public AOVReport(DataBaseSynchronizer dataBaseSync) : base(dataBaseSync)
        {
        }

        public AOVReport(DataBaseSynchronizer dataBaseSync, string month) : base(dataBaseSync, month)
        {
        }
        /// <summary>
        /// Названия колонок отчета
        /// </summary>
        public override List<string> Names
        {
            get
            {
                return names;
            }
        }
        /// <summary>
        /// Название полей отчета
        /// </summary>
        protected override List<string> names { get; } = new List<string>() 
        {
            "Источник",
            "Общий доход",
            "Количество клиентов",
            "Показатель AOV",
        };
        /// <summary>
        /// Создание отчета в виде таблицы
        /// </summary>
        /// <returns>Таблица с отчётом</returns>
        public override DataTable GenerateReoport()
        {
            DataTable result = new DataTable();
            foreach (var name in names)
            {
                result.Columns.Add(name);
            }

            result.TableName = $"Отчёт AOV за {Month}";

            using (DataTable sources = DB.DataBaseWorker.SelectTable("source"))
            {
                foreach (DataRow row in sources.Rows)
                {
                    var temp = DB.DataBaseWorker.MakeQuery($"SELECT SUM(`Profit`) FROM `clients`WHERE `Source` = '{row["Id"]}' AND MONTH(`Date`) = '{Months[Month]}'");
                    var profit = (temp.Rows.Count == 1 && temp.Rows[0][0].ToString() != string.Empty) ? int.Parse(temp.Rows[0][0].ToString()) : 0;
                    temp = DB.DataBaseWorker.MakeQuery($"SELECT Count(`Id`) FROM `clients` WHERE `Source` = '{row["Id"]}' AND Month(`Date`) = '{Months[Month]}'");
                    var countOfClient = (temp.Rows.Count == 1 && temp.Rows[0][0].ToString() != string.Empty) ? int.Parse(temp.Rows[0][0].ToString()) : 0;
                    var aov =  countOfClient != 0 ? (float)profit / (float)countOfClient : 0;
                    result.Rows.Add(row["Title"], profit, countOfClient, aov);
                }
            }

            return result;
        }
        /// <summary>
        /// Генерация отчета в виде списка
        /// </summary>
        /// <returns>Список с отчётом</returns>
        public override List<object> GenerateReportAsList()
        {
            List<object> result = new List<object>();

            foreach (DataRow row in GenerateReoport().Rows)
            {
                result.Add(new AOVModel()
                {
                    Source        = row[names[0]].ToString(),
                    TotalProfit   = row[names[1]].ToString(),
                    CountOfClient = row[names[2]].ToString(),
                    AOV           = row[names[3]].ToString()                    
                }
                );
            }

            return result;
        }
    }
}
