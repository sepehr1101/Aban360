using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IInvoiceCommandService
    {
        Task Add(Invoice invoice);
        Task Remove(Invoice invoice);
    }
}
