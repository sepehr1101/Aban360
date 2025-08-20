namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterDistanceofRequestAndInstallationHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public string AvergeDistance { get; set; }
        public string MaxDistance { get; set; }
    }
}
