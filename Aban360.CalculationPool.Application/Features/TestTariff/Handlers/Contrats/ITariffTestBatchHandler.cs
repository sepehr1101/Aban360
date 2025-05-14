using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Contrats
{
    public interface ITariffTestBatchHandler
    {
        Task<CaluclationIntervalDiscrepancytWrapper> Handle(CaluclationIntervalBatchTestInput testInput, CancellationToken cancellationToken);
    }
}