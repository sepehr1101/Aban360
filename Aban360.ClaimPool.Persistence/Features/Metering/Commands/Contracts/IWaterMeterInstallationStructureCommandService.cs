using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IWaterMeterInstallationStructureCommandService
    {
        Task Add(WaterMeterInstallationStructure waterMeterInstallationStructure);
        Task Remove(WaterMeterInstallationStructure waterMeterInstallationStructure);
    }
}
