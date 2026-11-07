using Aban360.Api.Hubs.Contracts;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Microsoft.AspNetCore.SignalR;

namespace Aban360.Api.Hubs.Implementations
{
    public sealed class ReportCompletionNotifier : IReportCompletionNotifier
    {
        private readonly IHubContext<NotifyHub, INotifyHub> _hub;
        private readonly ILogger<ReportCompletionNotifier> _logger;

        public ReportCompletionNotifier(IHubContext<NotifyHub, INotifyHub> hub, ILogger<ReportCompletionNotifier> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task Notify(Guid userId, string reportTitle, Guid reportId)
        {
            try
            {
                await _hub.Clients.User(userId.ToString())
                    .InformReportCompletion(new ReportCompletionNotification(reportTitle, reportId));
            }
            catch (Exception exception)
            {
                // The report is already saved; retrying its generation would duplicate completed work.
                _logger.LogError(exception, "Could not send completion notification for report {ReportId} to user {UserId}.", reportId, userId);
            }
        }
    }
}
