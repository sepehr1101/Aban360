using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.UserPool.Application.Common.Base;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class UserUpdateHandler : UserBaseCreateOrUpdateService, IUserUpdateHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IUserCommandService _userCommandService;
        private readonly IUserQueryService _userQueryService;
        private readonly IUserClaimQueryService _userClaimsQueryService;
        private readonly IUserRoleQueryService _userRoleQueryService;
        private readonly IUserClaimCommandService _userClaimCommandService;
        private readonly IUserRoleCommandService _userRoleCommandService;
        private readonly IZoneCountQueryAddhoc _zoneCountQueryAddhoc;
        private readonly IEndpointQueryService _endpointQueryService;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserUpdateHandler(
            IUnitOfWork uow,
            IMapper mapper,
            IUserQueryService userQueryService,
            IUserClaimQueryService userClaimsQueryService,
            IUserRoleQueryService userRoleQueryService,
            IUserCommandService userCommandService,
            IUserClaimCommandService userClaimCommandService,
            IUserRoleCommandService userRoleCommandService,
            IZoneCountQueryAddhoc zoneCountQueryAddhoc,
            IEndpointQueryService endpointQueryService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _userCommandService = userCommandService;
            _userCommandService.NotNull(nameof(userCommandService));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));

            _userClaimsQueryService = userClaimsQueryService;
            _userClaimsQueryService.NotNull(nameof(userClaimsQueryService));

            _userRoleQueryService = userRoleQueryService;
            _userRoleQueryService.NotNull(nameof(userRoleQueryService));

            _userClaimCommandService = userClaimCommandService;
            _userClaimCommandService.NotNull(nameof(_userClaimCommandService));

            _userRoleCommandService = userRoleCommandService;
            _userRoleCommandService.NotNull(nameof(_userRoleCommandService));

            _zoneCountQueryAddhoc = zoneCountQueryAddhoc;
            _zoneCountQueryAddhoc.NotNull(nameof(zoneCountQueryAddhoc));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(_endpointQueryService));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }
        public async Task Handle(UserUpdateDto userUpdateDto, CancellationToken cancellationToken)
        {
            var logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
            var logInfoString = JsonOperation.Marshal(logInfo);
            Guid operationGroupId = Guid.NewGuid();
            var userInDb = await _userQueryService.Get(userUpdateDto.Id);
            var user = _mapper.Map<User>(userInDb);

            ICollection<UserClaim> previousClaims = await _userClaimsQueryService.Get(userUpdateDto.Id);
            ICollection<UserRole> previousRoles = await _userRoleQueryService.Get(userUpdateDto.Id);

            _userClaimCommandService.Remove(previousClaims, logInfoString);
            _userRoleCommandService.Remove(previousRoles, logInfoString);

            var zoneCount = await _zoneCountQueryAddhoc.GetCount(userUpdateDto.SelectedZoneIds, cancellationToken);
            var endpointValue = await _endpointQueryService.GetAuthValue(userUpdateDto.SelectedEndpointIds);
            Validate(zoneCount, userUpdateDto.SelectedZoneIds.Count(), endpointValue.Count(), userUpdateDto.SelectedEndpointIds.Count());

            var zones = CreateUserClaim(userUpdateDto.SelectedZoneIds.Select(x => x.ToString()).ToList(), ClaimType.ZoneId, logInfoString, operationGroupId, userUpdateDto.Id);
            var endpionts = CreateUserClaim(endpointValue, ClaimType.Endpoint, logInfoString, operationGroupId, userUpdateDto.Id);
            var userCliams = zones.Union(endpionts).ToList();

            var userRoles = CreateUserRoles(userUpdateDto.SelectedRoleIds, logInfoString, operationGroupId, userUpdateDto.Id);

            await _userClaimCommandService.Add(userCliams);
            await _userRoleCommandService.Add(userRoles);
        }
    }
}
