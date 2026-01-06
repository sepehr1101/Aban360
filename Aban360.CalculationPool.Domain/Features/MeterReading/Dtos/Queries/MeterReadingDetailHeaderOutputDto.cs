namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public double Amount { get; set; }
        public double Consumption { get; set; }

        public int Closed { get; set; }
        public int Obstacle { get; set; }
        public int Temporarily { get; set; }
        public int Ruined { get; set; }
        public int PureReading { get; set; }
    }
}
