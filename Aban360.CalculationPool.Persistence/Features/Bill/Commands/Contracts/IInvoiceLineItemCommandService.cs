using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IInvoiceLineItemCommandService
    {
        Task Add(InvoiceLineItem invoiceLineItem);
        Task Add(ICollection<InvoiceLineItem> invoiceLineItem);
        Task Remove(InvoiceLineItem invoiceLineItem);
    }
}
