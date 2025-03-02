using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Implementation
{
    public class OfferingUnitDeleteHandler : IOfferingUnitDeleteHandler
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
            var offeringUnit = await _offeringUnitQueryService.Get(deleteDto.Id);
            if (offeringUnit == null)
            {
                throw new InvalidDataException();
            }
            await _offeringUnitCommandService.Remove(offeringUnit);
        }
    }
}
