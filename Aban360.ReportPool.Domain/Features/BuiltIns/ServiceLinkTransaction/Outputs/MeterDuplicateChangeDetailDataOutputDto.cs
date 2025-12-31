namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterDuplicateChangeDetailDataOutputDto
    {
        public string FullName { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int MeterChangeCount { get; set; }
        public string UsageTitle { get; set; }
        public string MeterDiamterTitle { get; set; }
        public string BranchTypeTitle { get; set; }
    }
}