namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record ServiceLinkDebtorCustomersInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public long FromAmount { get; set; }
        public long ToAmount { get; set; }
    }
}
