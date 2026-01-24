namespace Aban360.Common.BaseEntities
{
    public record FlatReportOutput<TReportHeader, TReportData> 
    {
        public string Title { get; } = default!;
        public TReportHeader ReportHeader { get; } = default!;
        public TReportData ReportData { get; } = default!;
        public FlatReportOutput(string title, TReportHeader header, TReportData data)
        {
            Title = title;
            ReportData = data;
            ReportHeader = header;
        }
    }


}