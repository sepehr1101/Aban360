using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class CapacityCalculationIndexQueryService : ICapacityCalculationIndexQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CapacityCalculationIndex> _capacityCalculationIndex;
        public CapacityCalculationIndexQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _capacityCalculationIndex = _uow.Set<CapacityCalculationIndex>();
            _capacityCalculationIndex.NotNull(nameof(_capacityCalculationIndex));
        }

        public async Task<CapacityCalculationIndex> Get(short id)
        {
            return await _uow.FindOrThrowAsync<CapacityCalculationIndex>(id);
        }

        public async Task<ICollection<CapacityCalculationIndex>> Get()
        {
            return await _capacityCalculationIndex.ToListAsync();
        }
    }
}
