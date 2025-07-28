using Aban360.CommunicationPool.Domain.Constants;

namespace Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands
{
    public record ReportCompletionNotification
    {
        public string ReportTitle { get; set; } = default!;
        public Guid ReportId { get; set; }
        public string Message { get; set; } = default!;
        public ReportCompletionNotification(string reportTitle, Guid reportId)
        {
            ReportId = reportId;
            ReportTitle = reportTitle;
            Message = MessageLiterals.ReportCompleted;
        }
    }
}
