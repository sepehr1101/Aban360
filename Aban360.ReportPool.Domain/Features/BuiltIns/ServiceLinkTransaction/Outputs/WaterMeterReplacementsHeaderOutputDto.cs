namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record WaterMeterReplacementsHeaderOutputDto
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportDate { get; set; }
        public int RecordCount { get; set; }
    }
}
