using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertise.Models
{
    /// <summary>
    /// Представляет универсальную модель таблицы
    /// </summary>
    public interface IModel
    {
        int Id { get; set; }
    }
}
