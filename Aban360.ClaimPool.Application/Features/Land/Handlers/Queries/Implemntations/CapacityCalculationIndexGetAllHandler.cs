using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class CapacityCalculationIndexGetAllHandler : ICapacityCalculationIndexGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICapacityCalculationIndexQueryService _capacityCalculationIndexQueryService;
        public CapacityCalculationIndexGetAllHandler(
            IMapper mapper,
            ICapacityCalculationIndexQueryService capacityCalculationIndexQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _capacityCalculationIndexQueryService = capacityCalculationIndexQueryService;
            _capacityCalculationIndexQueryService.NotNull(nameof(_capacityCalculationIndexQueryService));
        }

        public async Task<ICollection<CapacityCalculationIndexGetDto>> Handle(CancellationToken cancellationToken)
        {
            var capacityCalculationIndex = await _capacityCalculationIndexQueryService.Get();
            return _mapper.Map<ICollection<CapacityCalculationIndexGetDto>>(capacityCalculationIndex);
        }
    }
}
