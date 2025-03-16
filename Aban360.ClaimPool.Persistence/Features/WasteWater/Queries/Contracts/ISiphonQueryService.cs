using Aban360.ClaimPool.Domain.Features.WasteWater.Base;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts
{
    public interface ISiphonQueryService
    {
        Task<Siphon> Get(int id);
        Task<ICollection<Siphon>> Get();
    }
    //
}
