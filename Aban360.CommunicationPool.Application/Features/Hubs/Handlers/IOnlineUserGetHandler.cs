using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Queries;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Handlers
{
    public interface IOnlineUserGetHandler
    {
        Task<IEnumerable<OnlineUserGetDto>> Handle( CancellationToken cancellationToken);
    }
}
