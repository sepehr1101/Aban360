namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingStatusStatementSummaryByZoneDataOutputDto
    {
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int ReadingNet { get; set; }
        public int Closed { get; set; }
        public int Obstacle { get; set; }
        public int Temporarily { get; set; }
        public int AllCount { get; set; }
        public int Ruined { get; set; }
        public int SelfClaimedCount { get; set; }
        public double SumItems { get; set; }
    }
}