namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto
    {
        public string UsageTitle { get; set; }
        public float DistanceAverage { get; set; }
        public float? DistanceMedian { get; set; }
    }
}
