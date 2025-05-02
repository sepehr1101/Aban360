using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IWaterMeterInstallationMethodQueryService
    {
        Task<WaterMeterInstallationMethod> Get(short id);
        Task<ICollection<WaterMeterInstallationMethod>> Get();
    }
}
