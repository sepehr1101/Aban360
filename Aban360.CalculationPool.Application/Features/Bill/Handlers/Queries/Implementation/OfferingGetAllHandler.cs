using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class OfferingGetAllHandler : IOfferingGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingQueryService _offeringQueryService;
        public OfferingGetAllHandler(
            IMapper mapper,
            IOfferingQueryService offeringQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));
        }

        public async Task<ICollection<OfferingGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Offering> offering = await _offeringQueryService.Get();
            return _mapper.Map<ICollection<OfferingGetDto>>(offering);
        }
    }
}
