using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Factories;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserQueryParamsOfCreate : IUserQueryParamsOfCreate
    {
        private readonly IEndpointQueryService _endpointQueryService;
        private readonly ILocationTreeAdHoc _locationTreeAdHoc;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IMapper _mapper;
        public UserQueryParamsOfCreate(
            IEndpointQueryService endpointQueryService,
            ILocationTreeAdHoc locationTreeAdHoc,
            IRoleQueryService roleQueryService,
            IMapper mapper)
        {
            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));

            _locationTreeAdHoc = locationTreeAdHoc;
            _locationTreeAdHoc.NotNull(nameof(locationTreeAdHoc));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }
        public async Task<UserParamsOfCreateDto> Handle(CancellationToken cancellationToken)
        {
            UserRoleInfo roleInfo = await CreateRoleInfo();
            LocationTree locationTree = await _locationTreeAdHoc.Handle(cancellationToken);
            AccessTreeValueKeyDto accessTree = await CreateAccessTree();
            UserParamsOfCreateDto userParamsOfCreateDto = new(roleInfo, locationTree, accessTree);
            return userParamsOfCreateDto;
        }
        private async Task<UserRoleInfo> CreateRoleInfo()
        {
            ICollection<Role> roles = await _roleQueryService.Get();
            ICollection<UserRoleQueryDto> roleQueryDtos = _mapper.Map<ICollection<UserRoleQueryDto>>(roles);
            return new UserRoleInfo(roleQueryDtos);
        }
        private async Task<AccessTreeValueKeyDto> CreateAccessTree()
        {
            ICollection<Endpoint> endpoints = await _endpointQueryService.GetIncludeAll();
            AccessTreeValueKeyDto accessTree = endpoints.CreateAccessTree();
            return accessTree;
        }
    }
}
