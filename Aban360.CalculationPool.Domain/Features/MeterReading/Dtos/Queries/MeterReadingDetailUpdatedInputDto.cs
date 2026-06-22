namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailUpdatedInputDto
    {
        public int ZoneId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
    }
}
