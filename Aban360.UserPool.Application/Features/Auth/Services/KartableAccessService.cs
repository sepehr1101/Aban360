using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Common.Base;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Services;
public interface IKartableAccessService
{
    Task UpdateAsync(KartableAccessUpdateDto inputDto, CancellationToken cancellationToken);
    Task<IEnumerable<SelectionDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}

internal sealed class KartableAccessService : UserBaseCreateOrUpdateService, IKartableAccessService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserClaimCommandService _userClaimCommandService;
    private readonly IUserClaimQueryService _userClaimQueryService;
    private readonly IUserQueryService _userQueryService;
    private readonly IRequestStatusQueryService _requestStatusQueryService;
    public KartableAccessService(IHttpContextAccessor contextAccessor, IUserClaimCommandService userClaimCommandService, IUserClaimQueryService userClaimQueryService, IUserQueryService userQueryService, IRequestStatusQueryService requestStatusQueryService)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _userClaimCommandService = userClaimCommandService;
        _userClaimCommandService.NotNull(nameof(userClaimCommandService));
        _userClaimQueryService = userClaimQueryService;
        _userClaimQueryService.NotNull(nameof(userClaimQueryService));
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
        _requestStatusQueryService = requestStatusQueryService;
        _requestStatusQueryService.NotNull(nameof(requestStatusQueryService));
    }

    public async Task UpdateAsync(KartableAccessUpdateDto inputDto, CancellationToken cancellationToken)
    {
        LogInfo logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
        string logInfoString = JsonOperation.Marshal(logInfo);
        Guid insertGroupId = Guid.NewGuid();
        User user = await _userQueryService.Get(inputDto.UserId);
        ICollection<UserClaim> previousUserClaims = await _userClaimQueryService.Get(inputDto.UserId, ClaimType.RequestKartable);
        _userClaimCommandService.Remove(previousUserClaims, logInfoString);
        ICollection<UserClaim> newUserClaims = CreateUserClaim(inputDto.KartableIds.Select(r => r.ToString()).ToList(), ClaimType.RequestKartable, logInfoString, insertGroupId, inputDto.UserId);
        await _userClaimCommandService.Add(newUserClaims);
    }

    public async Task<IEnumerable<SelectionDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        IEnumerable<SelectionDto> requestStatuses = await _requestStatusQueryService.GetIsKartable();
        ICollection<UserClaim> userAccesses = await _userClaimQueryService.GetValid(userId, ClaimType.RequestKartable);
        foreach (var item in requestStatuses)
        {
            item.IsSelected = userAccesses.Where(u => int.Parse(u.ClaimValue) == item.Id).Any();
        //userAccesses.Select(u=> int.Parse(u.ClaimValue)).Contains(item.Id);
        }

        return requestStatuses;
    }
}
