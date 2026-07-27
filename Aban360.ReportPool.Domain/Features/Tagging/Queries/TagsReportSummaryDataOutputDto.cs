namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record TagsReportSummaryDataOutputDto
    {
        public string TagsTitle { get; set; }
        public string ItemTitle { get; set; }
        public int CustomerCount { get; set; }
    }
}
