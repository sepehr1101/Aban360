using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts
{
    public interface ILineItemTypeUpdateHandler
    {
        Task Handle(LineItemTypeUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
