namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterUsageGroupedHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string ReportDateJalali { get; set; }
        public long TotalAmount { get; set; }
        public int RecordCount { get; set; }
    }
}
