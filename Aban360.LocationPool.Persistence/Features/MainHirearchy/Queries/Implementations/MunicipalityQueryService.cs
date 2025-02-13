using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Implementations
{
    public class MunicipalityQueryService : IMunicipalityQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Municipality> _municipality;
        public MunicipalityQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _municipality = _uow.Set<Municipality>();
            _municipality.NotNull(nameof(_municipality));
        }

        public async Task<Municipality> Get(int id)
        {
            return await _municipality
                .Include(m => m.Zone)
                .Where(m => m.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Municipality>> Get()
        {
            return await _municipality
                .Include(m=>m.Zone)
                .ToListAsync();
        }
    }

}
