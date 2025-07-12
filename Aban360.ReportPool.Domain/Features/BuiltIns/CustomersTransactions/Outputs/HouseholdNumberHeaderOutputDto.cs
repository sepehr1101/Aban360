namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record HouseholdNumberHeaderOutputDto
    {
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public string FromHouseholdDateJalali { get; set; }
        public string ToHouseholdDateJalali { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
    }
}
