namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record ServiceLinkDebtorCustomersInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public long FromAmout { get; set; }
        public long ToAmount { get; set; }
    }
}
