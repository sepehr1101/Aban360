namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitByBillIdSummaryHeaderOutputDto
    {
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int EmptyUnit { get; set; }
    }
}
