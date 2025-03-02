using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IOfferingUnitCommandService
    {
        Task Add(OfferingUnit offeringUnit);
        Task Remove(OfferingUnit offeringUnit);
    }
}
