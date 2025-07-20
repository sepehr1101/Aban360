namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands
{
    public record ServerReportsCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReportName { get; set; }
        public string ConnectionId { get; set; }
        public ServerReportsCreateDto(Guid _Id,Guid _UserId,string _ReportName,string _ConnectionId)
        {
            Id = _Id;
            UserId = _UserId;
            ReportName = _ReportName;   
            ConnectionId = _ConnectionId;
        }
    }
}
