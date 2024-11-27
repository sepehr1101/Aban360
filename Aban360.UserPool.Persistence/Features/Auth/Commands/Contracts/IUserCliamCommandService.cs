using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts
{
    public interface IUserCliamCommandService
    {
        Task Add(ICollection<UserClaim> userCliams);
    }
}