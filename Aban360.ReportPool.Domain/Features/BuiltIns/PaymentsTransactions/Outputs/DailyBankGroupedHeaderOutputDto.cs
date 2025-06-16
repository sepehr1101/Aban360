namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record DailyBankGroupedHeaderOutputDto
    {
        public string ReportDate { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }
    }
}
