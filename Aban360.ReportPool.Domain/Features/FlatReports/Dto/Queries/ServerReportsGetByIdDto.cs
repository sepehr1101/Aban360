namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries
{
    public record ServerReportsGetByIdDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public string ConnectionId { get; set; }
        public string CompletionDateJalali { get; set; }
        public string InsertDateJalali { get; set; }
        public string ErrorDateJalali { get; set; }
        public bool IsInformed { get; set; }
    }
}
