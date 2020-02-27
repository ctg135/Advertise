﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advertise.ReportGenerators.Models;
using System.Diagnostics;

namespace Advertise.ReportGenerators
{
    class CACReport : ReoportGenerator
    {
        protected override List<string> names { get; } = new List<string>() { "Источник", "Количество клиентов", "Показатель CAC" };
        public override List<string> Names
        {
            get
            {
                return names;
            }
        }
        public CACReport(DataBaseSynchronizer dataBaseSync, string month) : base(dataBaseSync, month)
        {
        }
        public CACReport(DataBaseSynchronizer dataBaseSync) : base(dataBaseSync)
        {
        }
        public override DataTable GenerateReoport()
        {
            DataTable result = new DataTable();
            foreach (var name in names)
            {
                result.Columns.Add(name);
            }
            using (DataTable sources = DB.DataBaseWorker.SelectTable("source"))
            {
                foreach(DataRow row in sources.Rows)
                { 
                    var count1 = DB.DataBaseWorker.MakeQuery($"SELECT COUNT(`Id`) as `count` FROM `clients` WHERE `Source` = '{row["Id"]}' AND MONTH(`Date`) = '{Months[Month]}'");
                    var count = count1.Rows.Count == 1 ? int.Parse(count1.Rows[0][0].ToString()) : 0;
                    var investment1 = DB.DataBaseWorker.MakeQuery($"SELECT `Amount` FROM `investments` WHERE `Source` = '{row["Id"]}' AND `Month` = '{Month}'");
                    var investment = investment1.Rows.Count == 1 ? int.Parse(investment1.Rows[0][0].ToString()) : 0;
                    var cac = count != 0 && investment != 0 ? (float)investment / (float)count : 0;
                    result.Rows.Add(row["Title"], count, cac);
                }
            }
            return result;
        }

        public override List<object> GenerateReportAsList()
        {
            List<object> result = new List<object>();

            foreach(DataRow row in GenerateReoport().Rows)
            {
                result.Add(new CACModel()
                    {
                        Source = row[0].ToString(),
                        CountClients = row[1].ToString(),
                        CAC = row[2].ToString()
                    }                    
                );
            }

            return result;
        }

        
    }
}
