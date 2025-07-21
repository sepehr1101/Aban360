namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries
{
    public record ServerReportsGetAllDto
    {
        public Guid Id { get; set; }
        public string ReportName { get; set; }
        public string CompletionDateTimeJalali { get; set; }
        public string InsertDateTimeJalali { get; set; }
        public string ErrorDateTimeJalali { get; set; }
        public bool IsInformed { get; set; }
        public bool IsCompleted { get; set; }
    }

}