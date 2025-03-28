using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts
{
    public interface IUsageLevel1CreateHandler
    {
        Task Handle(UsageLevel1CreateDto createDto, CancellationToken cancellationToken);
    }
}
