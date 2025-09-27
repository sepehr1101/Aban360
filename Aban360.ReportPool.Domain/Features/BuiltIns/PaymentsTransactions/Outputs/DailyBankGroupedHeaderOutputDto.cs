namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record DailyBankGroupedHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int? FromBankId { get; set; }
        public int? ToBankId { get; set; }
        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }
        public int CustomerCount { get; set; }

        public int TotalCount { get; set; }
        public long TotalAmount { get; set; }
    }
}
