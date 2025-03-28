using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts
{
    public interface IUsageLevel3DeleteHandler
    {
        Task Handle(UsageLevel3DeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
