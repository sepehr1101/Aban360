using Aban360.ClaimPool.Domain.Features.Metering;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IWaterMeterCommandService
    {
        Task Add(WaterMeter waterMeter);
        Task Remove(WaterMeter waterMeter);
    }
}
