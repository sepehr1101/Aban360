using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IInvoiceLineItemInsertModeQueryService
    {
        Task<InvoiceLineItemInsertMode> Get(InvoiceLineItemInsertModeEnum id);
        Task<ICollection<InvoiceLineItemInsertMode>> Get();
    }
}
