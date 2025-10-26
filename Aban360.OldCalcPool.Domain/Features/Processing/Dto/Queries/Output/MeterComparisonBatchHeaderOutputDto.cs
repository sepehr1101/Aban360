namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterComparisonBatchHeaderOutputDto
    {
        public string ReportDateJalali { get; set; } = default!;
        public int RecordCount { get; set; }

        public string ZoneTitle { get; set; } = default!;

        public double SumPreviousAmount { get; set; }
        public double SumCurrentAmount { get; set; }

        public int ValidCount { get; set; }
        public int InvalidCount { get; set; }
        public double DifferenceSum { get; set; }
    }
}
