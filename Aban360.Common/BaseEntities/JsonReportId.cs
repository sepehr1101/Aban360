namespace Aban360.Common.BaseEntities
{
    public record JsonReportId
    {
        public Guid JsonFileId { get; }
        public int ReportCode { get; set; }
        public JsonReportId(Guid id, int reportCode)
        {
            JsonFileId = id;
            ReportCode = reportCode;
        }
    }
}
