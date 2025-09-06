namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record ClientValidationDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string UsageTitle { get; set; }
        public int ContractualCapacity { get; set; }
        public bool HasSewage { get; set; }
        public string SewageInstallationDateJalali { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int CommercialArea { get; set; }
        public int DomesticArea { get; set; }
        public int FieldArea { get; set; }
        public int ConstructedArea { get; set; }
        public string Description { get; set; }
     
    }
}
