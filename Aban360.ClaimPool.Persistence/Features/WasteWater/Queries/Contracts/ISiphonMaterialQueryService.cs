using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts
{
    public interface ISiphonMaterialQueryService
    {
        Task<SiphonMaterial> Get(short id);
        Task<ICollection<SiphonMaterial>> Get();
    }
}
