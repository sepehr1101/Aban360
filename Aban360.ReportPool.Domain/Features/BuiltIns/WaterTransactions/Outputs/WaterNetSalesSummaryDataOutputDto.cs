namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterNetSalesSummaryDataOutputDto
    {
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
        public long Payable { get; set; }
        public int Count { get; set; }
    }
}
