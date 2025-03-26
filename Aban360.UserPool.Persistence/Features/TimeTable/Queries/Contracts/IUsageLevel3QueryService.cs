using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts
{
    public interface IUsageLevel3QueryService
    {
        Task<UsageLevel3> Get(short id);
        Task<ICollection<UsageLevel3>> Get();
    }
}
