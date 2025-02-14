using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts
{
    public interface ISiphonDiameterCommandService
    {
        Task Add(SiphonDiameter siphonDiameter);
        Task Remove(SiphonDiameter siphonDiameter);
    }
}
