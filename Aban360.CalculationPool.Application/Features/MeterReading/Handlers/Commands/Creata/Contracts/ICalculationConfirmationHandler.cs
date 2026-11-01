using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    public interface ICalculationConfirmationHandler
    {
        Task<MeterReadingCheckedOutputDto> Handle(int latestFlowId, Guid userId, CancellationToken cancellationToken);
    }
}
