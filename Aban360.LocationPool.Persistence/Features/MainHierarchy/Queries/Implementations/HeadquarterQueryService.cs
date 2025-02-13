using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class HeadquarterQueryService : IHeadquarterQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Headquarters> _headquarterList;
        public HeadquarterQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _headquarterList = _uow.Set<Headquarters>();
            _headquarterList.NotNull(nameof(_headquarterList));

        }
        public async Task<Headquarters> Get(short id)
        {
            return await _headquarterList
                 .Include(h => h.Province)
                 .Where(h => h.Id == id)
                 .SingleAsync();
        }
        public async Task<ICollection<Headquarters>> Get()
        {
            return await _headquarterList
                .Include(h=>h.Province)
                .ToListAsync();
        }
    }
}
