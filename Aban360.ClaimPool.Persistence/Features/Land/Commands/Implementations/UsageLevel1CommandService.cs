using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class UsageLevel1CommandService : IUsageLevel1CommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel1> _usageLevel1;
        public UsageLevel1CommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageLevel1 = _uow.Set<UsageLevel1>();
            _usageLevel1.NotNull(nameof(_usageLevel1));
        }

        public async Task Add(UsageLevel1 usageLevel1)
        {
            await _usageLevel1.AddAsync(usageLevel1);
        }

        public async Task Remove(UsageLevel1 usageLevel1)
        {
            _usageLevel1.Remove(usageLevel1);
        }
    }

}
