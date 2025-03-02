using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IInvoiceTypeQueryService
    {
        Task<InvoiceType> Get(short id);
        Task<ICollection<InvoiceType>> Get();
    }
}
