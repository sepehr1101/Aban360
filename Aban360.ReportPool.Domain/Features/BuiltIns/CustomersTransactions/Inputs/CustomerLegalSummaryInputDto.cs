namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerLegalSummaryInputDto
    {
        public ICollection<int> ItemIds { get; set; }

    }
}
