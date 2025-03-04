using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface ILineItemTypeGroupCommandService
    {
        Task Add(LineItemTypeGroup lineItemTypeGroup);
        Task Remove(LineItemTypeGroup lineItemTypeGroup);

    }
}
