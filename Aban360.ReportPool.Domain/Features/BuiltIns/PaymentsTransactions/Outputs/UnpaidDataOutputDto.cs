namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record UnpaidDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FullName { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string UsageSellTitle { get; set; }
        public long DebtAmount { get; set; }
        public string Address { get; set; }
        public int PeriodCount { get; set; }
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string BillId { get; set; }

    }
}
