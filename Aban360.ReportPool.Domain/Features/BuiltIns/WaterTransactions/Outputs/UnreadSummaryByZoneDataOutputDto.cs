namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record UnreadSummaryByZoneDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public int Closed { get; set; }
        public int Barrier { get; set; }

    }
}
