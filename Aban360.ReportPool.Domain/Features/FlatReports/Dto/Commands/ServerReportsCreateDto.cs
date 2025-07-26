namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands
{
    public record ServerReportsCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReportName { get; set; }
        public string ConnectionId { get; set; }
        public ServerReportsCreateDto(Guid id,Guid userId,string reportName,string connectionId)
        {
            Id = id;
            UserId = userId;
            ReportName = reportName;   
            ConnectionId = connectionId;
        }
    }
}
