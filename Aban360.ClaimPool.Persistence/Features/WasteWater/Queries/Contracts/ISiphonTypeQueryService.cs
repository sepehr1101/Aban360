using Aban360.ClaimPool.Domain.Features.WasteWater;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts
{
    public interface ISiphonTypeQueryService
    {
        Task<SiphonType> Get(short id);
        Task<ICollection<SiphonType>> Get();
    }
    //
}
