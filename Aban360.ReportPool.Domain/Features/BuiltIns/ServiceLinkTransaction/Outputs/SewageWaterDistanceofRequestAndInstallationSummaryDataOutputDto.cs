namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto
    {
        public string ItemTitle { get; set; }
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string UsageTitle { get; set; }
        public float DistanceAverage { get; set; }
        public string DistanceAverageText { get; set; }
        public float? DistanceMedian { get; set; }
        public int CustomerCount { get; set; }

        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
    }
}