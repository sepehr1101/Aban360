using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Implementations
{
    internal sealed class EndpointCreateHandler : IEndpointCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEndpointCommandService _endpointCommandService;
        public EndpointCreateHandler(
            IMapper mapper,
            IEndpointCommandService endpointCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _endpointCommandService = endpointCommandService;
            _endpointCommandService.NotNull(nameof(endpointCommandService));
        }

        public async Task Handle(EndpointCreateDto createDto, CancellationToken cancellationToken)
        {
            Endpoint endpoint = _mapper.Map<Endpoint>(createDto);
            await _endpointCommandService.Add(endpoint);
        }
    }
}
