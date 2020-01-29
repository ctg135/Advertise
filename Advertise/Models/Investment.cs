using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertise.Models
{
    public class Investment : IModel
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string Amount { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }
}
