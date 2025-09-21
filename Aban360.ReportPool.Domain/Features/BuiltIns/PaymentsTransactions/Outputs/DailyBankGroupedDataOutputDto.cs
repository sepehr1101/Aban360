namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record DailyBankGroupedDataOutputDto
    {
        public string RegisterDate { get; set; }
        public string BankDate { get; set; }
        public int ItemCount { get; set; }
        public long ItemAmount { get; set; }
        public int TotalCount { get; set; }
        public long TotalAmount { get; set; }
        public string ZoneTitle { get; set; }
        public string BankName { get; set; }

    }
}
