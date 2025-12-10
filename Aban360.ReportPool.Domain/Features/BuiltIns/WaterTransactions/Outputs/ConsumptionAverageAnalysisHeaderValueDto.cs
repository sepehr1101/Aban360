namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ConsumptionAverageAnalysisHeaderValueDto
    {
        public string FromToConsumption { get; set; }

        public int Count { get; set; }
        public int Unit { get; set; }
    }
}
