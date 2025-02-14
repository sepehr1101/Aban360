using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Implementation
{
    public class SiphonQueryService : ISiphonQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Siphon> _siphon;
        public SiphonQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphon = _uow.Set<Siphon>();
            _siphon.NotNull(nameof(_siphon));
        }

        public async Task<Siphon> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Siphon>(id);
        }

        public async Task<ICollection<Siphon>> Get()
        {
            return await _siphon.ToListAsync();
        }
    }
}
