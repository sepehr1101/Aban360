using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class EstateBoundTypeQueryService : IEstateBoundTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EstateBoundType> _estateBoundType;
        public EstateBoundTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estateBoundType = _uow.Set<EstateBoundType>();
            _estateBoundType.NotNull(nameof(_estateBoundType));
        }

        public async Task<EstateBoundType> Get(short id)
        {
            return await _uow.FindOrThrowAsync<EstateBoundType>(id);
        }

        public async Task<ICollection<EstateBoundType>> Get()
        {
            return await _estateBoundType.ToListAsync();
        }
    }
}
