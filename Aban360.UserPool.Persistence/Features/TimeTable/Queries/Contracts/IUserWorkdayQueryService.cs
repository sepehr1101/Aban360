using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts
{
    public interface IUserWorkdayQueryService
    {
        Task<UserWorkday> Get(short id);
        Task<ICollection<UserWorkday>> Get();
    }
}
