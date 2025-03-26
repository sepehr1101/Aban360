using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IUsageLevel4GetAllHandler
    {
        Task<ICollection<UsageLevel4GetDto>> Handle(CancellationToken cancellationToken);
    }
}
