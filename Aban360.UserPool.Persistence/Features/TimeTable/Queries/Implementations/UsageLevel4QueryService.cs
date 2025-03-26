using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Implementations
{
    internal sealed class UsageLevel4QueryService : IUsageLevel4QueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel4> _usageLevel4;
        public UsageLevel4QueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageLevel4 = _uow.Set<UsageLevel4>();
            _usageLevel4.NotNull(nameof(_usageLevel4));
        }

        public async Task<UsageLevel4> Get(short id)
        {
            return await _usageLevel4
                .Include(u => u.UsageLevel3)
                .Where(u => u.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<UsageLevel4>> Get()
        {
            return await _usageLevel4
                .Include(u => u.UsageLevel3)
                .ToListAsync();
        }
    }
}
