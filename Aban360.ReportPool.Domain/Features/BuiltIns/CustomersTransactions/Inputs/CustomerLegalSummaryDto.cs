namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerLegalSummaryDto
    {
        public ICollection<int> ItemIds { get; set; }
        public bool IsZone { get; set; }
        public CustomerLegalSummaryDto(ICollection<int> itemIds, bool isZone)
        {
            ItemIds = itemIds;
            IsZone = isZone;
        }
    }
}
