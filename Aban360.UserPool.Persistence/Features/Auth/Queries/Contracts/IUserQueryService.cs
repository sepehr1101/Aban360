using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IUserQueryService
    {
        Task<User> Get(Guid id);
        Task<User?> Get(string username);
    }
}