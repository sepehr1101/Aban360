using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IWaterMeterTagDefinitionCommandService
    {
        Task Add(WaterMeterTagDefinition waterMeterTagDefinition);
        Task Remove(WaterMeterTagDefinition waterMeterTagDefinition);
    }
}
