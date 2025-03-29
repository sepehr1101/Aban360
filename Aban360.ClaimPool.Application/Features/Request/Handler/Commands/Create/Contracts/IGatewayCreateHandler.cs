using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface IGatewayCreateHandler
    {
        Task Handle(GatewayCreateDto createDto, CancellationToken cancellationToken);
    }
}