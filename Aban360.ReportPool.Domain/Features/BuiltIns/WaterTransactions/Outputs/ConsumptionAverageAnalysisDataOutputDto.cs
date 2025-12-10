namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ConsumptionAverageAnalysisDataOutputDto
    {
        public string ItemTitle { get; set; }
        public string FromToConsumption { get; set; }
        public int Count { get; set; }
        public int Unit { get; set; }
    }
}
