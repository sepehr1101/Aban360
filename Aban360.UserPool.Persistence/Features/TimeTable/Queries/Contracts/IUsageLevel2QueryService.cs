using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts
{
    public interface IUsageLevel2QueryService
    {
        Task<UsageLevel2> Get(short id);
        Task<ICollection<UsageLevel2>> Get();
    }
}
