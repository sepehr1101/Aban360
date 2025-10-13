namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record UnreadSummaryHeaderOutputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public int FromPeriodCount { get; set; }
        public int ToPeriodCount { get; set; }

        public string  ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public string? Title { get; set; }

        public int SumClosed { get; set; }
        public int SumBarrier { get; set; }
        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int CustomerCount { get; set; }

        public int? Count0 { get; set; }
        public int? Count1 { get; set; }
        public int? Count2 { get; set; }
        public int? CountMore { get; set; }
    }
}
