namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record FinancialStatementDataOutputDto
    {
        public string UsageGroupTitle { get; set; }
        public int CustomerCount { get; set; }
        public int ConsumptionTotalUnit { get; set; }
        public int DailyAverage { get; set; }
        public int NetConsumption { get; set; }
        public long NetAmount { get; set; }
        public int ReturnedConsumption { get; set; }
        public long ReturnedAmount { get; set; }
        public long DiscountAmount { get; set; }
        public int RawConsumption { get; set; }
        public long RawAmount { get; set; }
        public float RawAmountAverage { get; set; }
        public float ConsumptionAverageInMonth { get; set; }
    }
}
