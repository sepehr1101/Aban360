namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record PendingPaymentsInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public string FromDateJalali { get; set; }//todo: validation: morethan 1401/01/01
        public string ToDateJalali { get; set; }//todo: befor today
        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }
        public int? FromDebtPeriodCount { get; set; }
        public int? ToDebtPeriodCount { get; set; }
        public ICollection<int>? UsageConsumptionIds { get; set; }
        public ICollection<int>? UsageSellIds { get; set; }
        public ICollection<int> ZoneIds { get; set; }


    }

}
