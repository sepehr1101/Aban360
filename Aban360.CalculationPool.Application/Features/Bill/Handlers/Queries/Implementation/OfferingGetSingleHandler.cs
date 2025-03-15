using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class OfferingGetSingleHandler : IOfferingGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingQueryService _offeringQueryService;
        public OfferingGetSingleHandler(
            IMapper mapper,
            IOfferingQueryService offeringQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));
        }

        public async Task<OfferingGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            Offering offering = await _offeringQueryService.Get(id);
            return _mapper.Map<OfferingGetDto>(offering);
        }
    }
}
