using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advertise.ReportGenerators.Models;

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
            return new DataTable()
            {
                Columns = { "a"},
                Rows =  {"1", "2" }
            };
        }

        public override List<object> GenerateReportAsList()
        {
            List<object> result = new List<object>();

            foreach(DataRow row in GenerateReoport().Rows)
            {
                result.Add(new CACModel()
                    {
                        Source = "one",
                        CountClients = 1,
                        CAC = (float)1/2
                    }                    
                );
            }

            return result;
        }

        
    }
}
