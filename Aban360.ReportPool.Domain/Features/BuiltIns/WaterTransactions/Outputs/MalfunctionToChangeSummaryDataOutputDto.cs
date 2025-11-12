namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionToChangeSummaryDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string Duration { get; set; }
        public string MaxDuration { get; set; }
        public string MinDuration { get; set; }
        public int Count { get; set; }
    }
}
