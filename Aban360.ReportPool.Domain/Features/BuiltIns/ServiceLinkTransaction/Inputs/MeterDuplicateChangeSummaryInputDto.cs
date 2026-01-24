namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record MeterDuplicateChangeSummaryInputDto
    {
        public ICollection<int> ZoneIds { get; set; } = default!;
        public ICollection<int> UsageIds { get; set; } = default!;

        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;

        public bool IncludeBodySerial { get; set; }
        public bool IsRegisterDate { get; set; }
        public bool IsZoneGroup { get; set; }
    }
}
