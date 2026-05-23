using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Common.Base;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class KartableAccessUpdateHandler : UserBaseCreateOrUpdateService, IKartableAccessUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserClaimCommandService _userClaimCommandService;
        private readonly IUserClaimQueryService _userClaimQueryService;
        private readonly IUserQueryService _userQueryService;
        public KartableAccessUpdateHandler(
            IHttpContextAccessor contextAccessor,
            IUserClaimCommandService userClaimCommandService,
            IUserClaimQueryService userClaimQueryService,
            IUserQueryService userQueryService)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _userClaimCommandService = userClaimCommandService;
            _userClaimCommandService.NotNull(nameof(userClaimCommandService));

            _userClaimQueryService = userClaimQueryService;
            _userClaimQueryService.NotNull(nameof(userClaimQueryService));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(KartableAccessUpdateDto inputDto, CancellationToken cancellationToken)
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
    }
}
