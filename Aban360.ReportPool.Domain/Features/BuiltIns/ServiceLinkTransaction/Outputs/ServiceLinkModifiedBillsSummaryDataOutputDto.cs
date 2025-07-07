namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkModifiedBillsSummaryDataOutputDto
    {
        public string  ItemTitle { get; set; }
        public string ZoneTitle { get; set; }
        public long Amount { get; set; }
        public long OffAmount { get; set; }
        public long FinalAmount { get; set; }
    }
}
