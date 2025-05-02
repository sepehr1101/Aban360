using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class CapacityCalculationIndexCommandService : ICapacityCalculationIndexCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CapacityCalculationIndex> _capacityCalculationIndex;
        public CapacityCalculationIndexCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _capacityCalculationIndex = _uow.Set<CapacityCalculationIndex>();
            _capacityCalculationIndex.NotNull(nameof(_capacityCalculationIndex));
        }

        public async Task Add(CapacityCalculationIndex capacityCalculationIndex)
        {
            await _capacityCalculationIndex.AddAsync(capacityCalculationIndex);
        }

        public async Task Remove(CapacityCalculationIndex capacityCalculationIndex)
        {
            _capacityCalculationIndex.Remove(capacityCalculationIndex);
        }
    }
}
