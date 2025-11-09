namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterDateInfoWithMonthlyConsumptionOutputDto
    {
        public string BillId { get; set; }
        public string PreviousDateJalali { get; set; } = default!;
        public string CurrentDateJalali { get; set; } = default!;
        public double MonthlyAverageConsumption { get; set; }
        public MeterDateInfoWithMonthlyConsumptionOutputDto(string billId, string previousDate, string currentDate, double monthlyAverageConsumption)
        {
            BillId = billId;
            PreviousDateJalali = previousDate;
            CurrentDateJalali = currentDate;
            MonthlyAverageConsumption = monthlyAverageConsumption;
        }
        public MeterDateInfoWithMonthlyConsumptionOutputDto()
        {

        }
    }
}
