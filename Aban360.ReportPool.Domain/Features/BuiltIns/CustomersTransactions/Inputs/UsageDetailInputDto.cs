namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record UsageDetailInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public ICollection<int> UsageSellIds { get; set; } = default!;
        public ICollection<int> ZoneIds { get; set; } = default!;
    }
}
