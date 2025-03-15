using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    internal sealed class WaterMeterTagDefinitionCommandService : IWaterMeterTagDefinitionCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterTagDefinition> _waterMeterTagDefinitions;
        public WaterMeterTagDefinitionCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterTagDefinitions = _uow.Set<WaterMeterTagDefinition>();
            _waterMeterTagDefinitions.NotNull(nameof(_waterMeterTagDefinitions));
        }

        public async Task Add(WaterMeterTagDefinition waterMeterTagDefinition)
        {
            await _waterMeterTagDefinitions.AddAsync(waterMeterTagDefinition);
        }

        public async Task Remove(WaterMeterTagDefinition waterMeterTagDefinition)
        {
            _waterMeterTagDefinitions.Remove(waterMeterTagDefinition);
        }
    }
}
