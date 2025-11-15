namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitPossibilityHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToateJalali { get; set; }

        public int CustomerCount { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }    
    }
}
