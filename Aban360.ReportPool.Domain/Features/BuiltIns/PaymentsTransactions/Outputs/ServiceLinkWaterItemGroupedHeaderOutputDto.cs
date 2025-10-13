namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record ServiceLinkWaterItemGroupedHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string? FromBankId { get; set; }
        public string? ToBankId { get; set; }

        public string ReportDateJalali { get; set; }
        public long TotalAmount { get; set; }
        public int RecordCount { get; set; }
        public string? Title { get; set; }

        public int CustomerCount { get; set; }
        public float SumDomesticUnit { get; set; }
        public float SumCommercialUnit { get; set; }
        public float SumOtherUnit { get; set; }
        public float TotalUnit { get; set; }
    }
}