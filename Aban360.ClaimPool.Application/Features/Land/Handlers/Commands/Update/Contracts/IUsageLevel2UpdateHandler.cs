using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IUsageLevel2UpdateHandler
    {
        Task Handle(UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken);
    }
}
