using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Implementations
{
    internal sealed class EndpointDeleteHandler : IEndpointDeleteHandler
    {
        private readonly IEndpointQueryService _endpointQueryService;
        private readonly IEndpointCommandService _endpointCommandService;
        public EndpointDeleteHandler(
            IEndpointQueryService endpointQueryService,
            IEndpointCommandService endpointCommandService)
        {
            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));

            _endpointCommandService = endpointCommandService;
            _endpointCommandService.NotNull(nameof(endpointCommandService));
        }

        public async Task Handle(EndpointDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Endpoint endpoint = await _endpointQueryService.Get(deleteDto.Id);
            _endpointCommandService.Remove(endpoint);
        }
    }
}
