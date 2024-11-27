using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts
{
    public interface ITokenStoreService
    {
        void Add(UserToken userToken);
        void Remove(UserToken userToken);
    }
}