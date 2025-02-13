using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IMeterDiameterCommandService
    {
        Task Add(MeterDiameter meterDiameter);
        Task Remove(MeterDiameter meterDiameter);
    }
}
