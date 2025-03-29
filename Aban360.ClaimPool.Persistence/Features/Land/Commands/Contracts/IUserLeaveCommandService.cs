using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IUserLeaveCommandService
    {
        Task Add(UserLeave userLeave);
        Task Remove(UserLeave userLeave);
    }
}
