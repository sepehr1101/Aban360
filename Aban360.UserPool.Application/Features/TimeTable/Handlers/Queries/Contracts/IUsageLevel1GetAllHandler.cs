using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IUsageLevel1GetAllHandler
    {
        Task<ICollection<UsageLevel1GetDto>> Handle(CancellationToken cancellationToken);
    }
}
