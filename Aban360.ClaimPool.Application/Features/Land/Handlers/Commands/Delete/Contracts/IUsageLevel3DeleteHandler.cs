using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IUsageLevel3DeleteHandler
    {
        Task Handle(UsageLevel3DeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
