namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailExcludedInputDto
    {
        public int ZoneId { get; set; }
        public string DateJalali { get; set; }
    }
}
