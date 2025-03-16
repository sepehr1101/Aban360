using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    internal sealed class RegionQueryService : IRegionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Region> _regions;
        public RegionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _regions = _uow.Set<Region>();
            _regions.NotNull(nameof(_regions));
        }

        public async Task<Region> Get(int id)
        {
            return await _regions
                    .Include(r => r.Headquarters)
                    .Where(r => r.Id == id)
                    .SingleAsync();
        }

        public async Task<ICollection<Region>> Get()
        {
            return await _regions
                .Include(r=>r.Headquarters)
                .ToListAsync();
        }
    }
}
