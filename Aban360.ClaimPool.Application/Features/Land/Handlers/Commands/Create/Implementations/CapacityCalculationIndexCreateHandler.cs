using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class CapacityCalculationIndexCreateHandler : ICapacityCalculationIndexCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICapacityCalculationIndexCommandService _capacityCalculationIndexCommandService;
        public CapacityCalculationIndexCreateHandler(
            IMapper mapper,
            ICapacityCalculationIndexCommandService capacityCalculationIndexCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _capacityCalculationIndexCommandService = capacityCalculationIndexCommandService;
            _capacityCalculationIndexCommandService.NotNull(nameof(_capacityCalculationIndexCommandService));
        }

        public async Task Handle(CapacityCalculationIndexCreateDto createDto, CancellationToken cancellationToken)
        {
            var capacityCalculationIndex = _mapper.Map<CapacityCalculationIndex>(createDto);
            await _capacityCalculationIndexCommandService.Add(capacityCalculationIndex);
        }
    }
}
