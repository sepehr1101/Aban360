namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record UnconfirmedSubscribersInputDto
    {
        //Other
        public ICollection<int> ZoneIds { get; set; }
    }
}
