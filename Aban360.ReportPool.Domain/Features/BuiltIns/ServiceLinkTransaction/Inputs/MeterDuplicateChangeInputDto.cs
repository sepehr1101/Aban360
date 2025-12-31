namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record MeterDuplicateChangeInputDto
    {
        public ICollection<int> ZoneIds { get; set; } = default!;
        public ICollection<int> UsageIds { get; set; } = default!;

        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;

        public bool IsRegisterDate { get; set; }
    }
}
