namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record WithoutSewageRequestDetailDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string FullName { get; set; } = default!;
        public string ReadingNumber { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string UsageTitle { get; set; } = default!;
        public string MeterDiameterTitle { get; set; } = default!;
        public string SiphonDiameterTitle { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string ZoneTitle { get; set; } = default!;
        public int DomesticUnit { get; set; } = default!;
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public string BillId { get; set; }
        public string UseStateTitle { get; set; } = default!;
        public int ContractualCapacity { get; set; } = default!;
        public string WaterRequestDate { get; set; } = default!;
        public string WaterInstallationDate { get; set; } = default!;
        public string WaterRegistrationDate { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string MobileNumber { get; set; } = default!;
        public string DeletionStateTitle { get; set; } = default!;
        public string BodySerial { get; set; } = default!;
        public string NatoinalCode { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string BranchTypeTitle { get; set; } = default!;
    }
}
