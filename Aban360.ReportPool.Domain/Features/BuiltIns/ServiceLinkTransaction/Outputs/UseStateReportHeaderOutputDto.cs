namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record UseStateReportHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string  TotalDeptAmount { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDateJalili { get; set; }
    }
}