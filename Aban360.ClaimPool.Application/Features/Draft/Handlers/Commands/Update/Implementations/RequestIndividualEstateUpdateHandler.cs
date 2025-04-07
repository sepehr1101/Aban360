using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualEstateUpdateHandler : IRequestIndividualEstateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualEstateQueryService _requestIndividualEstateQueryService;
        public RequestIndividualEstateUpdateHandler(
            IMapper mapper,
            IRequestIndividualEstateQueryService requestIndividualEstateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualEstateQueryService = requestIndividualEstateQueryService;
            _requestIndividualEstateQueryService.NotNull(nameof(_requestIndividualEstateQueryService));
        }

        public async Task Handle(IndividualEstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestIndividualEstate = await _requestIndividualEstateQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestIndividualEstate);
        }
    }
}
