using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Implementations
{
    public class ZoneQueryService : IZoneQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Zone> _zone;
        public ZoneQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _zone = _uow.Set<Zone>();
            _zone.NotNull(nameof(_zone));
        }

        public async Task<Zone> Get(int id)
        {
            return await _zone
                    .Include(z => z.Region)
                    .Where(z => z.Id == id)
                    .SingleAsync();
        }

        public async Task<ICollection<Zone>> Get()
        {
            return await _zone
                .Include(z => z.Region)
                .ToListAsync();
        }
    }
}
