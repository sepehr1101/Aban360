using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface IGetewayCreateHandler
    {
        Task Handle(GetewayCreateDto createDto, CancellationToken cancellationToken);
    }
}