using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Repositories
{
    internal sealed class HeadquarterRepository : IHeadquarterCommandService, IHeadquarterQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Headquarters> _headquarter;
        public HeadquarterRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _headquarter = _uow.Set<Headquarters>();
            _headquarter.NotNull(nameof(_headquarter));

        }

        public async Task Add(Headquarters headquarter)
        {
            await _headquarter.AddAsync(headquarter);
        }

        public async Task Remove(Headquarters headquarter)
        {
            _headquarter.Remove(headquarter);
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
