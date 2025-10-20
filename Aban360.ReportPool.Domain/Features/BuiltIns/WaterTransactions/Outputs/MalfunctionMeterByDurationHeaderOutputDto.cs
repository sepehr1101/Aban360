namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionMeterByDurationHeaderOutputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public int FromMalfunctionPeriodCount { get; set; }
        public int ToMalfunctionPeriodCount { get; set; }

        public string? Title { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }


        public int TotalUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumDomesticUnit { get; set; }
        public int SumOtherUnit { get; set; }
    }
}
