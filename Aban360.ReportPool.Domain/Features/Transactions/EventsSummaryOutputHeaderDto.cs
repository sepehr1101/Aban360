namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record EventsSummaryOutputHeaderDto
    {
        public string FullName { get; set; }
        public string ReadingNumber { get; set; }
        public string UsageTitle { get; set; }
        public int TotalUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public int EmptyUnit { get; set; }
        public string DiscountTypeTitle{ get; set; }
        public int HouseholdNumber { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public int MeterTagCount { get; set; }
        public string UsageStateTitle { get; set; }
        public string WaterRequestDate { get; set; }
        public string SewageRequestDate { get; set; }
        public string WaterInstallationDate { get; set; }
        public string SewageInstallationDate { get; set; }
    }
}
