namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record ConnectDisconnectPrintsDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public int CustomerNumber2 { get; set; }
        public string ReadingNumber { get; set; }
        public long DebtAmount { get; set; }
        public string EmptyDueDateJalali { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string? Address { get; set; }
        public int UsageId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public int DebtPeriodCount { get; set; }
        public string PreviousReadingDateJalali { get; set; }
        public long PreviousBillAmount { get; set; }
        public string DueDateJalali { get; set; }
        public string BillId { get; set; }
        public string PayId { get; set; }

    }
}
