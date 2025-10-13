namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerSearchHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }
    }
}
