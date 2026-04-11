using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class AssessmentGetByZoneIdHandler : IAssessmentGetByZoneIdHandler
    {
        private readonly IUserQueryService _userQueryService;
        public AssessmentGetByZoneIdHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task<IEnumerable<UserQueryDto>> Handle(int zoneId, CancellationToken cancellationToken)
        {
            UserSearchByRoleTitleAndZoneIdDto userSearch = new(zoneId, ClaimType.ZoneId, BaseRoles.Evaluator);
            IEnumerable<UserQueryDto> result = await _userQueryService.Get(userSearch);
            return result;
        }
    }
}
