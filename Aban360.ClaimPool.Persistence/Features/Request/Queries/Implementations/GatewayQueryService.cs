using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class GatewayQueryService : IGatewayQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Gateway> _geteway;
        public GatewayQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _geteway = _uow.Set<Gateway>();
            _geteway.NotNull(nameof(_geteway));
        }

        public async Task<Gateway> Get(short id)
        {
            return await _uow.FindOrThrowAsync<Gateway>(id);
        }

        public async Task<ICollection<Gateway>> Get()
        {
            return await _geteway.ToListAsync();
        }
    }
}