using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Advertise.ReportGenerators
{
    abstract class ReoportGenerator
    {
        protected DataBaseWorker DB { get; private set; }
        public ReoportGenerator(DataBaseWorker dataBaseWorker)
        {
            DB = dataBaseWorker;
        }
        public abstract DataTable GenerateReoport();
        public abstract List<object> GenerateReportAsList();
    }
}
