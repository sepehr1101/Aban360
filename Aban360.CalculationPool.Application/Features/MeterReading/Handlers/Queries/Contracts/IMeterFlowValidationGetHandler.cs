namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface IMeterFlowValidationGetHandler
    {
        Task Handle(string fileName, CancellationToken cancellationToken);
        Task Handle(int id, CancellationToken cancellationToken);
    }
}
