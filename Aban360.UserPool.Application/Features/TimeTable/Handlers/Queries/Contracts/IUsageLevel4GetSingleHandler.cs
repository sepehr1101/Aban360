using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IUsageLevel4GetSingleHandler
    {
        Task<UsageLevel4GetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
