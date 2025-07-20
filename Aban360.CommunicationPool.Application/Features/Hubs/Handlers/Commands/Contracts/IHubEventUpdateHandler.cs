using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts
{
    public interface IHubEventUpdateHandler
    {
        Task Handle(HubEventUpdateDto input, CancellationToken cancellationToken);
    }
}
