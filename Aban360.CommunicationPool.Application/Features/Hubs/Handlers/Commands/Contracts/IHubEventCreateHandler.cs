using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts
{
    public interface IHubEventCreateHandler
    {
        Task Handle(HubEventCreateDto input, CancellationToken cancellationToken);
    }
}
