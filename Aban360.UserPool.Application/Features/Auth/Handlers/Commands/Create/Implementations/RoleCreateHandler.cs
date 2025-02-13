using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using AutoMapper;
using System.Text.Json;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    public class RoleCreateHandler : IRoleCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleCommandService _roleCommandService;
        public RoleCreateHandler(
            IMapper mapper,
            IRoleCommandService roleCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleCommandService = roleCommandService;
            _roleCommandService.NotNull(nameof(roleCommandService));
        }

        public async Task Handle(RoleCreateDto createDto, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<Role>(createDto);
            if (createDto.SelectedEndpointIds is not null && createDto.SelectedEndpointIds.Any())
            {
                role.DefaultClaims = JsonOperation.Marshal(createDto.SelectedEndpointIds);
            }
            await _roleCommandService.Add(role);
        }
    }
}
