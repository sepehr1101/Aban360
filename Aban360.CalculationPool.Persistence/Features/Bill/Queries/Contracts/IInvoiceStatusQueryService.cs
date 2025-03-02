using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IInvoiceStatusQueryService
    {
        Task<InvoiceStatus> Get(short id);
        Task<ICollection<InvoiceStatus>> Get();
    }
}
