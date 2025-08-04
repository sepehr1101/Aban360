namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands
{
    public record ServerReportsUpdateDto
    {
        public Guid Id { get; set; }
        public string ReportPath { get; set; }
        public DateTime CompletionDateTime { get; set; }
        public DateTime? ErrorDateTime { get; set; }
        public bool IsInformed { get; set; }
        //todo : remove nullable all prop
        public ServerReportsUpdateDto(Guid id,string reportPath, DateTime completionDateTime, bool isInformed)
        {
            Id = id;
            ReportPath = reportPath;
            CompletionDateTime = completionDateTime;
            IsInformed = isInformed;    
        }
    }
}
