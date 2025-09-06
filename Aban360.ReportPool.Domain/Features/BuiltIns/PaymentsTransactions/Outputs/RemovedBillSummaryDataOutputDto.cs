namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record RemovedBillSummaryDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public int AverageConsumption { get; set; }
        public float SumConsumption { get; set; }
        public long Amount { get; set; }
    }
}
