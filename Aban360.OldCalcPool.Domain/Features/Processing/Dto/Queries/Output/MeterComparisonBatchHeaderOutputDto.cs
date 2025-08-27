namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterComparisonBatchHeaderOutputDto
    {
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public string ZoneTitle { get; set; }

        public double SumPreviousAmount { get; set; }
        public double SumCurrentAmount { get; set; }

    }

}
