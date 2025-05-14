using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Contrats
{
    public interface ITariffTestSingleCustomerHandler
    {
        Task<IntervalCalculationResultWrapper> Handle(TariffTestInput tariffTestInput, CancellationToken cancellationToken);
    }
}