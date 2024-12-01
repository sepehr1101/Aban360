using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts
{
    public interface ITokenStoreCommandService
    {
        Task Add(UserToken userToken);        
        Task RemoveTokensWithSameRefreshTokenSource(Guid userId);
        Task Remove(Guid userId);
    }
}