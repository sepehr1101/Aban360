using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IWaterMeterQueryService
    {
        Task<WaterMeter> Get(int id);
        Task<ICollection<WaterMeter>> Get();
    }
}
