using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using System.Text.Json;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public class RoleGetSingleHandler : IRoleGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleQueryService _roleQueryService;
        public RoleGetSingleHandler(
            IMapper mapper,
            IRoleQueryService roleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));
        }

        public async Task<RoleGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var role = await _roleQueryService.Get(id);
            var dto= _mapper.Map<RoleGetDto>(role);
            if (role.DefaultClaims is not null && role.DefaultClaims.Any())
            {
                dto.SelectedEndpointIds = JsonOperation.Unmarshal<int[]>(role.DefaultClaims);

            }
            return dto;
        }
    }
}
