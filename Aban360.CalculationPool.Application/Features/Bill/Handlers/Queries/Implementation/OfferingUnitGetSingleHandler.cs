using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class OfferingUnitGetSingleHandler : IOfferingUnitGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingUnitQueryService _offeringUnitQueryService;
        public OfferingUnitGetSingleHandler(
            IMapper mapper,
            IOfferingUnitQueryService offeringUnitQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringUnitQueryService = offeringUnitQueryService;
            _offeringUnitQueryService.NotNull(nameof(offeringUnitQueryService));
        }

        public async Task<OfferingUnitGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var offeringUnit = await _offeringUnitQueryService.Get(id);
            if (offeringUnit == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<OfferingUnitGetDto>(offeringUnit);
        }
    }
}
