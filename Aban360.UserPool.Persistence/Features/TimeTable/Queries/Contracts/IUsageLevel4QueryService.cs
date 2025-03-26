using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts
{
    public interface IUsageLevel4QueryService
    {
        Task<UsageLevel4> Get(short id);
        Task<ICollection<UsageLevel4>> Get();
    }
}
