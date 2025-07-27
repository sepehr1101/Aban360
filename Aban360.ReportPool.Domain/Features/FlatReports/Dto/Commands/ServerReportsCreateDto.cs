namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands
{
    public record ServerReportsCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReportName { get; set; } = default!;
        public string? ConnectionId { get; set; }
        public string HeaderType { get; set; }=default!;
        public string DataType { get; set; } = default!;
        public string? ReportInputType { get; set; }
        public string? ReportInputJson { get; set; }
        public string HandlerKey { get; set; } = default!;
    }
}