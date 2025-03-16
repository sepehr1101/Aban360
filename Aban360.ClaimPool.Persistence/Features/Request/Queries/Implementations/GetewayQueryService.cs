using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class GetewayQueryService : IGetewayQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Geteway> _geteway;
        public GetewayQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _geteway = _uow.Set<Geteway>();
            _geteway.NotNull(nameof(_geteway));
        }

        public async Task<Geteway> Get(short id)
        {
            return await _uow.FindOrThrowAsync<Geteway>(id);
        }

        public async Task<ICollection<Geteway>> Get()
        {
            return await _geteway.ToListAsync();
        }
    }
}