using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts
{
    public interface IUserClaimCommandService
    {
        Task Add(ICollection<UserClaim> userCliams);
        void Remove(ICollection<UserClaim> userCliams, string logInfo);
    }
}