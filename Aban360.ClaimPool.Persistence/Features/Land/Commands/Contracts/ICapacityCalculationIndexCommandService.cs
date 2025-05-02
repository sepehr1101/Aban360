using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface ICapacityCalculationIndexCommandService
    {
        Task Add(CapacityCalculationIndex capacityCalculationIndex);
        Task Remove(CapacityCalculationIndex capacityCalculationIndex);
    }
}
