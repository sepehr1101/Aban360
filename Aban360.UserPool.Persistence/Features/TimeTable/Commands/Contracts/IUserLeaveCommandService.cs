using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts
{
    public interface IUserLeaveCommandService
    {
        Task Add(UserLeave userLeave);
        Task Remove(UserLeave userLeave);
    }
}
