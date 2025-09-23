namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts
{
    public interface IZaribCDeleteHandler
    {
        Task Handle(int id, CancellationToken cancellationToken);
    }
}
