using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserSearchByRoleIdHandler : IUserSearchByRoleIdHandler
    {
        private readonly IUserRoleQueryService _userRoleQueryService;
        public UserSearchByRoleIdHandler(IUserRoleQueryService userRoleQueryService)
        {
            _userRoleQueryService = userRoleQueryService;
            _userRoleQueryService.NotNull(nameof(userRoleQueryService));
        }

        public async Task<IEnumerable<UserQueryDto>> Handle(int roleId, CancellationToken cancellationToken)
        {
            return await _userRoleQueryService.Get(roleId);
        }
    }
}