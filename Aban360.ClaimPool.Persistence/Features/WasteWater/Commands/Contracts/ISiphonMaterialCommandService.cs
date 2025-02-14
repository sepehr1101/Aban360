using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts
{
    public interface ISiphonMaterialCommandService
    {
        Task Add(SiphonMaterial siphonMaterial);
        Task Remove(SiphonMaterial siphonMaterial);
    }
}
