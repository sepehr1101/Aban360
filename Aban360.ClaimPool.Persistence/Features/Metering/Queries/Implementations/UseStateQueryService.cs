using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    public class UseStateQueryService : IUseStateQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UseState> _useState;
        public UseStateQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useState = _uow.Set<UseState>();
            _useState.NotNull(nameof(_useState));
        }

        public async Task<UseState> Get(UseStateEnum id)
        {
            return await _uow.FindOrThrowAsync<UseState>(id);
        }

        public async Task<ICollection<UseState>> Get()
        {
            return await _useState.ToListAsync();
        }
    }
}
