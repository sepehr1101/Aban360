using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class UsageCommandSevice : IUsageCommandSevice
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Usage> _usage;
        public UsageCommandSevice(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _usage = _uow.Set<Usage>();
            _usage.NotNull(nameof(_usage));
        }

        public async Task Add(Usage usage)
        {
            await _usage.AddAsync(usage);
        }

        public async Task Remove(Usage usage)
        {
            _usage.Remove(usage);
        }
    }
}
