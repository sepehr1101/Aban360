namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterModifiedBillsHeaderOutputDto
    {
        public string FromDataJalali { get; set; }
        public string ToDataJalali { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public long TotalInstallationAmount { get; set; }
        public long TotalServiceLinkAmount { get; set; }
        public long TotalImpureAmount { get; set; }
        public long TotalSumAmount { get; set; }
    }
}
