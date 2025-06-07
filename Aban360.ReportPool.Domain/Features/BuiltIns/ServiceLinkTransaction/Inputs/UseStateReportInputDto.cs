namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record UseStateReportInputDto
    {
        public short UseStateId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public ICollection<int> ZoneIds { get; set; }
    }
}
