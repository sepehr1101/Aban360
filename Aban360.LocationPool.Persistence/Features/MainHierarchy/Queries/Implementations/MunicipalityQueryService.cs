using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    internal sealed class MunicipalityQueryService : IMunicipalityQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Municipality> _municipalities;
        public MunicipalityQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _municipalities = _uow.Set<Municipality>();
            _municipalities.NotNull(nameof(_municipalities));
        }

        public async Task<Municipality> Get(int id)
        {
            return await _municipalities
                .Include(m => m.Zone)
                .Where(m => m.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Municipality>> Get()
        {
            return await _municipalities
                .Include(m=>m.Zone)
                .ToListAsync();
        }
        public async Task<IEnumerable<NumericDictionary>> GetByZoneId(int zoneId)
        {
            return await _municipalities
                .Where(m => m.ZoneId == zoneId)
                .OrderBy(r=>r.Id)
                .Select(r => new NumericDictionary(r.Id, r.Title))
                .ToListAsync();
        }
    }
}
