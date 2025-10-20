namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitSummaryHeaderOutputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }

        public string ReportDateJalali { get; set; } = default!;
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int SumEmptyUnit { get; set; }
        public int Field1 { get; set; }
        public int Field2 { get; set; }
        public int Field3 { get; set; }
        public int Field4 { get; set; }
        public int Field5 { get; set; }
        public int Field6 { get; set; }
        public int MoreThanField6 { get; set; }
    }
}
