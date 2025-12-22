namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record ConsumptionPartialInfo
    {
        public double AllowedRatio
        {
            get
            {
                double allowedRatio = Consumption == 0 ? 1 : AllowedConsumption / Consumption;
                return allowedRatio;
            }
        }
        public double DisallwedRatio
        {
            get
            {
                return 1 - AllowedRatio;
            }
        }
        public double AllowedConsumption { get;}
        public double DisallowedConsumtion { get;}
        public double Consumption { get;}
        public int Duration { get;}
        public double OlgooOrCapacityInDuration { get; }
        public DateOnly StartDate { get; }
        public DateOnly EndDate { get; }
        public string StartDateJalali { get; set; } = default!;
        public string EndDateJalali { get; set; } = default!;
        public ConsumptionPartialInfo(double consumption, double allowedConsumption, double disallowedConsumption, int duration, 
            DateOnly startDate, DateOnly endDate, string startDateJalali, string endDateJalali, double olgooOrCapacityInDuration)
        {
            Consumption=consumption;
            AllowedConsumption = allowedConsumption;
            DisallowedConsumtion = disallowedConsumption;
            Duration = duration;
            StartDate = startDate;
            EndDate = endDate;
            StartDateJalali = startDateJalali;
            EndDateJalali = endDateJalali;       
            OlgooOrCapacityInDuration = olgooOrCapacityInDuration;
        }
    }
}
