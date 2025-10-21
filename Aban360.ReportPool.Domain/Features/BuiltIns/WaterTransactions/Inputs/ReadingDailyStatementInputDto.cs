namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ReadingDailyStatementInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int? FromConsumption { get; set; }
        public int? ToConsumption { get; set; }

        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }

        public ICollection<int> ZoneIds { get; set; }
    }
}
