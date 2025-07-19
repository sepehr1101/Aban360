namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands
{
    public record ServerReportsUpdateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public Guid CompletionId { get; set; }
        public string CompletionDateJalali { get; set; }
        public string InsertDateJalali { get; set; }
        public string ErrorDateJalali { get; set; }
        public bool IsInformed { get; set; }
    }
}
