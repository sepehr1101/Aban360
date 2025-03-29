using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IUsageLevel1DeleteHandler
    {
        Task Handle(UsageLevel1DeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
