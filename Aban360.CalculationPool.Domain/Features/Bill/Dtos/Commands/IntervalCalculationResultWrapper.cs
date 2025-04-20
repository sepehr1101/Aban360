namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record IntervalCalculationResultWrapper()
    {
        public ICollection<IntervalCalculationResult3>? IntervalCalculationResults { get; set; }
        public int Consumption { get; set; }
        public double ConsumptionAverage { get; set; }
        public int IntervalCount { get; set; }
        public double Amount { get; set; }
        public int Duration { get; set; }
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; } = default!;
        public IntervalCalculationResultWrapper(int consumption, int duration, double average, string @from, string @to) : this()
        {
            Consumption = consumption;
            Duration = duration;
            ConsumptionAverage = average;
            IntervalCount = duration;
            FromDate = @from;
            ToDate = @to;
        }
    }
}