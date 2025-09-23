namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record MalfunctionMeterByDurationInputDto
    {
        public bool IsMalfunctionLatest { get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public int FromMalfunctionPeriodCount { get; set; }
        public int ToMalfunctionPeriodCount { get; set; }
        public ICollection<int>? ZoneIds { get; set; }
    }
}
