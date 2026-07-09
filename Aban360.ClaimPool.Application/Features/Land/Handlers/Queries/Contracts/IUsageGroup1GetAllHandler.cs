using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageGroup1GetAllHandler
    {
        Task<IEnumerable<UsageGroup1GetDto>> Handle(CancellationToken cancellationToken);
    }
}
