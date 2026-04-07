namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ISetBillIdHandler
    {
        Task Handle(int trackNumber, CancellationToken cancellationToken);
    }
}
