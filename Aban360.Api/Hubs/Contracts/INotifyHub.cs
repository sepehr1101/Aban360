using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;

namespace Aban360.Api.Hubs.Contracts
{
    public interface INotifyHub
    {
        Task BroadcastMessage(NotifyTextMessageOutput notifyTextMessageOutput );
        Task InformReportCompletion(string message);
    }
}
