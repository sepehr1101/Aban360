using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts
{
    public interface IUsageLevel3CreateHandler
    {
        Task Handle(UsageLevel3CreateDto createDto, CancellationToken cancellationToken);
    }
}
