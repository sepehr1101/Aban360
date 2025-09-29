namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterPaymentReceivableDataOutputDto
    {
        public string CustomerNumber { get; set; }
        public string FullName { get; set; }
        public string ZoneTitle { get; set; }
        public string UsageTitle { get; set; }
        public long Amount { get; set; }
        public string EventDateJalali { get; set; }
        public string AmountState { get; set; }
    }
    public record WaterPaymentReceivableSummaryDataOutputDto
    {
        public string ItemTitle { get; set; }
        public long Amount { get; set; }
        public long DueAmount { get; set; }
        public long OverdueAmount { get; set; }
        public int DueCount { get; set; }
        public int OverdueCount { get; set; }
        public int? BillCount { get; set; }
        public int? CustomerCount { get; set; }

    }
}