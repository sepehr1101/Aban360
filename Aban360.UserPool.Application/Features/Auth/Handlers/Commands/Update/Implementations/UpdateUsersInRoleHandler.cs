using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Common.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class UpdateUsersInRoleHandler: UserBaseCreateOrUpdateService
    {
        private readonly IUserClaimQueryService _userClaimQueryService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IUserClaimCommandService _userClaimCommandService;
        private readonly IEndpointQueryService _endpointQueryService;

        public UpdateUsersInRoleHandler(
            IUserClaimQueryService userClaimQueryService,
            IHttpContextAccessor contextAccessor,
            IRoleQueryService roleQueryService,
            IUserClaimCommandService userClaimCommandService,
            IEndpointQueryService endpointQueryService)
        {
            _userClaimQueryService = userClaimQueryService;
            _userClaimQueryService.NotNull(nameof(userClaimQueryService));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));

            _userClaimCommandService = userClaimCommandService;
            _userClaimCommandService.NotNull(nameof(userClaimCommandService));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }

        public async Task Handle(RoleUpdateDto roleUpdateDto, CancellationToken cancellationToken)
        {
            roleUpdateDto.NotNull(nameof(roleUpdateDto));
            Role role = await _roleQueryService.Get(roleUpdateDto.Id);
            if (role.DefaultClaims == null)
            {
                return;
            }
            int[] roleEndpiontsId = JsonOperation.Unmarshal<int[]>(role.DefaultClaims);
            if(!roleEndpiontsId.Any())
            {
                return;
            }

            ICollection<UserClaim> userClaims = await _userClaimQueryService.Get(roleUpdateDto.Id, ClaimType.Endpoint);
            LogInfo logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
            string logInfoString = JsonOperation.Marshal(logInfo);
            Guid operationGroupId = Guid.NewGuid();
            _userClaimCommandService.Remove(userClaims, logInfoString);
            ICollection<Guid> userIds= userClaims.Select(user=>user.UserId).ToList();

            List<string> roleEndpointsValue = await _endpointQueryService.GetAuthValue(roleEndpiontsId);
            List<UserClaim> endpointClaims= new List<UserClaim>();
            foreach (var userId in userIds)
            {
                ICollection<UserClaim> userEndpointClaims = CreateUserClaim(roleEndpointsValue, ClaimType.Endpoint, logInfoString, operationGroupId, userId);
                if(userEndpointClaims.Any())
                {
                    endpointClaims.AddRange(userEndpointClaims);
                }
            }
            if (endpointClaims.Any())
            {
                await _userClaimCommandService.Add(endpointClaims);
            }
        }
    }
}
