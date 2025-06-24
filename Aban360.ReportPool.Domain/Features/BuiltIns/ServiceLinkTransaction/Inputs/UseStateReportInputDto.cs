namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record UseStateReportInputDto
    {
        public short UseStateId { get; set; }
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; } = default!;
        public string FromReadingNumber { get; set; } = default!;
        public string ToReadingNumber { get; set; } = default!;
        public ICollection<int> ZoneIds { get; set; } = default!;
    }
}
