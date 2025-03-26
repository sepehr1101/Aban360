using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Implementations
{
    internal sealed class UsageLevel4CommandService : IUsageLevel4CommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel4> _usageLevel4;
        public UsageLevel4CommandService(IUnitOfWork uow)
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
    }
}
