using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Queries;

namespace Aban360.CommunicationPool.Persistence.Features.Hubs.Queries.Contracts
{
    public interface IOnlineUsersGetQueryService
    {
        Task<IEnumerable<OnlineUserGetDto>> Get();
    }
}
