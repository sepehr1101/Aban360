using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;

namespace Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts
{
    public interface IHubEventCreateService
    {
        Task Create(HubEventCreateDto input);
    }
}
