namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingStatusStatementSummaryDataOutputDto
    {
        public bool IsFirstRow { get; set; }

        public string ItemTitle { get; set; }
        public string RegionTitle { get; set; }
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int PureReading { get; set; }
        public int Closed { get; set; }
        public int Obstacle { get; set; }
        public int Temporarily { get; set; }
        public int AllCount { get; set; }
        public int Ruined { get; set; }
        public int SelfClaimedCount { get; set; }
        public double SumItems { get; set; }
        public double Debt { get; set; }
    }
}