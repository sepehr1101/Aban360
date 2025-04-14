using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class IndividualDiscountTypeGetSingleHandler : IIndividualDiscountTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualDiscountTypeQueryService _individualDiscountTypeQueryService;
        public IndividualDiscountTypeGetSingleHandler(
            IMapper mapper,
            IIndividualDiscountTypeQueryService individualDiscountTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _individualDiscountTypeQueryService = individualDiscountTypeQueryService;
            _individualDiscountTypeQueryService.NotNull(nameof(_individualDiscountTypeQueryService));
        }

        public async Task<IndividualDiscountTypeGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var individualDiscountType = await _individualDiscountTypeQueryService.Get(id);
            return _mapper.Map<IndividualDiscountTypeGetDto>(individualDiscountType);
        }
    }
}
