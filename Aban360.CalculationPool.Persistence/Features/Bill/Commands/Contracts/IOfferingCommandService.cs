using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IOfferingCommandService
    {
        Task Add(Offering offering);
        Task Remove(Offering offering);
    }
}
