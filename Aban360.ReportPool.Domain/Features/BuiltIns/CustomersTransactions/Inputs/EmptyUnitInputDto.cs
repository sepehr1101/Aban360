namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record EmptyUnitInputDto
    {
        public string FromEmptyUnit { get; set; }
        public string ToEmptyUnit { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public ICollection<int> UsageSellIds { get; set; }
        public ICollection<int> ZoneIds { get; set; }
    }
}
