using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Implementations
{
    internal sealed class RequestIndividualDiscountTypeGetSingleHandler : IRequestIndividualDiscountTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualDiscountTypeQueryService _requestIndividualDiscountTypeQueryService;
        public RequestIndividualDiscountTypeGetSingleHandler(
            IMapper mapper,
            IRequestIndividualDiscountTypeQueryService requestIndividualDiscountTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualDiscountTypeQueryService = requestIndividualDiscountTypeQueryService;
            _requestIndividualDiscountTypeQueryService.NotNull(nameof(_requestIndividualDiscountTypeQueryService));
        }

        public async Task<RequestIndividualDiscountTypeGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var individualDiscountType = await _requestIndividualDiscountTypeQueryService.Get(id);
            return _mapper.Map<RequestIndividualDiscountTypeGetDto>(individualDiscountType);
        }
    }
}
