using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IMeterUseTypeCommandService
    {
        Task Add(MeterUseType meterUserType);
        Task Remove(MeterUseType meterUserType);
    }
}
