using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class CapacityCalculationIndexDeleteHandler : ICapacityCalculationIndexDeleteHandler
    {
        private readonly ICapacityCalculationIndexCommandService _capacityCalculationIndexCommandService;
        private readonly ICapacityCalculationIndexQueryService _capacityCalculationIndexQueryService;
        public CapacityCalculationIndexDeleteHandler(
            ICapacityCalculationIndexCommandService capacityCalculationIndexCommandService,
            ICapacityCalculationIndexQueryService capacityCalculationIndexQueryService)
        {
            _capacityCalculationIndexCommandService = capacityCalculationIndexCommandService;
            _capacityCalculationIndexCommandService.NotNull(nameof(_capacityCalculationIndexCommandService));

            _capacityCalculationIndexQueryService = capacityCalculationIndexQueryService;
            _capacityCalculationIndexQueryService.NotNull(nameof(_capacityCalculationIndexQueryService));
        }

        public async Task Handle(CapacityCalculationIndexDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var capacityCalculationIndex = await _capacityCalculationIndexQueryService.Get(deleteDto.Id);
            await _capacityCalculationIndexCommandService.Remove(capacityCalculationIndex);
        }
    }
}
