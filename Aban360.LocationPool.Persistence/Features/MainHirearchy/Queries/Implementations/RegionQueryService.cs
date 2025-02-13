using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Implementations
{
    public class RegionQueryService : IRegionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Region> _region;
        public RegionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _region = _uow.Set<Region>();
            _region.NotNull(nameof(_region));
        }

        public async Task<Region> Get(int id)
        {
            return await _region
                    .Include(r => r.Headquarters)
                    .Where(r => r.Id == id)
                    .SingleAsync();
        }

        public async Task<ICollection<Region>> Get()
        {
            return await _region
                .Include(r=>r.Headquarters)
                .ToListAsync();
        }
    }
}
