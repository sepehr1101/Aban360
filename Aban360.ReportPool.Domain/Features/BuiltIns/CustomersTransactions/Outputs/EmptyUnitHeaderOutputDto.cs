namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitHeaderOutputDto
    {
        public int FromEmptyUnit { get; set; }
        public int ToEmptyUnit { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string ReportDateJalali { get; set; } = default!;
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int SumEmptyUnit { get; set; }
    }
}
