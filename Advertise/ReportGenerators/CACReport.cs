using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertise.ReportGenerators
{
    class CACReport : ReoportGenerator
    {
        public CACReport(DataBaseWorker dataBaseWorker) : base(dataBaseWorker)
        {

        }
        public override DataTable GenerateReoport()
        {
            throw new NotImplementedException();
        }

        public override List<object> GenerateReportAsList()
        {
            throw new NotImplementedException();
        }
    }
}
