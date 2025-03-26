using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Implementations
{
    internal sealed class UsageLevel1QueryService : IUsageLevel1QueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel1> _usageLevel1;
        public UsageLevel1QueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageLevel1 = _uow.Set<UsageLevel1>();
            _usageLevel1.NotNull(nameof(_usageLevel1));
        }

        public async Task<UsageLevel1> Get(short id)
        {
            return await _uow.FindOrThrowAsync<UsageLevel1>(id);
        }

        public async Task<ICollection<UsageLevel1>> Get()
        {
            return await _usageLevel1.ToListAsync();
        }
    }

}
