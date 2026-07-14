using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface IMeterSmsFlowGetAllHandler
    {
        Task<IEnumerable<MeterSmsFlowGetDto>> Handle(CancellationToken cancellationToken);
    }
}
