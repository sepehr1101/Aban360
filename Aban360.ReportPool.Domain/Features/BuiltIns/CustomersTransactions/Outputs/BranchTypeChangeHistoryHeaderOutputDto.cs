namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record BranchTypeChangeHistoryHeaderOutputDto
    {
        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string ReportDateJalali { get; set; } = default!;
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

        public int SumDomesticCount { get; set; }
        public int SumCommercialCount { get; set; }
        public int SumOtherCount { get; set; }
        public int TotalUnit { get; set; }

    }
}
