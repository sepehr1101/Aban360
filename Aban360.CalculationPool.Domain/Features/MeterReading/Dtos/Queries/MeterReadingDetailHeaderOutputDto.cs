namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public double Amount { get; set; }
        public double Consumption { get; set; }
    }
}
