using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IOfferingGroupCommandService
    {
        Task Add(OfferingGroup offeringGroup);
        Task Remove(OfferingGroup offeringGroup);
    }
}
