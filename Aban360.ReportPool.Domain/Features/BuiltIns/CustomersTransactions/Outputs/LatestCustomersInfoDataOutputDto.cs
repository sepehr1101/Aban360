namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record LatestCustomersInfoDataOutputDto
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string? NationalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int Premises { get; set; }
        public int ImprovementsOverall { get; set; }
        public int ImprovementsDomestic { get; set; }
        public int ImprovementsCommercial{ get; set; }
        public int ImprovementsOther{ get; set; }
        public int ContractualCapacity { get; set; }
        public string? MeterRequestDateJalali { get; set; }
        public string? MeterInstallationDateJalali { get; set; }
        public string? SewageRequestDateJalali { get; set; }
        public string? SewageInstallationDateJalali { get; set; }


        public string LatestReadingDateJalali { get; set; }
        public int LatestMeterNumber { get; set; }
        public long WaterRemained { get; set; }
        public long SubscriptionRemained { get; set; }


    }
}
