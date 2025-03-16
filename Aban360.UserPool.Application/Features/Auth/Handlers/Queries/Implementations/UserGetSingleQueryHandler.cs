using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Factories;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using System.Collections.Generic;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserGetSingleQueryHandler : IUserGetSingleQueryHandler
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IEndpointQueryService _endpointQueryService;
        private readonly ILocationTreeAdHoc _locationTreeAdHoc;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IMapper _mapper;
        public UserGetSingleQueryHandler(
            IUserQueryService userQueryService,
            IEndpointQueryService endpointQueryService,
            ILocationTreeAdHoc locationTreeAdHoc,
            IRoleQueryService roleQueryService,
            IMapper mapper)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));

            _locationTreeAdHoc = locationTreeAdHoc;
            _locationTreeAdHoc.NotNull(nameof(locationTreeAdHoc));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }
        public async Task<UserDisplayDto> Handle(Guid userId, CancellationToken cancellationToken)
        {
            User user = await _userQueryService.GetIncludeUserAndClaims(userId);
            UserDisplayDto userDisplayDto = new();
            if (user.UserClaims.Any())
            {
                userDisplayDto.AccessTree = await CreateAccessTree(user.UserClaims);
                userDisplayDto.LocationTree = await CreateLocationTree(user.UserClaims, cancellationToken);
            }
            userDisplayDto.RoleInfo = await CreateRoleInfo(user.UserRoles);
            userDisplayDto.UserInfo = _mapper.Map<UserQueryDto>(user);
            return userDisplayDto;
        }
        private async Task<UserRoleInfo> CreateRoleInfo(ICollection<UserRole> userRoles)
        {
            if (userRoles == null)
            {
                return new UserRoleInfo();
            }
            ICollection<Role> roles = await _roleQueryService.Get();
            IEnumerable<UserRoleQueryDto> query = from role in roles
                        join userRole in userRoles
                        on role.Id equals userRole.RoleId into roleGroup
                        from rg in roleGroup.DefaultIfEmpty()
                        select new UserRoleQueryDto()
                        {
                            Id = role.Id,
                            Title = role.Title,
                            IsSelected = rg != null
                        };
            List<UserRoleQueryDto> userRoleQueryDtos = query.ToList();
            return new UserRoleInfo(userRoleQueryDtos);
        }
        private async Task<LocationTree> CreateLocationTree(ICollection<UserClaim> userClaims, CancellationToken cancellationToken)
        {
            List<int> zoneIds =
                userClaims
                .Where(userClaim => userClaim.ClaimTypeId == ClaimType.ZoneId)
                .Select(userClaim => int.Parse(userClaim.ClaimValue))
            .ToList();
            LocationTree locationTree = await _locationTreeAdHoc.Handle(zoneIds, cancellationToken);
            return locationTree;
        }
        private async Task<AccessTreeValueKeyDto> CreateAccessTree(ICollection<UserClaim> userClaims)
        {
            List<string> authValues = userClaims.Where(userClaim => userClaim.ClaimTypeId == ClaimType.Endpoint)
                    .Select(userClaim => userClaim.ClaimValue)
                    .ToList();
            ICollection<Endpoint> endpoints = await _endpointQueryService.GetIncludeAll();
            List<int> toBeSelectedEndpointIds = endpoints
                .Where(endpoint => authValues.Contains(endpoint.AuthValue))
                .Select(endpoint => endpoint.Id)
                .ToList();
            AccessTreeValueKeyDto accessTree = endpoints.CreateAccessTree(toBeSelectedEndpointIds);
            return accessTree;
        }
    }
}
