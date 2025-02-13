using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface ITokenStoreQueryService
    {
        Task<UserToken?> Get(string refreshTokenHash);
        Task<bool> IsValid(string accessToken, Guid userId);
    }
}