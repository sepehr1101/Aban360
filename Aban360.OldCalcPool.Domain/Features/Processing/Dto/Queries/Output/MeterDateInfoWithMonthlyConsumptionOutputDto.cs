namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterDateInfoWithMonthlyConsumptionOutputDto
    {
        public string BillId { get; set; }
        public string PreviousDateJalali { get; set; } = default!;
        public string CurrentDateJalali { get; set; } = default!;
        public double MonthlyAverageConsumption { get; set; }
        public decimal MeetingNumber { get; set; }
        public int InvoiceId { get; set; }
        public int Cause { get; set; }
        public bool ShouldSave { get; set; }
        public MeterDateInfoWithMonthlyConsumptionOutputDto(string billId, string previousDate, string currentDate, double monthlyAverageConsumption, decimal meetingNumber, int invoiceId, int cause, bool shouldSave)
        {
            BillId = billId;
            PreviousDateJalali = previousDate;
            CurrentDateJalali = currentDate;
            MonthlyAverageConsumption = monthlyAverageConsumption;
            MeetingNumber = meetingNumber;
            InvoiceId = invoiceId;
            Cause=cause;
            ShouldSave = shouldSave;
        }
        public MeterDateInfoWithMonthlyConsumptionOutputDto()
        {

        }
    }
}
