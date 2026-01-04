namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterDuplicateChangeHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int RecordCount { get; set; }
        public string? ReportDateJalali { get; set; }
        public string? Title { get; set; }
        public int CustomerCount { get; set; }
        public int MeterChangeCount { get; set; }

        public int FirstChange { get; set; }
        public int SecondChange { get; set; }
        public int MoreThanThirdChange { get; set; }
    }
}