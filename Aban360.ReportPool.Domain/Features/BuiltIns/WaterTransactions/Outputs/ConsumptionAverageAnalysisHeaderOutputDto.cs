namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ConsumptionAverageAnalysisHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string? Title { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public IEnumerable<ConsumptionAverageAnalysisHeaderValueDto> Values { get; set; }

    }
}
