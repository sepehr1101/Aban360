using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class OfferingUnitGetAllHandler : IOfferingUnitGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingUnitQueryService _offeringUnitQueryService;
        public OfferingUnitGetAllHandler(
            IMapper mapper,
            IOfferingUnitQueryService offeringUnitQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringUnitQueryService = offeringUnitQueryService;
            _offeringUnitQueryService.NotNull(nameof(offeringUnitQueryService));
        }

        public async Task<ICollection<OfferingUnitGetDto>> Handle(CancellationToken cancellationToken)
        {
            var offeringUnit = await _offeringUnitQueryService.Get();
            if (offeringUnit == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<OfferingUnitGetDto>>(offeringUnit);
        }
    }
}
