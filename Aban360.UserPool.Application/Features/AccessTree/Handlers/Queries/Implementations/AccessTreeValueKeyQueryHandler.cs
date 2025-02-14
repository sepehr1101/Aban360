using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Factories;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    internal sealed class AccessTreeValueKeyQueryHandler : IAccessTreeValueKeyQueryHandler
    {
        private readonly IEndpointQueryService _endpointQueryService;
        public AccessTreeValueKeyQueryHandler(IEndpointQueryService endpointQueryService)
        {
            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }
        public async Task<AccessTreeValueKeyDto> Handle(CancellationToken cancellationToken)
        {
            var endpoints = await _endpointQueryService.GetIncludeAll();
            return endpoints.CreateAccessTree();
        }
        public async Task<AccessTreeValueKeyDto> Handle(ICollection<int> selectedEndpointIds, CancellationToken cancellationToken)
        {
            var endpoints = await _endpointQueryService.GetIncludeAll();
            return endpoints.CreateAccessTree(selectedEndpointIds);
        }
    }
}
