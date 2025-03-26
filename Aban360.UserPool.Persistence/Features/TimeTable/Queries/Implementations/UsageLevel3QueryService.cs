using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Implementations
{
    internal sealed class UsageLevel3QueryService : IUsageLevel3QueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel3> _usageLevel3;
        public UsageLevel3QueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageLevel3 = _uow.Set<UsageLevel3>();
            _usageLevel3.NotNull(nameof(_usageLevel3));
        }

        public async Task<UsageLevel3> Get(short id)
        {
            return await _usageLevel3
                .Include(u => u.UsageLevel2)
                .Where(u => u.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<UsageLevel3>> Get()
        {
            return await _usageLevel3
                .Include(u => u.UsageLevel2)
                .ToListAsync();
        }
    }
}
