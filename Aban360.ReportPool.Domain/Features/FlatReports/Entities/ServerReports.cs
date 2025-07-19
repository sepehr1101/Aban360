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
        public Guid CompletionId { get; set; }
        public string CompletionDateJalali { get; set; }
        public string InsertDateJalali { get; set; }
        public string ErrorDateJalali { get; set; }
        public bool IsInformed { get; set; }
    }
}
