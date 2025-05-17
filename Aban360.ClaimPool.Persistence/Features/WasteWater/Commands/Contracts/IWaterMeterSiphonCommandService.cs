using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts
{
    public interface IWaterMeterSiphonCommandService
    {
        Task Add(WaterMeterSiphon waterMeterSiphon);
        Task Remove(WaterMeterSiphon waterMeterSiphon);
    }
}
