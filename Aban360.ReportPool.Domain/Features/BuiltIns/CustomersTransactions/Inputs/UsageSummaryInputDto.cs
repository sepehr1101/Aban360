namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record UsageSummaryInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public ICollection<int> UsageSellIds { get; set; } = default!;
        public ICollection<int> ZoneIds { get; set; } = default!;
    }
}
