using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Services;
public interface IAssessmentByZoneService
{
    Task<IEnumerable<StringDictionary>> AssessmentGetByZoneIdAsync(int zoneId, CancellationToken cancellationToken);
    Task<IEnumerable<StringDictionary>> AssessmentGetByZoneIdAsync(IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class AssessmentByZoneService : IAssessmentByZoneService
{
    public AssessmentByZoneService(IUserQueryService userQueryService, ICommonZoneService commonZoneService)
    {
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
    }

    private readonly IUserQueryService _userQueryService;
    private readonly ICommonZoneService _commonZoneService;
    public async Task<IEnumerable<StringDictionary>> AssessmentGetByZoneIdAsync(int zoneId, CancellationToken cancellationToken)
    {
        UserSearchByRoleTitleAndZoneIdDto userSearch = new(zoneId, ClaimType.ZoneId, BaseRoles.Evaluator);
        IEnumerable<StringDictionary> result = await _userQueryService.GetDictionary(userSearch);
        return result;
    }

    public async Task<IEnumerable<StringDictionary>> AssessmentGetByZoneIdAsync(IAppUser appUser, CancellationToken cancellationToken)
    {
        IEnumerable<int> allowedZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
        UserSearchByRoleTitleAndZoneIdsDto userSearch = new(allowedZoneIds, ClaimType.ZoneId, BaseRoles.Evaluator);
        IEnumerable<StringDictionary> result = await _userQueryService.GetDictionary(userSearch);
        return result;
    }
}
