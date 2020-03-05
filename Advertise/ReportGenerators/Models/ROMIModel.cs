namespace Advertise.ReportGenerators.Models
{
    /// <summary>
    /// Представление данных отчёта ROMI
    /// </summary>
    public class ROMIModel
    {
        /// <summary>
        /// Источник рекламы
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Общая прибыль с источника
        /// </summary>
        public string Profit { get; set; }
        /// <summary>
        /// Общаие расходы на источник
        /// </summary>
        public string Cost   { get; set; }
        /// <summary>
        /// Показатель ROMI
        /// </summary>
        public string ROMI   { get; set; }
    }
}
