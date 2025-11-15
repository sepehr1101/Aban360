namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitPossibilityDataOutputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int CommertialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyUnit { get; set; }
        public string BillId { get; set; }
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
        public float ConsumptionAverage { get; set; }
    }
}