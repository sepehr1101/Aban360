using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IInvoiceLineItemInsertModeQueryService
    {
        Task<InvoiceLineItemInsertMode> Get(short id);
        Task<ICollection<InvoiceLineItemInsertMode>> Get();
    }
}
