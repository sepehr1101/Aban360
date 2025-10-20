namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string UsageTitle { get; set; } = default!;
        public string MeterDiameterTitle { get; set; } = default!;
        public string SiphonDiameterTitle { get; set; } = default!;
        public string EventDateJalali { get; set; } = default!;
        public string Address { get; set; } = default!;
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public string BillId { get; set; } = default!;
        public string UseStateTitle { get; set; } = default!;
        public int EmptyUnit { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = default!;
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = default!;
        public string NationalCode { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string MobileNumber { get; set; } = default!;
        public string FatherName { get; set; } = default!;
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public double SumItems { get; set; }

    }
}
