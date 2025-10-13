namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record InvalidPaymentHeaderOutputDto
    {
        public string  FromDateJalali { get; set; }
        public string  ToDateJalali { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public long TotalAmount { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }
    }
}
