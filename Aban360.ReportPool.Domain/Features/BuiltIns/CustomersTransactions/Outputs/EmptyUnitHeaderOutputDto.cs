namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitHeaderOutputDto
    {
        public string FromEmptyUnit { get; set; }
        public string ToEmptyUnit { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string ReportDate { get; set; }
        public int RecordCount { get; set; }
    }
}
