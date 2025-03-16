using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class EstateWaterResourceQueryService : IEstateWaterResourceQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EstateWaterResource> _estateWaterResource;
        public EstateWaterResourceQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estateWaterResource = _uow.Set<EstateWaterResource>();
            _estateWaterResource.NotNull(nameof(_estateWaterResource));
        }

        public async Task<EstateWaterResource> Get(short id)
        {
            return await _estateWaterResource
                .Include(e => e.Estate)
                .Include(e => e.WaterResource)
                .Where(e => e.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<EstateWaterResource>> Get()
        {
            return await _estateWaterResource
                .Include(e => e.Estate)
                .Include(e => e.WaterResource)
                .ToListAsync();
        }
    }
}
