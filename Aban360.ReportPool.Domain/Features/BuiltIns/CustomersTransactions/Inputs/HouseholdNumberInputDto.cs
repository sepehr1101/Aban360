namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record HouseholdNumberInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string FromHouseholdDateJalali { get; set; }
        public string ToHouseholdDateJalali { get; set; }

        public ICollection<int> ZoneIds { get; set; }
    }
}
