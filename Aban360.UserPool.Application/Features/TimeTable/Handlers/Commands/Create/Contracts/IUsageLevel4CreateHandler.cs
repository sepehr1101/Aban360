using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts
{
    public interface IUsageLevel4CreateHandler
    {
        Task Handle(UsageLevel4CreateDto createDto, CancellationToken cancellationToken);
    }
}
