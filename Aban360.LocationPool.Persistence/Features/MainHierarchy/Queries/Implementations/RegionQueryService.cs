using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
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
            return await _uow.FindOrThrowAsync<Region>(id);
        }

        public async Task<ICollection<Region>> Get()
        {
            return await _region.ToListAsync();
        }
    }
}
