using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts
{
    public interface IUserLeaveQueryService
    {
        Task<UserLeave> Get(short id);
        Task<ICollection<UserLeave>> Get();
    }
}
