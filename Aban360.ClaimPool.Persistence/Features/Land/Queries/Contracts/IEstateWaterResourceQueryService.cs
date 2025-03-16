using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IEstateWaterResourceQueryService
    {
        Task<EstateWaterResource> Get(short id);
        Task<ICollection<EstateWaterResource>> Get();
    }
}
