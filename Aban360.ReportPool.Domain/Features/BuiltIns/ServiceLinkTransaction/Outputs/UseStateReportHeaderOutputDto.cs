namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record UseStateReportHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public long TotalDebtAmount { get; set; }
        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }
        public string? ReportDateJalali { get; set; }
    }
}