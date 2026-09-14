using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface ISmsFlowGetAllHandler
    {
        Task<IEnumerable<SmsFlowGetDto>> Handle(CancellationToken cancellationToken);
    }
}
