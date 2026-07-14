using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface IMeterSmsFlowGetHandler
    {
        Task<MeterSmsFlowGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
