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
