using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts
{
    public interface ITariffCalculationHandler
    {
        Task<IntervalCalculationResultWrapper> Test(TariffTestInput tariffTestInput,CancellationToken cancellationToken);
    }
}