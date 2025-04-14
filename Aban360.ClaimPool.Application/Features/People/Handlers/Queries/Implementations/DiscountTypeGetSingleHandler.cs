using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class DiscountTypeGetSingleHandler : IDiscountTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        public DiscountTypeGetSingleHandler(
            IMapper mapper,
            IDiscountTypeQueryService discountTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _discountTypeQueryService = discountTypeQueryService;
            _discountTypeQueryService.NotNull(nameof(_discountTypeQueryService));
        }

        public async Task<DiscountTypeGetDto> Handle(DiscountTypeEnum id, CancellationToken cancellationToken)
        {
            var discountType = await _discountTypeQueryService.Get(id);
            return _mapper.Map<DiscountTypeGetDto>(discountType);
        }
    }
}
