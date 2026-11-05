namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingUploadToRegisteredInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public IEnumerable<int> ZoneIds { get; set; }
    }
}
