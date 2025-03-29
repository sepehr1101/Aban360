using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageLevel3QueryService
    {
        Task<UsageLevel3> Get(short id);
        Task<ICollection<UsageLevel3>> Get();
    }
}
