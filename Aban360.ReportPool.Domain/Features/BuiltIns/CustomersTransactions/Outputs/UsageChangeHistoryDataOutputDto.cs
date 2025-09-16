namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record UsageChangeHistoryDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string FromUsageTitle { get; set; }
        public string ToUsageTitle { get; set; }
        public string ChangeDateJalali { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public string Address { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public string BillId { get; set; }
        public string UseStateTitle { get; set; }
        public int EmptyUnit { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FatherName { get; set; }
        public int Distance { get; set; }
        public string DistanceText { get; set; }
    }
}
