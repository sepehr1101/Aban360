namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts
{
    public interface IMotherDeleteHandler
    {
        Task Handle(int trackNumber, CancellationToken cancellationToken);
    }
}
