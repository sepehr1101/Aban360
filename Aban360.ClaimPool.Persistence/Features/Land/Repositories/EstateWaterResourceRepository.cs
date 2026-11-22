using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Repositories
{
    internal sealed class EstateWaterResourceRepository : IEstateWaterResourceCommandService, IEstateWaterResourceQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EstateWaterResource> _estateWaterResource;
        public EstateWaterResourceRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estateWaterResource = _uow.Set<EstateWaterResource>();
            _estateWaterResource.NotNull(nameof(_estateWaterResource));
        }

        public async Task Add(EstateWaterResource estateWaterResource)
        {
            await _estateWaterResource.AddAsync(estateWaterResource);
        }

        public async Task Remove(EstateWaterResource estateWaterResource)
        {
            _estateWaterResource.Remove(estateWaterResource);
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
