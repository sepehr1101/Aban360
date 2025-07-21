using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ReportPool.Domain.Features.FlatReports.Entities
{
    [Table(nameof(ServerReports))]
    public class ServerReports
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public string ConnectionId { get; set; }
        public DateTime CompletionDateTime { get; set; }
        public DateTime InsertDateTime { get; set; }
        public DateTime ErrorDateTime { get; set; }
        public bool IsInformed { get; set; }
    }
}
