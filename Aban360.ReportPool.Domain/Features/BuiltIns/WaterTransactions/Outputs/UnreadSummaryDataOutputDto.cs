namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record UnreadSummaryDataOutputDto
    {
        public string ItemTitle { get; set; }
        public int Closed { get; set; }
        public int Barrier { get; set; }

    }
}
