using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IMeterTypeCommandService
    {
        Task Add(MeterType meterType);
        Task Remove(MeterType meterType);
    }
}
