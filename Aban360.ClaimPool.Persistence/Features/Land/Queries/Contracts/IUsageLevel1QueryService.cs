using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageLevel1QueryService
    {
        Task<UsageLevel1> Get(short id);
        Task<ICollection<UsageLevel1>> Get();
    }

}
