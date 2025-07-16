namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record RuinedMeterIncomeSummaryDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RuinedCount { get; set; }
        public long Payable { get; set; }
        public long SumItems { get; set; }
    }
}
