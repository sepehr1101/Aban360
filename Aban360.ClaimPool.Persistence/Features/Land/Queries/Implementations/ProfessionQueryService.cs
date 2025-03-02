using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    public class ProfessionQueryService : IProfessionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Profession> _profession;
        public ProfessionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _profession = _uow.Set<Profession>();
            _profession.NotNull(nameof(_profession));
        }

        public async Task<Profession> Get(short id)
        {
            return await _profession
                .Include(x => x.Guild)
                .Where(x => x.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Profession>> Get()
        {
            return await _profession
                .Include(x => x.Guild)
                .ToListAsync();
        }
    }
}
