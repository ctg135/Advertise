using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Advertise.Models;
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
                tableNames.Add(row[0].ToString(), row[0].ToString());
            }
            tableNames["clients"] = "Клиенты";
            tableNames["source"] = "Источники";
            tableNames["investments"] = "Вложения";
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
                            Id = int.Parse(row["Id"].ToString()),
                            Title = row["Title"].ToString()                            
                        }
                        );
                    }
                    return sources;
                case "investments":
                    Dictionary<int, string> source = new Dictionary<int, string>();
                    List<Source> sourceDB = (List<Source>)SelectTable("source");
                    foreach(Source sou in sourceDB)
                    {
                        source.Add(sou.Id, sou.Title);
                    }
                    List<Investment> invesments = new List<Investment>();
                    foreach(DataRow row in DB.SelectTable(TableName).Rows)
                    {
                        invesments.Add(new Investment() 
                        { 
                            Id = int.Parse(row["Id"].ToString()),
                            Source = source[int.Parse(row["Source"].ToString())],
                            Amount = int.Parse(row["Amount"].ToString()),
                            Month = row["Month"].ToString()
                        }
                        );
                    }
                    return invesments;
                case "clients":
                    Dictionary<int, string> source1 = new Dictionary<int, string>();
                    List<Source> sourceDB1 = (List<Source>)SelectTable("source");
                    foreach (Source sou in sourceDB1)
                    {
                        source1.Add(sou.Id, sou.Title);
                    }
                    List<Client> clients = new List<Client>();
                    foreach(DataRow row in DB.SelectTable(TableName).Rows)
                    {
                        DateTime date = (DateTime)row["Date"];
                        clients.Add(new Client
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            Source = source1[int.Parse(row["Source"].ToString())],
                            Date = date.ToString("d"),
                            Time = row["Time"].ToString(),
                            Profit = int.Parse(row["Profit"].ToString()),
                        }
                        );
                    }
                    return clients;
            }
            return null;
        }
        /// <summary>
        /// Функция для получения русских названий столбцов таблицы
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <returns>Словарь типа { Поле - Название }</returns>
        public Dictionary<string, string> ColumnNames(string tableName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            switch(tableName)
            {
                case "source":
                    result.Add("Id", "Идентификатор");
                    result.Add("Title", "Название");
                    break;
                case "investments":
                    result.Add("Id", "Идентификатор");
                    result.Add("Source", "Источник");
                    result.Add("Amount", "Размер вложения");
                    result.Add("Month", "Месяц вложения");
                    break;
                case "clients":
                    result.Add("Id", "Идентификатор");
                    result.Add("Name", "Имя клиента");
                    result.Add("Source", "Источник");
                    result.Add("Date", "Дата покупки");
                    result.Add("Time", "Время покупки");
                    result.Add("Profit", "Прибыль");
                    break;
            }
            return result;
        }
        /// <summary>
        /// Функция для получения названий столбцов таблицы
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <returns>Словарь типа { Название - Поле }</returns>
        public Dictionary<string, string> ColumnNamesReversed(string tableName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(var pair in ColumnNames(tableName))
            {
                result.Add(pair.Value, pair.Key);
            }
            return result;
        }
        /// <summary>
        /// Функция для изменения таблицы в базе данных
        /// </summary>
        /// <param name="TableName">Имя таблицы</param>
        /// <param name="Id">Идентификатор записи</param>
        /// <param name="Column">Столбец с измененным значением</param>
        /// <param name="NewValue">Новое значение ячейки</param>
        public void UpdateTable(string TableName,  string Id, string Column, string NewValue) => DB.UpdateTable(TableName, Id, new Dictionary<string, string>() { { Column, NewValue } });
        /// <summary>
        /// Удаляет поля из базы данных
        /// </summary>
        /// <param name="TableName">Имя таблицы</param>
        /// <param name="Ids">Идентификаторы записей</param>
        public void DeleteSome(string TableName, IEnumerable<string> Ids) => DB.Delete(TableName, Ids);
        /// <summary>
        /// Вставляет записи в таблицу базы данных
        /// </summary>
        /// <param name="TableName">Название таблицы</param>
        /// <param name="newValues">Словарь полей типа "Поле" - "Значение"</param>
        public void Insert(string TableName, Dictionary<string, string> NewValues) => DB.Insert(TableName, NewValues);
    }
}
