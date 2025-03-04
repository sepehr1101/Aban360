using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    public class OfferingUnitUpdateHandler : IOfferingUnitUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingUnitQueryService _offeringUnitQueryService;
        public OfferingUnitUpdateHandler(
            IMapper mapper,
            IOfferingUnitQueryService offeringUnitQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringUnitQueryService = offeringUnitQueryService;
            _offeringUnitQueryService.NotNull(nameof(offeringUnitQueryService));
        }

        public async Task Handle(OfferingUnitUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var offeringUnit = await _offeringUnitQueryService.Get(updateDto.Id);
            if (offeringUnit == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, offeringUnit);
        }
    }
}
