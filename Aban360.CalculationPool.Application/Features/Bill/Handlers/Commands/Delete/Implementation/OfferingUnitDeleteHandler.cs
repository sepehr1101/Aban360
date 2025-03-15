using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Implementation
{
    internal sealed class OfferingUnitDeleteHandler : IOfferingUnitDeleteHandler
    {
        private readonly IOfferingUnitCommandService _offeringUnitCommandService;
        private readonly IOfferingUnitQueryService _offeringUnitQueryService;
        public OfferingUnitDeleteHandler(
            IOfferingUnitCommandService offeringUnitCommandService,
            IOfferingUnitQueryService offeringUnitQueryService)
        {
            _offeringUnitCommandService = offeringUnitCommandService;
            _offeringUnitCommandService.NotNull(nameof(offeringUnitCommandService));

            _offeringUnitQueryService = offeringUnitQueryService;
            _offeringUnitQueryService.NotNull(nameof(offeringUnitQueryService));
        }

        public async Task Handle(OfferingUnitDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            OfferingUnit offeringUnit = await _offeringUnitQueryService.Get(deleteDto.Id);
            await _offeringUnitCommandService.Remove(offeringUnit);
        }
    }
}
