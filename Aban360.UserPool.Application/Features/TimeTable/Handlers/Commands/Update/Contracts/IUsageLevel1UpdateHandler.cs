using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts
{
    public interface IUsageLevel1UpdateHandler
    {
        Task Handle(UsageLevel1UpdateDto updateDto, CancellationToken cancellationToken);
    }
}
