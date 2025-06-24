namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PendingPaymentsHeaderOutputDto
    {
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string FromAmount { get; set; }
        public string ToAmount { get; set; }
        public int FromDebtPeriodCount { get; set; }
        public int ToDebtPeriodCount { get; set; }
        public string ZoneTitle { get; set; }
        public int RecordCount { get; set; }
        public long TotalDebtPeriodCount { get; set; }
        public long TotalBeginDebt { get; set; }
        public long TotalEndingDebt { get; set; }
        public long TotalPayedAmount { get; set; }
    }

}
