using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts
{
    public interface IGetewayUpdateHandler
    {
        Task Handle(GetewayUpdateDto updateDto, CancellationToken cancellationToken);
    }
}