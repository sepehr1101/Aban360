namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record ConsumptionInfo
    {       
        public int MonthDays { get; } = 30;
        public int FinalDomesticUnit { get; set; }
        public double Consumption { get; set; }
        public int Duration { get; set; }
        public double DailyAverageConsumption { get; set; }
        public double MonthlyAverageConsumption { get; }
        public string PreviousReadingDate { get; set; }
        public string CurrentReadingDate { get; set; }
        public ConsumptionInfo(string @from, string @to, double consumption, int duration, double dailyAverageConsumption, int finalDomesticUnit)
        {
            PreviousReadingDate = @from;
            CurrentReadingDate = @to;
            Consumption = consumption;
            Duration = duration;
            DailyAverageConsumption = dailyAverageConsumption;
            MonthlyAverageConsumption = dailyAverageConsumption * MonthDays;
            FinalDomesticUnit= finalDomesticUnit;
        }
    }
}