using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IWaterResourceQueryService
    {
        Task<WaterResource> Get(short id);
        Task<ICollection<WaterResource>> Get();
    }
}
