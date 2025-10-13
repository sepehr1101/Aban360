namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record RemovedBillHeaderOutputDto
    {
        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public string? FromAmount { get; set; }
        public string? ToAmount { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

        public long SumAmount { get; set; }
    }
}
