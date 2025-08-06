using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Queries;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Queries.Contracts;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Mappers
{
    internal sealed class OnlineUserGetHandler : IOnlineUserGetHandler
    {
        private readonly IOnlineUsersGetQueryService _OnlineUserGetService;
        public OnlineUserGetHandler(IOnlineUsersGetQueryService OnlineUserGetService)
        {
            _OnlineUserGetService = OnlineUserGetService;
            _OnlineUserGetService.NotNull(nameof(OnlineUserGetService));
        }

        public async Task<IEnumerable<OnlineUserGetDto>> Handle(CancellationToken cancellationToken)
        {
            var result = await _OnlineUserGetService.Get();
            return result;
        }
    }
}
