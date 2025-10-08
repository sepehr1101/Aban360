namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record HouseholdNumberDataOutputDto
    {
        public string RegisterDayJalali { get; set; } = default!;
        public string RegionTitle { get; set; } = default!;
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string UsageTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string EventDateJalali { get; set; }
        public string Address { get; set; }
        public string ZoneTitle { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public string BillId { get; set; }
        public string UseStateTitle { get; set; }
        public string HouseholdDateJalali { get; set; }
        public string ToHouseholdDateJalali { get; set; }
        public int HouseholdCount { get; set; }
        public bool IsValid { get; set; }
        public int TotalUnit { get; set; }

    }
}
