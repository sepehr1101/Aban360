using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IUserClaimQueryService
    {
        Task<ICollection<UserClaim>> Get(Guid userId);
    }
}