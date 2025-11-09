namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterDateInfoByLastMonthlyConsumptionOutputDto
    {
        public string BillId { get; set; }
        public string PreviousDateJalali { get; set; } = default!;
        public string CurrentDateJalali { get; set; } = default!;
        public MeterDateInfoByLastMonthlyConsumptionOutputDto(string billId, string previousDate, string currentDate)
        {
            BillId = billId;
            PreviousDateJalali = previousDate;
            CurrentDateJalali = currentDate;
        }
        public MeterDateInfoByLastMonthlyConsumptionOutputDto()
        {

        }
    }
}
