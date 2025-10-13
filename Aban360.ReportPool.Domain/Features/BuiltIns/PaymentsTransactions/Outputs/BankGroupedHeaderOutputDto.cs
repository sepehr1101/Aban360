namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record BankGroupedHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int? FromBankId { get; set; }
        public int? ToBankId { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

        public int WaterCount { get; set; }
        public long WaterAmount { get; set; }
        public int ServiceLinkCount { get; set; }
        public long ServiceLinkAmount { get; set; }
        public int TotalCount { get; set; }
        public long TotalAmount { get; set; }

    }
}
