namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MeterChangeRandomHeaderOutputDto
    {
        public string ChangeDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int ChangeCount { get; set; }

        public string ReportDateJalali { get; set; } = default!;
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }
    }
}
