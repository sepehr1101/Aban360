namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts
{
    public interface I_CalculationCreateHandler
    {
        Task Handle(string billId,string counterNumber, CancellationToken cancellationToken);
    }
}
