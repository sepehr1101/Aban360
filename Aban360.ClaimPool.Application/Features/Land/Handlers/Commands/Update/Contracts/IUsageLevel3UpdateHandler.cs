using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IUsageLevel3UpdateHandler
    {
        Task Handle(UsageLevel3UpdateDto updateDto, CancellationToken cancellationToken);
    }
}
