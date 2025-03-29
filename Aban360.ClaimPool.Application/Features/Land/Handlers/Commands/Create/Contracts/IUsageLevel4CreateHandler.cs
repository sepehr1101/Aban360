using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IUsageLevel4CreateHandler
    {
        Task Handle(UsageLevel4CreateDto createDto, CancellationToken cancellationToken);
    }
}
