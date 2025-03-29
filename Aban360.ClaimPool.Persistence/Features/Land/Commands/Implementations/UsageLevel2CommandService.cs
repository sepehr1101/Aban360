using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class UsageLevel2CommandService : IUsageLevel2CommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel2> _usageLevel2;
        public UsageLevel2CommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageLevel2 = _uow.Set<UsageLevel2>();
            _usageLevel2.NotNull(nameof(_usageLevel2));
        }

        public async Task Add(UsageLevel2 usageLevel2)
        {
            await _usageLevel2.AddAsync(usageLevel2);
        }

        public async Task Remove(UsageLevel2 usageLevel2)
        {
            _usageLevel2.Remove(usageLevel2);
        }
    }
}
