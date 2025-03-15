using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class WaterResourceCommandService : IWaterResourceCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterResource> _waterResource;
        public WaterResourceCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterResource = _uow.Set<WaterResource>();
            _waterResource.NotNull(nameof(_waterResource));
        }

        public async Task Add(WaterResource waterResource)
        {
            await _waterResource.AddAsync(waterResource);
        }

        public async Task Remove(WaterResource waterResource)
        {
            _waterResource.Remove(waterResource);
        }
    }
}
