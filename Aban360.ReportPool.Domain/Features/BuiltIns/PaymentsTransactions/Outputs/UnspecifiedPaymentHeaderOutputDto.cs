namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record UnspecifiedPaymentHeaderOutputDto
    {
        public string FileName { get; set; }
        public int RecordCount { get; set; }
        public long TotalAmount { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }
        public string ReportDateJalali { get; set; }
    }
}

