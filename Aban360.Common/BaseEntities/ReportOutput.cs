namespace Aban360.Common.BaseEntities
{
    public record ReportOutput<TReportHeader, TReportData> //where TReportHeader : IReportHeaderBase
    {
        public string Title { get; } = default!;
        public TReportHeader ReportHeader { get; } = default!;
        public IEnumerable<TReportData> ReportData { get; } = default!;
        public ReportOutput(string title, TReportHeader header, IEnumerable<TReportData> data)
        {
            Title = title;
            ReportData = data;
            ReportHeader = header;
        }
    }
}