using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts
{
    public interface IUsageLevel2CreateHandler
    {
        Task Handle(UsageLevel2CreateDto createDto, CancellationToken cancellationToken);
    }
}
