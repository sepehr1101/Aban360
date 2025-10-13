namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PaymentDetailHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }
        public string? FromBankId { get; set; }
        public string? ToBankId { get; set; }
        public int RecordCount { get; set; }
        public long TotalAmount { get; set; }
        public string ReportDateJalali { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

    }
}
