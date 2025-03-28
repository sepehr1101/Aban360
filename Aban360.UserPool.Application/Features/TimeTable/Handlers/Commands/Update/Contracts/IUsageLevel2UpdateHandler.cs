using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts
{
    public interface IUsageLevel2UpdateHandler
    {
        Task Handle(UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken);
    }
}
