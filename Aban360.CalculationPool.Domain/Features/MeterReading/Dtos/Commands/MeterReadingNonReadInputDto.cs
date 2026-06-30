namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingNonReadInputDto
    {
        public int ZoneId { get; set; }
        public string CurrentDateJalali { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
    }
}