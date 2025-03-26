using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts
{
    public interface IUsageLevel1DeleteHandler
    {
        Task Handle(UsageLevel1DeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
