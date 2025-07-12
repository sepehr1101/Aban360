namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record UseStateReportInputDto
    {
        public short UseStateId { get; set; }
        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;
        public string FromReadingNumber { get; set; } = default!;
        public string ToReadingNumber { get; set; } = default!;
        public ICollection<int> ZoneIds { get; set; } = default!;
    }
}
