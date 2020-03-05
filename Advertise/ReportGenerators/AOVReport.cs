using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advertise.ReportGenerators.Models;

namespace Advertise.ReportGenerators
{
    public class AOVReport : ReportGenerator
    {
        public AOVReport(DataBaseSynchronizer dataBaseSync) : base(dataBaseSync)
        {
        }

        public AOVReport(DataBaseSynchronizer dataBaseSync, string month) : base(dataBaseSync, month)
        {
        }

        public override List<string> Names
        {
            get
            {
                return names;
            }
        }

        protected override List<string> names { get; } = new List<string>() 
        {
            "Источник",
            "Общий доход",
            "Количество клиентов",
            "Показатель AOV",
        };

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
