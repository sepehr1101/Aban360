using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualUpdateHandler : IRequestIndividualUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualQueryService _requestIndividualQueryService;
        public RequestIndividualUpdateHandler(
            IMapper mapper,
            IRequestIndividualQueryService requestIndividualQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualQueryService = requestIndividualQueryService;
            _requestIndividualQueryService.NotNull(nameof(_requestIndividualQueryService));
        }

        public async Task Handle(IndividualRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestIndividual = await _requestIndividualQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestIndividual);
        }
    }
}
