namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterInfoByLastMonthlyConsumptionOutputDto
    {
        public string BillId { get; set; }
        public string PreviousDateJalali { get; set; } = default!;
        public string CurrentDateJalali { get; set; } = default!;
        public decimal MeetingNumber { get; set; }
        public int InvoiceId { get; set; }
        public int Cause { get; set; }
        public bool ShouldSave { get; set; }
        public MeterInfoByLastMonthlyConsumptionOutputDto(string billId, string previousDate, string currentDate, decimal meetingNumber, int invoiceId, int cause, bool shouldSave)
        {
            BillId = billId;
            PreviousDateJalali = previousDate;
            CurrentDateJalali = currentDate;
            MeetingNumber = meetingNumber;
            InvoiceId = invoiceId;
            Cause=cause;
            ShouldSave = shouldSave;
        }
        public MeterInfoByLastMonthlyConsumptionOutputDto()
        {
            
        }
    }
}
