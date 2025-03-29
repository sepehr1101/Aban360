using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageLevel4QueryService
    {
        Task<UsageLevel4> Get(short id);
        Task<ICollection<UsageLevel4>> Get();
    }
}
