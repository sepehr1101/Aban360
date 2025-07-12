namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterRawSalesSummaryDataOutputDto
    {
        public string UsageTitle { get; set; }
        public int Count { get; set; }
        public long Payable { get; set; }
    }
}
