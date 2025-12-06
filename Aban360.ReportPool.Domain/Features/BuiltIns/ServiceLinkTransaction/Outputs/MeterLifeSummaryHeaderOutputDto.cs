namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterLifeSummaryHeaderOutputDto
    {
        public int FromLifeInDay { get; set; }
        public int ToLifeInDay { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string Title { get; set; }

    }
}
