using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageLevel2QueryService
    {
        Task<UsageLevel2> Get(short id);
        Task<ICollection<UsageLevel2>> Get();
    }
}
