namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record UnpaidHeaderOutputDto
    {
        public string FromDateTime { get; set; }
        public string ToDateTime { get; set; }

        public long FromAmount { get; set; }
        public long ToAmount { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public string ReportDate { get; set; }
        public int RecordCount { get; set; }
    }
}
