using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IUserWorkdayCommandService
    {
        Task Add(UserWorkday userWorkday);
        Task Remove(UserWorkday userWorkday);
    }
}
