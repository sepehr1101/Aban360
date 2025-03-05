using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class OfferingUnitCreateHandler : IOfferingUnitCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingUnitCommandService _offeringUnitCommandService;
        public OfferingUnitCreateHandler(
            IMapper mapper,
            IOfferingUnitCommandService OfferingUnitCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringUnitCommandService = OfferingUnitCommandService;
            _offeringUnitCommandService.NotNull(nameof(OfferingUnitCommandService));
        }

        public async Task Handle(OfferingUnitCreateDto createDto, CancellationToken cancellationToken)
        {
            var offeringUnit = _mapper.Map<OfferingUnit>(createDto);
            if (offeringUnit == null)
            {
                throw new InvalidDataException();
            }
            await _offeringUnitCommandService.Add(offeringUnit);
        }
    }
}
