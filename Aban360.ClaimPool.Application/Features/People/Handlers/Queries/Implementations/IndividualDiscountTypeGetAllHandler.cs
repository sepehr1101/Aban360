using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class IndividualDiscountTypeGetAllHandler : IIndividualDiscountTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualDiscountTypeQueryService _individualDiscountTypeQueryService;
        public IndividualDiscountTypeGetAllHandler(
            IMapper mapper,
            IIndividualDiscountTypeQueryService individualDiscountTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _individualDiscountTypeQueryService = individualDiscountTypeQueryService;
            _individualDiscountTypeQueryService.NotNull(nameof(_individualDiscountTypeQueryService));
        }

        public async Task<ICollection<IndividualDiscountTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var individualDiscountType = await _individualDiscountTypeQueryService.Get();
            return _mapper.Map<ICollection<IndividualDiscountTypeGetDto>>(individualDiscountType);
        }
    }
}
