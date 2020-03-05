namespace Advertise.ReportGenerators.Models
{
    /// <summary>
    /// Модель данных AOV отчёта
    /// </summary>
    public class AOVModel
    {
        /// <summary>
        /// Рекламный источник
        /// </summary>
        public string Source        { get; set; }
        /// <summary>
        /// Общая прибыль с источника
        /// </summary>
        public string TotalProfit   { get; set; }
        /// <summary>
        /// Количество клиентов
        /// </summary>
        public string CountOfClient { get; set; }
        /// <summary>
        /// Показатель AOV
        /// </summary>
        public string AOV           { get; set; }
    }
}
