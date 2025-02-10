using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    public class UserCreateHandler : IUserCreateHandler
    {
        private readonly IUserCommandService _userCommandService;
        private readonly IUserClaimCommandService _userClaimCommandService;
        private readonly IUserRoleCommandService _userRoleCommandService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserCreateHandler(
            IUserCommandService userCommandService,
            IUserClaimCommandService userClaimCommandService,
            IUserRoleCommandService userRoleCommandService,
            IMapper mapper,
            IHttpContextAccessor contextAccessor)
        {
            _userCommandService = userCommandService;
            _userCommandService.NotNull(nameof(userCommandService));

            _userClaimCommandService = userClaimCommandService;
            _userClaimCommandService.NotNull(nameof(userClaimCommandService));

            _userRoleCommandService = userRoleCommandService;
            _userRoleCommandService.NotNull(nameof(userRoleCommandService));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }
        public async Task Handle(UserCreateDto userCreateDto, CancellationToken cancellationToken)
        {
            var logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
            var logInfoString = JsonOperation.Marshal(logInfo);
            Guid operationGroupId = Guid.NewGuid();
            var userClaims = CreateUserClaims(userCreateDto.ClaimItems, logInfoString, operationGroupId, operationGroupId);
            var userRoles = CreateUserRoles(userCreateDto.RoleIds, logInfoString, operationGroupId, operationGroupId);

            //todo : hashPassword
            var user = _mapper.Map<User>(userCreateDto);
            user.Id = operationGroupId;
            user.InsertLogInfo = logInfoString;
            //user.ValidFrom = DateTime.Now;
            await _userCommandService.Add(user);
            await _userClaimCommandService.Add(userClaims);
            await _userRoleCommandService.Add(userRoles);
        }
        private ICollection<UserClaim> CreateUserClaims(ICollection<ClaimDto> claimItems, string logInfo, Guid operationGroupId, Guid userId)
        {
            if (claimItems.Any())
            {
                var userClaims = claimItems
                    .Select(claimDto => CreateUserClaim(claimDto, logInfo, operationGroupId, userId))
                    .ToList();
                return userClaims;
            }
            return default;
        }
        private UserClaim CreateUserClaim(ClaimDto claimDto, string logInfo, Guid operationGroupId, Guid userId)
        {
            return new UserClaim()
            {               
                ClaimTypeId = claimDto.ClaimTypeId,
                ClaimValue = claimDto.ClaimValue,
                InsertGroupId = operationGroupId,
                InsertLogInfo = logInfo,
                //ValidFrom = DateTime.Now,
                ValidTo = null,
                UserId = userId
            };
        }

        private ICollection<UserRole> CreateUserRoles(ICollection<int> roleIds, string logInfoString, Guid operationGroupId, Guid userId)
        {
            return roleIds
                .Select(roleId => CreateUserRole(roleId, logInfoString, operationGroupId, userId))
                .ToList();
        }
        private UserRole CreateUserRole(int roleId, string logInfoString, Guid operationGroupId, Guid userId)
        {
            return new UserRole()
            {
                RoleId = roleId,
                InsertGroupId = operationGroupId,
                InsertLogInfo = logInfoString,
                //ValidFrom = DateTime.Now,
                ValidTo = null,
                UserId = userId
            };
        }
    }
}