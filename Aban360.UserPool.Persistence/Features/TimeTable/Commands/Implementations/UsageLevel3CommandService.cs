using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Implementations
{
    internal sealed class UsageLevel3CommandService : IUsageLevel3CommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel3> _usageLevel3;
        public UsageLevel3CommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usageLevel3 = _uow.Set<UsageLevel3>();
            _usageLevel3.NotNull(nameof(_usageLevel3));
        }

        public async Task Add(UsageLevel3 usageLevel3)
        {
            await _usageLevel3.AddAsync(usageLevel3);
        }

        public async Task Remove(UsageLevel3 usageLevel3)
        {
            _usageLevel3.Remove(usageLevel3);
        }
    }
}
