namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record InstallationPrintDataOutputDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string BillId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string UsageTitle { get; set; }
        public int UsageId { get; set; }
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int BranchTypeId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int Premises { get; set; }
        public int ImprovementsCommercial { get; set; }
        public int ImprovementsDomestic { get; set; }
        public int ImprovementsOverall { get; set; }
        public int ContractualCapacity { get; set; }
        public int HouseholdeNumber { get; set; }
        public int EmptyUnit { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public string Base64Image { get; set; }
    }
}
