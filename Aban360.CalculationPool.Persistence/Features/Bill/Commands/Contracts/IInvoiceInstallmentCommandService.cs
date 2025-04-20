using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IInvoiceInstallmentCommandService
    {
        Task Add(InvoiceInstallment invoiceInstallment);
        Task Remove(InvoiceInstallment invoiceInstallment);
    }
}
