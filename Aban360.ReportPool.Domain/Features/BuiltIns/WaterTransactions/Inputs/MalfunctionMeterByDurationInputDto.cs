namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record MalfunctionMeterByDurationInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }

        public int FromMalfunctionPeriodCount { get; set; }
        public int ToMalfunctionPeriodCount { get; set; }
        public ICollection<int>? ZoneIds { get; set; }
    }
}
