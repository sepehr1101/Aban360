namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerLegalInputDto
    {
        public ICollection<int>  ZoneIds { get; set; }
    }
}
