namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record NonPermanentBranchSummaryDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string UsageTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int Count { get; set; }
        //public int CustomerCount { get; set; }
    }
}