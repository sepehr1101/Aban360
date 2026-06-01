using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
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
        private readonly ICommonZoneService _commonZoneService;
        public AssessmentGetByZoneIdHandler(
            IUserQueryService userQueryService,
            ICommonZoneService commonZoneService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<IEnumerable<StringDictionary>> Handle(int zoneId, CancellationToken cancellationToken)
        {
            UserSearchByRoleTitleAndZoneIdDto userSearch = new(zoneId, ClaimType.ZoneId, BaseRoles.Evaluator);
            IEnumerable<StringDictionary> result = await _userQueryService.GetDictionary(userSearch);
            return result;
        }
        public async Task<IEnumerable<StringDictionary>> Handle(IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> allowedZoneIds= await _commonZoneService.GetMyZoneIds(appUser);
            UserSearchByRoleTitleAndZoneIdsDto userSearch = new(allowedZoneIds, ClaimType.ZoneId, BaseRoles.Evaluator);
            IEnumerable<StringDictionary> result = await _userQueryService.GetDictionary(userSearch);
            return result;
        }
    }
}
