using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Factories;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public class RoleGetSingleHandler : IRoleGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IEndpointQueryService _endpointQueryService;
        public RoleGetSingleHandler(
            IMapper mapper,
            IRoleQueryService roleQueryService,
            IEndpointQueryService endpointQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }

        public async Task<RoleDisplayDto> Handle(int id, CancellationToken cancellationToken)
        {
            var role = await _roleQueryService.Get(id);
            var dto= _mapper.Map<RoleGetDto>(role);
            RoleDisplayDto roleDisplayDto = new();
            roleDisplayDto.RoleInfo = dto;
            if (role.DefaultClaims is not null && role.DefaultClaims.Any())
            {
                var endpointIds = JsonOperation.Unmarshal<int[]>(role.DefaultClaims);
                roleDisplayDto.AccessTree = await CreateAccessTree(endpointIds);
            }
            else
            {
                roleDisplayDto.AccessTree = await CreateAccessTree(new List<int>());
            }
            return roleDisplayDto;
        }
        private async Task<AccessTreeValueKeyDto> CreateAccessTree(ICollection<int> endpointIds)
        {
            var endpoints = await _endpointQueryService.GetIncludeAll();
            var accessTree = endpoints.CreateAccessTree(endpointIds);
            return accessTree;
        }
    }
}
