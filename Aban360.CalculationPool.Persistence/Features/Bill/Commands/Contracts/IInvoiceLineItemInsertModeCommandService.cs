using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IInvoiceLineItemInsertModeCommandService
    {
        Task Add(InvoiceLineItemInsertMode invoinceLineItemInsertMode);
        Task Remove(InvoiceLineItemInsertMode invoinceLineItemInsertMode);
    }
}
