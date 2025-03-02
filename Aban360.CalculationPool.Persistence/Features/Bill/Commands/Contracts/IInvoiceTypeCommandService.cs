using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IInvoiceTypeCommandService
    {
        Task Add(InvoiceType invoiceType);
        Task Remove(InvoiceType invoiceType);
    }
}
