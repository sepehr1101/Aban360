using Aban360.ClaimPool.Domain.Features.WasteWater;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Implementation
{
    public class siphonTypeQueryService : ISiphonTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonType> _siphonType;
        public siphonTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonType = _uow.Set<SiphonType>();
            _siphonType.NotNull(nameof(_siphonType));
        }

        public async Task<SiphonType> Get(short id)
        {
            return await _siphonType
                .Include(i => i.Siphons)
                .Where(i => i.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<SiphonType>> Get()
        {
            return await _siphonType.ToListAsync();
        }
    }
}
