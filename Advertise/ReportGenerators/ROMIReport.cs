using System.Collections.Generic;
using System.Data;
using Advertise.ReportGenerators.Models;

namespace Advertise.ReportGenerators
{
    /// <summary>
    /// Класс создания ROMI отчёта
    /// </summary>
    public class ROMIReport : ReportGenerator
    {
        /// <summary>
        /// Список названий полей отчёта
        /// </summary>
        public override List<string> Names
        {
            get
            {
                return names;
            }
        }
        /// <summary>
        /// Поля отчёта
        /// </summary>
        protected override List<string> names { get; } = new List<string>()
        {
            "Источник",
            "Прибыль от источника",
            "Расход на источник",
            "Показатель ROMI"
        };
        /// <summary>
        /// Функция создания отчёта
        /// </summary>
        /// <returns>Таблица с отчетом</returns>
        public override DataTable GenerateReoport()
        {
            DataTable result = new DataTable();
            foreach (var name in names)
            {
                result.Columns.Add(name);
            }

            using (DataTable sources = DB.DataBaseWorker.SelectTable("source"))
            {
                foreach (DataRow row in sources.Rows)
                {
                    var temp   = DB.DataBaseWorker.MakeQuery($"SELECT SUM(`Profit`) FROM `clients` WHERE MONTH(`Date`) = '{Months[Month]}' AND `Source` = '{row["Id"]}' ");
                    var profit = (temp.Rows.Count == 1 && temp.Rows[0][0].ToString() != string.Empty) ? int.Parse(temp.Rows[0][0].ToString()) : 0;
                    temp       = DB.DataBaseWorker.MakeQuery($"SELECT `amount` FROM `investments` WHERE `Month` = '{Month}' and `Source` = '{row["Id"]}'");
                    var cost   = (temp.Rows.Count == 1 && temp.Rows[0][0].ToString() != string.Empty) ? int.Parse(temp.Rows[0][0].ToString()) : 0;
                    var romi   = $"{(float)(profit - cost)/100}%";
                    result.Rows.Add(row["Title"], profit, cost, romi);
                }
            }

            return result;
        }
        /// <summary>
        /// Функция создания отчёта в виде списка
        /// </summary>
        /// <returns>Список с отчетом</returns>
        public override List<object> GenerateReportAsList()
        {
            List<object> result = new List<object>();

            foreach (DataRow row in GenerateReoport().Rows)
            {
                result.Add(new ROMIModel()
                {
                    Source = row[names[0]].ToString(),
                    Profit = row[names[1]].ToString(),
                    Cost   = row[names[2]].ToString(),
                    ROMI   = row[names[3]].ToString()
                }
                );
            }

            return result;
        }

        public ROMIReport(DataBaseSynchronizer dataBaseSync, string month) : base(dataBaseSync, month)
        {
        }
        public ROMIReport(DataBaseSynchronizer dataBaseSync) : base(dataBaseSync)
        {
        }
    }
}
