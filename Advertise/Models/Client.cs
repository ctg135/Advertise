using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertise.Models
{
    /// <summary>
    /// Модель таблицы "Клиенты"
    /// </summary>
    public class Client : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Source { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Profit { get; set; }
    }
}
