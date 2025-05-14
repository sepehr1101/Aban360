using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Contrats
{
    public interface ITariffTestImaginaryCustomerHandler
    {
        Task<IntervalCalculationResultWrapper> Handle(TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken);
    }
}