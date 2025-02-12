using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageQuerySevice
    {
        Task<Usage> Get(short id);
        Task<ICollection<Usage>> Get();
    }
}
