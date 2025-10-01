namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionMeterHeaderOutputDto
    {
        public string FromDateJalali{ get; set; }
        public string ToDateJalali{ get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public string ReportDateJalali { get; set; } = default!;
        public int RecordCount { get; set; }
        public long TotalPayable { get; set; }
        public int CustomerCount { get; set; }
    }
}
