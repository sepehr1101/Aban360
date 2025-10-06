namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingListSummaryDataOutputDto
    {
        public bool IsFirstRow { get; set; }

        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string UsageTitle { get; set; }
        public string NextDay{ get; set; }
        public string ItemTitle { get; set; }
        public int ReadingCount { get; set; }
        public int CloseCount { get; set; }
        public int ObstacleCount { get; set; }
        public int ReplacementBranchCount { get; set; }
        public int MalfunctionCount { get; set; }
        public int AdvancePaymentCount { get; set; }
        public int NetCount { get; set; }
        public int SelfClaimedCount { get; set; }
        // public int SettlementCount { get; set; }
    }
}
