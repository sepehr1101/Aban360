using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    internal sealed class EndpointGetAllHandler : IEndpointGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IEndpointQueryService _endpointQueryService;
        public EndpointGetAllHandler(
            IMapper mapper,
            IEndpointQueryService endpointQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }

        public async Task<ICollection<EndpointGetDto>> Handle( CancellationToken cancellationToken)
        {
            ICollection<Endpoint> endpoint = await _endpointQueryService.GetInclude();
            return _mapper.Map<ICollection<EndpointGetDto>>(endpoint);
        }
    }
}
