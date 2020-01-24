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
        /// <summary>
        /// Получает словарь типа { "Имя таблицы в БД", "Имя таблицы на русском" }
        /// </summary>
        /// <returns>Словарь типа { "Имя таблицы в БД", "Имя таблицы на русском" }</returns>
        /// /// <exception cref="System.Exception">Ошибка запроса, при её наличии</exception>
        public Dictionary<string, string> GetTableNames()
        {
            Dictionary<string, string> tableNames = new Dictionary<string, string>();
            foreach (DataRow row in DB.MakeQuery("SHOW TABLES").Rows)
            {
                tableNames.Add(row[0].ToString(), string.Empty);
            }
            tableNames["clients"] = "Клиенты";
            tableNames["source"] = "Источники";
            tableNames["investments"] = "Вложения";
            tableNames["viewsadd"] = "Просмотры";
            return tableNames;
        }
        /// <summary>
        /// Получает словарь типа { "Имя таблицы на русском", "Имя таблицы в БД" }
        /// </summary>
        /// <returns>Словарь с парами типа { "Имя таблицы на русском", "Имя таблицы в БД" }</returns>
        /// <exception cref="System.Exception">Ошибка запроса, при её наличии</exception>
        public Dictionary<string, string> GetTableNamesReversed()
        {
            Dictionary<string, string> reversed = new Dictionary<string, string>();
            foreach(KeyValuePair<string, string> pair in GetTableNames())
            {
                reversed.Add(pair.Value, pair.Key);
            }
            return reversed;
        }
        /// <summary>
        /// Возвращает содержимое таблицы из базы данных в виде IEnumerable соответсвующей модели
        /// </summary>
        /// <param name="TableName">Имя таблицы для выборки</param>
        /// <returns>IEnumerable с соответсвующей моделью и содержимым таблицы</returns>
        /// <exception cref="System.Exception">Ошибка запроса, при её наличии</exception>
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
                            Id = row["Id"].ToString(),
                            Title = row["Title"].ToString()                            
                        }
                        );
                    }
                    return sources;
                case "investments":
                    List<Investment> invesments = new List<Investment>();
                    foreach(DataRow row in DB.SelectTable(TableName).Rows)
                    {
                        invesments.Add(new Investment() 
                        { 
                            Id = row["Id"].ToString(),
                            Source = row["Source"].ToString(),
                            Amount = row["Amount"].ToString(),
                            Month = row["Month"].ToString(),
                            Year = row["Year"].ToString()
                        }
                        );
                    }
                    return invesments;
                case "clients":
                    List<Client> clients = new List<Client>();
                    foreach(DataRow row in DB.SelectTable(TableName).Rows)
                    {
                        DateTime date = (DateTime)row["Date"];
                        clients.Add(new Client
                        {
                            Id = row["Id"].ToString(),
                            Name = row["Name"].ToString(),
                            Source = row["Source"].ToString(),
                            Date = date.ToString("d"),
                            Time = row["Time"].ToString(),
                            Profit = row["Profit"].ToString(),
                        }
                        );
                    }
                    return clients;
                case "viewsadd":
                    List<View> views = new List<View>();
                    foreach (DataRow row in DB.SelectTable(TableName).Rows)
                    {
                        DateTime date = (DateTime)row["Date"];
                        views.Add(new View
                        {
                            Id = row["Id"].ToString(),
                            Source = row["Source"].ToString(),
                            Date = date.ToString("d"),
                            Time = row["Time"].ToString(),
                        }
                        );
                    }
                    return views;

            }
            return null;
        }
        
    }
}
