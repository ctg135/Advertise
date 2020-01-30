using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertise.Models
{
    /// <summary>
    /// Модель для таблицы "Просмотры"
    /// </summary>
    public class View : IModel
    {
        public int Id { get; set; }
        public int Source { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
