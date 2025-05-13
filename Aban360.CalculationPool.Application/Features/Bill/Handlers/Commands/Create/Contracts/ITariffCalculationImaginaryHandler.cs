using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts
{
    public interface ITariffCalculationImaginaryHandler
    {
        Task<IntervalCalculationResultWrapper> Handle(TariffTestImaginaryInput tariffTestInput,CancellationToken cancellationToken);
    }
}