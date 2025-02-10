using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    public class EndpointGetSingleHandler : IEndpointGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IEndpointQueryService _endpointQueryService;
        public EndpointGetSingleHandler(
            IMapper mapper,
            IEndpointQueryService endpointQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }

        public async Task<EndpointGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var endpoint = await _endpointQueryService.GetInclude(id);
            return _mapper.Map<EndpointGetDto>(endpoint);
        }
    }
}
