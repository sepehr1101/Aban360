namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterInfoOutputDto
    {
        public string PreviousDateJalali { get; set; } = default!;
        public int PreviousNumber { get; set; }
        public string CurrentDateJalali { get; set; } = default!;
        public int CurrentNumber { get; set; }

        public int? CounterStateCode { get; set; }
        public MeterInfoOutputDto(string previousDate, string currentDate, int previousNumber, int currentNumber, int? counterStateCode)
        {
            PreviousDateJalali = previousDate;
            CurrentDateJalali = currentDate;
            PreviousNumber = previousNumber;
            CurrentNumber = currentNumber;
            CounterStateCode = counterStateCode;
        }
        public MeterInfoOutputDto()
        {
                
        }
    }
}
