using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class ZoneQueryService : IZoneQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Zone> _zones;
        public ZoneQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _zones = _uow.Set<Zone>();
            _zones.NotNull(nameof(_zones));
        }

        public async Task<Zone> Get(int id)
        {
            return await _zones
                    .Include(z => z.Region)
                    .Where(z => z.Id == id)
                    .SingleAsync();
        }
        public async Task<ICollection<Zone>> Get()
        {
            return await _zones
                .Include(z => z.Region)
                .ToListAsync();
        }
        public async Task<ICollection<Zone>> GetIncludeAll()
        {
            return await _zones
                .Include(zone => zone.Region)
                .ThenInclude(region=>region.Headquarters)
                .ThenInclude(headquartes=>headquartes.Province)
                .ThenInclude(province=>province.CordinalDirection)
                .ToListAsync();
        }      
    }
}
