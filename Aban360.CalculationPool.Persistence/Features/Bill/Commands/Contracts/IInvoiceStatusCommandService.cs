using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IInvoiceStatusCommandService
    {
        Task Add(InvoiceStatus invoiceStatus);
        Task Remove(InvoiceStatus invoiceStatus);
    }
}
