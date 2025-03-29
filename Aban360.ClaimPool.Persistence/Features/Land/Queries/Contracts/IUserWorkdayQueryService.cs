using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUserWorkdayQueryService
    {
        Task<UserWorkday> Get(short id);
        Task<ICollection<UserWorkday>> Get();
    }
}
