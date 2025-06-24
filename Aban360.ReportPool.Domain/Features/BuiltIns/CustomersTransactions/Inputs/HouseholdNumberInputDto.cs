namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record HouseholdNumberInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
    }
}
