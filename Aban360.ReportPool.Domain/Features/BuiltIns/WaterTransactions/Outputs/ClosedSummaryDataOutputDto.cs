namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ClosedSummaryDataOutputDto
    {
        public string  RegionTitle { get; set; }
        public string ItemTitle { get; set; }
        public int CustomerCount { get; set; }
    }

}
