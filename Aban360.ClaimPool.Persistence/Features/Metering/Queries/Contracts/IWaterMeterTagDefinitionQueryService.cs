using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IWaterMeterTagDefinitionQueryService
    {
        Task<WaterMeterTagDefinition> Get(short id);
        Task<ICollection<WaterMeterTagDefinition>> Get();
    }
}
