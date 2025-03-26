using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IUsageLevel3GetAllHandler
    {
        Task<ICollection<UsageLevel3GetDto>> Handle(CancellationToken cancellationToken);
    }
}
