namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto
    {
        public string ItemTitle { get; set; }
        public float DistanceAverage { get; set; }
        public string DistanceAverageText { get; set; }
        public float? DistanceMedian { get; set; }
    }
}
