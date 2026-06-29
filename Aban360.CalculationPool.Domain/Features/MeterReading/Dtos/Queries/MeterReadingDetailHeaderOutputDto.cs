namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public double Amount { get; set; }
        public double Consumption { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public int Closed { get; set; }
        public int Obstacle { get; set; }
        public int Temporarily { get; set; }
        public int Malfunction { get; set; }
        public int PureReading { get; set; }
        public int NextRound{ get; set; }
        public int WithoutConsumption { get; set; }
        public int Changed { get; set; }
        public int Excluded { get; set; }
    }
}
