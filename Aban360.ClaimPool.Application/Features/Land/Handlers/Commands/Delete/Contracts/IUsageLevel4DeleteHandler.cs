using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IUsageLevel4DeleteHandler
    {
        Task Handle(UsageLevel4DeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
