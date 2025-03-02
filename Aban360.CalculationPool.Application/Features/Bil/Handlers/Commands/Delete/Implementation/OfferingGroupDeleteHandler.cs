using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Implementation
{
    public class OfferingGroupDeleteHandler : IOfferingGroupDeleteHandler
    {
        private readonly IOfferingGroupCommandService _offeringGroupCommandService;
        private readonly IOfferingGroupQueryService _offeringGroupQueryService;
        public OfferingGroupDeleteHandler(
            IOfferingGroupCommandService offeringGroupCommandService,
            IOfferingGroupQueryService offeringGroupQueryService)
        {
            _offeringGroupCommandService = offeringGroupCommandService;
            _offeringGroupCommandService.NotNull(nameof(offeringGroupCommandService));

            _offeringGroupQueryService = offeringGroupQueryService;
            _offeringGroupQueryService.NotNull(nameof(offeringGroupQueryService));
        }

        public async Task Handle(OfferingGroupDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var offeringGroup = await _offeringGroupQueryService.Get(deleteDto.Id);
            if (offeringGroup == null)
            {
                throw new InvalidDataException();
            }
            await _offeringGroupCommandService.Remove(offeringGroup);
        }
    }
}
