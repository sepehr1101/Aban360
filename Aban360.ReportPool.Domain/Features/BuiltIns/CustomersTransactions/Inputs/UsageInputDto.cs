namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record UsageInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
    }
}
