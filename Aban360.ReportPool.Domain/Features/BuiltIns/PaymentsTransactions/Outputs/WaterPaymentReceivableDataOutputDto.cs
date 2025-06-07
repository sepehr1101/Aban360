namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterPaymentReceivableDataOutputDto
    {
        public string UsageTitle { get; set; }
        public int TotalCount { get; set; }
        public int OverdueCount { get; set; }//
        public long OverdueAmount { get; set; }
        public long CurrentAmount { get; set; }
        public long TotalAmount { get; set; }

    }
}
