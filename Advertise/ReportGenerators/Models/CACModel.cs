namespace Advertise.ReportGenerators.Models
{
    /// <summary>
    /// Модель данных CAC
    /// </summary>
    public class CACModel
    {
        /// <summary>
        /// Рекламный источник 
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Количество клиентов
        /// </summary>
        public string CountClients { get; set; }
        /// <summary>
        /// Аоказатель CAC
        /// </summary>
        public string CAC { get; set; }
    }
}
