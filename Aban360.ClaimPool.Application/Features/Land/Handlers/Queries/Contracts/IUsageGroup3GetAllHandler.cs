using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageGroup3GetAllHandler
    {
        Task<IEnumerable<UsageGroup3GetDto>> Handle(CancellationToken cancellationToken);
    }
}
