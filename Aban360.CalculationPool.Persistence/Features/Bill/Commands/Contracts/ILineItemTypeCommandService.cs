using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface ILineItemTypeCommandService
    {
        Task Add(LineItemType lineItemType);
        Task Remove(LineItemType lineItemType);

    }
}
