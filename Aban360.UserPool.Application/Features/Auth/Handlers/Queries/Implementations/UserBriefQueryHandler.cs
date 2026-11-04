using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserBriefQueryHandler : IUserBriefQueryHandler
    {
        private readonly IUserQueryService _userQueryService;
        public UserBriefQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task<ICollection<UserBriefQueryDto>> Handle(CancellationToken cancellationToken)
        {
            return await _userQueryService.GetBrief(cancellationToken);
        }
    }
}
