using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    public class UsageQuerySevice : IUsageQuerySevice
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Usage> _usage;
        public UsageQuerySevice(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usage = _uow.Set<Usage>();
            _usage.NotNull(nameof(_usage));
        }
        public async Task<Usage> Get(short id)
        {
            return await _uow.FindOrThrowAsync<Usage>(id);
        }

        public async Task<ICollection<Usage>> Get()
        {
            return await _usage.ToListAsync();
        }
    }
}
