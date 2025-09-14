namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record NonPermanentBranchHeaderOutputDto
    {
        public string? FromReadingNumber{ get; set; }
        public string? ToReadingNumber { get; set; }

        public string Title { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }

    }
}
