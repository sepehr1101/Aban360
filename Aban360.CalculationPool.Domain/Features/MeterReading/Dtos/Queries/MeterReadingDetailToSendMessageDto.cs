namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailToSendMessageDto
    {
        public int MeterReadingDetailId { get; set; }
        public int FlowImportedId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public long Payable { get; set; }
        public string BillId { get; set; }
        public string PaymentId { get; set; }
        public int CounterSatetCode { get; set; }
        public string RegisterDateJalali { get; set; }
        public string DueDateJalali { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public long DebtAmount { get; set; }

    }
}
