namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record LinkServiceStatementInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
    }
}
