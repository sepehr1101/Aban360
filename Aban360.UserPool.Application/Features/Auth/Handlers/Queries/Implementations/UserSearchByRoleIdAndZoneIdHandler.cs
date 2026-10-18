using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserSearchByRoleIdAndZoneIdHandler : IUserSearchByRoleIdAndZoneIdHandler
    {
        private readonly IUserRoleQueryService _userRoleQueryService;
        public UserSearchByRoleIdAndZoneIdHandler(IUserRoleQueryService userRoleQueryService)
        {
            _userRoleQueryService = userRoleQueryService;
            _userRoleQueryService.NotNull(nameof(userRoleQueryService));
        }

        public async Task<IEnumerable<UserQueryDto>> Handle(int roleId, int zoneId, CancellationToken cancellationToken)
        {
            return await _userRoleQueryService.GetByDefaultZone(roleId, zoneId);
        }
    }
}
