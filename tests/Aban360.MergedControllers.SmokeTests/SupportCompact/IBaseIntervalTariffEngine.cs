using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace BeforeCompactSupport
{
    public interface IBaseIntervalTariffEngine
    {
        Task<Tuple<ConsumptionInfo, List<IntervalCalculationResult>>> Calculate(TariffTestInput tariffTestInput, CancellationToken cancellationToken);
        Task<Tuple<ConsumptionInfo, List<IntervalCalculationResult>>> Calculate(TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken);
    }
}