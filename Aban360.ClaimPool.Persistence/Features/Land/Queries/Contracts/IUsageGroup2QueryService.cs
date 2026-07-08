using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageGroup2QueryService
    {
        Task<IEnumerable<UsageGroup2GetDto>> Get();
        Task<UsageGroup2GetDto> Get(short id);
        Task<UsageGroup2GetDto?> Get(string title, short group1Id);
        Task<IEnumerable<UsageGroup2GetDto>> GetByParentId(short group1Id);
    }
}
