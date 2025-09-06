namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitByBillIdSummaryHeaderOutputDto
    {
        public int FromEmptyUnit { get; set; }
        public int ToEmptyUnit { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int EmptyUnit { get; set; }
    }
}
