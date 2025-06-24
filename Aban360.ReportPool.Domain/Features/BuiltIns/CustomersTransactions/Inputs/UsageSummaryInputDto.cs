namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record UsageSummaryInputDto
    {
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public ICollection<int> UsageSellIds { get; set; }
        public ICollection<int> ZoneIds { get; set; }
    }
}
