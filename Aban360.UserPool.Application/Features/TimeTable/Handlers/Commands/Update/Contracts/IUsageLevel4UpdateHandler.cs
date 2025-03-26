using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts
{
    public interface IUsageLevel4UpdateHandler
    {
        Task Handle(UsageLevel4UpdateDto updateDto, CancellationToken cancellationToken);
    }
}
