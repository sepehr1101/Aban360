using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class EstateQueryService : IEstateQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Estate> _estate;
        public EstateQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estate = _uow.Set<Estate>();
            _estate.NotNull(nameof(_estate));
        }

        public async Task<Estate> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Estate>(id);
        }

        public async Task<ICollection<Estate>> Get()
        {
            return await _estate.ToListAsync();
        }
    }
}
