using System.Collections.Generic;
using System.Data;
using Advertise.ReportGenerators.Models;

namespace Advertise.ReportGenerators
{
    /// <summary>
    /// Класс создания CAC отчета
    /// </summary>
    class CACReport : ReportGenerator
    {
        /// <summary>
        /// Поля отчёта
        /// </summary>
        protected override List<string> names { get; } = new List<string>() 
        { 
            "Источник",
            "Количество клиентов",
            "Показатель CAC" 
        };
        /// <summary>
        /// Названия полей отчёта
        /// </summary>
        public override List<string> Names
        {
            get
            {
                return names;
            }
        }
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

            result.TableName = $"Отчёт CAC за {Month}";

            using (DataTable sources = DB.DataBaseWorker.SelectTable("source"))
            {
                foreach(DataRow row in sources.Rows)
                { 
                    var count1 = DB.DataBaseWorker.MakeQuery($"SELECT COUNT(`Id`) as `count` FROM `clients` WHERE `Source` = '{row["Id"]}' AND MONTH(`Date`) = '{Months[Month]}'");
                    var count = count1.Rows.Count == 1 ? int.Parse(count1.Rows[0][0].ToString()) : 0;
                    var investment1 = DB.DataBaseWorker.MakeQuery($"SELECT `Amount` FROM `investments` WHERE `Source` = '{row["Id"]}' AND `Month` = '{Month}'");
                    var investment = investment1.Rows.Count == 1 ? int.Parse(investment1.Rows[0][0].ToString()) : 0;
                    int cac = count != 0 && investment != 0 ? investment / count : 0;
                    result.Rows.Add(row["Title"], count, cac);
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

            foreach(DataRow row in GenerateReoport().Rows)
            {
                result.Add(new CACModel()
                    {
                        Source       = row[names[0]].ToString(),
                        CountClients = row[names[1]].ToString(),
                        CAC          = row[names[2]].ToString()
                    }                    
                );
            }

            return result;
        }
        public CACReport(DataBaseSynchronizer dataBaseSync, string month) : base(dataBaseSync, month)
        {
        }
        public CACReport(DataBaseSynchronizer dataBaseSync) : base(dataBaseSync)
        {
        }

    }
}
