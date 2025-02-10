using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Implementations
{
    public class EndpointUpdateHandler : IEndpointUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEndpointQueryService _endpointQueryService;
        public EndpointUpdateHandler(
            IMapper mapper,
            IEndpointQueryService endpointQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }

        public async Task Handle(EndpointUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var endpoint = await _endpointQueryService.Get(updateDto.Id);
            if (endpoint == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, endpoint);
        }
    }
}
