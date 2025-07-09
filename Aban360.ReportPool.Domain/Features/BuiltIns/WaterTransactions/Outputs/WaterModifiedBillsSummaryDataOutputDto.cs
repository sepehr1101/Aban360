namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterModifiedBillsSummaryDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string UsageTitle { get; set; }
        public string TypeTitle { get; set; }
        public int Count { get; set; }
        public long Payable { get; set; }
        public long SumItems { get; set; }
    }
}
