using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageGroup1QueryService
    {
        Task<IEnumerable<UsageGroup1GetDto>> Get();
        Task<UsageGroup1GetDto> Get(int id);
        Task<UsageGroup1GetDto?> Get(string title);
    }
}
