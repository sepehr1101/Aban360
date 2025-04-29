using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class CapacityCalculationIndexUpdateHandler : ICapacityCalculationIndexUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICapacityCalculationIndexQueryService _capacityCalculationIndexQueryService;
        public CapacityCalculationIndexUpdateHandler(
            IMapper mapper,
            ICapacityCalculationIndexQueryService capacityCalculationIndexQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _capacityCalculationIndexQueryService = capacityCalculationIndexQueryService;
            _capacityCalculationIndexQueryService.NotNull(nameof(_capacityCalculationIndexQueryService));
        }

        public async Task Handle(CapacityCalculationIndexUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var capacityCalculationIndex = await _capacityCalculationIndexQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, capacityCalculationIndex);
        }
    }
}
