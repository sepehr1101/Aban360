using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IWaterMeterTagCommandService
    {
        Task Add(WaterMeterTag waterMeterTag);
        Task Remove(WaterMeterTag waterMeterTag);
    }
}
