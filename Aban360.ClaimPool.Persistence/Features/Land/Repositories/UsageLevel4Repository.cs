using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Repositories
{
    internal sealed class UsageLevel4Repository : IUsageLevel4CommandService, IUsageLevel4QueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel4> _usageLevel4;
        public UsageLevel4Repository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageLevel4 = _uow.Set<UsageLevel4>();
            _usageLevel4.NotNull(nameof(_usageLevel4));
        }

        public async Task Add(UsageLevel4 usageLevel4)
        {
            await _usageLevel4.AddAsync(usageLevel4);
        }

        public async Task Remove(UsageLevel4 usageLevel4)
        {
            _usageLevel4.Remove(usageLevel4);
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
