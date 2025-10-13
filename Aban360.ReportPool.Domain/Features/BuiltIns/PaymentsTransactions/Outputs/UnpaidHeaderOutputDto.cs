namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record UnpaidHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public string? Title { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public long DebtAmount { get; set; }

    }
}
