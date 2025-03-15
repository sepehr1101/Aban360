using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class EstateWaterResourceCommandService : IEstateWaterResourceCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EstateWaterResource> _estateWaterResource;
        public EstateWaterResourceCommandService(IUnitOfWork uow)
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
    }
}
