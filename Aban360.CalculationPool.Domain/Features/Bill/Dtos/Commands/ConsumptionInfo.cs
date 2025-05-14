namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record ConsumptionInfo
    {
        public int Consumption { get; set; }
        public int Duration { get; set; }
        public double AverageConsumption { get; set; }
        public string PreviousReadingDate { get; set; }
        public string CurrentReadingDate { get; set; }
        public ConsumptionInfo(string @from, string @to, int consumption, int duration, double averageConsumption)
        {
            PreviousReadingDate = @from;
            CurrentReadingDate = @to;
            Consumption = consumption;
            Duration = duration;
            AverageConsumption = averageConsumption;
        }
    }
}