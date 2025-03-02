using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts
{
    public interface IOfferingGroupUpdateHandler
    {
        Task Handle(OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
