using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface IMeterFlowValidationGetHandler
    {       
        Task Handle(int id, CancellationToken cancellationToken);
        Task Handle(int id, MeterFlowStepEnum latestFlowId, CancellationToken cancellationToken);
    }
}
