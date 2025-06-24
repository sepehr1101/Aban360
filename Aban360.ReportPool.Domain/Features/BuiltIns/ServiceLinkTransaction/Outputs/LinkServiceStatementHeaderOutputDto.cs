namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record LinkServiceStatementHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReportDate { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
