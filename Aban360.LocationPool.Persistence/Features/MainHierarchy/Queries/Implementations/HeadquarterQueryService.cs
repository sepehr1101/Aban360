using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class HeadquarterQueryService : IHeadquarterQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Headquarters> _headquarter;
        public HeadquarterQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _headquarter = _uow.Set<Headquarters>();
            _headquarter.NotNull(nameof(_headquarter));

        }

        public async Task<Headquarters> Get(short id)
        {
            return await _headquarter
                 .Include(h => h.Province)
                 .Where(h => h.Id == id)
                 .SingleAsync();
        }

        public async Task<ICollection<Headquarters>> Get()
        {
            return await _headquarter
                .Include(h=>h.Province)
                .ToListAsync();
        }
    }
}
