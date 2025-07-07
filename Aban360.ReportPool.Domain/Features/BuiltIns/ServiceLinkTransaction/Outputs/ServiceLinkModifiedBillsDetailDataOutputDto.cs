namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkModifiedBillsDetailDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public long Amount { get; set; }
        public long OffAmount { get; set; }
        public long FinalAmount { get; set; }
    }
}
