namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts
{
    public interface ISDeleteHandler
    {
        Task Handle(int id, CancellationToken cancellationToken);
    }
}
