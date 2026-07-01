namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingExcelFileDownloadDto
    {
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
    }
}
