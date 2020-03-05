namespace Advertise.Models
{
    /// <summary>
    /// Модель таблицы "Вложения"
    /// </summary>
    public class Investment : IModel
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public int Amount { get; set; }
        public string Month { get; set; }
    }
}
