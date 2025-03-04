using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IInvoiceStatusGetSingleHandler
    {
        Task<InvoiceStatusGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
