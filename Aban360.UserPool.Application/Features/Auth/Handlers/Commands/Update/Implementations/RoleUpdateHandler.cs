using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class RoleUpdateHandler : IRoleUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IEndpointQueryService _endpointQueryService;
        public RoleUpdateHandler(
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

        public async Task Handle(RoleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Role role = await _roleQueryService.Get(updateDto.Id);
            if (role == null)
            {
                throw new InvalidDataException();
            }

            List<string> endpointValue = await _endpointQueryService.GetAuthValue(updateDto.SelectedEndpointIds);

            if (updateDto.SelectedEndpointIds is not null && endpointValue.Count() == updateDto.SelectedEndpointIds.Count())
            {
                role.DefaultClaims = JsonOperation.Marshal(updateDto.SelectedEndpointIds);
            }
            _mapper.Map(updateDto, role);
        }
    }
}
