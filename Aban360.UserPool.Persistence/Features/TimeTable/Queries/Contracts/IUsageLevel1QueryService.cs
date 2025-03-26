using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts
{
    public interface IUsageLevel1QueryService
    {
        Task<UsageLevel1> Get(short id);
        Task<ICollection<UsageLevel1>> Get();
    }

}
