namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record UnconfirmedSubscribersHeaderOutputDto
    {
        public string Title { get; set; }
        public string ReportDateJalali { get; set; }
        public int  RecordCount { get; set; }
        public long SumFinalAmount { get; set; }
        public long SumPreInstallmentAmount { get; set; }
        public int CustomerCount { get; set; }
    }
}
