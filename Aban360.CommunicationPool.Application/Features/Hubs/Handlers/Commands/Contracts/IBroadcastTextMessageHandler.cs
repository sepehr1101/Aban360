using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts
{
    public interface IBroadcastTextMessageHandler
    {
        NotifyTextMessageOutput Handle(NotifyTextMessageInput input, CancellationToken cancellationToken);
    }
}