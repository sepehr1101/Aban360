namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record LatestCustomersInfoInputDto
    {
        public IEnumerable<int> ZoneIds { get; set; }
    }
}
