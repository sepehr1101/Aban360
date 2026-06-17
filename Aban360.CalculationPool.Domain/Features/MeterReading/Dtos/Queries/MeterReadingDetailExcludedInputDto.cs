namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailExcludedInputDto
    {
        public int ZoneId { get; set; }
        public string FromExcludeDateJalali { get; set; }
        public string ToExcludeDateJalali { get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
    }
}
