namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record PendingPaymentsInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public string? FromDateJalali { get; set; }//todo: validation: morethan 1401/01/01
        public string? ToDateJalali { get; set; }//todo: befor today
        public string FromAmount { get; set; }
        public string ToAmount { get; set; }
        public int FromDebtPeriodCount { get; set; }
        public int ToDebtPeriodCount { get; set; }
        public int UsageConsumptionId { get; set; }
        public int UsageSellId { get; set; }
        public int RegionId { get; set; }
        public int ZoneId { get; set; }


    }

}
