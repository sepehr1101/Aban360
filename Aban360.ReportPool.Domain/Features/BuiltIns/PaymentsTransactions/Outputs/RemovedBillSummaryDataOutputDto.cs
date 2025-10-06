namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record RemovedBillSummaryDataOutputDto
    {
        public bool IsFirstRow { get; set; }

        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string UsageTitle { get; set; }
        public string ItemTitle { get; set; }
        public int CustomerCount { get; set; }
        public int AverageConsumption { get; set; }
        public float SumConsumption { get; set; }
        public long Amount { get; set; }
    }
}
