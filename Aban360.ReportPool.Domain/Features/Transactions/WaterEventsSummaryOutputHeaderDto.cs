namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record WaterEventsSummaryOutputHeaderDto
    {
        public string FullName { get; set; } = default!;
        public string ReadingNumber { get; set; } = default!;
        public string BillId { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string CustomerNumber { get; set; } = default!;
        public string LastCustomerNumber { get; set; } = default!;
        public string LastBillId { get; set; } = default!;
        public string UsageTitle { get; set; } = default!;
        public string? JobTitle { get; set; }
        public string? GuildTitle { get; set; }
        public int ContractualCapacity { get; set; }
        public int TotalUnit { get; set; }
        public bool HasTag { get; set; }
        public string MeterDiameterTitle { get; set; } = default!;
        public string SiphonDiameterTitle { get; set; } = default!;
        public string WaterInstallationDate { get; set; } = default!;
        public string WaterReplacementDate { get; set; } = default!;
        public string WaterReplacementNumber { get; set; } = default!;
        public int ZoneId { get; set; }
        public long Remained { get; set; }
        public string ReportDateJalali{ get; set; } = default!;
        public string Title{ get; set; } = default!;

    }
}
