using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUserLeaveQueryService
    {
        Task<UserLeave> Get(short id);
        Task<ICollection<UserLeave>> Get();
    }
}
