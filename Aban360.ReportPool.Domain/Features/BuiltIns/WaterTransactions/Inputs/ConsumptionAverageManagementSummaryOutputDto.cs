namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionAverageManagementSummaryOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public float Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public int ContracutalOrOlgo { get; set; }
        public string RegisterDateJalali { get; set; }
    }
}