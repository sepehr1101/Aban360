using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IWaterMeterInstallationMethodCommandService
    {
        Task Add(WaterMeterInstallationMethod waterMeterInstallationStructure);
        Task Remove(WaterMeterInstallationMethod waterMeterInstallationStructure);
    }
}
