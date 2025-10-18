namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record UsageDetailInputDto
    {
        public string  FromDateJalali { get; set; }
        public string  ToDateJalali { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public ICollection<int> UsageSellIds { get; set; } = default!;
        public ICollection<int> ZoneIds { get; set; } = default!;
    }
}
