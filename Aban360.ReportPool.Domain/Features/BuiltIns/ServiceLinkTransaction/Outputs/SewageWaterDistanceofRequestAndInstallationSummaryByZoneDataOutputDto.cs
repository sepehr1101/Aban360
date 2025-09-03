namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterDistanceofRequestAndInstallationSummaryByZoneDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public float DistanceAverage { get; set; }
        public string DistanceAverageText { get; set; }
        public float? DistanceMedian { get; set; }
    }
}
