using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Common.Base;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class UpdateUsersInRoleHandler : UserBaseCreateOrUpdateService, IUpdateUsersInRoleHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserClaimQueryService _userClaimQueryService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IUserClaimCommandService _userClaimCommandService;
        private readonly IEndpointQueryService _endpointQueryService;

        public UpdateUsersInRoleHandler(
            IUnitOfWork unitOfWork,
            IUserClaimQueryService userClaimQueryService,
            IHttpContextAccessor contextAccessor,
            IRoleQueryService roleQueryService,
            IUserClaimCommandService userClaimCommandService,
            IEndpointQueryService endpointQueryService)
        {
            _unitOfWork=unitOfWork;
            _unitOfWork.NotNull(nameof(unitOfWork));

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

        public async Task Handle(int roleId, CancellationToken cancellationToken)
        {            
            Role role = await _roleQueryService.Get(roleId);
            if (role.DefaultClaims == null || string.IsNullOrWhiteSpace(role.DefaultClaims))
            {
                return;
            }
            int[] roleEndpiontsId = JsonOperation.Unmarshal<int[]>(role.DefaultClaims);
            if (!roleEndpiontsId.Any())
            {
                return;
            }

            var (userIds, userClaims) = await _userClaimQueryService.Get(roleId, ClaimType.Endpoint);
            if(userIds==null || !userIds.Any())
            {
                return;
            }
            Guid operationGroupId = Guid.NewGuid();    

            List<string> roleEndpointsValue = await _endpointQueryService.GetAuthValue(roleEndpiontsId);
            List<UserClaim> endpointClaims = new List<UserClaim>();
            foreach (var userId in userIds)
            {
                ICollection<UserClaim> userEndpointClaims = CreateUserClaim(roleEndpointsValue, ClaimType.Endpoint, "", operationGroupId, userId);
                if (userEndpointClaims.Any())
                {
                    endpointClaims.AddRange(userEndpointClaims);
                }
            }
            if (endpointClaims.Any())
            {
                await _userClaimCommandService.Add(endpointClaims);
            }
            if (userClaims != null && userClaims.Any())
            {
                _userClaimCommandService.Remove(userClaims, "");
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
