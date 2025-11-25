namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record ConsumptionPartialInfo
    {
        public double AllowedConsumption { get;}
        public double DisallowedConsumtion { get;}
        public double Consumption { get;}
        public int Duration { get;}
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string StartDateJalali { get; set; } = default!;
        public string EndDateJalali { get; set; } = default!;
        public ConsumptionPartialInfo(double consumption, double allowedConsumption, double disallowedConsumption, int duration, 
            DateOnly startDate, DateOnly endDate, string startDateJalali, string endDateJalali)
        {
            Consumption=consumption;
            AllowedConsumption = allowedConsumption;
            DisallowedConsumtion = disallowedConsumption;
            Duration = duration;
            StartDate = startDate;
            EndDate = endDate;
            StartDateJalali = startDateJalali;
            EndDateJalali = endDateJalali;           
        }
    }
}
