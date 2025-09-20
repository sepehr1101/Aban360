namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record UseStateReportDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string UsageTitle { get; set; } = default!;
        public string MeterDiameterTitle { get; set; } = default!;
        public string SiphonDiameterTitle { get; set; }
        public string EventDateJalali { get; set; } = default!;
        public long DebtAmount { get; set; }
        public string Address { get; set; } = default!;
        public string ZoneTitle { get; set; } = default!;
        public string DeletionStateTitle { get; set; } = default!;
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public string BillId { get; set; } = default!;
        public int ContractualCapacity { get; set; }
        public string BodySerial { get; set; }
        public string MeterInstallationDateJalali { get; set; }
        public string MeterRequestDateJalali { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string PostalCode { get; set; }
    }
}