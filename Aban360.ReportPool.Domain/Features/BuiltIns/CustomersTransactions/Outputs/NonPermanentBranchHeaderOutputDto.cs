namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record NonPermanentBranchHeaderOutputDto
    {
        public string? FromReadingNumber{ get; set; }
        public string? ToReadingNumber { get; set; }

        public string ReportDate { get; set; }
        public int RecordCount { get; set; }
    }
}
