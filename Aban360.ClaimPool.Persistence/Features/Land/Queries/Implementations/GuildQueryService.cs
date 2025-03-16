using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class GuildQueryService : IGuildQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Guild> _guild;
        public GuildQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guild = _uow.Set<Guild>();
            _guild.NotNull(nameof(_guild));
        }

        public async Task<Guild> Get(short id)
        {
            return await _guild
                  .Include(x => x.Usage)
                  .Where(x => x.Id == id)
                  .SingleAsync();
        }

        public async Task<ICollection<Guild>> Get()
        {
            return await _guild
                  .Include(x=>x.Usage)
                  .ToListAsync();
        }
    }
}
