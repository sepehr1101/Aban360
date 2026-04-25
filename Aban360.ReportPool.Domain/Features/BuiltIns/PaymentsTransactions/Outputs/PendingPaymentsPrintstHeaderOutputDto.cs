namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PendingPaymentsPrintstHeaderOutputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }
        public int? FromDebtPeriodCount { get; set; }
        public int? ToDebtPeriodCount { get; set; }
        public int ZoneCount { get; set; }
        public int RecordCount { get; set; }
        public long TotalDebtPeriodCount { get; set; }
        public long TotalDebtAmount { get; set; }
        public string ReportDateJalali { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }
    }
}
