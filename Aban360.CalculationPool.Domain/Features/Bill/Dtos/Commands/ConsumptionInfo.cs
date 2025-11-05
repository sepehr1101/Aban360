namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record ConsumptionInfo
    {
        public int Consumption { get;}
        public int Duration { get; set; }
        public double DailyAverageConsumption { get;}
        public double MonthlyAverageConsumption { get; }
        public string PreviousReadingDate { get; }
        public string CurrentReadingDate { get;}
        public ConsumptionInfo(string @from, string @to, int consumption, int duration, double dailyAverageConsumption)
        {
            PreviousReadingDate = @from;
            CurrentReadingDate = @to;
            Consumption = consumption;
            Duration = duration;
            DailyAverageConsumption = dailyAverageConsumption;
            MonthlyAverageConsumption= dailyAverageConsumption*30;
        }
    }
}