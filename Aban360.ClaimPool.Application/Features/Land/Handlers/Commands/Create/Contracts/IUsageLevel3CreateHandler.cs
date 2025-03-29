using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IUsageLevel3CreateHandler
    {
        Task Handle(UsageLevel3CreateDto createDto, CancellationToken cancellationToken);
    }
}
