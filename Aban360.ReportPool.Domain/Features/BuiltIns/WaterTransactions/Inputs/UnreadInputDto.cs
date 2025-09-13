namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record UnreadInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public int PeriodCount { get; set; }
        public ICollection<int> ZoneIds { get; set; }
    }
}
