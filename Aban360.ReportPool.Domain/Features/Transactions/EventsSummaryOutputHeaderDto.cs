namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record EventsSummaryOutputHeaderDto
    {
        public string FullName { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public string Address { get; set; }
        public string CustomerNumber { get; set; }
        public string LastCustomerNumber { get; set; }
        public string LastBillId { get; set; }
        public string UsageTitle { get; set; }
        public string? JobTitle { get; set; }
        public string? GuildTitle { get; set; }
        public int ContractualCapacity { get; set; }
        public int TotalUnit { get; set; }
        public bool HasTag { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public string WaterInstallationDate { get; set; }
        public string WaterReplacementDate { get; set; }
        public string WaterReplacementNumber { get; set; }

    }
}
