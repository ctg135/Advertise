using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Advertise.Models;

using System.Diagnostics;

namespace Advertise
{
    /// <summary>
    /// Класс, представляющий собой упрощенный интерфейс взаимодействия с объектами базой данных
    /// </summary>
    public class DataBaseSynchronizer
    {
        /// <summary>
        /// Экземпляр для подключения к базе данных
        /// </summary>
        private DataBaseWorker DB;
        public DataBaseWorker DataBaseWorker { get => DB; }
        /// <summary>
        /// Конструктор для подключения к базе данных
        /// </summary>
        /// <param name="dataBaseWorker"></param>
        public DataBaseSynchronizer(DataBaseWorker dataBaseWorker)
        {
            DB = dataBaseWorker;
        }
        public IEnumerable<object> GetModelAsArr(string Model)
        {
            switch (Model.ToLower())
            {
                case "source":
                    return new List<Source>();
            }
            return null;
        }
        /// <summary>
        /// Возвращает содержимое таблицы из базы данных в виде IEnumerable<object>
        /// </summary>
        /// <param name="TableName">Имя таблицы для выборки</param>
        /// <returns>IEnumerable<object> с соответсвующей моделью и содержимым таблицы</returns>
        public IEnumerable<object> SelectTable(string TableName)
        {
            switch (TableName.ToLower())
            {
                case "source":
                    List<Source> sources = new List<Source>();
                    foreach(DataRow row in DB.SelectTable(TableName).Rows)
                    {
                        sources.Add(new Source() 
                        { 
                            Id = row.ItemArray[0].ToString(), 
                            Name = row.ItemArray[1].ToString() 
                        }
                        );
                    }
                    return sources;
            }

            return null;
        }
        
    }
}
