namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record BankGroupedDataOutputDto
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public int WaterCount { get; set; }
        public long WaterAmount { get; set; }
        public long ServiceLinkAmount { get; set; }
        public int ServiceLinkCount { get; set; }
        public long TotalAmount { get; set; }
        public int TotalCount { get; set; }

    }
}