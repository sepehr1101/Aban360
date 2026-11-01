namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    public interface IMeterReadingCalculationConfirmThenSendSmsHandler
    {
        Task Handle(int latestFlowId, Guid userId, CancellationToken cancellationToken);
    }
}
