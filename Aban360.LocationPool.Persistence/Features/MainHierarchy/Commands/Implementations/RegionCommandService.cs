using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Implementations
{
    public class RegionCommandService : IRegionCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Region> _regions;
        public RegionCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _regions = _uow.Set<Region>();
            _regions.NotNull(nameof(_regions));
        }

        public async Task Add(Region region)
        {
            await _regions.AddAsync(region);
        }

        public async Task Remove(Region region)
        {
            _regions.Remove(region);
        }
    }

}
