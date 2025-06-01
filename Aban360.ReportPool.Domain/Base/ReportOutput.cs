namespace Aban360.ReportPool.Domain.Base
{
    public record ReportOutput<TReportHeader,TReportData>
    {
        public string Title { get; } = default!;
        public TReportHeader ReportHeader { get; }= default!;
        public IEnumerable<TReportData> ReportData { get; } = default!;
        public ReportOutput(string title, TReportHeader header, IEnumerable<TReportData> data)
        {
            Title = title;
            ReportData = data;
            ReportHeader = header;
        }
    }
}
