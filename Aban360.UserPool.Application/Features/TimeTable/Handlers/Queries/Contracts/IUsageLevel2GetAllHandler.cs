using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IUsageLevel2GetAllHandler
    {
        Task<ICollection<UsageLevel2GetDto>> Handle(CancellationToken cancellationToken);
    }
}
