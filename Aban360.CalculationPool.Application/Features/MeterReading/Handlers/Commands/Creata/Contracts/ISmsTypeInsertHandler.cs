namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    public interface ISmsTypeInsertHandler
    {
        Task Handle(string title, CancellationToken cancellationToken);
    }
}
