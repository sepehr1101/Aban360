using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Implementation
{
    internal sealed class siphonDiameterQueryService : ISiphonDiameterQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonDiameter> _siphonDiameter;
        public siphonDiameterQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonDiameter = _uow.Set<SiphonDiameter>();
            _siphonDiameter.NotNull(nameof(_siphonDiameter));
        }

        public async Task<SiphonDiameter> Get(short id)
        {
            return await _siphonDiameter
                .Include(i => i.Siphons)
                .Where(i => i.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<SiphonDiameter>> Get()
        {
            return await _siphonDiameter.ToListAsync();
        }
    }
}
