using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts
{
    public interface IInvoiceLineItemInsertModeCreateHandler
    {
        Task Handle(InvoiceLineItemInsertModeCreateDto createDto, CancellationToken cancellationToken);
    }
}
