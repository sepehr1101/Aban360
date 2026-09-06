namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record BedBesPreviousNumberAndDateOutputDto
    {
        public int CustomerNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public BedBesPreviousNumberAndDateOutputDto(int customerNumber, string previousDateJalali, int previousNumber, int counterStateCode, string counterStateTitle, int consumption, float consumptionAverage)
        {
            CustomerNumber = customerNumber;
            PreviousDateJalali = previousDateJalali;
            PreviousNumber = previousNumber;
            CounterStateCode = counterStateCode;
            CounterStateTitle = counterStateTitle;
            Consumption = consumption;
            ConsumptionAverage = consumptionAverage;
        }
        public BedBesPreviousNumberAndDateOutputDto()
        {
        }
    }
}
