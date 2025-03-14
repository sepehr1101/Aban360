using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts
{
    public interface IGetewayDeleteHandler
    {
        Task Handle(GetewayDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}