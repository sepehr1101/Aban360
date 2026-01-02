namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterDuplicateChangeSummaryDataOutputDto
    {
        public string ItemTitle { get; set; }
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerCount { get; set; }
        public int MeterChangeCount { get; set; }
        public int FirstChange { get; set; }
        public int SecondChange { get; set; }
        public int MoreThanThirdChange { get; set; }
    }
}