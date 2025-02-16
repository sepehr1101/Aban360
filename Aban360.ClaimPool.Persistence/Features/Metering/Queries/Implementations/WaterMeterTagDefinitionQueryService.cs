using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    public class WaterMeterTagDefinitionQueryService : IWaterMeterTagDefinitionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterTagDefinition> _waterMeterTagDefinitions;
        public WaterMeterTagDefinitionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterTagDefinitions = _uow.Set<WaterMeterTagDefinition>();
            _waterMeterTagDefinitions.NotNull(nameof(_waterMeterTagDefinitions));
        }

        public async Task<WaterMeterTagDefinition> Get(short id)
        {
            return await _uow.FindOrThrowAsync<WaterMeterTagDefinition>(id);
        }

        public async Task<ICollection<WaterMeterTagDefinition>> Get()
        {
            return await _waterMeterTagDefinitions.ToListAsync();
        }
    }
}
