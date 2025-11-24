using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface ICartableHandler
    {
        Task<IEnumerable<MeterFlowCartableGetDto>> Handle(CancellationToken cancellationToken);
    }
}
