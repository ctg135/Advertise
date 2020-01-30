using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertise.Models
{
    /// <summary>
    /// Модель данных для таблицы "Источники"
    /// </summary>
    public class Source : IModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
