namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries
{
    public record ServerReportsGetByIdDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public string ConnectionId { get; set; }
        public string CompletionDateTimeJalali { get; set; }
        public string InsertDateTimeJalali { get; set; }
        public string ErrorDateTimeJalali { get; set; }
        public bool IsInformed { get; set; }
        public string HeaderType { get; set; } = default!;
        public string DataType { get; set; } = default!;
        public string? ReportInputType { get; set; }
        public string? ReportInputJson { get; set; }
        public string HandlerKey { get; set; } = default!;
    }
}
