namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingNonReadOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public int Count { get; set; }
        public MeterReadingNonReadOutputDto(int zoneId, string zoneTitle, string fromReadinNumber, string toReadinNumber, int count)
        {
            ZoneId = zoneId;
            ZoneTitle = zoneTitle;
            FromReadingNumber = fromReadinNumber;
            ToReadingNumber = toReadinNumber;
            Count = count;
        }
        public MeterReadingNonReadOutputDto()
        {
        }
    }
}
