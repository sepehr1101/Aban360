using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IUserClaimQueryService
    {
        Task<ICollection<UserClaim>> Get(Guid userId);
        Task<ICollection<UserClaim>> Get(Guid userId, ClaimType claimType);
        IQueryable<UserClaim> GetQuery();
    }
}