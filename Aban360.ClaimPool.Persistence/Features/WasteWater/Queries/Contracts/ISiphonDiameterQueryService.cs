using Aban360.ClaimPool.Domain.Features.WasteWater;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts
{
    public interface ISiphonDiameterQueryService
    {
        Task<SiphonDiameter> Get(short id);
        Task<ICollection<SiphonDiameter>> Get();

    }
    //
}
