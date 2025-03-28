using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts
{
    public interface IUserWorkdayCommandService
    {
        Task Add(UserWorkday userWorkday);
        Task Remove(UserWorkday userWorkday);
    }
}
