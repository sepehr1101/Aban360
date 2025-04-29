using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IInvoiceInstallmentQueryService
    {
        Task<InvoiceInstallment> Get(long id);
        Task<InvoiceInstallment> Get(string billId);
        Task<ICollection<InvoiceInstallment>> Get();
    }
}
