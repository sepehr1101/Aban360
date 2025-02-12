using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IMeterMaterialCommandService
    {
        Task Add(MeterMaterial meterMaterial);
        Task Remove(MeterMaterial meterMaterial);
    }
}
