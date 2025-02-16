using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IWaterMeterTagQueryService
    {
        Task<ICollection<WaterMeterTag>> Get(string billId);
        Task<WaterMeterTag> Get(int id);
    }
}
