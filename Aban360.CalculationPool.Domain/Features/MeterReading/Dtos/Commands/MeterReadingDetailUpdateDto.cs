namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailUpdateDto
    {
        public int Id { get; set; }
        public short CurrentCounterStateCode { get; set; }
        public string? CurrentDateJalali { get; set; }
        public int CurrentNumber { get; set; }
        public float? MonthlyAverage { get; set; }
    }
}
